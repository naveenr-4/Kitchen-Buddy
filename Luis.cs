using System;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace KitchenBuddy
{
    class Luis
    {
        /* Method: getLUISResponse
         * Usage: Calls LUIS NLU to get the intent from the output text received from ASR
         * Parameters: ASRoutput */
        public LuisJson getLUISResponse(String ASROutput)
        {
            try
            {
                string url = $"https://api.projectoxford.ai/luis/v2.0/apps/6d9801a6-0dda-4026-a5d4-34f05943c899?subscription-key=e0f547ffc7554e9dbb2de915064f482c&q={ASROutput}";
                var syncClient = new WebClient();
                var content = syncClient.DownloadString(url);
                String strResponse = content.ToString();
                LuisJson lstResponse = new LuisJson();
                lstResponse = JsonConvert.DeserializeObject<LuisJson>(strResponse) as LuisJson;
                return lstResponse;
            }
            catch (Exception e)
            {
                String ASRFile = @"C:\test files\ASR test files\\Test01.txt";
                String strTextToWrite = DateTime.Now.ToString() + "Luis.getLUISResponse Exception: " + e.Message;
                File.AppendAllText(ASRFile, strTextToWrite + Environment.NewLine);
                return null;
            }
        }
    }
}

