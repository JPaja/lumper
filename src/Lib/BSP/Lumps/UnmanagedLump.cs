using System;
using System.IO;
using Lumper.Lib.BSP.Enum;

namespace Lumper.Lib.BSP.Lumps
{
    public interface IUnmanagedLump
    {
        public bool Compressed { get; set; }
        public long UncompressedLength { get; set; }

    }
    public class UnmanagedLump<T> : Lump<T>, IUnmanagedLump
    {
        public bool Compressed { get; set; }
        public long UncompressedLength { get; set; }
        public Stream DataStream { get; set; }
        public long DataStreamOffset { get; set; }
        public long DataStreamLength { get; set; }
        public static readonly int LzmaId = (('A' << 24) | ('M' << 16) | ('Z' << 8) | ('L'));

        public override void Read(Stream stream, long length)
        {
            DataStream = stream;
            DataStreamOffset = stream.Position;
            DataStreamLength = length;
        }
        public override void Write(Stream stream)
        {
            if (DataStream != null)
            {
                var startPos = DataStream.Position;
                DataStream.Seek(DataStreamOffset, SeekOrigin.Begin);
                var buffer = new byte[1024 * 80];
                int read;
                int remaining = (int)DataStreamLength;
                while ((read = DataStream.Read(buffer, 0, Math.Min(buffer.Length, remaining))) > 0)
                {
                    stream.Write(buffer, 0, read);
                    remaining -= read;
                }
                DataStream.Seek(startPos, SeekOrigin.Begin);
            }
        }

        public override bool Empty()
        {
            return DataStreamLength <= 0;
        }
        public UnmanagedLump(BspFile parent) : base(parent)
        { }
    }
}