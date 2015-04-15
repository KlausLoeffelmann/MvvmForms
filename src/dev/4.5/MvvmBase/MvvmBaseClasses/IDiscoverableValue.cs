namespace ActiveDevelop.MvvmBaseLib
{
	namespace Mvvm
	{

		public interface IDiscoverableValue
		{
			void SetValue(object value);
			object GetValue();
		}


		public interface IDiscoverableValue<T> : IDiscoverableValue
		{
			T Value {get; set;}

		}

	}

}