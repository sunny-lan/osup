namespace OsuAnalyzer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.refresh = new System.Windows.Forms.Button();
            this.mapSelect = new System.Windows.Forms.ListBox();
            this.replaySelect = new System.Windows.Forms.ListBox();
            this.replayInfo = new System.Windows.Forms.TextBox();
            this.mapInfo = new System.Windows.Forms.TextBox();
            this.loadReplay = new System.Windows.Forms.Button();
            this.scrubber1 = new OsuAnalyzer.Panels.Scrubber();
            this.coolPanel = new OsuAnalyzer.MainPanel();
            this.SuspendLayout();
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(1431, 814);
            this.refresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(267, 36);
            this.refresh.TabIndex = 1;
            this.refresh.Text = "Refresh replays";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // mapSelect
            // 
            this.mapSelect.FormattingEnabled = true;
            this.mapSelect.ItemHeight = 16;
            this.mapSelect.Location = new System.Drawing.Point(1431, 54);
            this.mapSelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mapSelect.Name = "mapSelect";
            this.mapSelect.Size = new System.Drawing.Size(265, 180);
            this.mapSelect.TabIndex = 2;
            this.mapSelect.SelectedIndexChanged += new System.EventHandler(this.mapSelect_SelectedIndexChanged);
            // 
            // replaySelect
            // 
            this.replaySelect.FormattingEnabled = true;
            this.replaySelect.ItemHeight = 16;
            this.replaySelect.Location = new System.Drawing.Point(1431, 430);
            this.replaySelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.replaySelect.Name = "replaySelect";
            this.replaySelect.Size = new System.Drawing.Size(265, 180);
            this.replaySelect.TabIndex = 3;
            this.replaySelect.SelectedIndexChanged += new System.EventHandler(this.replaySelect_SelectedIndexChanged);
            // 
            // replayInfo
            // 
            this.replayInfo.Location = new System.Drawing.Point(1431, 615);
            this.replayInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.replayInfo.Multiline = true;
            this.replayInfo.Name = "replayInfo";
            this.replayInfo.ReadOnly = true;
            this.replayInfo.Size = new System.Drawing.Size(265, 192);
            this.replayInfo.TabIndex = 5;
            this.replayInfo.Text = "-";
            // 
            // mapInfo
            // 
            this.mapInfo.Location = new System.Drawing.Point(1431, 240);
            this.mapInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mapInfo.Multiline = true;
            this.mapInfo.Name = "mapInfo";
            this.mapInfo.ReadOnly = true;
            this.mapInfo.Size = new System.Drawing.Size(265, 184);
            this.mapInfo.TabIndex = 4;
            this.mapInfo.Text = "-";
            // 
            // loadReplay
            // 
            this.loadReplay.Location = new System.Drawing.Point(1431, 14);
            this.loadReplay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadReplay.Name = "loadReplay";
            this.loadReplay.Size = new System.Drawing.Size(267, 36);
            this.loadReplay.TabIndex = 6;
            this.loadReplay.Text = "Load replay";
            this.loadReplay.UseVisualStyleBackColor = true;
            this.loadReplay.Click += new System.EventHandler(this.loadReplay_Click);
            // 
            // scrubber1
            // 
            this.scrubber1.Location = new System.Drawing.Point(12, 816);
            this.scrubber1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.scrubber1.Name = "scrubber1";
            this.scrubber1.Playing = false;
            this.scrubber1.Size = new System.Drawing.Size(1413, 33);
            this.scrubber1.TabIndex = 7;
            // 
            // coolPanel
            // 
            this.coolPanel.Location = new System.Drawing.Point(12, 14);
            this.coolPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.coolPanel.Name = "coolPanel";
            this.coolPanel.Size = new System.Drawing.Size(1151, 798);
            this.coolPanel.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1604, 863);
            this.Controls.Add(this.scrubber1);
            this.Controls.Add(this.loadReplay);
            this.Controls.Add(this.replayInfo);
            this.Controls.Add(this.mapInfo);
            this.Controls.Add(this.replaySelect);
            this.Controls.Add(this.mapSelect);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.coolPanel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Osup!";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MainPanel coolPanel;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.ListBox mapSelect;
        private System.Windows.Forms.ListBox replaySelect;
        private System.Windows.Forms.TextBox replayInfo;
        private System.Windows.Forms.TextBox mapInfo;
        private System.Windows.Forms.Button loadReplay;
        private Panels.Scrubber scrubber1;
    }
}

