using BlazorBrewery.Core.Models.Brewing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorBrewery.Database.Entities
{
    public class StepEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        [MaxLength(100)]
        public string? Name { get; set; }
        public int Position { get; set; }
        public int DurationSeconds { get; set; }
        public double TargetTemperature { get; set; }
        public BrewingStepTyp Typ { get; set; }

        public virtual RecipeEntity? Recipe { get; set; }
    }
}
