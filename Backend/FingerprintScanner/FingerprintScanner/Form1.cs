using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace FingerprintScanner
{
    public partial class Form1 : Form
    {
        static public void AppendTextBox(string value)
        {
            if (logBox.InvokeRequired)
            {
                logBox.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            logBox.AppendText(value);
        }
        static private RichTextBox logBox;
        Accounts fingerAccountManager = new Accounts();
        ArduinoCom ArduinoCom = new ArduinoCom();
        Database Database;

        public Form1()
        {
            fingerAccountManager = Database.Read("database.json");
            Database = new Database("database.json", fingerAccountManager);
            HttpServer.accounts = fingerAccountManager;
            InitializeComponent();
            logBox = richTextBox1;            
            logBox.AppendText("Initialized\n");
            comboBox2.Items.AddRange(SerialPort.GetPortNames());
            comboBox1.Items.AddRange(new string[] {"4800", "9600","57600", "128000" });            
            if (comboBox2.Items.Count > 0)
            comboBox2.SelectedItem = comboBox2.Items[0];
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedItem = comboBox1.Items[0];
            textBox2.Text = "Closed";
            textBox4.Text = "Inactive";
            Database.autosaveTime = new TimeSpan(0, (int)numericUpDown3.Value, 0);
            comboBox3.Items.AddRange(fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).ToArray());
            comboBox4.Items.AddRange(fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).ToArray());
            foreach (var fin in fingerAccountManager.fingerAccounts.Values.Where(n => n.loggedIn))
                richTextBox3.AppendText(fin.Name + "\n");
            foreach (var fin in fingerAccountManager.fingerAccounts.Values.Where(n => !(n.loggedIn)))
                richTextBox2.AppendText(fin.Name + "\n");
            comboBox3.Items.Add("");
            comboBox3.SelectedItem = "";
            numericUpDown3.Enabled = false;
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;

            //Change value of the delete thing
            int val = (int)numericUpDown2.Value;
            if (fingerAccountManager.fingerAccounts.ContainsKey(val))
            {
                var item = fingerAccountManager.fingerAccounts[val];
                if (comboBox3.Items.Contains(item.Name))
                {
                    comboBox3.SelectedItem = item.Name;
                }
                else
                {
                    comboBox3.Items.Add(item.Name);
                    comboBox3.SelectedItem = item.Name;
                }
            }
            else
            {
                comboBox3.SelectedItem = "";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Enabled = false;
                if (HttpServer.StartWebServer().GetAwaiter().GetResult())
                {
                    checkBox1.Checked = false;
                    return;
                }
                checkBox1.Enabled = true;
                textBox4.Text = "Active";
            }
            else
            {
                checkBox1.Enabled = false;
                HttpServer.StopWebServer().Wait();
                textBox4.Text = "Inactive";
                checkBox1.Enabled = true;
            }
        }

        //Add fingerprint checkbox
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                if (ArduinoCom.FingerRelay == null)
                {
                    checkBox3.Checked = false;
                    checkBox3.Enabled = false;
                }
            }
            else
            {
                if (ArduinoCom.FingerRelay == null)
                {
                    checkBox3.Checked = false;
                    checkBox3.Enabled = false;
                }
            }
        }
        //Add user button
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null && textBox1.Text.Length < 1) { return; }
            int tempnum = 0;
            if (checkBox3.Checked && ArduinoCom.FingerRelay != null)
            {
                while (tempnum == 0)
                tempnum = FingerInterfacer.CreateAScan(ArduinoCom, (int)numericUpDown1.Value);
            }
            else
            {
                tempnum = (int)numericUpDown1.Value;
            }
            if (fingerAccountManager.fingerAccounts.ContainsKey(tempnum))
            {
                var old = fingerAccountManager.fingerAccounts[tempnum];
                fingerAccountManager.fingerAccounts[tempnum] = new FingerAccount(textBox1.Text, tempnum);
                AppendTextBox($"Modified user {old.Name} ({old.templateId}) to fit the new properties of {textBox1.Text} ({tempnum})\n");
                //comboBox3.Items.Clear();
                //comboBox3.Items.AddRange(fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).ToArray());
                //comboBox3.Items.Add("");
            }
            else
            {
                fingerAccountManager.fingerAccounts.Add(tempnum, new FingerAccount(textBox1.Text, tempnum));
                AppendTextBox($"Added new user {textBox1.Text} ({tempnum})\n");
                //comboBox3.Items.Clear();
                //comboBox3.Items.AddRange(fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).ToArray());
                //comboBox3.Items.Add("");
            }

            ////Change delete numeric up down value
            //int val = (int)numericUpDown2.Value;
            //if (fingerAccountManager.fingerAccounts.ContainsKey(val))
            //{
            //    var item = fingerAccountManager.fingerAccounts[val];
            //    if (comboBox3.Items.Contains(item.Name))
            //    {
            //        comboBox3.SelectedItem = item.Name;
            //    }
            //    else
            //    {
            //        comboBox3.Items.Add(item.Name);
            //        comboBox3.SelectedItem = item.Name;
            //    }
            //}
            //else
            //{
            //    comboBox3.SelectedItem = "";
            //}
            RefreshAll();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Database.Save("database.json", fingerAccountManager);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                if (ArduinoCom.ArduinoPort != null)
                {
                    if (ArduinoCom.ArduinoPort.IsOpen)
                    ArduinoCom.ArduinoPort.Close();
                    logBox.AppendText("Closed COM\n");
                    textBox2.Text = "Closed";
                    button1.Enabled = false;
                    checkBox3.Enabled = false;
                    checkBox3.Checked = false;
                    checkBox4.Enabled = false;
                }
            }
            else
            {
                if (ArduinoCom.TryOpen(comboBox2.SelectedItem.ToString(), Convert.ToInt32(comboBox1.SelectedItem), out ArduinoCom))
                {
                    textBox2.Text = $"{comboBox2.SelectedItem} Open";
                    logBox.AppendText($"Opened {ArduinoCom.ArduinoPort.PortName}\n");
                    button1.Enabled = true;
                    checkBox3.Enabled = true;
                    checkBox4.Enabled = true;
                }
                else
                {
                    textBox2.Text = "Invalid port or baud";
                    checkBox2.Checked = false;
                    button1.Enabled = false;
                    checkBox3.Enabled = false;
                    checkBox3.Checked = false;
                    checkBox4.Enabled = false;
                }
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (ArduinoCom.ArduinoPort != null)
            {
                ArduinoCom.ArduinoPort.DiscardInBuffer();
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                Database.Autosave = true;
                Task.Run(Database.AutoSave);
                numericUpDown3.Enabled = true;
            }
            else
            {
                numericUpDown3.Enabled = false;
                Database.Autosave = false;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            Database.autosaveTime = new TimeSpan(0, (int)numericUpDown3.Value, 0);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)numericUpDown2.Value;
            if (fingerAccountManager.fingerAccounts.ContainsKey(val))
            {
                var item = fingerAccountManager.fingerAccounts[val];
                if (comboBox3.Items.Contains(item.Name))
                {
                    comboBox3.SelectedItem = item.Name;
                }
                else
                {
                    comboBox3.Items.Add(item.Name);
                    comboBox3.SelectedItem = item.Name;
                }
            }
            else
            {
                comboBox3.SelectedItem = "";
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int val = (int)numericUpDown2.Value;
            if (fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).Any(comboBox3.SelectedItem.Equals))
            {
                var item = fingerAccountManager.fingerAccounts.Values.Where(n => n.Name == (string)comboBox3.SelectedItem).First();
                //numericUpDown2.Enabled = false;
                numericUpDown2.Value = fingerAccountManager.fingerAccounts.Values.Select(n => n.templateId).First(n => n == item.templateId);
            }
            else
            {
                //numericUpDown2.Enabled = true;
            }
        }

        //Delete button
        private void button3_Click(object sender, EventArgs e)
        {
            //Delete fingerprint
            if (checkBox5.Checked || checkBox4.Checked)
            {
                if (checkBox4.Checked)
                {
                    int id = (int)numericUpDown2.Value;
                    if (fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).Any(comboBox3.SelectedItem.Equals))
                    {
                        var item = fingerAccountManager.fingerAccounts.Values.First(n => n.Name == (string)comboBox3.SelectedItem);
                        numericUpDown2.Value = item.templateId;
                        fingerAccountManager.fingerAccounts.Remove(id);
                        FingerInterfacer.INTERFACEDelete(ArduinoCom, id);
                        AppendTextBox($"Deleted fingerprint {id} and account {item.Name} ({item.templateId})\n");
                    }
                    else
                    {
                        FingerInterfacer.INTERFACEDelete(ArduinoCom, id);
                        AppendTextBox($"Deleted fingerprint with Id {id}\n");
                    }
                }
                else
                {
                    if (fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).Any(comboBox3.SelectedItem.Equals))
                    {
                        var item = fingerAccountManager.fingerAccounts.Values.First(n => n.Name == (string)comboBox3.SelectedItem);
                        numericUpDown2.Value = item.templateId;
                        fingerAccountManager.fingerAccounts.Remove(item.templateId);
                        //FingerInterfacer.INTERFACEDelete(ArduinoCom, item.templateId);
                        AppendTextBox($"Deleted account {item.Name} ({item.templateId})\n");
                    }
                }
                RefreshAll();
                return;
            }
            ////Delete account
            //if (checkBox5.Checked)
            //{
            //    if (fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).Any(comboBox3.SelectedItem.Equals))
            //    {
            //        var item = fingerAccountManager.fingerAccounts.Values.Where(n => n.Name == (string)comboBox3.SelectedItem).First();
            //        //numericUpDown2.Enabled = false;
            //        numericUpDown2.Value = item.templateId;
                    
            //            fingerAccountManager.fingerAccounts.Remove(item.templateId);
            //        comboBox3.Items.Remove(item.Name);
            //            comboBox3.SelectedItem = "";
                   
            //    }
            //    else
            //    {

            //    }
            //}
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            HttpServer.HostName = textBox3.Text;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            HttpServer.Port = (ushort)numericUpDown4.Value;
        }

        //Selected user changed in the user viewer
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string currentSession = "";
            string totalTime = "";
            string loggedIn = "";
            string templateId = "";
            if (fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).Any(comboBox4.SelectedItem.Equals))
            {
                var item = fingerAccountManager.fingerAccounts.Values.Where(n => n.Name == (string)comboBox4.SelectedItem).First();
                currentSession = item.CurrentTime.ToString("dd':'hh':'mm");
                totalTime = item.TotalTime.ToString("dd':'hh':'mm");
                if (item.loggedIn)
                {
                    loggedIn = "In";
                }
                else
                {
                    loggedIn = "Out";
                }
                templateId = item.templateId.ToString();
            }
            textBox8.Text = currentSession;
            textBox7.Text = totalTime;
            textBox6.Text = templateId;
            textBox5.Text = loggedIn;
        }

        //Apply changes
        private void button5_Click(object sender, EventArgs e)
        {
            if (fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).Any(comboBox4.SelectedItem.Equals))
            {
                var item = fingerAccountManager.fingerAccounts.Values.Where(n => n.Name == (string)comboBox4.SelectedItem).First();
                if (checkBox7.Checked != item.loggedIn)
                {
                    //item.loggedIn = checkBox7.Checked;
                    item.ToggleLogINOUT();
                    RefreshLogState();
                }
            }
        }

        //Log all in
        private void button7_Click(object sender, EventArgs e)
        {
            fingerAccountManager.fingerAccounts.Values.ToList().ForEach(n => n.LogIn());
            RefreshLogState();
        }

        private void RefreshLogState()
        {
            richTextBox3.Clear();
            richTextBox2.Clear();
            foreach (var fin in fingerAccountManager.fingerAccounts.Values.Where(n => n.loggedIn))
                richTextBox3.AppendText(fin.Name + "\n");
            foreach (var fin in fingerAccountManager.fingerAccounts.Values.Where(n => !(n.loggedIn)))
                richTextBox2.AppendText(fin.Name + "\n");
        }

        private void RefreshAll()
        {
            comboBox4.ResetText();
            comboBox3.ResetText();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            comboBox3.Items.AddRange(fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).ToArray());
            comboBox4.Items.AddRange(fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).ToArray());
            foreach (var fin in fingerAccountManager.fingerAccounts.Values.Where(n => n.loggedIn))
                richTextBox3.AppendText(fin.Name + "\n");
            foreach (var fin in fingerAccountManager.fingerAccounts.Values.Where(n => !(n.loggedIn)))
                richTextBox2.AppendText(fin.Name + "\n");
            comboBox3.Items.Add("");
            comboBox3.SelectedItem = "";            
            
            //Change value of the delete thing
            int val = (int)numericUpDown2.Value;
            if (fingerAccountManager.fingerAccounts.ContainsKey(val))
            {
                var item = fingerAccountManager.fingerAccounts[val];
                if (comboBox3.Items.Contains(item.Name))
                {
                    comboBox3.SelectedItem = item.Name;
                }
                else
                {
                    comboBox3.Items.Add(item.Name);
                    comboBox3.SelectedItem = item.Name;
                }
            }
            else
            {
                comboBox3.SelectedItem = "";
            }
        }

        //Log all out
        private void button6_Click(object sender, EventArgs e)
        {
            fingerAccountManager.fingerAccounts.Values.ToList().ForEach(n => n.LogOut());
            RefreshLogState();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                //checkBox5.Checked = true;
                checkBox5.Enabled = false;
            }
            else
            {
                checkBox5.Enabled = true;
            }
        }
    }
}
