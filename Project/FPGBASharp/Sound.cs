using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace gbemu
{
    public static class Sound
    {
        public const int SAMPLERATE = 16780000 / 44100;

        private static bool sound_on;

        private static IWavePlayer player;
        public static SoundGenerator soundGenerator;

        public static void reset()
        {
            soundGenerator = new SoundGenerator();
            sound_on = false;

            var waveOutEvent = new WaveOutEvent();
            waveOutEvent.NumberOfBuffers = 2;
            waveOutEvent.DesiredLatency = 100;
            player = waveOutEvent;
            player.Init(new SampleToWaveProvider(soundGenerator));
            player.Play();
        }

        public static void set_soundreg(UInt32 adr)
        {
            // channel 1
            if (adr == GBRegs.Sect_sound.SOUND1CNT_L.address)     
            {
                soundGenerator.soundchannels[0].Number_of_sweep_shift = (byte)GBRegs.Sect_sound.SOUND1CNT_L_Number_of_sweep_shift.read();
                soundGenerator.soundchannels[0].Sweep_Frequency_Direction = GBRegs.Sect_sound.SOUND1CNT_L_Sweep_Frequency_Direction.on();
                soundGenerator.soundchannels[0].Sweep_Time = (byte)GBRegs.Sect_sound.SOUND1CNT_L_Sweep_Time.read();
                soundGenerator.soundchannels[0].setNewSweep(); 
                return; 
            }
            if (adr == GBRegs.Sect_sound.SOUND1CNT_H.address + 2) 
            {
                soundGenerator.soundchannels[0].Sound_length = (byte)GBRegs.Sect_sound.SOUND1CNT_H_Sound_length.read();
                soundGenerator.soundchannels[0].Wave_Pattern_Duty = (byte)GBRegs.Sect_sound.SOUND1CNT_H_Wave_Pattern_Duty.read();
                soundGenerator.soundchannels[0].Envelope_Step_Time = (byte)GBRegs.Sect_sound.SOUND1CNT_H_Envelope_Step_Time.read();
                soundGenerator.soundchannels[0].Envelope_Direction = GBRegs.Sect_sound.SOUND1CNT_H_Envelope_Direction.on();
                soundGenerator.soundchannels[0].Initial_Volume_of_envelope = (byte)GBRegs.Sect_sound.SOUND1CNT_H_Initial_Volume_of_envelope.read();
                soundGenerator.soundchannels[0].setNewEnvelope();
                soundGenerator.soundchannels[0].setNewLength();
                return; 
            }
            if (adr == GBRegs.Sect_sound.SOUND1CNT_X.address)     
            {
                soundGenerator.soundchannels[0].Frequency = (Int32)GBRegs.Sect_sound.SOUND1CNT_X_Frequency.read();
                soundGenerator.soundchannels[0].Length_Flag = GBRegs.Sect_sound.SOUND1CNT_X_Length_Flag.on();
                soundGenerator.soundchannels[0].Initial = GBRegs.Sect_sound.SOUND1CNT_X_Initial.on();
                soundGenerator.soundchannels[0].setNewFreq(); 
                return; 
            }
            if (adr == GBRegs.Sect_sound.SOUND1CNT_XHighZero.address + 2)
            {
                GBRegs.Sect_sound.SOUND1CNT_XHighZero.write(0);
            }

            // channel 2
            if (adr == GBRegs.Sect_sound.SOUND2CNT_L.address)
            {
                soundGenerator.soundchannels[1].Sound_length = (byte)GBRegs.Sect_sound.SOUND2CNT_L_Sound_length.read();
                soundGenerator.soundchannels[1].Wave_Pattern_Duty = (byte)GBRegs.Sect_sound.SOUND2CNT_L_Wave_Pattern_Duty.read();
                soundGenerator.soundchannels[1].Envelope_Step_Time = (byte)GBRegs.Sect_sound.SOUND2CNT_L_Envelope_Step_Time.read();
                soundGenerator.soundchannels[1].Envelope_Direction = GBRegs.Sect_sound.SOUND2CNT_L_Envelope_Direction.on();
                soundGenerator.soundchannels[1].Initial_Volume_of_envelope = (byte)GBRegs.Sect_sound.SOUND2CNT_L_Initial_Volume_of_envelope.read();
                soundGenerator.soundchannels[1].setNewEnvelope();
                soundGenerator.soundchannels[1].setNewLength();
                return;
            }
            if (adr == GBRegs.Sect_sound.SOUND2CNT_H.address)
            {
                soundGenerator.soundchannels[1].Frequency = (Int32)GBRegs.Sect_sound.SOUND2CNT_H_Frequency.read();
                soundGenerator.soundchannels[1].Length_Flag = GBRegs.Sect_sound.SOUND2CNT_H_Length_Flag.on();
                soundGenerator.soundchannels[1].Initial = GBRegs.Sect_sound.SOUND2CNT_H_Initial.on();
                soundGenerator.soundchannels[1].setNewFreq();
                return;
            }
            if (adr == GBRegs.Sect_sound.SOUND2CNT_HHighZero.address + 2)
            {
                GBRegs.Sect_sound.SOUND2CNT_HHighZero.write(0);
            }

            // channel 3
            if (adr == GBRegs.Sect_sound.SOUND3CNT_L.address)
            {
                soundGenerator.soundchannels[2].waveenable = GBRegs.Sect_sound.SOUND3CNT_L_Sound_Channel_3_Off.on();
                soundGenerator.soundchannels[2].wavebanks = GBRegs.Sect_sound.SOUND3CNT_L_Wave_RAM_Dimension.on();
                soundGenerator.soundchannels[2].banknumber = GBRegs.Sect_sound.SOUND3CNT_L_Wave_RAM_Bank_Number.on();
                return;
            }
            if (adr == GBRegs.Sect_sound.SOUND3CNT_H.address + 2)
            {
                soundGenerator.soundchannels[2].Sound_length = (byte)GBRegs.Sect_sound.SOUND3CNT_H_Sound_length.read();
                byte volume = (byte)GBRegs.Sect_sound.SOUND3CNT_H_Sound_Volume.read();
                if (GBRegs.Sect_sound.SOUND3CNT_H_Force_Volume.on())
                {
                    soundGenerator.soundchannels[2].volume = (float)(0.25 * 0.75);
                }
                else
                {
                    switch (volume)
                    {
                        case 0: soundGenerator.soundchannels[2].volume = 0; break;
                        case 1: soundGenerator.soundchannels[2].volume = (float)(0.25); break;
                        case 2: soundGenerator.soundchannels[2].volume = (float)(0.25 * 0.5); break;
                        case 3: soundGenerator.soundchannels[2].volume = (float)(0.25 * 0.25); break;
                    }
                }
                soundGenerator.soundchannels[2].setNewLength();
                return;
            }
            if (adr == GBRegs.Sect_sound.SOUND3CNT_X.address)
            {
                soundGenerator.soundchannels[2].Frequency = (Int32)GBRegs.Sect_sound.SOUND3CNT_X_Sample_Rate.read();
                soundGenerator.soundchannels[2].Length_Flag = GBRegs.Sect_sound.SOUND3CNT_X_Length_Flag.on();
                soundGenerator.soundchannels[2].Initial = GBRegs.Sect_sound.SOUND3CNT_X_Initial.on();
                soundGenerator.soundchannels[2].setNewFreq();
                return;
            }

            if (adr >= GBRegs.Sect_sound.WAVE_RAM.address && adr < GBRegs.Sect_sound.WAVE_RAM.address + GBRegs.Sect_sound.WAVE_RAM.count * 4) 
            { 
                soundGenerator.soundchannels[2].setWaveTable(); 
                return; 
            }

            if (adr == GBRegs.Sect_sound.SOUND3CNT_XHighZero.address + 2)
            {
                GBRegs.Sect_sound.SOUND3CNT_XHighZero.write(0);
            }

            //channel 4
            if (adr == GBRegs.Sect_sound.SOUND4CNT_L.address)
            {
                soundGenerator.soundchannels[3].Sound_length = (byte)GBRegs.Sect_sound.SOUND4CNT_L_Sound_length.read();
                soundGenerator.soundchannels[3].Envelope_Step_Time = (byte)GBRegs.Sect_sound.SOUND4CNT_L_Envelope_Step_Time.read();
                soundGenerator.soundchannels[3].Envelope_Direction = GBRegs.Sect_sound.SOUND4CNT_L_Envelope_Direction.on();
                soundGenerator.soundchannels[3].Initial_Volume_of_envelope = (byte)GBRegs.Sect_sound.SOUND4CNT_L_Initial_Volume_of_envelope.read();
                soundGenerator.soundchannels[3].setNewEnvelope();
                soundGenerator.soundchannels[3].setNewLength();
                return;
            }
            if (adr == GBRegs.Sect_sound.SOUND4CNT_H.address)
            {
                soundGenerator.soundchannels[3].lfsr7bit = GBRegs.Sect_sound.SOUND4CNT_H_Counter_Step_Width.on();
                byte div_raw = (byte)GBRegs.Sect_sound.SOUND4CNT_H_Dividing_Ratio_of_Freq.read();
                byte shift_raw = (byte)GBRegs.Sect_sound.SOUND4CNT_H_Shift_Clock_Frequency.read();
                soundGenerator.soundchannels[3].setNewPoly(div_raw, shift_raw);
                soundGenerator.soundchannels[3].Length_Flag = GBRegs.Sect_sound.SOUND4CNT_H_Length_Flag.on();
                soundGenerator.soundchannels[3].Initial = GBRegs.Sect_sound.SOUND4CNT_H_Initial.on();
                soundGenerator.soundchannels[3].setNewInitial();
                return;
            }
            if (adr == GBRegs.Sect_sound.SOUND4CNT_LHighZero.address + 2)
            {
                GBRegs.Sect_sound.SOUND4CNT_LHighZero.write(0);
            }
            if (adr == GBRegs.Sect_sound.SOUND4CNT_HHighZero.address + 2)
            {
                GBRegs.Sect_sound.SOUND4CNT_HHighZero.write(0);
            }

            // common
            if (adr == GBRegs.Sect_sound.SOUNDCNT_L.address) 
            {
                soundGenerator.volume_left_1_4 = GBRegs.Sect_sound.SOUNDCNT_L_Sound_1_4_Master_Volume_LEFT.read();
                soundGenerator.volume_right_1_4 = GBRegs.Sect_sound.SOUNDCNT_L_Sound_1_4_Master_Volume_RIGHT.read();

                soundGenerator.enable_channels_left[0] = GBRegs.Sect_sound.SOUNDCNT_L_Sound_1_Enable_Flags_LEFT.on();
                soundGenerator.enable_channels_left[1] = GBRegs.Sect_sound.SOUNDCNT_L_Sound_2_Enable_Flags_LEFT.on();
                soundGenerator.enable_channels_left[2] = GBRegs.Sect_sound.SOUNDCNT_L_Sound_3_Enable_Flags_LEFT.on();
                soundGenerator.enable_channels_left[3] = GBRegs.Sect_sound.SOUNDCNT_L_Sound_4_Enable_Flags_LEFT.on();

                soundGenerator.enable_channels_right[0] = GBRegs.Sect_sound.SOUNDCNT_L_Sound_1_Enable_Flags_RIGHT.on();
                soundGenerator.enable_channels_right[1] = GBRegs.Sect_sound.SOUNDCNT_L_Sound_2_Enable_Flags_RIGHT.on();
                soundGenerator.enable_channels_right[2] = GBRegs.Sect_sound.SOUNDCNT_L_Sound_3_Enable_Flags_RIGHT.on();
                soundGenerator.enable_channels_right[3] = GBRegs.Sect_sound.SOUNDCNT_L_Sound_4_Enable_Flags_RIGHT.on();
            }
            

            if (adr == GBRegs.Sect_sound.SOUNDCNT_H.address + 2) { SoundDMA.write_SOUNDCNT_H(); return; }

            if (adr == GBRegs.Sect_sound.SOUNDCNT_X.address) 
            { 
                sound_on = GBRegs.Sect_sound.SOUNDCNT_X_PSG_FIFO_Master_Enable.on(); 
                if (!sound_on)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        soundGenerator.soundchannels[i].on = false;
                    }
                }
                return;
            }

            if (adr == GBRegs.Sect_sound.SOUNDCNT_XHighZero.address + 2)
            {
                GBRegs.Sect_sound.SOUNDCNT_XHighZero.write(0);
            }
            if (adr == GBRegs.Sect_sound.SOUNDBIAS_HighZero.address + 2)
            {
                GBRegs.Sect_sound.SOUNDBIAS_HighZero.write(0);
            }

            //throw new Exception("Unknown Sound address: " + address.ToString("X4"));
        }

        public static void work()
        {
            if (sound_on)
            {
                soundGenerator.work();
            }
        }
    }
}
