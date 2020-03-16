using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public static class Joypad
    {
        public static bool KeyA;
        public static bool KeyB;
        public static bool KeyL;
        public static bool KeyR;
        public static bool KeyStart;
        public static bool KeySelect;
        public static bool KeyUp;
        public static bool KeyDown;
        public static bool KeyLeft;
        public static bool KeyRight;

        public static bool KeyAToggle;
        public static bool KeyBToggle;

        private static UInt16 value;
        public static UInt16 oldvalue;

        private static int count;

        public static void set_reg()
        {
            count++;
            if (count > 100) // don't check too often to speed up emulation
            {
                count = 0;

                oldvalue = value;
                value = 0x00;

                if (!KeyA) { value |= 1; }
                if (!KeyB) { value |= 2; }
                if (!KeySelect) { value |= 4; }
                if (!KeyStart) { value |= 8; }
                if (!KeyRight) { value |= 16; }
                if (!KeyLeft) { value |= 32; }
                if (!KeyUp) { value |= 64; }
                if (!KeyDown) { value |= 128; }
                if (!KeyR) { value |= 256; }
                if (!KeyL) { value |= 512; }

                if (KeyAToggle && DateTime.Now.Millisecond % 20 < 10) { value &= 0x3FE; }
                if (KeyBToggle && DateTime.Now.Millisecond % 20 < 10) { value &= 0x3FD; }

                if (value != oldvalue)
                {
                    GBRegs.Sect_keypad.KEYINPUT.write(value);
                    check_irp();
                }
            }
        }

        public static void check_irp()
        {
            UInt16 irpmask = (UInt16)GBRegs.Sect_keypad.KEYCNT.read();
            if ((irpmask & 0x4000) > 0)
            {
                if ((irpmask & 0x8000) > 0) // logical and -> all mentioned
                {
                    irpmask &= 0x3FF;
                    UInt16 newvalue = (UInt16)((~value) & irpmask);
                    if (newvalue == irpmask)
                    {
                        IRP.set_irp_bit(IRP.IRPMASK_Keypad);
                    }
                }
                else // logical or -> at least one
                {
                    irpmask &= 0x3FF;
                    UInt16 newvalue = (UInt16)((~value) & irpmask);
                    newvalue &= 0x3FF;
                    if (newvalue > 0)
                    {
                        IRP.set_irp_bit(IRP.IRPMASK_Keypad);
                    }
                }
            }
        }
    }
}
