namespace DirectoryMS.Application.Common;

public class NotFoundException : ApplicationExceptionBase
{
    public NotFoundException(string message) : base(message)
    {
    }
}
