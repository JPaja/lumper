namespace Lumper.Core.BSP.Exceptions;

public class InvalidLzmaHeaderException : BspException
{
    public InvalidLzmaHeaderException() : base("Invalid LZMA header")
    {
    }
}
