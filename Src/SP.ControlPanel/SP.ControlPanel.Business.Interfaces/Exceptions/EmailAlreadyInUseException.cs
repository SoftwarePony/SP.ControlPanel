using System;

namespace SP.ControlPanel.Business.Interfaces.Exceptions
{
    public class EmailAlreadyInUseException : Exception
    {
        public EmailAlreadyInUseException(string message) : base(message)
        {
            
        }
    }
}