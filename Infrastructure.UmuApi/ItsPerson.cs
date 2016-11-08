using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Infrastructure.UmuApi
{
    public class ItsPerson
    {
        [JsonProperty("Firstname")]
        public string FirstName { get; set; }

        [JsonProperty("Lastname")]
        public string LastName { get; set; }

        [JsonProperty("NickName")]
        public string CasId { get; set;}

        [JsonProperty("Guises")]
        public List<Guise> Guise { get; set; }

      
    }

}
