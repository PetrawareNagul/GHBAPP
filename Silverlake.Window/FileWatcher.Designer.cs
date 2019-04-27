namespace Silverlake.Window
{
    partial class FileWatcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileWatcher));
            this.fromADCLogListView = new MetroFramework.Controls.MetroListView();
            this.writeFromADCLog = new System.Windows.Forms.Timer(this.components);
            this.writeSplitFromADCLog = new System.Windows.Forms.Timer(this.components);
            this.splitFromADCLogListView = new MetroFramework.Controls.MetroListView();
            this.writeADCStatusLog = new System.Windows.Forms.Timer(this.components);
            this.ADCStatusLogListView = new MetroFramework.Controls.MetroListView();
            this.htmlLabel1 = new MetroFramework.Drawing.Html.HtmlLabel();
            this.htmlLabel2 = new MetroFramework.Drawing.Html.HtmlLabel();
            this.htmlLabel3 = new MetroFramework.Drawing.Html.HtmlLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // fromADCLogListView
            // 
            this.fromADCLogListView.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.fromADCLogListView.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.fromADCLogListView.ForeColor = System.Drawing.Color.Black;
            this.fromADCLogListView.FullRowSelect = true;
            this.fromADCLogListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.fromADCLogListView.Location = new System.Drawing.Point(23, 88);
            this.fromADCLogListView.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.fromADCLogListView.MultiSelect = false;
            this.fromADCLogListView.Name = "fromADCLogListView";
            this.fromADCLogListView.OwnerDraw = true;
            this.fromADCLogListView.Size = new System.Drawing.Size(350, 158);
            this.fromADCLogListView.TabIndex = 1;
            this.fromADCLogListView.UseCompatibleStateImageBehavior = false;
            this.fromADCLogListView.UseCustomBackColor = true;
            this.fromADCLogListView.UseCustomForeColor = true;
            this.fromADCLogListView.UseSelectable = true;
            this.fromADCLogListView.View = System.Windows.Forms.View.Details;
            // 
            // writeFromADCLog
            // 
            this.writeFromADCLog.Enabled = true;
            this.writeFromADCLog.Tick += new System.EventHandler(this.writeFromADCLog_Tick);
            // 
            // writeSplitFromADCLog
            // 
            this.writeSplitFromADCLog.Enabled = true;
            this.writeSplitFromADCLog.Tick += new System.EventHandler(this.writeSplitFromADCLog_Tick);
            // 
            // splitFromADCLogListView
            // 
            this.splitFromADCLogListView.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitFromADCLogListView.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.splitFromADCLogListView.ForeColor = System.Drawing.Color.Black;
            this.splitFromADCLogListView.FullRowSelect = true;
            this.splitFromADCLogListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.splitFromADCLogListView.Location = new System.Drawing.Point(23, 271);
            this.splitFromADCLogListView.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.splitFromADCLogListView.MultiSelect = false;
            this.splitFromADCLogListView.Name = "splitFromADCLogListView";
            this.splitFromADCLogListView.OwnerDraw = true;
            this.splitFromADCLogListView.Size = new System.Drawing.Size(350, 158);
            this.splitFromADCLogListView.TabIndex = 2;
            this.splitFromADCLogListView.UseCompatibleStateImageBehavior = false;
            this.splitFromADCLogListView.UseCustomBackColor = true;
            this.splitFromADCLogListView.UseCustomForeColor = true;
            this.splitFromADCLogListView.UseSelectable = true;
            this.splitFromADCLogListView.View = System.Windows.Forms.View.Details;
            // 
            // writeADCStatusLog
            // 
            this.writeADCStatusLog.Enabled = true;
            this.writeADCStatusLog.Tick += new System.EventHandler(this.writeADCStatusLog_Tick);
            // 
            // ADCStatusLogListView
            // 
            this.ADCStatusLogListView.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ADCStatusLogListView.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ADCStatusLogListView.ForeColor = System.Drawing.Color.Black;
            this.ADCStatusLogListView.FullRowSelect = true;
            this.ADCStatusLogListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ADCStatusLogListView.Location = new System.Drawing.Point(386, 88);
            this.ADCStatusLogListView.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.ADCStatusLogListView.MultiSelect = false;
            this.ADCStatusLogListView.Name = "ADCStatusLogListView";
            this.ADCStatusLogListView.OwnerDraw = true;
            this.ADCStatusLogListView.Size = new System.Drawing.Size(306, 341);
            this.ADCStatusLogListView.TabIndex = 3;
            this.ADCStatusLogListView.UseCompatibleStateImageBehavior = false;
            this.ADCStatusLogListView.UseCustomBackColor = true;
            this.ADCStatusLogListView.UseCustomForeColor = true;
            this.ADCStatusLogListView.UseSelectable = true;
            this.ADCStatusLogListView.View = System.Windows.Forms.View.Details;
            // 
            // htmlLabel1
            // 
            this.htmlLabel1.AutoScroll = true;
            this.htmlLabel1.AutoScrollMinSize = new System.Drawing.Size(62, 23);
            this.htmlLabel1.AutoSize = false;
            this.htmlLabel1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.htmlLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.htmlLabel1.Location = new System.Drawing.Point(311, 64);
            this.htmlLabel1.Name = "htmlLabel1";
            this.htmlLabel1.Size = new System.Drawing.Size(62, 23);
            this.htmlLabel1.TabIndex = 4;
            this.htmlLabel1.Text = "From ADC";
            // 
            // htmlLabel2
            // 
            this.htmlLabel2.AutoScroll = true;
            this.htmlLabel2.AutoScrollMinSize = new System.Drawing.Size(87, 23);
            this.htmlLabel2.AutoSize = false;
            this.htmlLabel2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.htmlLabel2.Location = new System.Drawing.Point(284, 247);
            this.htmlLabel2.Name = "htmlLabel2";
            this.htmlLabel2.Size = new System.Drawing.Size(89, 23);
            this.htmlLabel2.TabIndex = 5;
            this.htmlLabel2.Text = "Split From ADC";
            // 
            // htmlLabel3
            // 
            this.htmlLabel3.AutoScroll = true;
            this.htmlLabel3.AutoScrollMinSize = new System.Drawing.Size(67, 23);
            this.htmlLabel3.AutoSize = false;
            this.htmlLabel3.BackColor = System.Drawing.SystemColors.HighlightText;
            this.htmlLabel3.Location = new System.Drawing.Point(623, 64);
            this.htmlLabel3.Name = "htmlLabel3";
            this.htmlLabel3.Size = new System.Drawing.Size(69, 23);
            this.htmlLabel3.TabIndex = 6;
            this.htmlLabel3.Text = "ADC Status";
            // 
            // metroLabel3
            // 
            this.metroLabel3.CausesValidation = false;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel3.Location = new System.Drawing.Point(587, 31);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(105, 29);
            this.metroLabel3.TabIndex = 12;
            this.metroLabel3.Text = "File watcher";
            // 
            // FileWatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 452);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.htmlLabel3);
            this.Controls.Add(this.htmlLabel2);
            this.Controls.Add(this.htmlLabel1);
            this.Controls.Add(this.ADCStatusLogListView);
            this.Controls.Add(this.splitFromADCLogListView);
            this.Controls.Add(this.fromADCLogListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileWatcher";
            this.Style = MetroFramework.MetroColorStyle.Silver;
            this.Text = "Sync control for branch";
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroListView fromADCLogListView;
        private System.Windows.Forms.Timer writeFromADCLog;
        private System.Windows.Forms.Timer writeSplitFromADCLog;
        private MetroFramework.Controls.MetroListView splitFromADCLogListView;
        private System.Windows.Forms.Timer writeADCStatusLog;
        private MetroFramework.Controls.MetroListView ADCStatusLogListView;
        private MetroFramework.Drawing.Html.HtmlLabel htmlLabel1;
        private MetroFramework.Drawing.Html.HtmlLabel htmlLabel2;
        private MetroFramework.Drawing.Html.HtmlLabel htmlLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel3;
    }
}