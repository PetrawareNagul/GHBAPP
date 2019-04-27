using MetroFramework;
using MetroFramework.Forms;
using Silverlake.Utility;
using Silverlake.Utility.Helper;
using Silverlake.Window.Custom;
using Silverlake.Window.ServiceCalls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Silverlake.Window
{
    public partial class LoginForm : MetroForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string encryptedPassword = CustomEncryptorDecryptor.EncryptPassword(password);
            StringBuilder filter = new StringBuilder();
            filter.Append(" 1=1");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<User>(nameof(User.Username)) + "='" + username + "'");
            filter.Append(" and " + Converter.GetColumnNameByPropertyName<User>(nameof(User.Password)) + "='" + encryptedPassword + "'");
            User user = AuthenticationApiCalls.AuthenticateUser(filter.ToString());
            if (user.Username != null)
            {
                APIUser.SetUser(user);
                this.Hide();

                var folderWatcher = new FolderWatcher();
                folderWatcher.Closed += (s, args) => this.Close();
                folderWatcher.Show();

                var mainForm = new MainForm();
                mainForm.Closed += (s, args) => this.Close();
                mainForm.Show();

                //var fileWatcher = new FileWatcher();
                //fileWatcher.Closed += (s, args) => this.Close();
                //fileWatcher.Show();
            }
            else if(user.IsOnline == 0 && user.UniqueKey != null)
            {
                txtPassword.Text = "";
                MetroMessageBox.Show(this, user.UniqueKey, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                txtPassword.Text = "";
                MetroMessageBox.Show(this, "Not Valid! Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
