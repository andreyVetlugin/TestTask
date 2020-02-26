using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class GameGenre
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }

        [JsonIgnore]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
