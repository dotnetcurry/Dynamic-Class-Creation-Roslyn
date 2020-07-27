using System;

namespace DynamicClassCreation
{
    /// <summary>
    /// Demo class to obtain method source code dynamically
    /// </summary>
    public class Person
    {
        public int BirthYear { get; set; }
        public int BirthMonth { get; set; }
        public int BirthDay { get; set; }


        public int GetAge()
        {
            return (int) DateTime.Now.Subtract(
                       new DateTime((int) this.BirthYear,
                           (int) this.BirthMonth,
                           (int) this.BirthDay)).TotalDays / 365;
        }
    }
}
