using Artists.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artists.Domain.Objects
{
    public class Artist : Entity, IAggregateRoot
    {
        [Column("ArtistId")]
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Guid BandId { get; set; }

        //[ForeignKey("BandId")]
        public Band Band { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
