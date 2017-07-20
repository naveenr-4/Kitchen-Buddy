using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using unirest_net.http;
using System.Linq;
using System.IO;

namespace KitchenBuddy
{
    /* Class: Spoonacular
     * Usage: All Spoonacular API related calls are made here */
    class Spoonacular
    {
        /* Method: getRecipes
         * Usage: Gets the list of Recipes as a JSON response from Spoonacular. Deserializes it and passes it back to the calling function.
         * Parameters: List of Ingredients
         * Return: Returns the deserialized object*/
        String ASRFile = @"C:\test files\ASR test files\\Test01.txt";
        public List<IngredientsJson> getRecipes(String LUISIngredients)
        {
            try
            {
                string url = $"https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/findByIngredients?fillIngredients=true&ingredients={LUISIngredients}&limitLicense=true&number=5&ranking=2";
                HttpResponse<string> response = Unirest.get(url)
                .header("X-Mashape-Key", "waVWFXWmMEmshIIkFrNRfj8T1ii8p1AlRJrjsnkb9fZC0uN270")
                .header("Accept", "application/json")
                .asJson<string>();
                String strResponse = response.Body;
                List<IngredientsJson> lstRecipes = new List<IngredientsJson>();
                lstRecipes = JsonConvert.DeserializeObject<List<IngredientsJson>>(strResponse);
                return lstRecipes;
            }
            catch (Exception e)
            {
                String strTextToWrite = DateTime.Now.ToString() + "Spoonacular.getRecipes Exception: " + e.Message;
                File.AppendAllText(ASRFile, strTextToWrite + Environment.NewLine);
                return null;
            }
        }
        public String[] getInstructions(int recipeID, String strRecipeName)
        {
            try
            {
                string url = $"https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/{recipeID}/analyzedInstructions?stepBreakdown=true";
                HttpResponse<string> response = Unirest.get(url)
                .header("X-Mashape-Key", "I4Gh7MsEkJmshjKplIePxRXXw3Ump1uEReajsnOwO3FLB2mMsS")
                .header("Accept", "application/json")
                .asJson<string>();
                String strResponse = response.Body;
                List<RecipeInstructJSON> lstInstructions = new List<RecipeInstructJSON>();
                //List<String> lstSteps = new List<String>();

                lstInstructions = JsonConvert.DeserializeObject<List<RecipeInstructJSON>>(strResponse);
                String[] strSteps = new String[lstInstructions[0].steps.Count + 1];
                List<String> lstIngredients = new List<string>();
                //String strInstructions = "";
                for (int i = 2; i <= lstInstructions[0].steps.Count; i++)
                {
                    for (int k = 0; k < lstInstructions[0].steps[i - 2].ingredients.Count; k++)
                        lstIngredients.Add(lstInstructions[0].steps[i - 2].ingredients[k].name);

                    strSteps[i] = lstInstructions[0].steps[i - 2].step;
                    //strInstructions += lstInstructions[i].step + " ";
                }
                strSteps[0] = strRecipeName;
                lstIngredients = lstIngredients.Distinct(StringComparer.OrdinalIgnoreCase).ToList<String>();
                strSteps[1] = "The ingredients are " + String.Join(", ", lstIngredients) + ".";
                return strSteps;
            }
            catch (Exception e)
            {
                String strTextToWrite = DateTime.Now.ToString() + "Spoonacular.getInstructions Exception: " + e.Message;
                File.AppendAllText(ASRFile, strTextToWrite + Environment.NewLine);
                return null;
            }
        }
    }
}



