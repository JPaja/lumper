using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpCompress.Compressors.LZMA;
using Lumper.Lib.BSP.Lumps;

namespace Lumper.Lib.BSP.IO
{
    public abstract class LumpReader : BinaryReader
    {
        public byte[] Properties { get; }
        protected List<Tuple<Lump, LumpHeader>> Lumps = new();
        public LumpReader(Stream input) : base(input)
        { }
        protected abstract void ReadHeader();
        protected virtual void LoadAll()
        {
            foreach (var l in Lumps)
            {
                var lump = l.Item1;
                var lumpHeader = l.Item2;
                if (lumpHeader.Length > 0)
                    Read(lump, lumpHeader);
            }
        }
        private MemoryStream Decompress()
        {
            MemoryStream decompressedStream;
            const string magic = "LZMA";
            var id = ReadBytes(magic.Length);
            if (Encoding.ASCII.GetString(id) != magic)
                throw new InvalidDataException("that's not LZMA");
            decompressedStream = new MemoryStream();
            uint actualSize = ReadUInt32();
            uint lzmaSize = ReadUInt32();
            byte[] properties = new byte[5];
            Read(properties, 0, 5);

            var lzmaStream = new LzmaStream(properties, BaseStream, lzmaSize, actualSize);
            lzmaStream.CopyTo(decompressedStream);
            decompressedStream.Flush();
            decompressedStream.Seek(0, SeekOrigin.Begin);
            return decompressedStream;
        }
        protected void Read(Lump lump, LumpHeader lumpHeader)
        {
            bool decompress = lump is not IUnmanagedLump;

            Stream lumpStream;
            long lumpStreamLength;

            BaseStream.Seek(lumpHeader.Offset, SeekOrigin.Begin);
            var compressed = lumpHeader.Compressed;
            if (compressed)
            {
                if (decompress)
                {
                    MemoryStream decompressedStream = Decompress();
                    lumpStream = decompressedStream;
                    lumpStreamLength = decompressedStream.Length;
                    if (lump is IUnmanagedLump unmanagedLump)
                    {
                        unmanagedLump.Compressed = false;
                        unmanagedLump.UncompressedLength = -1;
                    }
                }
                else if (lump is IUnmanagedLump unmanagedLump)
                {
                    unmanagedLump.Compressed = true;
                    unmanagedLump.UncompressedLength = lumpHeader.UncompressedLength;
                    lumpStream = BaseStream;
                    lumpStreamLength = lumpHeader.CompressedLength;
                }
                else
                    throw new NotImplementedException(
                        "not decompressing a mangaged lump not implemented .."
                        + " make it unmanaged if you don't care about it or decompress and read");
            }
            else
            {
                if (lump is IUnmanagedLump unmanagedLump)
                {
                    unmanagedLump.Compressed = false;
                }
                lumpStream = BaseStream;
                lumpStreamLength = lumpHeader.UncompressedLength;
            }
            var startPos = lumpStream.Position;
            lump.Read(lumpStream, lumpStreamLength);
            lumpStream.Seek(startPos, SeekOrigin.Begin);
        }
    }
}