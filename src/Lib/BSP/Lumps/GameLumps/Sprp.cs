using System;
using System.IO;

namespace Lumper.Lib.BSP.Lumps.GameLumps
{
    public class Sprp : ManagedLump<GameLumpType>
    {
        public StaticPropDictLump StaticPropsDict { get; set; }
        public StaticPropLeafLump StaticPropsLeaf { get; set; }
        public StaticPropLump StaticProps { get; set; }
        public Sprp(BspFile parent) : base(parent)
        { }
        public override void Read(Stream stream, long length)
        {
            var r = new BinaryReader(stream);
            var startPos = stream.Position;

            int dictEntries = r.ReadInt32();
            StaticPropsDict = new(Parent);
            StaticPropsDict.Read(r.BaseStream, dictEntries * StaticPropsDict.StructureSize);
            int leafEntries = r.ReadInt32();
            StaticPropsLeaf = new(Parent);
            StaticPropsLeaf.Read(r.BaseStream, leafEntries * StaticPropsLeaf.StructureSize);
            int entries = r.ReadInt32();
            StaticProps = new(Parent);
            int remainingLength = (int)(length - (r.BaseStream.Position - startPos));
            StaticProps.SetVersion(Version);
            switch (StaticProps.ActualVersion)
            {
                case StaticPropVersion.V7:
                case StaticPropVersion.V10:
                    if (remainingLength % StaticProps.StructureSize != 0)
                    {
                        StaticProps.ActualVersion = StaticPropVersion.V7s;
                        Console.WriteLine($"remaining length doesn't fit version {Version} .. trying V7*");
                    }
                    break;
            }
            if (StaticProps.ActualVersion != StaticPropVersion.Unknown)
            {
                var tmpLength = entries * StaticProps.StructureSize;
                if (tmpLength != remainingLength)
                    throw new InvalidDataException($"funny staticprop length ({tmpLength} != {remainingLength})");
                StaticProps.Read(r.BaseStream, tmpLength);
            }
            else
                throw new NotImplementedException("unknown staticprop version");
        }

        public override void Write(Stream stream)
        {
            var w = new BinaryWriter(stream);
            w.Write((int)StaticPropsDict.Data.Count);
            StaticPropsDict.Write(w.BaseStream);
            w.Write((int)StaticPropsLeaf.Data.Count);
            StaticPropsLeaf.Write(w.BaseStream);
            w.Write((int)StaticProps.Data.Count);
            StaticProps.Write(w.BaseStream);
        }

        public override bool Empty()
        {
            return StaticPropsDict.Empty()
                && StaticPropsLeaf.Empty()
                && StaticProps.Empty();
        }
    }
}