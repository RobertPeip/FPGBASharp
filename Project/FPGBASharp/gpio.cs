using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public static class gpio
    {
        public enum GPIOState
        {
            IDLE = 0,
            COMMAND,
            DATA,
            READDATA
        };

        public class GPIOData
        {
            public byte retval; // 4 bit
            public byte select; // 4 bit
            public byte enable; // 1 bit
            public byte command; // 8 bit
            public int dataLen; // 3 bit
            public int bits; // 6 bit (0..55/56)
            public GPIOState state; // 2 bit
            public byte[] data;     // 7 * 8 bit 
            public byte clockslow; // 8 bit

            public GPIOData()
            {
                data = new byte[7];
            }
        }

        class Gbatime
        {
            public byte tm_year; // 0 = 2000
            public byte tm_mon;// 1..12
            public byte tm_mday; // 1 .. 31
            public byte tm_wday; // 0..6
            public byte tm_hour; //0..23 / 0..11
            public byte tm_min; // 0..59
            public byte tm_sec; // 0..59
        }

        static Gbatime gba_time;
        public static GPIOData gpioData;
        static bool gpioEnabled = true;
        static bool rumbleEnabled = false;

        public static byte solar = 0xE9;

        public const ushort GYRO_MIN = 0xAC1;
        public const ushort GYRO_MIDDLE = 0xD80;
        public const ushort GYRO_MAX = 0xFFF;
        public static ushort gyro = GYRO_MIDDLE;

        //static uint countTicks = 0;

        public static void rtcEnable(bool e)
        {
            gpioEnabled = e;
            //System.IO.StreamWriter writer = new System.IO.StreamWriter("gpio_test.txt");
            //writer.Close();
        }

        static void rtcEnableRumble(bool e)
        {
            rumbleEnabled = e;
        }

        static byte systemGetSensorDarkness()
        {
            return solar;
        }

        static ushort systemGetSensorZ()
        {
            return gyro;
        }

        static void systemCartridgeRumble(bool rumble)
        {

        }

        static byte toBCD(byte value)
        {
            value = (byte)(value % 100);
            int l = value % 10;
            int h = value / 10;
            return (byte)(h * 16 + l);
        }

        static void SetGBATime()
        {
            gba_time.tm_year = 7;
            gba_time.tm_mon = 1;
            gba_time.tm_mday = 2;
            gba_time.tm_wday = 3;
            gba_time.tm_hour = 4;
            gba_time.tm_min = 5;
            gba_time.tm_sec = 6;
        }

        //static void rtcUpdateTime(int ticks)
        //{
        //    countTicks = (uint)(countTicks + ticks);
        //
        //    const uint TICKS_PER_SECOND = 16777216;
        //
        //    if (countTicks > TICKS_PER_SECOND)
        //    {
        //        countTicks -= TICKS_PER_SECOND;
        //        gba_time.tm_sec++;
        //        if (gba_time.tm_sec > 59) { gba_time.tm_sec = 0; gba_time.tm_min++; }
        //    }
        //}

        public static ushort gpioRead(uint address)
        {
            ushort res = 0;

            switch (address)
            {
                case 0x80000c8:
                    res = gpioData.enable;
                    break;

                case 0x80000c6:
                    res = gpioData.select;
                    break;

                case 0x80000c4:
                    if ((gpioData.enable & 1) == 0)
                    {
                        res = 0;
                    }
                    else
                    {
                        // Boktai Solar Sensor
                        if (gpioData.select == 0x07)
                        {
                            if (gpioData.clockslow >= systemGetSensorDarkness())
                            {
                                res |= 8;
                            }
                        }

                        // WarioWare Twisted Tilt Sensor
                        if (gpioData.select == 0x0b)
                        {
                            ushort v = systemGetSensorZ();
                            res |= (ushort)(((v >> gpioData.clockslow) & 1) << 2);
                        }

                        // Real Time Clock
                        if (res == 0 && gpioEnabled && ((gpioData.select & 0x04) > 0))
                        {
                            res |= gpioData.retval;
                        }
                    }

                    break;
            }

            //System.IO.StreamWriter writer = new System.IO.StreamWriter("gpio_test.txt", true);
            //writer.WriteLine("compare_gbbus_16bit(" + res + ", 0x" + address.ToString("X8") + ") --" + gpioData.select + " | " + gpioData.bits + " | " + gpioData.clockslow + " | x" + gpioData.command.ToString("X2") + " | " + gpioData.state + " | " + gpioData.dataLen);
            //writer.Close();

            return res;
        }

        public static bool gpioWrite(uint address, ushort value)
        {
            //System.IO.StreamWriter writer = new System.IO.StreamWriter("gpio_test.txt", true);
            //writer.WriteLine("write_gbbus_16bit(" + value + ", 0x" + address.ToString("X8") + ") --" + gpioData.select + " | " + gpioData.bits + " | " + gpioData.clockslow + " | x" + gpioData.command.ToString("X2") + " | " + gpioData.state + " | " + gpioData.dataLen);
            //writer.Close();

            byte oldretval = gpioData.retval;

            if (address == 0x80000c8)
            {
                gpioData.enable = (byte)value; // bit 0 = enable reading from 0x80000c4 c6 and c8
            }
            else if (address == 0x80000c6)
            {
                gpioData.select = (byte)value; // 0=read/1=write (for each of 4 low bits)

                // rumble is off when not writing to that pin
                if (rumbleEnabled && (value & 8) == 0)
                    systemCartridgeRumble(false);
            }
            else if (address == 0x80000c4) // 4 bits of I/O Port Data (upper bits not used)
            {
                // WarioWare Twisted rumble
                if (rumbleEnabled && (gpioData.select & 0x08) > 0)
                {
                    systemCartridgeRumble((value & 8) > 0);
                }

                // Boktai solar sensor
                if (gpioData.select == 0x07)
                {
                    if ((value & 2) > 0)
                    {
                        gpioData.clockslow = 0;
                    }
                    else if ((value & 1) > 0)
                    {
                        gpioData.clockslow++; // 8 bit wraparound
                    }
                }

                // WarioWare Twisted rotation sensor
                if (gpioData.select == 0x0b)
                {
                    if ((value & 2) > 0)
                    {
                        // clock goes high in preperation for reading a bit
                        gpioData.clockslow--;
                    }

                    if ((value & 1) > 0)
                    {
                        // start ADC conversion
                        gpioData.clockslow = 15;
                    }

                    gpioData.retval = (byte)(value & gpioData.select);
                }

                // Real Time Clock
                if ((gpioData.select & 4) > 0)
                {
                    if (gpioData.state == GPIOState.IDLE && gpioData.retval == 1 && value == 5)
                    {
                        gpioData.state = GPIOState.COMMAND;
                        gpioData.bits = 0;
                        gpioData.command = 0;
                    }
                    else if ((gpioData.retval & 1) == 0 && (value & 1) > 0) // bit transfer
                    {
                        gpioData.retval = (byte)value;

                        switch (gpioData.state)
                        {
                            case GPIOState.COMMAND:
                                gpioData.command |= (byte)(((value & 2) >> 1) << (7 - gpioData.bits));
                                gpioData.bits++;

                                if (gpioData.bits == 8)
                                {
                                    gpioData.bits = 0;

                                    switch (gpioData.command)
                                    {
                                        case 0x60:
                                            // not sure what this command does but it doesn't take parameters
                                            // maybe it is a reset or stop
                                            gpioData.state = GPIOState.IDLE;
                                            gpioData.bits = 0;
                                            break;

                                        case 0x62:
                                            // this sets the control state but not sure what those values are
                                            gpioData.state = GPIOState.READDATA;
                                            gpioData.dataLen = 1;
                                            break;

                                        case 0x63:
                                            gpioData.dataLen = 1;
                                            gpioData.data[0] = 0x40;
                                            gpioData.state = GPIOState.DATA;
                                            break;

                                        case 0x64:
                                            break;

                                        case 0x65:
                                            {
                                                if (gpioEnabled)
                                                    SetGBATime();

                                                gpioData.dataLen = 7;
                                                gpioData.data[0] = toBCD(gba_time.tm_year);
                                                gpioData.data[1] = toBCD(gba_time.tm_mon);
                                                gpioData.data[2] = toBCD(gba_time.tm_mday);
                                                gpioData.data[3] = toBCD(gba_time.tm_wday);
                                                gpioData.data[4] = toBCD(gba_time.tm_hour);
                                                gpioData.data[5] = toBCD(gba_time.tm_min);
                                                gpioData.data[6] = toBCD(gba_time.tm_sec);
                                                gpioData.state = GPIOState.DATA;
                                            }
                                            break;

                                        case 0x67:
                                            {
                                                if (gpioEnabled)
                                                    SetGBATime();

                                                gpioData.dataLen = 3;
                                                gpioData.data[0] = toBCD(gba_time.tm_hour);
                                                gpioData.data[1] = toBCD(gba_time.tm_min);
                                                gpioData.data[2] = toBCD(gba_time.tm_sec);
                                                gpioData.state = GPIOState.DATA;
                                            }
                                            break;

                                        default:
                                            gpioData.state = GPIOState.IDLE;
                                            break;
                                    }
                                }

                                break;

                            case GPIOState.DATA:
                                if ((gpioData.select & 2) > 0)
                                {
                                }
                                else if ((gpioData.select & 4) > 0)
                                {
                                    gpioData.retval = (byte)(gpioData.retval & ~2); // clear bit 1
                                    gpioData.retval |= (byte)(((gpioData.data[gpioData.bits / 8] >> (gpioData.bits & 7)) & 1) * 2); // set new bit 1 value
                                    gpioData.bits++;

                                    if (gpioData.bits == 8 * gpioData.dataLen)
                                    {
                                        gpioData.bits = 0;
                                        gpioData.state = GPIOState.IDLE;
                                    }
                                }

                                break;

                            case GPIOState.READDATA:
                                if ((gpioData.select & 2) > 0)
                                {
                                    gpioData.data[gpioData.bits >> 3] = (byte)((gpioData.data[gpioData.bits >> 3] >> 1) | ((value << 6) & 128));
                                    gpioData.bits++;

                                    if (gpioData.bits == 8 * gpioData.dataLen)
                                    {
                                        gpioData.bits = 0;
                                        gpioData.state = GPIOState.IDLE;
                                    }
                                }

                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        gpioData.retval = (byte)value;
                    }
                }
            }

            return true;
        }

        public static void rtcReset()
        {
            gba_time = new Gbatime();
            gpioData = new GPIOData();
            gpioData.retval = 0;
            gpioData.select = 0;
            gpioData.enable = 0;
            gpioData.command = 0;
            gpioData.dataLen = 0;
            gpioData.bits = 0;
            gpioData.state = GPIOState.IDLE;
            gpioData.clockslow = 0;
            SetGBATime();
        }
    }

}
