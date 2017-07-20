using System;
using System.IO;
using System.Speech.Synthesis;

namespace KitchenBuddy
{
    /* Class: TTS
       Usage: All TTS related functions will be written here*/
    class TTS
    {
        /* Method: TextToSpeech
         * Usage: Calls the Speech Synthesizer with the text to be spoken
         * Parameters: Text to be spoken */
        public void TextToSpeech(string strToSpeak)
        {
            try
            {
                // Set a value for the speaking rate.
                SpeechSynthesizer objSynth = new SpeechSynthesizer();
                //objSynth.SelectVoice("GB");
                objSynth.Rate = 0; //Set the rate at which the text should be spoken
                objSynth.SetOutputToDefaultAudioDevice();
                objSynth.Speak(strToSpeak);
            }
            catch (Exception e)
            {
                String ASRFile = @"C:\test files\ASR test files\\Test01.txt";
                String strTextToWrite = DateTime.Now.ToString() + "TTS.TextToSpeech Exception: " + e.Message;
                File.AppendAllText(ASRFile, strTextToWrite + Environment.NewLine);
            }
        }
    }
}
