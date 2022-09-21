using Lumper.Core.BSP.Exceptions;
using Lumper.Core.BSP.Lumps;

namespace Lumper.Core.BSP;

partial class BspImage
{

    private readonly static ReadOnlyMemory<byte> BspHeader = new byte[]{ 0x56, 0x42, 0x53, 0x50 }; //VBSP;

    private void Read(BinaryReader reader)
    {
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        Span<byte> magic = stackalloc byte[4];
        reader.Read(magic);

        if (!magic.SequenceEqual(BspHeader.Span))
            throw new InvalidBspHeaderException();

        Version = reader.ReadInt32();
        for (int i = 0; i < _lumps.Length; i++)
        {
            var lump = IBspLump.Read(reader, i);
        }
        Revision = reader.ReadInt32();
    }
}
