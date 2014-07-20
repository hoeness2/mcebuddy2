using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCEBuddy.Util;
using MCEBuddy.Configuration;
using MCEBuddy.Globals;

namespace MCEBuddy.GUI
{
    public partial class CredentialsForm : Form
    {
        private bool _cancelled = true;
        private object options;

        public CredentialsForm(GeneralOptions go)
        {
            InitializeComponent();
            options = go;

            domainNameTxt.Text = go.domainName;
            userNameTxt.Text = go.userName;
            passwordTxt.Text = confirmPasswordTxt.Text = go.password;

            fallbackToSourceChk.Visible = fallbackToSourceChk.Enabled = false; // This option is not valid for general network credentials
        }
        
        public CredentialsForm(MonitorJobOptions mjo)
        {
            InitializeComponent();
            options = mjo;

            domainNameTxt.Text = mjo.domainName;
            userNameTxt.Text = mjo.userName;
            passwordTxt.Text = confirmPasswordTxt.Text = mjo.password;

            fallbackToSourceChk.Visible = fallbackToSourceChk.Enabled = false; // This option is not valid for Monitor Jobs
        }

        public CredentialsForm(ConversionJobOptions cjo)
        {
            InitializeComponent();
            options = cjo;

            domainNameTxt.Text = cjo.domainName;
            userNameTxt.Text = cjo.userName;
            passwordTxt.Text = confirmPasswordTxt.Text = cjo.password;

            fallbackToSourceChk.Checked = cjo.fallbackToSourcePath;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (passwordTxt.Text != confirmPasswordTxt.Text)
            {
                MessageBox.Show(Localise.GetPhrase("Passwords do not match, please reenter the passwords"), Localise.GetPhrase("Password Mismatch"));
                return;
            }

            _cancelled = false;

            if (object.ReferenceEquals(options.GetType(), (new MonitorJobOptions()).GetType())) // Check type of object
            {
                MonitorJobOptions mjo = (MonitorJobOptions)options;
                mjo.domainName = domainNameTxt.Text.Trim();
                mjo.userName = userNameTxt.Text.Trim();
                mjo.password = passwordTxt.Text;
            }
            else if (object.ReferenceEquals(options.GetType(), (new ConversionJobOptions()).GetType())) // Check type of object
            {
                ConversionJobOptions cjo = (ConversionJobOptions)options;
                cjo.domainName = domainNameTxt.Text.Trim();
                cjo.userName = userNameTxt.Text.Trim();
                cjo.password = passwordTxt.Text;
                cjo.fallbackToSourcePath = fallbackToSourceChk.Checked;
            }
            else if (object.ReferenceEquals(options.GetType(), (new GeneralOptions()).GetType())) // Check type of object
            {
                GeneralOptions go = (GeneralOptions)options;
                go.domainName = domainNameTxt.Text.Trim();
                go.userName = userNameTxt.Text.Trim();
                go.password = passwordTxt.Text;
            }

            this.Close();
        }

        private void ConnectionCredentials_Load(object sender, EventArgs e)
        {
            // Check for changes in screen resolution and screen changes (multi monitor support) - this HAS to be in Load and NOT in the constructor since the form has not been initiatized yet
            this.MaximumSize = Screen.FromControl(this).WorkingArea.Size; // Set the maximum size for the form based on working areas so we don't end up with dead/inaccessible locations

            LocaliseForms.LocaliseForm(this, toolTip);

            if (String.IsNullOrWhiteSpace(domainNameTxt.Text) && String.IsNullOrWhiteSpace(userNameTxt.Text) && String.IsNullOrWhiteSpace(passwordTxt.Text)) // if all are empty then use the default Guest username
                userNameTxt.Text = GlobalDefs.DEFAULT_NETWORK_USERNAME;

            _cancelled = true;
        }

        public bool Cancelled
        {
            get { return _cancelled; }
        }
    }
}
