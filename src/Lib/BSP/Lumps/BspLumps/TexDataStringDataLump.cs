using System.IO;
using Lumper.Lib.BSP.Struct;

namespace Lumper.Lib.BSP.Lumps.BspLumps
{
    public class TexDataStringDataLump : ManagedLump<BspLumpType>
    {
        public byte[] Data;

        public override void Read(Stream stream, long length)
        {
            var r = new BinaryReader(stream);
            Data = r.ReadBytes((int)length);
        }

        public override void Write(Stream stream)
        {
            stream.Write(Data, 0, Data.Length);
        }

        public override bool Empty()
        {
            return Data.Length <= 0;
        }

        public TexDataStringDataLump(BspFile parent) : base(parent)
        {
        }
    }
}