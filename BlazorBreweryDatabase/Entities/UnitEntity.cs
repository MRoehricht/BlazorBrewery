using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorBrewery.Database.Entities
{
    public class UnitEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        [MaxLength(100)]
        public string? Name { get; set; }

        public virtual List<IngredientEntity> Ingredients { get; set; } = new List<IngredientEntity>();
    }
}
