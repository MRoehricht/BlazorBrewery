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
            SetSteps(entity, brewingRecipe.BrewingSteps);
        }

        private void SetSteps(RecipeEntity entity, List<BrewingStep> brewingSteps)
        {
            foreach (var step in brewingSteps)
            {
                var dbStep = _recipeContext.Steps.Find(step.Id);
                if (dbStep != null)
                {
                    dbStep.Name = step.Name;
                    dbStep.RecipeId = entity.Id;
                    dbStep.DurationSeconds = step.DurationSeconds;
                    dbStep.Position = step.Position;
                    dbStep.TargetTemperature = step.TargetTemperature;
                    dbStep.Typ = step.Typ;
                    dbStep.PumpIntervalId = step.Pumpinterval?.Id ?? step.PumpintervalId;
                }
                else
                {
                    _recipeContext.Steps.Add(new StepEntity
                    {
                        Id = step.Id,
                        Name = step.Name,
                        DurationSeconds = step.DurationSeconds,
                        Position = step.Position,
                        RecipeId = entity.Id,
                        TargetTemperature = step.TargetTemperature,
                        Typ = step.Typ,
                        PumpIntervalId = step.Pumpinterval?.Id ?? step.PumpintervalId
                    });
                }
            }

            var dbGuids = _recipeContext.Steps.Where(_ => _.RecipeId == entity.Id).Select(_ => _.Id).ToList();
            var recipeGuids = brewingSteps.Select(s => s.Id).ToList();

            //Gelöschte
            var deletedIds = dbGuids.Except(recipeGuids).ToList();
            foreach (var id in deletedIds)
            {
                _recipeContext.Steps.Remove(_recipeContext.Steps.Find(id));
            }

            _recipeContext.SaveChanges();
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
            Pumpinterval pumpinterval = null;
            if (entity.PumpIntervalId != null)
            {
                var interval = _recipeContext.PumpIntervals.Find(entity.PumpIntervalId);
                if (interval != null)
                {
                    pumpinterval = Parse(interval);
                }
            }

            return new BrewingStep { Id = entity.Id, Name = entity.Name, BrewingRecipeId = entity.RecipeId, DurationSeconds = entity.DurationSeconds, Position = entity.Position, TargetTemperature = entity.TargetTemperature, Typ = entity.Typ, PumpintervalId = entity.PumpIntervalId, Pumpinterval = pumpinterval };
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

        public async Task<BrewingStep?> GetBrewingStep(Guid id)
        {
            var entity = await _recipeContext.Steps.FindAsync(id);

            if (entity == null) return null;

            return new BrewingStep
            {
                Id = id,
                BrewingRecipeId = entity.RecipeId,
                DurationSeconds = entity.DurationSeconds,
                Position = entity.Position,
                Name = entity.Name,
                TargetTemperature = entity.TargetTemperature,
                Typ = entity.Typ
            };
        }

        public async Task<List<Pumpinterval>> GetAllPumpintervals()
        {
            var list = await _recipeContext.PumpIntervals.ToListAsync();
            List<Pumpinterval> output = new List<Pumpinterval>();
            foreach (var interval in list)
            {
                output.Add(Parse(interval));
            }

            return output;
        }

        public Pumpinterval Parse(PumpIntervalEntity entity)
        {
            return new Pumpinterval
            {
                Id = entity.Id,
                Name = entity.Name,
                PausetimeSeconds = entity.PausetimeSeconds,
                RuntimeSeconds = entity.RuntimeSeconds
            };
        }
    }
}
