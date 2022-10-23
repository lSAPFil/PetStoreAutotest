using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace SpecFlowProject_PetStore
{
    public class PetsInfo
    {
       //[JsonProperty("id")]
        public long id { get; set; }

        //[JsonProperty("category")]
        public Category category { get; set; }

        //[JsonProperty("name")]
        public string name { get; set; }

        //[JsonProperty("photoUrls")]
        public List<string> photoUrls { get; set; }

        //[JsonProperty("tags")]
        public List<Category> tags { get; set; }

        //[JsonProperty("status")]
        public string status { get; set; }
    }

    public class Category
    {
        //[JsonProperty("id")]
        public long id { get; set; }

        //[JsonProperty("name")]
        public string name { get; set; }
    }
    
}
