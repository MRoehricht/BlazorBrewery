using BlazorBrewery.Core.Models.Ingredients;
using System.ComponentModel.DataAnnotations;

namespace BlazorBrewery.Core.Models.Brewing
{
    /// <summary>
    /// Braurezept
    /// </summary>
    public class BrewingRecipe
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name ist erforderlich. ")]
        [StringLength(100, ErrorMessage = "Die Namenslänge darf nicht mehr als 100 Zeichen betragen.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "Die Beschreibung darf nicht mehr als 500 Zeichen betragen.")]
        public string? Description { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<BrewingStep> BrewingSteps { get; set; } = new List<BrewingStep>();
    }
}
