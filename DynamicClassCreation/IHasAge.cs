namespace DynamicClassCreation
{
    /// <summary>
    /// Demo interface for the example
    /// </summary>
    public interface IHasAge
    {
        public int BirthYear { get; set; }
        public int BirthMonth { get; set; }
        public int BirthDay { get; set; }

        public int GetAge();

    }
}
