using System;
using System.ComponentModel;
using System.Reflection;

namespace ActiveDevelop.MvvmBaseLib
{
    public class ChangeTracking<T> : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private Func<T, T, bool> myHasChangedFunc;
		private T myValue;
		private T myOldValue;
		private bool myHasChanged = false;


		public ChangeTracking()
		{
			if (typeof(T).GetTypeInfo().IsValueType)
			{
				// Vergleich über Value-Types und Object.equals
				myHasChangedFunc = (p1, p2) => {
									   return !(object.Equals(p1, p2));
								   };
			}
			else
			{
				// Vergleich als Referenz-typ
				myHasChangedFunc = (p1, p2) => {
									   return ((object)p1) != (object)p2;
								   };
			}
		}

		public T Value
		{
			get
			{
				return myValue;
			}
			set
			{
				var old = myValue;
				myValue = value;
				CheckChanged();
				if (myHasChangedFunc(myValue, old))
				{
					OnPropertyChanged(new PropertyChangedEventArgs("Value"));
				}
			}
		}

		public T OldValue
		{
			get
			{
				return myOldValue;
			}
			set
			{
				myOldValue = value;
			}
		}

		public bool HasChanged
		{
			get
			{
				//Return Not Object.Equals(myValue, myOldValue)       ' wäre das ggf auch ein weg? dann würde myHasChanged + der Workflow entfallen
				return myHasChanged;
			}
		}

		private void CheckChanged()
		{
			myHasChanged = myHasChangedFunc(myValue, myOldValue);
		}

		protected void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}
	}

}