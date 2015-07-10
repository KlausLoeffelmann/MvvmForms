using System;
using System.Collections.Generic;
using System.Reflection;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{
    public class TypeDetector
    {

        private object myObjectToExamine;
        private List<Type> myTypes;

        private TypeDetector(object objToTypeDetect)
        {
            myObjectToExamine = objToTypeDetect;
        }

        private void FindTypes(Type t)
        {
            foreach (var propItem in myObjectToExamine.GetType().GetRuntimeProperties())
            {
                if (propItem.PropertyType.GetTypeInfo().IsPrimitive)
                {
                    continue;
                }

                if (myTypes.Contains(propItem.PropertyType))
                {
                    continue;
                }
                else
                {
                    myTypes.Add(propItem.PropertyType);
                    FindTypes(propItem.PropertyType);
                }
            }
        }

        public static IEnumerable<Type> GetTypes(object obj)
        {

            if (obj.GetType().GetTypeInfo().IsPrimitive)
            {
                return new List<Type>() { obj.GetType() };
            }

            TypeDetector td = new TypeDetector(obj);
            td.myTypes = new List<Type>() { obj.GetType() };
            td.FindTypes(obj.GetType());
            return td.myTypes;

        }
    }
}