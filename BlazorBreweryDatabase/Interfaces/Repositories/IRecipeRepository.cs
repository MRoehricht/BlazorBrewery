using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Ingredients;

namespace BlazorBrewery.Database.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        BrewingRecipe Add(BrewingRecipe brewingRecipe);
        Task<List<BrewingRecipe>> AllBrewingRecipes();
        void Delete(BrewingRecipe brewingRecipe);
        BrewingRecipe GetBrewingRecipe(Guid Id);
        Task<BrewingRecipe> Save(BrewingRecipe brewingRecipe);
        Task<List<Unit>> GetUnits();
        Task<BrewingStep?> GetBrewingStep(Guid id);
    }
}