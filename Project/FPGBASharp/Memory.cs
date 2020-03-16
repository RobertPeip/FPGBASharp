using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gbemu
{
    public static class Memory
    {
        public static bool has_tilt = false;
        public static bool gpio_enable = false;

        public static Byte[] WRAM_Large = new Byte[262144];
        public static Byte[] WRAM_Small = new Byte[32768];
        public static Byte[] VRAM = new Byte[98304];
        public static Byte[] OAMRAM = new Byte[1024];
        public static Byte[] PaletteRAM = new Byte[1024];
        public static Byte[] GameRom = new Byte[33554432];

        public static UInt32 GameRom_max;
        public static byte blockcmd_lowerbits;

        public static Byte[] GameRamSnapshot;
        public static bool createGameRAMSnapshot; 

        private static Byte[] GBRom;

        public static bool EEPROMEnabled;
        public static bool FlashEnabled;
        public static bool SramEnabled;
        public static bool EEPROMSensorEnabled;
        public static Func<UInt32, byte, object> SaveGameFunc;

        public static byte[] biosProtected;

        public static UInt64[] readcnt8;
        public static UInt64[] readcnt16;
        public static UInt64[] readcnt32;
        public static UInt64[] writecnt8;
        public static UInt64[] writecnt16;
        public static UInt64[] writecnt32;

        public static UInt16 tiltx;
        public static UInt16 tilty;

        public static bool unreadable;
        public static UInt32 lastreadvalue;

        public static bool enable_debugout = false;
        public static List<char> debug_out;

        public static bool gpio_used;

        public static void reset(string filename)
        {
            FileInfo fileInfo;
#if DEBUG
            fileInfo = new FileInfo("gba_bios.bin");
            //fileInfo = new FileInfo("gba_bios_fast.bin");
#else
            //fileInfo = new FileInfo("gba_bios_fast.bin");
            fileInfo = new FileInfo("gba_bios.bin");
#endif
            if (!fileInfo.Exists)
            {
                MessageBox.Show("gba_bios.bin not found!");
                Application.Exit();
            }

            GBRom = new byte[fileInfo.Length];
            FileStream fileStream = fileInfo.OpenRead();
            fileStream.Read(GBRom, 0, GBRom.Length);
            fileStream.Close();

            fileInfo = new FileInfo(filename);
            //GameRom = new byte[fileInfo.Length];
            fileStream = fileInfo.OpenRead();
            fileStream.Read(GameRom, 0, GameRom.Length);
            fileStream.Close();
            GameRom_max = (UInt32)fileInfo.Length;

            GameRamSnapshot = new Byte[131072 + 8192];

            EEPROMEnabled = true;
            FlashEnabled = true;
            SramEnabled = true;
            EEPROMSensorEnabled = false;
            SaveGameFunc = Flash.flashSaveDecide;

            biosProtected = new byte[4];
            //biosProtected = new byte[4] { 0x02, 0xC0, 0x5E, 0xE5 };
            //biosProtected = new byte[4] { 0x04, 0x20, 0xA0, 0xE3 };

            readcnt8 = new UInt64[16];
            readcnt16 = new UInt64[16];
            readcnt32 = new UInt64[16];
            writecnt8 = new UInt64[16];
            writecnt16 = new UInt64[16];
            writecnt32 = new UInt64[16];

            Header.autocheck();

            tiltx = 0x3A0;
            tilty = 0x3A0;

            gpio_used = false;
            gpio.rtcEnable(true);

            load_gameram();
        }

        public static void GameRAMSnapshot()
        {
            if (createGameRAMSnapshot)
            {
                int index = 0;
                for (int i = 0; i < Flash.flashSaveMemory.Length; i++)
                {
                    GameRamSnapshot[index] = Flash.flashSaveMemory[i];
                    index++;
                }
                for (int i = 0; i < EEProm.eepromData.Length; i++)
                {
                    GameRamSnapshot[index] = EEProm.eepromData[i];
                    index++;
                }

                createGameRAMSnapshot = false;
            }
        }


        private static void load_gameram()
        {
            if (gameboy.filename != null)
            {
                string filename = Path.GetFileNameWithoutExtension(gameboy.filename) + ".sav";
                if (File.Exists(filename))
                {
                    FileInfo fileInfo = new FileInfo(filename);
                    if (fileInfo.Length == GameRamSnapshot.Length)
                    {
                        FileStream fileStream = fileInfo.OpenRead();
                        fileStream.Read(GameRamSnapshot, 0, GameRamSnapshot.Length);
                        fileStream.Close();

                        int index = 0;
                        for (int i = 0; i < Flash.flashSaveMemory.Length; i++)
                        {
                            Flash.flashSaveMemory[i] = GameRamSnapshot[index];
                            index++;
                        }
                        for (int i = 0; i < EEProm.eepromData.Length; i++)
                        {
                            EEProm.eepromData[i] = GameRamSnapshot[index];
                            index++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Savegame corrupt!");
                    }
                    
                }
            }
        }

        public static void save_gameram()
        {
            if (gameboy.filename != null)
            {
                string filename = Path.GetFileNameWithoutExtension(gameboy.filename) + ".sav";
                FileStream fileStream = new FileStream(filename, FileMode.Create);
                fileStream.Write(GameRamSnapshot, 0, GameRamSnapshot.Length);
                fileStream.Close();
            }
        }

        public static byte read_unreadable_byte(UInt32 offset)
        {
            byte value;

            if (CPU.thumbmode && ((CPU.regs[15] >> 24) & 15) == 3)
            {
                if ((CPU.regs[15] & 0x3) == 0)
                {
                    value = read_byte(CPU.regs[15] - 2 + offset);
                }
                else
                {
                    value = read_byte(CPU.regs[15] + offset);
                }
            }
            else
            {
                value = read_byte(CPU.regs[15] + offset);
            }

            unreadable = true;

            return value;
        }
        public static UInt16 read_unreadable_word()
        {
            UInt16 value;
            if (CPU.thumbmode && ((CPU.regs[15] >> 24) & 15) == 3)
            {
                if ((CPU.regs[15] & 0x3) == 0)
                {
                    value = (UInt16)read_word(CPU.regs[15] - 2);
                }
                else
                {
                    value = (UInt16)read_word(CPU.regs[15]);
                }
            }
            else
            {
                value = (UInt16)read_word(CPU.regs[15]);
            }

            unreadable = true;

            return value;
        }

        public static UInt32 read_unreadable_dword()
        {
            if (CPU.op_since_dma < 2)
            {
                return DMA.last_dma_value;
            }

            UInt32 value;

            if (CPU.thumbmode)
            {
                //For THUMB code in 32K - WRAM on GBA, GBA SP, GBA Micro, NDS-Lite(but not NDS):
                //LSW = [$+4], MSW = OldHI   ; for opcodes at 4 - byte aligned locations
                //LSW = OldLO, MSW = [$+4]   ; for opcodes at non - 4 - byte aligned locations
                //OldLO=[$+2], OldHI=[$+2]
                if (((CPU.regs[15] >> 24) & 15) == 3)
                {
                    if ((CPU.regs[15] & 0x3) == 0)
                    {
                        UInt32 retval_low = read_word(CPU.regs[15]) & 0xFFFF;
                        UInt32 retval_high = read_word(CPU.regs[15] - 2) & 0xFFFF;
                        value = retval_high << 16 | retval_low;
                    }
                    else
                    {
                        UInt32 retval_low = read_word(CPU.regs[15] - 2) & 0xFFFF;
                        UInt32 retval_high = read_word(CPU.regs[15]) & 0xFFFF;
                        value = retval_high << 16 | retval_low;
                    }
                }
                else // standard case LSW = [$+4], MSW = [$+4]
                {
                    UInt32 retval = read_word(CPU.regs[15]);
                    value = retval << 16 | retval;
                }
            }
            else
            {
                value = read_dword(CPU.regs[15]);
            }

            unreadable = true;

            return value;
        }

        public static byte read_byte(UInt32 address)
        {
#if DEBUG
            readcnt8[(address >> 24) & 0xF]++;
#endif
            unreadable = false;
            uint adr;

            byte select = (byte)(address >> 24);
            switch (select)
            {
                case 0:
                    if ((CPU.regs[15] >> 24) > 0)
                    {
                        if (address < 0x4000)
                        {
                            return biosProtected[address & 3];
                        }
                        else
                        {
                            return read_unreadable_byte(address & 1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            biosProtected[i] = GBRom[((address + 8) & 0x3FFC) + i];
                        }
                        return GBRom[address & 0x3FFF];
                    }

                case 1: return read_unreadable_byte(address & 1);
                case 2: return WRAM_Large[address & 0x03FFFF];
                case 3: return WRAM_Small[address & 0x7FFF];

                case 4:
                    if (address < 0x04000400)
                    {
                        adr = address & 0x3FF;
                        byte rwmask = GBRegs.rwmask[adr];

                        if (rwmask == 0)
                        {
                            return read_unreadable_byte(address & 1);
                        }
                        else
                        {
                            prepare_read_gbreg(adr);
                            byte value = GBRegs.data[adr];
                            value &= rwmask;
                            return value;
                        }
                    }
                    else
                    {
                        return read_unreadable_byte(address & 1);
                    }

                case 5: return PaletteRAM[address & 0x3FF];

                case 6:
                    adr = address & 0x1FFFF;
                    if (adr > 0x17FFF) { adr -= 0x8000; }
                    return VRAM[adr & 0x1FFFF];

                case 7: return OAMRAM[address & 0x3FF];

                case 8:
                case 9:
                case 0xA:
                case 0xB:
                case 0xC:
                    if ((address & 0x01FFFFFF) < GameRom_max)
                    {
                        return GameRom[address & 0x01FFFFFF];
                    }
                    else
                    {
                        if ((address & 1) == 0)
                        {
                            return (byte)(address / 2);
                        }
                        else
                        {
                            return (byte)((address >> 8) / 2);
                        }
                    }

                case 0xD:
                    if (EEPROMEnabled)
                    {
                        return (byte)EEProm.read();
                    }
                    else
                    {
                        return read_unreadable_byte(address & 1);
                    }

                case 0xE:
                case 0xF:
                    if (has_tilt && address == 0x0E008200)
                    {
                        return (byte)tiltx;
                    }
                    if (has_tilt && address == 0x0E008300)
                    {
                        return (byte)(0x80 | ((tiltx >> 8) & 0xF));
                    }
                    if (has_tilt && address == 0x0E008400)
                    {
                        return (byte)tilty;
                    }
                    if (has_tilt && address == 0x0E008500)
                    {
                        return (byte)((tilty >> 8) & 0xF);
                    }

                    if (FlashEnabled | SramEnabled)
                    {
                        return Flash.flashRead(address);
                    }
                    else
                    {
                        return read_unreadable_byte(address);
                    }

                default: return read_unreadable_byte(address & 1);

            }
        }

        public static UInt32 read_word(UInt32 address)
        {
#if DEBUG
            readcnt16[(address >> 24) & 0xF]++;
#endif

            unreadable = false;

            UInt32 value = 0;
            byte rotate = (byte)(address & 1);
            address = address & 0xFFFFFFFE;
            uint adr;

            byte select = (byte)(address >> 24);
            switch (select)
            {
                case 0:
                    if ((CPU.regs[15] >> 24) > 0)
                    {
                        if (address < 0x4000)
                        {
                            value = BitConverter.ToUInt16(biosProtected, (int)address & 2);
                        }
                        else
                        {
                            value = read_unreadable_word();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            biosProtected[i] = GBRom[((address + 8) & 0x3FFC) + i];
                        }
                        value = BitConverter.ToUInt16(GBRom, (int)address & 0x3FFE);
                    }
                    break;

                case 1: value = read_unreadable_word(); break;
                case 2: value = BitConverter.ToUInt16(WRAM_Large, (int)address & 0x03FFFF); break;
                case 3: value = BitConverter.ToUInt16(WRAM_Small, (int)address & 0x7FFF); break;

                case 4:
                    if (address < 0x04000400)
                    {
                        adr = address & 0x3FF;

                        if (adr == GBRegs.Sect_dma.DMA0CNT_L.address ||
                            adr == GBRegs.Sect_dma.DMA1CNT_L.address ||
                            adr == GBRegs.Sect_dma.DMA2CNT_L.address ||
                            adr == GBRegs.Sect_dma.DMA3CNT_L.address)
                        {
                            return 0;
                        }
                        else
                        {
                            UInt16 rwmask = BitConverter.ToUInt16(GBRegs.rwmask, (int)adr & 0x3FFE);

                            if (rwmask == 0)
                            {
                                value = read_unreadable_word();
                            }
                            else
                            {
                                prepare_read_gbreg(adr);
                                value = BitConverter.ToUInt16(GBRegs.data, (int)adr);
                                value &= rwmask;
                            }
                        }
                    }
                    else
                    {
                        value = read_unreadable_word();
                    }
                    break;

                case 5: value = BitConverter.ToUInt16(PaletteRAM, (int)address & 0x3FF); break;

                case 6:
                    adr = address & 0x1FFFF;
                    if (adr > 0x17FFF) { adr -= 0x8000; }
                    value = BitConverter.ToUInt16(VRAM, (int)adr);
                    break;

                case 7: value = BitConverter.ToUInt16(OAMRAM, (int)address & 0x3FF); break;

                case 8:
                case 9:
                case 0xA:
                case 0xB:
                case 0xC:
                    if (gpio_enable && gpio_used && address >= 0x80000c4 && address <= 0x80000c8)
                    {
                        value = gpio.gpioRead(address);
                    }
                    else
                    {
                        if ((address & 0x01FFFFFF) < GameRom_max)
                        {
                            value = BitConverter.ToUInt16(GameRom, (int)address & 0x01FFFFFF);
                        }
                        else
                        {
                            value = (address / 2) & 0xFFFF;
                        }
                    }
                    break;

                case 0xD:
                    if (EEPROMEnabled)
                    {
                        value = (UInt32)EEProm.read();
                    }
                    break;

                case 0xE:
                case 0xF:
                    if (FlashEnabled | SramEnabled)
                    {
                        value = Flash.flashRead(address + rotate) * (UInt32)0x0101;
                    }
                    else
                    {
                        value = read_unreadable_word();
                    }
                    break;

                default: value = read_unreadable_word(); break;
            }

            lastreadvalue = value;

            if (rotate == 0)
            {
                return value;
            }
            else
            {
                value = ((value & 0xFF) << 24) | (value >> 8);
            }

            return value;
        }

        public static UInt32 read_dword(UInt32 address)
        {
#if DEBUG
            readcnt32[(address >> 24) & 0xF]++;
#endif
            unreadable = false;

            UInt32 value = 0;
            byte rotate = (byte)(address & 3);
            address = address & 0xFFFFFFFC;

            byte select = (byte)(address >> 24);
            switch (select)
            {
                case 0:
                    if ((CPU.regs[15] >> 24) > 0)
                    {
                        if (address < 0x4000)
                        {
                            value = BitConverter.ToUInt32(biosProtected, 0);
                        }
                        else
                        {
                            value = read_unreadable_dword();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            biosProtected[i] = GBRom[((address + 8) & 0x3FFC) + i];
                        }
                        value = BitConverter.ToUInt32(GBRom, (int)address & 0x3FFC);
                    }
                    break;

                case 1: value = read_unreadable_dword(); break;
                case 2: value = BitConverter.ToUInt32(WRAM_Large, (int)address & 0x03FFFF); break;
                case 3: value = BitConverter.ToUInt32(WRAM_Small, (int)address & 0x7FFF); break;

                case 4:
                    if (address < 0x04000400)
                    {
                        value = (UInt16)read_word(address) | (read_word(address + 2) << 16);
                    }
                    else
                    {
                        value = read_unreadable_dword();
                    }
                    break;

                case 5: value = BitConverter.ToUInt32(PaletteRAM, (int)address & 0x3FF); break;

                case 6:
                    uint adr = address & 0x1FFFF;
                    if (adr > 0x17FFF) { adr -= 0x8000; }
                    value = BitConverter.ToUInt32(VRAM, (int)adr);
                    break;

                case 7: value = BitConverter.ToUInt32(OAMRAM, (int)address & 0x3FF); break;

                case 8:
                case 9:
                case 0xA:
                case 0xB:
                case 0xC:
                    if (gpio_enable && gpio_used && address >= 0x80000c4 && address <= 0x80000c8)
                    {
                        value = gpio.gpioRead(address);
                    }
                    else
                    {
                        if ((address & 0x01FFFFFF) < GameRom_max)
                        {
                            value = BitConverter.ToUInt32(GameRom, (int)address & 0x01FFFFFF);
                        }
                        else
                        {
                            value = ((address / 2) & 0xFFFF) + ((((address / 2) + 1) & 0xFFFF) << 16);
                        }
                    }
                    break;

                case 0xD:
                    if (EEPROMEnabled)
                    {
                        value = (UInt32)EEProm.read();
                    }
                    break;

                case 0xE:
                case 0xF:
                    if (FlashEnabled | SramEnabled)
                    {
                        value = Flash.flashRead(address + rotate + blockcmd_lowerbits) * (UInt32)0x01010101;
                    }
                    else
                    {
                        value = read_unreadable_dword();
                    }
                    break;

                default: value = read_unreadable_dword(); break;
            }

            lastreadvalue = value;

            if (rotate == 0)
            {
                return value;
            }
            else
            {
                value = CPU.RotateRight(value, 8 * rotate);
            }

            return value;
        }

        public static void write_byte(UInt32 address, byte data)
        {
#if DEBUG
            writecnt8[(address >> 24) & 0xF]++;
            if (enable_debugout && address >= 0x04FFF600 & address < 0x04FFF700)
            {
                debug_out.Add((char)data);
            }
            if (enable_debugout && address == 0x04FFF700)
            {
                int a = 5;
            }
#endif
            uint adr;
            byte select = (byte)(address >> 24);
            switch (select)
            {
                case 2: WRAM_Large[address & 0x03FFFF] = (byte)(data & 0xFF); return;
                case 3: WRAM_Small[address & 0x7FFF] = (byte)(data & 0xFF); return;

                case 4:
                    if (address < 0x04000400)
                    {
                        adr = address & 0x3FF;
                        GBRegs.data[adr] = data;
                        write_gbreg(adr & 0xFFFFFFFE, data, false);
                    }
                    return;

                // Writing 8bit Data to Video Memory
                // Video Memory(BG, OBJ, OAM, Palette) can be written to in 16bit and 32bit units only.Attempts to write 8bit data(by STRB opcode) won't work:
                // Writes to OBJ(6010000h - 6017FFFh)(or 6014000h - 6017FFFh in Bitmap mode) and to OAM(7000000h - 70003FFh) are ignored, the memory content remains unchanged.
                // Writes to BG(6000000h - 600FFFFh)(or 6000000h - 6013FFFh in Bitmap mode) and to Palette(5000000h - 50003FFh) are writing the new 8bit value to BOTH upper and lower 8bits of the addressed halfword, ie. "[addr AND NOT 1]=data*101h".

                case 5: PaletteRAM[address & 0x3FE] = data; PaletteRAM[(address & 0x3FE) + 1] = data; return;

                case 6:
                    adr = address & 0x1FFFE;
                    if ((GPU.videomode <= 2 && adr <= 0xFFFF) || GPU.videomode >= 3 && adr <= 0x013FFF)
                    {
                        if (adr > 0x17FFF) { adr -= 0x8000; }
                        VRAM[adr] = data;
                        VRAM[adr + 1] = data;
                    }
                    return;

                case 7: return; // no saving here!

                case 0xD:
                    if (EEPROMEnabled)
                    {
                        EEProm.write(data);
                    }
                    return;

                case 0xE:
                case 0xF:
                    if (!EEPROMEnabled | FlashEnabled | SramEnabled)
                    {
                        SaveGameFunc(address, data);
                    }
                    return;
            }
        }

        public static void write_word(UInt32 address, UInt16 data)
        {
#if DEBUG
            writecnt16[(address >> 24) & 0xF]++;

            if (address == 0x04FFF780)
            {
                enable_debugout = true;
                debug_out = new List<char>();
            }
            if (enable_debugout && address >= 0x04FFF600 & address < 0x04FFF700)
            {
                debug_out.Add((char)(data & 0xFF));
                debug_out.Add((char)((data >> 8) & 0xFF));
            }
            if (enable_debugout && address == 0x04FFF700)
            {
                if ((data & 0xFF) < 3)
                {
                    int a = 5;
                }
            }
#endif
            byte offset = (byte)(address & 1);
            address = address & 0xFFFFFFFE;
            uint adr;

            byte select = (byte)(address >> 24);
            switch (select)
            {
                case 2:
                    adr = address & 0x03FFFF;
                    WRAM_Large[adr] = (byte)(data & 0xFF);
                    WRAM_Large[adr + 1] = (byte)((data >> 8) & 0xFF);
                    return;

                case 3:
                    adr = address & 0x7FFF;
                    WRAM_Small[adr] = (byte)(data & 0xFF);
                    WRAM_Small[adr + 1] = (byte)((data >> 8) & 0xFF);
                    return;

                case 4:
                    if (address < 0x04000400)
                    {
                        adr = address & 0x3FF;
                        GBRegs.data[adr] = (byte)(data & 0xFF);
                        GBRegs.data[adr + 1] = (byte)((data >> 8) & 0xFF);
                        write_gbreg(adr, data, false);
                    }
                    return;

                case 5:
                    adr = address & 0x3FF;
                    PaletteRAM[adr] = (byte)(data & 0xFF);
                    PaletteRAM[adr + 1] = (byte)((data >> 8) & 0xFF);
                    return;

                case 6:
                    adr = address & 0x1FFFF;
                    if (GPU.videomode < 3 || ((address & 0x1C000) != 0x18000))
                    {
                        if (adr > 0x17FFF) { adr -= 0x8000; }
                        VRAM[adr] = (byte)(data & 0xFF);
                        VRAM[adr + 1] = (byte)((data >> 8) & 0xFF);
                    }
                    return;

                case 7:
                    adr = address & 0x3FF;
                    OAMRAM[adr] = (byte)(data & 0xFF);
                    OAMRAM[adr + 1] = (byte)((data >> 8) & 0xFF);
                    return;

                case 0x8:
                    if (gpio_enable)
                    {
                        if (address >= 0x80000c4 && address <= 0x80000c8)
                        {
                            gpio_used = true;
                            gpio.gpioWrite(address, (ushort)data);
                        }
                    }
                    return;

                case 0xD:
                    if (EEPROMEnabled)
                    {
                        EEProm.write((byte)data);
                    }
                    return;

                case 0xE:
                case 0xF:
                    if (!EEPROMEnabled | FlashEnabled | SramEnabled)
                    {
                        SaveGameFunc(address + offset + blockcmd_lowerbits, (byte)data);
                    }
                    return;
            }
        }

        public static void write_dword(UInt32 address, UInt32 data)
        {
#if DEBUG
            writecnt32[(address >> 24) & 0xF]++;
            if (enable_debugout && address >= 0x04FFF600 & address < 0x04FFF700)
            {
                debug_out.Add((char)(data & 0xFF));
                debug_out.Add((char)((data >> 8) & 0xFF));
                debug_out.Add((char)((data >> 16) & 0xFF));
                debug_out.Add((char)((data >> 24) & 0xFF));
            }
            if (enable_debugout && address == 0x04FFF700)
            {
                int a = 5;
            }
#endif
            byte offset = (byte)(address & 3);
            address = address & 0xFFFFFFFC;
            uint adr;

            byte select = (byte)(address >> 24);
            switch (select)
            {
                case 2:
                    adr = address & 0x03FFFF;
                    WRAM_Large[adr] = (byte)(data & 0xFF);
                    WRAM_Large[adr + 1] = (byte)((data >> 8) & 0xFF);
                    WRAM_Large[adr + 2] = (byte)((data >> 16) & 0xFF);
                    WRAM_Large[adr + 3] = (byte)((data >> 24) & 0xFF);
                    return;

                case 3:
                    adr = address & 0x7FFF;
                    WRAM_Small[adr] = (byte)(data & 0xFF);
                    WRAM_Small[adr + 1] = (byte)((data >> 8) & 0xFF);
                    WRAM_Small[adr + 2] = (byte)((data >> 16) & 0xFF);
                    WRAM_Small[adr + 3] = (byte)((data >> 24) & 0xFF);
                    return;

                case 4:
                    if (address < 0x04000400)
                    {
                        adr = address & 0x3FF;

                        GBRegs.data[adr] = (byte)(data & 0xFF);
                        GBRegs.data[adr + 1] = (byte)((data >> 8) & 0xFF);
                        GBRegs.data[adr + 2] = (byte)((data >> 16) & 0xFF);
                        GBRegs.data[adr + 3] = (byte)((data >> 24) & 0xFF);

                        write_gbreg(adr, data, true);
                        write_gbreg(adr + 2, data, true);
                    }
                    return;

                case 5:
                    adr = address & 0x3FF;
                    PaletteRAM[adr] = (byte)(data & 0xFF);
                    PaletteRAM[adr + 1] = (byte)((data >> 8) & 0xFF);
                    PaletteRAM[adr + 2] = (byte)((data >> 16) & 0xFF);
                    PaletteRAM[adr + 3] = (byte)((data >> 24) & 0xFF);
                    return;

                case 6:
                    adr = address & 0x1FFFF;
                    if (GPU.videomode < 3 || ((address & 0x1C000) != 0x18000))
                    {
                        if (adr > 0x17FFF) { adr -= 0x8000; }
                        VRAM[adr] = (byte)(data & 0xFF);
                        VRAM[adr + 1] = (byte)((data >> 8) & 0xFF);
                        VRAM[adr + 2] = (byte)((data >> 16) & 0xFF);
                        VRAM[adr + 3] = (byte)((data >> 24) & 0xFF);
                    }
                    return;

                case 7:
                    adr = address & 0x3FF;
                    OAMRAM[adr] = (byte)(data & 0xFF);
                    OAMRAM[adr + 1] = (byte)((data >> 8) & 0xFF);
                    OAMRAM[adr + 2] = (byte)((data >> 16) & 0xFF);
                    OAMRAM[adr + 3] = (byte)((data >> 24) & 0xFF);
                    return;

                case 0x8:
                    if (gpio_enable)
                    {
                        if (address >= 0x80000c4 && address <= 0x80000c8)
                        {
                            gpio_used = true;
                            gpio.gpioWrite(address, (ushort)data);
                        }
                    }
                    return;

                case 0xD:
                    if (EEPROMEnabled)
                    {
                        EEProm.write((byte)data);
                    }
                    return;

                case 0xE:
                case 0xF:
                    if (!EEPROMEnabled | FlashEnabled | SramEnabled)
                    {
                        SaveGameFunc(address + offset + blockcmd_lowerbits, (byte)data);
                    }
                    return;
            }
        }

        public static void prepare_read_gbreg(UInt32 adr)
        {
            if (adr == GBRegs.Sect_timer.TM0CNT_L.address) 
            { 
                UInt16 value = Timer.timers[0].retval; 
                GBRegs.data[adr] = (byte)(value & 0xFF); 
                GBRegs.data[adr + 1] = (byte)((value >> 8) & 0xFF); 
            }
            else if (adr == GBRegs.Sect_timer.TM1CNT_L.address) 
            { 
                UInt16 value = Timer.timers[1].retval; 
                GBRegs.data[adr] = (byte)(value & 0xFF); GBRegs.data[adr + 1] = (byte)((value >> 8) & 0xFF); 
            }
            else if (adr == GBRegs.Sect_timer.TM2CNT_L.address) 
            { 
                UInt16 value = Timer.timers[2].retval; GBRegs.data[adr] = (byte)(value & 0xFF); 
                GBRegs.data[adr + 1] = (byte)((value >> 8) & 0xFF); 
            }
            else if (adr == GBRegs.Sect_timer.TM3CNT_L.address)
            {
                UInt16 value = Timer.timers[3].retval; 
                GBRegs.data[adr] = (byte)(value & 0xFF); GBRegs.data[adr + 1] = (byte)((value >> 8) & 0xFF); 
            }
            
            else if (adr == GBRegs.Sect_sound.SOUNDCNT_X.address) 
            {
                GBRegs.data[adr] = (byte)(GBRegs.data[adr] & 0x80);
                if (Sound.soundGenerator.soundchannels[0].on && Sound.soundGenerator.enable_channels_left[0] || Sound.soundGenerator.enable_channels_right[0]) { GBRegs.data[adr] |= 0x01; }
                if (Sound.soundGenerator.soundchannels[1].on && Sound.soundGenerator.enable_channels_left[1] || Sound.soundGenerator.enable_channels_right[1]) { GBRegs.data[adr] |= 0x02; }
                if (Sound.soundGenerator.soundchannels[2].on && Sound.soundGenerator.enable_channels_left[2] || Sound.soundGenerator.enable_channels_right[2]) { GBRegs.data[adr] |= 0x04; }
                if (Sound.soundGenerator.soundchannels[3].on && Sound.soundGenerator.enable_channels_left[3] || Sound.soundGenerator.enable_channels_right[3]) { GBRegs.data[adr] |= 0x08; }
            }
        }

        public static void write_gbreg(UInt32 adr, UInt32 value, bool dwaccess)
        {
            if (adr == GBRegs.Sect_display.DISPCNT.address) { GPU.dispcnt_write(); }
            if (adr == GBRegs.Sect_display.DISPSTAT.address) { GPU_Timing.dispstat_write(); }
            else if (adr == GBRegs.Sect_display.BG2RefX.address) { GPU.refpoint_update_2x_new(); }
            else if (adr == GBRegs.Sect_display.BG2RefX.address + 2) { GPU.refpoint_update_2x_new(); }
            else if (adr == GBRegs.Sect_display.BG2RefY.address) { GPU.refpoint_update_2y_new(); }
            else if (adr == GBRegs.Sect_display.BG2RefY.address + 2) { GPU.refpoint_update_2y_new(); }
            else if (adr == GBRegs.Sect_display.BG3RefX.address) { GPU.refpoint_update_3x_new(); }
            else if (adr == GBRegs.Sect_display.BG3RefX.address + 2) { GPU.refpoint_update_3x_new(); }
            else if (adr == GBRegs.Sect_display.BG3RefY.address) { GPU.refpoint_update_3y_new(); }
            else if (adr == GBRegs.Sect_display.BG3RefY.address + 2) { GPU.refpoint_update_3y_new(); }

            else if (adr >= GBRegs.Sect_sound.SOUND1CNT_L.address && adr < GBRegs.Sect_sound.FIFO_A.address) 
            { 
                Sound.set_soundreg(adr); 
            }

            else if (adr == GBRegs.Sect_sound.FIFO_A.address) { SoundDMA.fill_fifo(0, value, dwaccess); }
            else if (adr == GBRegs.Sect_sound.FIFO_B.address) { SoundDMA.fill_fifo(1, value, dwaccess); }

            else if (adr == GBRegs.Sect_serial.SIOCNT.address) { Serial.write_SIOCNT(BitConverter.ToUInt16(GBRegs.data, (int)GBRegs.Sect_serial.SIOCNT.address)); }

            else if (adr == GBRegs.Sect_timer.TM0CNT_L.address) { Timer.set_reload(0); }
            else if (adr == GBRegs.Sect_timer.TM0CNT_L.address + 2) { Timer.set_settings(0); }
            else if (adr == GBRegs.Sect_timer.TM1CNT_L.address) { Timer.set_reload(1); }
            else if (adr == GBRegs.Sect_timer.TM1CNT_L.address + 2) { Timer.set_settings(1); }
            else if (adr == GBRegs.Sect_timer.TM2CNT_L.address) { Timer.set_reload(2); }
            else if (adr == GBRegs.Sect_timer.TM2CNT_L.address + 2) { Timer.set_settings(2); }
            else if (adr == GBRegs.Sect_timer.TM3CNT_L.address) { Timer.set_reload(3); }
            else if (adr == GBRegs.Sect_timer.TM3CNT_L.address + 2) { Timer.set_settings(3); }

            else if (adr == GBRegs.Sect_dma.DMA0CNT_H.address + 2) { DMA.set_settings(0); }
            else if (adr == GBRegs.Sect_dma.DMA1CNT_H.address + 2) { DMA.set_settings(1); }
            else if (adr == GBRegs.Sect_dma.DMA2CNT_H.address + 2) { DMA.set_settings(2); }
            else if (adr == GBRegs.Sect_dma.DMA3CNT_H.address + 2) { DMA.set_settings(3); }

            else if (adr == GBRegs.Sect_keypad.KEYINPUT.address) { Joypad.set_reg(); }

            else if (adr == GBRegs.Sect_system.IME.address) { IRP.update_IME(BitConverter.ToUInt16(GBRegs.data, (int)GBRegs.Sect_system.IME.address)); }
            else if (adr == GBRegs.Sect_system.IE.address) { IRP.update_IE(); }
            else if (adr == GBRegs.Sect_system.IF.address + 2) { IRP.clear_irp_bits(); }

            else if (adr == GBRegs.Sect_system.WAITCNT.address) { BusTiming.update(BitConverter.ToUInt16(GBRegs.data, (int)GBRegs.Sect_system.WAITCNT.address)); }
            else if (adr == GBRegs.Sect_system.POSTFLG.address & value == 1) { }
            else if (adr == GBRegs.Sect_system.HALTCNT.address && !gameboy.loading_state) 
            { 
                if ((GBRegs.Sect_system.HALTCNT.read() & 0x80) == 0x80) 
                    CPU.stop = true; 
                else 
                    CPU.halt = true; 
            }
        }

    }
}
