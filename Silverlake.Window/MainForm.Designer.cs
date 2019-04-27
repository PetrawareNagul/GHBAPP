namespace Silverlake.Window
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnPostFiles = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ddlBranches = new System.Windows.Forms.ComboBox();
            this.ddlDepartments = new System.Windows.Forms.ComboBox();
            this.txtBatchCount = new System.Windows.Forms.TextBox();
            this.ddlBatches = new System.Windows.Forms.ComboBox();
            this.ddlStages = new System.Windows.Forms.ComboBox();
            this.btnUpdateBatchStatus = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSplitAndPost = new System.Windows.Forms.Button();
            this.btnConvertTIFFsToPDF = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.btnOpenFolderWatcher = new MetroFramework.Controls.MetroButton();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPostFiles
            // 
            this.btnPostFiles.Location = new System.Drawing.Point(3, 3);
            this.btnPostFiles.Name = "btnPostFiles";
            this.btnPostFiles.Size = new System.Drawing.Size(161, 23);
            this.btnPostFiles.TabIndex = 0;
            this.btnPostFiles.Text = "Post Files";
            this.btnPostFiles.UseVisualStyleBackColor = true;
            this.btnPostFiles.Click += new System.EventHandler(this.btnPostFiles_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(168, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Post New Batch";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnNewBatch_Click);
            // 
            // ddlBranches
            // 
            this.ddlBranches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlBranches.FormattingEnabled = true;
            this.ddlBranches.Location = new System.Drawing.Point(168, 94);
            this.ddlBranches.Name = "ddlBranches";
            this.ddlBranches.Size = new System.Drawing.Size(121, 21);
            this.ddlBranches.TabIndex = 4;
            // 
            // ddlDepartments
            // 
            this.ddlDepartments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDepartments.FormattingEnabled = true;
            this.ddlDepartments.Location = new System.Drawing.Point(168, 121);
            this.ddlDepartments.Name = "ddlDepartments";
            this.ddlDepartments.Size = new System.Drawing.Size(121, 21);
            this.ddlDepartments.TabIndex = 5;
            this.ddlDepartments.SelectedIndexChanged += new System.EventHandler(this.ddlDepartments_SelectedIndexChanged);
            // 
            // txtBatchCount
            // 
            this.txtBatchCount.Location = new System.Drawing.Point(168, 148);
            this.txtBatchCount.Name = "txtBatchCount";
            this.txtBatchCount.Size = new System.Drawing.Size(121, 20);
            this.txtBatchCount.TabIndex = 6;
            // 
            // ddlBatches
            // 
            this.ddlBatches.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlBatches.FormattingEnabled = true;
            this.ddlBatches.Location = new System.Drawing.Point(324, 94);
            this.ddlBatches.Name = "ddlBatches";
            this.ddlBatches.Size = new System.Drawing.Size(121, 21);
            this.ddlBatches.TabIndex = 7;
            this.ddlBatches.SelectedIndexChanged += new System.EventHandler(this.ddlBatches_SelectedIndexChanged);
            // 
            // ddlStages
            // 
            this.ddlStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlStages.FormattingEnabled = true;
            this.ddlStages.Location = new System.Drawing.Point(324, 121);
            this.ddlStages.Name = "ddlStages";
            this.ddlStages.Size = new System.Drawing.Size(121, 21);
            this.ddlStages.TabIndex = 8;
            // 
            // btnUpdateBatchStatus
            // 
            this.btnUpdateBatchStatus.Location = new System.Drawing.Point(324, 148);
            this.btnUpdateBatchStatus.Name = "btnUpdateBatchStatus";
            this.btnUpdateBatchStatus.Size = new System.Drawing.Size(121, 23);
            this.btnUpdateBatchStatus.TabIndex = 9;
            this.btnUpdateBatchStatus.Text = "Update Batch Status";
            this.btnUpdateBatchStatus.UseVisualStyleBackColor = true;
            this.btnUpdateBatchStatus.Click += new System.EventHandler(this.btnUpdateBatchStatus_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnPostFiles);
            this.flowLayoutPanel1.Controls.Add(this.btnSplitAndPost);
            this.flowLayoutPanel1.Controls.Add(this.btnConvertTIFFsToPDF);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(164, 228);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(320, 58);
            this.flowLayoutPanel1.TabIndex = 10;
            // 
            // btnSplitAndPost
            // 
            this.btnSplitAndPost.Location = new System.Drawing.Point(3, 32);
            this.btnSplitAndPost.Name = "btnSplitAndPost";
            this.btnSplitAndPost.Size = new System.Drawing.Size(161, 23);
            this.btnSplitAndPost.TabIndex = 11;
            this.btnSplitAndPost.Text = "Split and Post";
            this.btnSplitAndPost.UseVisualStyleBackColor = true;
            this.btnSplitAndPost.Click += new System.EventHandler(this.btnSplitAndPost_Click);
            // 
            // btnConvertTIFFsToPDF
            // 
            this.btnConvertTIFFsToPDF.Location = new System.Drawing.Point(170, 32);
            this.btnConvertTIFFsToPDF.Name = "btnConvertTIFFsToPDF";
            this.btnConvertTIFFsToPDF.Size = new System.Drawing.Size(147, 23);
            this.btnConvertTIFFsToPDF.TabIndex = 11;
            this.btnConvertTIFFsToPDF.Text = "Convert TIFFs to PDF";
            this.btnConvertTIFFsToPDF.UseVisualStyleBackColor = true;
            this.btnConvertTIFFsToPDF.Click += new System.EventHandler(this.btnConvertTIFFsToPDF_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(167, 211);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(79, 13);
            this.lblMessage.TabIndex = 11;
            this.lblMessage.Text = "Label Message";
            // 
            // metroLabel3
            // 
            this.metroLabel3.CausesValidation = false;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel3.Location = new System.Drawing.Point(379, 34);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(105, 29);
            this.metroLabel3.TabIndex = 13;
            this.metroLabel3.Text = "Simulation";
            // 
            // btnOpenFolderWatcher
            // 
            this.btnOpenFolderWatcher.Location = new System.Drawing.Point(23, 295);
            this.btnOpenFolderWatcher.Name = "btnOpenFolderWatcher";
            this.btnOpenFolderWatcher.Size = new System.Drawing.Size(472, 23);
            this.btnOpenFolderWatcher.TabIndex = 14;
            this.btnOpenFolderWatcher.Text = "Open folder watcher";
            this.btnOpenFolderWatcher.UseSelectable = true;
            this.btnOpenFolderWatcher.Click += new System.EventHandler(this.btnOpenFolderWatcher_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 341);
            this.Controls.Add(this.btnOpenFolderWatcher);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnUpdateBatchStatus);
            this.Controls.Add(this.ddlStages);
            this.Controls.Add(this.ddlBatches);
            this.Controls.Add(this.txtBatchCount);
            this.Controls.Add(this.ddlDepartments);
            this.Controls.Add(this.ddlBranches);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Style = MetroFramework.MetroColorStyle.Silver;
            this.Text = "Sync control for branch";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPostFiles;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox ddlBranches;
        private System.Windows.Forms.ComboBox ddlDepartments;
        private System.Windows.Forms.TextBox txtBatchCount;
        private System.Windows.Forms.ComboBox ddlBatches;
        private System.Windows.Forms.ComboBox ddlStages;
        private System.Windows.Forms.Button btnUpdateBatchStatus;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnSplitAndPost;
        private System.Windows.Forms.Button btnConvertTIFFsToPDF;
        private System.Windows.Forms.Label lblMessage;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroButton btnOpenFolderWatcher;
    }
}

