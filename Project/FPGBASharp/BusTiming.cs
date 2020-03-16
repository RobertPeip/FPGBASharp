using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public static class BusTiming
    {
        const bool prefetch_off = false;

        public static readonly byte[] gamepakRamWaitState = { 4, 3, 2, 8 };
        public static readonly byte[] gamepakWaitState = { 4, 3, 2, 8 };
        public static readonly byte[] gamepakWaitState0 = { 2, 1 };
        public static readonly byte[] gamepakWaitState1 = { 4, 1 };
        public static readonly byte[] gamepakWaitState2 = { 8, 1 };

        //public static bool busPrefetch;
        public static bool busPrefetchEnable;
        public static int busPrefetchCount;

        public static byte[] memoryWait = new byte[16];
        public static byte[] memoryWait32 = new byte[16];
        public static byte[] memoryWaitSeq = new byte[16];
        public static byte[] memoryWaitSeq32 = new byte[16];

        public static void reset()
        {
            memoryWait = new byte[16] { 0, 0, 2, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, 4, 0 };
            memoryWait32 = new byte[16] { 0, 0, 5, 0, 0, 1, 1, 0, 7, 7, 9, 9, 13, 13, 4, 0 };
            memoryWaitSeq = new byte[16] { 0, 0, 2, 0, 0, 0, 0, 0, 2, 2, 4, 4, 8, 8, 4, 0 };
            memoryWaitSeq32 = new byte[16] { 0, 0, 5, 0, 0, 1, 1, 0, 5, 5, 9, 9, 17, 17, 4, 0 };
        }

        public static void update(UInt16 value)
        {
            memoryWait[0x0e] = memoryWaitSeq[0x0e] = gamepakRamWaitState[value & 3];

            memoryWait[0x08] = memoryWait[0x09] = gamepakWaitState[(value >> 2) & 3];
            memoryWaitSeq[0x08] = memoryWaitSeq[0x09] = gamepakWaitState0[(value >> 4) & 1];
            
            memoryWait[0x0a] = memoryWait[0x0b] = gamepakWaitState[(value >> 5) & 3];
            memoryWaitSeq[0x0a] = memoryWaitSeq[0x0b] = gamepakWaitState1[(value >> 7) & 1];
            
            memoryWait[0x0c] = memoryWait[0x0d] = gamepakWaitState[(value >> 8) & 3];
            memoryWaitSeq[0x0c] = memoryWaitSeq[0x0d] = gamepakWaitState2[(value >> 10) & 1];

            for (int i = 8; i < 15; i++)
            {
                memoryWait32[i] = (byte)(memoryWait[i] + memoryWaitSeq[i] + 1);
                memoryWaitSeq32[i] = (byte)(memoryWaitSeq[i] * 2 + 1);
            }

            if ((value & 0x4000) == 0x4000)
            {
                busPrefetchEnable = true;
                //busPrefetch = false;
                //busPrefetchCount = 0;
            }
            else
            {
                busPrefetchEnable = false;
                //busPrefetch = false;
                //busPrefetchCount = 0;
            }

            //Game_Pak_Type_Flag = ((value >> 15) & 0x1) == 1;

        }

        public static int dataTicksAccess16(UInt32 address, int cycleadd) // DATA 8/16bits NON SEQ
        {
            UInt32 addr = (address >> 24) & 15;
            int value = memoryWait[addr];
            if (prefetch_off) { return value; }

            if (addr >= 0x08)
            {
                busPrefetchCount = 0;
                //busPrefetch = false;
            }
            else if (busPrefetchEnable)
            {
                int seq = memoryWaitSeq[(CPU.regs[15] >> 24) & 15];
                int non = memoryWait[(CPU.regs[15] >> 24) & 15];
                int max = memoryWaitSeq[(CPU.regs[15] >> 24) & 15] * 8;
                busPrefetchCount = Math.Min(max, busPrefetchCount + value + (non - seq - 1) + cycleadd); ;
            }

            return value;
        }

        public static int dataTicksAccess32(UInt32 address, int cycleadd) // DATA 32bits NON SEQ
        {
            UInt32 addr = (address >> 24) & 15;
            int value = memoryWait32[addr];
            if (prefetch_off) { return value; }

            if (addr == 6 && GPU_Timing.gpustate == GPU_Timing.GPUState.VISIBLE)
            {
                if (address < 0x06010000 && GPU_Timing.cycles < 980)
                {
                    if ((byte)GBRegs.Sect_display.DISPCNT_BG_Mode.read() < 3)
                    {
                        value += 980 - GPU_Timing.cycles;
                    }
                }
            }

            if (addr >= 0x08)
            {
                busPrefetchCount = 0;
                //busPrefetch = false;
            }
            else if (busPrefetchEnable)
            {
                int seq = memoryWaitSeq[(CPU.regs[15] >> 24) & 15];
                int non = memoryWait[(CPU.regs[15] >> 24) & 15];
                int max = memoryWaitSeq[(CPU.regs[15] >> 24) & 15] * 8;
                busPrefetchCount = Math.Min(max, busPrefetchCount + value + (non - seq - 1) + cycleadd);
            }

            return value;
        }

        public static int dataTicksAccessSeq16(UInt32 address, int cycleadd) // DATA 8/16bits SEQ
        {
            UInt32 addr = (address >> 24) & 15;
            int value = memoryWaitSeq[addr];
            if (prefetch_off) { return value; }

            if (addr >= 0x08)
            {
                busPrefetchCount = 0;
                //busPrefetch = false;
            }
            else if (busPrefetchEnable)
            {
                int seq = memoryWaitSeq[(CPU.regs[15] >> 24) & 15];
                int non = memoryWait[(CPU.regs[15] >> 24) & 15];
                int max = memoryWaitSeq[(CPU.regs[15] >> 24) & 15] * 8;
                busPrefetchCount = Math.Min(max, busPrefetchCount + value + (non - seq - 1) + cycleadd);
            }

            return value;
        }

        public static int dataTicksAccessSeq32(UInt32 address, int cycleadd) // DATA 32bits SEQ
        {
            UInt32 addr = (address >> 24) & 15;
            int value = memoryWaitSeq32[addr];
            if (prefetch_off) { return value; }

            if (addr >= 0x08)
            {
                busPrefetchCount = 0;
                //busPrefetch = false;
            }
            else if (busPrefetchEnable)
            {
                int seq = memoryWaitSeq[(CPU.regs[15] >> 24) & 15];
                int non = memoryWait[(CPU.regs[15] >> 24) & 15];
                int max = memoryWaitSeq[(CPU.regs[15] >> 24) & 15] * 8;
                busPrefetchCount = Math.Min(max, busPrefetchCount + value + (non - seq - 1) + cycleadd);
            }

            return value;
        }

        // Waitstates when executing opcode
        public static int codeTicksAccess16(UInt32 address) // THUMB NON SEQ
        {
            UInt32 addr = (address >> 24) & 15;
            if (prefetch_off) { return memoryWait[addr]; }

            int value = memoryWait[addr];
            if ((addr >= 0x08) && (addr <= 0x0D) && busPrefetchCount > 0)
            {
                int subtract = Math.Min(busPrefetchCount, value);
                busPrefetchCount -= subtract;
                return (value - subtract);
            }
            else
            {
                busPrefetchCount = 0;
                return memoryWait[addr];
            }
        }

        public static int codeTicksAccess32(UInt32 address) // ARM NON SEQ
        {
            UInt32 addr = (address >> 24) & 15;
            if (prefetch_off) { return memoryWait32[addr]; }

            int value = memoryWait32[addr];
            if ((addr >= 0x08) && (addr <= 0x0D) && busPrefetchCount > 0)
            {
                int subtract = Math.Min(busPrefetchCount, value);
                busPrefetchCount -= subtract;
                return (value - subtract);
            }
            else
            {
                busPrefetchCount = 0;
                return memoryWait32[addr];
            }
        }

        public static int codeTicksAccessSeq16(UInt32 address) // THUMB SEQ
        {
            UInt32 addr = (address >> 24) & 15;
            if (prefetch_off) { return memoryWaitSeq[addr]; }

            int value = memoryWaitSeq[addr];
            if ((addr >= 0x08) && (addr <= 0x0D) && busPrefetchCount > 0)
            {
                int subtract = Math.Min(busPrefetchCount, value);
                busPrefetchCount -= subtract;
                return (value - subtract);
            }
            else
            {
                busPrefetchCount = 0;
                return memoryWaitSeq[addr];
            }
        }

        public static int codeTicksAccessSeq32(UInt32 address) // ARM SEQ
        {
            UInt32 addr = (address >> 24) & 15;
            if (prefetch_off) { return memoryWaitSeq32[addr]; }

            int value = memoryWaitSeq32[addr];
            if ((addr >= 0x08) && (addr <= 0x0D) && busPrefetchCount > 0)
            {
                int subtract = Math.Min(busPrefetchCount, value);
                busPrefetchCount -= subtract;
                return (value - subtract);
            }
            else
            {
                return memoryWaitSeq32[addr];
            }
        }
    }
}
