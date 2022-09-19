using Lumper.Core.Collections;

namespace Lumper.Core.BSP.Lumps;

public abstract class BspLump : IOwnedElement<BspImage>
{
    public abstract BspImage? Owner { get; set; }

}
