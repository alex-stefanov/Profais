namespace Profais.Common.Exceptions;

public class ItemNotFoundException 
    : Exception
{
    public ItemNotFoundException(string message) 
        : base(message) { }
}
