using ReactiveUI;

namespace Lumper.UI.ViewModels.Bsp.Lump.Entity;

public class EntityPropertyViewModel : BspNode
{
    private string _name = "";
    private string _value = "";
    public override string NodeName => Name;
    public override string NodeIcon => "EntityProperty.png";

    public string Name
    {
        get => _name;
        set =>  this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }
}