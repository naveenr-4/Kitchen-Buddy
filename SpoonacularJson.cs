using Newtonsoft.Json;

namespace KitchenBuddy
{
    /* Class: IngredientsJson
       Usage: Class used for deserializing the JSON response from spoonacular APIs*/
    class IngredientsJson
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("title")]
        public string title { get; set; }
        [JsonProperty("image")]
        public string image { get; set; }
        [JsonProperty("usedIngredientCount")]
        public int usedIngredientCount { get; set; }
        [JsonProperty("missedIngredientCount")]
        public int missedIngredientCount { get; set; }
        [JsonProperty("likes")]
        public int likes { get; set; }
    }
}


