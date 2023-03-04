using BlazorBrewery.Core.Models.Brewing;

namespace BlazorBreweryServer.ViewModels.Brewing
{
    public class BrewingViewModel
    {
        public List<BrewingRecipe> Recipes { get; set; } = new List<BrewingRecipe>();

        public BrewingRecipe? SelectedRecipe { get; set; }
    }
}
