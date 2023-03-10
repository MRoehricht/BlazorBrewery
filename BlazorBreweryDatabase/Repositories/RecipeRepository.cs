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
        private void CreateDefaultRecipe()
        {
            var brewingRecipe = new RecipeEntity { Id = Guid.NewGuid(), Name = "Tenne", Description = "Erstes Default Rezept" };
            _recipeContext.Recipes.Add(brewingRecipe);
            _recipeContext.SaveChanges();
        }

        public async Task<List<BrewingRecipe>> AllBrewingRecipes()
        {
            var recipes = await _recipeContext.Recipes.Include(c => c.Ingredients).ThenInclude(c => c.Unit).Include(c => c.Steps).ToListAsync();

            if (recipes.Count == 0)
                CreateDefaultRecipe();

            var output = new List<BrewingRecipe>();

            foreach (var recipe in recipes)
            {
                var parsedValue = Parse(recipe);
                if (parsedValue == null) continue;
                output.Add(parsedValue);
            }

            return output;
        }

        public BrewingRecipe GetBrewingRecipe(Guid Id)
        {
            return new BrewingRecipe();
        }

        public async Task<BrewingRecipe> Add(BrewingRecipe brewingRecipe)
        {
            var brewingRecipeDB = new RecipeEntity { Id = brewingRecipe.Id };
            _recipeContext.Recipes.Add(brewingRecipeDB);
            await _recipeContext.SaveChangesAsync();

            SetRecipeEntity(brewingRecipeDB, brewingRecipe);
            await _recipeContext.SaveChangesAsync();

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
                await Add(brewingRecipe);
            }


            return brewingRecipe;
        }

        public async Task Delete(BrewingRecipe brewingRecipe)
        {
            var dbItem = await _recipeContext.Recipes.FindAsync(brewingRecipe.Id);
            if (dbItem != null)
            {
                _recipeContext.Recipes.Remove(dbItem);
                await _recipeContext.SaveChangesAsync();
            }
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
                    dbStep.DurationSeconds = step.Typ == BrewingStepTyp.HoldTemperature ? step.DurationMinutes * 60 : 0;
                    dbStep.Position = step.Position;
                    dbStep.TargetTemperature = step.Typ == BrewingStepTyp.HoldTemperature || step.Typ == BrewingStepTyp.Heat ? step.TargetTemperature : 0;
                    dbStep.Typ = step.Typ;
                    dbStep.PumpIntervalId = step.Pumpinterval?.Id ?? step.PumpintervalId;
                    dbStep.Acknowledge = step.Acknowledge;
                }
                else
                {
                    _recipeContext.Steps.Add(new StepEntity
                    {
                        Id = step.Id,
                        Name = step.Name,
                        DurationSeconds = step.Typ == BrewingStepTyp.HoldTemperature ? step.DurationMinutes * 60 : 0,
                        Position = step.Position,
                        RecipeId = entity.Id,
                        TargetTemperature = step.Typ == BrewingStepTyp.HoldTemperature || step.Typ == BrewingStepTyp.Heat ? step.TargetTemperature : 0,
                        Typ = step.Typ,
                        PumpIntervalId = step.Pumpinterval?.Id ?? step.PumpintervalId,
                        Acknowledge = step.Acknowledge
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
                var parsedValue = Parse(ingredientsin);
                if (parsedValue == null) continue;
                recipe.Ingredients.Add(parsedValue);
            }

            foreach (var step in entity.Steps)
            {
                var parsedValue = Parse(step);
                if (parsedValue == null) continue;
                recipe.BrewingSteps.Add(parsedValue);
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

        private BrewingStep? Parse(StepEntity entity)
        {
            if (entity == null) return null;
            Pumpinterval? pumpinterval = null;
            if (entity.PumpIntervalId != null)
            {
                var interval = _recipeContext.PumpIntervals.Find(entity.PumpIntervalId);
                if (interval != null)
                {
                    pumpinterval = Parse(interval);
                }
            }

            return new BrewingStep { Id = entity.Id, Name = entity.Name, BrewingRecipeId = entity.RecipeId, DurationMinutes = (entity.DurationSeconds / 60), Position = entity.Position, TargetTemperature = entity.TargetTemperature, Typ = entity.Typ, PumpintervalId = entity.PumpIntervalId, Pumpinterval = pumpinterval, Acknowledge = entity.Acknowledge };
        }

        public async Task<List<Unit>> GetUnits()
        {
            var output = new List<Unit>();
            var unitEnities = await _recipeContext.Units.ToListAsync();
            if (unitEnities.Count == 0)
            {
                await CreateDefaultUnits();
            }

            foreach (var unit in unitEnities)
            {
                if (unit == null) continue;
                output.Add(Parse(unit));
            }
            return output;
        }

        public async Task<BrewingStep?> GetBrewingStep(Guid id)
        {
            var entity = await _recipeContext.Steps.FindAsync(id);

            if (entity == null) return null;

            return Parse(entity);
        }

        private async Task CreateDefaultUnits()
        {
            var unit_ml = new UnitEntity { Id = Guid.NewGuid(), Name = "ml" };
            var unit_l = new UnitEntity { Id = Guid.NewGuid(), Name = "l" };
            var unit_kg = new UnitEntity { Id = Guid.NewGuid(), Name = "kg" };
            var unit_g = new UnitEntity { Id = Guid.NewGuid(), Name = "g" };
            _recipeContext.Units.Add(unit_ml);
            _recipeContext.Units.Add(unit_l);
            _recipeContext.Units.Add(unit_kg);
            _recipeContext.Units.Add(unit_g);

            await _recipeContext.SaveChangesAsync();
        }

        private async Task CreateDefaultPumpIntervals()
        {
            PumpIntervalEntity pumpIntervalEntityNichtPumpen = new PumpIntervalEntity { Id = Guid.NewGuid(), Name = "Nicht pumpen", RuntimeSeconds = 0, PausetimeSeconds = 0 };
            PumpIntervalEntity pumpIntervalEntity120_45 = new PumpIntervalEntity { Id = Guid.NewGuid(), Name = "120/45", RuntimeSeconds = 120, PausetimeSeconds = 45 };
            PumpIntervalEntity pumpIntervalEntityPumpen = new PumpIntervalEntity { Id = Guid.NewGuid(), Name = "Pumpen", RuntimeSeconds = 60, PausetimeSeconds = 0 };
            _recipeContext.PumpIntervals.Add(pumpIntervalEntityNichtPumpen);
            _recipeContext.PumpIntervals.Add(pumpIntervalEntity120_45);
            _recipeContext.PumpIntervals.Add(pumpIntervalEntityPumpen);
            await _recipeContext.SaveChangesAsync();

        }

        public async Task<List<Pumpinterval>> GetAllPumpintervals()
        {
            var list = await _recipeContext.PumpIntervals.ToListAsync();
            if (list.Count == 0)
            {
                await CreateDefaultPumpIntervals();
            }


            List<Pumpinterval> output = new List<Pumpinterval>();
            foreach (var interval in list)
            {
                output.Add(Parse(interval));
            }

            return output;
        }

        public async Task<Pumpinterval> CreateEmptyPumpInterval()
        {
            var id = Guid.NewGuid();
            await _recipeContext.PumpIntervals.AddAsync(new PumpIntervalEntity { Id = id, Name = "neu", PausetimeSeconds = 0, RuntimeSeconds = 0 });
            await _recipeContext.SaveChangesAsync();
            var newInterval = await _recipeContext.PumpIntervals.FindAsync(id);

            return Parse(newInterval);
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
