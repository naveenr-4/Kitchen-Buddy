using GoogleCloudSamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace KitchenBuddy
{


    


    /* Class: StartHere
       Usage: Calls all the other functions*/
    class StartHere
    {
        static Boolean bIngredients = false, bRecipes = false;
        static List<String> lstIngredients;
        static String[] lstRecipes;
        static int nGroundingCt = 0;
        static String strUtterance;
        static Boolean bExplicitGround = false;
        static String[] strRecipeList = new String[5];
        static int nCounterDefault = 0;
        static List<int> lstRecipeID = new List<int>();
        String ASRFile = @"C:\test files\ASR test files\\Test01.txt";
        /* Method: getRecipes
         * Usage: Parse the ingredients from the JSON response and call Spoonacular.RecipeNames to get the recipe names
         * Parameters: JSON response */



        public String[] getRecipeNames(string strLUISIngredients)
        {
            try
            {
                string Recipes;
                Spoonacular objSpoon = new Spoonacular();
                List<IngredientsJson> lstRecipes = objSpoon.getRecipes(strLUISIngredients);
                int nArrayLen = lstRecipes.Count == 0 ? 1 : lstRecipes.Count;
                String[] strRecipeList = new String[nArrayLen];
                Recipes = Dialogues.recipeHeader + lstRecipes[0].title.ToString();
                strRecipeList[0] = lstRecipes[0].title.ToString();
                for (int i = 1; i < lstRecipes.Count; i++)
                {
                    strRecipeList[i] = lstRecipes[i].title.ToString();
                }
                for (int i = 0; i < lstRecipes.Count; i++)
                {
                    lstRecipeID.Add(lstRecipes[i].id);
                }
                return strRecipeList;
            }
            catch (Exception e)
            {
                String strTextToWrite = DateTime.Now.ToString() + "StartHere.getRecipeNames Exception: " + e.Message;
                File.AppendAllText(ASRFile, strTextToWrite + Environment.NewLine);
                return null;
            }
        }
        /* Method: callTTS
         * Usage: To call TTS
         * Parameters: Text to be spoken */
        public void callTTS(String strText)
        {
            try
            {
                TTS objTTS = new TTS();
                objTTS.TextToSpeech(strText);
            }
            catch (Exception e)
            {
                String strTextToWrite = DateTime.Now.ToString() + "StartHere.callTTS Exception: " + e.Message;
                File.AppendAllText(ASRFile, strTextToWrite + Environment.NewLine);
            }
        }
        /* Method: CallLUIS
         * Usage: To call LUIS NLU to get the intent from the output text received from ASR
         * Parameters: ASRoutput */
        public LuisJson CallLUIS(string ASROutput)
        {
            //ASR call which gives a string ;
            try
            {
                Luis LuisObject = new Luis();
                LuisJson objResponse = LuisObject.getLUISResponse(ASROutput);
                return objResponse;
            }
            catch (Exception e)
            {
                String strTextToWrite = DateTime.Now.ToString() + "StartHere.CallLUIS Exception: " + e.Message;
                File.AppendAllText(ASRFile, strTextToWrite + Environment.NewLine);
                return null;
            }
        }
            
        public String[] Process(String[] strAudioFile)
        {
            String[] lstResponse;
            String[] strTranscribedText;
            try
			{
                strTranscribedText = TranscribeAsync.Main(strAudioFile);
                if (strTranscribedText == null)
                {
                    lstResponse = new String[1];
                    lstResponse[0] = Dialogues.defaultText;
                    //callTTS(Dialogues.defaultText);
                    goto end;
                }
                strTranscribedText[0] = String.Join(".", strTranscribedText);
                String s = strTranscribedText[0].TrimEnd('.');
                strTranscribedText[0] = strTranscribedText[0].TrimEnd('.');
                String strTextToWrite = DateTime.Now.ToString() + " " + strTranscribedText[0];
                File.AppendAllText(ASRFile, strTextToWrite + Environment.NewLine);
                /*redoLUIS:*/
                LuisJson objResponse = CallLUIS(strTranscribedText[0]);
                int nArraySize = objResponse.entities.Count == 0 ? 1 : objResponse.entities.Count;
                lstResponse = new String[nArraySize];
                if (objResponse.topScoringIntent.score < 0.5)
                {
                    strUtterance = strTranscribedText[0];
                    //lstResponse[0] = Dialogues.explicitGrounding + strUtterance + "?";
                    lstResponse[0] = getDefaultText();
                    bExplicitGround = true;
                    if (bRecipes)
                    {
                        callTTS(Dialogues.defaultRecipe);
                        lstResponse = lstRecipes;
                    }
                    goto end;
                }
                else if (objResponse.entities.Count == 0 && objResponse.topScoringIntent.intent == "listofIngredients")
                {
                    strUtterance = strTranscribedText[0];
                    //lstResponse[0] = Dialogues.explicitGrounding + strUtterance + "?";
                    lstResponse[0] = getDefaultText();
                    bExplicitGround = true;
                    if (bRecipes)
                    {
                        lstResponse = lstRecipes;
                        callTTS(Dialogues.defaultRecipe);
                    }
                    goto end;
                }
                //Below block calls the TTS with the input based on the intent
                switch (objResponse.topScoringIntent.intent)
                {
                    case "GreetingS":
                    case "Restart":
                        //callTTS(Dialogues.Greet1);
                        lstResponse[0] = Dialogues.Greet1;
                        break;
                    case "Repeat":
                    case "listofIngredients":
                        #region ingredient
                        bIngredients = true;
                        if (lstIngredients == null)
                            lstIngredients = new List<string>();
                        if (objResponse.entities.Count > 1)
                        {
                            for (int i = 0; i < objResponse.entities.Count; i++)
                            {

                                if (objResponse.entities[i].type == "Ingredients")
                                    lstIngredients.Add(objResponse.entities[i].entity);
                            }
                            lstResponse[0] = "You have " + String.Join(", ", lstIngredients) + ". " + Dialogues.moreIngredient;
                            //callTTS(Dialogues.moreIngredient);
                        }
                        else
                        {
                            lstIngredients.Add(objResponse.entities[0].entity);
                            switch (nGroundingCt)
                            {
                                case 0:
                                    lstResponse[0] = Dialogues.nextIngredient0;
                                    //callTTS(Dialogues.nextIngredient0);
                                    nGroundingCt++;
                                    break;
                                case 1:
                                    lstResponse[0] = Dialogues.nextIngredient1;
                                    //callTTS(Dialogues.nextIngredient1);
                                    nGroundingCt++;
                                    break;
                                case 2:
                                    lstResponse[0] = Dialogues.nextIngredient2;
                                    //callTTS(Dialogues.nextIngredient2);
                                    nGroundingCt++;
                                    break;
                                case 3:
                                    lstResponse[0] = Dialogues.nextIngredient3;
                                    //callTTS(Dialogues.nextIngredient3);
                                    break;
                                default:
                                    lstResponse[0] = Dialogues.nextIngredient3;
                                    nGroundingCt++;
                                    nGroundingCt = 0;
                                    goto end;
                            }
                            // lstResponse
                            //lstResponse = getRecipeNames(lstIngredients);
                        }
                        break;
                    #endregion
                    case "negativeAck":
                        String strIngredientList = "";
                        if (bIngredients)
                        {
                            strIngredientList = String.Join(",", lstIngredients.ToArray());
                            strIngredientList = Dialogues.Ingredients + " " + strIngredientList;
                            strRecipeList = getRecipeNames(strIngredientList);
                            if (lstRecipes == null)
                                lstRecipes = new String[strRecipeList.Count() + 1];
                            lstResponse = new String[strRecipeList.Count() + 1];
                            lstResponse[0] = strIngredientList + " " + Dialogues.recipeHeader;
                            for (int i = 0; i < strRecipeList.Count(); i++)
                            {
                                lstResponse[i + 1] = (i + 1).ToString() + ". " + strRecipeList[i];
                            }
                            lstRecipes = lstResponse;
                            bRecipes = true;
                        }
                        else if (bExplicitGround)
                        {

                        }
                        break;
                    case "RecipeNumber":
                        #region recipeNum
                        lstIngredients = new List<string>();
                        if (objResponse.topScoringIntent.score > 0.5)
                        {
                            String strRecipeNum = objResponse.entities[0].entity;
                            Spoonacular objSpoon = new Spoonacular();
                            String[] lstSteps;
                            switch (strRecipeNum)
                            {
                                case "first":
                                case "one":
                                case "1st":
                                case "1":
                                    lstSteps = objSpoon.getInstructions(lstRecipeID[0], strRecipeList[0]);
                                    lstResponse = lstSteps;
                                    //callTTS(Dialogues.recipeInstructions);
                                    break;
                                case "second":
                                case "two":
                                case "2nd":
                                case "2":
                                    lstSteps = objSpoon.getInstructions(lstRecipeID[1], strRecipeList[1]);
                                    lstResponse = lstSteps;
                                    //callTTS(Dialogues.recipeInstructions);
                                    break;
                                case "third":
                                case "three":
                                case "3rd":
                                case "3":
                                    lstSteps = objSpoon.getInstructions(lstRecipeID[2], strRecipeList[2]);
                                    lstResponse = lstSteps;
                                    //callTTS(Dialogues.recipeInstructions);
                                    break;
                                case "fourth":
                                case "four":
                                case "4th":
                                case "4":
                                    lstSteps = objSpoon.getInstructions(lstRecipeID[3], strRecipeList[3]);
                                    lstResponse = lstSteps;
                                    //callTTS(Dialogues.recipeInstructions);
                                    break;
                                case "fifth":
                                case "five":
                                case "5th":
                                case "5":
                                case "last":
                                    lstSteps = objSpoon.getInstructions(lstRecipeID[4], strRecipeList[4]);
                                    lstResponse = lstSteps;
                                    //callTTS(Dialogues.recipeInstructions);
                                    break;
                            }
                            bRecipes = false;
                        }
                        else
                        {
                            lstResponse[0] = getDefaultText();
                        }
                        lstIngredients = new List<string>();
                        strRecipeList = new String[5];
                        break;
                    #endregion
                    //case "positiveAck":
                    //    if (bExplicitGround)
                    //    {
                    //        strTranscribedText[0] = strUtterance;
                    //        strUtterance = "";
                    //        bExplicitGround = false;
                    //        goto redoLUIS;
                    //    }
                    //    break;
                    default:
                        lstResponse[0] = getDefaultText();
                        if (bRecipes)
                        {
                            lstResponse = lstRecipes;
                            callTTS(Dialogues.defaultRecipe);
                        }
                        break;
                }
            end: return lstResponse;
            }
            catch (Exception e)
            {
                lstResponse = new String[1];
                lstResponse[0] = Dialogues.defaultText.ToString();
                String strTextToWrite = DateTime.Now.ToString() + "StartHere.Process Exception: " + e.Message;
                File.AppendAllText(ASRFile, strTextToWrite + Environment.NewLine);
                return lstResponse;
            }
        }
        String getDefaultText()
        {
            String retText;
            if (nCounterDefault % 3 == 0)
            {
                retText = Dialogues.defaultText;
            }
            else if (nCounterDefault % 3 == 1)
            {
                retText = Dialogues.defaultText2;
            }
            else
            {
                retText = Dialogues.defaultText3;
            }
            nCounterDefault++;
            return retText;
        }
    }
}
