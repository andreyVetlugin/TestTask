using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Genre : IDbEntity
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string GenreTitle { get; set; }

        [JsonIgnore]
        public List<GameGenre> GameGenres { get; set; }
    }
}
