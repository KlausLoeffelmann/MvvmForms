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

        private static PropertyChangedEventArgs CachedNotifyValuePropertyChangedEventArgs = new PropertyChangedEventArgs("Value");
        private static PropertyChangedEventArgs CachedNotifyDefaultValuePropertyChangedEventArgs = new PropertyChangedEventArgs("DefaultValue");
        private static PropertyChangedEventArgs CachedNotifyIsLoadedPropertyChangedEventArgs = new PropertyChangedEventArgs("IsLoaded");

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
                        return IsLoaded;
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
					OnValueChanged();
				}
			}
		}

        /// <summary>
        /// Singnals, if the value has already been been gotten asynchronously.
        /// </summary>
        public bool IsLoaded
        {
            get { return myIsLoaded; }

            set
            {
                if (!object.Equals(myIsLoaded, value))
                {
                    myIsLoaded = value;
                    OnIsLoadedChanged();
                }
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for IsLoaded.
        /// </summary>
        protected virtual void OnIsLoadedChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, CachedNotifyIsLoadedPropertyChangedEventArgs);
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
				IsLoaded = true;
			}

            mySyncContext.Post((SendOrPostCallback)((ignoreValue) => {
								   OnValueChanged();
							   }), null);
		}

        /// <summary>
        /// Raises the PropertyChanged event for Value.
        /// </summary>
        protected virtual void OnValueChanged()
		{
			if (PropertyChanged != null)
				PropertyChanged(this, CachedNotifyValuePropertyChangedEventArgs);
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
			if (this.IsLoaded)
			{
				return myValue;
			}
			else
			{
				return myDefaultValue;
			}
		}

        /// <summary>
        /// Params for the Loader delegate.
        /// </summary>
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
                    IsLoaded = false;
                    myHasBeenCalled = false;
                }

                OnValueChanged();
			}
		}

        /// <summary>
        /// Default Value, which is returned until the actual value has been loaded asynchronously. 
        /// Setting this value resets IsLoaded.
        /// </summary>
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
						IsLoaded = false;
						myHasBeenCalled = false;
					}
                    OnDefaultValueChanged();
				}

			}
		}

        /// <summary>
        /// Raises the PropertyChanged event for DefaultValue.
        /// </summary>

        protected virtual void OnDefaultValueChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, CachedNotifyDefaultValuePropertyChangedEventArgs);
        }


        /// <summary>
        /// The delegate, which loads the Value asynchronously.
        /// </summary>
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