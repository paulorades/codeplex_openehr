using System;
using System.Collections.Generic;
using System.Text;

namespace OpenEhr.Validation
{
    public class ValidationException: System.ApplicationException
    {
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class RmInvariantException : ValidationException
    {
         public RmInvariantException(string message) : base(message) { }
        public RmInvariantException(string message, Exception innerException) : base(message, innerException) { }

    }
}

namespace OpenEhr.Serialisation
{
    public class InvalidXmlException : System.ApplicationException
    {
        public InvalidXmlException(string message) : base(message) { }
        public InvalidXmlException(string message, Exception innerException) : base(message, innerException) { }
        public InvalidXmlException(string expectedElementName, string elementName) : 
            base("Expected element name is '"+ expectedElementName +"', but it is "+elementName) { }
    }
}
