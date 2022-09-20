using Lumper.Core.Collections;
using System.Runtime.Versioning;

namespace Lumper.Core.BSP.Lumps.BspLumps;

public class BspDataLump : IBspLump
{
    BspImage? IOwnedElement<BspImage>.Owner { get; set; }

    private BspDataLump(BinaryReader reader, int lenght)
    {
        Data = reader.ReadBytes(lenght);
    }

    public BspDataLump(byte[] data)
    {
        Data = data;
    }

    public BspDataLump()
    {
        Data = Array.Empty<byte>();
    }

    public byte[] Data { get; set; }

    /*
    //TODO: Enable .net 7 static abstract interfaces
    [RequiresPreviewFeatures]
    static IBspLump IBspLump.Read(BinaryReader reader, int lenght)
            => Read(reader, lenght);
    */
    
    public static BspDataLump Read(BinaryReader reader, int lenght)
        => new BspDataLump(reader, lenght);
}
