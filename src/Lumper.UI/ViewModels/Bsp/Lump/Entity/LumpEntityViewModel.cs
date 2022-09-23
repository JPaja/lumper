using DynamicData;

namespace Lumper.UI.ViewModels.Bsp.Lump.Entity;

public class LumpEntityViewModel : LumpViewModel
{
    private SourceList<EntityViewModel> _entities = new();
    public LumpEntityViewModel()
    {
        InitializeNodes(_entities);
    }

    public ISourceList<EntityViewModel> Entities => _entities;
    public override string Type => "LUMP-Entity";
    public override string NodeIcon => "Lump-Entity.png";
}