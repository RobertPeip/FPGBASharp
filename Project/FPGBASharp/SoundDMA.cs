using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public static class SoundDMA
    {
        public class SingleSoundDMA
        {
            public GBReg Enable_RIGHT;
            public GBReg Enable_LEFT;
            public GBReg Timer_Select;
            public GBReg Reset_FIFO;

            public byte timerindex;
            public List<byte> fifo;
            public bool any_on;

            public List<byte> outfifo;
            public int tickcount;  

            public SingleSoundDMA(GBReg Enable_RIGHT, GBReg Enable_LEFT, GBReg Timer_Select, GBReg Reset_FIFO)
            {
                this.Enable_RIGHT = Enable_RIGHT;
                this.Enable_LEFT = Enable_LEFT;
                this.Timer_Select = Timer_Select;
                this.Reset_FIFO = Reset_FIFO;

                timerindex = 0;
                fifo = new List<byte>();
                any_on = false;

                outfifo = new List<byte>();
                outfifo.Add(0);
                tickcount = 0;
            }

            public void work()
            {
                tickcount += CPU.newticks;
                while (tickcount >= Sound.SAMPLERATE)
                {
                    tickcount -= Sound.SAMPLERATE;
                    if (fifo.Count > 0 && outfifo.Count < 15000)
                    {
                        outfifo.Add(fifo[0]);
                    }
                }
            }
        }

        public static SingleSoundDMA[] soundDMAs;

        public static void reset()
        {
            soundDMAs = new SingleSoundDMA[2];

            soundDMAs[1] = new SingleSoundDMA(GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_B_Enable_RIGHT, GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_B_Enable_LEFT, 
                GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_B_Timer_Select, GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_B_Reset_FIFO);

            soundDMAs[0] = new SingleSoundDMA(GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_A_Enable_RIGHT, GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_A_Enable_LEFT,
                GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_A_Timer_Select, GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_A_Reset_FIFO);
        }

        public static void timer_overflow(uint timerindex)
        {
            CPU.newticks = 0;
            for (uint i = 0; i < 2; i++)
            {
                if (soundDMAs[i].any_on && soundDMAs[i].timerindex == timerindex)
                {
                    if (soundDMAs[i].fifo.Count > 0)
                    {
                        soundDMAs[i].fifo.RemoveAt(0);
                    }

                    if (soundDMAs[i].fifo.Count == 16 || soundDMAs[i].fifo.Count == 0)
                    {
                        DMA.request_audio(i);
                        DMA.work();
                        DMA.delayed = true;
                    }
                }
            }
        }

        public static void write_SOUNDCNT_H()
        {
            for (int i = 0; i < 2; i++)
            {
                if (soundDMAs[i].Reset_FIFO.on())
                {
                    soundDMAs[i].fifo.Clear();
                }
                soundDMAs[i].timerindex = (byte)soundDMAs[i].Timer_Select.read();
                soundDMAs[i].any_on = soundDMAs[i].Enable_LEFT.on() || soundDMAs[i].Enable_RIGHT.on();
            }

            uint oldval = GBRegs.Sect_sound.SOUNDCNT_H.read();
            GBRegs.Sect_sound.SOUNDCNT_H.write(oldval & 0x770F);


            switch(GBRegs.Sect_sound.SOUNDCNT_H_Sound_1_4_Volume.read())
            {
                case 0: Sound.soundGenerator.volume_1_4 = 0.25f; break;
                case 1: Sound.soundGenerator.volume_1_4 = 0.5f; break;
                case 2: Sound.soundGenerator.volume_1_4 = 1.0f; break;
            }
            if (GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_A_Volume.on()) { Sound.soundGenerator.volume_dma0 = 1.0f; } else { Sound.soundGenerator.volume_dma0 = 0.5f; }
            if (GBRegs.Sect_sound.SOUNDCNT_H_DMA_Sound_B_Volume.on()) { Sound.soundGenerator.volume_dma1 = 1.0f; } else { Sound.soundGenerator.volume_dma1 = 0.5f; }

        }

        public static void fill_fifo(int index, UInt32 value, bool dwaccess)
        {
            soundDMAs[index].fifo.Add((byte)value);
            soundDMAs[index].fifo.Add((byte)(value >> 8));
            if (dwaccess)
            {
                soundDMAs[index].fifo.Add((byte)(value >> 16));
                soundDMAs[index].fifo.Add((byte)(value >> 24));
            }
        }
    }
}
