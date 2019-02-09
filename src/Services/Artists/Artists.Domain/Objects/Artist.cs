﻿using Artists.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artists.Domain.Objects
{
    public class Artist : Entity, IAggregateRoot
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
    }
}
