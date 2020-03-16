using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gbemu
{
    public class Cheat
    {
        public enum OPTYPE
        {
            ALWAYS,     
            EQUALS,     
            GREATER,    
            LESS,       
            GREATER_EQ, 
            LESS_EQ,    
            NOT_EQ     
        }

        public byte bytemask;
        public OPTYPE optype;
        public UInt32 address;
        public UInt32 compare;
        public UInt32 replace;

        public Cheat(byte bytemask, OPTYPE optype, UInt32 address, UInt32 compare, UInt32 replace)
        {
            this.bytemask = bytemask;
            this.optype = optype;
            this.address = address;
            this.compare = compare;
            this.replace = replace;
        }
    }

    public static class Cheats
    {
        public static object cheatlock = new object();

        private static List<Cheat> cheats = new List<Cheat>();

        public static void add_cheats(byte[] newcheats)
        {
            lock (Cheats.cheatlock)
            {
                for (int i = 0; i < newcheats.Length; i += 16)
                {
                    byte bytemask = (byte)(newcheats[i] >> 4);
                    Cheat.OPTYPE optype = (Cheat.OPTYPE)(newcheats[i] & 0xF);
                    UInt32 address = BitConverter.ToUInt32(newcheats, i + 4);
                    UInt32 compare = BitConverter.ToUInt32(newcheats, i + 8);
                    UInt32 replace = BitConverter.ToUInt32(newcheats, i + 12);
                    cheats.Add(new Cheat(bytemask, optype, address, compare, replace));
                }
            }
        }

        public static void apply_cheats()
        {
            bool skipnext = false;

            lock (Cheats.cheatlock)
            {
                foreach(Cheat cheat in cheats)
                {
                    if (skipnext)
                    {
                        skipnext = false;
                    }
                    else
                    {
                        UInt32 old = Memory.read_dword(cheat.address);

                        switch (cheat.optype)
                        {
                            case Cheat.OPTYPE.ALWAYS:
                                UInt32 newval = old;
                                if ((cheat.bytemask & 1) > 0) { newval &= 0xFFFFFF00; newval |= cheat.replace & 0x000000FF; }
                                if ((cheat.bytemask & 2) > 1) { newval &= 0xFFFF00FF; newval |= cheat.replace & 0x0000FF00; }
                                if ((cheat.bytemask & 4) > 1) { newval &= 0xFF00FFFF; newval |= cheat.replace & 0x00FF0000; }
                                if ((cheat.bytemask & 8) > 1) { newval &= 0x00FFFFFF; newval |= cheat.replace & 0xFF000000; }
                                Memory.write_dword(cheat.address, newval);
                                break;

                            case Cheat.OPTYPE.EQUALS:
                            case Cheat.OPTYPE.GREATER:
                            case Cheat.OPTYPE.GREATER_EQ:
                            case Cheat.OPTYPE.LESS:
                            case Cheat.OPTYPE.LESS_EQ:
                            case Cheat.OPTYPE.NOT_EQ:
                                if ((cheat.bytemask & 1) == 0) { old &= 0xFFFFFF00; }
                                if ((cheat.bytemask & 2) == 0) { old &= 0xFFFF00FF; }
                                if ((cheat.bytemask & 4) == 0) { old &= 0xFF00FFFF; }
                                if ((cheat.bytemask & 8) == 0) { old &= 0x00FFFFFF; }
                                switch(cheat.optype)
                                {
                                    case Cheat.OPTYPE.EQUALS:     if(old != cheat.replace) { skipnext = true; } break;
                                    case Cheat.OPTYPE.GREATER:    if(old <= cheat.replace) { skipnext = true; } break;
                                    case Cheat.OPTYPE.GREATER_EQ: if(old  < cheat.replace) { skipnext = true; } break;
                                    case Cheat.OPTYPE.LESS:       if(old >= cheat.replace) { skipnext = true; } break;
                                    case Cheat.OPTYPE.LESS_EQ:    if(old  > cheat.replace) { skipnext = true; } break;
                                    case Cheat.OPTYPE.NOT_EQ:     if(old == cheat.replace) { skipnext = true; } break;
                                }
                                break;

                            default: MessageBox.Show("Cheat opcode unknown: " + (int)cheat.optype); break;
                        }
                    }
                }
            }
        }
    }
}
