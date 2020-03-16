using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public class SoundChannel
    {
        private int id;

        public byte Number_of_sweep_shift;
        public bool Sweep_Frequency_Direction;
        public byte Sweep_Time;

        public byte Sound_length;
        public byte Wave_Pattern_Duty;
        public byte Envelope_Step_Time;
        public bool Envelope_Direction;
        public byte Initial_Volume_of_envelope;

        public Int32 Frequency;
        public bool Length_Flag;
        public bool Initial;

        public bool waveenable;
        public bool wavebanks;
        public bool banknumber;
        private byte[] wavetable;
        private byte[] wavetable2;

        public float volume;
        public bool on;
        private int wavetable_ptr;

        // channel1-4
        private UInt32 freq_cnt;
        private UInt32 length_left;

        //channel1
        private UInt32 sweepcnt;

        // channel1,2,4
        private UInt32 envelope_cnt;
        private Int32 envelope_add;

        // channel 3

        // channel 4
        private UInt32 lfsr;
        private UInt32 ch4_freqdivider;
        public bool lfsr7bit;

        private UInt32 soundcycles_freq; 
        private UInt32 soundcycles_sweep; 
        private UInt32 soundcycles_envelope; 
        private UInt32 soundcycles_length; 

        public SoundChannel(int id)
        {
            this.id = id;
            volume = 0;
            on = false;
            soundcycles_freq = 0;
            soundcycles_sweep = 0;
            soundcycles_envelope = 0;
            sweepcnt = 0;
            freq_cnt = 0;

            if (id == 3)
            {
                wavetable = new byte[32];
                wavetable2 = new byte[32];
            }
            else if (id == 4)        
            {
                wavetable = new byte[1];
            }
            else
            {
                wavetable = new byte[8];
            }
            wavetable_ptr = 0;
        }

        public void setNewSweep()
        {
            sweepcnt = 0;
        }

        public void setNewLength()
        {
            if (id == 3)
            {
                length_left = (UInt32)(256 - Sound_length);
            }
            else
            {
                length_left = (UInt32)(64 - Sound_length);
            }
            recalc_freq();
        }

        public void setNewEnvelope()
        {
            envelope_cnt = 0;
            envelope_add = 0;
            if (id == 3)
            {
                volume = 0.25f;
            }
            else
            {
                volume = 0.25f * Initial_Volume_of_envelope / 15;
            }
        }

        public void setNewFreq()
        {
            recalc_freq();

            if (Initial)
            {
                sweepcnt = 0;
                envelope_cnt = 0;
                envelope_add = 0;
                on = true;
                freq_cnt = 0;
            }
        }

        public void setWaveTable()
        {
            for (int i = 0; i < GBRegs.Sect_sound.WAVE_RAM.count * 4; i++)
            {
                byte data = GBRegs.data[GBRegs.Sect_sound.WAVE_RAM.address + i];
                if (banknumber)
                {
                    this.wavetable[i * 2 + 0] = (byte)(data >> 4);
                    this.wavetable[i * 2 + 1] = (byte)(data & 0xF);
                }
                else
                {
                    this.wavetable2[i * 2 + 0] = (byte)(data >> 4);
                    this.wavetable2[i * 2 + 1] = (byte)(data & 0xF);
                }
            }
        }

        public void setNewPoly(byte divider_raw, byte shift_raw)
        {
            switch (divider_raw)
            {
                case 0: ch4_freqdivider = 8; break;
                case 1: ch4_freqdivider = 16; break;
                case 2: ch4_freqdivider = 32; break;
                case 3: ch4_freqdivider = 48; break;
                case 4: ch4_freqdivider = 64; break;
                case 5: ch4_freqdivider = 80; break;
                case 6: ch4_freqdivider = 96; break;
                case 7: ch4_freqdivider = 112; break;
            }

            ch4_freqdivider = ch4_freqdivider << shift_raw;
            ch4_freqdivider *= 1;
        }

        public void setNewInitial()
        {
            if (Initial)
            {
                envelope_cnt = 0;
                envelope_add = 0;
                on = true;
                freq_cnt = 0;
                if (lfsr7bit)
                {
                    lfsr = 0x7F;
                }
                else
                {
                    lfsr = 0x7FFF;
                }
                soundcycles_freq = 0;
            }
        }

        private void recalc_freq()
        {
            if (id < 3)
            {
                switch (Wave_Pattern_Duty)
                {
                    case 0: // 12,5%
                        for (int i = 0; i < 7; i++) { wavetable[i] = 0x0; }
                        wavetable[7] = 0xF;
                        break;

                    case 1: // 25%
                        for (int i = 1; i < 7; i++) { wavetable[i] = 0x0; }
                        wavetable[0] = 0xF;
                        wavetable[7] = 0xF;
                        break;

                    case 2: // 50%
                        for (int i = 1; i < 5; i++) { wavetable[i] = 0x0; }
                        wavetable[0] = 0xF;
                        for (int i = 5; i < 8; i++) { wavetable[i] = 0xF; }
                        break;

                    case 3: // 75%
                        for (int i = 1; i < 8; i++) { wavetable[i] = 0xF; }
                        wavetable[0] = 0x0;
                        wavetable[7] = 0x0;
                        break;
                }
            }
        }

        private void update_timebased(UInt32 new_cycles)
        {
            // freq - wavetable
            soundcycles_freq += new_cycles;
            if (id == 4)
            {
                if (soundcycles_freq >= ch4_freqdivider)
                {
                    soundcycles_freq -= ch4_freqdivider;
                    if ((lfsr & 1) == 0)
                    {
                        wavetable[0] = 0xF;
                    }
                    else
                    {
                        wavetable[0] = 0x0;
                    }
                    UInt32 bit0 = lfsr & 1;
                    UInt32 bit1 = (lfsr >> 1) & 1;
                    lfsr = lfsr >> 1;
                    if (lfsr7bit)
                    {
                        lfsr |= (bit0 ^ bit1) << 6;
                    }
                    else
                    {
                        lfsr |= (bit0 ^ bit1) << 14;
                    }
                }
            }
            else
            {
                if (id == 3)
                {
                    UInt32 newfreq_steps = soundcycles_freq / 2;
                    freq_cnt += newfreq_steps;
                    soundcycles_freq -= (newfreq_steps * 2);
                }
                else
                {
                    UInt32 newfreq_steps = soundcycles_freq / 4;
                    freq_cnt += newfreq_steps;
                    soundcycles_freq -= (newfreq_steps * 4);
                }
                if (freq_cnt >= (2048 - Frequency))
                {
                    freq_cnt = (UInt32)(freq_cnt - (2048 - Frequency));

                    if (id == 3)
                    {
                        if (banknumber)
                        {
                            byte swap_sample = wavetable2[0];
                            for (int i = 0; i < 31; i++)
                            {
                                wavetable2[i] = wavetable2[i + 1];
                            }
                            wavetable2[31] = swap_sample;
                        }
                        else
                        {
                            byte swap_sample = wavetable[0];
                            for (int i = 0; i < 31; i++)
                            {
                                wavetable[i] = wavetable[i + 1];
                            }
                            wavetable[31] = swap_sample;
                        }
                    }

                    wavetable_ptr++;
                    if (wavetable_ptr >= wavetable.Length)
                    {
                        wavetable_ptr = 0;
                        if (wavebanks)
                        {
                            banknumber = !banknumber;
                        }
                    }

                }
            }

            // sweep
            if (id == 1)
            {
                soundcycles_sweep += new_cycles;
                if (soundcycles_sweep >= 32768) // 128 Hz?
                {
                    soundcycles_sweep -= 32768;
                    if (Sweep_Time > 0)
                    {
                        sweepcnt++;
                        if (sweepcnt >= Sweep_Time)
                        {
                            sweepcnt = 0;
                            if (!Sweep_Frequency_Direction) // increase
                            {
                                Frequency = Frequency + (Frequency >> Number_of_sweep_shift);
                            }
                            else
                            {
                                Frequency = Frequency - (Frequency >> Number_of_sweep_shift);
                            }
                            if (Frequency <= 0 || Frequency >= 2048)
                            {
                                on = false;
                            }
                        }
                        recalc_freq();
                    }
                }
            }

            // envelope
            if (id != 3)
            {
                soundcycles_envelope += new_cycles;
                if (soundcycles_envelope >= 65536) // 64 Hz
                {
                    soundcycles_envelope -= 65536;
                    if (Envelope_Step_Time > 0)
                    {
                        envelope_cnt++;
                        if (envelope_cnt >= Envelope_Step_Time)
                        {
                            envelope_cnt = 0;
                            envelope_add = Math.Min(15, envelope_add + 1);
                            Int32 volume_current = Initial_Volume_of_envelope;
                            if (!Envelope_Direction) // decrease
                            {
                                volume_current = Math.Max(0, volume_current - envelope_add);
                            }
                            else
                            {
                                volume_current = Math.Min(15, volume_current + envelope_add);
                            }
                            volume = 0.25f * volume_current / 15;
                        }
                    }
                }
            }

            //length
            soundcycles_length += new_cycles;
            if (soundcycles_length >= 16384) // 256 Hz
            {
                soundcycles_length -= 16384;
                if (length_left > 0 && Length_Flag)
                {
                    length_left--;
                    if (length_left == 0)
                    {
                        on = false;
                    }
                }
            }
        }

        public float get_next()
        {
            update_timebased(95); // 4,..mhz / 44100

            float value = 0;

            if (id == 3)
            {
                if (on && waveenable)
                {
                    if (banknumber)
                    {
                        value = wavetable2[0] * volume;
                    }
                    else
                    {
                        value = wavetable[0] * volume;
                    }
                }
            }
            else
            {
                if (on)
                {
                    value = volume * wavetable[wavetable_ptr] / 15;
                }
            }

            return value;
        }

    }
}
