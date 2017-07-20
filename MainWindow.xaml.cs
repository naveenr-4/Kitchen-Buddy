
using GoogleCloudSamples;
using NAudio.Wave;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using System.Windows.Threading;

namespace KitchenBuddy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;
        static int buttonclicks = 0;
        String fileNameOnly = System.IO.Path.GetFileNameWithoutExtension(@"C:\\test files\\Test01.wav");
        String extension = System.IO.Path.GetExtension(@"C:\\test files\\Test01.wav");
        String path = System.IO.Path.GetDirectoryName(@"C:\\test files\\Test01.wav");
        String audioFile = @"C:\\test files\\Test01.wav";
        String tempFileName = "";
        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            buttonclicks++;
            if (buttonclicks % 2 == 1)
            {
                RecordButton.Opacity = 0.5;
                lblDisplay.Content = "Recognizing.. Press the button to stop.";
                waveSource = new WaveIn();
                waveSource.WaveFormat = new WaveFormat(16000, 1);

                waveSource.DataAvailable += WaveSource_DataAvailable;
                waveSource.RecordingStopped += WaveSource_RecordingStopped;
                int count = 1;
                while (File.Exists(audioFile))
                {
                    tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                    audioFile = System.IO.Path.Combine(path, tempFileName + extension);
                }
                //waveFile = new WaveFileWriter(@"D:\Test0001.wav", waveSource.WaveFormat);
                waveFile = new WaveFileWriter(audioFile, waveSource.WaveFormat);
                waveSource.StartRecording();
            }
            if (buttonclicks % 2 == 0)
            {
                RecordButton.Opacity = 1;
                lblDisplay.Content = "Press button to start Recognition";
                StopButton_Click(waveFile);

            }
        }
        private void WaveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
            //String[] args = { "d:\\Test0001.wav" };
            String[] args = { audioFile };
            String strTTS;
            ObservableCollection<String> items;
            StartHere objStart = new StartHere();
            WaveFileReader wf = new WaveFileReader(audioFile);
            TimeSpan duration = wf.TotalTime;
            if (duration.Minutes > 0)
            {
                items = new ObservableCollection<string>();
                items.Add(Dialogues.defaultText4);
                lbTodoList.ItemsSource = items;
                strTTS = Dialogues.defaultText4;
            }
            else
            {
                String[] lstLUISResponse = objStart.Process(args); //objStart.CallLUIS(s[0]);
                items = new ObservableCollection<string>(lstLUISResponse);
                strTTS = lstLUISResponse[0];
            }
            lbTodoList.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                lbTodoList.ItemsSource = items;
            }));
            lbTodoList.Refresh();
            objStart.callTTS(strTTS);
        }
        private void WaveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        private void StopButton_Click(object sender)
        {
            waveSource.StopRecording();
            RecordButton.Opacity = 1;
        }

    }
    public static class ExtensionMethods
    {

        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
