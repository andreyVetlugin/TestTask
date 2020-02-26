using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    //using Genre = System.String;
    public class Publisher
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; } 

        [JsonIgnore]
        public List<Game> Games{ get; set; }
    }
}