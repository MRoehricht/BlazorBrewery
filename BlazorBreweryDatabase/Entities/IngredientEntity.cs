using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorBrewery.Database.Entities
{
    public class IngredientEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
        public Guid UnitId { get; set; }

        public virtual RecipeEntity? Recipe { get; set; }
        public virtual UnitEntity? Unit { get; set; }
    }
}
