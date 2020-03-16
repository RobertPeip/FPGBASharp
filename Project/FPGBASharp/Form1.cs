using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace gbemu
{
    public partial class Form1 : Form
    {
        Thread gbthread;
        Thread drawerthread;

        Bitmap temp_bmp;
        Bitmap temp_bmp_2x;
        private DateTime blockdrawtime;
        object drawinglock;

        int fps = 60;

        public Form1()
        {
            InitializeComponent();

            temp_bmp = new Bitmap(240, 160);
            temp_bmp_2x = new Bitmap(480, 320);
            pictureBox1.Image = temp_bmp;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            blockdrawtime = DateTime.Now;
            drawinglock = new object();

#if DEBUG
            comboBox_fps.SelectedIndex = 3;
            comboBox_frameskip.SelectedIndex = 3;
#else
            comboBox_fps.SelectedIndex = 0;
            comboBox_frameskip.SelectedIndex = 0;
#endif
            // debug
            //gameboy.filename = "a.gba";

            openrom();

            //loadstate_fromdisk("savestate.sst");

            //loadcheat("cheat.gg");
        }

        private void resizeform(int x, int y)
        {
            lock (drawinglock)
            {
                blockdrawtime = DateTime.Now;
                pictureBox1.Width = x;
                pictureBox1.Height = y;
                this.Width = x + 50;
                this.Height = y + 80;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (gbthread != null) { gbthread.Abort(); }
            if (drawerthread != null) { drawerthread.Abort(); }
        }

        private void saveStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savegame();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.A: Joypad.KeyA = true; Joypad.KeyAToggle = false; break;
                case Keys.S: Joypad.KeyB = true; Joypad.KeyBToggle = false; break;
                case Keys.Q: Joypad.KeyL = true; break;
                case Keys.W: Joypad.KeyR = true; break;
                case Keys.X: Joypad.KeyStart = true; break;
                case Keys.Y: Joypad.KeySelect = true; break;
                case Keys.Up: Joypad.KeyUp = true; break;
                case Keys.Down: Joypad.KeyDown = true; break;
                case Keys.Left: Joypad.KeyLeft = true; break;
                case Keys.Right: Joypad.KeyRight = true; break;

                case Keys.R:
                    Joypad.KeyA = true;
                    Joypad.KeyB = true;
                    Joypad.KeyStart = true;
                    Joypad.KeySelect = true;
                    break;

                case Keys.D1: GPU.lockSpeed = true; GPU.speedmult = 1; break;
                case Keys.D2: GPU.lockSpeed = true; GPU.speedmult = 2; break;
                case Keys.D3: GPU.lockSpeed = true; GPU.speedmult = 3; break;
                case Keys.D4: GPU.lockSpeed = true; GPU.speedmult = 4; break;
                case Keys.D5: GPU.lockSpeed = true; GPU.speedmult = 5; break;
                case Keys.D6: GPU.lockSpeed = true; GPU.speedmult = 6; break;
                case Keys.D7: GPU.lockSpeed = true; GPU.speedmult = 7; break;
                case Keys.D8: GPU.lockSpeed = true; GPU.speedmult = 8; break;

                case Keys.D: Joypad.KeyAToggle = true; break;
                case Keys.F: Joypad.KeyBToggle = true; break;

                case Keys.Space:
                    fps = 10;
                    GPU.frameskip = 10;
                    GPU.lockSpeed = false;
                    break;

                case Keys.K: Memory.tiltx -= 20; break;
                case Keys.L: Memory.tiltx += 20; break;
                case Keys.I: Memory.tilty -= 20; break;
                case Keys.O: Memory.tilty += 20; break;

                case Keys.B:
                    if (gpio.gyro > gpio.GYRO_MIDDLE) gpio.gyro = gpio.GYRO_MIDDLE;
                    if (gpio.gyro > gpio.GYRO_MIN) gpio.gyro -= 20; 
                    break;
                case Keys.N:
                    if (gpio.gyro < gpio.GYRO_MIDDLE) gpio.gyro = gpio.GYRO_MIDDLE;
                    if (gpio.gyro < gpio.GYRO_MAX) gpio.gyro += 20;
                    break;
                case Keys.V:
                    gpio.gyro = gpio.GYRO_MIN;
                    break;
                case Keys.M:
                    gpio.gyro = gpio.GYRO_MAX;
                    break;
            }

            //if (gameboy.on)
            //{
            //    Joypad.set_reg();
            //}
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.A: Joypad.KeyA = false; break;
                case Keys.S: Joypad.KeyB = false; break;
                case Keys.Q: Joypad.KeyL = false; break;
                case Keys.W: Joypad.KeyR = false; break;
                case Keys.X: Joypad.KeyStart = false; break;
                case Keys.Y: Joypad.KeySelect = false; break;
                case Keys.Up: Joypad.KeyUp = false; break;
                case Keys.Down: Joypad.KeyDown = false; break;
                case Keys.Left: Joypad.KeyLeft = false; break;
                case Keys.Right: Joypad.KeyRight = false; break;

                case Keys.R:
                    Joypad.KeyA = false;
                    Joypad.KeyB = false;
                    Joypad.KeyStart = false;
                    Joypad.KeySelect = false;
                    break;

                case Keys.D: Joypad.KeyAToggle = false; break;
                case Keys.F: Joypad.KeyBToggle = false; break;

                case Keys.Space:
                    set_fps();
                    set_frameskip();
                    GPU.lockSpeed = true;
                    break;

                case Keys.F5: savestate(); break;
                case Keys.F9: loadstate(); break;

            }

            //if (gameboy.on)
            //{
            //    Joypad.set_reg();
            //}
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '0': GPU.lockSpeed = !GPU.lockSpeed; break;
            }
        }

        private void openRomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "(*.gba)|*.gba";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                gameboy.filename = fd.FileName;
                blockdrawtime = DateTime.Now;
                openrom();
            }
        }

        private void x1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizeform(240 * 1, 160 * 1);
        }

        private void x2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            resizeform(240 * 2, 160 * 2);

        }

        private void x3ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            resizeform(240 * 3, 160 * 3);
        }

        private void x4ToolStripMenuItem3_Click(object sender, EventArgs e)
        { 
            resizeform(240 * 4, 160 * 4);
        }

        private void xToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            resizeform(240 * 5, 160 * 5);
        }

        private void xToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            resizeform(240 * 6, 160 * 6);
        }

        private void xToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            resizeform(240 * 7, 160 * 7);
        }

        private void xToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            resizeform(240 * 8, 160 * 8);
        }

        private void openrom()
        {
            if (gameboy.filename != null)
            {
                if (gbthread != null)
                {
                    gbthread.Abort();
                }
                gameboy.coldreset = true;
                gbthread = new Thread(gameboy.run);
                gbthread.Start();

                if (drawerthread != null)
                {
                    drawerthread.Abort();
                }
                drawerthread = new Thread(drawer);
                drawerthread.Start();
            }
        }

        private void savegame()
        {
            Memory.createGameRAMSnapshot = true;
            while (Memory.createGameRAMSnapshot)
            { }
            Memory.save_gameram();
        }

        private void drawer()
        {
            UInt64 oldcycles = 0;
            UInt64 oldcommands = 0;
            UInt64 frames = 0;
            Stopwatch stopwatch_second = new Stopwatch();
            stopwatch_second.Start();

            Stopwatch stopwatch_frame= new Stopwatch();
            stopwatch_frame.Start();

            while (true)
            {
                long frametime = (1000000 / fps);
                long frametimeleft = frametime;

                //if (stopwatch_frame.ElapsedMilliseconds >= 16)
                long micros = 1000000 * stopwatch_frame.ElapsedTicks / Stopwatch.Frequency;
                if (micros >= frametimeleft)
                {
                    stopwatch_frame.Restart();
                    frametimeleft = Math.Max(5000, frametimeleft + frametime - micros);
                    try
                    {
                        if ((DateTime.Now - blockdrawtime).TotalMilliseconds > 2000)
                        {
                            //lock (drawinglock)
                            {
                                if (GPU.doubleres)
                                {
                                    GPU.draw_game(temp_bmp_2x);
                                }
                                else
                                {
                                    GPU.draw_game(temp_bmp);
                                }
                                this.Invoke((MethodInvoker)delegate
                                {
                                    pictureBox1.Refresh();
                                });
                            }
                        }

                        if (gpio.gyro < gpio.GYRO_MIDDLE) { gpio.gyro++; }
                        if (gpio.gyro > gpio.GYRO_MIDDLE) { gpio.gyro--; }
                    }
                    catch //(Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    frames++;
                }
                else //if (frametimeleft - micros > 3000)
                {
#if DEBUG
                    Thread.Sleep(1);
#endif
                }

                DateTime now = DateTime.Now;

                if (stopwatch_second.ElapsedMilliseconds > 1000)
                {
                    UInt64 cpucycles = (UInt64)gameboy.cycles;
                    double newcycles = cpucycles - oldcycles;
                    string text = "CPU%: " + (int)(100 * newcycles / 16780000);
                    text += " | FPS: " + frames;
                    text += " | Intern FPS: " + GPU.intern_frames + "(" + GPU.videomode_frames + ")";
                    text += " | AVG Cycles: " + (newcycles / (CPU.commands - oldcommands)).ToString("F");
                    text += " | Cyclemod: " + CPU.additional_steps;
                    if (Memory.has_tilt)
                    {
                        text += " | Tilt: " + Memory.tiltx.ToString("X4") + "|" + Memory.tilty.ToString("X4");
                    }
                    if (Memory.gpio_enable)
                    {
                        text += " | Solar: " + gpio.solar.ToString("X2");
                    }

                    try
                    {
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                this.Text = text;
                            });
                        }
                    }
                    catch { }

                    GPU.intern_frames = 0;
                    GPU.videomode_frames = 0;
                    oldcycles = cpucycles;
                    oldcommands = CPU.commands;
                    frames = 0;
                    stopwatch_second.Restart();
                }
            }
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            blockdrawtime = DateTime.Now;
        }

        public void set_fps()
        {
            switch (comboBox_fps.SelectedIndex)
            {
                case 0: fps = 60; break;
                case 1: fps = 30; break;
                case 2: fps = 25; break;
                case 3: fps = 20; break;
            }
        }

        public void set_frameskip()
        {
            switch (comboBox_frameskip.SelectedIndex)
            {
                case 0: GPU.frameskip = 0; break;
                case 1: GPU.frameskip = 1; break;
                case 2: GPU.frameskip = 2; break;
                case 3: GPU.frameskip = 3; break;
                case 4: GPU.frameskip = 4; break;
                case 5: GPU.frameskip = 5; break;
                case 6: GPU.frameskip = 6; break;
            }
        }

        private void comboBox_fps_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_fps();
            this.ActiveControl = null;
        }

        private void comboBox_frameskip_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_frameskip();
            this.ActiveControl = null;
        }

        private void paletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox_fps.Enabled = !comboBox_fps.Enabled;
            comboBox_frameskip.Enabled = !comboBox_frameskip.Enabled;
            trackBar_additionalsteps.Enabled = !trackBar_additionalsteps.Enabled;
            trackBar_solar.Enabled = !trackBar_solar.Enabled;
        }

        private void trackBar_additionalsteps_ValueChanged(object sender, EventArgs e)
        {
            CPU.additional_steps = trackBar_additionalsteps.Value;
        }

        private void trackBar_solar_ValueChanged(object sender, EventArgs e)
        {
            gpio.solar = (byte)(0xFA - trackBar_solar.Value);
            Memory.tiltx = (ushort)(0x200 + trackBar_solar.Value * 4);
            Memory.tilty = (ushort)(0x200 + trackBar_solar.Value * 4);
        }

        private void profilingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Memory.writecnt8 != null)
            {
                string allinfo = "";
                allinfo += ("w8").PadLeft(11) + ";";
                allinfo += ("w16").PadLeft(11) + ";";
                allinfo += ("w32").PadLeft(11) + ";";
                allinfo += ("r8").PadLeft(11) + ";";
                allinfo += ("r16").PadLeft(11) + ";";
                allinfo += ("r32").PadLeft(11) + ";\n";

                for (int i = 0; i < 16; i++)
                {
                    allinfo += Memory.writecnt8[i].ToString().PadLeft(11) + ";";
                    allinfo += Memory.writecnt16[i].ToString().PadLeft(11) + ";";
                    allinfo += Memory.writecnt32[i].ToString().PadLeft(11) + ";";
                    allinfo += Memory.readcnt8[i].ToString().PadLeft(11) + ";";
                    allinfo += Memory.readcnt16[i].ToString().PadLeft(11) + ";";
                    allinfo += Memory.readcnt32[i].ToString().PadLeft(11) + ";\n";

                    Memory.writecnt8[i] = 0;
                    Memory.writecnt16[i] = 0;
                    Memory.writecnt32[i] = 0;
                    Memory.readcnt8[i] = 0;
                    Memory.readcnt16[i] = 0;
                    Memory.readcnt32[i] = 0;
                }

                Clipboard.SetText(allinfo);
                MessageBox.Show("Copied to clipboard -> only in debug build!");
            }
        }

        private void timer_save_Tick(object sender, EventArgs e)
        {
            //if (gameboy.on)
            //{
            //    Joypad.set_reg();
            //}
        }

        private void interlaceBlendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPU.interlace_blending = !GPU.interlace_blending;
        }

        private void pixelshaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPU.colorshader = !GPU.colorshader;
        }

        private void xResolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPU.doubleres = !GPU.doubleres;
            if (GPU.doubleres)
            {
                pictureBox1.Image = temp_bmp_2x;
            }
            else
            {
                pictureBox1.Image = temp_bmp;
            }
        }

        private void sSAA4xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPU.SSAA4x = !GPU.SSAA4x;
        }

        private void saveStateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            savestate();
        }

        private void loadStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadstate();
        }

        private void stateToDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                savestate_todisk(fd.FileName);
            }
        }

        private void stateFromDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                loadstate_fromdisk(fd.FileName);
            }
        }

        private void savestate()
        {
            gameboy.do_savestate = true;

            while (gameboy.do_savestate)
            {
                Thread.Sleep(1);
            }
        }

        private void loadstate()
        {
            gameboy.do_loadstate = true;

            while (gameboy.do_loadstate)
            {
                Thread.Sleep(1);
            }
        }

        private void savestate_todisk(string filename)
        {
            FileStream fileStream = new FileStream(filename, FileMode.Create);
            byte[] byteArray = gameboy.savestate.SelectMany(BitConverter.GetBytes).ToArray();
            fileStream.Write(byteArray, 0, byteArray.Length);
            fileStream.Close();
        }

        private void loadstate_fromdisk(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            FileStream fileStream = fileInfo.OpenRead();
            byte[] bytedata = new byte[fileInfo.Length];
            fileStream.Read(bytedata, 0, bytedata.Length);
            fileStream.Close();
            Buffer.BlockCopy(bytedata, 0, gameboy.savestate, 0, bytedata.Length);
            loadstate();
        }

        private void loadCheatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                loadcheat(fd.FileName);
            }
        }

        private void loadcheat(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            FileStream fileStream = fileInfo.OpenRead();
            byte[] bytedata = new byte[fileInfo.Length];
            fileStream.Read(bytedata, 0, bytedata.Length);
            fileStream.Close();
            Cheats.add_cheats(bytedata);
        }


    }
}
