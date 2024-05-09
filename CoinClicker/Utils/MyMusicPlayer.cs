using NAudio.Wave;
using System;
using System.IO;

namespace CoinClicker
{
    public class MyMusicPlayer
    {

        private List<string> musicList;
        private int currentIndex;
        private string musicFolder;
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;

        public MyMusicPlayer(string musicFolder)
        {
            this.musicFolder = musicFolder;
            musicList = LoadMusic();
            currentIndex = 0;
            waveOutDevice = new WaveOutEvent();
            audioFileReader = new AudioFileReader(musicList[currentIndex]);
        }

        private List<string> LoadMusic()
        {
            List<string> musicList = new List<string>();
            string[] files = Directory.GetFiles(musicFolder, "*.mp3");
            musicList.AddRange(files);
            return musicList;
        }

        public void Play()
        {
            if (musicList.Count == 0)
            {
                return;
            }

            waveOutDevice.Init(audioFileReader);
            waveOutDevice.PlaybackStopped += WaveOutDevice_PlaybackStopped;

            waveOutDevice.Play();
        }

        public void SetVolume(float value) {
            waveOutDevice.Volume = value;
        }

        private void WaveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            currentIndex = (currentIndex + 1) % musicList.Count;
            audioFileReader = new AudioFileReader(musicList[currentIndex]);
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }
    }
}
