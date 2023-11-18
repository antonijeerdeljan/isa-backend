using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISA.Core.Domain.Exceptions.UserExceptions
{
    public class InvalidRoleException : Exception
    {
        public InvalidRoleException() { }
        public InvalidRoleException(string message) : base(message) { }
        
    }
}
