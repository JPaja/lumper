namespace Lumper.Core.BSP.Exceptions;

public class BspLumpReadOverflow : BspException
{
    public BspLumpReadOverflow() 
        : base($"Bsp lump reading overflow'")
    {
    }
}
