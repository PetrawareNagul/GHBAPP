namespace Silverlake.Window
{
    partial class FolderWatcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderWatcher));
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.backgroundWorkerTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // metroLabel3
            // 
            this.metroLabel3.CausesValidation = false;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel3.Location = new System.Drawing.Point(129, 60);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(130, 29);
            this.metroLabel3.TabIndex = 13;
            this.metroLabel3.Text = "Folder watcher";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Sync control";
            this.notifyIcon.BalloonTipTitle = "For branch";
            this.notifyIcon.Text = "Sync control for branch";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // backgroundWorkerTimer
            // 
            this.backgroundWorkerTimer.Enabled = true;
            this.backgroundWorkerTimer.Interval = 500;
            this.backgroundWorkerTimer.Tick += new System.EventHandler(this.backgroundWorkerTimer_Tick);
            // 
            // FolderWatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 109);
            this.Controls.Add(this.metroLabel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FolderWatcher";
            this.Text = "Sync control for branch";
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer backgroundWorkerTimer;
    }
}