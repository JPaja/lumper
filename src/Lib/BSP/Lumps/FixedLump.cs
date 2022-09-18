using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using Lumper.Lib.BSP.Struct;

namespace Lumper.Lib.BSP.Lumps
{
    public abstract class FixedLump<T, U> : ManagedLump<T>
    {
        public List<U> Data { get; set; } = new();

        public abstract int StructureSize { get; }

        protected abstract void ReadItem(BinaryReader reader);
        protected abstract void WriteItem(BinaryWriter writer, int index);

        public override void Read(Stream stream, long length)
        {
            var r = new BinaryReader(stream);
            if (length % StructureSize != 0)
                throw new InvalidDataException("bad length");
            for (var i = 0; i < length / StructureSize; i++)
            {
                ReadItem(r);
            }
        }

        public override void Write(Stream stream)
        {
            var w = new BinaryWriter(stream);
            for (var i = 0; i < Data.Count; i++)
            {
                WriteItem(w, i);
            }
        }

        public override bool Empty()
        {
            return !Data.Any();
        }

        protected FixedLump(BspFile parent) : base(parent)
        {
        }
    }
}