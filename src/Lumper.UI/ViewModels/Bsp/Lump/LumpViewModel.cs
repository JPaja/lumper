namespace Lumper.UI.ViewModels.Bsp.Lump;

public abstract class LumpViewModel : BspNode
{
    public override string NodeName => Type;
    public abstract string Type { get; }
}