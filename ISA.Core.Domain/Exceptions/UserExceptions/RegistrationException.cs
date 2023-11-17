namespace ISA.Core.Domain.Exceptions.UserExceptions;

public class RegistrationException : Exception
{
	public RegistrationException() : base() { }
	public RegistrationException(string Message) : base(Message) { }
    public RegistrationException(string message, Exception innerException) : base(message, innerException) {}


}
