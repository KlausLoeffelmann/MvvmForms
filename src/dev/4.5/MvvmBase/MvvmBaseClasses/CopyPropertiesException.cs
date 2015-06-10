using System;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{
    public class CopyPropertiesException : Exception
    {
        public CopyPropertiesException(string message) : base(message)
        {
        }

        public CopyPropertiesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}