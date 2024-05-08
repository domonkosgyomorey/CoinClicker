using Accord.Math;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoinClicker.Utils
{
    public class RealTimeMusicAnalyzer
    {
        public event EventHandler<double> DominantFrequencyChanged;

        private WaveInEvent waveIn;
        private Complex[] fftBuffer;
        private int bufferSize = 1024;
        private int sampleRate = 44100;
        private double[] spectrum;
        private double[] previousSpectrum;

        public delegate void BeatDetected();
        public event BeatDetected OnBeatDetected;

        public RealTimeMusicAnalyzer()
        {
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(sampleRate, 1);
            waveIn.DataAvailable += WaveIn_DataAvailable;

            fftBuffer = new Complex[bufferSize];
            spectrum = new double[bufferSize / 2];

            waveIn.StartRecording();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            for (int i = 0; i < Math.Min(e.BytesRecorded / 2, fftBuffer.Length); i++)
            {
                short sample = BitConverter.ToInt16(e.Buffer, i * 2);
                fftBuffer[i] = new Complex(sample, 0);
            }

            FourierTransform.FFT(fftBuffer, FourierTransform.Direction.Forward);

            for (int i = 0; i < fftBuffer.Length / 2; i++)
            {
                spectrum[i] = fftBuffer[i].Magnitude;
            }

            double dominantFrequency = GetDominantFrequency();
            OnDominantFrequencyChanged(dominantFrequency);

            bool rhythmDetected = IsRhythmDetected(spectrum);

            if (previousSpectrum == null)
            {
                previousSpectrum = new double[spectrum.Length];
                Array.Copy(spectrum, previousSpectrum, spectrum.Length);
            }
            else
            {
                Array.Copy(spectrum, previousSpectrum, spectrum.Length);
            }

            if(rhythmDetected)
                OnBeatDetected?.Invoke();
        }

        private double CalculateThreshold(double[] spectrum)
        {
            double sum = 0;

            for (int i = 0; i < spectrum.Length / 8; i++)
            {
                sum += spectrum[i];
            }

            double average = sum / (spectrum.Length / 8);
            return average * 1.5;
        }

        private bool IsRhythmDetected(double[] spectrum)
        {
            double sum = 0;

            for (int i = 0; i < spectrum.Length / 8; i++)
            {
                sum += spectrum[i];
            }

            return sum > CalculateThreshold(spectrum);
        }

        private double GetDominantFrequency()
        {
            double maxMagnitude = 0;
            int maxIndex = 0;

            for (int i = 0; i < spectrum.Length; i++)
            {
                if (spectrum[i] > maxMagnitude)
                {
                    maxMagnitude = spectrum[i];
                    maxIndex = i;
                }
            }

            double dominantFrequency = (double)maxIndex * sampleRate / bufferSize;

            return dominantFrequency;
        }

        protected virtual void OnDominantFrequencyChanged(double frequency)
        {
            DominantFrequencyChanged?.Invoke(this, frequency);
        }

        public void Stop()
        {
            waveIn.StopRecording();
            waveIn.Dispose();
        }
    }
}
