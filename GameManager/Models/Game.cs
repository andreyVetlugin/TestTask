using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesManager.Models
{
    public class Game
    {
        //[JsonIgnore]
        public int Id { get; set; }


        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        public List<GameGenre> GameGenres { get; set; }

        public Publisher Publisher { get; set; }

        public void Update(Game otherGame)
        {
            Title = otherGame.Title;                // как-то через рефлексию ?
            ReleaseDate = otherGame.ReleaseDate;
            GameGenres = otherGame.GameGenres;
            Publisher = otherGame.Publisher;
        }
    }
}
