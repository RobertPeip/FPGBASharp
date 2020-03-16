using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public static class IRP
    {
        public const int IRPMASK_LCD_V_Blank = 0x1;
        public const int IRPMASK_LCD_H_Blank = 0x2;
        public const int IRPMASK_LCD_V_Counter_Match = 0x4;
        public const int IRPMASK_Timer_0 = 0x8;
        public const int IRPMASK_Timer_1 = 0x10;
        public const int IRPMASK_Timer_2 = 0x20;
        public const int IRPMASK_Timer_3 = 0x40;
        public const int IRPMASK_Serial = 0x80;
        public const int IRPMASK_DMA_0 = 0x100;
        public const int IRPMASK_DMA_1 = 0x200;
        public const int IRPMASK_DMA_2 = 0x400;
        public const int IRPMASK_DMA_3 = 0x800;
        public const int IRPMASK_Keypad = 0x1000;
        public const int IRPMASK_Game_Pak = 0x2000;

        public static bool Master_enable;

        public static UInt16 IRP_Flags;
        public static UInt16 IE;

        public static void set_irp_bit(UInt16 mask)
        {
            //if (Master_enable)
            {
                IRP_Flags |= mask;
                GBRegs.Sect_system.IF.write(IRP_Flags);
            }
        }

        public static void update_IE()
        {
            IE = (UInt16)GBRegs.Sect_system.IE.read();
        } 
        
        public static void clear_irp_bits()
        {
            UInt16 clearvector = (UInt16)GBRegs.Sect_system.IF.read();
            IRP_Flags = (UInt16)(IRP_Flags & (~clearvector));
            GBRegs.Sect_system.IF.write(IRP_Flags);
        }

        public static void update_IME(UInt16 value)
        {
            Master_enable = (value & 1) == 1;
        }

        public static UInt16 get_IF_with_mask()
        {
            return (UInt16)(IRP_Flags & IE);
        }
    }
}
