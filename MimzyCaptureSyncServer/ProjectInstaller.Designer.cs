namespace MimzyCaptureSyncServer
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MimzyCaptureSyncServiceProcessInstallerv2 = new System.ServiceProcess.ServiceProcessInstaller();
            this.MimzyCaptureSyncServiceInstallerv2 = new System.ServiceProcess.ServiceInstaller();
            // 
            // MimzyCaptureSyncServiceProcessInstallerv2
            // 
            this.MimzyCaptureSyncServiceProcessInstallerv2.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.MimzyCaptureSyncServiceProcessInstallerv2.Password = null;
            this.MimzyCaptureSyncServiceProcessInstallerv2.Username = null;
            // 
            // MimzyCaptureSyncServiceInstallerv2
            // 
            this.MimzyCaptureSyncServiceInstallerv2.DisplayName = "Mimzy Capture Sync Service";
            this.MimzyCaptureSyncServiceInstallerv2.ServiceName = "Mimzy Capture Sync Service";
            this.MimzyCaptureSyncServiceInstallerv2.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.MimzyCaptureSyncServiceProcessInstallerv2,
            this.MimzyCaptureSyncServiceInstallerv2});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller MimzyCaptureSyncServiceProcessInstallerv2;
        private System.ServiceProcess.ServiceInstaller MimzyCaptureSyncServiceInstallerv2;
    }
}