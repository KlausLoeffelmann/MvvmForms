using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using ActiveDevelop.MvvmBaseLib.Mvvm;

namespace ActiveDevelop.MvvmBaseLib
{
    /// <summary>
    /// Base class for binding properties having a default value and which are asynchronously retrieving their actual values on requesting their bound properties.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    [MvvmSystemElement]
    public class BindableAsyncLazy<t> : INotifyPropertyChanged, IDiscoverableValue<t>
	{
		private t myDefaultValue;
		private t myValue;
		private bool myIsLoaded;
		private Func<object, Task<t>> myValueLoader;
		private object myParam;
		private object mySyncLockObject = new object();
		private SynchronizationContext mySyncContext;
		private bool myHasBeenCalled;

        /// <summary>
        /// Event which is called whenever a property has been set to a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private static PropertyChangedEventArgs CachedNotifyPropertyChangedEventArgs = new PropertyChangedEventArgs("Value");

        /// <summary>
        /// Creates an Instance of this method.
        /// </summary>
        /// <param name="valueLoaderAsync">Function delegate, which can be called asynchronously, and which is resposible for loading the actual value.</param>
        /// <param name="defaultValue">A default value, which is been handed out as long as the actual property value is been loaded asynchronously.</param>
        public BindableAsyncLazy(Func<object, Task<t>> valueLoaderAsync, t defaultValue)
		{
			myValueLoader = valueLoaderAsync;
			myDefaultValue = defaultValue;
			mySyncContext = SynchronizationContext.Current;
		}

        /// <summary>
        /// The actual Value of the property. First reading returns the default value, and triggers loading the actual value asynchronously.
        /// </summary>
        public t Value
		{
			get
			{
                Func<bool> tmpBoolEval = () => {
                    lock (mySyncLockObject) {
                        return myIsLoaded;
                    } };

                if (!(tmpBoolEval()))
                {
                    //It's not really fire and forget, since the PropertyChangedEvent is called after completion.
                    if (!myHasBeenCalled)
                    {
                        myHasBeenCalled = true;
                        OnLoadValueAsync(myParam);
                    }

                    return myDefaultValue;
                }
                else
                {
                    return myValue;
                }

			}
			set
			{
				if (!(object.Equals(myValue, value)))
				{
					myValue = value;
					OnValuePropertyChanged();
				}
			}
		}

		[DebuggerNonUserCode]
		private async void OnLoadValueAsync(object o)
		{
			var oTemp = o;

            if (myValueLoader != null)
			{
				myValue = await myValueLoader(o);
			}
			else
			{
				return;
			}

			lock (mySyncLockObject)
			{
				myIsLoaded = true;
			}

			mySyncContext.Post((ignoreValue) => {
								   OnValuePropertyChanged();
							   }, null);
		}

		private void OnValuePropertyChanged()
		{
			if (PropertyChanged != null)
				PropertyChanged(this, CachedNotifyPropertyChangedEventArgs);
		}

		void ActiveDevelop.MvvmBaseLib.Mvvm.IDiscoverableValue.SetValue(object value)
		{
			this.SetValue(value);
		}
		private void SetValue(object value)
		{
			this.myDefaultValue = (t)value;
		}

		object ActiveDevelop.MvvmBaseLib.Mvvm.IDiscoverableValue.GetValue()
		{
			return this.GetValue();
		}
		private object GetValue()
		{
			if (this.myIsLoaded)
			{
				return myValue;
			}
			else
			{
				return myDefaultValue;
			}
		}

		public object Param
		{
			get
			{
				return myParam;
			}
			set
			{
				myParam = value;
                DefaultValue = default(t);

                lock (mySyncLockObject)
                {
                    myIsLoaded = false;
                    myHasBeenCalled = false;
                }

                OnValuePropertyChanged();
			}
		}

		public t DefaultValue
		{
			get
			{
				return myDefaultValue;
			}
			set
			{
				if (!(object.Equals(myDefaultValue, value)))
				{
					myDefaultValue = value;
					lock (mySyncLockObject)
					{
						myIsLoaded = false;
						myHasBeenCalled = false;
					}
				}

			}
		}

		public Func<object, Task<t>> ValueLoader
		{
			get
			{
				return myValueLoader;
			}
			set
			{
				if (!(object.Equals(myValueLoader, value)))
				{
					myValueLoader = value;
				}
			}
		}

	}

}