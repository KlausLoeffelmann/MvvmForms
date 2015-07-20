using System;
using System.Threading.Tasks;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{

        public interface IValidatable
		{
			string ValidationErrorText {get; set;}
			bool ValidationFailed {get; set;}
			Func<Task<bool>> ValidatorAsync {get;}

		}

		public class Validatable<T> : MvvmBase, IDiscoverableValue<T>
		{
			private T myValue;
			private string myValidationErrorText;
			private bool myValidationFailed;
			private T myLastValidValue;

			public Validatable() : base()
			{
			}

			public Validatable(T initialValue)
			{
				this.myValue = initialValue;
				this.myLastValidValue = initialValue;
			}

			public Validatable(T initialvalue, ValidationTypes validationType)
			{
				this.myValue = initialvalue;
				this.myLastValidValue = initialvalue;
				this.ValidationType = validationType;
			}

			public Validatable(T initialValue, ValidationTypes validationType, Func<Task<bool>> validatorAsync)
			{
				this.myValue = initialValue;
				this.myLastValidValue = initialValue;
				this.ValidationType = validationType;
				this.ValidatorAsync = validatorAsync;
			}

			public T Value
			{
				get
				{
					return myValue;
				}
				set
				{
					SetProperty(ref myValue, value);
				}
			}

			public T LastValidValue
			{
				get
				{
					return myLastValidValue;
				}
				set
				{
					SetProperty(ref myLastValidValue, value);
				}
			}

			public T ValidValue
			{
				get
				{
					return (HasErrors ? Value : LastValidValue);
				}
			}

			public string ValidationErrorText
			{
				get
				{
					return myValidationErrorText;
				}
				set
				{
					if (SetProperty(ref myValidationErrorText, value))
					{
						if (string.IsNullOrEmpty(value))
						{
							ValidationFailed = false;
						}
						else
						{
							ValidationFailed = true;
						}
					}
				}
			}

			public bool ValidationFailed
			{
				get
				{
					return myValidationFailed;
				}
				set
				{
					if (SetProperty(ref myValidationFailed, value))
					{
						if (!value)
						{
							ValidationErrorText = null;
						}
					}
				}
			}

			public Func<Task<bool>> ValidatorAsync {get; set;}

			public ValidationTypes ValidationType {get; set;}

			public static implicit operator T(Validatable<T> orgType)
			{
				return orgType.Value;
			}

			public static string operator + (Validatable<T> orgType, string sType)
			{
				return orgType.ToString() + sType;
			}

			public static string operator + (string sType, Validatable<T> orgType)
			{
				return sType + orgType.ToString();
			}

			public override string ToString()
			{
				return (((((IDiscoverableValue<T>)this).Value == null) ? "<null>" : ((IDiscoverableValue<T>)this).Value.ToString()));
			}

			public void SetValue(object value)
			{
				this.Value = (T)value;
			}

			public object GetValue()
			{
				return this.Value;
			}
		}

    public enum ValidationTypes
    {
        Syntactic,
        Semantic,
        SyntacticAndSemantic
    }
}