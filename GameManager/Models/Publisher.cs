using Newtonsoft.Json;
using System.Collections.Generic;

namespace GamesManager.Models
{
    //using Genre = System.String;
    public class Publisher
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; } 

        [JsonIgnore]
        public List<Game> Games{ get; set; }

        public void Update(Publisher otherPublisher)
        {
            Title = otherPublisher.Title;
            Games = otherPublisher.Games;
        }
    }
}