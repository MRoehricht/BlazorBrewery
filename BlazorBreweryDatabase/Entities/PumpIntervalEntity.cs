using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorBrewery.Database.Entities
{
    public class PumpIntervalEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        [MaxLength(100)]
        public string? Name { get; set; }

        public int RuntimeSeconds { get; set; }

        public int PausetimeSeconds { get; set; }
    }
}
