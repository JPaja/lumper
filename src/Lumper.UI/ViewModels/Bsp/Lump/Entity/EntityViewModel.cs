using System.Reactive;
using DynamicData;

namespace Lumper.UI.ViewModels.Bsp.Lump.Entity;

public class EntityViewModel : BspNode
{
    private SourceList<EntityPropertyViewModel> _properties = new SourceList<EntityPropertyViewModel>();
    private ListObservable<BspNode> _someList;
    public EntityViewModel()
    {
        InitializeNodes(_properties);
    }

    public override string NodeName => "Entity";
    public override string NodeIcon => "Entity.png";

    public ISourceList<EntityPropertyViewModel> Properties => _properties;
}