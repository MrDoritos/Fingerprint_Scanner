namespace FingerprintScanner
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.Info = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(125, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Enable HTTP Server";
            this.Info.SetToolTip(this.checkBox1, "Enable the frontend web client");
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(15, 31);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(125, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Enable Arduino COM";
            this.Info.SetToolTip(this.checkBox2, "Enable serial communications with an Arduino");
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(166, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "COM Port";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(163, 155);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Clear Serial Buffer";
            this.Info.SetToolTip(this.button1, "May never need to use this with my programming :)");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(163, 309);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(123, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Add";
            this.Info.SetToolTip(this.button2, "Add an account");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(231, 284);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(55, 20);
            this.numericUpDown1.TabIndex = 6;
            this.Info.SetToolTip(this.numericUpDown1, "The template ID of a certain fingerprint or new account");
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 286);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Template ID";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(163, 211);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(126, 17);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.Text = "Add a new fingerprint";
            this.Info.SetToolTip(this.checkBox3, "Take a new scan to add to the fingerprint library");
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(163, 257);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(123, 20);
            this.textBox1.TabIndex = 9;
            this.Info.SetToolTip(this.textBox1, "Name of the account");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(160, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Name (Last, First)";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 309);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(123, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Delete";
            this.Info.SetToolTip(this.button3, "Delete the account or fingerprint");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(15, 210);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(109, 17);
            this.checkBox4.TabIndex = 12;
            this.checkBox4.Text = "Delete Fingerprint";
            this.Info.SetToolTip(this.checkBox4, "Delete the fingerprint from the image library");
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(15, 233);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(100, 17);
            this.checkBox5.TabIndex = 13;
            this.checkBox5.Text = "Delete Account";
            this.Info.SetToolTip(this.checkBox5, "Delete the account from the account library");
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(80, 284);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(58, 20);
            this.numericUpDown2.TabIndex = 14;
            this.Info.SetToolTip(this.numericUpDown2, "The template ID of a certain fingerprint or account");
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 286);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Template ID";
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(15, 54);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(120, 17);
            this.checkBox6.TabIndex = 17;
            this.checkBox6.Text = "Autosave Database";
            this.Info.SetToolTip(this.checkBox6, "Autosave the database");
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(15, 75);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(42, 23);
            this.button4.TabIndex = 18;
            this.button4.Text = "Save";
            this.Info.SetToolTip(this.button4, "Save the database.");
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(61, 77);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(77, 20);
            this.numericUpDown3.TabIndex = 19;
            this.Info.SetToolTip(this.numericUpDown3, "Time in minutes between autosaves.");
            this.numericUpDown3.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(166, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Baud Rate";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(163, 128);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(123, 21);
            this.comboBox1.TabIndex = 21;
            this.Info.SetToolTip(this.comboBox1, "Baud rate for Arduino communication");
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(163, 88);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(123, 21);
            this.comboBox2.TabIndex = 22;
            this.Info.SetToolTip(this.comboBox2, "Serial port used for communication to the Arduino");
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(163, 184);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(123, 20);
            this.textBox2.TabIndex = 23;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Location = new System.Drawing.Point(15, 106);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox1.Size = new System.Drawing.Size(123, 98);
            this.richTextBox1.TabIndex = 25;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(15, 256);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(123, 21);
            this.comboBox3.TabIndex = 24;
            this.Info.SetToolTip(this.comboBox3, "Account to delete, or alternatively the template ID");
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(163, 25);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(62, 20);
            this.textBox3.TabIndex = 26;
            this.textBox3.Text = "localhost";
            this.Info.SetToolTip(this.textBox3, "Hostname of the TCP HTTP listening socket");
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(231, 25);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            65565,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(55, 20);
            this.numericUpDown4.TabIndex = 27;
            this.Info.SetToolTip(this.numericUpDown4, "Port number of the listening socket");
            this.numericUpDown4.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(228, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "TCP Port";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(160, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Hostname";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(163, 49);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(123, 20);
            this.textBox4.TabIndex = 30;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(309, 24);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(122, 21);
            this.comboBox4.TabIndex = 31;
            this.Info.SetToolTip(this.comboBox4, "The user to modify");
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(309, 182);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(123, 23);
            this.button5.TabIndex = 32;
            this.button5.Text = "Apply";
            this.Info.SetToolTip(this.button5, "Apply new settings");
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(309, 51);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(74, 17);
            this.checkBox7.TabIndex = 35;
            this.checkBox7.Text = "Logged In";
            this.Info.SetToolTip(this.checkBox7, "Checked if you want to log the user in");
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(305, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "User";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(373, 153);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(58, 20);
            this.textBox6.TabIndex = 38;
            this.Info.SetToolTip(this.textBox6, "Template ID");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(306, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 39;
            this.label9.Text = "Template ID";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(373, 127);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(58, 20);
            this.textBox5.TabIndex = 40;
            this.Info.SetToolTip(this.textBox5, "Current login state");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(306, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 41;
            this.label10.Text = "Login State";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(373, 100);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(58, 20);
            this.textBox7.TabIndex = 42;
            this.Info.SetToolTip(this.textBox7, "Total time (DD:HH:SS)");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(306, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 43;
            this.label11.Text = "Total Time";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(373, 74);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(58, 20);
            this.textBox8.TabIndex = 44;
            this.Info.SetToolTip(this.textBox8, "Current session time (DD:HH:SS)");
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(306, 77);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 45;
            this.label12.Text = "Current Time";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(373, 250);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(73, 82);
            this.richTextBox2.TabIndex = 46;
            this.richTextBox2.Text = "";
            this.Info.SetToolTip(this.richTextBox2, "Currently logged out users");
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(294, 250);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(73, 82);
            this.richTextBox3.TabIndex = 47;
            this.richTextBox3.Text = "";
            this.Info.SetToolTip(this.richTextBox3, "Currently logged in users");
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(373, 210);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(73, 23);
            this.button6.TabIndex = 48;
            this.button6.Text = "Log All Out";
            this.Info.SetToolTip(this.button6, "Log all users out");
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(294, 210);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(73, 23);
            this.button7.TabIndex = 49;
            this.button7.Text = "Log All In";
            this.Info.SetToolTip(this.button7, "Log all users in");
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(291, 236);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 13);
            this.label13.TabIndex = 50;
            this.label13.Text = "Logged In";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(370, 236);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 13);
            this.label14.TabIndex = 51;
            this.label14.Text = "Logged Out";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(460, 344);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBox7);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(317, 383);
            this.Name = "Form1";
            this.Text = "Ian\'s Cool Software";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.ToolTip Info;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
    }
}

