using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBreweryServer.ViewModels.Recipes
{
    public class RecipesViewModel
    {
        public List<BrewingRecipe> Recipes { get; set; } = new List<BrewingRecipe>();
    }
}
