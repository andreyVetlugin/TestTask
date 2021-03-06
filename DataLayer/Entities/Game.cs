﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Entities
{
    public class Game : IDbEntity
    {
        //[JsonIgnore]
        public Guid Id { get; set; }

        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        public List<GameGenre> GameGenres { get; set; }

        public Guid PublisherID { get; set; }
        public Publisher Publisher { get; set; }

    }
}
