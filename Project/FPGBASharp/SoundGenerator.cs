using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace gbemu
{
    public class SoundGenerator : ISampleProvider
    {
        public SoundChannel[] soundchannels;

        public float Volume { get; set; }
        public WaveFormat WaveFormat { get; private set; }

        private List<float> nextSamples;

        public UInt32 volume_left_1_4;
        public UInt32 volume_right_1_4;

        public float volume_1_4;
        public float volume_dma0;
        public float volume_dma1;


        public bool[] enable_channels_left;
        public bool[] enable_channels_right;

        public SoundGenerator(int sampleRate = 44100)
        {
            soundchannels = new SoundChannel[4];
            for (int i = 0; i < 4; i++)
            {
                soundchannels[i] = new SoundChannel(i + 1);
            }

            enable_channels_left = new bool[4];
            enable_channels_right = new bool[4];

            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1);
            nextSamples = new List<float>();
        }

        public int Read(float[] buffer, int offset, int count)
        {
            lock (nextSamples)
            {
                for (int n = 0; n < count; ++n)
                {
                    if (nextSamples.Count > 0)
                    {
                        buffer[n + offset] = nextSamples[0];
                        nextSamples.RemoveAt(0);
                    }
                    else
                    {
                        buffer[n + offset] = 0;
                    }
                }
            }
            return count;
        }

        public void work()
        {
            SoundDMA.soundDMAs[0].work();
            SoundDMA.soundDMAs[1].work();

            lock (nextSamples)
            {
                if (nextSamples.Count < 5000)
                {
                    float value = 0;
                    for (int i = 0; i < 4; i++)
                    //for (int i = 0; i < 0; i++)
                    {
                        if (enable_channels_left[i] || enable_channels_right[i])
                        {
                            value += (soundchannels[i].get_next() * volume_1_4);
                        }
                    }
                    value /= 2;

                    value = value * (volume_left_1_4 + volume_right_1_4) / 14;

                    value += volume_dma0 * ((float)(SByte)SoundDMA.soundDMAs[0].outfifo[0] / 64);
                    if (SoundDMA.soundDMAs[0].outfifo.Count > 1)
                    {
                        SoundDMA.soundDMAs[0].outfifo.RemoveAt(0);
                    }
                    
                    value += volume_dma1 * ((float)(SByte)SoundDMA.soundDMAs[1].outfifo[0] / 64);
                    if (SoundDMA.soundDMAs[1].outfifo.Count > 1)
                    {
                        SoundDMA.soundDMAs[1].outfifo.RemoveAt(0);
                    }

                    nextSamples.Add(value);

                }
            }
        }



    }
}
