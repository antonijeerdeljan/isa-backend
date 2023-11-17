using ISA.Core.Domain.Exceptions.UserExceptions;

namespace ISA.Core.Domain.Entities.User;

public enum UserType
{
    REGISTERED,
    CORPADMIN,
    SYSADMIN
}

public static class Type
{
    public static string GetType(UserType type)
    {
        switch(type) 
        {
            case UserType.REGISTERED:
                return "registered";
            case UserType.CORPADMIN:
                return "corpadmin";
            case UserType.SYSADMIN:
                return "sysadmin";
            default:
                throw new RoleException("Role doesn't exist!");
        }
    }
}
