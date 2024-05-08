using CoinClicker.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CoinClicker
{
    public class MyMusicPlayer
    {

        private IList<AudioFileReader> audioFiles;
        private WaveOutEvent outputDevice;
        private RealTimeMusicAnalyzer musicAnalyzer;

        public RealTimeMusicAnalyzer MusicAnalyzer { get => musicAnalyzer; set => musicAnalyzer = value; }

        public MyMusicPlayer(IList<string> links) {
            audioFiles = new List<AudioFileReader>();
            foreach (var link in links) { 
                audioFiles.Add(new AudioFileReader(link));
            }
            Init();
        }

        public MyMusicPlayer(string musicDirPath)
        {
            audioFiles = new List<AudioFileReader>();
            foreach (var link in Directory.GetFiles(musicDirPath))
            {
                audioFiles.Add(new AudioFileReader(link));
            }
            Init();
        }

        public void SetVolume(float value) { 
            outputDevice.Volume = value;
        }

        public void GetFreq() { 
        }

        private void Init() {

            //musicAnalyzer = new RealTimeMusicAnalyzer();
            //musicAnalyzer.DominantFrequencyChanged += MusicAnalyzer_DominantFrequencyChanged;

            outputDevice = new WaveOutEvent();
            if (audioFiles.Count > 0)
                outputDevice.Init(audioFiles.First());
            outputDevice.Play();
        }

        //private void MusicAnalyzer_DominantFrequencyChanged(object sender, double frequency)
        //{
        //    if (outputDevice != null)
        //    {
        //        outputDevice.Volume = frequency > 100 ? 0.5f * 0.5f : 0.5f;
        //    }
        //}
    }
}
