using System.Diagnostics.CodeAnalysis;

namespace Lumper.Core.BSP.Lumps;

public record struct BspLumpHeader (int Offset, int Lenght, int Version, int? CompressedLenght)
{
    [MemberNotNullWhen(true, nameof(CompressedLenght))]
    public bool Compressed => CompressedLenght.HasValue;
    public int SegmentLenght => Compressed ? CompressedLenght.Value : Lenght;

    public static BspLumpHeader Parse(BinaryReader reader)
    {
        var offset = reader.ReadInt32();
        var lenght = reader.ReadInt32();
        var version = reader.ReadInt32();
        var compressedLenght = reader.ReadInt32();
        var compressed = compressedLenght != 0;
        return new BspLumpHeader(offset, lenght, version, compressed ? compressedLenght : null);
    }
}
