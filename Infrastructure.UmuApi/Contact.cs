using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infrastructure.UmuApi
{
    public class Contact
    {
        [JsonProperty("Contact")]
        public string EMail { get; set; }

      
    }
}
