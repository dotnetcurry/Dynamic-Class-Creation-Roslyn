using System;
using System.Collections.Generic;
using System.Linq;
using PerformanceMeasurementLibrary;

namespace DynamicClassCreation
{
    class Program
    {
        /// <summary>
        /// Iterations for performance testing
        /// </summary>
        public static int Iterations = 2000000;

        /// <summary>
        /// Creates a demo expando object
        /// </summary>
        static void ExpandoTest()
        {
            var properties = new Dictionary<string, object> {
                { "Name", "John Wick" },
                { "Pet", "Dog" }, { "BirthYear", 1978 },
                { "BirthMonth", 5 }, { "BirthDay", 8 }
            };

            dynamic person = DynamicExpandoCreator.BuildDynamicExpando("Employee",
                properties);

            //Print dynamically added properties
            Console.WriteLine("{0}, born in {3}/{2}/{1}.",
                person.Name, person.BirthYear,
                person.BirthMonth, person.BirthDay);

            Func<int> getAge = () => (int)DateTime.Now.Subtract(
                                          new DateTime((int)person.BirthYear,
                                              (int)person.BirthMonth,
                                              (int)person.BirthDay)).TotalDays / 365;
            person.GetAge = getAge; // Add a method

            Func<string> getPet = () => person.Pet;
            person.GetPet = getPet; // Add another method

            Console.WriteLine("{0} is {1} years old.",
                person.Name, person.GetAge());
            Console.WriteLine("{0} has a {1}.",
                person.Name, person.GetPet());

            // Call performance testing
            var result = Performance.Steady(() =>
            {
                for (int i = 0; i < Iterations; i++)
                {
                    var result = person.GetAge();
                }
            });

            Console.WriteLine("Expando time = " + result + " ms.");
        }

        static void DynamicClassTest()
        {
            //Dynamic properties to match the demo interface
            var p1 = new DynamicProperty { Name = "BirthYear", FType = typeof(int) };
            var p2 = new DynamicProperty { Name = "BirthMonth", FType = typeof(int) };
            var p3 = new DynamicProperty { Name = "BirthDay", FType = typeof(int) };

            //Other properties
            var p4 = new DynamicProperty { Name = "Pet", FType = typeof(string) };
            var p5 = new DynamicProperty { Name = "Name", FType = typeof(string) };

            //Obtain part of the dynamic method source from other source files
            var scf = new SourceCodeFromFile(@"..\..\..\MethodSource.cs");
            var m1 = new DynamicMethod
            {
                Signature = scf.GetMethodSignature("GetAge"),
                Body = scf.GetMethodSourceCode("GetAge")
            };

            //New methods created from source code strings
            var m2 = new DynamicMethod
            {
                Signature = "public string GetPet()",
                Body = "{ return Pet; }"
            };

            //Build dynamic class with an statically known interface. Return an instance of this
            //class
            IHasAge obj = null;
            try
            {
                obj = DynamicClassCreator.BuildDynamicClass<IHasAge>("MyDynamicEmployee",
                    new DynamicProperty[] { p1, p2, p3, p4, p5 },
                    new DynamicMethod[] { m1, m2 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }

            //Test the instance
            obj.BirthYear = 1978;
            obj.BirthMonth = 5;
            obj.BirthDay = 8;
            Console.WriteLine("This person is " + obj.GetAge() + " old.");

            //Of course dynamically typing works too
            ((dynamic)obj).Name = "John Wick";
            ((dynamic)obj).Pet = "Dog";

            Console.WriteLine("{0} has a {1}.",
                ((dynamic)obj).Name, ((dynamic)obj).GetPet());

            //Call performance testing
            var result = Performance.Steady(() =>
            {
                for (int i = 0; i < Iterations; i++)
                {
                    var result = obj.GetAge();
                }
            });

            Console.WriteLine("Dynamic class time = " + result + " ms.");
        }
        static void Main(string[] args)
        {
            ExpandoTest();
            DynamicClassTest();
        }
    }
}
