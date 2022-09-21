using Lumper.Core.BSP.Exceptions;
using Lumper.Core.BSP.Lumps.BspLumps;
using Lumper.Core.BSP.Lumps.BspLumps.Entity;
using Lumper.Core.Collections;
using SharpCompress.Compressors.LZMA;

namespace Lumper.Core.BSP.Lumps;


public partial interface IBspLump : IOwnedElement<BspImage>
{
    private readonly static ReadOnlyMemory<byte> LzmaHeader = new byte[] { 0x4C, 0x5A, 0x4D, 0x41 }; //LZMA;

    public static IBspLump? Read(BinaryReader reader, int index)
    {
        var offset = reader.ReadInt32();
        var lenght = reader.ReadInt32();
        var version = reader.ReadInt32();
        var compressedLenght = reader.ReadInt32();

        if (offset == 0 && lenght == 0 && version == 0 && compressedLenght == 0)
            return null;

        var continuePosition = reader.BaseStream.Position;
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);

        var lumpReader = reader;

        if (compressedLenght != 0)
        {
            lumpReader = PrepareDecompressedReader(reader, ref lenght);
        }

        IBspLump? lump = index switch
        {
            0 => BspEntityLump.ReadInternal(lumpReader, lenght),
            >= 0 and < 64 => null,// TODO: throw new NotImplementedException(),
            _ => throw new IndexOutOfRangeException(),
        };

        reader.BaseStream.Seek(continuePosition, SeekOrigin.Begin);
        return lump;
    }

    private static BinaryReader PrepareDecompressedReader(BinaryReader reader, ref int lenght)
    {
        Span<byte> magic = stackalloc byte[4];
        reader.Read(magic);

        if (!magic.SequenceEqual(LzmaHeader.Span))
            throw new InvalidLzmaHeaderException();

        var inputSize = reader.ReadInt32();
        var outputSize = reader.ReadInt32();
        lenght = outputSize;
        Span<byte> properties = stackalloc byte[5];
        reader.Read(properties);

        var stream = new LzmaStream(properties.ToArray(), reader.BaseStream, inputSize, outputSize);
        return new BinaryReader(stream);
    }
}