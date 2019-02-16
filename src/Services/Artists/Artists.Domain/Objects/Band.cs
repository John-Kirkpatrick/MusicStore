using Artists.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artists.Domain.Objects
{
    public class Band : Entity, IAggregateRoot
    {
        public Band()
        {
            Artists = new HashSet<Artist>();
        }

        [Column("BandId")]
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        //[ForeignKey("ArtistId")]
        //[InverseProperty("Band")]
        public ICollection<Artist> Artists { get; set; }
    }
}
