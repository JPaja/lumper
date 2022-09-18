using System.IO;

namespace Lumper.Lib.BSP.Lumps
{
    public abstract class Lump<T> : Lump
    {
        public T Type { get; set; }
        protected Lump(BspFile parent) : base(parent)
        { }
    }

    public abstract class Lump
    {
        public bool Compress { get; set; }
        public BspFile Parent { get; set; }
        public int Version { get; set; }
        public int Flags { get; set; }

        protected Lump(BspFile parent)
        {
            Parent = parent;
        }

        public abstract void Read(Stream stream, long length);
        public abstract void Write(Stream stream);
        public abstract bool Empty();
    }
}