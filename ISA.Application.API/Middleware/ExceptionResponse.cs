namespace ISA.Application.API.Middleware;

public class ExceptionResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string? StackTrace { get; set; }

    public ExceptionResponse(Exception exception)
    {
        Type = exception.HelpLink;
        Title = exception.Message;
        Status = (int)exception.ExceptionToStatusCode();
    }

    public ExceptionResponse(Exception exception, string stackTrace)
    {
        Type = exception.HelpLink;
        Title = exception.Message;
        Status = (int)exception.ExceptionToStatusCode();
        StackTrace = stackTrace;
    }
}