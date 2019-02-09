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

        public string Name { get; set; }
    }
}
