using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public static class Header
    {
        public static void autocheck()
        {
            check_flash_size();
        }

        private static void check_flash_size()
        {
            byte[] bytes = Encoding.ASCII.GetBytes("FLASH1M_V");
            if (FindArray(Memory.GameRom, bytes))
            {
                Flash.flashSetSize(0x20000);
            }
        }

        private static bool FindArray(byte[] array, byte[] pattern)
        {
            int success = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == pattern[success])
                {
                    success++;
                }
                else
                {
                    success = 0;
                }

                if (pattern.Length == success)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
