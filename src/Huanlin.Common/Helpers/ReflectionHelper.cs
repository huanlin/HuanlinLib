using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;

namespace Huanlin.Common.Helpers
{
    public static class ReflectionHelper
    {
        public static string GetAssemblyFullPath(Assembly asmb)
        {
            Uri codeBaseUri = new Uri(asmb.CodeBase);
            return codeBaseUri.AbsolutePath;
        }

        public static object GetProperty(object theObject, string propertyName)
        {
            var aType = theObject.GetType();
            return aType.InvokeMember(propertyName, BindingFlags.GetProperty, null, theObject, null);
        }

        public static void SetProperty(object theObject, string propertyName, object newValue)
        {
            var aType = theObject.GetType();
            aType.InvokeMember(propertyName, BindingFlags.SetProperty, null, theObject, new object[] { newValue });
        }

        public static bool HasProperty(object theObject, string propertyName)
        {
            var aType = theObject.GetType();
            return aType.GetProperty(propertyName) != null;
        }

        /// <summary>
        /// Set object properties from an URL query string. The properties should be type of String. The other types are not tested.
        /// Note: If a property name does not exist in the object, it will be simply ignored (which mean no exception).
        /// </summary>
        /// <param name="theObject"></param>
        /// <param name="queryString">Form example</param>
        //public static void SetPropertiesFromUrlQueryString(object theObject, string queryString)
        //{
        //    string[] parameters = queryString.Split('&');
        //    foreach (string param in parameters)
        //    {
        //        string[] nameValue = param.Split('=');
        //        if (nameValue.Length == 2)
        //        {
        //            string name = nameValue[0];
        //            string value = HttpUtility.UrlDecode(nameValue[1]);

        //            var aType = theObject.GetType();
        //            var aProperty = aType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        //            if (aProperty != null)
        //            {
        //                if (aProperty.PropertyType.IsEnum)
        //                {
        //                    var valueObj = Enum.Parse(aProperty.PropertyType, value);
        //                    aProperty.SetValue(theObject, valueObj, null);
        //                }
        //                else
        //                {
        //                    aProperty.SetValue(theObject, value, null);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}