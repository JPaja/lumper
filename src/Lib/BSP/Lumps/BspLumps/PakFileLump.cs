using System;
using System.IO;
using SharpCompress.Archives.Zip;

namespace Lumper.Lib.BSP.Lumps.BspLumps
{
    public class PakFileLump : ManagedLump<BspLumpType>
    {
        public MemoryStream DataStream { get; set; }
        public override void Read(Stream stream, long lenght)
        {
            DataStream = new MemoryStream();
            int read;
            long remaining = lenght;
            var buffer = new byte[80 * 1024];
            while ((read = stream.Read(buffer, 0, (int)Math.Min(buffer.Length, remaining))) > 0)
            {
                DataStream.Write(buffer, 0, read);
            }
        }

        public override void Write(Stream stream)
        {
            if (Compress)
                throw new InvalidDataException("don't compress the zip");
            DataStream.Seek(0, SeekOrigin.Begin);
            DataStream.CopyTo(stream);
        }

        public override bool Empty()
        {
            return DataStream is null || DataStream.Length <= 0;
        }

        public PakFileLump(BspFile parent) : base(parent)
        {
            Compress = false;
        }
        public ZipArchive GetZipArchive()
        {
            return ZipArchive.Open(DataStream);
        }

        public void SetZipArchive(ZipArchive zip)
        {
            var temp = new MemoryStream();
            zip.SaveTo(temp, new SharpCompress.Writers.WriterOptions(SharpCompress.Common.CompressionType.None));
            DataStream = temp;
        }
    }
}