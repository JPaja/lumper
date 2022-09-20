using Lumper.Core.BSP.Enums;
using Lumper.Core.BSP.Exceptions;
using Lumper.Core.BSP.Lumps;
using Lumper.Core.BSP.Lumps.BspLumps;
using System.Diagnostics;

namespace Lumper.Core.BSP;

partial class BspImage
{
    private const int HeaderLumps = 64;
    private const int HeaderSize = 1036;
    private const int MaxLumps = 128;

    private readonly static ReadOnlyMemory<byte> BspHeader = new byte[]{ 0x56, 0x42, 0x53, 0x50 }; //VBSP;
    private readonly static ReadOnlyMemory<BspLumpType> BspTypes = Enum.GetValues<BspLumpType>();

    private void Read(BinaryReader reader)
    {
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        Span<byte> header = stackalloc byte[4];
        reader.Read(header);

        if (!header.SequenceEqual(BspHeader.Span))
            throw new InvalidBspHeaderException();

        Version = reader.ReadInt32();
        ReadBspLumps(reader);
        Revision = reader.ReadInt32();
    }

    private void ReadBspLumps(BinaryReader reader)
    {
        var bspTypes = BspTypes.Span;
        Debug.Assert(bspTypes.Length == 64);
        Debug.Assert(bspTypes.SequenceEqual(Enumerable.Range(0, 64)
                                            .Cast<BspLumpType>().ToArray()));

        var headers = new Dictionary<BspLumpType, BspLumpHeader>();
        foreach(var type in bspTypes)
        {
            var bspLumpHeader = BspLumpHeader.Parse(reader);
            headers.Add(type, bspLumpHeader);
        }

        foreach (var (type, header) in headers)
        {
            var segmentReader = PrepareSegmentReader(reader, header);
            var lump = type switch
            {
                _ => BspDataLump.Read(segmentReader, header.SegmentLenght)
            };

            Lumps[type] = lump;
        }
    }

    private BinaryReader PrepareSegmentReader(BinaryReader reader, BspLumpHeader header)
    {
        if (!header.Compressed)
        {
            reader.BaseStream.Seek(header.Offset, SeekOrigin.Begin);
            return reader;
        }

        //TODO: Generate decompress stream
        throw new NotSupportedException();
    }

}
