using BlazorBrewery.Core.Models.Brewing;
using BlazorBrewery.Core.Models.Ingredients;
using BlazorBrewery.Database.Context;
using BlazorBrewery.Database.Entities;
using BlazorBrewery.Database.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlazorBrewery.Database.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly RecipeContext _recipeContext;

        public RecipeRepository(RecipeContext recipeContext)
        {
            _recipeContext = recipeContext;
        }

        public async Task<List<BrewingRecipe>> AllBrewingRecipes()
        {
            var recipes = await _recipeContext.Recipes.Include(c => c.Ingredients).ThenInclude(c => c.Unit).Include(c => c.Steps).ToListAsync();

            var output = new List<BrewingRecipe>();

            foreach (var recipe in recipes)
            {
                output.Add(Parse(recipe));
            }

            return output;
        }

        public BrewingRecipe GetBrewingRecipe(Guid Id)
        {
            return new BrewingRecipe();
        }

        public BrewingRecipe Add(BrewingRecipe brewingRecipe)
        {
            return brewingRecipe;
        }

        public async Task<BrewingRecipe> Save(BrewingRecipe brewingRecipe)
        {

            var recipeEntity = await _recipeContext.Recipes.FindAsync(brewingRecipe.Id);
            if (recipeEntity != null)
            {
                SetRecipeEntity(recipeEntity, brewingRecipe);
                await _recipeContext.SaveChangesAsync();
            }
            else
            {
                Add(brewingRecipe);
            }


            return brewingRecipe;
        }

        public void Delete(BrewingRecipe brewingRecipe)
        {

        }

        private void SetRecipeEntity(RecipeEntity entity, BrewingRecipe brewingRecipe)
        {
            entity.Name = brewingRecipe.Name;
            entity.Description = brewingRecipe.Description;
            SetIngredientEntity(entity, brewingRecipe.Ingredients);
        }

        private void SetIngredientEntity(RecipeEntity entity, List<Ingredient> ingredients)
        {

        }


        private BrewingRecipe? Parse(RecipeEntity? entity)
        {
            if (entity == null) return null;
            var recipe = new BrewingRecipe { Id = entity.Id, Name = entity.Name, Description = entity.Description };
            foreach (var ingredientsin in entity.Ingredients)
            {
                recipe.Ingredients.Add(Parse(ingredientsin));
            }

            foreach (var step in entity.Steps)
            {
                recipe.BrewingSteps.Add(Parse(step));
            }


            return recipe;
        }

        private Ingredient? Parse(IngredientEntity? entity)
        {
            if (entity == null) return null;
            return new Ingredient { Id = entity.Id, Name = entity.Name, Amount = entity.Amount, BrewingRecipeId = entity.RecipeId, UnitId = entity.UnitId, Unit = Parse(entity.Unit) };
        }

        private Unit? Parse(UnitEntity? entity)
        {
            if (entity == null) return null;
            return new Unit { Id = entity.Id, Name = entity.Name };
        }

        private BrewingStep Parse(StepEntity entity)
        {
            if (entity == null) return null;
            return new BrewingStep { Id = entity.Id, Name = entity.Name, BrewingRecipeId = entity.RecipeId, DurationSeconds = entity.DurationSeconds, Position = entity.Position, TargetTemperature = entity.TargetTemperature, Typ = entity.Typ };
        }

        public async Task<List<Unit>> GetUnits()
        {
            var output = new List<Unit>();
            var unitEnities = await _recipeContext.Units.ToListAsync();

            foreach (var unit in unitEnities)
            {
                output.Add(Parse(unit));
            }
            return output;
        }
    }
}
