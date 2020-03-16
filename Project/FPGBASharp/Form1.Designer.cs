namespace gbemu
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveStateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stateToDiskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stateFromDiskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCheatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interlaceBlendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pixelshaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xResolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sSAA4xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.profilingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_save = new System.Windows.Forms.Timer(this.components);
            this.comboBox_fps = new System.Windows.Forms.ComboBox();
            this.comboBox_frameskip = new System.Windows.Forms.ComboBox();
            this.trackBar_additionalsteps = new System.Windows.Forms.TrackBar();
            this.trackBar_solar = new System.Windows.Forms.TrackBar();
            this.pictureBox1 = new gbemu.PictureBoxWithInterpolationMode();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_additionalsteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_solar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(986, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRomToolStripMenuItem,
            this.saveStateToolStripMenuItem,
            this.saveStateToolStripMenuItem1,
            this.loadStateToolStripMenuItem,
            this.stateToDiskToolStripMenuItem,
            this.stateFromDiskToolStripMenuItem,
            this.loadCheatToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openRomToolStripMenuItem
            // 
            this.openRomToolStripMenuItem.Name = "openRomToolStripMenuItem";
            this.openRomToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.openRomToolStripMenuItem.Text = "Open Rom";
            this.openRomToolStripMenuItem.Click += new System.EventHandler(this.openRomToolStripMenuItem_Click);
            // 
            // saveStateToolStripMenuItem
            // 
            this.saveStateToolStripMenuItem.Name = "saveStateToolStripMenuItem";
            this.saveStateToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.saveStateToolStripMenuItem.Text = "SaveGame";
            this.saveStateToolStripMenuItem.Click += new System.EventHandler(this.saveStateToolStripMenuItem_Click);
            // 
            // saveStateToolStripMenuItem1
            // 
            this.saveStateToolStripMenuItem1.Name = "saveStateToolStripMenuItem1";
            this.saveStateToolStripMenuItem1.Size = new System.Drawing.Size(153, 22);
            this.saveStateToolStripMenuItem1.Text = "SaveState";
            this.saveStateToolStripMenuItem1.Click += new System.EventHandler(this.saveStateToolStripMenuItem1_Click);
            // 
            // loadStateToolStripMenuItem
            // 
            this.loadStateToolStripMenuItem.Name = "loadStateToolStripMenuItem";
            this.loadStateToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.loadStateToolStripMenuItem.Text = "LoadState";
            this.loadStateToolStripMenuItem.Click += new System.EventHandler(this.loadStateToolStripMenuItem_Click);
            // 
            // stateToDiskToolStripMenuItem
            // 
            this.stateToDiskToolStripMenuItem.Name = "stateToDiskToolStripMenuItem";
            this.stateToDiskToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.stateToDiskToolStripMenuItem.Text = "State to disk";
            this.stateToDiskToolStripMenuItem.Click += new System.EventHandler(this.stateToDiskToolStripMenuItem_Click);
            // 
            // stateFromDiskToolStripMenuItem
            // 
            this.stateFromDiskToolStripMenuItem.Name = "stateFromDiskToolStripMenuItem";
            this.stateFromDiskToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.stateFromDiskToolStripMenuItem.Text = "State from disk";
            this.stateFromDiskToolStripMenuItem.Click += new System.EventHandler(this.stateFromDiskToolStripMenuItem_Click);
            // 
            // loadCheatToolStripMenuItem
            // 
            this.loadCheatToolStripMenuItem.Name = "loadCheatToolStripMenuItem";
            this.loadCheatToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.loadCheatToolStripMenuItem.Text = "Load Cheat";
            this.loadCheatToolStripMenuItem.Click += new System.EventHandler(this.loadCheatToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.paletteToolStripMenuItem,
            this.interlaceBlendingToolStripMenuItem,
            this.pixelshaderToolStripMenuItem,
            this.xResolutionToolStripMenuItem,
            this.sSAA4xToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // paletteToolStripMenuItem
            // 
            this.paletteToolStripMenuItem.Name = "paletteToolStripMenuItem";
            this.paletteToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.paletteToolStripMenuItem.Text = "Unlock FPS/Frameskip";
            this.paletteToolStripMenuItem.Click += new System.EventHandler(this.paletteToolStripMenuItem_Click);
            // 
            // interlaceBlendingToolStripMenuItem
            // 
            this.interlaceBlendingToolStripMenuItem.Name = "interlaceBlendingToolStripMenuItem";
            this.interlaceBlendingToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.interlaceBlendingToolStripMenuItem.Text = "InterlaceBlending";
            this.interlaceBlendingToolStripMenuItem.Click += new System.EventHandler(this.interlaceBlendingToolStripMenuItem_Click);
            // 
            // pixelshaderToolStripMenuItem
            // 
            this.pixelshaderToolStripMenuItem.Name = "pixelshaderToolStripMenuItem";
            this.pixelshaderToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.pixelshaderToolStripMenuItem.Text = "Pixelshader";
            this.pixelshaderToolStripMenuItem.Click += new System.EventHandler(this.pixelshaderToolStripMenuItem_Click);
            // 
            // xResolutionToolStripMenuItem
            // 
            this.xResolutionToolStripMenuItem.Name = "xResolutionToolStripMenuItem";
            this.xResolutionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.xResolutionToolStripMenuItem.Text = "2xResolution";
            this.xResolutionToolStripMenuItem.Click += new System.EventHandler(this.xResolutionToolStripMenuItem_Click);
            // 
            // sSAA4xToolStripMenuItem
            // 
            this.sSAA4xToolStripMenuItem.Name = "sSAA4xToolStripMenuItem";
            this.sSAA4xToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.sSAA4xToolStripMenuItem.Text = "SSAA4x";
            this.sSAA4xToolStripMenuItem.Click += new System.EventHandler(this.sSAA4xToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sizeToolStripMenuItem,
            this.profilingToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // sizeToolStripMenuItem
            // 
            this.sizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xToolStripMenuItem,
            this.xToolStripMenuItem1,
            this.xToolStripMenuItem2,
            this.xToolStripMenuItem3,
            this.xToolStripMenuItem4,
            this.xToolStripMenuItem5,
            this.xToolStripMenuItem6,
            this.xToolStripMenuItem7});
            this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            this.sizeToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.sizeToolStripMenuItem.Text = "Size";
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem.Text = "1x";
            this.xToolStripMenuItem.Click += new System.EventHandler(this.x1ToolStripMenuItem_Click);
            // 
            // xToolStripMenuItem1
            // 
            this.xToolStripMenuItem1.Name = "xToolStripMenuItem1";
            this.xToolStripMenuItem1.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem1.Text = "2x";
            this.xToolStripMenuItem1.Click += new System.EventHandler(this.x2ToolStripMenuItem1_Click);
            // 
            // xToolStripMenuItem2
            // 
            this.xToolStripMenuItem2.Name = "xToolStripMenuItem2";
            this.xToolStripMenuItem2.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem2.Text = "3x";
            this.xToolStripMenuItem2.Click += new System.EventHandler(this.x3ToolStripMenuItem2_Click);
            // 
            // xToolStripMenuItem3
            // 
            this.xToolStripMenuItem3.Name = "xToolStripMenuItem3";
            this.xToolStripMenuItem3.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem3.Text = "4x";
            this.xToolStripMenuItem3.Click += new System.EventHandler(this.x4ToolStripMenuItem3_Click);
            // 
            // xToolStripMenuItem4
            // 
            this.xToolStripMenuItem4.Name = "xToolStripMenuItem4";
            this.xToolStripMenuItem4.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem4.Text = "5x";
            this.xToolStripMenuItem4.Click += new System.EventHandler(this.xToolStripMenuItem4_Click);
            // 
            // xToolStripMenuItem5
            // 
            this.xToolStripMenuItem5.Name = "xToolStripMenuItem5";
            this.xToolStripMenuItem5.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem5.Text = "6x";
            this.xToolStripMenuItem5.Click += new System.EventHandler(this.xToolStripMenuItem5_Click);
            // 
            // xToolStripMenuItem6
            // 
            this.xToolStripMenuItem6.Name = "xToolStripMenuItem6";
            this.xToolStripMenuItem6.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem6.Text = "7x";
            this.xToolStripMenuItem6.Click += new System.EventHandler(this.xToolStripMenuItem6_Click);
            // 
            // xToolStripMenuItem7
            // 
            this.xToolStripMenuItem7.Name = "xToolStripMenuItem7";
            this.xToolStripMenuItem7.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem7.Text = "8x";
            this.xToolStripMenuItem7.Click += new System.EventHandler(this.xToolStripMenuItem7_Click);
            // 
            // profilingToolStripMenuItem
            // 
            this.profilingToolStripMenuItem.Name = "profilingToolStripMenuItem";
            this.profilingToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.profilingToolStripMenuItem.Text = "Profiling";
            this.profilingToolStripMenuItem.Click += new System.EventHandler(this.profilingToolStripMenuItem_Click);
            // 
            // timer_save
            // 
            this.timer_save.Enabled = true;
            this.timer_save.Interval = 1;
            this.timer_save.Tick += new System.EventHandler(this.timer_save_Tick);
            // 
            // comboBox_fps
            // 
            this.comboBox_fps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_fps.Enabled = false;
            this.comboBox_fps.FormattingEnabled = true;
            this.comboBox_fps.Items.AddRange(new object[] {
            "60 FPS",
            "30 FPS",
            "25 FPS",
            "20 FPS"});
            this.comboBox_fps.Location = new System.Drawing.Point(156, 3);
            this.comboBox_fps.Name = "comboBox_fps";
            this.comboBox_fps.Size = new System.Drawing.Size(86, 21);
            this.comboBox_fps.TabIndex = 4;
            this.comboBox_fps.TabStop = false;
            this.comboBox_fps.SelectedIndexChanged += new System.EventHandler(this.comboBox_fps_SelectedIndexChanged);
            // 
            // comboBox_frameskip
            // 
            this.comboBox_frameskip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_frameskip.Enabled = false;
            this.comboBox_frameskip.FormattingEnabled = true;
            this.comboBox_frameskip.Items.AddRange(new object[] {
            "Frameskip 0",
            "Frameskip 1",
            "Frameskip 2",
            "Frameskip 3",
            "Frameskip 4",
            "Frameskip 5",
            "Frameskip 6"});
            this.comboBox_frameskip.Location = new System.Drawing.Point(248, 3);
            this.comboBox_frameskip.Name = "comboBox_frameskip";
            this.comboBox_frameskip.Size = new System.Drawing.Size(86, 21);
            this.comboBox_frameskip.TabIndex = 5;
            this.comboBox_frameskip.TabStop = false;
            this.comboBox_frameskip.SelectedIndexChanged += new System.EventHandler(this.comboBox_frameskip_SelectedIndexChanged);
            // 
            // trackBar_additionalsteps
            // 
            this.trackBar_additionalsteps.AutoSize = false;
            this.trackBar_additionalsteps.Enabled = false;
            this.trackBar_additionalsteps.LargeChange = 1;
            this.trackBar_additionalsteps.Location = new System.Drawing.Point(340, 3);
            this.trackBar_additionalsteps.Minimum = -10;
            this.trackBar_additionalsteps.Name = "trackBar_additionalsteps";
            this.trackBar_additionalsteps.Size = new System.Drawing.Size(150, 20);
            this.trackBar_additionalsteps.TabIndex = 6;
            this.trackBar_additionalsteps.TabStop = false;
            this.trackBar_additionalsteps.ValueChanged += new System.EventHandler(this.trackBar_additionalsteps_ValueChanged);
            // 
            // trackBar_solar
            // 
            this.trackBar_solar.AutoSize = false;
            this.trackBar_solar.Enabled = false;
            this.trackBar_solar.LargeChange = 1;
            this.trackBar_solar.Location = new System.Drawing.Point(496, 3);
            this.trackBar_solar.Maximum = 200;
            this.trackBar_solar.Name = "trackBar_solar";
            this.trackBar_solar.Size = new System.Drawing.Size(150, 20);
            this.trackBar_solar.TabIndex = 7;
            this.trackBar_solar.TabStop = false;
            this.trackBar_solar.ValueChanged += new System.EventHandler(this.trackBar_solar_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(960, 640);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 691);
            this.Controls.Add(this.trackBar_solar);
            this.Controls.Add(this.trackBar_additionalsteps);
            this.Controls.Add(this.comboBox_frameskip);
            this.Controls.Add(this.comboBox_fps);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Move += new System.EventHandler(this.Form1_Move);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_additionalsteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_solar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem paletteToolStripMenuItem;
        private System.Windows.Forms.Timer timer_save;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRomToolStripMenuItem;
        private PictureBoxWithInterpolationMode pictureBox1;
        private System.Windows.Forms.ComboBox comboBox_fps;
        private System.Windows.Forms.ComboBox comboBox_frameskip;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem7;
        private System.Windows.Forms.TrackBar trackBar_additionalsteps;
        private System.Windows.Forms.ToolStripMenuItem profilingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interlaceBlendingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveStateToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stateToDiskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stateFromDiskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadCheatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pixelshaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xResolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sSAA4xToolStripMenuItem;
        private System.Windows.Forms.TrackBar trackBar_solar;
    }
}

