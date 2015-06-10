using System;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ModelPropertyIgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ModelPropertiesCompleteAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ModelPropertyNameAttribute : Attribute
    {
        public ModelPropertyNameAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public ModelPropertyNameAttribute(string propertyName, bool mustExist)
        {
            this.PropertyName = propertyName;
            this.MustExist = mustExist;
        }

        public string PropertyName { get; set; }
        public bool MustExist { get; set; }

    }
}