using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorBrewery.Database.Entities
{
    public class RecipeEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Column(TypeName = "VARCHAR(500)")]
        [MaxLength(500)]
        public string? Description { get; set; }

        public virtual List<StepEntity> Steps { get; set; } = new List<StepEntity>();

        public virtual List<IngredientEntity> Ingredients { get; set; } = new List<IngredientEntity>();
    }
}
