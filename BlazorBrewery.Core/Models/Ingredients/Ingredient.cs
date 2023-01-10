namespace BlazorBrewery.Core.Models.Ingredients
{
    /// <summary>
    /// Zutat 
    /// </summary>
    public class Ingredient
    {
        public Guid Id { get; set; }
        public Guid BrewingRecipeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public Guid UnitId { get; set; }
        public Unit? Unit { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Amount}{Unit?.Name}";
        }
    }
}
