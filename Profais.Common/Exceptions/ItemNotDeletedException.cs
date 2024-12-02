namespace Profais.Common.Exceptions;

public class ItemNotDeletedException
    : Exception
{
    public ItemNotDeletedException(string message)
        : base(message) { }
}