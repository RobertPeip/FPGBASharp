using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbemu
{
    public enum CPUMODES
    {
        USER,
        FIQ,
        IRQ,
        SUPERVISOR,
        ABORT,
        UNDEFINED,
        SYSTEM
    }

    public static class CPU
    {
        public static bool thumbmode;

        public static bool output_debug = true;

        public static UInt32[] regs;

        public static UInt32 PC;
        public static UInt32 PC_old;

        public static bool Flag_Zero;
        public static bool Flag_Carry;
        public static bool Flag_Negative;
        public static bool Flag_V_Overflow;

        public static bool IRQ_disable;
        public static bool FIQ_disable;
        public static bool irpnext;
        public static int irpdelay;
        public static bool irpdelay_next;

        public static CPUMODES cpu_mode;

        public static bool halt;
        public static bool stop;

        public static UInt32[,] regbanks;

        public static Int32 newticks;
        public static Int32 totalticks;

        public static int additional_steps;
        public static UInt64 commands;
        public static UInt64 cyclenr;
        public static StreamWriter writer;
        public static bool tracenext;
        public static cpustate[] Tracelist = new cpustate[2000000];
        public static int traclist_ptr;
        public static int runmoretrace;

        public static List<int> irplist;
        public static int maxirp_fromfile;
        public static int nextirp_fromfile;

        private static UInt32 debug_outdivcnt;

        public static int op_since_dma = 0;

        public class cpustate
        {
            public UInt32[] debugregs;

            public UInt32 opcode;
            public UInt32 flags;
            public Int32 newticks;
            public uint busprefetch;
            public byte thumbmode;
            public byte armmode;
            public byte irpdisable;
            public UInt16 IF_intern;
            public int irp_wait;

            public UInt32 timer0;
            public UInt32 timer1;
            public UInt32 timer2;
            public UInt32 timer3;

            public UInt32 memory01;
            public UInt32 memory02;
            public UInt32 memory03;
            public UInt32 debug_dmatranfers;

            public UInt32 R16;
            public UInt32 R17;
            public UInt32 R13_USR;
            public UInt32 R14_USR;
            public UInt32 R13_IRQ;
            public UInt32 R14_IRQ;
            public UInt32 R13_SVC;
            public UInt32 R14_SVC;
            public UInt32 SPSR_IRQ;
            public UInt32 SPSR_SVC;

            public cpustate()
            {
                debugregs = new UInt32[18];
            }

            public void update(byte mode)
            {
                int saveticks = newticks;
                this.busprefetch = 0; // (uint)BusTiming.busPrefetchCount;

                for (int i = 0; i < regs.Length; i++)
                {
                    this.debugregs[i] = regs[i];
                }
                flags = 0;
                if (Flag_Negative) flags += 0x1000;
                if (Flag_Carry) flags += 0x100;
                if (Flag_Zero) flags += 0x10;
                if (Flag_V_Overflow) flags += 0x1;

                this.thumbmode = mode;

                if (mode < 2)
                {
                    if (mode == 1)
                    {
                        this.opcode = Memory.read_word(PC);
                    }
                    else
                    {
                        this.opcode = Memory.read_dword(PC);
                    }
                }
                else
                {
                    this.opcode = 0;
                }

                switch (cpu_mode)
                {
                    case CPUMODES.USER: this.armmode = 0x10; break;
                    case CPUMODES.FIQ: this.armmode = 0x11; break;
                    case CPUMODES.IRQ: this.armmode = 0x12; break;
                    case CPUMODES.SUPERVISOR: this.armmode = 0x13; break;
                    case CPUMODES.ABORT: this.armmode = 0x17; break;
                    case CPUMODES.UNDEFINED: this.armmode = 0x1B; break;
                    case CPUMODES.SYSTEM: this.armmode = 0x1F; break;
                }

                this.irpdisable = 0;
                if (IRQ_disable) this.irpdisable += 1;

                this.IF_intern = IRP.IRP_Flags;
                irp_wait = 0; //irpwait;

                this.timer0 = Memory.read_dword(0x04000100); // timer 0
                this.timer1 = Memory.read_dword(0x04000104); // timer 1
                this.timer2 = Memory.read_dword(0x04000108); // timer 2
                this.timer3 = Memory.read_dword(0x0400010C); // timer 3

                //UInt32 memory02 = Memory.read_dword(0x04000080); // sound cnt
                //UInt32 memory01 = Memory.read_dword(0x04000000); // display settings
                //this.memory03 = Memory.read_dword(0x04000004); // vcount
                //this.memory03 = Memory.read_dword(0x04000208); // master irp
                //this.memory01 = Memory.read_dword(0x04000200); // IME/IF

                this.memory01 = 0;// Memory.read_dword(0x04000000); // display settings
                this.memory02 = (UInt32)SoundDMA.soundDMAs[0].fifo.Count;
                this.memory03 = Memory.read_dword(0x04000004); // vcount

                this.debug_dmatranfers = DMA.debug_dmatranfers;

                R16 = get_CPSR();
                R17 = regs[17];
                R13_USR = regbanks[0, 13];
                R14_USR = regbanks[0, 14];
                R13_IRQ = regbanks[2, 13];
                R14_IRQ = regbanks[2, 14];
                R13_SVC = regbanks[3, 13];
                R14_SVC = regbanks[3, 14];
                SPSR_IRQ = regbanks[2, 17];
                SPSR_SVC = regbanks[3, 17];

                newticks = saveticks;
            }

            public void update_ticks(Int32 newticks)
            {
                this.newticks = totalticks;
                //this.newticks = newticks;

                //if (thumbmode == 2) // irp
                //{
                //    this.newticks = 0;
                //}
                //else
                //{
                //    this.newticks = newticks;
                //}
            }
        }

        public static void reset()
        {
            cpu_mode = CPUMODES.SUPERVISOR;
            thumbmode = false;
            regs = new UInt32[18];
            regbanks = new UInt32[6, 18];
            PC = 0;
            IRQ_disable = true;
            FIQ_disable = true;
            irpnext = false;

            halt = false;
            stop = false;

            totalticks = 0;
            commands = 0;
            cyclenr = 0;
            traclist_ptr = 0;
            runmoretrace = 0;

#if DEBUG
            for (int i = 0; i < Tracelist.Length; i++)
            {
                Tracelist[i] = new cpustate();
            }
#endif

            maxirp_fromfile = 0;
            nextirp_fromfile = -1;
            irplist = new List<int>();
            if (File.Exists("C:\\Projekte\\de2proc\\debug_gbasim_irp.txt"))
            {
                FileStream fs = new FileStream("C:\\Projekte\\de2proc\\debug_gbasim_irp.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(fs);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    int nextirp = int.Parse(line, System.Globalization.NumberStyles.HexNumber);
                    maxirp_fromfile = nextirp;
                    irplist.Add(nextirp);
                }
                reader.Close();
                fs.Close();

                if (irplist.Count > 0)
                {
                    nextirp_fromfile = irplist[0];
                    irplist.RemoveAt(0);
                }
            }
        }

        private static void trace_file(cpustate state)
        {
            //writer.Write(state.cyclenr.ToString().PadLeft(8) + " ");
            for (int i = 0; i < 16; i++)
            {
                writer.Write(state.debugregs[i].ToString("X8") + " ");
            }
            writer.Write(state.opcode.ToString("X8") + " ");
            writer.Write(state.flags.ToString("X4") + " ");
            writer.Write(state.newticks.ToString("X8") + " ");
            writer.Write(state.busprefetch.ToString("X3") + " ");
            writer.Write(state.thumbmode.ToString() + " ");
            writer.Write(state.armmode.ToString("X2") + " ");

            writer.Write(state.irpdisable.ToString() + " ");
            writer.Write(state.IF_intern.ToString("X4") + " ");
            writer.Write(state.irp_wait.ToString() + " ");

            writer.Write(state.timer0.ToString("X8") + " ");
            writer.Write(state.timer1.ToString("X8") + " ");
            writer.Write(state.timer2.ToString("X8") + " ");
            writer.Write(state.timer3.ToString("X8") + " ");

            writer.Write(state.memory01.ToString("X8") + " ");
            writer.Write(state.memory02.ToString("X8") + " ");
            writer.Write(state.memory03.ToString("X8") + " ");

            writer.Write(state.debug_dmatranfers.ToString("X8") + " ");

            writer.Write(state.R16.ToString("X8") + " ");
            writer.Write(state.R17.ToString("X8") + " ");
            writer.Write(state.R13_USR.ToString("X8") + " ");
            writer.Write(state.R14_USR.ToString("X8") + " ");
            writer.Write(state.R13_IRQ.ToString("X8") + " ");
            writer.Write(state.R14_IRQ.ToString("X8") + " ");
            writer.Write(state.R13_SVC.ToString("X8") + " ");
            writer.Write(state.R14_SVC.ToString("X8") + " ");
            writer.Write(state.SPSR_IRQ.ToString("X8") + " ");
            writer.Write(state.SPSR_SVC.ToString("X8") + " ");

            writer.WriteLine();
            writer.Flush();
        }

        private static void trace_file_last()
        {
            //writer = new StreamWriter("debug.txt");
            writer = new StreamWriter("C:\\Projekte\\de2proc\\debug_emu.txt");

            int i = 0;// traclist_ptr + 1;
            writer.WriteLine("reg 00   reg 01   reg 02   reg 03   reg 04   reg 05   reg 06   reg 07   reg 08   reg 09   reg 10   reg 11   " +
                "reg 12   reg 13   reg 14   reg 15   opcode   NCZV newticks PF  T Md I IFin T " +
                "Timer0   Timer1   Timer2   Timer3   MEMORY01 MEMORY02 MEMORY03 DMATrans " +
                "Reg 16   Reg 17   R13usr   R14usr   R13irq   R14irq   R13svc   R14svc   SPSR_irq SPSR_svc ");
            while (true)
            {
                trace_file(Tracelist[i]);
                i = (i + 1) % Tracelist.Length;
                if (i == traclist_ptr)
                {
                    break;
                }
            }
            writer.Close();
        }


        public static void nextInstr()
        {
            if (DMA.dma_active)
            {
                if (!DMA.delayed)
                {
                    newticks = 0;
                }
                return;
            }

            if (irpdelay > 0)
            {
                irpdelay-= Math.Max(1, newticks);
                if (irpdelay <= 0)
                {
                    irpnext = true;
                }
            }

            if (irpdelay_next)
            {
                irpdelay = 4;
                irpdelay_next = false;
            }
            
            if (!IRQ_disable && IRP.Master_enable && IRP.get_IF_with_mask() > 0 && !irpnext && !irpdelay_next && irpdelay <= 0)
            {
                //irpdelay = 4;
                irpdelay_next = true;
                //irpnext = true;
            }
#if DEBUG
            //if (nextirp_fromfile != -1 && (int)cyclenr <= nextirp_fromfile)
            //{
            //    irpnext = false;
            //    if (!IRQ_disable && (int)cyclenr == nextirp_fromfile)
            //    {
            //        irpdelay = 0;
            //        irpnext = true;
            //        if (irplist.Count > 0)
            //        {
            //            nextirp_fromfile = irplist[0];
            //            irplist.RemoveAt(0);
            //        }
            //        else
            //        {
            //            nextirp_fromfile = -1;
            //        }
            //    }
            //}

            //if (!IRQ_disable && commands == 24158) { irpdelay = 1; irpnext = false; } 

            //if (!IRQ_disable && commands == 44349) { irpdelay_next = false; irpdelay = 0; irpnext = true; } 

            bool tracethis = false;
            bool update_ticks = false;
            if (!halt || irpnext)
            {
                if (thumbmode)
                {
                    regs[15] = PC + 2;
                }
                else
                {
                    regs[15] = PC + 4;
                }

                //if (cyclenr == 7100000 && runmoretrace == 0) // end of bios
                //if (cyclenr == 38000000 && runmoretrace == 0)
                if (commands == 0000001 && runmoretrace == 0) 
                {
                    traclist_ptr = 0;
                    //runmoretrace = 300001;
                    runmoretrace = 1000000;
                }

                if (irpnext)
                {
                    debug_outdivcnt = 0;
                }

                update_ticks = true;
                if (runmoretrace > 0 && debug_outdivcnt == 0)
                {
                    if (thumbmode)
                    {
                        Tracelist[traclist_ptr].update(1);
                    }
                    else
                    {
                        Tracelist[traclist_ptr].update(0);
                    }

                    tracethis = true;

                    runmoretrace = runmoretrace - 1;
                    if (runmoretrace == 0)
                    {
                        //trace_file_last();
                    }
                }

                if (thumbmode && (PC & 1) == 1 || !thumbmode && (PC & 3) > 0)
                {
                    throw new Exception("Wrong PC with last bit set");
                }

                //if (traclist_ptr == 422) - game entry
                if (traclist_ptr == 1100)
                {
                    int xx = 0;
                }
            }
            UInt32 debug_outdiv = 1;

            //if (commands == 3000000) { Joypad.KeyStart = true; Joypad.set_reg(); }
            //if (commands == 3100000) { Joypad.KeyStart = false; Joypad.set_reg(); }
            //if (commands == 4000000) { Joypad.KeyStart = true; Joypad.set_reg(); }
            //if (commands % 1000 == 0) { Joypad.KeyStart = !Joypad.KeyStart; Joypad.set_reg(); }
#endif
            newticks = 0;

            PC_old = PC;

            if (irpnext && !IRQ_disable)
            {
#if DEBUG
                //Tracelist[traclist_ptr].opcode = 0;
                //Tracelist[traclist_ptr].thumbmode = 2;
                traclist_ptr--;
                cyclenr--;
#endif
                interrupt();
                halt = false;
                irpnext = false;
                irpdelay_next = false;
            }
            else if (!halt)
            {
                op_since_dma++;

                if (thumbmode)
                {
                    regs[15] = PC + 4;
                }
                else
                {
                    regs[15] = PC + 8;
                }

                if (thumbmode)
                {
                    thumb_command();

                    if (newticks < 0)
                    {
                        newticks = 0;
                    }
                    if (newticks == 0)
                    {
                        newticks = 1 + BusTiming.codeTicksAccessSeq16(PC_old);
                    }
                }
                else
                {
                    arm_command();

                    if (newticks < 0)
                    {
                        newticks = 0;
                    }
                    if (newticks == 0)
                    {
                        newticks = 1 + BusTiming.codeTicksAccessSeq32(PC_old);
                    }
                }

                commands++;
                if (newticks + additional_steps > 0)
                {
                    newticks += additional_steps;
                }
            }
            else
            {
                newticks++;
            }

#if DEBUG
            //if (commands == 5645810) { newticks -= 0xE; }

            if (update_ticks) totalticks += newticks;

            if (tracethis)
            {
                Tracelist[traclist_ptr].update_ticks(newticks);
                traclist_ptr = (traclist_ptr + 1) % Tracelist.Length;
                cyclenr++;
            }

            debug_outdivcnt = (debug_outdivcnt + 1) % debug_outdiv;
#endif

            if (irpnext)
            {
                newticks -= 1;
            }
        }

        private static void interrupt()
        {
            CPUSwitchMode(CPUMODES.IRQ, true);
            regs[14] = PC + 4;
            PC = 0x18;
            IRQ_disable = true;
            thumbmode = false;
        }

        private static void thumb_command()
        {
            UInt16 asmcmd = (UInt16)Memory.read_word(PC);

            Byte opcode_1513 = (Byte)((asmcmd >> 13) & 0x7); // bit 15..13

            switch (opcode_1513)
            {
                case 0:
                    if ((asmcmd >> 11) == 3)
                    {
                        add_subtract(((asmcmd >> 10) & 1) == 1, ((asmcmd >> 9) & 1) == 1, (byte)((asmcmd >> 6) & 0x7), (byte)((asmcmd >> 3) & 0x7), (byte)(asmcmd & 0x7));
                    }
                    else
                    {
                        move_shifted_register((byte)((asmcmd >> 11) & 3), (byte)((asmcmd >> 6) & 0x1F), (byte)((asmcmd >> 3) & 0x7), (byte)(asmcmd & 0x7));
                    }
                    break;

                case 1:
                    move_compare_add_subtract_immediate((byte)((asmcmd >> 11) & 3), (byte)((asmcmd >> 8) & 7), (byte)(asmcmd & 0xFF));
                    break;

                case 2:
                    switch ((asmcmd >> 10) & 7)
                    {
                        case 0:
                            alu_operations((byte)((asmcmd >> 6) & 0xF), (byte)((asmcmd >> 3) & 0x7), (byte)(asmcmd & 0x7));
                            break;

                        case 1:
                            hi_register_operations_branch_exchange((byte)((asmcmd >> 6) & 0xF), (byte)((asmcmd >> 3) & 0x7), (byte)(asmcmd & 0x7));
                            break;

                        case 2:
                        case 3:
                            pc_relative_load((byte)((asmcmd >> 8) & 0x7), (byte)(asmcmd & 0xFF));
                            break;

                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            if (((asmcmd >> 9) & 1) == 0)
                            {
                                load_store_with_register_offset(((asmcmd >> 11) & 1) == 1, ((asmcmd >> 10) & 1) == 1, (byte)((asmcmd >> 6) & 0x7), (byte)((asmcmd >> 3) & 0x7), (byte)(asmcmd & 0x7));
                            }
                            else
                            {
                                load_store_sign_extended_byte_halfword((byte)((asmcmd >> 10) & 0x3), (byte)((asmcmd >> 6) & 0x7), (byte)((asmcmd >> 3) & 0x7), (byte)(asmcmd & 0x7));
                            }
                            break;
                    }
                    break;

                case 3:
                    load_store_with_immidiate_offset(((asmcmd >> 11) & 1) == 1, ((asmcmd >> 12) & 1) == 1, (byte)((asmcmd >> 6) & 0x1F), (byte)((asmcmd >> 3) & 0x7), (byte)(asmcmd & 0x7));
                    break;

                case 4:
                    if (((asmcmd >> 12) & 1) == 0)
                    {
                        load_store_halfword(((asmcmd >> 11) & 1) == 1, (byte)((asmcmd >> 6) & 0x1F), (byte)((asmcmd >> 3) & 0x7), (byte)(asmcmd & 0x7));
                    }
                    else
                    {
                        sp_relative_load_store(((asmcmd >> 11) & 1) == 1, (byte)((asmcmd >> 8) & 0x7), (byte)(asmcmd & 0xFF));
                    }
                    break;

                case 5:
                    if (((asmcmd >> 12) & 1) == 0)
                    {
                        load_address(((asmcmd >> 11) & 1) == 1, (byte)((asmcmd >> 8) & 0x7), (byte)(asmcmd & 0xFF));
                    }
                    else
                    {
                        if (((asmcmd >> 10) & 1) == 0)
                        {
                            add_offset_to_stack_pointer(((asmcmd >> 7) & 1) == 1, (byte)(asmcmd & 0x7F));
                        }
                        else
                        {
                            push_pop_register(((asmcmd >> 11) & 1) == 1, ((asmcmd >> 8) & 1) == 1, (byte)(asmcmd & 0xFF));
                        }
                    }
                    break;

                case 6:
                    if (((asmcmd >> 12) & 1) == 0)
                    {
                        multiple_load_store(((asmcmd >> 11) & 1) == 1, (byte)((asmcmd >> 8) & 0x7), (byte)(asmcmd & 0xFF));
                    }
                    else
                    {
                        if (((asmcmd >> 8) & 0xF) == 0xF)
                        {
                            software_interrupt();
                        }
                        else
                        {
                            conditional_branch((byte)((asmcmd >> 8) & 0xF), (byte)(asmcmd & 0xFF));
                        }
                    }
                    break;

                case 7:
                    if (((asmcmd >> 12) & 1) == 0)
                    {
                        unconditional_branch((UInt16)(asmcmd & 0x7FF));
                    }
                    else
                    {
                        long_branch_with_link(((asmcmd >> 11) & 1) == 1, (UInt16)(asmcmd & 0x7FF));
                    }
                    break;
            }

            PC += 2;
        }

        private static void long_branch_with_link(bool high, UInt16 SOffset11)
        {
            if (!high) // high = 0!
            {
                if (((SOffset11 >> 10) & 1) == 1)
                {
                    regs[14] = (UInt32)(regs[15] + ((((Int16)(SOffset11 | 0xF800))) << 12));
                }
                else
                {
                    regs[14] = (UInt32)(regs[15] + (SOffset11 << 12));
                }
            }
            else
            {
                UInt32 old_pc = PC;
                PC = (UInt32)(regs[14] + (SOffset11 << 1));
                PC &= 0xFFFFFFFE;

                BusTiming.busPrefetchCount = 0;

                newticks = 3 + (BusTiming.codeTicksAccessSeq16(PC) * 2) + BusTiming.codeTicksAccess16(PC);

                PC -= 2;
                regs[14] = (old_pc + 2) | 1;
            }
        }

        private static void unconditional_branch(UInt16 SOffset11)
        {
            if (((SOffset11 >> 10) & 1) == 1)
            {
                PC = (UInt32)(regs[15] + ((((Int16)(SOffset11 | 0xF800))) << 1));
            }
            else
            {
                PC = (UInt32)(regs[15] + (SOffset11 << 1));
            }
            PC &= 0xFFFFFFFE;
            BusTiming.busPrefetchCount = 0;
            newticks = 3 + (BusTiming.codeTicksAccessSeq16(PC) * 2) + BusTiming.codeTicksAccess16(PC);
            PC -= 2;
        }

        private static void conditional_branch(byte cond, byte SOffset8)
        {
            bool execute = false;
            switch (cond)
            {
                case 0x0: if (Flag_Zero) { execute = true; } break;
                case 0x1: if (!Flag_Zero) { execute = true; } break;
                case 0x2: if (Flag_Carry) { execute = true; } break;
                case 0x3: if (!Flag_Carry) { execute = true; } break;
                case 0x4: if (Flag_Negative) { execute = true; } break;
                case 0x5: if (!Flag_Negative) { execute = true; } break;
                case 0x6: if (Flag_V_Overflow) { execute = true; } break;
                case 0x7: if (!Flag_V_Overflow) { execute = true; } break;
                case 0x8: if (Flag_Carry && !Flag_Zero) { execute = true; } break;
                case 0x9: if (!Flag_Carry || Flag_Zero) { execute = true; } break;
                case 0xA: if (Flag_Negative == Flag_V_Overflow) { execute = true; } break;
                case 0xB: if (Flag_Negative != Flag_V_Overflow) { execute = true; } break;
                case 0xC: if (!Flag_Zero && (Flag_Negative == Flag_V_Overflow)) { execute = true; } break;
                case 0xD: if (Flag_Zero || (Flag_Negative != Flag_V_Overflow)) { execute = true; } break;
            }

            if (execute)
            {
                PC = (UInt32)(regs[15] + ((SByte)SOffset8 << 1)) - 2;
                PC &= 0xFFFFFFFE;
                BusTiming.busPrefetchCount = 0;
                newticks = 3 + (2 * BusTiming.codeTicksAccessSeq16(PC + 2)) + BusTiming.codeTicksAccess16(PC + 2);
            }
            else
            {
                // this handling is wrong, just to be compatible...
                int oldprefetch = BusTiming.busPrefetchCount;
                BusTiming.busPrefetchCount = 0;
                newticks = BusTiming.codeTicksAccessSeq16(PC) + 1;
                BusTiming.busPrefetchCount = oldprefetch;
            }
        }

        private static void multiple_load_store(bool load, byte Rb, byte Rlist)
        {
            // opcode = !pre up !csr store
            block_data_transfer(0x5, load, Rb, (UInt16)Rlist);
        }

        private static void push_pop_register(bool load, bool link_pc, byte rlist)
        {
            UInt16 Rlist = rlist;
            if (link_pc)
            {
                if (load)
                {
                    Rlist |= 0x8000;
                }
                else
                {
                    Rlist |= 0x4000;
                }
            }
            if (load) 
            {
                // LDMIA!  opcode = !pre up !csr store
                block_data_transfer(0x5, load, 13, Rlist);
            }
            else
            {
                // STMDB !  opcode = pre !up !csr store
                block_data_transfer(0x9, load, 13, Rlist);
            }
        }

        private static void add_offset_to_stack_pointer(bool sign, byte word7)
        {
            if (sign)
            {
                regs[13] = regs[13] - (UInt32)(word7 << 2);
            }
            else
            {
                regs[13] = regs[13] + (UInt32)(word7 << 2);
            }
        }

        private static void load_address(bool sp, byte Rd, byte word8)
        {
            if (sp)
            {
                regs[Rd] = regs[13] + (UInt32)(word8 << 2);
            }
            else
            {
                regs[Rd] = (regs[15] & 0xFFFFFFFC) + (UInt32)(word8 << 2);
            }
        }

        private static void sp_relative_load_store(bool load, byte Rd, byte word8)
        {
            //if (BusTiming.busPrefetchCount == 0)
            //{
            //    BusTiming.busPrefetch = BusTiming.busPrefetchEnable;
            //}
            UInt32 address = regs[13] + (UInt32)(word8 << 2);
            if (load)
            {
                regs[Rd] = Memory.read_dword(address);
                newticks = 3 + BusTiming.dataTicksAccess32(address, 3) + BusTiming.codeTicksAccess16(PC + 2);
            }
            else
            {
                Memory.write_dword(address, regs[Rd]);
                newticks = 2 + BusTiming.dataTicksAccess32(address, 2) + BusTiming.codeTicksAccess16(PC + 2);
            }
        }

        private static void load_store_halfword(bool load, byte Offset5, byte Rb, byte Rd)
        {
            //if (BusTiming.busPrefetchCount == 0)
            //{
            //    BusTiming.busPrefetch = BusTiming.busPrefetchEnable;
            //}
            UInt32 address = regs[Rb] + (UInt32)(Offset5 << 1);
            if (load)
            {
                regs[Rd] = Memory.read_word(address);
                newticks = 3 + BusTiming.dataTicksAccess16(address, 3) + BusTiming.codeTicksAccess16(PC + 2);
            }
            else
            {
                newticks = 2 + BusTiming.dataTicksAccess16(address, 2) + BusTiming.codeTicksAccess16(PC + 2); // done before write, so there is time for bustiming change to happen
                Memory.write_word(address, (UInt16)regs[Rd]);
            }
        }

        private static void load_store_with_immidiate_offset(bool load, bool byteflag, byte Offset5, byte Rb, byte Rd)
        {
            //if (BusTiming.busPrefetchCount == 0)
            //{
            //    BusTiming.busPrefetch = BusTiming.busPrefetchEnable;
            //}
            if (load)
            {
                if (byteflag)
                {
                    UInt32 address = regs[Rb] + Offset5;
                    regs[Rd] = Memory.read_byte(address);
                    newticks = 3 + BusTiming.dataTicksAccess16(address, 3) + BusTiming.codeTicksAccess16(PC + 2);
                }
                else
                {
                    UInt32 address = regs[Rb] + (UInt32)(Offset5 << 2);
                    regs[Rd] = Memory.read_dword(address);
                    newticks = 3 + BusTiming.dataTicksAccess32(address, 3) + BusTiming.codeTicksAccess16(PC + 2);
                }
            }
            else
            {
                if (byteflag)
                {
                    UInt32 address = regs[Rb] + Offset5;
                    newticks = 2 + BusTiming.dataTicksAccess16(address, 2) + BusTiming.codeTicksAccess16(PC + 2);
                    Memory.write_byte(address, (byte)regs[Rd]);
                }
                else
                {
                    UInt32 address = regs[Rb] + (UInt32)(Offset5 << 2);
                    newticks = 2 + BusTiming.dataTicksAccess32(address, 2) + BusTiming.codeTicksAccess16(PC + 2);
                    Memory.write_dword(address, regs[Rd]);
                }
            }
        }

        private static void load_store_sign_extended_byte_halfword(byte opcode_hs, byte Ro, byte Rb, byte Rd)
        {
            //if (BusTiming.busPrefetchCount == 0)
            //{
            //    BusTiming.busPrefetch = BusTiming.busPrefetchEnable;
            //}
            UInt32 address = regs[Rb] + regs[Ro];
            switch (opcode_hs)
            {
                case 0: // Store halfword
                    newticks = 2 + BusTiming.dataTicksAccess16(address, 2) + BusTiming.codeTicksAccess16(PC + 2);
                    Memory.write_word(address, (UInt16)regs[Rd]);
                    break;

                case 1: // Load sign-extended byte
                    regs[Rd] = (UInt32)((Int32)(SByte)Memory.read_byte(address));
                    newticks = 3 + BusTiming.dataTicksAccess16(address, 3) + BusTiming.codeTicksAccess16(PC + 2);
                    break;

                case 2: // Load halfword
                    regs[Rd] = Memory.read_word(address);
                    newticks = 3 + BusTiming.dataTicksAccess16(address, 3) + BusTiming.codeTicksAccess16(PC + 2);
                    break;

                case 3: // Load sign-extended halfword:
                    if ((address & 1) == 0)
                    {
                        regs[Rd] = (UInt32)((Int32)(Int16)Memory.read_word(address));
                    }
                    else
                    {
                        regs[Rd] = (UInt32)((Int32)(SByte)Memory.read_word(address));
                    }
                    
                    newticks = 3 + BusTiming.dataTicksAccess16(address, 3) + BusTiming.codeTicksAccess16(PC + 2);
                    break;
            }
        }

        private static void load_store_with_register_offset(bool load, bool byteflag, byte Ro, byte Rb, byte Rd)
        {
            //if (BusTiming.busPrefetchCount == 0)
            //{
            //    BusTiming.busPrefetch = BusTiming.busPrefetchEnable;
            //}

            UInt32 address = regs[Rb] + regs[Ro];

            if (load)
            {
                if (byteflag)
                {
                    regs[Rd] = Memory.read_byte(address);
                    newticks = 3 + BusTiming.dataTicksAccess16(address, 3) + BusTiming.codeTicksAccess16(PC + 2);
                }
                else
                {
                    regs[Rd] = Memory.read_dword(address);
                    newticks = 3 + BusTiming.dataTicksAccess32(address, 3) + BusTiming.codeTicksAccess16(PC + 2);
                }
            }
            else
            {
                if (byteflag)
                {
                    Memory.write_byte(address, (byte)regs[Rd]);
                    newticks = 2 + BusTiming.dataTicksAccess16(address, 2) + BusTiming.codeTicksAccess16(PC + 2);
                }
                else
                {
                    Memory.write_dword(address, regs[Rd]);
                    newticks = 2 + BusTiming.dataTicksAccess32(address, 2) + BusTiming.codeTicksAccess16(PC + 2);
                }
            }
        }

        private static void pc_relative_load(byte Rd, byte word8)
        {
            uint addr = regs[15] + (UInt32)(word8 << 2);
            regs[Rd] = Memory.read_dword(addr & 0xFFFFFFFC);
            newticks = 3 + BusTiming.dataTicksAccess32(addr, 3) + BusTiming.codeTicksAccess16(PC + 4);
        }

        private static void thumbbranch_by_R15_write()
        {
            PC = (regs[15] & 0xFFFFFFFE) - 2;
            BusTiming.busPrefetchCount = 0;
            newticks = BusTiming.codeTicksAccessSeq16(PC - 2);
            newticks = (newticks * 2) + BusTiming.codeTicksAccess16(PC - 2) + 3;
        }

        private static void hi_register_operations_branch_exchange(byte opcode_h1h2, byte RsHs, byte RdHd)
        {
            switch (opcode_h1h2)
            {
                case 0x1: alu_add(RdHd, regs[RdHd], regs[RsHs + 8], false, true); break;    // 0001 ADD Rd, Hs ADD Rd, Rd, Hs Add a register in the range 8 - 15 to a register in the range 0 - 7.
                case 0x2:                                                                   // 0010 ADD Hd, Rs ADD Hd, Hd, Rs Add a register in the range 0 - 7 to a register in the range 8 - 15.
                    alu_add((byte)(RdHd + 8), regs[RdHd + 8], regs[RsHs], false, true);
                    if (RdHd + 8 == 15) { thumbbranch_by_R15_write(); }
                    break;
                case 0x3:                                                                   // 0011 ADD Hd, Hs ADD Hd, Hd, Hs Add two registers in the range 8 - 15
                    alu_add((byte)(RdHd + 8), regs[RdHd + 8], regs[RsHs + 8], false, true);
                    if (RdHd + 8 == 15) { thumbbranch_by_R15_write(); }
                    break;

                case 0x5: alu_sub(0, regs[RdHd], regs[RsHs + 8], true, false); break;       // 0101 CMP Rd, Hs CMP Rd, Hs Compare a register in the range 0 - 7 with a register in the range 8 - 15.Set the condition code flags on the result.
                case 0x6: alu_sub(0, regs[RdHd + 8], regs[RsHs], true, false); break;       // 0110 CMP Hd, Rs CMP Hd, Rs Compare a register in the range 8 - 15 with a register in the range 0 - 7.Set the condition code flags on the result.
                case 0x7: alu_sub(0, regs[RdHd + 8], regs[RsHs + 8], true, false); break;   // 0111 CMP Hd, Hs CMP Hd, Hs Compare two registers in the range 8 - 15.Set the condition code flags on the result.

                case 0x8: regs[RdHd] = regs[RsHs]; break;                                   // 1000 -> undefined but probably just using low for both
                case 0x9: regs[RdHd] = regs[RsHs + 8]; break;                               // 1001 MOV Rd, Hs MOV Rd, Hs Move a value from a register in the range 8 - 15 to a register in the range 0 - 7.  
                case 0xA:                                                                   // 1010 MOV Hd, Rs MOV Hd, Rs Move a value from a register in the range 0 - 7 to a register in the range 8 - 15.
                    regs[RdHd + 8] = regs[RsHs];
                    if (RdHd + 8 == 15) { thumbbranch_by_R15_write(); }
                    break;                                                   
                case 0xB:                                                                   // 1011 MOV Hd, Hs MOV Hd, Hs Move a value between two registers in the range 8 - 15.
                    regs[RdHd + 8] = regs[RsHs + 8];
                    if (RdHd + 8 == 15) { thumbbranch_by_R15_write(); }
                    break;     
                    
                case 0xC:                                                                   // 1100 BX Rs Perform branch(plus optional state change) to address in a register in the range 0 - 7.
                    branch_and_exchange(RsHs);
                    break;
                case 0xD:                                                                   // 1101 BX Hs Perform branch(plus optional state change) to address in a register in the range 8 - 15.
                    branch_and_exchange((byte)(RsHs + 8));
                    break;
            }
        }

        private static void alu_operations(byte opcode, byte Rs, byte Rd)
        {
            UInt32 op1_val = regs[Rd];
            UInt32 op2_val = regs[Rs];
            bool shiftercarry;
            switch (opcode)
            {
                case 0x0: alu_and(Rd, op1_val, op2_val, true, true, Flag_Carry); break;                              // 0000 AND Rd, Rs ANDS Rd, Rd, Rs Rd:= Rd AND Rs
                case 0x1: alu_xor(Rd, op1_val, op2_val, true, true, Flag_Carry); break;                              // 0001 EOR Rd, Rs EORS Rd, Rd, Rs Rd:= Rd EOR Rs
                case 0x2:                                                                                            // 0010 LSL Rd, Rs MOVS Rd, Rd, LSL Rs Rd := Rd << Rs
                    op1_val = shift_logicalleft(out shiftercarry, op1_val, (int)op2_val);
                    alu_mov(Rd, op1_val, true, shiftercarry);
                    if (BusTiming.busPrefetchEnable)
                    {
                        BusTiming.busPrefetchCount += 1;
                    }
                    newticks = 2 + BusTiming.codeTicksAccessSeq16(PC + 2); 
                    //newticks = 2 + BusTiming.codeTicksAccess16(PC + 2); 
                    break;             
                case 0x3:                                                                                           // 0011 LSR Rd, Rs MOVS Rd, Rd, LSR Rs Rd := Rd >> Rs
                    //if (op2_val == 0) { op2_val = 32; } -> wrong!
                    op1_val = shift_logicalright(out shiftercarry, op1_val, (int)op2_val);
                    alu_mov(Rd, op1_val, true, shiftercarry);
                    if (BusTiming.busPrefetchEnable)
                    {
                        BusTiming.busPrefetchCount += 1;
                    }
                    newticks = 2 + BusTiming.codeTicksAccessSeq16(PC + 2);
                    //newticks = 2 + BusTiming.codeTicksAccess16(PC + 2);
                    break;             
                case 0x4:                                                                                           // 0100 ASR Rd, Rs MOVS Rd, Rd, ASR Rs Rd := Rd ASR Rs
                    //if (op2_val == 0) { op2_val = 32; } -> wrong!
                    op1_val = shift_arithright(out shiftercarry, op1_val, (int)op2_val);
                    alu_mov(Rd, op1_val, true, shiftercarry);
                    if (BusTiming.busPrefetchEnable)
                    {
                        BusTiming.busPrefetchCount += 1;
                    }
                    newticks = 2 + BusTiming.codeTicksAccessSeq16(PC + 2); 
                    //newticks = 2 + BusTiming.codeTicksAccess16(PC + 2); 
                    break;   

                case 0x5: alu_add_withcarry(Rd, op1_val, op2_val, true, Flag_Carry); break;                         // 0101 ADC Rd, Rs ADCS Rd, Rd, Rs Rd:= Rd + Rs + C - bit
                case 0x6: alu_sub_withcarry(Rd, op1_val, op2_val, true, Flag_Carry); break;                         // 0110 SBC Rd, Rs SBCS Rd, Rd, Rs Rd:= Rd - Rs - NOT C - bit
                case 0x7:                                                                                           // 0111 ROR Rd, Rs MOVS Rd, Rd, ROR Rs Rd := Rd ROR Rs   
                    op1_val = shift_rotateright(out shiftercarry, op1_val, (int)op2_val);
                    alu_mov(Rd, op1_val, true, shiftercarry);
                    if (BusTiming.busPrefetchEnable)
                    {
                        BusTiming.busPrefetchCount += 1;
                    }
                    newticks = 2 + BusTiming.codeTicksAccessSeq16(PC + 2); 
                    //newticks = 2 + BusTiming.codeTicksAccess16(PC + 2); 
                    break;  
                case 0x8: alu_and(Rd, op1_val, op2_val, true, false, Flag_Carry); break;                            // 1000 TST Rd, Rs TST Rd, Rs Set condition codes on Rd AND Rs
                case 0x9: alu_sub(Rd, 0, op2_val, true, true); break;                                               // 1001 NEG Rd, Rs RSBS Rd, Rs, #0 Rd = -Rs
                case 0xA: alu_sub(Rd, op1_val, op2_val, true, false); break;                                        // 1010 CMP Rd, Rs CMP Rd, Rs Set condition codes on Rd - Rs
                case 0xB: alu_add(Rd, op1_val, op2_val, true, false); break;                                        // 1011 CMN Rd, Rs CMN Rd, Rs Set condition codes on Rd + Rs
                case 0xC: alu_or(Rd, op1_val, op2_val, true, Flag_Carry); break;                                    // 1100 ORR Rd, Rs ORRS Rd, Rd, Rs Rd:= Rd OR Rs
        
                case 0xD:                                                                                           // 1101 MUL Rd, Rs MULS Rd, Rs, Rd Rd:= Rs * Rd
                    multiply(0, true, 0, Rd, Rd, Rs);
                    break;
        
                case 0xE: alu_and_not(Rd, op1_val, op2_val, true, Flag_Carry); break;                               // 1110 BIC Rd, Rs BICS Rd, Rd, Rs Rd:= Rd AND NOT Rs
                case 0xF: alu_mov_not(Rd, op2_val, true, Flag_Carry); break;                                        // 1111 MVN Rd, Rs MVNS Rd, Rs Rd:= NOT Rs
            }
        }
        
        private static void move_compare_add_subtract_immediate(byte op, byte Rd, byte offset8)
        {
            UInt32 op1_val = regs[Rd];
            UInt32 op2_val = offset8;
            switch (op)
            {
                case 0: //mov
                    alu_mov(Rd, op2_val, true, Flag_Carry);
                    break;

                case 1: //cmp
                    alu_sub(Rd, op1_val, op2_val, true, false);
                    break;

                case 2: //add
                    alu_add(Rd, op1_val, op2_val, true, true);
                    break;

                case 3: //sub
                    alu_sub(Rd, op1_val, op2_val, true, true);
                    break;
            }
        }

        private static void add_subtract(bool imm, bool op_sub, byte Rn_offset3, byte Rs, byte Rd)
        {
            UInt32 op1_val = regs[Rs];
            UInt32 op2_val;
            if (imm)
            {
                op2_val = Rn_offset3;
            }
            else
            {
                op2_val = regs[Rn_offset3];
            }

            if (op_sub)
            {
                alu_sub(Rd, op1_val, op2_val, true, true);
            }
            else
            {
                alu_add(Rd, op1_val, op2_val, true, true);
            }
        }

        private static void move_shifted_register(byte op, byte offset5, byte Rs, byte Rd)
        {
            bool shiftercarry = false;
            UInt32 shiftervalue = regs[Rs];

            if ((op == 1 || op == 2) && offset5 == 0)
            {
                offset5 = 32;
            }

            switch (op)
            {
                case 0: // logical left
                    shiftervalue = shift_logicalleft(out shiftercarry, shiftervalue, offset5);
                    break;
                case 1: // logical right
                    shiftervalue = shift_logicalright(out shiftercarry, shiftervalue, offset5);
                    break;
                case 2: // arithmetic right
                    shiftervalue = shift_arithright(out shiftercarry, shiftervalue, offset5);
                    break;
            }

            alu_mov(Rd, shiftervalue, true, shiftercarry);
        }

        private static void arm_command()
        {
            UInt32 asmcmd = Memory.read_dword(PC);

            byte cond = (byte)(asmcmd >> 28);
            bool execute = false;
            switch (cond)
            {
                case 0x0: if (Flag_Zero) { execute = true; } break;
                case 0x1: if (!Flag_Zero) { execute = true; } break;
                case 0x2: if (Flag_Carry) { execute = true; } break;
                case 0x3: if (!Flag_Carry) { execute = true; } break;
                case 0x4: if (Flag_Negative) { execute = true; } break;
                case 0x5: if (!Flag_Negative) { execute = true; } break;
                case 0x6: if (Flag_V_Overflow) { execute = true; } break;
                case 0x7: if (!Flag_V_Overflow) { execute = true; } break;
                case 0x8: if (Flag_Carry && !Flag_Zero) { execute = true; } break;
                case 0x9: if (!Flag_Carry || Flag_Zero) { execute = true; } break;
                case 0xA: if (Flag_Negative == Flag_V_Overflow) { execute = true; } break;
                case 0xB: if (Flag_Negative != Flag_V_Overflow) { execute = true; } break;
                case 0xC: if (!Flag_Zero && (Flag_Negative == Flag_V_Overflow)) { execute = true; } break;
                case 0xD: if (Flag_Zero || (Flag_Negative != Flag_V_Overflow)) { execute = true; } break;
                case 0xE: execute = true; break;
            }

            if (execute)
            {
                Byte opcode_high3 = (Byte)((asmcmd >> 25) & 0x7); // bit 27..25
                Byte opcode_mid;
                Byte opcode_low;
                bool use_imm;
                bool updateflags;
                Byte Rn_op1;
                Byte Rdest;
                Byte RM_op2;
                UInt16 OP2;

                switch (opcode_high3)
                {
                    case 0: // (27..25) = 000 => alu commands?
                        opcode_low = (Byte)((asmcmd >> 4) & 0xF);   // bit 7..4
                        switch (opcode_low)
                        {
                            case 1:
                                if (((asmcmd >> 8) & 0x1FFFF) == 0x12FFF)
                                {
                                    RM_op2 = (Byte)(asmcmd & 0xF);   // bit 3..0
                                    branch_and_exchange(RM_op2);
                                }
                                else
                                {
                                    opcode_mid = (Byte)((asmcmd >> 21) & 0xF); // bit 24..21
                                    use_imm = (((asmcmd >> 25) & 1) == 1);
                                    updateflags = (((asmcmd >> 20) & 1) == 1);
                                    Rn_op1 = (Byte)((asmcmd >> 16) & 0xF);   // bit 19..16
                                    Rdest = (Byte)((asmcmd >> 12) & 0xF);   // bit 15..12
                                    OP2 = (UInt16)(asmcmd & 0xFFF);   // bit 11..0
                                    data_processing(use_imm, opcode_mid, updateflags, Rn_op1, Rdest, OP2, asmcmd);
                                }
                                break;

                            case 9:
                                opcode_mid = (Byte)((asmcmd >> 21) & 0xF); // bit 24..21
                                Rn_op1 = (Byte)((asmcmd >> 16) & 0xF);   // bit 19..16
                                Rdest = (Byte)((asmcmd >> 12) & 0xF);   // bit 15..12
                                OP2 = (UInt16)(asmcmd & 0xFFF);   // bit 11..0
                                if (opcode_mid >= 8)
                                {
                                    single_data_swap((opcode_mid & 2) == 2, Rn_op1, Rdest, OP2);
                                }
                                else if (opcode_mid >= 4)
                                {
                                    updateflags = (((asmcmd >> 20) & 1) == 1);
                                    multiply_long(opcode_mid, updateflags, Rn_op1, Rdest, OP2);
                                }
                                else
                                {
                                    updateflags = (((asmcmd >> 20) & 1) == 1);
                                    multiply(opcode_mid, updateflags, Rdest, Rn_op1, (byte)((OP2 >> 8) & 0xF), (byte)(OP2 & 0xF));
                                }
                                break;

                            case 0xB:  // halfword data transfer
                            case 0xD:
                            case 0xF:
                                opcode_mid = (Byte)((asmcmd >> 21) & 0xF); // bit 24..21
                                updateflags = (((asmcmd >> 20) & 1) == 1);
                                Rn_op1 = (Byte)((asmcmd >> 16) & 0xF);   // bit 19..16
                                Rdest = (Byte)((asmcmd >> 12) & 0xF);   // bit 15..12
                                OP2 = (UInt16)(asmcmd & 0xFFF);   // bit 11..0
                                RM_op2 = (Byte)(asmcmd & 0xF);   // bit 3..0
                                if (((asmcmd >> 22) & 1) == 1) // immidiate offset
                                {
                                    halfword_data_transfer(opcode_mid, opcode_low, updateflags, Rn_op1, Rdest, (UInt32)(((OP2 >> 4) & 0xF0) | RM_op2));
                                }
                                else // register offset
                                {
                                    halfword_data_transfer(opcode_mid, opcode_low, updateflags, Rn_op1, Rdest, regs[RM_op2]);
                                }
                                break;

                            default:
                                opcode_mid = (Byte)((asmcmd >> 21) & 0xF); // bit 24..21
                                use_imm = (((asmcmd >> 25) & 1) == 1);
                                updateflags = (((asmcmd >> 20) & 1) == 1);
                                Rn_op1 = (Byte)((asmcmd >> 16) & 0xF);   // bit 19..16
                                Rdest = (Byte)((asmcmd >> 12) & 0xF);   // bit 15..12
                                OP2 = (UInt16)(asmcmd & 0xFFF);   // bit 11..0
                                data_processing(use_imm, opcode_mid, updateflags, Rn_op1, Rdest, OP2, asmcmd);
                                break;
                        }
                        break;

                    case 1:
                        opcode_mid = (Byte)((asmcmd >> 21) & 0xF); // bit 24..21
                        use_imm = (((asmcmd >> 25) & 1) == 1);
                        updateflags = (((asmcmd >> 20) & 1) == 1);
                        Rn_op1 = (Byte)((asmcmd >> 16) & 0xF);   // bit 19..16
                        Rdest = (Byte)((asmcmd >> 12) & 0xF);   // bit 15..12
                        OP2 = (UInt16)(asmcmd & 0xFFF);   // bit 11..0
                        data_processing(use_imm, opcode_mid, updateflags, Rn_op1, Rdest, OP2, asmcmd);
                        break;

                    case 2:
                    case 3:
                        opcode_mid = (Byte)((asmcmd >> 21) & 0xF); // bit 24..21
                        use_imm = (((asmcmd >> 25) & 1) == 1);
                        updateflags = (((asmcmd >> 20) & 1) == 1);
                        Rn_op1 = (Byte)((asmcmd >> 16) & 0xF);   // bit 19..16
                        Rdest = (Byte)((asmcmd >> 12) & 0xF);   // bit 15..12
                        OP2 = (UInt16)(asmcmd & 0xFFF);   // bit 11..0
                        opcode_low = (Byte)((asmcmd >> 4) & 0xF);   // bit 7..4
                        single_data_transfer(use_imm, opcode_mid, opcode_low, updateflags, Rn_op1, Rdest, OP2);
                        break;

                    case 4:
                        opcode_mid = (Byte)((asmcmd >> 21) & 0xF); // bit 24..21
                        updateflags = (((asmcmd >> 20) & 1) == 1);
                        Rn_op1 = (Byte)((asmcmd >> 16) & 0xF);   // bit 19..16
                        block_data_transfer(opcode_mid, updateflags, Rn_op1, (UInt16)asmcmd);
                        break;

                    case 5:
                        opcode_mid = (Byte)((asmcmd >> 21) & 0xF); // bit 24..21
                        branch((opcode_mid & 8) == 8, asmcmd & 0xFFFFFF);
                        break;

                    case 7:
                        software_interrupt();
                        break;
                }
            }

            PC += 4;
        }

        private static void software_interrupt()
        {
            uint oldPC = PC;
            bool old_thumb = thumbmode;
            bool old_IRQ_disable = IRQ_disable;

            CPUSwitchMode(CPUMODES.SUPERVISOR, true);
            if (old_IRQ_disable)
            {
                regs[17] |= 0x80;
            }
            if (old_thumb) // only for SWI, not for normal interrupt...
            {
                regs[14] = PC + 2;
            }
            else
            {
                regs[14] = PC + 4;
            }

            BusTiming.busPrefetchCount = 0;

            if (old_thumb)
            {
                PC = 6; // will be 8 after end of thumb command
                newticks = 3;
            }
            else
            {
                PC = 4; // will be 8 after end of arm command
                newticks = BusTiming.codeTicksAccessSeq32(PC) + 1;
                newticks = (newticks * 2) + BusTiming.codeTicksAccess32(PC) + 1;
            }

            thumbmode = false;
            IRQ_disable = true;
        }

        private static void branch(bool link, UInt32 offset)
        {
            if (link)
            {
                regs[14] = PC + 4;
            }

            Int32 offset_int;
            if ((offset & 0x800000) == 0)
            {
                offset_int = (Int32)offset;
            }
            else
            {
                offset_int = (Int32)(offset | 0xFF000000);
            }
            offset_int = offset_int << 2;
            PC = (UInt32)(regs[15] + offset_int) & 0xFFFFFFFC;

            BusTiming.busPrefetchCount = 0;
            newticks = 3 + BusTiming.codeTicksAccess32(PC) + 2 * BusTiming.codeTicksAccessSeq32(PC);
            
            PC -= 4;
        }

        private static void block_data_transfer(byte opcode, bool load_store, byte Rn_op1, UInt16 reglist)
        {
            //if (BusTiming.busPrefetchCount == 0)
            //{
            //    BusTiming.busPrefetch = BusTiming.busPrefetchEnable;
            //}

            bool first = true;

            bool R15set = (reglist & 0x8000) == 0x8000 || reglist == 0;
            UInt32 address = regs[Rn_op1];
            UInt32 endaddress = 0;
            UInt32 endaddress_baserlist = 0;
            if (reglist == 0)
            {
                if ((opcode & 4) != 4) //down
                {
                    endaddress = address - 64;
                }
                else
                {
                    endaddress = address + 64;
                }
            }
            bool pre = (opcode & 8) == 8;
            bool usermode_regs = false;
            bool writeback_in_reglist = false;

            // should only happen in priviledged mode
            if ((opcode & 2) == 2) // PSR & force user bit
            {
                if (R15set) //CPSR = SPSR_<mode>
                {
                    if (!load_store)
                    {
                        // write user bank regs instead of current process regs
                        usermode_regs = true;
                    }
                }
                else
                {
                    // load/write user bank regs instead of current process regs
                    usermode_regs = true;
                }
            }

            if ((opcode & 4) != 4) //down
            {
                UInt16 reglist_copy = reglist;
                // must calculate fully decremented addressed based on how many regs will be written
                for (int i = 0; i < 16; i++)
                {
                    if ((reglist_copy & 1) == 1)
                    {
                        address -= 4;
                    }
                    reglist_copy = (UInt16)(reglist_copy >> 1);
                }
                if (reglist != 0)
                {
                    endaddress = address;
                    endaddress_baserlist = address;
                }
                else
                {
                    address = endaddress;
                }
                if (!pre)
                {
                    address += 4;
                }
            }
            else
            {
                UInt16 reglist_copy = reglist;
                endaddress_baserlist = address;
                // must calculate fully incremented addressed based on how many regs will be written
                for (int i = 0; i < 16; i++)
                {
                    if ((reglist_copy & 1) == 1)
                    {
                        endaddress_baserlist += 4;
                    }
                    reglist_copy = (UInt16)(reglist_copy >> 1);
                }
                if (pre)
                {
                    address += 4;
                }
            }

            Memory.blockcmd_lowerbits = (byte)(address & 3);
            if (load_store) // load
            {
                reglist &= 0x7FFF;
                for (int i = 0; i < 15; i++)
                {
                    if ((reglist & 1) == 1)
                    {
                        if (i == Rn_op1)
                        {
                            writeback_in_reglist = true;
                        }
                        if (usermode_regs && cpu_mode != CPUMODES.USER && cpu_mode != CPUMODES.SYSTEM)
                        {
                            regbanks[0, i] = Memory.read_dword(address & 0xFFFFFFFC);
                        }
                        else
                        {
                            regs[i] = Memory.read_dword(address & 0xFFFFFFFC);
                        }
                        if (first)
                        {
                            newticks += 2 + BusTiming.dataTicksAccess32(address & 0xFFFFFFFC, 3);
                            first = false;
                        }
                        else
                        {
                            newticks += 1 + BusTiming.dataTicksAccessSeq32(address & 0xFFFFFFFC, 1);
                        }
                        address += 4;
                    }
                    reglist = (UInt16)(reglist >> 1);
                }
                if (R15set)
                {
                    PC = Memory.read_dword(address & 0xFFFFFFFC);
                    if (first)
                    {
                        newticks += 2 + BusTiming.dataTicksAccess32(address & 0xFFFFFFFC, 1);
                    }
                    else
                    {
                        newticks += 1 + BusTiming.dataTicksAccessSeq32(address & 0xFFFFFFFC, 1);
                    }
                    BusTiming.busPrefetchCount = 0;
                    address += 4;
                    if ((opcode & 2) != 2 || !R15set)
                    {
                        if (thumbmode)
                        {
                            newticks += 3 + BusTiming.codeTicksAccess16(PC) + (BusTiming.codeTicksAccessSeq16(PC) * 2);
                        }
                        else
                        {
                            newticks += 3 + BusTiming.codeTicksAccess32(PC) + (BusTiming.codeTicksAccessSeq32(PC) * 2);
                        }
                    }
                }
                else
                {
                    if (thumbmode)
                    {
                        newticks += 1 + BusTiming.codeTicksAccess16(PC + 2);
                    }
                    else
                    {
                        newticks += 1 + BusTiming.codeTicksAccess32(PC + 4);
                    }
                }
            }
            else
            {
                if (reglist == 0)
                {
                    reglist = 0x8000;
                }
                for (int i = 0; i < 16; i++)
                {
                    if ((reglist & 1) == 1)
                    {
                        UInt32 writeval;
                        if (usermode_regs && cpu_mode != CPUMODES.USER && cpu_mode != CPUMODES.SYSTEM)
                        {
                            if (i >= 8 && i <= 14)
                            {
                                writeval = regbanks[0, i];
                            }
                            else
                            {
                                writeval = 0;
                            }
                        }
                        else
                        {
                            writeval = regs[i];
                        }
                        if (i == 15)
                        {
                            if (thumbmode)
                            {
                                writeval += 2;
                            }
                            else
                            {
                                writeval += 4;
                            }
                        }
                        if (!first && i == Rn_op1)
                        {
                            writeval = endaddress_baserlist;
                        }
                        Memory.write_dword(address & 0xFFFFFFFC, writeval);
                        if (first)
                        {
                            newticks += 1 + BusTiming.dataTicksAccess32(address & 0xFFFFFFFC, 2);
                            first = false;
                        }
                        else
                        {
                            newticks += 1 + BusTiming.dataTicksAccessSeq32(address & 0xFFFFFFFC, 1);
                        }
                        address += 4;
                        // baseaddress register usually changed if written as second or later!
                    }
                    reglist = (UInt16)(reglist >> 1);
                }
                if (thumbmode)
                {
                    newticks += 1 + BusTiming.codeTicksAccess16(PC + 2);
                }
                else
                {
                    newticks += 1 + BusTiming.codeTicksAccess32(PC + 4);
                }
            }
            Memory.blockcmd_lowerbits = 0;

            if ((opcode & 1) == 1 && !writeback_in_reglist) //writeback
            {
                if (pre)
                {
                    address -= 4;
                }
                if (endaddress == 0)
                {
                    regs[Rn_op1] = address;
                }
                else
                {
                    regs[Rn_op1] = endaddress;
                }
            }

            bool oldmode = thumbmode;

            // should only happen in priviledged mode
            if ((opcode & 2) == 2) // PSR & force user bit
            {
                if (R15set) //CPSR = SPSR_<mode>
                {
                    if (load_store) // load
                    {
                        CPUSwitchMode(get_mode_from_value(regs[17]), true);
                        if (thumbmode)
                        {
                            newticks += 3 + BusTiming.codeTicksAccess16(PC) + (BusTiming.codeTicksAccessSeq16(PC) * 2);
                        }
                        else
                        {
                            newticks += 3 + BusTiming.codeTicksAccess32(PC) + (BusTiming.codeTicksAccessSeq32(PC) * 2);
                        }
                    }
                }
            }

            if (load_store && R15set)
            {
                if (thumbmode)
                {
                    PC &= 0xFFFFFFFE;
                }
                else
                {
                    PC &= 0xFFFFFFFC;
                }
                if (oldmode)
                {
                    PC -= 2;
                }
                else
                {
                    PC -= 4;
                }
            }
        }

        private static void single_data_transfer(bool use_imm, byte opcode, byte opcode_low, bool load_store, byte Rn_op1, byte Rdest, UInt16 op2)
        {
            //if (BusTiming.busPrefetchCount == 0)
            //{
            //    BusTiming.busPrefetch = BusTiming.busPrefetchEnable;
            //}

            UInt32 imm_value = 0;
            if (!use_imm)
            {
                imm_value = op2;
            }
            else
            {
                bool shiftercarry;
                shifter(out shiftercarry, out imm_value, op2);
            }

            UInt32 address = regs[Rn_op1];
            if ((opcode & 8) == 8) //pre
            {
                if ((opcode & 4) == 4) //up
                {
                    address += imm_value;
                }
                else
                {
                    address -= imm_value;
                }
            }

            if (load_store) // load
            {
                if ((opcode & 2) == 2) // byte transfer
                {
                    regs[Rdest] = (UInt32)Memory.read_byte(address);
                    newticks = 3 + BusTiming.dataTicksAccess16(address, 3);
                }
                else // word transfer
                {
                    regs[Rdest] = Memory.read_dword(address);
                    newticks = 3 + BusTiming.dataTicksAccess32(address, 3);
                }
                if (Rdest == 15)
                {
                    PC = regs[Rdest] & 0xFFFFFFFC;
                    BusTiming.busPrefetchCount = 0;
                    newticks += 3 + BusTiming.codeTicksAccess32(PC) + 2 * BusTiming.codeTicksAccessSeq32(PC);
                    PC -= 4;
                }
                else
                {
                    newticks += BusTiming.codeTicksAccess32(PC + 4);
                }
            }
            else
            {
                uint value = regs[Rdest];
                if (Rdest == 15)
                {
                    value += 4;
                }
                if ((opcode & 2) == 2) // byte transfer
                {
                    newticks = 2 + BusTiming.dataTicksAccess16(address, 2) + BusTiming.codeTicksAccess32(PC + 4);
                    Memory.write_byte(address, (byte)value);
                }
                else // word transfer
                {
                    newticks = 2 + BusTiming.dataTicksAccess32(address, 2) + BusTiming.codeTicksAccess32(PC + 4);
                    Memory.write_dword(address, value);
                }
            }

            if ((opcode & 8) == 0) //post
            {
                if ((opcode & 4) == 4) //up
                {
                    address += imm_value;
                }
                else
                {
                    address -= imm_value;
                }
            }

            if ((opcode & 8) == 0 || (opcode & 1) == 1) //writeback
            {
                if (Rn_op1 != Rdest || !load_store) // when storing, result address can be written
                {
                    regs[Rn_op1] = address;
                }
            }
        }

        private static void halfword_data_transfer(byte opcode, byte opcode_low, bool load_store, byte Rn_op1, byte Rdest, UInt32 value)
        {
            //if (BusTiming.busPrefetchCount == 0)
            //{
            //    BusTiming.busPrefetch = BusTiming.busPrefetchEnable;
            //}

            UInt32 address = regs[Rn_op1];
            if ((opcode & 8) == 8) //pre
            {
                if ((opcode & 4) == 4) //up
                {
                    address += value;
                }
                else
                {
                    address -= value;
                }
            }

            if (load_store) // load
            {
                switch (opcode_low)
                {
                    case 0xB: // unsigned halfword
                        regs[Rdest] = (UInt32)Memory.read_word(address);
                        newticks = 3 + BusTiming.dataTicksAccess16(address, 3) + BusTiming.codeTicksAccess32(PC + 4);
                        break;

                    case 0xD: // signed byte
                        regs[Rdest] = (UInt32)((Int32)(SByte)Memory.read_byte(address));
                        newticks = 3 + BusTiming.dataTicksAccess16(address, 3) + BusTiming.codeTicksAccess32(PC + 4);
                        break;

                    case 0xF: // signed halfword
                        if ((address & 1) == 0)
                        {
                            regs[Rdest] = (UInt32)((Int32)(Int16)Memory.read_word(address));
                        }
                        else
                        {
                            regs[Rdest] = (UInt32)((Int32)(SByte)Memory.read_byte(address));
                        }
                        newticks = 3 + BusTiming.dataTicksAccess16(address, 3) + BusTiming.codeTicksAccess32(PC + 4);
                        break;
                }
            }
            else
            {
                uint writevalue = regs[Rdest];
                if (Rdest == 15)
                {
                    writevalue += 4;
                }
                switch (opcode_low)
                {
                    case 0xB: // unsigned halfword
                    case 0xD: // signed halfword
                        newticks = 2 + BusTiming.dataTicksAccess16(address, 2) + BusTiming.codeTicksAccess32(PC + 4);
                        Memory.write_word(address, (UInt16)writevalue);
                        break;

                    case 0xF: // signed byte
                        newticks = 2 + BusTiming.dataTicksAccess16(address, 2) + BusTiming.codeTicksAccess32(PC + 4);
                        Memory.write_byte(address, (byte)writevalue);
                        break;
                }
            }

            if ((opcode & 8) == 0) //post
            {
                if ((opcode & 4) == 4) //up
                {
                    address += value;
                }
                else
                {
                    address -= value;
                }

            }

            if ((opcode & 8) == 0 || (opcode & 1) == 1) //writeback
            {
                if (Rn_op1 != Rdest || !load_store) // when storing, result address can be written
                {
                    regs[Rn_op1] = address;
                }
            }
        }

        private static void branch_and_exchange(byte register)
        {
            bool old_thumbmode = thumbmode;
            BusTiming.busPrefetchCount = 0;
            thumbmode = (regs[register] & 1) == 1;
            if (thumbmode)
            {
                PC = regs[register] & 0xFFFFFFFE;
                newticks = 3 + BusTiming.codeTicksAccessSeq16(PC) + BusTiming.codeTicksAccessSeq16(PC) + BusTiming.codeTicksAccess16(PC);
            }
            else
            {
                PC = regs[register] & 0xFFFFFFFC;
                newticks = 3 + BusTiming.codeTicksAccessSeq32(PC) + BusTiming.codeTicksAccessSeq32(PC) + BusTiming.codeTicksAccess32(PC);
            }
            if (old_thumbmode)
            {
                PC -= 2;
            }
            else
            {
                PC -= 4;
            }
        }

        private static void single_data_swap(bool byteswap, byte Rn_op1, byte Rdest, UInt16 Op2)
        {
            if (byteswap)
            {
                byte swap = Memory.read_byte(regs[Rn_op1]);
                Memory.write_byte(regs[Rn_op1], (byte)regs[(Op2 & 0xF)]);
                regs[Rdest] = (UInt32)swap;
            }
            else
            {
                UInt32 swap = Memory.read_dword(regs[Rn_op1]);
                Memory.write_dword(regs[Rn_op1], regs[(Op2 & 0xF)]);
                regs[Rdest] = swap;
            }

            // shouldn't this use access8 for byte?
            newticks = 3 + BusTiming.dataTicksAccess32(regs[Rn_op1], 3) + BusTiming.dataTicksAccess32(regs[Rn_op1], 0) + BusTiming.codeTicksAccess32(PC + 4);
        }

        private static void multiply(byte add, bool s_updateflags, byte Rn_op1, byte Rdest, byte reg1, byte reg2)
        {
            UInt32 rs = regs[reg1];

            UInt64 result = (UInt64)rs * (UInt64)regs[reg2];
            if (add > 0)
            {
                result += regs[Rn_op1];
                newticks += 1;
            }
            regs[Rdest] = (UInt32)result;

            if (s_updateflags)
            {
                Flag_Negative = ((result >> 31) & 1) == 1;
                Flag_Zero = (UInt32)result == 0;
            }

            if ((int)rs < 0)
            {
                rs = ~rs;
            }
            if ((rs & 0xFFFFFF00) == 0) { newticks += 1; }
            else if ((rs & 0xFFFF0000) == 0) { newticks += 2; }
            else if ((rs & 0xFF000000) == 0) { newticks += 3; }
            else newticks += 4;

            if (thumbmode)
            {
                if (BusTiming.busPrefetchEnable)
                {
                    BusTiming.busPrefetchCount += newticks;
                    newticks += 1 + BusTiming.codeTicksAccessSeq16(PC);
                }
                else
                {
                    newticks += 1 + BusTiming.codeTicksAccess16(PC);
                }
            }
            else
            {
                if (BusTiming.busPrefetchEnable)
                {
                    BusTiming.busPrefetchCount += newticks;
                    newticks += 1 + BusTiming.codeTicksAccessSeq32(PC);
                }
                else
                {
                    newticks += 1 + BusTiming.codeTicksAccess32(PC);
                }
            }
        }

        private static void multiply_long(byte opcode, bool s_updateflags, byte RdestHi, byte RdestLo, UInt16 Op2)
        {
            UInt64 result = 0;

            UInt32 rs = regs[((Op2 >> 8) & 0xF)];

            switch (opcode & 3)
            {
                case 0: // unsigned, multiply only
                    result = (UInt64)rs * (UInt64)regs[(Op2 & 0xF)];
                    break;

                case 1: // unsigned with add
                    result = (UInt64)rs * (UInt64)regs[(Op2 & 0xF)];
                    result = result + (((UInt64)regs[RdestHi] << 32) | (UInt64)regs[RdestLo]);
                    newticks += 1;
                    break;

                case 2: // signed, multiply only
                    result = (UInt64)((Int64)(Int32)rs * (Int64)(Int32)regs[(Op2 & 0xF)]);
                    break;

                case 3: // signed with add
                    result = (UInt64)((Int64)(Int32)rs * (Int64)(Int32)regs[(Op2 & 0xF)]);
                    result = (UInt64)((Int64)result + (((Int64)regs[RdestHi] << 32) | (Int64)regs[RdestLo]));
                    newticks += 1;
                    break;
            }

            regs[RdestHi] = (UInt32)(result >> 32);
            regs[RdestLo] = (UInt32)(result & 0xFFFFFFFF);

            if (s_updateflags)
            {
                Flag_Negative = ((result >> 63) & 1) == 1;
                Flag_Zero = result == 0;
            }

            if ((int)rs < 0)
            {
                rs = ~rs;
            }
            if ((rs & 0xFFFFFF00) == 0) { newticks += 2; }
            else if ((rs & 0xFFFF0000) == 0) { newticks += 3; }
            else if ((rs & 0xFF000000) == 0) { newticks += 4; }
            else newticks += 5;

            if (BusTiming.busPrefetchEnable)
            {
                BusTiming.busPrefetchCount += newticks;
                newticks += 1 + BusTiming.codeTicksAccessSeq32(PC);
            }
            else
            {
                newticks += 1 + BusTiming.codeTicksAccess32(PC);
            }
        }

        private static void data_processing_PSR(UInt32 asmcmd)
        {
            bool destination_psr = ((asmcmd >> 22) & 1) == 1;

            if (((asmcmd >> 16) & 0x3F) == 0x0F)  // MRS (transfer PSR contents to a register)
            {
                if (destination_psr) // spsr
                {
                    uint target = (asmcmd >> 12) & 0xF;
                    regs[target] = regs[17];
                }
                else
                {
                    uint target = (asmcmd >> 12) & 0xF;
                    regs[target] = get_CPSR();
                }
            }
            else //MSR (transfer register contents or immdiate value to PSR)
            {
                UInt32 value;
                if (((asmcmd >> 25) & 1) == 1) 
                {
                    UInt32 shift = ((asmcmd >> 8) & 0xF) * 2;
                    value = RotateRight((asmcmd & 0xFF), (int)shift) & 0xFF00000F; // immidiate is for flags only
                }
                else // from register
                {
                    value = regs[asmcmd & 0xF];
                }
                if (destination_psr) // should transfer to spsr
                {
                    if (cpu_mode != CPUMODES.USER)
                    {
                        if ((asmcmd & 0x00010000) > 0)
                        {
                            regs[17] = (regs[17] & 0xFFFFFF00) | (value & 0x000000FF);
                        }
                        if ((asmcmd & 0x00020000) > 0)
                        {
                            regs[17] = (regs[17] & 0xFFFF00FF) | (value & 0x0000FF00);
                        }
                        if ((asmcmd & 0x00040000) > 0)
                        {
                            regs[17] = (regs[17] & 0xFF00FFFF) | (value & 0x00FF0000);
                        }
                        if ((asmcmd & 0x00080000) > 0)
                        {
                            regs[17] = (regs[17] & 0x00FFFFFF) | (value & 0xFF000000);
                        }
                    }
                }
                else // should transfer to cpsr
                {
                    UInt32 newval;
                    if (((asmcmd >> 25) & 1) == 1) // immidiate
                    {
                        newval = (get_CPSR() & 0x00FFFFFF) | value;
                    }
                    else
                    {
                        newval = get_CPSR();
                    }
                    if (cpu_mode != CPUMODES.USER)
                    {
                        if ((asmcmd & 0x00010000) > 0)
                        {
                            newval = (newval & 0xFFFFFF00) | (value & 0x000000FF);
                        }
                        if ((asmcmd & 0x00020000) > 0)
                        {
                            newval = (newval & 0xFFFF00FF) | (value & 0x0000FF00);
                        }
                        if ((asmcmd & 0x00040000) > 0)
                        {
                            newval = (newval & 0xFF00FFFF) | (value & 0x00FF0000);
                        }
                    }
                    if ((asmcmd & 0x00080000) > 0)
                    {
                        newval = (newval & 0x00FFFFFF) | (value & 0xFF000000);
                    }
                    newval |= 0x10;
                    CPUSwitchMode(get_mode_from_value(newval & 0x1F), false);
                    set_CPSR(newval);
                }
            }
        }

        private static UInt32 shift_logicalleft(out bool shiftercarry, UInt32 shiftervalue, int shiftamount)
        {
            UInt32 shiftresult = shiftervalue;

            if (shiftamount >= 32)
            {
                if (shiftamount == 32)
                {
                    shiftercarry = (shiftervalue & 1) == 1;
                }
                else
                {
                    shiftercarry = false;
                }
                shiftresult = 0;
            }
            else if (shiftamount > 0)
            {
                shiftercarry = ((shiftervalue >> (32 - shiftamount)) & 1) == 1;
                shiftresult = shiftervalue << shiftamount;
            }
            else
            {
                shiftercarry = Flag_Carry;
            }
            return shiftresult;
        }

        private static UInt32 shift_logicalright(out bool shiftercarry, UInt32 shiftervalue, int shiftamount)
        {
            UInt32 shiftresult = shiftervalue;

            if (shiftamount >= 32)
            {
                if (shiftamount == 32)
                {
                    shiftercarry = ((shiftervalue >> 31) & 1) == 1;
                }
                else
                {
                    shiftercarry = false;
                }
                shiftresult = 0;
            }
            else if (shiftamount > 0)
            {
                shiftercarry = ((shiftervalue >> (shiftamount - 1)) & 1) == 1;
                shiftresult = shiftervalue >> shiftamount;
            }
            else
            {
                shiftercarry = Flag_Carry;
            }

            return shiftresult;
        }

        private static UInt32 shift_arithright(out bool shiftercarry, UInt32 shiftervalue, int shiftamount)
        {
            UInt32 shiftresult = shiftervalue;

            if (shiftamount >= 32)
            {
                shiftercarry = ((shiftervalue >> 31) & 1) == 1;
                shiftresult = (uint)(((int)shiftervalue) >> 31);
            }
            else if (shiftamount > 0)
            {
                shiftercarry = ((shiftervalue >> (shiftamount - 1)) & 1) == 1;
                shiftresult = (uint)(((int)shiftervalue) >> shiftamount);
            }
            else
            {
                shiftercarry = Flag_Carry;
            }

            return shiftresult;
        }

        private static UInt32 shift_rotateright(out bool shiftercarry, UInt32 shiftervalue, int shiftamount)
        {
            UInt32 shiftresult = shiftervalue;

            if (shiftamount > 32)
            {
                shiftamount = shiftamount % 32;
            }
            if (shiftamount == 32)
            {
                shiftercarry = ((shiftervalue >> 31) & 1) == 1;
            }
            else if (shiftamount > 0)
            {
                shiftercarry = ((shiftervalue >> (shiftamount - 1)) & 1) == 1;
                shiftresult = RotateRight(shiftervalue, shiftamount);
            }
            else
            {
                shiftercarry = Flag_Carry;
            }

            return shiftresult;
        }

        private static bool shifter(out bool shiftercarry, out UInt32 shiftervalue, UInt16 shift_opcode)
        {
            shiftercarry = Flag_Carry;
            byte shiftsettings = (Byte)(shift_opcode >> 4);
            byte shiftmode = (byte)((shiftsettings >> 1) & 3);
            int shiftamount;
            shiftervalue = regs[(shift_opcode & 0xF)];
            bool regbased = (shiftsettings & 1) == 1;
            if (!regbased) // immidiate
            {
                shiftamount = shiftsettings >> 3;
                if ((shiftmode == 1 || shiftmode == 2) && shiftamount == 0)
                {
                    shiftamount = 32;
                }
                if (shiftmode == 3 && shiftamount == 0)
                {
                    shiftmode = 4;
                }
            }
            else // reg based
            {
                if ((shift_opcode & 0xF) == 15)
                {
                    shiftervalue += 4;
                }
                shiftamount = (int)(regs[(shiftsettings >> 4)] & 0xFF);
                if (BusTiming.busPrefetchEnable)
                {
                    BusTiming.busPrefetchCount++;
                }
                newticks = 2 + BusTiming.codeTicksAccessSeq32(PC + 2);
                if (shiftamount == 0)
                {
                    shiftercarry = Flag_Carry;
                }
            }

            // ARM DOC: For all these instructions except ROR:
            // if the shift is 32, Rd is cleared, and the last bit shifted out remains in the C flag
            // if the shift is greater than 32, Rd and the C flag are cleared.

            // however this seems to be wrong. Setting rd to zero for asr does not pass armwrestler tests
   
            switch (shiftmode)
            {
                case 0: // logical left
                    shiftervalue = shift_logicalleft(out shiftercarry, shiftervalue, shiftamount);
                    break;
                case 1: // logical right
                    shiftervalue = shift_logicalright(out shiftercarry, shiftervalue, shiftamount);
                    break;
                case 2: // arithmetic right
                    shiftervalue = shift_arithright(out shiftercarry, shiftervalue, shiftamount);
                    break;
                case 3: // rotate right
                    shiftervalue = shift_rotateright(out shiftercarry, shiftervalue, shiftamount);
                    break;
                case 4: // rrx
                    shiftercarry = (shiftervalue & 1) == 1;
                    shiftervalue = shiftervalue >> 1;
                    if (Flag_Carry)
                    {
                        shiftervalue += 0x80000000;
                    }
                    break;
            }

            return regbased;
        }

        private static void update_flags_logical(UInt32 result, bool shiftercarry)
        {
            Flag_Negative = ((result >> 31) & 1) == 1;
            Flag_Zero = result == 0;
            Flag_Carry = shiftercarry;
        }

        private static void alu_and(byte Rdest, UInt32 op1_val, UInt32 op2_val, bool updateFlags, bool writeback, bool shiftercarry)
        {
            UInt32 result = op1_val & op2_val;
            if (writeback)
            {
                regs[Rdest] = result;
            }

            if (updateFlags)
            {
                update_flags_logical(result, shiftercarry);
            }
        }

        private static void alu_xor(byte Rdest, UInt32 op1_val, UInt32 op2_val, bool updateFlags, bool writeback, bool shiftercarry)
        {
            UInt32 result = op1_val ^ op2_val;

            if (writeback)
            {
                regs[Rdest] = result;
            }

            if (updateFlags)
            {
                update_flags_logical(result, shiftercarry);
            }
        }

        private static void alu_or(byte Rdest, UInt32 op1_val, UInt32 op2_val, bool updateFlags, bool shiftercarry)
        {
            UInt32 result = op1_val | op2_val;
            regs[Rdest] = result;

            if (updateFlags)
            {
                update_flags_logical(result, shiftercarry);
            }
        }

        private static void alu_and_not(byte Rdest, UInt32 op1_val, UInt32 op2_val, bool updateFlags, bool shiftercarry)
        {
            UInt32 result = op1_val & (~op2_val);
            regs[Rdest] = result;

            if (updateFlags)
            {
                update_flags_logical(result, shiftercarry);
            }
        }

        private static void alu_mov(byte Rdest, UInt32 op2_val, bool updateFlags, bool shiftercarry)
        {
            UInt32 result = op2_val;
            regs[Rdest] = result;

            if (updateFlags)
            {
                update_flags_logical(result, shiftercarry);
            }
        }

        private static void alu_mov_not(byte Rdest, UInt32 op2_val, bool updateFlags, bool shiftercarry)
        {
            UInt32 result = ~op2_val;
            regs[Rdest] = result;

            if (updateFlags)
            {
                update_flags_logical(result, shiftercarry);
            }
        }

        private static void alu_add(byte Rdest, UInt32 op1_val, UInt32 op2_val, bool updateFlags, bool writeback)
        {
            Int64 result = (Int64)op1_val + (Int64)op2_val;

            if (writeback)
            {
                regs[Rdest] = (UInt32)result;
            }

            if (updateFlags)
            {
                Flag_Negative = ((result >> 31) & 1) == 1;
                Flag_Zero = (UInt32)result == 0;
                Flag_V_Overflow = ((op1_val ^ (UInt32)result) & (op2_val ^ (UInt32)result) & 0x80000000) != 0;
                Flag_Carry = (result >> 32) == 1;
            }
        }
        
        private static void alu_add_withcarry(byte Rdest, UInt32 op1_val, UInt32 op2_val, bool updateFlags, bool carry)
        {
            Int64 result = (Int64)(op1_val + (Int64)op2_val + Convert.ToInt64(carry));
            regs[Rdest] = (UInt32)result;

            if (updateFlags)
            {
                Flag_Negative = ((result >> 31) & 1) == 1;
                Flag_Zero = (UInt32)result == 0;
                Flag_V_Overflow = ((op1_val ^ (UInt32)result) & (op2_val ^ (UInt32)result) & 0x80000000) != 0;
                Flag_Carry = (result >> 32) == 1;
            }
        }

        private static void alu_sub(byte Rdest, UInt32 op1_val, UInt32 op2_val, bool updateFlags, bool writeback)
        {
            UInt32 result = op1_val - op2_val;

            if (writeback)
            {
                regs[Rdest] = result;
            }

            if (updateFlags)
            {
                Flag_Negative = ((result >> 31) & 1) == 1;
                Flag_Zero = result == 0;
                // subs -> carry is 0 if borror, 1 otherwise
                Flag_V_Overflow = ((op1_val & 0x80000000) != (op2_val & 0x80000000) && (op1_val & 0x80000000) != ((UInt32)result & 0x80000000));
                Flag_Carry = op1_val >= op2_val;
            }
        }

        private static void alu_sub_withcarry(byte Rdest, UInt32 op1_val, UInt32 op2_val, bool updateFlags, bool carry)
        {
            Int64 result = (Int64)(op1_val - (Int64)op2_val + Convert.ToInt64(carry) - 1);
            regs[Rdest] = (UInt32)result;

            if (updateFlags)
            {
                Flag_Negative = ((result >> 31) & 1) == 1;
                Flag_Zero = (UInt32)result == 0;
                Flag_V_Overflow = ((op1_val & 0x80000000) != (op2_val & 0x80000000) && (op1_val & 0x80000000) != ((UInt32)result & 0x80000000));
                Flag_Carry = (Int64)(op1_val + Convert.ToInt64(Flag_Carry) - 1) >= op2_val;
            }
        }

        private static void data_processing(bool imm, byte opcode, bool s_updateflags, byte Rn_op1, byte Rdest, UInt16 Op2, UInt32 asmcmd)
        {
            if (!s_updateflags && opcode >= 8 && opcode <= 11) // PSR Transfer
            {
                data_processing_PSR(asmcmd);
                return;
            }

            UInt32 op1_val = regs[Rn_op1];
            UInt32 op2_val;
            bool shiftercarry;
            if (imm)
            {
                int shiftamount = (int)((Op2 >> 8) & 0xF) * 2;
                op2_val = (UInt32)(Op2 & 0xFF);
                shiftercarry = ((op2_val >> (shiftamount - 1)) & 1) == 1;
                op2_val = RotateRight(op2_val, shiftamount);
                if (shiftamount == 0)
                {
                    shiftercarry = Flag_Carry;
                }
            }
            else
            {
                bool regbased = shifter(out shiftercarry, out op2_val, (UInt16)(asmcmd & 0xFFF));
                if (Rn_op1 == 15 && regbased)
                {
                    op1_val += 4;
                }
            }

            UInt32 old_cpsr = get_CPSR();

            bool writeback = false;
            switch (opcode)
            {
                case 0x0: alu_and(Rdest, op1_val, op2_val, s_updateflags, true, shiftercarry); writeback = true; break;             // AND 0000 operand1 AND operand2
                case 0x1: alu_xor(Rdest, op1_val, op2_val, s_updateflags, true, shiftercarry); writeback = true; break;             // EOR 0001 operand1 EOR operand2
                case 0x2: alu_sub(Rdest, op1_val, op2_val, s_updateflags, true); writeback = true; break;                           // SUB 0010 operand1 - operand2
                case 0x3: alu_sub(Rdest, op2_val, op1_val, s_updateflags, true); writeback = true; break;                           // RSB 0011 operand2 - operand1
                case 0x4: alu_add(Rdest, op1_val, op2_val, s_updateflags, true);  writeback = true;  break;                         // ADD 0100 operand1 + operand2
                case 0x5: alu_add_withcarry(Rdest, op1_val, op2_val, s_updateflags, Flag_Carry); writeback = true; break;           // ADC 0101 operand1 + operand2 + carry
                case 0x6: alu_sub_withcarry(Rdest, op1_val, op2_val, s_updateflags, Flag_Carry); writeback = true; break;           // SBC 0110 operand1 - operand2 + carry - 1
                case 0x7: alu_sub_withcarry(Rdest, op2_val, op1_val, s_updateflags, Flag_Carry); writeback = true; break;           // RSC 0111 operand2 - operand1 + carry - 1
                case 0x8: alu_and(Rdest, op1_val, op2_val, s_updateflags, false, shiftercarry); s_updateflags = false; break;       // TST 1000 as AND, but result is not written
                case 0x9: alu_xor(Rdest, op1_val, op2_val, s_updateflags, false, shiftercarry); s_updateflags = false; break;       // TEQ 1001 as EOR, but result is not written
                case 0xA: alu_sub(Rdest, op1_val, op2_val, s_updateflags, false); s_updateflags = false; break;                     // CMP 1010 as SUB, but result is not written
                case 0xB: alu_add(Rdest, op1_val, op2_val, s_updateflags, false); s_updateflags = false; break;                     // CMN 1011 as ADD, but result is not written
                case 0xC: alu_or(Rdest, op1_val, op2_val, s_updateflags, shiftercarry); writeback = true; break;                    // ORR 1100 operand1 OR operand2
                case 0xD: alu_mov(Rdest, op2_val, s_updateflags, shiftercarry); writeback = true; break;                            // MOV 1101 operand2(operand1 is ignored)
                case 0xE: alu_and_not(Rdest, op1_val, op2_val, s_updateflags, shiftercarry); writeback = true; break;               // BIC 1110 operand1 AND NOT operand2(Bit clear)
                case 0xF: alu_mov_not(Rdest, op2_val, s_updateflags, shiftercarry); writeback = true; break;                        // MVN 1111 NOT operand2(operand1 is ignored)
            }

            if (writeback)
            {
                if (Rdest == 15)
                {
                    PC = (UInt32)regs[15];
                    newticks = BusTiming.codeTicksAccessSeq32(PC) + 1;
                    newticks = (newticks * 2) + BusTiming.codeTicksAccess32(PC) + 1;
                    if (s_updateflags) // leave IRP
                    {
                        if (cpu_mode == CPUMODES.SYSTEM || cpu_mode == CPUMODES.USER)
                        {
                            set_CPSR(old_cpsr);
                            CPUSwitchMode(cpu_mode, false);
                        }
                        else
                        {
                            UInt32 value = regs[17];
                            CPUSwitchMode(get_mode_from_value(value), false);
                            set_CPSR(value);
                        }
                        PC -= 4;
                        if (thumbmode)
                        {
                            PC &= 0xFFFFFFFE;
                            newticks = BusTiming.codeTicksAccessSeq16(PC) + 1;
                            newticks = (newticks * 2) + BusTiming.codeTicksAccess16(PC) + 1;
                        }
                        else
                        {
                            PC &= 0xFFFFFFFC;
                        }
                        return;
                    }
                    PC &= 0xFFFFFFFC;
                    PC -= 4;
                }
            }
        }

        public static int get_mode_index(CPUMODES mode)
        {
            switch (mode)
            {
                case CPUMODES.USER: return 0;
                case CPUMODES.FIQ: return 1;
                case CPUMODES.IRQ: return 2;
                case CPUMODES.SUPERVISOR: return 3;
                case CPUMODES.ABORT: return 4;
                case CPUMODES.UNDEFINED: return 5;
                case CPUMODES.SYSTEM: return 0;
            }

            return 0;
        }

        public static CPUMODES get_mode_from_value(uint value)
        {
            switch (value & 0x1F)
            {
                case 0x10: return CPUMODES.USER;
                case 0x11: return CPUMODES.FIQ;
                case 0x12: return CPUMODES.IRQ;
                case 0x13: return CPUMODES.SUPERVISOR;
                case 0x17: return CPUMODES.ABORT;
                case 0x1B: return CPUMODES.UNDEFINED;
                case 0x1F: return CPUMODES.SYSTEM;
            }

            return CPUMODES.USER;
        }

        public static UInt32 get_CPSR()
        {
            UInt32 value = 0;

            if (Flag_Negative) value |= 0x80000000;
            if (Flag_Zero) value |= 0x40000000;
            if (Flag_Carry) value |= 0x20000000;
            if (Flag_V_Overflow) value |= 0x10000000;

            if (IRQ_disable) value |= 0x80;
            if (FIQ_disable) value |= 0x40;

            if (thumbmode) value |= 0x20;

            switch (cpu_mode)
            {
                case CPUMODES.USER:       value |= 0x10; break;
                case CPUMODES.FIQ:        value |= 0x11; break;
                case CPUMODES.IRQ:        value |= 0x12; break;
                case CPUMODES.SUPERVISOR: value |= 0x13; break;
                case CPUMODES.ABORT:      value |= 0x17; break;
                case CPUMODES.UNDEFINED:  value |= 0x1B; break;
                case CPUMODES.SYSTEM:     value |= 0x1F; break;        
            }

            return value;
        }

        public static void set_CPSR(UInt32 value)
        {
            Flag_Negative = (value & 0x80000000) == 0x80000000;
            Flag_Zero = (value & 0x40000000) == 0x40000000;
            Flag_Carry = (value & 0x20000000) == 0x20000000;
            Flag_V_Overflow = (value & 0x10000000) == 0x10000000;

            FIQ_disable = (value & 0x40) == 0x40;
            IRQ_disable = (value & 0x80) == 0x80;
            thumbmode = (value & 0x20) == 0x20;
        }

        private static void CPUSwitchMode(CPUMODES mode, bool saveState)
        {
            switch (cpu_mode)
            {
                case CPUMODES.USER:
                case CPUMODES.SYSTEM:
                    if (mode == CPUMODES.FIQ)
                    {
                        regbanks[0, 8] = regs[8];
                        regbanks[0, 9] = regs[9];
                        regbanks[0, 10] = regs[10];
                        regbanks[0, 11] = regs[11];
                        regbanks[0, 12] = regs[12];
                    }
                    regbanks[0, 13] = regs[13];
                    regbanks[0, 14] = regs[14];
                    regs[17] = get_CPSR();
                    break;
                case CPUMODES.FIQ:
                    regbanks[1, 8] = regs[8];
                    regbanks[1, 9] = regs[9];
                    regbanks[1, 10] = regs[10];
                    regbanks[1, 11] = regs[11];
                    regbanks[1, 12] = regs[12];
                    regbanks[1, 13] = regs[13];
                    regbanks[1, 14] = regs[14];
                    regbanks[1, 17] = regs[17];
                    break;
                case CPUMODES.IRQ:
                    regbanks[2, 13] = regs[13];
                    regbanks[2, 14] = regs[14];
                    regbanks[2, 17] = regs[17];
                    break;
                case CPUMODES.SUPERVISOR:
                    regbanks[3, 13] = regs[13];
                    regbanks[3, 14] = regs[14];
                    regbanks[3, 17] = regs[17];
                    break;
                case CPUMODES.ABORT:
                    regbanks[4, 13] = regs[13];
                    regbanks[4, 14] = regs[14];
                    regbanks[4, 17] = regs[17];
                    break;
                case CPUMODES.UNDEFINED:
                    regbanks[5, 13] = regs[13];
                    regbanks[5, 14] = regs[14];
                    regbanks[5, 17] = regs[17];
                    break;
            }

            UInt32 newvalue;

            switch (mode)
            {
                case CPUMODES.USER:
                case CPUMODES.SYSTEM:
                    if (cpu_mode == CPUMODES.FIQ)
                    {
                        regs[8] = regbanks[0, 8];
                        regs[9] = regbanks[0, 9];
                        regs[10] = regbanks[0, 10];
                        regs[11] = regbanks[0, 11];
                        regs[12] = regbanks[0, 12];
                    }
                    regs[13] = regbanks[0, 13];
                    regs[14] = regbanks[0, 14];
                    set_CPSR(regs[17]);
                    break;
                case CPUMODES.FIQ: // should also save 8..12
                    regs[8] = regbanks[1, 8];
                    regs[9] = regbanks[1, 9];
                    regs[10] = regbanks[1, 10];
                    regs[11] = regbanks[1, 11];
                    regs[12] = regbanks[1, 12];
                    regs[13] = regbanks[1, 13];
                    regs[14] = regbanks[1, 14];
                    if (saveState) { regs[17] = get_CPSR(); } else { regs[17] = regbanks[1, 17]; }
                    break;
                case CPUMODES.IRQ:
                    regs[13] = regbanks[2, 13];
                    regs[14] = regbanks[2, 14];
                    newvalue = regs[17];
                    if (saveState) { regs[17] = get_CPSR(); } else { regs[17] = regbanks[2, 17]; }
                    set_CPSR(newvalue);
                    break;
                case CPUMODES.SUPERVISOR:
                    regs[13] = regbanks[3, 13];
                    regs[14] = regbanks[3, 14];
                    newvalue = regs[17];
                    if (saveState) { regs[17] = get_CPSR(); } else { regs[17] = regbanks[3, 17]; }
                    set_CPSR(newvalue);
                    break;
                case CPUMODES.ABORT:
                    regs[13] = regbanks[4, 13];
                    regs[14] = regbanks[4, 14];
                    newvalue = regs[17];
                    if (saveState) { regs[17] = get_CPSR(); } else { regs[17] = regbanks[4, 17]; }
                    set_CPSR(newvalue);
                    break;
                case CPUMODES.UNDEFINED:
                    regs[13] = regbanks[5, 13];
                    regs[14] = regbanks[5, 14];
                    newvalue = regs[17];
                    if (saveState) { regs[17] = get_CPSR(); } else { regs[17] = regbanks[5, 17]; }
                    set_CPSR(newvalue);
                    break;
            }
            cpu_mode = mode;
        }

        public static UInt32 RotateRight(UInt32 x, int n)
        {
            return (((x) >> (n)) | ((x) << (32 - (n))));
        }

    }
}
