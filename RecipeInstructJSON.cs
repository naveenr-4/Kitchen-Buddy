using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KitchenBuddy
{
    class RecipeInstructJSON
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("steps")]
        public List<Step> steps { get; set; }
    }
    public class Step
    {
        [JsonProperty("number")]
        public int number { get; set; }
        [JsonProperty("step")]
        public string step { get; set; }
        [JsonProperty("ingredients")]
        public List<Ingredient> ingredients { get; set; }
        [JsonProperty("equipment")]
        public List<Equipment> equipment { get; set; }
    }
    public class Ingredient
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("image")]
        public string image { get; set; }
    }
    public class Equipment
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("image")]
        public string image { get; set; }

    }

    //public class Ingredient
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string image { get; set; }
    //}

    //public class Step
    //{
    //    public int number { get; set; }
    //    public string step { get; set; }
    //    public List<Ingredient> ingredients { get; set; }
    //    public List<object> equipment { get; set; }
    //}

    //public class RootObject
    //{
    //    public string name { get; set; }
    //    public List<Step> steps { get; set; }
    //}



}
