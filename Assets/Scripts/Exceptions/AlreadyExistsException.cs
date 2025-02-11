using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Exceptions
{
    public class AlreadyExistsException: Exception
    {
        public const string BaseMessage = "This element already exists in collection!";
        public AlreadyExistsException(): base(BaseMessage) { }
        public AlreadyExistsException(string message) : base(message) { }
        public AlreadyExistsException(string message,  Exception innerException) : base(message, innerException) { }
    }
}
