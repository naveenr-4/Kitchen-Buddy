using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenBuddy
{
    class Dialogues
    {
        //Greetings
        public static String Greet1= "Hello I am Gordon. I will be your cooking buddy today. Please tell me what all ingredients you have at home. Also, state more than one ingredient. You can either list the ingredients at once or tell them one by one.";
        //Ingredients
        public static String moreIngredient = "Do you want to add more ingredients to your list? If yes, name them.";
        public static String nextIngredient0 = "Got that. What's the next ingredient? Say no if you don't have any more ingredients.";
        public static String nextIngredient1 = "What's the next ingredient? Say no if you don't have any.";
        public static String nextIngredient2 = "Okay. Next ingredient?";
        public static String nextIngredient3 = "Alright. Next?";
        public static String Ingredients = "The ingredients you have are ";
        public static String ChangeIngredient = "Do you want to change the ingredients?";
        public static String relistIngredient = "Please list your ingredients again.";
        //Recipe
        public static String recipeHeader = ". Please say the recipe number from the list below to get the recipe instructions.";
        public static String recipeSelection = "What recipe's instructions would you like to look at now?";
        public static String recipeInstructions = "So, these are the step by step instructions for the recipe ";
        public static String recipeNoSelection = "Please say the recipe number for the instructions.";
        //Others
        public static String defaultText = "I am sorry. I do not recognize that.";
        public static String defaultText2 =  "Sorry. I am having trouble understanding that.";
        public static String defaultText3 = "This is embarrassing . I am really sorry. Please try again.";
        public static String defaultText4 = "Can you please repeat that?";
        public static String defaultRecipe = "I did not get that. You can say first or second or one, two.";
        //Survey
        public static String UserSurveyPermission = "Hey. Are you interested in a short survey about me?";
        //Grounding
        public static String explicitGrounding = "I did not get that. Did you say ";
    }
}
