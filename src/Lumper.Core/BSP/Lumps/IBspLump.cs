using Lumper.Core.Collections;

namespace Lumper.Core.BSP.Lumps;

public partial interface IBspLump<T> : IOwnedElement<BspImage> where T: IBspLump<T>
{
    static abstract IBspLump ReadInternal(BinaryReader reader, int lenght);
    static abstract int Index { get; }
}
