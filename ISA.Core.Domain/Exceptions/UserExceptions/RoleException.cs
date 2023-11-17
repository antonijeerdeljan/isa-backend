namespace ISA.Core.Domain.Exceptions.UserExceptions;

public class RoleException : Exception
{
	public RoleException() : base() { }

    public RoleException(string message) : base(message) { }

    public RoleException(string message, Exception innerException) : base(message, innerException) { }

}
