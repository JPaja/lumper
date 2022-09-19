using Lumper.Core.BSP.Exceptions;

namespace Lumper.Core.BSP;

partial class BspImage
{
    private const int HeaderLumps = 64;
    private const int HeaderSize = 1036;
    private const int MaxLumps = 128;

    private readonly static ReadOnlyMemory<byte> BspHeader = new byte[]{ 0x56, 0x42, 0x53, 0x50 }; //VBSP;

    private void Read(BinaryReader reader)
    {
        Span<byte> header = stackalloc byte[4];
        reader.Read(header);

        if (!header.SequenceEqual(BspHeader.Span))
            throw new InvalidBspHeaderException();

        Version = reader.ReadInt32();
        ReadHeaderLumps(reader);
        Revision = reader.ReadInt32();
    }

    private void ReadHeaderLumps(BinaryReader reader)
    {

    }

    private void ReadLumps(BinaryReader reader)
    {

    }
}
