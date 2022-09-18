using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Lumper.Lib.BSP.Lumps.GameLumps;
using Lumper.Lib.BSP.Struct;
using Lumper.Lib.BSP.IO;

namespace Lumper.Lib.BSP.Lumps.BspLumps
{
    public class GameLump : ManagedLump<BspLumpType>
    {
        //todo dictionary with id as key?
        //todo int or gamelumptype?
        public Dictionary<GameLumpType, Lump> Lumps { get; set; } = new();
        public T GetLump<T>() where T : Lump<GameLumpType>
        {
            var tLumps = Lumps.Where(x => x.Value.GetType() == typeof(T));
            return (T)tLumps.Select(x => x.Value).FirstOrDefault((T)Activator.CreateInstance(typeof(T), Parent));
        }
        public override void Read(Stream stream, long length)
        {
            var gameLumpReader = new GameLumpReader(this, stream, length);
            gameLumpReader.Load();
        }

        public override void Write(Stream stream)
        {
            var gameLumpWriter = new GameLumpWriter(this, stream);
            gameLumpWriter.Save();
        }

        public override bool Empty()
        {
            return !Lumps.Any();
        }

        public GameLump(BspFile parent) : base(parent)
        {
            Compress = false;
        }
    }
}