using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{
    public abstract class ValidatableBase : MvvmViewModelBase
    {
        private string myValidationErrors;

        public async Task<bool> ValidateAsync(List<Tuple<object, bool>> propertyCrumbList = null)
        {

            try
            {
                if (propertyCrumbList == null)
                {
                    propertyCrumbList = new List<Tuple<object, bool>>();
                    propertyCrumbList.Add(new Tuple<object, bool>(this, string.IsNullOrWhiteSpace(this.ValidationErrors)));
                }
                else
                {
                    var findMe = (
                        from item in propertyCrumbList
                        where object.Equals(item.Item1, this)
                        select item).SingleOrDefault();
                    if (findMe != null)
                    {
                        return findMe.Item2;
                    }
                }

                string validationResult = null;

                foreach (var propToPropItem in
                    from propItem in this.GetType().GetRuntimeProperties()
                    select propItem)
                {

                    if (typeof(ValidatableBase).GetTypeInfo().IsAssignableFrom(propToPropItem.PropertyType.GetTypeInfo()))
                    {
                        var validatableTemp = (ValidatableBase)propToPropItem.GetValue(this);
                        if (validatableTemp == null)
                        {
                            Debug.WriteLine("WARNING: A validatable property is Nothing (null in CSharp)," + "but this class may be used in a different context with this property. Propertyname:" + propToPropItem.Name);
                        }
                        else
                        {
                            if (!(await validatableTemp.ValidateAsync(propertyCrumbList)))
                            {
                                ValidationErrors += ((ValidatableBase)propToPropItem.GetValue(this)).ValidationErrors;
                            }
                        }
                    }

                    if (propToPropItem.PropertyType.GetTypeInfo().IsGenericType && propToPropItem.PropertyType.GetTypeInfo().GetGenericTypeDefinition().Name == "Validatable`1")
                    {
                        var validatableForProperty = propToPropItem.GetValue(this) as IValidatable;

                        if (validatableForProperty != null)
                        {
                            if (validatableForProperty.ValidatorAsync != null)
                            {
                                if (!(await validatableForProperty.ValidatorAsync.Invoke()))
                                {
                                    if (validationResult == null)
                                        validationResult = "";
                                    validationResult += validatableForProperty.ValidationErrorText + Environment.NewLine;
                                }
                            }
                        }
                    }
                }

                this.ValidationErrors += validationResult;
            }
            catch
            {
                System.Diagnostics.Debugger.Break();
            }

            return string.IsNullOrWhiteSpace(ValidationErrors);

        }

        public string ValidationErrors
        {
            get
            {
                return myValidationErrors;
            }
            set
            {
                SetProperty(ref myValidationErrors, value);
            }
        }
    }
}