using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public static class Serial
    {
        public static Int32 serialcnt;

        public static bool IRP_enable;
        public static bool start;
        public static bool hispeed;
        public static int bits;
        public static int bitcount;

        public static void reset()
        {
            serialcnt = 25;
            IRP_enable = false;
        }

        public static void work()
        {
            if (start)
            {
                serialcnt += CPU.newticks;

                if ((!hispeed && serialcnt >= 64) || (hispeed && serialcnt >= 8))
                {
                    if (hispeed)
                    {
                        serialcnt -= 8;
                    }
                    else
                    {
                        serialcnt -= 64;
                    }
                    bitcount++;
                    if (bitcount == bits)
                    {
                        if (IRP_enable)
                        {
                            IRP.set_irp_bit(IRP.IRPMASK_Serial);
                        }
                        GBRegs.Sect_serial.SIOCNT.mask(0xDF7F);
                        start = false;
                    }
                }
            }
        }

        public static void write_SIOCNT(UInt16 value)
        {
            IRP_enable = ((value >> 14) & 1) == 1;

            hispeed = ((value >> 1) & 1) == 1;

            start = ((value >> 7) & 1) == 1;
            if (((value >> 12) & 1) == 1)
            {
                bits = 32;
            }
            else
            {
                bits = 8;
            }
            if (start)
            {
                bitcount = 0;
                serialcnt = 0;
            }
        }



    }
}
