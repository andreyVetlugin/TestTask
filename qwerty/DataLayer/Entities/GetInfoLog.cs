using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Entities
{
    public class GetInfoLog: IBenefitsEntity
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }

        public DateTime Date { get; set; }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string HttpMethod { get; set; }
        public string RequestBody { get; set; }
        
    }
         


}
