namespace MCEBuddy.GUI
{
    partial class MonitorTaskExpertSettingsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorTaskExpertSettingsForm));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.oKcmd = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.monitorConvertedChk = new System.Windows.Forms.CheckBox();
            this.reMonitorRecordedChk = new System.Windows.Forms.CheckBox();
            this.grpBoxFiles = new System.Windows.Forms.GroupBox();
            this.grpBoxFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.cmdCancel.Location = new System.Drawing.Point(307, 93);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 1100;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // oKcmd
            // 
            this.oKcmd.Location = new System.Drawing.Point(19, 93);
            this.oKcmd.Name = "oKcmd";
            this.oKcmd.Size = new System.Drawing.Size(75, 23);
            this.oKcmd.TabIndex = 1000;
            this.oKcmd.Text = "OK";
            this.oKcmd.UseVisualStyleBackColor = true;
            this.oKcmd.Click += new System.EventHandler(this.oKcmd_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 9000;
            this.toolTip.InitialDelay = 40;
            this.toolTip.ReshowDelay = 40;
            // 
            // monitorConvertedChk
            // 
            this.monitorConvertedChk.AutoSize = true;
            this.monitorConvertedChk.ForeColor = System.Drawing.Color.Firebrick;
            this.monitorConvertedChk.Location = new System.Drawing.Point(15, 28);
            this.monitorConvertedChk.Name = "monitorConvertedChk";
            this.monitorConvertedChk.Size = new System.Drawing.Size(146, 17);
            this.monitorConvertedChk.TabIndex = 110;
            this.monitorConvertedChk.Text = "Monitor converted videos";
            this.toolTip.SetToolTip(this.monitorConvertedChk, resources.GetString("monitorConvertedChk.ToolTip"));
            this.monitorConvertedChk.UseVisualStyleBackColor = true;
            this.monitorConvertedChk.CheckedChanged += new System.EventHandler(this.monitorConvertedChk_CheckedChanged);
            // 
            // reMonitorRecordedChk
            // 
            this.reMonitorRecordedChk.AutoSize = true;
            this.reMonitorRecordedChk.ForeColor = System.Drawing.Color.Firebrick;
            this.reMonitorRecordedChk.Location = new System.Drawing.Point(192, 28);
            this.reMonitorRecordedChk.Name = "reMonitorRecordedChk";
            this.reMonitorRecordedChk.Size = new System.Drawing.Size(156, 17);
            this.reMonitorRecordedChk.TabIndex = 110;
            this.reMonitorRecordedChk.Text = "Re-monitor recorded videos";
            this.toolTip.SetToolTip(this.reMonitorRecordedChk, resources.GetString("reMonitorRecordedChk.ToolTip"));
            this.reMonitorRecordedChk.UseVisualStyleBackColor = true;
            this.reMonitorRecordedChk.CheckedChanged += new System.EventHandler(this.reMonitorChk_CheckedChanged);
            // 
            // grpBoxFiles
            // 
            this.grpBoxFiles.Controls.Add(this.reMonitorRecordedChk);
            this.grpBoxFiles.Controls.Add(this.monitorConvertedChk);
            this.grpBoxFiles.Location = new System.Drawing.Point(19, 13);
            this.grpBoxFiles.Name = "grpBoxFiles";
            this.grpBoxFiles.Size = new System.Drawing.Size(363, 61);
            this.grpBoxFiles.TabIndex = 100;
            this.grpBoxFiles.TabStop = false;
            this.grpBoxFiles.Text = "Files";
            // 
            // MonitorTaskExpertSettingsForm
            // 
            this.AcceptButton = this.oKcmd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(410, 145);
            this.Controls.Add(this.grpBoxFiles);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.oKcmd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MonitorTaskExpertSettingsForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 25, 20);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Expert Settings";
            this.Load += new System.EventHandler(this.SourceForm_Load);
            this.grpBoxFiles.ResumeLayout(false);
            this.grpBoxFiles.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button oKcmd;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox monitorConvertedChk;
        private System.Windows.Forms.GroupBox grpBoxFiles;
        private System.Windows.Forms.CheckBox reMonitorRecordedChk;
    }
}