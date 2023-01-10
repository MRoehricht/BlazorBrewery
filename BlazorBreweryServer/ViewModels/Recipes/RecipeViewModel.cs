using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Ingredients;

namespace BlazorBreweryServer.ViewModels.Recipes
{
    public class RecipeViewModel
    {
        public Guid RecipeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<BrewingStep> BrewingSteps { get; set; } = new List<BrewingStep>();
    }
}
