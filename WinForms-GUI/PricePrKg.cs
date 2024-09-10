using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WinformsGUI
{
    public class PricePrKg
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("KgPrice")]
        public string pricePrKg { get; set; }

    }
}
