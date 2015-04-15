using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ActiveDevelop.MvvmBaseLib
{
    public static class Extender
	{
		public static Dictionary<string, object> GetChangedValues(this INotifyPropertyChanged container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			Dictionary<string, object> ret = new Dictionary<string, object>();

			var props = container.GetType().GetRuntimeProperties();
			foreach (var prop in props)
			{
				if (GetSimpleTypeName(prop.PropertyType) == GetSimpleTypeName(typeof(ChangeTracking<>)))
				{
					var hasChangedProp = prop.PropertyType.GetRuntimeProperty("HasChanged");
					if (hasChangedProp != null)
					{
						// nun den wert von "HasChanged" ermitteln
						// dazu brauchen wir als erstes aber die Instanz des ChangeTrackers
						var ct = prop.GetValue(container, null);
						if (ct != null)
						{
							var hasChanged = Convert.ToBoolean(hasChangedProp.GetValue(ct, null));
							if (hasChanged)
							{
								ret.Add(prop.Name, ct);
							}
						}
					}
				}
			}
			return ret;
		}

		private static string GetSimpleTypeName(System.Type ty)
		{
			return ty.GetTypeInfo().Assembly.GetName().Name + "." + ty.Name;
		}
	}

}