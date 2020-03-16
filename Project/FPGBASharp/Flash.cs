using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public static class Flash
    {
        public enum FLASHSTATE
        {
            FLASH_READ_ARRAY,
            FLASH_CMD_1,
            FLASH_CMD_2,
            FLASH_AUTOSELECT,
            FLASH_CMD_3,
            FLASH_CMD_4,
            FLASH_CMD_5,
            FLASH_ERASE_COMPLETE,
            FLASH_PROGRAM,
            FLASH_SETBANK
        }

        public enum SAVETYPE
        {
            GBA_SAVE_AUTO,
            GBA_SAVE_EEPROM,
            GBA_SAVE_SRAM,
            GBA_SAVE_FLASH,
            GBA_SAVE_EEPROM_SENSOR,
            GBA_SAVE_NONE
        }

        const int SIZE_FLASH1M = 131072;
        const int SIZE_FLASH512 = 65536;

        public static byte[] flashSaveMemory;
        //public static byte[] SRAM;

        public static SAVETYPE saveType;

        public static FLASHSTATE flashState;
        public static FLASHSTATE flashReadState;
        private static int flashSize;
        private static byte flashDeviceID;
        private static byte flashManufacturerID;
        public static byte flashBank = 0;

        public static void reset()
        {
            //SRAM = new Byte[65536];

            flashSaveMemory = new byte[SIZE_FLASH1M];
            for (int i = 0; i < flashSaveMemory.Length; i++)
            {
                flashSaveMemory[i] = 0xFF;
            }

            flashState = FLASHSTATE.FLASH_READ_ARRAY;
            flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
            flashSize = SIZE_FLASH512;
            flashDeviceID = 0x1b;
            flashManufacturerID = 0x32;
            flashBank = 0;
        }

        public static void flashSetSize(int size)
        {
            if (size == SIZE_FLASH512)
            {
                flashDeviceID = 0x1b;
                flashManufacturerID = 0x32;
            }
            else
            {
                flashDeviceID = 0x13; //0x09;
                flashManufacturerID = 0x62; //0xc2;
            }
            flashSize = size;
        }

        public static byte flashRead(UInt32 address)
        {
            address &= 0xFFFF;

            switch (flashReadState)
            {
                case FLASHSTATE.FLASH_READ_ARRAY:
                    return flashSaveMemory[(flashBank << 16) + address];
                case FLASHSTATE.FLASH_AUTOSELECT:
                    switch (address & 0xFF)
                    {
                        case 0:
                            return flashManufacturerID;
                        case 1:
                            return flashDeviceID;
                    }
                    break;
                case FLASHSTATE.FLASH_ERASE_COMPLETE:
                    flashState = FLASHSTATE.FLASH_READ_ARRAY;
                    flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                    return 0xFF;
            };
            return 0;
        }

        public static object flashSaveDecide(UInt32 address, byte data)
        {
            if (saveType == SAVETYPE.GBA_SAVE_EEPROM)
            {
                return 0;
            }

            if (Memory.SramEnabled && Memory.FlashEnabled)
            {
                if (address == 0x0e005555)
                {
                    saveType = SAVETYPE.GBA_SAVE_FLASH;
                    Memory.SramEnabled = false;
                    Memory.SaveGameFunc = flashWrite;
                }
                else
                {
                    saveType = SAVETYPE.GBA_SAVE_SRAM;
                    Memory.FlashEnabled = false;
                    Memory.SaveGameFunc = sramWrite;
                }
            }

            Memory.SaveGameFunc(address, data);

            return 0;
        }

        public static object sramWrite(UInt32 address, byte data)
        {
            flashSaveMemory[address & 0xFFFF] = data;

            return 0;
        }

        public static object flashWrite(UInt32 address, byte data)
        {
            address &= 0xFFFF;
            switch (flashState)
            {
                case FLASHSTATE.FLASH_READ_ARRAY:
                    if (address == 0x5555 && data == 0xAA)
                        flashState = FLASHSTATE.FLASH_CMD_1;
                    break;
                case FLASHSTATE.FLASH_CMD_1:
                    if (address == 0x2AAA && data == 0x55)
                        flashState = FLASHSTATE.FLASH_CMD_2;
                    else
                        flashState = FLASHSTATE.FLASH_READ_ARRAY;
                    break;
                case FLASHSTATE.FLASH_CMD_2:
                    if (address == 0x5555)
                    {
                        if (data == 0x90)
                        {
                            flashState = FLASHSTATE.FLASH_AUTOSELECT;
                            flashReadState = FLASHSTATE.FLASH_AUTOSELECT;
                        }
                        else if (data == 0x80)
                        {
                            flashState = FLASHSTATE.FLASH_CMD_3;
                        }
                        else if (data == 0xF0)
                        {
                            flashState = FLASHSTATE.FLASH_READ_ARRAY;
                            flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                        }
                        else if (data == 0xA0)
                        {
                            flashState = FLASHSTATE.FLASH_PROGRAM;
                        }
                        else if (data == 0xB0 && flashSize == SIZE_FLASH1M)
                        {
                            flashState = FLASHSTATE.FLASH_SETBANK;
                        }
                        else
                        {
                            flashState = FLASHSTATE.FLASH_READ_ARRAY;
                            flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                        }
                    }
                    else
                    {
                        flashState = FLASHSTATE.FLASH_READ_ARRAY;
                        flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                    }
                    break;
                case FLASHSTATE.FLASH_CMD_3:
                    if (address == 0x5555 && data == 0xAA)
                    {
                        flashState = FLASHSTATE.FLASH_CMD_4;
                    }
                    else
                    {
                        flashState = FLASHSTATE.FLASH_READ_ARRAY;
                        flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                    }
                    break;
                case FLASHSTATE.FLASH_CMD_4:
                    if (address == 0x2AAA && data == 0x55)
                    {
                        flashState = FLASHSTATE.FLASH_CMD_5;
                    }
                    else
                    {
                        flashState = FLASHSTATE.FLASH_READ_ARRAY;
                        flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                    }
                    break;
                case FLASHSTATE.FLASH_CMD_5: // SECTOR ERASE
                    if (data == 0x30)
                    {
                        for (int i = 0; i < 0x1000; i++)
                        {
                            flashSaveMemory[(flashBank << 16) + (address & 0xF000) + i] = 0xFF;
                        }
                        flashReadState = FLASHSTATE.FLASH_ERASE_COMPLETE;
                    }
                    else if (data == 0x10) // CHIP ERASE
                    {
                        for(int i = 0; i < flashSaveMemory.Length; i++)
                        {
                            flashSaveMemory[i] = 0xFF;
                        }
                        flashReadState = FLASHSTATE.FLASH_ERASE_COMPLETE;
                    }
                    else
                    {
                        flashState = FLASHSTATE.FLASH_READ_ARRAY;
                        flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                    }
                    break;
                case FLASHSTATE.FLASH_AUTOSELECT:
                    if (data == 0xF0)
                    {
                        flashState = FLASHSTATE.FLASH_READ_ARRAY;
                        flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                    }
                    else if (address == 0x5555 && data == 0xAA)
                        flashState = FLASHSTATE.FLASH_CMD_1;
                    else
                    {
                        flashState = FLASHSTATE.FLASH_READ_ARRAY;
                        flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                    }
                    break;
                case FLASHSTATE.FLASH_PROGRAM:
                    flashSaveMemory[(flashBank << 16) + address] = data;
                    flashState = FLASHSTATE.FLASH_READ_ARRAY;
                    flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                    break;
                case FLASHSTATE.FLASH_SETBANK:
                    if (address == 0)
                    {
                        flashBank = (byte)(data & 1);
                    }
                    flashState = FLASHSTATE.FLASH_READ_ARRAY;
                    flashReadState = FLASHSTATE.FLASH_READ_ARRAY;
                    break;
            }

            return 0;
        }

    }
}
