namespace OsuAnalyzer
{
    partial class Form1
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
            this.coolPanel = new OsuAnalyzer.CoolPanel();
            this.SuspendLayout();
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(932, 12);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(156, 36);
            this.refresh.TabIndex = 1;
            this.refresh.Text = "Refresh replays";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // mapSelect
            // 
            this.mapSelect.FormattingEnabled = true;
            this.mapSelect.ItemHeight = 16;
            this.mapSelect.Location = new System.Drawing.Point(932, 54);
            this.mapSelect.Name = "mapSelect";
            this.mapSelect.Size = new System.Drawing.Size(156, 180);
            this.mapSelect.TabIndex = 2;
            this.mapSelect.SelectedIndexChanged += new System.EventHandler(this.mapSelect_SelectedIndexChanged);
            // 
            // replaySelect
            // 
            this.replaySelect.FormattingEnabled = true;
            this.replaySelect.ItemHeight = 16;
            this.replaySelect.Location = new System.Drawing.Point(1094, 54);
            this.replaySelect.Name = "replaySelect";
            this.replaySelect.Size = new System.Drawing.Size(156, 180);
            this.replaySelect.TabIndex = 3;
            this.replaySelect.SelectedIndexChanged += new System.EventHandler(this.replaySelect_SelectedIndexChanged);
            // 
            // replayInfo
            // 
            this.replayInfo.Location = new System.Drawing.Point(932, 367);
            this.replayInfo.Multiline = true;
            this.replayInfo.Name = "replayInfo";
            this.replayInfo.ReadOnly = true;
            this.replayInfo.Size = new System.Drawing.Size(318, 125);
            this.replayInfo.TabIndex = 5;
            this.replayInfo.Text = "-";
            // 
            // mapInfo
            // 
            this.mapInfo.Location = new System.Drawing.Point(932, 249);
            this.mapInfo.Multiline = true;
            this.mapInfo.Name = "mapInfo";
            this.mapInfo.ReadOnly = true;
            this.mapInfo.Size = new System.Drawing.Size(318, 112);
            this.mapInfo.TabIndex = 4;
            this.mapInfo.Text = "-";
            // 
            // loadReplay
            // 
            this.loadReplay.Location = new System.Drawing.Point(1094, 12);
            this.loadReplay.Name = "loadReplay";
            this.loadReplay.Size = new System.Drawing.Size(156, 36);
            this.loadReplay.TabIndex = 6;
            this.loadReplay.Text = "Load replay";
            this.loadReplay.UseVisualStyleBackColor = true;
            this.loadReplay.Click += new System.EventHandler(this.loadReplay_Click);
            // 
            // scrubber1
            // 
            this.scrubber1.Location = new System.Drawing.Point(12, 697);
            this.scrubber1.Name = "scrubber1";
            this.scrubber1.Size = new System.Drawing.Size(1238, 33);
            this.scrubber1.TabIndex = 7;
            // 
            // coolPanel
            // 
            this.coolPanel.Location = new System.Drawing.Point(12, 12);
            this.coolPanel.Name = "coolPanel";
            this.coolPanel.Size = new System.Drawing.Size(914, 679);
            this.coolPanel.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 742);
            this.Controls.Add(this.scrubber1);
            this.Controls.Add(this.loadReplay);
            this.Controls.Add(this.replayInfo);
            this.Controls.Add(this.mapInfo);
            this.Controls.Add(this.replaySelect);
            this.Controls.Add(this.mapSelect);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.coolPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CoolPanel coolPanel;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.ListBox mapSelect;
        private System.Windows.Forms.ListBox replaySelect;
        private System.Windows.Forms.TextBox replayInfo;
        private System.Windows.Forms.TextBox mapInfo;
        private System.Windows.Forms.Button loadReplay;
        private Panels.Scrubber scrubber1;
    }
}

