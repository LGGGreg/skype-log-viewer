using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SkypeLogViewerLGG
{
    public partial class PreferencesForm : Form
    {
        private MainWindowForm mwf;
        public PreferencesForm(MainWindowForm inMwf)
        {
            this.mwf = inMwf;
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (Properties.Settings.Default.FormatSelection == "Default")
            {
                this.radioButtonDefault1.Checked = true;
            }
            else if (Properties.Settings.Default.FormatSelection == "OtherDefault")
            {
                this.radioButtonOtherDefault.Checked = true;
            }
            else if (Properties.Settings.Default.FormatSelection == "Custom")
            {
                this.radioButtonCustom.Checked = true;
            }
            if(Properties.Settings.Default.CustomFormat!="")this.textBox1custom.Text = Properties.Settings.Default.CustomFormat;

            this.updatePreview(null,null);
        }

        private void button1Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Format = this.getFormat();
            Properties.Settings.Default.CustomFormat = this.textBox1custom.Text;
            if (radioButtonDefault1.Checked)
            {
                Properties.Settings.Default.FormatSelection = "Default";
            }else if (radioButtonOtherDefault.Checked)
            {
                Properties.Settings.Default.FormatSelection = "OtherDefault";
            }
            else if (radioButtonCustom.Checked)
            {
                Properties.Settings.Default.FormatSelection = "Custom";
            }
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void updatePreview(object sender, EventArgs e)
        {
            SkypeMessage message = new SkypeMessage();
            message.senderID = "lordgreggreg";
            message.sender = "LGG";
            message.setData("Hello World!");
            message.msgDate = DateTime.Now;

            this.textBox1Preview.Text = mwf.formatMessage(message, this.getFormat());
        }

        private String getFormat()
        {
            String format = "[[Month]/[Day]/[Year] [Time]] [DisplayName]:";
            if (radioButtonOtherDefault.Checked)
            {
                format = "[[Month]/[Day]/[Year] [Time]] [DisplayName] ([UserName]):";
            }
            if (radioButtonCustom.Checked)
            {
                format = textBox1custom.Text;
            }
            return format;
        }

    }
}
