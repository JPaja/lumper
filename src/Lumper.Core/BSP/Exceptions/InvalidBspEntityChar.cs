namespace Lumper.Core.BSP.Exceptions;

public class InvalidBspEntityChar : BspException
{
    public InvalidBspEntityChar(char expected, char actual) 
        : base($"Bsp entity expected char \'{expected}\' but actually got char \'{actual}\'")
    {
    }
}
