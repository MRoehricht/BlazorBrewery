using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorBrewery.Database.Entities
{
    public class ConfigEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        [MaxLength(100)]
        public string Description { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        [MaxLength(100)]
        public string Value { get; set; }
    }
}
