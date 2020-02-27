using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class GameGenre : IDbEntity
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }

        [JsonIgnore]
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
