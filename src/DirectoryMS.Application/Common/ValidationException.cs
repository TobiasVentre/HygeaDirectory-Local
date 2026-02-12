namespace DirectoryMS.Application.Common;

public class ValidationException : ApplicationExceptionBase
{
    public ValidationException(string message) : base(message)
    {
    }
}
