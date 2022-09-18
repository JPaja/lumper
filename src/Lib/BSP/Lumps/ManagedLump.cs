namespace Lumper.Lib.BSP.Lumps
{
    public abstract class ManagedLump<T> : Lump<T>
    {
        public ManagedLump(BspFile parent) : base(parent)
        {
        }
    }
}