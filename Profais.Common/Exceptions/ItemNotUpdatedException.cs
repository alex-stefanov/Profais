namespace Profais.Common.Exceptions;

public class ItemNotUpdatedException
    : Exception
{
    public ItemNotUpdatedException(string message)
        : base(message) { }
}

