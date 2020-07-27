using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace DynamicClassCreation
{
    /// <summary>
    /// Auxiliary object to hold the dynamically created class
    /// </summary>
    public class Globals
    {
        public dynamic CreatedObj;
    }

    public class DynamicClassCreator
    {
        /// <summary>
        /// Calls the Roslyn compiler with the provided code 
        /// </summary>
        /// <param name="code">C# Source code</param>
        /// <param name="globals">Global variables that the source code uses</param>
        /// <param name="references">Assembly references variables that the source code uses</param>
        /// <param name="imports">Explicit imports that the source code uses</param>
        /// <returns></returns>
        private static async Task<object> Exec(string code, object globals = null,
            string[] references = null, string imports = null)
        {
            try
            {
                object result = null;
                ScriptOptions options = null;
                if (references != null)
                {
                    options =
                        ScriptOptions.Default.WithReferences(references);
                }
                if (imports != null)
                {
                    if (options == null)
                        options = ScriptOptions.Default.WithImports(imports);
                    else options = options.WithImports(imports);
                }
                result = await CSharpScript.EvaluateAsync(code,
                    options, globals: globals);
                //Evaluation result
                return result;
            }
            catch (CompilationErrorException e)
            {
                //Returns full compilation error 
                return string.Join(Environment.NewLine, e.Diagnostics);
            }
        }

        /// <summary>
        /// Demo function about how a dynamic class with a known interface at compile time
        /// can be created for both flexibility and type safety.
        ///
        /// If the code is somewhat invalid, a compilation error is thrown.
        /// This demo returns a single instance of the class. It should be easy to modify the
        /// code to, once the class is created, return as many instances as necessary.
        /// </summary>
        /// <typeparam name="T">Interface or class that the dynamically created class inherits or implements</typeparam>
        /// <param name="className">Name of the dynamic class</param>
        /// <param name="fields">Fields to add to the class</param>
        /// <param name="methods">Methods to add to the class</param>
        /// <returns>An instance of a new class created at runtime implementing the T interface members</returns>
        public static T BuildDynamicClass<T>(string className, IEnumerable<DynamicProperty> fields,
            IEnumerable<DynamicMethod> methods = null)
        {
            var dt = new DynamicType { Name = className, Implements = typeof(T) };

            foreach (var field in fields)
                dt.Fields.Add(field);

            foreach (var method in methods)
                dt.Methods.Add(method);

            //Console.WriteLine(dt + "; createdObj = new " + dt.Name + "()");

            // This demo only supports non-parameter constructors
            var globals = new Globals { };

            //If this functionality is extended, extra references / imports could be provided
            //as parameters. This demo just use a default configuration of both
            dynamic ret2 = Exec(dt + ";\n CreatedObj = new " + dt.Name + "();",
                globals: globals,
                references: new string[] { "System", typeof(T).Assembly.ToString() },
                imports: typeof(T).Namespace).Result;
            if (ret2 != null) throw new Exception("Compilation error: \n" + ret2);

            return (T)globals.CreatedObj;
        }
    }
}
