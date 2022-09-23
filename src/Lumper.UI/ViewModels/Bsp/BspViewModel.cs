using Lumper.UI.ViewModels.Bsp.Lump;
using ReactiveUI;

namespace Lumper.UI.ViewModels.Bsp;

public class BspViewModel : ViewModelBase
{
    public LumpViewModel?[] Lumps { get; } = new LumpViewModel?[64];
}