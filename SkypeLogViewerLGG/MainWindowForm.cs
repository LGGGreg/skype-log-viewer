using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Data.SQLite;
using System.Reflection;
/*
This file is part of SkypeLogViewerLGG

    SkypeLogViewerLGG is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    SkypeLogViewerLGG is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with SkypeLogViewerLGG.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace SkypeLogViewerLGG
{
    public partial class MainWindowForm : Form
    {
        private string skypePath = "";
        SQLiteConnection connection;
        Conversation currentConversation;

        public MainWindowForm()
        {
            loadSQL();
            InitializeComponent();
            this.imageList1.Images.Add((Image)(Properties.Resources.SkypeLogSLGG));
            this.imageList1.Images.Add((Image)(Properties.Resources.SkypeLogLGG));
            this.imageList1.Images.Add((Image)(Properties.Resources.SkypeLogSBLGG));            
        }
        private void loadSQL()
        {
            string name = "System.Data.SQLite.dll";
            String resourceName = "SkypeLogViewerLGG." +
              name;
            if (File.Exists(name)) return;
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(resourceName))
            {
                using (System.IO.FileStream fs = 
                    new System.IO.FileStream(name, System.IO.FileMode.Create))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    fs.Write(assemblyData, 0, assemblyData.Length);
                }
            }
        }
        public void debugAdd(string info)
        {
            this.textBox1debug.Text += "\r\n" + info;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            skypePath= Environment.GetEnvironmentVariable("APPDATA") + @"\Skype";
            this.textBoxDataBasePath.Text = skypePath;
            if (Properties.Settings.Default.LastPath != "")
            {
                fillUsers();
                this.textBoxDataBasePath.Text = Properties.Settings.Default.LastPath;
                tryLoad();
            } else
            if (fillUsers())
            {
                this.comboBox1.SelectedIndex = 0;//triggers the open                
            }
        }
        public bool checkAndKilledSkype()
        {
            foreach (Process process in Process.GetProcesses(Environment.MachineName))
            {
                if (process.ProcessName.Equals("Skype"))
                {
                    debugAdd("Skype Running");
                    DialogResult result = MessageBox.Show("You must close skype to use this program\nDo you wish to do this now?",
                        "Close Skype?", MessageBoxButtons.YesNoCancel);
                    if (result== DialogResult.Yes)
                    {
                        process.Kill();                        
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return false;
                    }
                    return true;
                }
            }
            return true;
        }
        private bool fillUsers()
        {
            bool found = false;
            if (!Directory.Exists(skypePath)) return found;
            string[] folders = Directory.GetDirectories(skypePath);
            foreach (string folderName in folders)
            {
                FileInfo folderInfo = new FileInfo(folderName);
                if (!Regex.Match(folderInfo.Name, "content|pictures|dbtemp|shared.*|My Skype Received Files", RegexOptions.IgnoreCase).Success)
                {
                    if (File.Exists(folderInfo.FullName + @"\main.db"))
                    {
                        this.comboBox1.Items.Add(folderInfo.Name);
                        found = true;
                    }
                }
            }
            return found;
        }
        public void tryLoad()
        {
            listViewConversations.Items.Clear();

            connection = new SQLiteConnection(
                        "data source=" + this.textBoxDataBasePath.Text);
            if (true)//checkAndKilledSkype()
            {
                if (File.Exists(textBoxDataBasePath.Text))
                {                    
                    try
                    {
                        connection.Open();
                        debugAdd("connection  opened");
                        Properties.Settings.Default.LastPath = textBoxDataBasePath.Text;
                        Properties.Settings.Default.Save();
                    }
                    catch
                    {
                        if(checkAndKilledSkype())
                            tryLoad();
                    }
                }
                else
                {
                    MessageBox.Show("Error, file does not exist");
                    return;
                }
            }
            //file should be open 
            SQLiteCommand cmd = connection.CreateCommand();
            //last_activity_timestamp
            //consumption_horizon
            //inbox_timestamp
            cmd.CommandText = "select * from Conversations order by last_activity_timestamp desc";
            SQLiteDataReader dataRead = cmd.ExecuteReader();
            listViewConversations.Items.Clear();
            while (dataRead.Read())
            {
                Conversation tempConv = new Conversation(
                    dataRead["displayname"].ToString(),
                    dataRead["identity"].ToString(),
                    dataRead["type"].ToString());
                if (tempConv.DisplayName == "") continue;
                if (tempConv.getType()==1)
                {
                    //we need the real name
                    SQLiteCommand namecmd = connection.CreateCommand();
                    namecmd.CommandText = "select name from Chats WHERE dialog_partner ='"
                        +dataRead["identity"]+"' order by timestamp";
                    SQLiteDataReader chatDataRead = namecmd.ExecuteReader();
                    List<String> chats = new List<String>();
                    while (chatDataRead.Read())
                        chats.Add(chatDataRead[0].ToString());
                    tempConv.chats = chats.ToArray();
                    if (tempConv.chats.Length <= 0)
                    {                        
                        //tempConv.setType(3);
                        continue;
                    }
                }
                if (tempConv.getType() < 3)
                {
                    ListViewItem tempItem = new ListViewItem(tempConv.DisplayName);
                    tempItem.Tag = tempConv;
                    tempItem.ToolTipText = tempConv.DisplayName + "\r\n" + tempConv.Identity;
                    tempItem.ImageIndex = tempItem.StateImageIndex = tempConv.getType() - 1;
                    listViewConversations.Items.Add(tempItem);
                }
                debugAdd("Found: "+ dataRead["displayname"] + " at "+tempConv.Identity);
            }
            cmd.Dispose();
            // find broken msn b.s. 
            if (true)
            {

                SQLiteCommand brokenCmd = connection.CreateCommand();
                brokenCmd.CommandText = "select DISTINCT author,from_dispname from Messages where ifnull(chatname, '') = '' and ifnull(dialog_partner, '') = '' order by timestamp desc";
                SQLiteDataReader brokenDataRead = brokenCmd.ExecuteReader();
                while (brokenDataRead.Read())
                {
                    debugAdd("found missing partner with id of " + brokenDataRead["from_dispname"]);
                    Conversation tempConv = new Conversation(
                        brokenDataRead["from_dispname"].ToString(),
                        "",
                        "3");
                    List<String> chats = new List<String>();
                    chats.Add(brokenDataRead["from_dispname"].ToString());
                    chats.Add(brokenDataRead["author"].ToString());
                    tempConv.chats = chats.ToArray();
                    ListViewItem tempItem = new ListViewItem(tempConv.DisplayName);
                    tempItem.Tag = tempConv;
                    tempItem.ToolTipText = tempConv.DisplayName + "\r\n" + tempConv.Identity;
                    tempItem.ImageIndex = tempItem.StateImageIndex = tempConv.getType() - 1;
                    listViewConversations.Items.Add(tempItem);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = skypePath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxDataBasePath.Text = openFileDialog1.FileName;
            }
            tryLoad();
        }
        private void loadMessagesFromIdentity(Conversation c)
        {
            List<SkypeMessage> messages = getSkypeMessages(c);
            displayMessages(messages);            
        }
        private void displayMessages(List<SkypeMessage> messages)
        {
            listView1.Items.Clear();
            foreach (SkypeMessage message in messages)
            {
                ListViewItem tempItem = new ListViewItem(message.msgDate.ToString());
                tempItem.SubItems.Add(message.sender);
                tempItem.SubItems.Add(message.msgData);
                tempItem.Tag = message;
                tempItem.ToolTipText = message.msgDate.ToString() + "\r\n" + message.sender + "\r\n" + message.msgData;
                listView1.Items.Add(tempItem);
            }
        }
        private List<SkypeMessage> getSkypeMessages(Conversation c)
        {
            List<SkypeMessage> messages = new List<SkypeMessage>();

            //debugAdd("Loading messages for " + c.Identity);
            if (c.getType() == 2)
            {
                SQLiteCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select timestamp,from_dispname,author,body_xml from Messages where chatname='" + c.Identity + "' order by timestamp";
                SQLiteDataReader dataRead = cmd.ExecuteReader();
                while (dataRead.Read())
                {
                    //debugAdd("Found message " + dataRead["body_xml"]);
                    SkypeMessage message = new SkypeMessage();
                    message.sender = dataRead["from_dispname"].ToString();
                    message.senderID = dataRead["author"].ToString();
                    message.setData(dataRead["body_xml"].ToString());
                    message.setDate(dataRead["timestamp"].ToString());
                    messages.Add(message);
                }
            }
            else if(c.getType()==1)
            {
                if (c.chats.Length == 0) return messages;
                String wheres = "";
                foreach (string name in c.chats)
                {
                    wheres += "OR chatname='" + name + "' ";
                }
                SQLiteCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select timestamp,from_dispname,author,body_xml from Messages where "
                    + wheres.Substring(3)
                    + " order by timestamp";
                SQLiteDataReader dataRead = cmd.ExecuteReader();
                while (dataRead.Read())
                {
                    //debugAdd("Found message " + dataRead["body_xml"]);
                    SkypeMessage message = new SkypeMessage();
                    message.sender = dataRead["from_dispname"].ToString();
                    message.senderID = dataRead["author"].ToString();
                    message.setData(dataRead["body_xml"].ToString());
                    message.setDate(dataRead["timestamp"].ToString());
                    messages.Add(message);
                }
            }
            else if (c.getType() == 3)
            {
                String wheres = "";
                if(c.chats!=null)
                foreach (string name in c.chats)
                {
                    wheres += "OR dialog_partner='" + name + "' ";
                }
                SQLiteCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select timestamp,from_dispname,author,body_xml from Messages where "
                    + "ifnull(chatname, '') = '' and from_dispname='"+c.DisplayName+"'" + wheres
                    + " order by timestamp";
                SQLiteDataReader dataRead = cmd.ExecuteReader();
                while (dataRead.Read())
                {
                    SkypeMessage message = new SkypeMessage();
                    message.sender = dataRead["from_dispname"].ToString();
                    message.senderID = dataRead["author"].ToString();
                    message.setData(dataRead["body_xml"].ToString());
                    message.setDate(dataRead["timestamp"].ToString());
                    messages.Add(message);
                }
            }

            return messages;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxDataBasePath.Text = skypePath + @"\" + this.comboBox1.Items[0] + @"\main.db";
                
            tryLoad();
        }

        private void listViewConversations_DoubleClick(object sender, EventArgs e)
        {
            //loadMessagesFromIdentity((listViewConversations.SelectedItems[0].Tag as Conversation));
            if (saveAllWorker.IsBusy)
            {
                MessageBox.Show("Please watch the progress bar\nAnd wait till the save all is completed");
                return;
            }
            while (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            backgroundWorker1.RunWorkerAsync(listViewConversations.SelectedItems[0].Tag);
            currentConversation = (listViewConversations.SelectedItems[0].Tag as Conversation);
            this.toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            this.toolStripStatusLabel1.Text = "Loading, please wait!";
        }
        
        private void button1clearDebug_Click(object sender, EventArgs e)
        {
            textBox1debug.Text = "";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Conversation c = (e.Argument as Conversation);
            List<SkypeMessage> messages = getSkypeMessages(c);
            e.Result = messages;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(!e.Cancelled)
                displayMessages((e.Result as List<SkypeMessage>));
            this.toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
            this.toolStripStatusLabel1.Text = "Ready";
        }

        private void exportCurrentConvo_Click(object sender, EventArgs e)
        {
            if (currentConversation != null)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    exportConversation(currentConversation, folderBrowserDialog1.SelectedPath);
                }
            }
        }

        public String formatMessage(SkypeMessage message, String messageFormat = "[[Month]/[Day]/[Year] [Time]] [DisplayName]:")
        {
            
            messageFormat = messageFormat.Replace("[Day]", message.msgDate.Day.ToString());
            messageFormat = messageFormat.Replace("[Month]", message.msgDate.Month.ToString());
            messageFormat = messageFormat.Replace("[Year]", message.msgDate.Year.ToString());

            messageFormat = messageFormat.Replace("[Time]", message.msgDate.ToString("h:mm:ss tt"));


            messageFormat = messageFormat.Replace("[DisplayName]", message.sender);
            messageFormat = messageFormat.Replace("[UserName]", message.senderID);

            messageFormat += " " + message.msgData;

            return messageFormat;
        }

        private void exportConversation(Conversation c, String directory)
        {
            String fileName = directory + "\\" +
                (c.DisplayName+(c.getType()==3?"-Broken Log":"")).Replace("\\", "-").Replace("/", "-").Replace(":", "-").Replace("*", "-")
                .Replace("\"", "-").Replace("|", "-").Replace(">", "-").Replace("<", "-").Replace("?", "-")
                +".txt";
            List<SkypeMessage> messages = getSkypeMessages(c);               
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                foreach (SkypeMessage message in messages)
                {
                    String format = (Properties.Settings.Default.Format != "") ? Properties.Settings.Default.Format : "[[Month]/[Day]/[Year] [Time]] [DisplayName]:";
                    file.WriteLine(this.formatMessage(message,format));
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm();
            form.ShowDialog();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesForm form = new PreferencesForm(this);
            form.ShowDialog();
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(
                "This process will take quite a long time\r\nPlease watch the progress bar at the bottom for it to finish\r\n"
                , "Long wait expected", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    convsAndSaveDir casd = new convsAndSaveDir();
                    casd.saveDir = folderBrowserDialog1.SelectedPath;
                    casd.convs = new List<Conversation>();
                    foreach (ListViewItem i in listViewConversations.Items)
                    {
                        casd.convs.Add(i.Tag as Conversation);
                    }
                    saveAllWorker.RunWorkerAsync(casd);
                    toolStripProgressBar1.Value = 0;
                    toolStripStatusLabel1.Text = "Saving all Conversations";
                }                
            }
        }
        private void exportConversations(List<Conversation> conversations, string dest)
        {
            int done = 1;
            foreach(Conversation c in conversations)
            {
                exportConversation(c, dest);
                saveAllWorker.ReportProgress((int)(100.0f*((float)(done++))/((float)(conversations.Count))));
            }
        }

        private void saveAllWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            convsAndSaveDir casd = (e.Argument as convsAndSaveDir);
            exportConversations(casd.convs, casd.saveDir);
        }

        private void saveAllWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            toolStripStatusLabel1.Text = "Ready";
            MessageBox.Show("Export completed");
        }

        private void saveAllWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder("");
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                SkypeMessage message = item.Tag as SkypeMessage;
                sb.AppendLine("["+message.msgDate.ToString()+"] "
                    +message.sender+": " 
                    +message.msgData);
            }
            if(sb.ToString()!="")Clipboard.SetText(sb.ToString());
        }

    }
    public class convsAndSaveDir
    {
        public List<Conversation> convs { get; set; }
        public string saveDir { get; set; }
    }
    public class Conversation
    {
        public string DisplayName { get; set; }
        public string Identity { get; set; }
        private int type = 0;
        public string[] chats { get; set; }
        public Conversation(string _displayName, string _identity, string _type)
        {
            DisplayName = _displayName;
            Identity = _identity;
            this.type = int.Parse(_type);
        }
        public int getType() { return type; }
        public void setType(int newType) { type = newType; }
        public override string ToString()
        {
            return DisplayName;
        }
    }
    public class SkypeMessage
    {
        private DateTime _msgDate;
        private string _msgData;
        public DateTime msgDate { get { return _msgDate; } set { _msgDate = value; } }
        public string msgData { get { return _msgData; } }
        public string sender { get; set; }
        public string senderID { get; set; }
        public void setData(string xmlin)
        {
            _msgData =
                Regex.Replace(
                Regex.Replace(
                xmlin
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&amp;", "&")
                .Replace("&quot;", "\"")
                .Replace("&apos;", "'")
                .Replace("</ss>", "")
                .Replace("</a>"," "),
                "<ss type=\".+\">", ""),
                "<a href=\".+\">", "");
        }
        public void setDate(string timestamp)
        {
            _msgDate=new DateTime(1970, 1, 1, 0, 0, 0, 0)
                        .AddSeconds(int.Parse(timestamp)).ToLocalTime();
        }
    }

}
