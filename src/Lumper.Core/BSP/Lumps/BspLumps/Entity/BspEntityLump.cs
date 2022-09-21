using Lumper.Core.Collections;

namespace Lumper.Core.BSP.Lumps.BspLumps.Entity;

public partial class BspEntityLump : IBspLump
{
    private readonly List<BspEntity> _entities;

    public BspEntityLump()
    {
        _entities = new();
    }

    public BspEntityLump(IEnumerable<BspEntity> entities)
    {
        _entities = entities.ToList();
    }

    public static int Index => 0;

    BspImage? IOwnedElement<BspImage>.Owner { get; set; }

    public List<BspEntity> Entities => _entities;
}
