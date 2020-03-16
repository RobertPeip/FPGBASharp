using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public static class gameboy
    {
        public static bool on = false;
        public static bool coldreset = false;

        public static string filename;

        public static Int64 cycles;

        public static string statefilename;
        public static bool do_savestate = false;
        public static bool do_loadstate = false;
        public static bool loading_state;

        public static UInt32[] savestate = new UInt32[131072];

        public static void reset()
        {
            cycles = 0;

            if (coldreset)
            {
                GBRegs.reset();
                Flash.reset();
                EEProm.reset();
                GPU.reset();
                Memory.reset(filename);
            }

            CPU.reset();
            GPU_Timing.reset();
            Sound.reset();
            Joypad.set_reg();
            Timer.reset();
            DMA.reset();
            SoundDMA.reset();
            BusTiming.reset();
            gpio.rtcReset();

            //Serial.reset();

            loading_state = false;
            coldreset = false;

            on = true;
        }

        public static void run()
        {
            reset();

            while (true)
            {
                CPU.nextInstr();
                DMA.work();
                GPU_Timing.work();
                Sound.work();
                Timer.work();
                Joypad.set_reg();

                Serial.work();

                cycles = cycles + CPU.newticks;

                Memory.GameRAMSnapshot();

                if (do_savestate)
                {
                    create_savestate();
                    do_savestate = false;
                }

                if (do_loadstate)
                {
                    load_savestate();
                    do_loadstate = false;
                }
            }
        }

        private static void create_savestate()
        {
            savestate[0]++; // header -> number of savestate, 0 = invalid

            uint index = 2;
            savestate[index++] = CPU.PC;
            for (int i = 0; i < 18; i++)
            {
                savestate[index++] = CPU.regs[i];
            }
            for (int i = 0; i <= 1; i++)
            {
                savestate[index++] = CPU.regbanks[i, 8];
                savestate[index++] = CPU.regbanks[i, 9];
                savestate[index++] = CPU.regbanks[i, 10];
                savestate[index++] = CPU.regbanks[i, 11];
                savestate[index++] = CPU.regbanks[i, 12];
                savestate[index++] = CPU.regbanks[i, 13];
                savestate[index++] = CPU.regbanks[i, 14];
            }
            savestate[index++] = CPU.regbanks[1, 17];
            for (int i = 2; i <= 5; i++)
            {
                savestate[index++] = CPU.regbanks[i, 13];
                savestate[index++] = CPU.regbanks[i, 14];
                savestate[index++] = CPU.regbanks[i, 17];
            }

            uint cpu_mixed = 0;
            if (CPU.halt) cpu_mixed |= 1;
            if (CPU.Flag_Zero) cpu_mixed |= 1 << 1;
            if (CPU.Flag_Carry) cpu_mixed |= 1 << 2;
            if (CPU.Flag_Negative) cpu_mixed |= 1 << 3;
            if (CPU.Flag_V_Overflow) cpu_mixed |= 1 << 4;
            if (CPU.thumbmode) cpu_mixed |= 1 << 5;
            cpu_mixed |= (CPU.get_CPSR() & 0xF) << 6;
            if (CPU.IRQ_disable) cpu_mixed |= 1 << 10;
            if (CPU.FIQ_disable) cpu_mixed |= 1 << 11;
            savestate[index++] = cpu_mixed;

            savestate[index++] = IRP.IRP_Flags | (uint)((gpio.gpioData.bits & 0x3F) << 16);

            UInt32 eeprom_mix = 0;
            for (int i = 0; i < 8; i++)
            {
                eeprom_mix |= (uint)((EEProm.eepromBuffer[i] & 1) << i);
            }
            eeprom_mix |= (uint)((EEProm.eepromBits & 0xFF) << 8);
            eeprom_mix |= (uint)((EEProm.eepromByte & 0x3F) << 16);
            eeprom_mix |= (uint)((EEProm.eepromAddress & 0x3FF) << 22);
            savestate[index++] = eeprom_mix;

            UInt32 flash_mix = 0;
            flash_mix |= (uint)((0 & 0x7) << 0); // eeprombitpos
            flash_mix |= (uint)((Flash.flashBank & 0x1) << 3);
            if (!Memory.SramEnabled) { flash_mix |= 1 << 4; }
            if (Memory.SramEnabled && Memory.FlashEnabled) { flash_mix |= 1 << 5; }
            flash_mix |= (uint)((Convert.ToUInt32(EEProm.eepromMode) & 0x7) << 6);
            flash_mix |= (uint)((Convert.ToUInt32(Flash.flashState) & 0xF) << 9);
            flash_mix |= (uint)((Convert.ToUInt32(Flash.flashReadState) & 0xF) << 13);
            savestate[index++] = flash_mix;

            for (int i = 0; i < 4; i++) { savestate[index++] = DMA.DMAs[i].addr_source; }
            for (int i = 0; i < 4; i++) { savestate[index++] = DMA.DMAs[i].addr_target; }
            for (int i = 0; i < 4; i++)
            {
                UInt32 dmamix = DMA.DMAs[i].count & 0x1FFFF;
                if (DMA.DMAs[i].dMA_Enable) dmamix |= 1 << 17;
                if (DMA.DMAs[i].running) dmamix |= 1 << 18;
                if (DMA.DMAs[i].waiting) dmamix |= 1 << 19;
                if (DMA.DMAs[i].first) dmamix |= 1 << 20;
                dmamix |= (uint)((DMA.DMAs[i].dest_Addr_Control & 0x3) << 21);
                dmamix |= (uint)((DMA.DMAs[i].source_Adr_Control & 0x3) << 23);
                dmamix |= (uint)((DMA.DMAs[i].dMA_Start_Timing & 0x3) << 25);
                if (DMA.DMAs[i].dMA_Repeat) dmamix |= 1 << 27;
                if (DMA.DMAs[i].dMA_Transfer_Type) dmamix |= 1 << 28;
                if (DMA.DMAs[i].iRQ_on) dmamix |= 1 << 29;
                // if (DMA.DMAs[i].dmaon) dmamix |= 1 << 30; // only for hardware ?

                savestate[index++] = dmamix;
            }

            for (int i = 0; i < 4; i++)
            {
                UInt32 timermix = 0;
                if (Timer.timers[i].on) timermix |= 1;
                if (Timer.timers[i].startnow) timermix |= 1 << 1;
                timermix |= (uint)((Timer.timers[i].value & 0x1FFFF) << 2);
                timermix |= (uint)((Timer.timers[i].prescalevalue & 0x7FF) << 19);
                savestate[index++] = timermix;
            }

            UInt32 gpumix = (uint)GPU_Timing.cycles & 0xFFF;
            gpumix |= (uint)((GPU_Timing.line & 0xFF) << 12);
            gpumix |= (uint)((Convert.ToUInt32(GPU_Timing.gpustate) & 0x3) << 20);
            if (GBRegs.Sect_display.DISPSTAT_V_Counter_flag.read() == 1) gpumix |= 1 << 22;
            if (GBRegs.Sect_display.DISPSTAT_H_Blank_flag.read() == 1) gpumix |= 1 << 23;
            if (GBRegs.Sect_display.DISPSTAT_V_Blank_flag.read() == 1) gpumix |= 1 << 24;
            savestate[index++] = gpumix;

            UInt32 gpiomix = (uint)gpio.gpioData.clockslow & 0xFF;
            gpiomix |= (uint)((gpio.gpioData.command & 0xFF) << 8);
            gpiomix |= (uint)((gpio.gpioData.dataLen & 7) << 16);
            gpiomix |= (uint)((gpio.gpioData.enable & 1) << 19);
            gpiomix |= (uint)((gpio.gpioData.retval & 0xF) << 20);
            gpiomix |= (uint)((gpio.gpioData.select & 0xF) << 24);
            gpiomix |= (uint)((Convert.ToUInt32(gpio.gpioData.state) & 0x3) << 28);
            savestate[index++] = gpiomix;

            for (int i = 0; i < 256; i++) 
            { 
                UInt32 value = BitConverter.ToUInt32(GBRegs.data, i * 4);
                savestate[index++] = value; 
            }

            for (int i = 0; i < 65536; i++) { savestate[index++] = BitConverter.ToUInt32(Memory.WRAM_Large, i * 4); }
            for (int i = 0; i < 8192; i++) { savestate[index++] = BitConverter.ToUInt32(Memory.WRAM_Small, i * 4); }
            for (int i = 0; i < 256; i++) { savestate[index++] = BitConverter.ToUInt32(Memory.PaletteRAM, i * 4); }
            for (int i = 0; i < 24576; i++) { savestate[index++] = BitConverter.ToUInt32(Memory.VRAM, i * 4); }
            for (int i = 0; i < 256; i++) { savestate[index++] = BitConverter.ToUInt32(Memory.OAMRAM, i * 4); }

            savestate[1] = index; // header -> size
        }

        private static void load_savestate()
        {
            if (savestate[0] != 0)
            {
                reset();

                uint index = 2 + 68;

                loading_state = true;
                for (int i = 0; i < 256; i++)
                {
                    UInt32 data = savestate[index++];
                    int adr = i * 4;
                    Memory.write_dword((uint)(i * 4 + 0x4000000), data);
                }
                loading_state = false;

                for (int i = 0; i < 65536; i++)
                {
                    UInt32 data = savestate[index++];
                    int adr = i * 4;
                    Memory.WRAM_Large[adr] = (byte)(data & 0xFF);
                    Memory.WRAM_Large[adr + 1] = (byte)((data >> 8) & 0xFF);
                    Memory.WRAM_Large[adr + 2] = (byte)((data >> 16) & 0xFF);
                    Memory.WRAM_Large[adr + 3] = (byte)((data >> 24) & 0xFF);
                }
                for (int i = 0; i < 8192; i++)
                {
                    UInt32 data = savestate[index++];
                    int adr = i * 4;
                    Memory.WRAM_Small[adr] = (byte)(data & 0xFF);
                    Memory.WRAM_Small[adr + 1] = (byte)((data >> 8) & 0xFF);
                    Memory.WRAM_Small[adr + 2] = (byte)((data >> 16) & 0xFF);
                    Memory.WRAM_Small[adr + 3] = (byte)((data >> 24) & 0xFF);
                }
                for (int i = 0; i < 256; i++)
                {
                    UInt32 data = savestate[index++];
                    int adr = i * 4;
                    Memory.PaletteRAM[adr] = (byte)(data & 0xFF);
                    Memory.PaletteRAM[adr + 1] = (byte)((data >> 8) & 0xFF);
                    Memory.PaletteRAM[adr + 2] = (byte)((data >> 16) & 0xFF);
                    Memory.PaletteRAM[adr + 3] = (byte)((data >> 24) & 0xFF);
                }
                for (int i = 0; i < 24576; i++)
                {
                    UInt32 data = savestate[index++];
                    int adr = i * 4;
                    Memory.VRAM[adr] = (byte)(data & 0xFF);
                    Memory.VRAM[adr + 1] = (byte)((data >> 8) & 0xFF);
                    Memory.VRAM[adr + 2] = (byte)((data >> 16) & 0xFF);
                    Memory.VRAM[adr + 3] = (byte)((data >> 24) & 0xFF);
                }
                for (int i = 0; i < 256; i++)
                {
                    UInt32 data = savestate[index++];
                    int adr = i * 4;
                    Memory.OAMRAM[adr] = (byte)(data & 0xFF);
                    Memory.OAMRAM[adr + 1] = (byte)((data >> 8) & 0xFF);
                    Memory.OAMRAM[adr + 2] = (byte)((data >> 16) & 0xFF);
                    Memory.OAMRAM[adr + 3] = (byte)((data >> 24) & 0xFF);
                }

                index = 2;
                CPU.PC = savestate[index++];
                for (int i = 0; i < 18; i++)
                {
                    CPU.regs[i] = savestate[index++];
                }
                for (int i = 0; i <= 1; i++)
                {
                    CPU.regbanks[i, 8] = savestate[index++];
                    CPU.regbanks[i, 9] = savestate[index++];
                    CPU.regbanks[i, 10] = savestate[index++];
                    CPU.regbanks[i, 11] = savestate[index++];
                    CPU.regbanks[i, 12] = savestate[index++];
                    CPU.regbanks[i, 13] = savestate[index++];
                    CPU.regbanks[i, 14] = savestate[index++];
                }
                CPU.regbanks[1, 17] = savestate[index++];
                for (int i = 2; i <= 5; i++)
                {
                    CPU.regbanks[i, 13] = savestate[index++];
                    CPU.regbanks[i, 14] = savestate[index++];
                    CPU.regbanks[i, 17] = savestate[index++];
                }
                uint cpu_mixed = savestate[index++];
                if ((cpu_mixed & 1) > 0) CPU.halt = true; else CPU.halt = false;
                if ((cpu_mixed & (1 << 1)) > 0) CPU.Flag_Zero = true; else CPU.Flag_Zero = false;
                if ((cpu_mixed & (1 << 2)) > 0) CPU.Flag_Carry = true; else CPU.Flag_Carry = false;
                if ((cpu_mixed & (1 << 3)) > 0) CPU.Flag_Negative = true; else CPU.Flag_Negative = false;
                if ((cpu_mixed & (1 << 4)) > 0) CPU.Flag_V_Overflow = true; else CPU.Flag_V_Overflow = false;
                if ((cpu_mixed & (1 << 5)) > 0) CPU.thumbmode = true; else CPU.thumbmode = false;
                CPU.cpu_mode = CPU.get_mode_from_value(0x10 | ((cpu_mixed >> 6) & 0xF));
                if ((cpu_mixed & (1 << 10)) > 0) CPU.IRQ_disable = true; else CPU.IRQ_disable = false;
                if ((cpu_mixed & (1 << 11)) > 0) CPU.FIQ_disable = true; else CPU.FIQ_disable = false;

                IRP.IRP_Flags = (UInt16)(savestate[index] & 0xFFFF);
                GBRegs.Sect_system.IF.write(IRP.IRP_Flags);

                gpio.gpioData.bits = (UInt16)((savestate[index++] >> 16) & 0x3F);

                UInt32 eeprom_mix = savestate[index++];
                for (int i = 0; i < 8; i++)
                {
                    EEProm.eepromBuffer[i] = (byte)((eeprom_mix >> (7 - i)) & 1);
                }
                EEProm.eepromBits = (int)((eeprom_mix >> 8) & 0xFF);
                EEProm.eepromByte = (int)((eeprom_mix >> 16) & 0x3F);
                EEProm.eepromAddress = (int)((eeprom_mix >> 22) & 0x3FF);

                UInt32 flash_mix = savestate[index++];
                // eeprombitpos
                Flash.flashBank = (byte)((flash_mix >> 3) & 0x1);
                if ((flash_mix & (1 << 4)) > 0) Memory.SramEnabled = false; else Memory.SramEnabled = true;
                if ((flash_mix & (1 << 5)) > 0) Memory.SramEnabled = true; Memory.FlashEnabled = true;
                EEProm.eepromMode = (EEProm.EEPROMSTATE)((flash_mix >> 6) & 0x7);
                Flash.flashState = (Flash.FLASHSTATE)((flash_mix >> 9) & 0xF);
                Flash.flashReadState = (Flash.FLASHSTATE)((flash_mix >> 13) & 0xF);

                for (int i = 0; i < 4; i++) { DMA.DMAs[i].addr_source = savestate[index++]; }
                for (int i = 0; i < 4; i++) { DMA.DMAs[i].addr_target = savestate[index++]; }
                for (int i = 0; i < 4; i++)
                {
                    UInt32 dmamix = savestate[index++];
                    DMA.DMAs[i].count = dmamix & 0x1FFFF;
                    if ((dmamix & (1 << 17)) > 0) DMA.DMAs[i].dMA_Enable = true; else DMA.DMAs[i].dMA_Enable = false;
                    if ((dmamix & (1 << 18)) > 0) DMA.DMAs[i].running = true; else DMA.DMAs[i].running = false;
                    if ((dmamix & (1 << 19)) > 0) DMA.DMAs[i].waiting = true; else DMA.DMAs[i].waiting = false;
                    if ((dmamix & (1 << 20)) > 0) DMA.DMAs[i].first = true; else DMA.DMAs[i].first = false;
                    DMA.DMAs[i].dest_Addr_Control = (byte)((dmamix >> 21) & 0x3);
                    DMA.DMAs[i].source_Adr_Control = (byte)((dmamix >> 23) & 0x3);
                    DMA.DMAs[i].dMA_Start_Timing = (byte)((dmamix >> 25) & 0x3);
                    if ((dmamix & (1 << 27)) > 0) DMA.DMAs[i].dMA_Repeat = true; else DMA.DMAs[i].dMA_Repeat = false;
                    if ((dmamix & (1 << 28)) > 0) DMA.DMAs[i].dMA_Transfer_Type = true; else DMA.DMAs[i].dMA_Transfer_Type = false;
                    if ((dmamix & (1 << 29)) > 0) DMA.DMAs[i].iRQ_on = true; else DMA.DMAs[i].iRQ_on = false;
                }

                for (int i = 0; i < 4; i++)
                {
                    UInt32 timermix = savestate[index++];

                    if ((timermix & (1 << 0)) > 0) Timer.timers[i].on = true; else Timer.timers[i].on = false;
                    if ((timermix & (1 << 1)) > 0) Timer.timers[i].startnow = true; else Timer.timers[i].startnow = false;
                    Timer.timers[i].value = (int)((timermix >> 2) & 0x1FFFF);
                    Timer.timers[i].prescalevalue = (int)((timermix >> 19) & 0x7FF);

                    if (Timer.timers[i].on && Timer.timers[i].startnow) { Timer.timers[i].startnow = false; } // different handling in hw
                    Timer.timers[i].retval = (UInt16)Timer.timers[i].value;
                }

                UInt32 gpumix = savestate[index++];
                GPU_Timing.cycles = (int)((gpumix >> 0) & 0xFFF);
                GPU_Timing.line = (byte)((gpumix >> 12) & 0xFF);
                GPU_Timing.gpustate = (GPU_Timing.GPUState)((gpumix >> 20) & 0x3);
                GBRegs.Sect_display.DISPSTAT_V_Blank_flag.write((gpumix >> 22) & 1);
                GBRegs.Sect_display.DISPSTAT_H_Blank_flag.write((gpumix >> 23) & 1);
                GBRegs.Sect_display.DISPSTAT_V_Blank_flag.write((gpumix >> 24) & 1);
                GBRegs.Sect_display.VCOUNT.write(GPU_Timing.line);
                GPU_Timing.old_dispstat = GBRegs.data[4];

                UInt32 gpiomix = savestate[index++];
                gpio.gpioData.clockslow = (byte)((gpiomix >> 0) & 0xFF);
                gpio.gpioData.command = (byte)((gpiomix >> 8) & 0xFF);
                gpio.gpioData.dataLen = (byte)((gpiomix >> 16) & 0x7);
                gpio.gpioData.enable = (byte)((gpiomix >> 19) & 0x1);
                gpio.gpioData.retval = (byte)((gpiomix >> 20) & 0xF);
                gpio.gpioData.select = (byte)((gpiomix >> 24) & 0xF);
                gpio.gpioData.state = (gpio.GPIOState)((gpiomix >> 28) & 0x3);
            }
        }

    }
}
