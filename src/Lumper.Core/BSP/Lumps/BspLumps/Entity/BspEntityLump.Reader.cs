using Lumper.Core.BSP.Exceptions;
using System.Text;

namespace Lumper.Core.BSP.Lumps.BspLumps.Entity;

public partial class BspEntityLump : IBspLump<BspEntityLump>
{
    public static IBspLump ReadInternal(BinaryReader reader, int lenght)
    {
        var basePosition = reader.BaseStream.Position;
        var entities = new List<BspEntity>();

        while (reader.BaseStream.Position - basePosition < lenght -1)
        {
            SkipWhiteSpaces(reader, basePosition, lenght);
            if (reader.BaseStream.Position - basePosition >= lenght - 1)
                continue;
            var entity = ParseEntity(reader, basePosition, lenght - 1);
            entities.Add(entity);
        }

        ParseChar(reader, basePosition, lenght, '\0');

        return new BspEntityLump(entities);
    }

    private static BspEntity ParseEntity(BinaryReader reader, long basePosition, int lenght)
    {
        var entity = new BspEntity();
        ParseChar(reader, basePosition, lenght, '{');
        SkipWhiteSpaces(reader, basePosition, lenght);

        while (PeekChar(reader, basePosition, lenght) != '}')
        {
            var key = ParseString(reader, basePosition, lenght);
            SkipWhiteSpaces(reader, basePosition, lenght);
            var value = ParseString(reader, basePosition, lenght);
            SkipWhiteSpaces(reader, basePosition, lenght);
            entity.Properties.Add((key, value));
        }

        ParseChar(reader, basePosition, lenght, '}');
        return entity;
    }

    private static string ParseString(BinaryReader reader, long basePosition, int lenght)
    {
        ParseChar(reader, basePosition, lenght, '"');
        var str = ParseIdent(reader, basePosition, lenght, '"');
        ParseChar(reader, basePosition, lenght, '"');
        return str;
    }

    private static string ParseIdent(BinaryReader reader, long basePosition, int lenght, char lastChar)
    {
        var builder = new StringBuilder();
        while(PeekChar(reader, basePosition, lenght) != lastChar)
        {
            builder.Append(reader.ReadChar());
        }
        return builder.ToString();
    }

    private static void ParseChar(BinaryReader reader, long basePosition, int lenght, char expected)
    {
        CheckCanReadChar(reader, basePosition, lenght);
        char actual = reader.ReadChar();
        if (actual != expected)
        {
            CheckCanReadChar(reader, basePosition, lenght);
            throw new InvalidBspEntityChar(expected, actual);
        }
    }

    private static char ParseChar(BinaryReader reader, long basePosition, int lenght)
    {
        CheckCanReadChar(reader, basePosition, lenght);
        return reader.ReadChar();
    }

    private static void SkipWhiteSpaces(BinaryReader reader, long basePosition, int lenght)
    {
        while (PeekChar(reader, basePosition, lenght) is ' ' or '\n')
            ParseChar(reader, basePosition, lenght);
    }

    private static char PeekChar(BinaryReader reader, long basePosition, int lenght)
    {
        CheckCanReadChar(reader, basePosition, lenght);
        var position = reader.BaseStream.Position;
        char chr = reader.ReadChar();
        reader.BaseStream.Position = position;
        return chr;
    }

    private static void CheckCanReadChar(BinaryReader reader, long basePosition, int lenght)
    {
        if (reader.BaseStream.Position - basePosition >= lenght)
            throw new BspLumpReadOverflow();
    }
}
