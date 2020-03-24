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
            public byte tm_year_high; // 0 = 2000           4 bit   4
            public byte tm_year_low;  // 0 = 2000           4 bit   8
            public byte tm_mon_high;  // 1..12              1 bit   9
            public byte tm_mon_low;   // 1..12              4 bit   13
            public byte tm_mday_high; // 1 .. 31            2 bit   15
            public byte tm_mday_low;  // 1 .. 31            4 bit   19
            public byte tm_wday_high; // 0..6               0 bit   19
            public byte tm_wday_low;  // 0..6               3 bit   22
            public byte tm_hour_high; //0..23 / 0..11       2 bit   24
            public byte tm_hour_low;  //0..23 / 0..11       4 bit   28
            public byte tm_min_high;  // 0..59              3 bit   31
            public byte tm_min_low;   // 0..59              4 bit   35
            public byte tm_sec_high;  // 0..59              3 bit   38
            public byte tm_sec_low;   // 0..59              4 bit   42
        }

        static Gbatime gba_time;
        public static GPIOData gpioData;
        static bool gpioEnabled = true;
        static bool rumbleEnabled = false;

        public static byte solar = 0xE9;

        public static bool saveRTC = false;

        public const ushort GYRO_MIN = 0xAC1;
        public const ushort GYRO_MIDDLE = 0xD80;
        public const ushort GYRO_MAX = 0xFFF;
        public static ushort gyro = GYRO_MIDDLE;

        public static int framecount;

        public static int diffSeconds;

        //static uint countTicks = 0;

        public static void reset()
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

            framecount = 0;

            gba_time.tm_year_high = 0; //0;
            gba_time.tm_year_low  = 0; //9;
            gba_time.tm_mon_high  = 0; //1;
            gba_time.tm_mon_low   = 0; //2;
            gba_time.tm_mday_high = 0; //3;
            gba_time.tm_mday_low  = 0; //1;
            gba_time.tm_wday_high = 0; //0;
            gba_time.tm_wday_low  = 0; //6;
            gba_time.tm_hour_high = 0; //2;
            gba_time.tm_hour_low  = 0; //3;
            gba_time.tm_min_high  = 0; //5;
            gba_time.tm_min_low   = 0; //9;
            gba_time.tm_sec_high  = 0; //4;
            gba_time.tm_sec_low   = 0; //5;

            diffSeconds = 0; // 3600;
        }

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

        public static void update_time()
        {
            bool anychange = false;

            if (gba_time.tm_year_high > 9) { gba_time.tm_year_high = 0; anychange = true; }
            if (gba_time.tm_year_low  > 9) { gba_time.tm_year_low  = 0;  gba_time.tm_year_high++; anychange = true; }

            byte month = (byte)((gba_time.tm_mon_high << 4) | gba_time.tm_mon_low);
            if (month == 0x13) { gba_time.tm_mon_high = 0; gba_time.tm_mon_low = 1; gba_time.tm_year_low++; anychange = true; }
            if (gba_time.tm_mon_low > 9) { gba_time.tm_mon_low = 0;  gba_time.tm_mon_high ++; anychange = true; }

            byte mday = (byte)((gba_time.tm_mday_high << 4) | gba_time.tm_mday_low);
            switch (month)
            {
                case 0x01: if (mday > 0x31) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x02: if (mday > 0x28) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x03: if (mday > 0x31) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x04: if (mday > 0x30) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x05: if (mday > 0x31) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x06: if (mday > 0x30) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x07: if (mday > 0x31) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x08: if (mday > 0x31) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x09: if (mday > 0x30) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x10: if (mday > 0x31) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x11: if (mday > 0x30) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
                case 0x12: if (mday > 0x31) { gba_time.tm_mday_high = 0; gba_time.tm_mday_low = 1; gba_time.tm_mon_low++; anychange = true; }  break;
            }
            if (gba_time.tm_mday_low  > 9) { gba_time.tm_mday_low  = 0;  gba_time.tm_mday_high++; anychange = true; }

            if (gba_time.tm_wday_low > 6) { gba_time.tm_wday_low = 0; anychange = true; }

            byte hour = (byte)((gba_time.tm_hour_high << 4) | gba_time.tm_hour_low);
            if (hour > 0x23) { gba_time.tm_hour_high = 0; gba_time.tm_hour_low = 0; gba_time.tm_wday_low++; gba_time.tm_mday_low++; anychange = true; }
            if (gba_time.tm_hour_low  > 9) { gba_time.tm_hour_low  = 0;  gba_time.tm_hour_high++; anychange = true; }
            if (gba_time.tm_min_high  > 5) { gba_time.tm_min_high  = 0;  gba_time.tm_hour_low++;  anychange = true; }
            if (gba_time.tm_min_low   > 9) { gba_time.tm_min_low   = 0;  gba_time.tm_min_high++;  anychange = true; }
            if (gba_time.tm_sec_high  > 5) { gba_time.tm_sec_high  = 0;  gba_time.tm_min_low++;   anychange = true; }
            if (gba_time.tm_sec_low   > 9) { gba_time.tm_sec_low   = 0;  gba_time.tm_sec_high++;  anychange = true; }
            if (framecount > 59)           { framecount = 0;             gba_time.tm_sec_low++;   anychange = true; }
            if (diffSeconds > 0)           { diffSeconds--;              gba_time.tm_sec_low++;   anychange = true; }

            if (anychange)
            {
                update_time();
            }
        }

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

            System.IO.StreamWriter writer = new System.IO.StreamWriter("gpio_test.txt", true);
            writer.WriteLine("compare_gbbus_16bit(" + res + ", 0x" + address.ToString("X8") + ") --" + gpioData.select + " | " + gpioData.bits + " | " + gpioData.clockslow + " | x" + gpioData.command.ToString("X2") + " | " + gpioData.state + " | " + gpioData.dataLen);
            writer.Close();

            return res;
        }

        public static bool gpioWrite(uint address, ushort value)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter("gpio_test.txt", true);
            writer.WriteLine("write_gbbus_16bit(" + value + ", 0x" + address.ToString("X8") + ") --" + gpioData.select + " | " + gpioData.bits + " | " + gpioData.clockslow + " | x" + gpioData.command.ToString("X2") + " | " + gpioData.state + " | " + gpioData.dataLen);
            writer.Close();

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
                                            gpioData.state = GPIOState.READDATA;
                                            gpioData.dataLen = 7;
                                            saveRTC = true;
                                            break;

                                        case 0x65:
                                            {
                                                update_time();
                                                gpioData.dataLen = 7;
                                                gpioData.data[0] = (byte)((gba_time.tm_year_high << 4) | gba_time.tm_year_low);
                                                gpioData.data[1] = (byte)((gba_time.tm_mon_high << 4) | gba_time.tm_mon_low); 
                                                gpioData.data[2] = (byte)((gba_time.tm_mday_high << 4) | gba_time.tm_mday_low); 
                                                gpioData.data[3] = (byte)((gba_time.tm_wday_high << 4) | gba_time.tm_wday_low); 
                                                gpioData.data[4] = (byte)((gba_time.tm_hour_high << 4) | gba_time.tm_hour_low); 
                                                gpioData.data[5] = (byte)((gba_time.tm_min_high << 4) | gba_time.tm_min_low); 
                                                gpioData.data[6] = (byte)((gba_time.tm_sec_high << 4) | gba_time.tm_sec_low); 
                                                gpioData.state = GPIOState.DATA;
                                            }
                                            break;

                                        case 0x67:
                                            {
                                                update_time();
                                                gpioData.dataLen = 3;
                                                gpioData.data[0] = (byte)((gba_time.tm_hour_high << 4) | gba_time.tm_hour_low); 
                                                gpioData.data[1] = (byte)((gba_time.tm_min_high << 4) | gba_time.tm_min_low); 
                                                gpioData.data[2] = (byte)((gba_time.tm_sec_high << 4) | gba_time.tm_sec_low);
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
                                        if (saveRTC)
                                        {
                                            gba_time.tm_year_high = (byte)(gpioData.data[0] >> 4);
                                            gba_time.tm_year_low  = (byte)(gpioData.data[0] & 0xF);
                                            gba_time.tm_mon_high  = (byte)(gpioData.data[1] >> 4);
                                            gba_time.tm_mon_low   = (byte)(gpioData.data[1] & 0xF);
                                            gba_time.tm_mday_high = (byte)(gpioData.data[2] >> 4);
                                            gba_time.tm_mday_low  = (byte)(gpioData.data[2] & 0xF);
                                            gba_time.tm_wday_high = (byte)(gpioData.data[3] >> 4);
                                            gba_time.tm_wday_low  = (byte)(gpioData.data[3] & 0xF);
                                            gba_time.tm_hour_high = (byte)(gpioData.data[4] >> 4);
                                            gba_time.tm_hour_low  = (byte)(gpioData.data[4] & 0xF);
                                            gba_time.tm_min_high  = (byte)(gpioData.data[5] >> 4);
                                            gba_time.tm_min_low   = (byte)(gpioData.data[5] & 0xF);
                                            gba_time.tm_sec_high  = (byte)(gpioData.data[6] >> 4);
                                            gba_time.tm_sec_low   = (byte)(gpioData.data[6] & 0xF);
                                            saveRTC = false;
                                        }
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

    }

}
