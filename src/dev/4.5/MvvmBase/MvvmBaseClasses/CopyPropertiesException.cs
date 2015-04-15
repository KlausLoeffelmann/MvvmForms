using System;

namespace ActiveDevelop.MvvmBaseLib
{
	namespace Mvvm
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

}