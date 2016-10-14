using Newtonsoft.Json;

namespace Infrastructure.UmuApi
{
    public class ItsPerson
    {
        [JsonProperty("Firstname")]
        public string FirstName { get; set; }

        [JsonProperty("Lastname")]
        public string LastName { get; set; }

        [JsonProperty("Nickname")]
        public string CasId { get; set; }
    }
}