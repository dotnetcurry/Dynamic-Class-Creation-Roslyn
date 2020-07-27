using System;
using System.Collections.Generic;

namespace DynamicClassCreation
{
    /// <summary>
    /// Class representing the meta-information to build a dynamic class
    /// </summary>
    public class DynamicType
    {
        /// <summary>
        /// Name of the dynamic class
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Static type that this class will inherit from or implement
        /// </summary>
        public Type Implements { get; set; }

        /// <summary>
        /// Field collection
        /// </summary>
        public List<DynamicProperty> Fields { get; set; } = new List<DynamicProperty>();

        /// <summary>
        /// Method collection
        /// </summary>
        public List<DynamicMethod> Methods { get; set; } = new List<DynamicMethod>();

        /// <summary>
        /// Build the class source code so it can be compiled dynamically later
        /// </summary>
        /// <returns>C# Source code of the class</returns>
        public override string ToString()
        {
            string result = "";
            if (Implements != null)
                result = "using " + Implements.Namespace + ";\n";
            //System is always included
            result += "using System;\n";

            result += "public class " + Name + ((Implements != null) ? ": " + Implements.FullName : "");//": IEquatable<Object>" : ""); //
            result += "\n{\n";
            foreach (var dynamicField in Fields)
                result += dynamicField;

            foreach (var dynamicMethod in Methods)
                result += dynamicMethod;

            result += "}";

            return result;
        }

    }

    public class DynamicProperty
    {
        public string Name { get; set; }
        public Type FType { get; set; }

        public override string ToString()
        {
            return "public " + FType.FullName + " " + Name + " {get; set;}\n";
        }
    }

    public class DynamicMethod
    {
        public object Signature { get; set; }
        public object Body { get; set; }

        public override string ToString()
        {
            return Signature + "\n" + Body + "\n";
        }
    }
}
