using Lumper.Core.BSP.Enums;
using Lumper.Core.BSP.Lumps;
using Lumper.Core.Collections;

namespace Lumper.Core.BSP;

public partial class BspImage
{
    private readonly OwnedDictionary<BspImage, BspLumpType, IBspLump> _lumps;

    private BspImage(BinaryReader reader)
    {
        _lumps = new(this);
        Read(reader);
    }

    public BspImage()
    {
        _lumps = new(this);
    }

    public OwnedDictionary<BspImage, BspLumpType, IBspLump> Lumps => _lumps;
    public int Revision { get; set; } = 0;
    public int Version { get; set; } = 0;


    public static BspImage FromFile(string path)
    {
        var stream = File.OpenRead(path);
        return FromStream(stream);
    }

    public static BspImage FromStream(Stream stream)
    {
        var reader = new BinaryReader(stream);
        return new BspImage(reader);
    }
}
