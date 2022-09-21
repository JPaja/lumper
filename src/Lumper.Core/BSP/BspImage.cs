using Lumper.Core.BSP.Enums;
using Lumper.Core.BSP.Lumps;
using Lumper.Core.Collections;

namespace Lumper.Core.BSP;

public partial class BspImage
{
    private const int HeaderLumps = 64;

    private readonly IBspLump?[] _lumps = new IBspLump?[HeaderLumps];

    private BspImage(BinaryReader reader)
    {
        Read(reader);
    }

    public BspImage()
    {
    }

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
