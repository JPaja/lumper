namespace Lumper.Lib.BSP.IO
{
    public class LumpHeader
    {
        public LumpHeader()
        {
            Offset = -1;
            UncompressedLength = -1;
            CompressedLength = -1;
        }
        public LumpHeader(long offset,
                          long uncompressedLength,
                          long compressedLength)
        {
            Offset = offset;
            UncompressedLength = uncompressedLength;
            CompressedLength = compressedLength;

        }
        public long Offset { get; set; }
        public long UncompressedLength { get; set; }
        public long CompressedLength { get; set; }

        public bool Compressed { get { return CompressedLength >= 0; } }
        //the actual length
        public long Length { get { return Compressed ? CompressedLength : UncompressedLength; } }
    }
}