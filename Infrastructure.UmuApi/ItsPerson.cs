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
        public string CasId { get; set; }

        public bool IsActive { get; set; }

    }

    //public class CustomerData
    //{
    //    [JsonProperty("CasId")]
    //    public string CasId { get; set; }
    //    [JsonProperty("Name")]
    //    public string Name { get; set; }
    //    [JsonProperty("Address")]
    //    public string Address { get; set; }
    //    [JsonProperty("Telephone")]
    //    public string Telephone { get; set; }

    //    public bool IsActive { get; set; }

    //}


}