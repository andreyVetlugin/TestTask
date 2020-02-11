using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesManager.Models
{
    public class Genre
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string GenreTitle { get; set; }

        [JsonIgnore]
        public List<GameGenre> GameGenres { get; set; }
    }
}
