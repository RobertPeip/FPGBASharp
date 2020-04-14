using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gbemu
{
    public class Pixel
    {
        public Byte color_red;
        public Byte color_green;
        public Byte color_blue;
        public bool transparent;
        public byte prio;
        public bool undrawn;
        public bool objwnd;
        public bool alpha;

        public Pixel()
        {
        }

        public void update(Byte color_red, Byte color_green, Byte color_blue)
        {
            this.color_red = color_red;
            this.color_green = color_green;
            this.color_blue = color_blue;
        }

        public void copy(Pixel source)
        {
            this.color_red = source.color_red;
            this.color_green = source.color_green;
            this.color_blue = source.color_blue;
            this.transparent = source.transparent;
            this.undrawn = source.undrawn;
            this.prio = source.prio;
            this.objwnd = source.objwnd;
            this.alpha = source.alpha;
        }

        public void copycolor(Pixel source)
        {
            this.color_red = source.color_red;
            this.color_green = source.color_green;
            this.color_blue = source.color_blue;
        }

        public void mixcolor(Pixel source, Pixel secondpixel)
        {
            color_red = (byte)(Math.Min(255, (source.color_red + secondpixel.color_red) >> 1));
            color_green = (byte)(Math.Min(255, (source.color_green + secondpixel.color_green) >> 1));
            color_blue = (byte)(Math.Min(255, (source.color_blue + secondpixel.color_blue) >> 1));
        }

        public void calcalpha(Pixel secondpixel, byte eva, byte evb)
        {
            color_red = (byte)(Math.Min(255, (color_red * eva + secondpixel.color_red * evb) >> 4));
            color_green = (byte)(Math.Min(255, (color_green * eva + secondpixel.color_green * evb) >> 4));
            color_blue = (byte)(Math.Min(255, (color_blue * eva + secondpixel.color_blue * evb) >> 4));
        }

        public void calcalpha(byte color2_red, byte color2_green, byte color2_blue, byte eva, byte evb)
        {
            color_red = (byte)(Math.Min(255, (color_red * eva + color2_red * evb) >> 4));
            color_green = (byte)(Math.Min(255, (color_green * eva + color2_green * evb) >> 4));
            color_blue = (byte)(Math.Min(255, (color_blue * eva + color2_blue * evb) >> 4));
        }

        public void whiter(byte change)
        {
            color_red = (byte)Math.Min(255, color_red + (((255 - color_red) * change) >> 4));
            color_green = (byte)Math.Min(255, color_green + (((255 - color_green) * change) >> 4));
            color_blue = (byte)Math.Min(255, color_blue + (((255 - color_blue) * change) >> 4));
        }

        public void blacker(byte change)
        {
            color_red = (byte)Math.Max(0, color_red - ((color_red * change) >> 4));
            color_green = (byte)Math.Max(0, color_green - ((color_green * change) >> 4));
            color_blue = (byte)Math.Max(0, color_blue - ((color_blue * change) >> 4));
        }

        public void colorshader()
        {
            //ld[i] = i / 256.0;
            //lx[i] = Math.Pow(ld[i], 2.2);
            //ly[i] = Math.Pow(lx[i], 1 / 2.2);
            //lz[i] = (int)Math.Round(ly[i] * 256);

            //out_r = 0.845 * r + 0.170 * g - 0.015 * b
            //out_g = 0.090 * r + 0.680 * g + 0.230 * b
            //out_b = 0.160 * r + 0.085 * g + 0.755 * b

            // double direct calc
            //double red_d = color_red / 256.0;
            //double green_d = color_green / 256.0;
            //double blue_d = color_blue / 256.0;
            //
            //double red_x = Math.Pow(red_d, 2.2);
            //double green_x = Math.Pow(green_d, 2.2);
            //double blue_x = Math.Pow(blue_d, 2.2);
            //
            //double red_y = 0.845 * red_x + 0.170 * green_x - 0.015 * blue_x;
            //double green_y = 0.090 * red_x + 0.680 * green_x + 0.230 * blue_x;
            //double blue_y = 0.160 * red_x + 0.085 * green_x + 0.755 * blue_x;
            //
            //double red_z = Math.Pow(red_y, 1 / 2.2);
            //double green_z = Math.Pow(green_y, 1 / 2.2);
            //double blue_z = Math.Pow(blue_y, 1 / 2.2);
            //
            //color_red = (byte)Math.Min(255, Math.Round(red_z * 256));
            //color_green = (byte)Math.Min(255, Math.Round(green_z * 256));
            //color_blue = (byte)Math.Min(255, Math.Round(blue_z * 256));

            // integer lookup
            //int red_x = GPU.shade_lookup_linear[color_red / 8] ;
            //int green_x = GPU.shade_lookup_linear[color_green / 8] ;
            //int blue_x = GPU.shade_lookup_linear[color_blue / 8] ;
            //
            //int red_y = 865 * red_x + 174 * green_x - 015 * blue_x;
            //int green_y = 92 * red_x + 696 * green_x + 236 * blue_x;
            //int blue_y = 164 * red_x + 87 * green_x + 773 * blue_x;
            //
            //red_y = Math.Max(0, Math.Min(1023, red_y / 1024));
            //green_y = Math.Max(0, Math.Min(1023, green_y / 1024));
            //blue_y = Math.Max(0, Math.Min(1023, blue_y / 1024));
            //
            //color_red = (byte)(GPU.shade_lookup_rgb[red_y] * 4);
            //color_green = (byte)(GPU.shade_lookup_rgb[green_y] * 4);
            //color_blue = (byte)(GPU.shade_lookup_rgb[blue_y] * 4);

            // integer border lookup
            int red_x = GPU.shade_lookup_linear[color_red / 8] ;
            int green_x = GPU.shade_lookup_linear[color_green / 8] ;
            int blue_x = GPU.shade_lookup_linear[color_blue / 8] ;
            
            int red_y = 865 * red_x + 174 * green_x - 015 * blue_x;
            int green_y = 92 * red_x + 696 * green_x + 236 * blue_x;
            int blue_y = 164 * red_x + 87 * green_x + 773 * blue_x;
            
            red_y = Math.Max(0, Math.Min(1023, red_y / 1024));
            green_y = Math.Max(0, Math.Min(1023, green_y / 1024));
            blue_y = Math.Max(0, Math.Min(1023, blue_y / 1024));

            color_red = 0;
            color_green = 0;
            color_blue = 0;
            for (int i = 0; i < 64; i++) { if (red_y   >= GPU.shade_lookup_rgb_border[i]) { color_red   = (byte)(i * 4); } }
            for (int i = 0; i < 64; i++) { if (green_y >= GPU.shade_lookup_rgb_border[i]) { color_green = (byte)(i * 4); } }
            for (int i = 0; i < 64; i++) { if (blue_y  >= GPU.shade_lookup_rgb_border[i]) { color_blue  = (byte)(i * 4); } }
        }

    }

    public static class GPU
    {
        enum WINDOWSTATE
        {
            Init,
            Win0,
            Win1,
            Obj,
            Out
        }

        public static bool lockSpeed = true;
        public static int speedmult = 1;
        public static int frameskip = 0;
        public static int frameskip_counter = 0;
        public static bool interlace_blending = false;
        public static bool colorshader = false;
        public static bool doubleres = false;
        public static bool SSAA4x = false;
        public static bool even_frame;

        private static object drawlock;

        public static UInt64 intern_frames;
        public static UInt64 videomode_frames;
        public static bool framebuffer_select;

        // registers
        public static byte videomode;
        public static bool forcedblank;

        // static array, so less allocate
        public static Pixel[,] pixels = new Pixel[480, 320];
        public static Pixel[,] pixels_interlace = new Pixel[480, 320];

        public static Pixel[] pixels_bg0 = new Pixel[240];
        public static Pixel[] pixels_bg1 = new Pixel[240];
        public static Pixel[] pixels_bg2   = new Pixel[480];
        public static Pixel[] pixels_bg2_1 = new Pixel[480];
        public static Pixel[] pixels_bg2_2 = new Pixel[480];
        public static Pixel[] pixels_bg3 = new Pixel[240];
        public static Pixel[] pixels_obj = new Pixel[240];
        public static Pixel pixelbackdrop;
        public static Pixel pixelspecial;
        public static Pixel pixelalpha2;
        public static Pixel pixelfinal;

        public static Sprite[] spritelist = new Sprite[128];

        private static Stopwatch stopwatch_frame = new Stopwatch();

        // on delay
        private static bool[] on_delay_bg0 = new bool[3];
        private static bool[] on_delay_bg1 = new bool[3];
        private static bool[] on_delay_bg2 = new bool[3];
        private static bool[] on_delay_bg3 = new bool[3];

        // affine bg
        private static Int32 ref2_x;
        private static Int32 ref2_y;
        private static Int32 ref3_x;
        private static Int32 ref3_y;

        private static Int32 ref2_x_next;
        private static Int32 ref2_y_next;
        private static bool ref2_x_new;
        private static bool ref2_y_new;
        private static Int32 ref3_x_next;
        private static Int32 ref3_y_next;
        private static bool ref3_x_new;
        private static bool ref3_y_new;

        // mosaic
        private static byte mosaik_bg0_vcnt;
        private static byte mosaik_bg1_vcnt;
        private static byte mosaik_bg2_vcnt;
        private static byte mosaik_bg3_vcnt;

        // colorshade
        public static int[] shade_lookup_linear = new int[32];
        public static int[] shade_lookup_rgb = new int[1024];
        public static int[] shade_lookup_rgb_border = new int[64];

        // frametime
        const long FRAMETIME = (1000000 / 60);
        private static long frametimeleft;

        public class Sprite
        {
            public int xpos;
            public int ypos;
            public byte tileid;
            public bool bg_prio;
            public bool yflip;
            public bool xflip;
            public bool second_map;
            public byte palette;

            public void update(int xpos, int ypos, byte tileid, bool bg_prio, bool yflip, bool xflip, bool second_map, byte palette)
            {
                this.xpos = xpos;
                this.ypos = ypos;
                this.tileid = tileid;
                this.bg_prio = bg_prio;
                this.yflip = yflip;
                this.xflip = xflip;
                this.second_map = second_map;
                this.palette = palette;
            }
        }

        public static void reset()
        {
#if DEBUG
            lockSpeed = false;
#endif
            forcedblank = true;

            drawlock = new object();

            intern_frames = 0;
            for (int i = 0; i < 10; i++)
            {
                spritelist[i] = new Sprite();
            }

            for (int y = 0; y < 320; y++)
            {
                for (int x = 0; x < 480; x++)
                {
                    pixels[x, y] = new Pixel();
                }
            }

            for (int y = 0; y < 160; y++)
            {
                for (int x = 0; x < 240; x++)
                {
                    pixels_bg0[x] = new Pixel();
                    pixels_bg1[x] = new Pixel();
                    pixels_bg3[x] = new Pixel();
                    pixels_obj[x] = new Pixel();
                    pixels_interlace[x, y] = new Pixel();
                }
                for (int x = 0; x < 480; x++)
                {
                    pixels_bg2_1[x] = new Pixel();
                    pixels_bg2_2[x] = new Pixel();
                }
            }

            pixelbackdrop = new Pixel();
            pixelbackdrop.transparent = false;
            pixelbackdrop.prio = 4;

            pixelspecial = new Pixel();
            pixelalpha2 = new Pixel();
            pixelfinal = new Pixel();

            frametimeleft = FRAMETIME;
            stopwatch_frame.Start();


            // color shader

            //int[] lx = new int[256];
            //int[] ly = new int[256];
            //int[] lz = new int[256];
            //
            int errorcount = 0;
            //
            //for (int i = 0; i < 256; i++)
            //{
            //    lx[i] = (int)Math.Round((Math.Pow(i / 256.0, 2.2) * 1024));
            //    ly[i] = (int)Math.Round(Math.Pow(lx[i] / 1024.0, 1 / 2.2) * 256); 
            //
            //    if (ly[i] != i)
            //    {
            //        errorcount++;
            //    }
            //}

            double gamma = 2.2;

            // basic calc
            for (int i = 0; i < 32; i++) 
            {
                shade_lookup_linear[i] = (int)Math.Round((Math.Pow(i / 32.0, gamma) * 1024.0));
            }
            // cheat to not have banding
            for (int i = 1; i < 31; i++) 
            {
                if (shade_lookup_linear[i - 1] == shade_lookup_linear[i])
                {
                    shade_lookup_linear[i]++;
                }
            }

            // basic calc
            for (int i = 0; i < 1024; i++)
            {
                shade_lookup_rgb[i] = 0;
            }
            for (int i = 0; i < 1024; i+= 16)
            {
                int linear = (int)Math.Round((Math.Pow(i / 1024.0, gamma) * 1024.0));
                shade_lookup_rgb[linear] = (int)Math.Round(Math.Pow(linear / 1024.0, 1 / gamma) * 64);
            }
            // fill empty spaces
            int holdvalue = 0;
            for (int i = 0; i < 1024; i++)
            {
                if (shade_lookup_rgb[i] == 0)
                {
                    shade_lookup_rgb[i] = holdvalue;
                }
                else
                {
                    holdvalue = shade_lookup_rgb[i];
                }
            }
            // cheat to not have banding
            for (int i = 1; i < 1023; i++)
            {
                if (shade_lookup_rgb[i] > (shade_lookup_rgb[i - 1] + 1))
                {
                    shade_lookup_rgb[i] = shade_lookup_rgb[i - 1] + 1;
                }
            }

            // calculate borders
            holdvalue = 0;
            int holdindex = 1;
            for (int i = 0; i < 1024; i++)
            {
                if (shade_lookup_rgb[i] != holdvalue)
                {
                    holdvalue = shade_lookup_rgb[i];
                    shade_lookup_rgb_border[holdindex] = i;
                    holdindex++;
                }
            }



            errorcount = 0;
            for (int i = 0; i < 32; i++)
            {
                int test = shade_lookup_linear[i];
                test = shade_lookup_rgb[test];
                if (test != i)
                {
                    errorcount++;
                }
            }

            string text = "";
            for (int x = 0; x < 32; x++)
            {
                text += shade_lookup_linear[x].ToString().PadLeft(4)+", ";
            }
            text += ";";
            for (int x = 0; x < 64; x++)
            {
                text += shade_lookup_rgb_border[x].ToString().PadLeft(4) + ", ";
            }

            //Clipboard.SetText(text);
        }

        public static void dispcnt_write()
        {
            videomode = (byte)GBRegs.Sect_display.DISPCNT_BG_Mode.read();

            bool new_forcedblank = GBRegs.Sect_display.DISPCNT_Forced_Blank.on();
            if (forcedblank && !new_forcedblank)
            {
                GPU_Timing.restart_line();
            }
            forcedblank = new_forcedblank;
        }

        public static void refpoint_update_all()
        {
            refpoint_update_2x();
            refpoint_update_2y();
            refpoint_update_3x();
            refpoint_update_3y();
        }

        public static void refpoint_update_2x()
        {
            ref2_x = (Int32)GBRegs.Sect_display.BG2RefX.read();
            if (((ref2_x >> 27) & 1) == 1) { ref2_x = (Int32)(ref2_x | 0xF0000000); }
            ref2_x_new = false;
        }
        public static void refpoint_update_2y()
        {
            ref2_y = (Int32)GBRegs.Sect_display.BG2RefY.read();
            if (((ref2_y >> 27) & 1) == 1) { ref2_y = (Int32)(ref2_y | 0xF0000000); }
            ref2_y_new = false;
        }
        public static void refpoint_update_3x()
        {
            ref3_x = (Int32)GBRegs.Sect_display.BG3RefX.read();
            if (((ref3_x >> 27) & 1) == 1) { ref3_x = (Int32)(ref3_x | 0xF0000000); }
        }
        public static void refpoint_update_3y()
        {
            ref3_y = (Int32)GBRegs.Sect_display.BG3RefY.read();
            if (((ref3_y >> 27) & 1) == 1) { ref3_y = (Int32)(ref3_y | 0xF0000000); }
        }

        public static void refpoint_update_2x_new()
        {
            if (GPU_Timing.gpustate == GPU_Timing.GPUState.VISIBLE)
            {
                ref2_x_next = (Int32)GBRegs.Sect_display.BG2RefX.read();
                if (((ref2_x_next >> 27) & 1) == 1) { ref2_x_next = (Int32)(ref2_x_next | 0xF0000000); }
                ref2_x_new = true;
            }
            else
            {
                refpoint_update_2x();
            }
        }
        public static void refpoint_update_2y_new()
        {
            if (GPU_Timing.gpustate == GPU_Timing.GPUState.VISIBLE)
            {
                ref2_y_next = (Int32)GBRegs.Sect_display.BG2RefY.read();
                if (((ref2_y_next >> 27) & 1) == 1) { ref2_y_next = (Int32)(ref2_y_next | 0xF0000000); }
                ref2_y_new = true;
            }
            else
            {
                refpoint_update_2y();
            }
        }
        public static void refpoint_update_3x_new()
        {
            if (GPU_Timing.gpustate == GPU_Timing.GPUState.VISIBLE)
            {
                ref3_x_next = (Int32)GBRegs.Sect_display.BG3RefX.read();
                if (((ref3_x_next >> 27) & 1) == 1) { ref3_x_next = (Int32)(ref3_x_next | 0xF0000000); }
                ref3_x_new = true;
            }
            else
            {
                refpoint_update_3x();
            }
        }
        public static void refpoint_update_3y_new()
        {
            if (GPU_Timing.gpustate == GPU_Timing.GPUState.VISIBLE)
            {
                ref3_y_next = (Int32)GBRegs.Sect_display.BG3RefY.read();
                if (((ref3_y_next >> 27) & 1) == 1) { ref3_y_next = (Int32)(ref3_y_next | 0xF0000000); }
                ref3_y_new = true;
            }
            else
            {
                refpoint_update_3y();
            }
        }

        public static void once_per_hblank()
        {
            bool DISPCNT_Screen_Display_BG0 = GBRegs.Sect_display.DISPCNT_Screen_Display_BG0.on();
            on_delay_bg0[2] = on_delay_bg0[1] && DISPCNT_Screen_Display_BG0;
            on_delay_bg0[1] = on_delay_bg0[0] && DISPCNT_Screen_Display_BG0;
            on_delay_bg0[0] = DISPCNT_Screen_Display_BG0;

            bool DISPCNT_Screen_Display_BG1 = GBRegs.Sect_display.DISPCNT_Screen_Display_BG1.on();
            on_delay_bg1[2] = on_delay_bg1[1] && DISPCNT_Screen_Display_BG1;
            on_delay_bg1[1] = on_delay_bg1[0] && DISPCNT_Screen_Display_BG1;
            on_delay_bg1[0] = DISPCNT_Screen_Display_BG1;

            bool DISPCNT_Screen_Display_BG2 = GBRegs.Sect_display.DISPCNT_Screen_Display_BG2.on();
            on_delay_bg2[2] = on_delay_bg2[1] && DISPCNT_Screen_Display_BG2;
            on_delay_bg2[1] = on_delay_bg2[0] && DISPCNT_Screen_Display_BG2;
            on_delay_bg2[0] = DISPCNT_Screen_Display_BG2;

            bool DISPCNT_Screen_Display_BG3 = GBRegs.Sect_display.DISPCNT_Screen_Display_BG3.on();
            on_delay_bg3[2] = on_delay_bg3[1] && DISPCNT_Screen_Display_BG3;
            on_delay_bg3[1] = on_delay_bg3[0] && DISPCNT_Screen_Display_BG3;
            on_delay_bg3[0] = DISPCNT_Screen_Display_BG3;
        }

        public static void next_line(byte line)
        {
            if (line == 0)
            {
                mosaik_bg0_vcnt = 0;
                mosaik_bg1_vcnt = 0;
                mosaik_bg2_vcnt = 0;
                mosaik_bg3_vcnt = 0;
                even_frame = !even_frame;
            }

            draw_line(line);
            if (line == 159)
            {
                GPU.frameskip_counter++;
                if (GPU.frameskip_counter > GPU.frameskip)
                {
                    GPU.frameskip_counter = 0;
                }
                long micros = 0;
                if (lockSpeed)
                {
                    while (micros < frametimeleft)
                    {
                        micros = 1000000 * stopwatch_frame.ElapsedTicks / Stopwatch.Frequency;
                        if (frametimeleft - micros > 1000)
                        {
                            //Thread.Sleep(1);
                        }
                    }
                    stopwatch_frame.Restart();
                    frametimeleft = Math.Max(1000, frametimeleft - micros + (FRAMETIME / speedmult));
                }
                intern_frames++;
                stopwatch_frame.Restart();

                if (videomode > 3)
                {
                    bool framebuffer_select_new = GBRegs.Sect_display.DISPCNT_Display_Frame_Select.on();
                    if (framebuffer_select_new != framebuffer_select)
                    {
                        framebuffer_select = framebuffer_select_new;
                        videomode_frames++;
                    }
                }
                else
                {
                    videomode_frames++;
                }
            }

            if ((videomode > 0 && GBRegs.Sect_display.DISPCNT_Screen_Display_BG2.on()))
            {
                Int16 BG2_DMX = (Int16)GBRegs.Sect_display.BG2RotScaleParDMX.read();
                Int16 BG2_DMY = (Int16)GBRegs.Sect_display.BG2RotScaleParDMY.read();
                ref2_x += BG2_DMX;
                ref2_y += BG2_DMY;
            }

            if (videomode == 2 && GBRegs.Sect_display.DISPCNT_Screen_Display_BG3.on())
            {
                Int16 BG3_DMX = (Int16)GBRegs.Sect_display.BG3RotScaleParDMX.read();
                Int16 BG3_DMY = (Int16)GBRegs.Sect_display.BG3RotScaleParDMY.read();
                //if (BG3_DMX == 0 && BG3_DMY == 0) { BG3_DMY = 0x100; }
                ref3_x += BG3_DMX;
                ref3_y += BG3_DMY;
            }
        }

        private static void draw_line(byte y_in)
        {
            if (frameskip_counter < frameskip)
            {
                return;
            }

            byte y = y_in;

            if (forcedblank)
            {
                lock (drawlock)
                {
                    for (Byte x = 0; x < 240; x++)
                    {
                        pixels[x, y].update(255, 255, 255);
                    }
                }
                return;
            }

            lock (drawlock)
            {
                UInt16 colorall = BitConverter.ToUInt16(Memory.PaletteRAM, 0);
                pixelbackdrop.update((Byte)((colorall & 0x1F) * 8), (byte)(((colorall >> 5) & 0x1F) * 8), (byte)(((colorall >> 10) & 0x1F) * 8));

                byte prio_bg0 = (byte)GBRegs.Sect_display.BG0CNT_BG_Priority.read();
                byte prio_bg1 = (byte)GBRegs.Sect_display.BG1CNT_BG_Priority.read();
                byte prio_bg2 = (byte)GBRegs.Sect_display.BG2CNT_BG_Priority.read();
                byte prio_bg3 = (byte)GBRegs.Sect_display.BG3CNT_BG_Priority.read();

                byte mosaic_bg_h = (byte)GBRegs.Sect_display.MOSAIC_BG_Mosaic_H_Size.read();
                byte mosaic_bg_v = (byte)GBRegs.Sect_display.MOSAIC_BG_Mosaic_V_Size.read();

                int pixelcount = 240;
                if (doubleres)
                {
                    pixelcount = 480;
                }

                if (on_delay_bg0[2])
                {
                    bool mosaic_on = GBRegs.Sect_display.BG0CNT_Mosaic.on();
                    if (!mosaic_on || mosaik_bg0_vcnt == 0)
                    {
                        for (int x = 0; x < 240; x++) 
                        { 
                            pixels_bg0[x].prio = prio_bg0;
                            pixels_bg0[x].transparent = true;
                        }
                        switch (videomode)
                        {
                            case 0:
                            case 1:
                                draw_bg_mode0(pixels_bg0,
                                    y,
                                    GBRegs.Sect_display.BG0CNT_Screen_Base_Block.read(),
                                    GBRegs.Sect_display.BG0CNT_Character_Base_Block.read(),
                                    GBRegs.Sect_display.BG0CNT_Colors_Palettes.on(),
                                    (byte)GBRegs.Sect_display.BG0CNT_Screen_Size.read(),
                                    (UInt16)GBRegs.Sect_display.BG0HOFS.read(),
                                    (UInt16)GBRegs.Sect_display.BG0VOFS.read());
                                break;
                        }
                        if (mosaic_on)
                        {
                            byte mosaic_h = 0;
                            for (int x = 0; x < 240; x++)
                            {
                                if (mosaic_h != 0)
                                {
                                    pixels_bg0[x].copy(pixels_bg0[x - mosaic_h]);
                                }
                                if (mosaic_h >= mosaic_bg_h) { mosaic_h = 0; } else mosaic_h++;
                            }
                        }
                    }
                    if (mosaik_bg0_vcnt >= mosaic_bg_v) { mosaik_bg0_vcnt = 0; } else mosaik_bg0_vcnt++;
                }
                else
                {
                    for (int x = 0; x < 240; x++)
                    {
                        pixels_bg0[x].transparent = true;
                    }
                }

                if(on_delay_bg1[2])
                {
                    bool mosaic_on = GBRegs.Sect_display.BG1CNT_Mosaic.on();
                    if (!mosaic_on || mosaik_bg1_vcnt == 0)
                    {
                        for (int x = 0; x < 240; x++) 
                        { 
                            pixels_bg1[x].prio = prio_bg1;
                            pixels_bg1[x].transparent = true;
                        }
                        switch (videomode)
                        {
                            case 0:
                            case 1:
                                draw_bg_mode0(pixels_bg1,
                                    y,
                                    GBRegs.Sect_display.BG1CNT_Screen_Base_Block.read(),
                                    GBRegs.Sect_display.BG1CNT_Character_Base_Block.read(),
                                    GBRegs.Sect_display.BG1CNT_Colors_Palettes.on(),
                                    (byte)GBRegs.Sect_display.BG1CNT_Screen_Size.read(),
                                    (UInt16)GBRegs.Sect_display.BG1HOFS.read(),
                                    (UInt16)GBRegs.Sect_display.BG1VOFS.read());
                                break;
                        }
                        if (mosaic_on)
                        {
                            byte mosaic_h = 0;
                            for (int x = 0; x < 240; x++)
                            {
                                if (mosaic_h != 0)
                                {
                                    pixels_bg1[x].copy(pixels_bg1[x - mosaic_h]);
                                }
                                if (mosaic_h >= mosaic_bg_h) { mosaic_h = 0; } else mosaic_h++;
                            }
                        }
                    }
                    if (mosaik_bg1_vcnt >= mosaic_bg_v) { mosaik_bg1_vcnt = 0; } else mosaik_bg1_vcnt++;
                }
                else
                {
                    for (int x = 0; x < 240; x++)
                    {
                        pixels_bg1[x].transparent = true;
                    }
                }

                if (on_delay_bg2[2])
                {
                    bool mosaic_on = GBRegs.Sect_display.BG2CNT_Mosaic.on();
                    if (!mosaic_on || mosaik_bg2_vcnt == 0)
                    {
                        for (int x = 0; x < pixelcount; x++)
                        {
                            pixels_bg2_1[x].prio = prio_bg2;
                            pixels_bg2_1[x].transparent = true;
                            if (doubleres)
                            {
                                pixels_bg2_2[x].prio = prio_bg2;
                                pixels_bg2_2[x].transparent = true;
                            }
                        }
                        switch (videomode)
                        {
                            case 0:
                                draw_bg_mode0(pixels_bg2_1,
                                    y,
                                    GBRegs.Sect_display.BG2CNT_Screen_Base_Block.read(),
                                    GBRegs.Sect_display.BG2CNT_Character_Base_Block.read(),
                                    GBRegs.Sect_display.BG2CNT_Colors_Palettes.on(),
                                    (byte)GBRegs.Sect_display.BG2CNT_Screen_Size.read(),
                                    (UInt16)GBRegs.Sect_display.BG2HOFS.read(),
                                    (UInt16)GBRegs.Sect_display.BG2VOFS.read());
                                break;
                            case 1:
                            case 2:
                                draw_bg_mode2(pixels_bg2_1,
                                    GBRegs.Sect_display.BG2CNT_Screen_Base_Block.read(),
                                    GBRegs.Sect_display.BG2CNT_Character_Base_Block.read(),
                                    GBRegs.Sect_display.BG2CNT_Display_Area_Overflow.on(),
                                    (byte)GBRegs.Sect_display.BG2CNT_Screen_Size.read(),
                                    ref2_x,
                                    ref2_y,
                                    (Int16)GBRegs.Sect_display.BG2RotScaleParDX.read(),
                                    (Int16)GBRegs.Sect_display.BG2RotScaleParDY.read(),
                                    doubleres,
                                    true);
                                if (doubleres)
                                {
                                    Int16 BG2_DMX = (Int16)GBRegs.Sect_display.BG2RotScaleParDMX.read();
                                    Int16 BG2_DMY = (Int16)GBRegs.Sect_display.BG2RotScaleParDMY.read();
                                    draw_bg_mode2(pixels_bg2_2,
                                        GBRegs.Sect_display.BG2CNT_Screen_Base_Block.read(),
                                        GBRegs.Sect_display.BG2CNT_Character_Base_Block.read(),
                                        GBRegs.Sect_display.BG2CNT_Display_Area_Overflow.on(),
                                        (byte)GBRegs.Sect_display.BG2CNT_Screen_Size.read(),
                                        ref2_x + BG2_DMX / 2,
                                        ref2_y + BG2_DMY / 2,
                                        (Int16)GBRegs.Sect_display.BG2RotScaleParDX.read(),
                                        (Int16)GBRegs.Sect_display.BG2RotScaleParDY.read(),
                                        doubleres,
                                        true);
                                }
                                break;
                            case 3:
                                draw_bg_mode3(
                            ref2_x,
                            ref2_y,
                            (Int16)GBRegs.Sect_display.BG2RotScaleParDX.read(),
                            (Int16)GBRegs.Sect_display.BG2RotScaleParDY.read());
                                break;
                            case 4:
                                draw_bg_mode4(
                            ref2_x,
                            ref2_y,
                            (Int16)GBRegs.Sect_display.BG2RotScaleParDX.read(),
                            (Int16)GBRegs.Sect_display.BG2RotScaleParDY.read());
                                break;
                            case 5:
                                draw_bg_mode5(
                            ref2_x,
                            ref2_y,
                            (Int16)GBRegs.Sect_display.BG2RotScaleParDX.read(),
                            (Int16)GBRegs.Sect_display.BG2RotScaleParDY.read());
                                break;
                        }
                        if (mosaic_on)
                        {
                            byte mosaic_h = 0;
                            for (int x = 0; x < 240; x++)
                            {
                                if (mosaic_h != 0)
                                {
                                    pixels_bg2[x].copy(pixels_bg2[x - mosaic_h]);
                                }
                                if (mosaic_h >= mosaic_bg_h) { mosaic_h = 0; } else mosaic_h++;
                            }
                        }
                    }
                    if (mosaik_bg2_vcnt >= mosaic_bg_v) { mosaik_bg2_vcnt = 0; } else mosaik_bg2_vcnt++;
                }
                else
                {
                    if (doubleres)
                    {
                        for (int x = 0; x < pixelcount; x++) { pixels_bg2_1[x].transparent = true; pixels_bg2_2[x].transparent = true; }
                        }
                    else
                    {
                        for (int x = 0; x < pixelcount; x++) pixels_bg2_1[x].transparent = true;
                    }
                }

                if (on_delay_bg3[2])
                {
                    bool mosaic_on = GBRegs.Sect_display.BG3CNT_Mosaic.on();
                    if (!mosaic_on || mosaik_bg3_vcnt == 0)
                    {
                        for (int x = 0; x < 240; x++) 
                        {
                            pixels_bg3[x].transparent = true;
                            pixels_bg3[x].prio = prio_bg3; 
                        }
                        switch (videomode)
                        {
                            case 0:
                                draw_bg_mode0(pixels_bg3,
                                    y,
                                    GBRegs.Sect_display.BG3CNT_Screen_Base_Block.read(),
                                    GBRegs.Sect_display.BG3CNT_Character_Base_Block.read(),
                                    GBRegs.Sect_display.BG3CNT_Colors_Palettes.on(),
                                    (byte)GBRegs.Sect_display.BG3CNT_Screen_Size.read(),
                                    (UInt16)GBRegs.Sect_display.BG3HOFS.read(),
                                    (UInt16)GBRegs.Sect_display.BG3VOFS.read());
                                break;
                            case 2:
                                draw_bg_mode2(pixels_bg3,
                                    GBRegs.Sect_display.BG3CNT_Screen_Base_Block.read(),
                                    GBRegs.Sect_display.BG3CNT_Character_Base_Block.read(),
                                    GBRegs.Sect_display.BG3CNT_Display_Area_Overflow.on(),
                                    (byte)GBRegs.Sect_display.BG3CNT_Screen_Size.read(),
                                    ref3_x,
                                    ref3_y,
                                    (Int16)GBRegs.Sect_display.BG3RotScaleParDX.read(),
                                    (Int16)GBRegs.Sect_display.BG3RotScaleParDY.read(),
                                    false,
                                    false);
                                break;
                        }
                        if (mosaic_on)
                        {
                            byte mosaic_h = 0;
                            for (int x = 0; x < 240; x++)
                            {
                                if (mosaic_h != 0)
                                {
                                    pixels_bg3[x].copy(pixels_bg3[x - mosaic_h]);
                                }
                                if (mosaic_h >= mosaic_bg_h) { mosaic_h = 0; } else mosaic_h++;
                            }
                        }
                    }
                    if (mosaik_bg3_vcnt >= mosaic_bg_v) { mosaik_bg3_vcnt = 0; } else mosaik_bg3_vcnt++;
                }
                else
                {
                    for (int x = 0; x < 240; x++)
                    {
                        pixels_bg3[x].transparent = true;
                    }
                }

                if (ref2_x_new == true) { ref2_x = ref2_x_next; ref2_x_new = false;}
                if (ref2_y_new == true) { ref2_y = ref2_y_next; ref2_y_new = false;}
                if (ref3_x_new == true) { ref3_x = ref3_x_next; ref3_x_new = false;}
                if (ref3_y_new == true) { ref3_y = ref3_y_next; ref3_y_new = false;}

                bool spriteon = GBRegs.Sect_display.DISPCNT_Screen_Display_OBJ.on();
                for (int x = 0; x < 240; x++)
                {
                    pixels_obj[x].transparent = true;
                }
                if (spriteon)
                {
                    draw_obj(y, 0x10000);
                }

                ////////////////////////
                // merge
                ////////////////////////
                ///

                // check windows on
                bool anywindow = false;

                bool inwin_0y = false;
                UInt32 win0_X1 = 0;
                UInt32 win0_X2 = 0;
                if (GBRegs.Sect_display.DISPCNT_Window_0_Display_Flag.on())
                {
                    anywindow = true;
                    UInt32 Y1 = GBRegs.Sect_display.WIN0V_Y1.read();
                    UInt32 Y2 = GBRegs.Sect_display.WIN0V_Y2.read();
                    if ((Y1 <= Y2 && y >= Y1 && y < Y2) || (Y1 > Y2 && (y >= Y1 || y < Y2)))
                    {
                        inwin_0y = true;
                        win0_X1 = GBRegs.Sect_display.WIN0H_X1.read();
                        win0_X2 = GBRegs.Sect_display.WIN0H_X2.read();
                    }
                }

                bool inwin_1y = false;
                UInt32 win1_X1 = 0;
                UInt32 win1_X2 = 0;
                if (GBRegs.Sect_display.DISPCNT_Window_1_Display_Flag.on())
                {
                    anywindow = true;
                    UInt32 Y1 = GBRegs.Sect_display.WIN1V_Y1.read();
                    UInt32 Y2 = GBRegs.Sect_display.WIN1V_Y2.read();
                    if ((Y1 <= Y2 && y >= Y1 && y < Y2) || (Y1 > Y2 && (y >= Y1 || y < Y2)))
                    {
                        inwin_1y = true;

                        win1_X1 = GBRegs.Sect_display.WIN1H_X1.read();
                        win1_X2 = GBRegs.Sect_display.WIN1H_X2.read();
                    }
                }

                bool objwindow_on = GBRegs.Sect_display.DISPCNT_OBJ_Wnd_Display_Flag.on() && spriteon;
                anywindow |= objwindow_on;

                byte enables_wnd0 = (byte)(GBRegs.Sect_display.WININ.read() & 0x1F);
                byte enables_wnd1 = (byte)((GBRegs.Sect_display.WININ.read() >> 8) & 0x1F);
                byte enables_out = (byte)(GBRegs.Sect_display.WINOUT.read() & 0x1F);
                byte enables_obj = (byte)((GBRegs.Sect_display.WINOUT.read() >> 8) & 0x1F);

                bool inwin_0_special = GBRegs.Sect_display.WININ_Window_0_Special_Effect.on();
                bool inwin_1_special = GBRegs.Sect_display.WININ_Window_1_Special_Effect.on();
                bool obj_special = GBRegs.Sect_display.WINOUT_Objwnd_Special_Effect.on();
                bool outside_special = GBRegs.Sect_display.WINOUT_Outside_Special_Effect.on();

                byte special_effect_base = (byte)GBRegs.Sect_display.BLDCNT_Color_Special_Effect.read();
                byte first_target = (byte)(GBRegs.Sect_display.BLDCNT.read() & 0x3F);
                byte second_target = (byte)((GBRegs.Sect_display.BLDCNT.read() >> 8) & 0x3F);
                byte bldy = (byte)Math.Min(16, GBRegs.Sect_display.BLDY.read());
                byte eva = (byte)Math.Min(16, GBRegs.Sect_display.BLDALPHA_EVA_Coefficient.read());
                byte evb = (byte)Math.Min(16, GBRegs.Sect_display.BLDALPHA_EVB_Coefficient.read());

                int step = 2;
                if (doubleres)
                {
                    step = 1;
                }

                pixels_bg2 = pixels_bg2_1;
                for (byte yi = 0; yi <= 2 - step; yi++)
                {
                    int yd = y;
                    if (doubleres)
                    {
                        yd = y * 2 + yi;
                        if (yi == 1 && (videomode == 1 || videomode == 2))
                        {
                            pixels_bg2 = pixels_bg2_2;
                        }
                    }
                    for (int xi = 0; xi < 480; xi += step)
                    {
                        int x = xi / 2;
                        int xd = xi / 2;
                        int x2 = xi / 2;
                        if (doubleres)
                        {
                            xd = xi;
                            if (videomode == 1 || videomode == 2)
                            {
                                x2 = x2 = xi;
                            }
                        }

                        // base
                        byte enables = 0;
                        if (!pixels_bg0[x].transparent) { enables |= 1; };
                        if (!pixels_bg1[x].transparent) { enables |= 2; };
                        if (!pixels_bg2[x2].transparent) { enables |= 4; };
                        if (!pixels_bg3[x].transparent) { enables |= 8; };
                        if (!pixels_obj[x].transparent) { enables |= 16; };

                        // window select
                        bool special_enable = true;
                        if (anywindow)
                        {
                            if (inwin_0y && ((win0_X1 <= win0_X2 && x >= win0_X1 && x < win0_X2) || ((win0_X1 > win0_X2 && (x >= win0_X1 || x < win0_X2)))))
                            {
                                enables &= enables_wnd0;
                                special_enable = inwin_0_special;
                            }
                            else if (inwin_1y && ((win1_X1 <= win1_X2 && x >= win1_X1 && x < win1_X2) || ((win1_X1 > win1_X2 && (x >= win1_X1 || x < win1_X2)))))
                            {
                                enables &= enables_wnd1;
                                special_enable = inwin_1_special;
                            }
                            else if (pixels_obj[x].objwnd)
                            {
                                enables &= enables_obj;
                                special_enable = obj_special;
                            }
                            else
                            {
                                enables &= enables_out;
                                special_enable = outside_special;
                            }
                        }
                        enables |= 0x20; //backdrop is always on

                        // priority
                        byte topprio = enables;
                        if ((topprio & 0x11) == 0x11 && pixels_obj[x].prio > pixels_bg0[x].prio) topprio &= 0xF;
                        if ((topprio & 0x12) == 0x12 && pixels_obj[x].prio > pixels_bg1[x].prio) topprio &= 0xF;
                        if ((topprio & 0x14) == 0x14 && pixels_obj[x].prio > pixels_bg2[x2].prio) topprio &= 0xF;
                        if ((topprio & 0x18) == 0x18 && pixels_obj[x].prio > pixels_bg3[x].prio) topprio &= 0xF;

                        if ((topprio & 0x01) == 0x1 && (topprio & 0x02) == 0x2 && pixels_bg0[x].prio > pixels_bg1[x].prio) topprio &= 0x1E;
                        if ((topprio & 0x01) == 0x1 && (topprio & 0x04) == 0x4 && pixels_bg0[x].prio > pixels_bg2[x2].prio) topprio &= 0x1E;
                        if ((topprio & 0x01) == 0x1 && (topprio & 0x08) == 0x8 && pixels_bg0[x].prio > pixels_bg3[x].prio) topprio &= 0x1E;
                        if ((topprio & 0x02) == 0x2 && (topprio & 0x04) == 0x4 && pixels_bg1[x].prio > pixels_bg2[x2].prio) topprio &= 0x1D;
                        if ((topprio & 0x02) == 0x2 && (topprio & 0x08) == 0x8 && pixels_bg1[x].prio > pixels_bg3[x].prio) topprio &= 0x1D;
                        if ((topprio & 0x04) == 0x4 && (topprio & 0x08) == 0x8 && pixels_bg2[x2].prio > pixels_bg3[x].prio) topprio &= 0x1B;

                        if ((topprio & 0x10) == 0x10) { topprio = 0x10; }
                        else if ((topprio & 0x01) == 0x01) { topprio = 0x01; }
                        else if ((topprio & 0x02) == 0x02) { topprio = 0x02; }
                        else if ((topprio & 0x04) == 0x04) { topprio = 0x04; }
                        else if ((topprio & 0x08) == 0x08) { topprio = 0x08; }
                        else { topprio = 0x20; }

                        bool special_out = false;
                        byte special_effect = special_effect_base;
                        if ((special_enable && special_effect > 0) || pixels_obj[x].alpha)
                        {
                            // special select
                            byte first = (byte)(enables & first_target);

                            if (pixels_obj[x].alpha)
                            {
                                first = (byte)(first | 0x10);
                            }

                            first = (byte)(first & topprio);

                            if (first != 0)
                            {
                                if (pixels_obj[x].alpha && first == 0x10)
                                {
                                    special_effect = 1;
                                }

                                byte second;
                                if (special_effect == 1)
                                {
                                    second = (byte)(enables & (~first));

                                    if ((second & 0x11) == 0x11) { if (pixels_obj[x].prio > pixels_bg0[x].prio) second &= 0x2F; else second &= 0x3E; }
                                    if ((second & 0x12) == 0x12) { if (pixels_obj[x].prio > pixels_bg1[x].prio) second &= 0x2F; else second &= 0x3D; }
                                    if ((second & 0x14) == 0x14) { if (pixels_obj[x].prio > pixels_bg2[x2].prio) second &= 0x2F; else second &= 0x3B; }
                                    if ((second & 0x18) == 0x18) { if (pixels_obj[x].prio > pixels_bg3[x].prio) second &= 0x2F; else second &= 0x37; }

                                    if ((second & 0x01) == 0x1 && (second & 0x02) == 0x2) { if (pixels_bg0[x].prio > pixels_bg1[x].prio) second &= 0x3E; else second &= 0x3D; }
                                    if ((second & 0x01) == 0x1 && (second & 0x04) == 0x4) { if (pixels_bg0[x].prio > pixels_bg2[x2].prio) second &= 0x3E; else second &= 0x3B; }
                                    if ((second & 0x01) == 0x1 && (second & 0x08) == 0x8) { if (pixels_bg0[x].prio > pixels_bg3[x].prio) second &= 0x3E; else second &= 0x37; }
                                    if ((second & 0x02) == 0x2 && (second & 0x04) == 0x4) { if (pixels_bg1[x].prio > pixels_bg2[x2].prio) second &= 0x3D; else second &= 0x3B; }
                                    if ((second & 0x02) == 0x2 && (second & 0x08) == 0x8) { if (pixels_bg1[x].prio > pixels_bg3[x].prio) second &= 0x3D; else second &= 0x37; }
                                    if ((second & 0x04) == 0x4 && (second & 0x08) == 0x8) { if (pixels_bg2[x2].prio > pixels_bg3[x].prio) second &= 0x3B; else second &= 0x37; }

                                    second = (byte)(second & second_target);

                                    if ((first & 0x10) == 0x10) { pixelspecial.copy(pixels_obj[x]); }
                                    else if ((first & 0x01) == 0x01) { pixelspecial.copy(pixels_bg0[x]); }
                                    else if ((first & 0x02) == 0x02) { pixelspecial.copy(pixels_bg1[x]); }
                                    else if ((first & 0x04) == 0x04) { pixelspecial.copy(pixels_bg2[x2]); }
                                    else if ((first & 0x08) == 0x08) { pixelspecial.copy(pixels_bg3[x]); }
                                    else { pixelspecial.transparent = true; }

                                    if ((second & 0x10) == 0x10) { pixelalpha2.copy(pixels_obj[x]); }
                                    else if ((second & 0x01) == 0x01) { pixelalpha2.copy(pixels_bg0[x]); }
                                    else if ((second & 0x02) == 0x02) { pixelalpha2.copy(pixels_bg1[x]); }
                                    else if ((second & 0x04) == 0x04) { pixelalpha2.copy(pixels_bg2[x2]); }
                                    else if ((second & 0x08) == 0x08) { pixelalpha2.copy(pixels_bg3[x]); }
                                    else if ((second & 0x20) == 0x20) { pixelalpha2.copy(pixelbackdrop); }
                                    else { pixelalpha2.transparent = true; }

                                    if (!pixelspecial.transparent && !pixelalpha2.transparent)
                                    {
                                        if (pixelalpha2.prio >= pixelspecial.prio)
                                        {
                                            pixelspecial.calcalpha(pixelalpha2, eva, evb);
                                            special_out = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((first & 0x10) == 0x10) { pixelspecial.copycolor(pixels_obj[x]); }
                                    else if ((first & 0x01) == 0x01) { pixelspecial.copycolor(pixels_bg0[x]); }
                                    else if ((first & 0x02) == 0x02) { pixelspecial.copycolor(pixels_bg1[x]); }
                                    else if ((first & 0x04) == 0x04) { pixelspecial.copycolor(pixels_bg2[x2]); }
                                    else if ((first & 0x08) == 0x08) { pixelspecial.copycolor(pixels_bg3[x]); }
                                    else { pixelspecial.copycolor(pixelbackdrop); }

                                    if (special_effect == 2)
                                    {
                                        pixelspecial.whiter(bldy);
                                    }
                                    else
                                    {
                                        pixelspecial.blacker(bldy);
                                    }

                                    special_out = true;
                                }
                            }
                        }

                        if (special_out)
                        {
                            pixelfinal.copycolor(pixelspecial);
                        }
                        else if ((topprio & 0x10) == 0x10)
                        {
                            pixelfinal.copycolor(pixels_obj[x]);
                        }
                        else if ((topprio & 0x01) == 0x01)
                        {
                            pixelfinal.copycolor(pixels_bg0[x]);
                        }
                        else if ((topprio & 0x02) == 0x02)
                        {
                            pixelfinal.copycolor(pixels_bg1[x]);
                        }
                        else if ((topprio & 0x04) == 0x04)
                        {
                            pixelfinal.copycolor(pixels_bg2[x2]);
                        }
                        else if ((topprio & 0x08) == 0x08)
                        {
                            pixelfinal.copycolor(pixels_bg3[x]);
                        }
                        else
                        {
                            pixelfinal.copycolor(pixelbackdrop);
                        }

                        // choose
                        if (interlace_blending)
                        {
                            pixels[xd, yd].mixcolor(pixelfinal, pixels_interlace[xd, yd]);
                            pixels_interlace[xd, yd].copycolor(pixelfinal);
                        }
                        else
                        {
                            pixels[xd, yd].copycolor(pixelfinal);
                        }

                        if (colorshader)
                        {
                            pixels[xd, yd].colorshader();
                        }

                    }
                }
                
            }
        }

        private static void draw_bg_mode0(Pixel[] pixelslocal, byte y, UInt32 mapbase, UInt32 tilebase, bool hicolor, byte screensize, UInt16 scrollX, UInt16 scrollY)
        {
            Int32 mapbaseaddr = (int)mapbase * 2048; // 2kb blocks
            Int32 tilebaseaddr = (int)tilebase * 0x4000; // 16kb blocks

            scrollX = (UInt16)(scrollX & 0x1FF);
            scrollY = (UInt16)(scrollY & 0x1FF);

            UInt16 y_scrolled = (UInt16)(y + scrollY);
            byte tilesize = screensize;
            int offset_y = 32;
            int scroll_x_mod = 256;
            int scroll_y_mod = 256;
            switch (screensize)
            {
                case 1: scroll_x_mod = 512; break;
                case 2: scroll_y_mod = 512; break;
                case 3: scroll_x_mod = 512; scroll_y_mod = 512; break;
            }
            y_scrolled = (UInt16)(y_scrolled % scroll_y_mod);
            offset_y = ((y_scrolled % 256) / 8) * offset_y;

            int tilemult;
            int x_flip_offset;
            int x_div;
            int x_size;
            if (!hicolor)
            {
                tilemult = 32;
                x_flip_offset = 3;
                x_div = 2;
                x_size = 4;
            }
            else
            {
                tilemult = 64;
                x_flip_offset = 7;
                x_div = 1;
                x_size = 8;
            }

            for (Byte x = 0; x < 240; x++)
            {
                UInt16 x_scrolled = (UInt16)(x + scrollX);
                x_scrolled = (UInt16)(x_scrolled % scroll_x_mod);

                int tileindex = 0;
                if (x_scrolled >= 256 || y_scrolled >= 256 && screensize == 2)
                {
                    tileindex += 1024;
                    x_scrolled = (UInt16)(x_scrolled % 256);
                }
                if (y_scrolled >= 256 && screensize == 3)
                {
                    tileindex += 2048;
                }
                tileindex += offset_y + (x_scrolled / 8);

                UInt16 tileinfo = BitConverter.ToUInt16(Memory.VRAM, Math.Min(mapbaseaddr + (tileindex * 2), 0x17FFF));
                UInt16 tilenr = (UInt16)(tileinfo & 0x3FF);
                bool horflip = ((tileinfo >> 10) & 1) == 1;
                bool verflip = ((tileinfo >> 11) & 1) == 1;
                int pixeladdr = tilebaseaddr + tilenr * tilemult;
                if (horflip)
                {
                    pixeladdr += (x_flip_offset - ((x_scrolled % 8) / x_div));
                }
                else
                {
                    pixeladdr += (x_scrolled % 8) / x_div;
                }
                if (verflip)
                {
                    pixeladdr += (7 - (y_scrolled % 8)) * x_size;
                }
                else
                {
                    pixeladdr += y_scrolled % 8 * x_size;
                }
                pixeladdr = Math.Min(pixeladdr, 0x17FFF);
                byte colordata = Memory.VRAM[pixeladdr];

                if (!hicolor)
                {
                    if (horflip && (x_scrolled & 1) == 0 || !horflip && (x_scrolled & 1) == 1)
                    {
                        colordata = (byte)(colordata >> 4);
                    }
                    else
                    {
                        colordata = (byte)(colordata & 0xF);
                    }

                    pixelslocal[x].transparent = colordata == 0;
                    if (!pixelslocal[x].transparent)
                    {
                        byte palette = (byte)(tileinfo >> 12);
                        UInt16 colorall = BitConverter.ToUInt16(Memory.PaletteRAM, palette * 32 + colordata * 2);
                        pixelslocal[x].update((Byte)((colorall & 0x1F) * 8), (byte)(((colorall >> 5) & 0x1F) * 8), (byte)(((colorall >> 10) & 0x1F) * 8));
                    }
                }
                else
                {
                    pixelslocal[x].transparent = colordata == 0;
                    if (!pixelslocal[x].transparent)
                    {
                        UInt16 colorall = BitConverter.ToUInt16(Memory.PaletteRAM, colordata * 2);
                        pixelslocal[x].update((Byte)((colorall & 0x1F) * 8), (byte)(((colorall >> 5) & 0x1F) * 8), (byte)(((colorall >> 10) & 0x1F) * 8));
                    }
                }
            }
        }

        private static void draw_bg_mode2(Pixel[] pixelslocal, UInt32 mapbase, UInt32 tilebase, bool wrapping, byte screensize, Int32 refX, Int32 refY, Int16 dx, Int16 dy, bool doubleres, bool is_bg2)
        {
            if (SSAA4x)
            {
                draw_bg_mode2_SSAA4x(pixelslocal, mapbase, tilebase, wrapping, screensize, refX, refY, dx, dy, is_bg2);
                return;
            }

            Int32 mapbaseaddr = (int)mapbase * 2048; // 2kb blocks
            Int32 tilebaseaddr = (int)tilebase * 0x4000; // 16kb blocks

            Int32 realX = refX;
            Int32 realY = refY;

            Int32 scroll_x_mod = 128;
            Int32 scroll_y_mod = 128;
            switch (screensize)
            {
                case 1: scroll_x_mod = 256; scroll_y_mod = 256; break;
                case 2: scroll_x_mod = 512; scroll_y_mod = 512; break;
                case 3: scroll_x_mod = 1024; scroll_y_mod = 1024; break;
            }

            int yshift = screensize + 4;

            int pixelcount = 240;
            if (doubleres)
            {
                pixelcount = 480;
            }

            for (int x = 0; x < pixelcount; x++)
            {
                bool draw = true;
                int xxx;
                int yyy;
                if (wrapping)
                {
                    xxx = (realX >> 8) & (scroll_x_mod - 1);
                    yyy = (realY >> 8) & (scroll_y_mod - 1);
                }
                else
                {
                    xxx = (realX >> 8);
                    yyy = (realY >> 8);

                    if (xxx < 0 || yyy < 0 || xxx >= scroll_x_mod || yyy >= scroll_y_mod)
                    {
                        pixelslocal[x].transparent = true;
                        draw = false;
                    }
                }

                if (draw)
                {
                    int tileindex = (xxx >> 3) + ((yyy >> 3) << yshift);
                    int addr = Math.Max(0, Math.Min(0x17FFF, mapbaseaddr + tileindex));
                    byte tileinfo = Memory.VRAM[addr];

                    int tileX = (xxx & 7);
                    int tileY = yyy & 7;
                    int pixeladdr = (tileinfo << 6) + (tileY * 8) + tileX;
                    byte colordata = Memory.VRAM[tilebaseaddr + pixeladdr];

                    UInt16 colorall = BitConverter.ToUInt16(Memory.PaletteRAM, colordata * 2);
                    pixelslocal[x].update((Byte)((colorall & 0x1F) * 8), (byte)(((colorall >> 5) & 0x1F) * 8), (byte)(((colorall >> 10) & 0x1F) * 8));
                    pixelslocal[x].transparent = colordata == 0;
                }
                if (doubleres)
                {
                    realX = refX + (x * dx) / 2;
                    realY = refY + (x * dy) / 2;
                }
                else
                {
                    realX += dx;
                    realY += dy;
                }
            }
        }

        private static void draw_bg_mode2_SSAA4x(Pixel[] pixelslocal, UInt32 mapbase, UInt32 tilebase, bool wrapping, byte screensize, Int32 refX, Int32 refY, Int16 dx, Int16 dy, bool is_bg2)
        {
            Int16 BG_DMX;
            Int16 BG_DMY;
            if (is_bg2)
            {
                BG_DMX = (Int16)GBRegs.Sect_display.BG2RotScaleParDMX.read();
                BG_DMY = (Int16)GBRegs.Sect_display.BG2RotScaleParDMY.read();
            }
            else
            {
                BG_DMX = (Int16)GBRegs.Sect_display.BG3RotScaleParDMX.read();
                BG_DMY = (Int16)GBRegs.Sect_display.BG3RotScaleParDMY.read();
            }

            Int32 mapbaseaddr = (int)mapbase * 2048; // 2kb blocks
            Int32 tilebaseaddr = (int)tilebase * 0x4000; // 16kb blocks

            Int32 realX = refX;
            Int32 realY = refY;

            Int32 scroll_x_mod = 128;
            Int32 scroll_y_mod = 128;
            switch (screensize)
            {
                case 1: scroll_x_mod = 256; scroll_y_mod = 256; break;
                case 2: scroll_x_mod = 512; scroll_y_mod = 512; break;
                case 3: scroll_x_mod = 1024; scroll_y_mod = 1024; break;
            }

            int yshift = screensize + 4;

            Pixel[] pixels = new Pixel[16];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Pixel();
            }

            for (int x = 0; x < 240; x++)
            {
                bool draw = true;
                int xxx;
                int yyy;
                if (wrapping)
                {
                    xxx = (realX >> 8) & (scroll_x_mod - 1);
                    yyy = (realY >> 8) & (scroll_y_mod - 1);
                }
                else
                {
                    xxx = (realX >> 8);
                    yyy = (realY >> 8);

                    if (xxx < 0 || yyy < 0 || xxx >= scroll_x_mod || yyy >= scroll_y_mod)
                    {
                        pixelslocal[x].transparent = true;
                        draw = false;
                    }
                }

                if (draw)
                {
                    for (int yi = 0; yi < 4; yi++)
                    {
                        Int32 refx_mod = refX;
                        Int32 refy_mod = refY;

                        refx_mod += (BG_DMX * yi) / 4;
                        refy_mod += (BG_DMY * yi) / 4;

                        for (int xi = 0; xi < 4; xi++)
                        {
                        
                            realX = refx_mod + (((x * 4 + xi) * dx / 4));
                            realY = refy_mod + (((x * 4 + xi) * dy / 4));

                            if (wrapping)
                            {
                                xxx = (realX >> 8) % scroll_x_mod;
                                yyy = (realY >> 8) % scroll_y_mod;
                            }
                            else
                            {
                                xxx = (realX >> 8);
                                yyy = (realY >> 8);
                            }

                            int tileindex = (xxx >> 3) + ((yyy >> 3) << yshift);
                            int addr = Math.Max(0, Math.Min(0x17FFF, mapbaseaddr + tileindex));
                            byte tileinfo = Memory.VRAM[addr];

                            int tileX = xxx & 7;
                            int tileY = yyy & 7;
                            int pixeladdr = (tileinfo << 6) + (tileY * 8) + tileX;
                            byte colordata = Memory.VRAM[tilebaseaddr + pixeladdr];

                            UInt16 colorall = BitConverter.ToUInt16(Memory.PaletteRAM, colordata * 2);
                            pixelslocal[x].transparent = colorall == 0;
                            pixels[yi * 4 + xi].update((Byte)((colorall & 0x1F) * 8), (byte)(((colorall >> 5) & 0x1F) * 8), (byte)(((colorall >> 10) & 0x1F) * 8));
                        }
                    }

                    int color_red = 0;
                    int color_green = 0;
                    int color_blue = 0;
                    for (int i = 0; i < pixels.Length; i++)
                    {
                        color_red += pixels[i].color_red;
                        color_green += pixels[i].color_green;
                        color_blue += pixels[i].color_blue;
                    }

                    pixelslocal[x].color_red = (byte)(color_red / pixels.Length);
                    pixelslocal[x].color_green = (byte)(color_green / pixels.Length);
                    pixelslocal[x].color_blue = (byte)(color_blue / pixels.Length);
                }

                realX += dx;
                realY += dy;
            }
        }

        private static void draw_bg_mode3(Int32 refX, Int32 refY, Int16 dx, Int16 dy)
        {
            int realX = refX;
            int realY = refY;
            int xxx = (realX >> 8);
            int yyy = (realY >> 8);

            for (Byte x = 0; x < 240; x++)
            {
                if (xxx >= 0 && yyy >= 0 && xxx < 240 && yyy < 160)
                {
                    int address = yyy * 480 + (xxx * 2);
                    UInt16 colorall = BitConverter.ToUInt16(Memory.VRAM, address);
                    pixels_bg2_1[x].update((Byte)((colorall & 0x1F) * 8), (byte)(((colorall >> 5) & 0x1F) * 8), (byte)(((colorall >> 10) & 0x1F) * 8));
                    pixels_bg2_1[x].transparent = false;
                }
                realX += dx;
                realY += dy;
                xxx = (realX >> 8);
                yyy = (realY >> 8);
            }
        }

        private static void draw_bg_mode4(Int32 refX, Int32 refY, Int16 dx, Int16 dy)
        {
            int realX = refX;
            int realY = refY;
            int xxx = (realX >> 8);
            int yyy = (realY >> 8);

            for (Byte x = 0; x < 240; x++)
            {
                if (xxx >= 0 && yyy >= 0 && xxx < 240 && yyy < 160)
                {
                    int address = yyy * 240 + xxx;
                    if (GBRegs.Sect_display.DISPCNT_Display_Frame_Select.on())
                    {
                        address += 0xA000;
                    }
                    byte colorptr = Memory.VRAM[address];
                    UInt16 colorall = BitConverter.ToUInt16(Memory.PaletteRAM, colorptr * 2);
                    pixels_bg2_1[x].update((Byte)((colorall & 0x1F) * 8), (byte)(((colorall >> 5) & 0x1F) * 8), (byte)(((colorall >> 10) & 0x1F) * 8));
                    pixels_bg2_1[x].transparent = colorptr == 0;
                }
                realX += dx;
                realY += dy;
                xxx = (realX >> 8);
                yyy = (realY >> 8);
            }
        }

        private static void draw_bg_mode5(Int32 refX, Int32 refY, Int16 dx, Int16 dy)
        {
            int realX = refX;
            int realY = refY;
            int xxx = (realX >> 8);
            int yyy = (realY >> 8);

            for (Byte x = 0; x < 240; x++)
            {
                if (xxx >= 0 && yyy >= 0 && xxx < 160 && yyy < 128)
                {
                    int address = yyy * 320 + (xxx * 2);
                    if (GBRegs.Sect_display.DISPCNT_Display_Frame_Select.on())
                    {
                        address += 0xA000;
                    }
                    UInt16 colorall = BitConverter.ToUInt16(Memory.VRAM, address);
                    pixels_bg2_1[x].update((Byte)((colorall & 0x1F) * 8), (byte)(((colorall >> 5) & 0x1F) * 8), (byte)(((colorall >> 10) & 0x1F) * 8));
                    pixels_bg2_1[x].transparent = false;
                }
                realX += dx;
                realY += dy;
                xxx = (realX >> 8);
                yyy = (realY >> 8);
            }
        }

        private static void draw_obj(int y, int baseaddr)
        {
            for (Byte x = 0; x < 240; x++)
            {
                pixels_obj[x].undrawn = true;
                pixels_obj[x].objwnd = false;
                pixels_obj[x].alpha = false;
            }

            bool one_dim_mapping = GBRegs.Sect_display.DISPCNT_OBJ_Char_VRAM_Map.on();

            byte mosaic_h = (byte)GBRegs.Sect_display.MOSAIC_OBJ_Mosaic_H_Size.read();
            byte mosaic_v = (byte)GBRegs.Sect_display.MOSAIC_OBJ_Mosaic_V_Size.read();

            int cycles = 0;
            int pixellimit = 1210;
            if (GBRegs.Sect_display.DISPCNT_H_Blank_IntervalFree.on())
            {
                pixellimit = 954;
            }

            for (int i = 0; i < 128; i++)
            {
                if (i == 0 && y == 50)
                {
                    int a = 5;
                }

                UInt16 Atr0 = BitConverter.ToUInt16(Memory.OAMRAM, i * 8);
                if (((Atr0 >> 8) & 1) == 1 || ((Atr0 >> 9) & 1) == 0) // rot/scale or disable
                {
                    UInt16 Atr1 = BitConverter.ToUInt16(Memory.OAMRAM, i * 8 + 2);
                    Int16 posY = (Int16)(Atr0 & 0xFF);
                    if (posY > 0xC0) { posY = (Int16)(posY - 0x100); }
                    bool affine = ((Atr0 >> 8) & 1) == 1;

                    UInt16 objshape = (UInt16)((Atr0 >> 14)  & 0x3);
                    UInt16 objsize = (UInt16)((Atr1 >> 14)  & 0x3);

                    byte sizeX = 0;
                    byte sizeY = 0;

                    switch (objshape)
                    {
                        case 0: // square
                            switch (objsize)
                            {
                                case 0: sizeX = 8; sizeY = 8; break;
                                case 1: sizeX = 16; sizeY = 16; break;
                                case 2: sizeX = 32; sizeY = 32; break;
                                case 3: sizeX = 64; sizeY = 64; break;
                            }
                            break;
                        case 1: // hor
                            switch (objsize)
                            {
                                case 0: sizeX = 16; sizeY = 8; break;
                                case 1: sizeX = 32; sizeY = 8; break;
                                case 2: sizeX = 32; sizeY = 16; break;
                                case 3: sizeX = 64; sizeY = 32; break;
                            }
                            break;
                        case 2: // vert
                            switch (objsize)
                            {
                                case 0: sizeX = 8; sizeY = 16; break;
                                case 1: sizeX = 8; sizeY = 32; break;
                                case 2: sizeX = 16; sizeY = 32; break;
                                case 3: sizeX = 32; sizeY = 64; break;
                            }
                            break;
                    }

                    int fieldX = sizeX;
                    int fieldY = sizeY;

                    if (((Atr0 >> 8) & 3) == 3)
                    {
                        fieldX *= 2;
                        fieldY *= 2;
                    }

                    int ty = y - posY;

                    bool mosaic_on = ((Atr0 >> 12) & 1) == 1;
                    if (mosaic_on)
                    {
                        ty -= (ty % (mosaic_v + 1));
                    }

                    if (ty >= 0 && ty < fieldY)
                    {
                        Int16 posX = (Int16)(Atr1 & 0x1FF);
                        if (posX > 0x100) { posX = (Int16)(posX - 0x200); }
                        UInt16 Atr2 = BitConverter.ToUInt16(Memory.OAMRAM, i * 8 + 4);

                        int tileindex = Atr2 & 0x3FF;

                        bool horflip = ((Atr1 >> 12) & 1) == 1;
                        bool verflip = ((Atr1 >> 13) & 1) == 1;
                        byte prio = (byte)((Atr2 >> 10) & 3);

                        bool hicolor = ((Atr0 >> 13) & 1) == 1;

                        if (hicolor && !one_dim_mapping)
                        {
                            tileindex = tileindex & 0x3FE;
                        }

                        int pixeladdr = baseaddr + tileindex * 32;
                        int tilemult;
                        int x_flip_offset;
                        int y_flip_offset;
                        int x_size;
                        int x_div;
                        if (!hicolor)
                        {
                            tilemult = 32;
                            x_flip_offset = 3;
                            y_flip_offset = 28;
                            x_div = 2;
                            x_size = 4;
                        }
                        else
                        {
                            tilemult = 64;
                            x_flip_offset = 7;
                            y_flip_offset = 56;
                            x_div = 1;
                            x_size = 8;
                        }

                        Int16 dx = 0;
                        Int16 dmx;
                        Int16 dy = 0;
                        Int16 dmy;
                        int realX = 0;
                        int realY = 0;
                        if (affine)
                        {
                            cycles += 10;
                            horflip = false;
                            verflip = false;
                            byte rotparpos = (byte)((Atr1 >> 9) & 0x1F);
                            dx = BitConverter.ToInt16(Memory.OAMRAM, rotparpos * 32 + 6);
                            dmx = BitConverter.ToInt16(Memory.OAMRAM, rotparpos * 32 + 14);
                            dy = BitConverter.ToInt16(Memory.OAMRAM, rotparpos * 32 + 22);
                            dmy = BitConverter.ToInt16(Memory.OAMRAM, rotparpos * 32 + 30);
                            realX = ((sizeX) << 7) - (fieldX >> 1) * dx - (fieldY >> 1) * dmx + ty * dmx;
                            realY = ((sizeY) << 7) - (fieldX >> 1) * dy - (fieldY >> 1) * dmy + ty * dmy;
                        }
                        else
                        {
                            if (verflip)
                            {
                                if (one_dim_mapping)
                                {
                                    pixeladdr += (y_flip_offset - (ty % 8) * x_size);
                                    pixeladdr += (((sizeY / 8) - 1) - (ty / 8)) * (sizeX * x_size);
                                }
                                else
                                {
                                    pixeladdr += (y_flip_offset - (ty % 8) * x_size);
                                    pixeladdr += (((sizeY / 8) - 1) - (ty / 8)) * 1024;
                                }
                            }
                            else
                            {
                                if (one_dim_mapping)
                                {
                                    pixeladdr += (ty % 8) * x_size;
                                    pixeladdr += (ty / 8) * (sizeX * x_size);
                                }
                                else
                                {
                                    pixeladdr += (ty % 8) * x_size;
                                    pixeladdr += (ty / 8) * 1024;
                                }
                            }
                        }

                        byte mosaik_h_cnt = 0;
                        for (UInt16 x = 0; x < fieldX; x++)
                        {
                            if ((affine && cycles + x * 2 >= pixellimit) || (!affine && cycles + x  >= pixellimit))
                            {
                                return;
                            }

                            int target = x + posX;
                            bool second_pix = false;
                            if (target > 239) { break; }
                            if (target >= 0)
                            {
                                if (mosaic_on && mosaik_h_cnt > 0 && (target - mosaik_h_cnt) >= 0)
                                {
                                    pixels_obj[target].copy(pixels_obj[target - mosaik_h_cnt]);
                                }
                                else
                                {
                                    int pixeladdr_X = -1;
                                    if (affine)
                                    {
                                        int xxx = realX >> 8;
                                        int yyy = realY >> 8;
                                        if (xxx >= 0 && xxx < sizeX && yyy >= 0 && yyy < sizeY)
                                        {
                                            second_pix = (xxx & 1) == 1;
                                            pixeladdr_X = pixeladdr;
                                            if (one_dim_mapping)
                                            {
                                                pixeladdr_X += (yyy % 8) * x_size;
                                                pixeladdr_X += (yyy / 8) * (sizeX * x_size);
                                            }
                                            else
                                            {
                                                pixeladdr_X += (yyy % 8) * x_size;
                                                pixeladdr_X += (yyy / 8) * 1024;
                                            }
                                            pixeladdr_X += (xxx % 8) / x_div;
                                            pixeladdr_X += (xxx / 8) * tilemult;
                                        }
                                    }
                                    else
                                    {
                                        pixeladdr_X = pixeladdr;
                                        if (horflip)
                                        {
                                            pixeladdr_X += x_flip_offset - ((x % 8) / x_div);
                                            pixeladdr_X -= ((x / 8) - ((sizeX / 8) - 1)) * tilemult;
                                        }
                                        else
                                        {
                                            pixeladdr_X += (x % 8) / x_div;
                                            pixeladdr_X += (x / 8) * tilemult;
                                        }
                                    }

                                    if (pixeladdr_X >= 0)
                                    {
                                        byte colordata;
                                        if (pixeladdr_X < 0x14000 && videomode >= 3)
                                        {
                                            colordata = 0;
                                        }
                                        else
                                        {
                                            pixeladdr_X = pixeladdr_X & 0x7FFF;
                                            pixeladdr_X += 0x10000;
                                            colordata = Memory.VRAM[pixeladdr_X];
                                        }

                                        if (!hicolor)
                                        {
                                            if (affine)
                                            {
                                                if (second_pix)
                                                {
                                                    colordata = (byte)(colordata >> 4);
                                                }
                                                else
                                                {
                                                    colordata = (byte)(colordata & 0xF);
                                                }
                                            }
                                            else
                                            {
                                                if (horflip && (x & 1) == 0 || !horflip && (x & 1) == 1)
                                                {
                                                    colordata = (byte)(colordata >> 4);
                                                }
                                                else
                                                {
                                                    colordata = (byte)(colordata & 0xF);
                                                }
                                            }
                                        }

                                        bool objwnd = ((Atr0 >> 10) & 3) == 2;
                                        bool transparent = colordata == 0;
                                        if (!transparent)
                                        {
                                            pixels_obj[target].objwnd |= objwnd;
                                        }
                                        if (!objwnd)
                                        {
                                            if (pixels_obj[target].undrawn || pixels_obj[target].transparent || prio < pixels_obj[target].prio)
                                            {
                                                UInt16 colorall;
                                                if (!hicolor)
                                                {
                                                    byte palette = (byte)(Atr2 >> 12);
                                                    colorall = BitConverter.ToUInt16(Memory.PaletteRAM, 512 + palette * 32 + colordata * 2);
                                                }
                                                else
                                                {
                                                    colorall = BitConverter.ToUInt16(Memory.PaletteRAM, 512 + colordata * 2);
                                                }

                                                if (!transparent)
                                                {
                                                    pixels_obj[target].update((Byte)((colorall & 0x1F) * 8), (byte)(((colorall >> 5) & 0x1F) * 8), (byte)(((colorall >> 10) & 0x1F) * 8));
                                                    pixels_obj[target].transparent = transparent;
                                                    pixels_obj[target].undrawn = false;
                                                }
                                                pixels_obj[target].prio = prio;
                                                pixels_obj[target].alpha = ((Atr0 >> 10) & 3) == 1;
                                            }
                                        }
                                    }
                                }
                            }
                            realX += dx;
                            realY += dy;
                            if (mosaik_h_cnt >= mosaic_h) { mosaik_h_cnt = 0; } else mosaik_h_cnt++;
                        }

                        if (affine)
                        {
                            cycles += fieldX * 2;
                        }
                        else
                        {
                            cycles += fieldX;
                        }

                    }
                }
            }

            
        }

        public static void draw_game(Bitmap temp_bmp)
        {
            lock (drawlock)
            {
                try
                {
                    BitmapData data = temp_bmp.LockBits(new Rectangle(0, 0, temp_bmp.Width, temp_bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    int stride = data.Stride;

                    unsafe
                    {
                        byte* ptr = (byte*)data.Scan0;

                        if (doubleres)
                        {
                            if (temp_bmp.Width == 480)
                            {
                                for (int y = 0; y < 320; y++)
                                {
                                    for (int x = 0; x < 480; x++)
                                    {
                                        ptr[(x * 3) + y * stride] = pixels[x, y].color_blue;
                                        ptr[(x * 3) + y * stride + 1] = pixels[x, y].color_green;
                                        ptr[(x * 3) + y * stride + 2] = pixels[x, y].color_red;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int y = 0; y < 160; y++)
                            {
                                for (int x = 0; x < 240; x++)
                                {
                                    ptr[(x * 3) + y * stride] = pixels[x, y].color_blue;
                                    ptr[(x * 3) + y * stride + 1] = pixels[x, y].color_green;
                                    ptr[(x * 3) + y * stride + 2] = pixels[x, y].color_red;
                                }
                            }
                        }

                        temp_bmp.UnlockBits(data);
                    }
                }
                catch //(Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }

                if (false && Memory.has_tilt)
                {
                    float angle = (Memory.tiltx - 0x3A0) / 3.0f;

                    // Make a Matrix to represent rotation
                    // by this angle.
                    System.Drawing.Drawing2D.Matrix rotate_at_origin = new System.Drawing.Drawing2D.Matrix();
                    rotate_at_origin.Rotate(angle);

                    // Rotate the image's corners to see how big
                    // it will be after rotation.
                    PointF[] points =
                    {
                        new PointF(0, 0),
                        new PointF(temp_bmp.Width, 0),
                        new PointF(temp_bmp.Width, temp_bmp.Height),
                        new PointF(0, temp_bmp.Height),
                    };
                    rotate_at_origin.TransformPoints(points);
                    float xmin, xmax, ymin, ymax;
                    GetPointBounds(points, out xmin, out xmax, out ymin, out ymax);

                    // Make a bitmap to hold the rotated result.
                    int wid = (int)Math.Round(xmax - xmin);
                    int hgt = (int)Math.Round(ymax - ymin);
                    Bitmap result = new Bitmap(wid, hgt);

                    
                    float scale = 160.0f / hgt;
                    int offsetx = (int)(240 - (wid * scale));

                    // Create the real rotation transformation.
                    System.Drawing.Drawing2D.Matrix rotate_at_center = new System.Drawing.Drawing2D.Matrix();
                    rotate_at_center.RotateAt(angle, new PointF(wid / 2f, hgt / 2f));

                    // Draw the image onto the new bitmap rotated.
                    using (Graphics gr = Graphics.FromImage(result))
                    {
                        // Use smooth image interpolation.
                        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;

                        // Clear with the color in the image's upper left corner.
                        gr.Clear(Color.Black);

                        //// For debugging. (It's easier to see the background.)
                        //gr.Clear(Color.LightBlue);

                        // Set up the transformation to rotate.
                        gr.Transform = rotate_at_center;

                        // Draw the image centered on the bitmap.
                        int x = (wid - temp_bmp.Width) / 2;
                        int y = (hgt - temp_bmp.Height) / 2;
                        gr.DrawImage(temp_bmp, x, y);
                    }

                    using (Graphics gr = Graphics.FromImage(temp_bmp))
                    {
                        gr.Clear(Color.Black);
                        gr.ScaleTransform(scale, scale);
                        gr.DrawImage(result, offsetx, 0);
                    }
                }
            }
        }

        private static void GetPointBounds(PointF[] points, out float xmin, out float xmax, out float ymin, out float ymax)
        {
            xmin = points[0].X;
            xmax = xmin;
            ymin = points[0].Y;
            ymax = ymin;
            foreach (PointF point in points)
            {
                if (xmin > point.X) xmin = point.X;
                if (xmax < point.X) xmax = point.X;
                if (ymin > point.Y) ymin = point.Y;
                if (ymax < point.Y) ymax = point.Y;
            }
        }


    }
}
