namespace BlazorBrewery.Core.Models.Ingredients
{
    /// <summary>
    /// Einheit
    /// </summary>
    public class Unit
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name;
        }
    }
}
