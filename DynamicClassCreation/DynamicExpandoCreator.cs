using System.Collections.Generic;
using System.Dynamic;

namespace DynamicClassCreation
{
    /// <summary>
    /// This class shows a demo of how to dynamically create an object using an ExpandoObject
    /// </summary>
    public class DynamicExpandoCreator
    {
        /// <summary>
        /// Assign properties to an expando
        /// </summary>
        /// <param name="myObj">Expando object (or any IDictionary implementation)</param>
        /// <param name="dict">Property list (name, value)</param>
        private static void AssignProperties(dynamic myObj,
            IDictionary<string, object> dict)
        {
            // Add an attribute programatically
            foreach (var item in dict)
                ((IDictionary<string, object>)myObj)[item.Key] = item.Value;
        }

        /// <summary>
        /// Builds a new expando object
        /// </summary>
        /// <param name="className">Value of the ClassName field</param>
        /// <param name="fields">Other fields</param>
        /// <returns></returns>
        public static ExpandoObject BuildDynamicExpando(string className,
            Dictionary<string, object> fields)
        {
            dynamic obj = new ExpandoObject();
            obj.ClassName = className;
            AssignProperties(obj, fields);

            return obj;
        }
    }
}
