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
            comboBox1.Items.AddRange(new string[] {"9600","57600" });
            if (comboBox2.Items.Count > 0)
            comboBox2.SelectedItem = comboBox2.Items[0];
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedItem = comboBox1.Items[0];
            textBox2.Text = "Inactive";
            textBox4.Text = "Inactive";
            Database.autosaveTime = new TimeSpan(0, (int)numericUpDown3.Value, 0);
            comboBox3.Items.AddRange(fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).ToArray());
            comboBox3.Items.Add("");
            comboBox3.SelectedItem = "";
            numericUpDown3.Enabled = false;
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Enabled = false;
                if (HttpServer.StartWebServer().GetAwaiter().GetResult())
                {
                    checkBox1.Checked = false;
                }
                checkBox1.Enabled = true;
            }
            else
            {
                checkBox1.Enabled = false;
                HttpServer.StopWebServer().Wait();
                checkBox1.Enabled = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

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
                fingerAccountManager.fingerAccounts[tempnum] = new FingerAccount(textBox1.Text, tempnum);
                comboBox3.Items.Clear();
                comboBox3.Items.AddRange(fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).ToArray());
                comboBox3.Items.Add("");
            }
            else
            {
                fingerAccountManager.fingerAccounts.Add(tempnum, new FingerAccount(textBox1.Text, tempnum));
                comboBox3.Items.Clear();
                comboBox3.Items.AddRange(fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).ToArray());
                comboBox3.Items.Add("");
            }
        }

        //Name text box
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //Add user up down
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Info_Popup(object sender, PopupEventArgs e)
        {

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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

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
        //Delete account
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }
        //Delete fingerprint
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Delete fingerprint
            if (checkBox4.Checked)
            {
                if (fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).Any(comboBox3.SelectedItem.Equals))
                {
                    var item = fingerAccountManager.fingerAccounts.Values.Where(n => n.Name == (string)comboBox3.SelectedItem).First();
                    //numericUpDown2.Enabled = false;
                    numericUpDown2.Value = item.templateId;

                    //Insert deleter
                    FingerInterfacer.INTERFACEDelete(ArduinoCom, (int)numericUpDown2.Value);

                    if (checkBox5.Checked)
                    {
                        fingerAccountManager.fingerAccounts.Remove(item.templateId);
                        comboBox3.SelectedItem = "";
                    }
                }
                else
                {

                }
                return;
            }
            //Delete account
            if (checkBox5.Checked)
            {
                if (fingerAccountManager.fingerAccounts.Values.Select(n => n.Name).Any(comboBox3.SelectedItem.Equals))
                {
                    var item = fingerAccountManager.fingerAccounts.Values.Where(n => n.Name == (string)comboBox3.SelectedItem).First();
                    //numericUpDown2.Enabled = false;
                    numericUpDown2.Value = item.templateId;
                    
                        fingerAccountManager.fingerAccounts.Remove(item.templateId);
                    comboBox3.Items.Remove(item.Name);
                        comboBox3.SelectedItem = "";
                   
                }
                else
                {

                }
            }
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
    }
}
