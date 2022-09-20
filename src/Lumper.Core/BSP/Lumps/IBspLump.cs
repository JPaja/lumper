using Lumper.Core.Collections;
using System.Runtime.Versioning;

namespace Lumper.Core.BSP.Lumps;


public interface IBspLump : IOwnedElement<BspImage>
{
    /*
    //TODO: Enable .net 7 static abstract interfaces
    [RequiresPreviewFeatures]
    static abstract IBspLump Read(BinaryReader reader, int lenght);
    */
}
