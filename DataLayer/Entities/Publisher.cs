using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Publisher : IDbEntity
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Title { get; set; } 

        [JsonIgnore]
        public List<Game> Games{ get; set; }
    }
}