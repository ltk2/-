namespace ModuleReaderManager
{
    partial class Form_tagtemperled
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbstartaddr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbbopbank = new System.Windows.Forms.ComboBox();
            this.tbblocks = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnread = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_mulselect = new System.Windows.Forms.Button();
            this.textBox_ledlightt = new System.Windows.Forms.TextBox();
            this.checkBox_ledtime = new System.Windows.Forms.CheckBox();
            this.textBox_readwait = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbasypro = new System.Windows.Forms.CheckBox();
            this.cbasyemd = new System.Windows.Forms.CheckBox();
            this.cbasyrfu = new System.Windows.Forms.CheckBox();
            this.cbasytm = new System.Windows.Forms.CheckBox();
            this.cbasyfre = new System.Windows.Forms.CheckBox();
            this.cbasyantid = new System.Windows.Forms.CheckBox();
            this.cbasyrssi = new System.Windows.Forms.CheckBox();
            this.cbasyreadcnt = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_selwait = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_optime = new System.Windows.Forms.TextBox();
            this.cbb16opant = new System.Windows.Forms.ComboBox();
            this.lab16devtip = new System.Windows.Forms.Label();
            this.cbbfilterrule = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbfilteraddr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbfilterbank = new System.Windows.Forms.ComboBox();
            this.rbant4 = new System.Windows.Forms.RadioButton();
            this.rbant3 = new System.Windows.Forms.RadioButton();
            this.cbisaccesspasswd = new System.Windows.Forms.CheckBox();
            this.rbant2 = new System.Windows.Forms.RadioButton();
            this.cbisfilter = new System.Windows.Forms.CheckBox();
            this.rbant1 = new System.Windows.Forms.RadioButton();
            this.tbkillpasswd = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbfldata = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbaccesspasswd = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_ledtime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_readled = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1_status = new System.Windows.Forms.Label();
            this.lvTags = new ModuleReaderManager.DoubleBufferListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbstartaddr);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbbopbank);
            this.groupBox2.Controls.Add(this.tbblocks);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnread);
            this.groupBox2.Location = new System.Drawing.Point(12, 254);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(556, 95);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标签温度";
            // 
            // tbstartaddr
            // 
            this.tbstartaddr.Location = new System.Drawing.Point(97, 22);
            this.tbstartaddr.Name = "tbstartaddr";
            this.tbstartaddr.Size = new System.Drawing.Size(44, 21);
            this.tbstartaddr.TabIndex = 9;
            this.tbstartaddr.Text = "7F";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(293, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "bank";
            // 
            // cbbopbank
            // 
            this.cbbopbank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbopbank.FormattingEnabled = true;
            this.cbbopbank.Items.AddRange(new object[] {
            "ReserveBank",
            "EPCBank",
            "TIDBank",
            "USERBank"});
            this.cbbopbank.Location = new System.Drawing.Point(328, 23);
            this.cbbopbank.Name = "cbbopbank";
            this.cbbopbank.Size = new System.Drawing.Size(87, 20);
            this.cbbopbank.TabIndex = 1;
            // 
            // tbblocks
            // 
            this.tbblocks.Location = new System.Drawing.Point(248, 22);
            this.tbblocks.Name = "tbblocks";
            this.tbblocks.Size = new System.Drawing.Size(39, 21);
            this.tbblocks.TabIndex = 11;
            this.tbblocks.Text = "1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(201, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "读块数";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "起始地址(HEX)";
            // 
            // btnread
            // 
            this.btnread.Location = new System.Drawing.Point(17, 49);
            this.btnread.Name = "btnread";
            this.btnread.Size = new System.Drawing.Size(493, 36);
            this.btnread.TabIndex = 7;
            this.btnread.Text = "读";
            this.btnread.UseVisualStyleBackColor = true;
            this.btnread.Click += new System.EventHandler(this.btnread_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_mulselect);
            this.groupBox1.Controls.Add(this.textBox_ledlightt);
            this.groupBox1.Controls.Add(this.checkBox_ledtime);
            this.groupBox1.Controls.Add(this.textBox_readwait);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.textBox_selwait);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBox_optime);
            this.groupBox1.Controls.Add(this.cbb16opant);
            this.groupBox1.Controls.Add(this.lab16devtip);
            this.groupBox1.Controls.Add(this.cbbfilterrule);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.tbfilteraddr);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbbfilterbank);
            this.groupBox1.Controls.Add(this.rbant4);
            this.groupBox1.Controls.Add(this.rbant3);
            this.groupBox1.Controls.Add(this.cbisaccesspasswd);
            this.groupBox1.Controls.Add(this.rbant2);
            this.groupBox1.Controls.Add(this.cbisfilter);
            this.groupBox1.Controls.Add(this.rbant1);
            this.groupBox1.Controls.Add(this.tbkillpasswd);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbfldata);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbaccesspasswd);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(559, 243);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作参数配置";
            // 
            // button_mulselect
            // 
            this.button_mulselect.Location = new System.Drawing.Point(438, 213);
            this.button_mulselect.Name = "button_mulselect";
            this.button_mulselect.Size = new System.Drawing.Size(100, 23);
            this.button_mulselect.TabIndex = 37;
            this.button_mulselect.Text = "多标签过滤......";
            this.button_mulselect.UseVisualStyleBackColor = true;
            this.button_mulselect.Click += new System.EventHandler(this.button_mulselect_Click);
            // 
            // textBox_ledlightt
            // 
            this.textBox_ledlightt.Location = new System.Drawing.Point(438, 185);
            this.textBox_ledlightt.Name = "textBox_ledlightt";
            this.textBox_ledlightt.Size = new System.Drawing.Size(100, 21);
            this.textBox_ledlightt.TabIndex = 36;
            this.textBox_ledlightt.Text = "3000";
            // 
            // checkBox_ledtime
            // 
            this.checkBox_ledtime.AutoSize = true;
            this.checkBox_ledtime.Location = new System.Drawing.Point(438, 163);
            this.checkBox_ledtime.Name = "checkBox_ledtime";
            this.checkBox_ledtime.Size = new System.Drawing.Size(108, 16);
            this.checkBox_ledtime.TabIndex = 35;
            this.checkBox_ledtime.Text = "亮灯时间(毫秒)";
            this.checkBox_ledtime.UseVisualStyleBackColor = true;
            // 
            // textBox_readwait
            // 
            this.textBox_readwait.Location = new System.Drawing.Point(471, 134);
            this.textBox_readwait.Name = "textBox_readwait";
            this.textBox_readwait.Size = new System.Drawing.Size(79, 21);
            this.textBox_readwait.TabIndex = 34;
            this.textBox_readwait.Text = "100";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbasypro);
            this.groupBox5.Controls.Add(this.cbasyemd);
            this.groupBox5.Controls.Add(this.cbasyrfu);
            this.groupBox5.Controls.Add(this.cbasytm);
            this.groupBox5.Controls.Add(this.cbasyfre);
            this.groupBox5.Controls.Add(this.cbasyantid);
            this.groupBox5.Controls.Add(this.cbasyrssi);
            this.groupBox5.Controls.Add(this.cbasyreadcnt);
            this.groupBox5.Location = new System.Drawing.Point(16, 160);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(405, 67);
            this.groupBox5.TabIndex = 34;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "读取标签元数据选项";
            // 
            // cbasypro
            // 
            this.cbasypro.AutoSize = true;
            this.cbasypro.Location = new System.Drawing.Point(187, 42);
            this.cbasypro.Name = "cbasypro";
            this.cbasypro.Size = new System.Drawing.Size(48, 16);
            this.cbasypro.TabIndex = 7;
            this.cbasypro.Text = "协议";
            this.cbasypro.UseVisualStyleBackColor = true;
            // 
            // cbasyemd
            // 
            this.cbasyemd.AutoSize = true;
            this.cbasyemd.Location = new System.Drawing.Point(258, 42);
            this.cbasyemd.Name = "cbasyemd";
            this.cbasyemd.Size = new System.Drawing.Size(72, 16);
            this.cbasyemd.TabIndex = 6;
            this.cbasyemd.Text = "附加数据";
            this.cbasyemd.UseVisualStyleBackColor = true;
            // 
            // cbasyrfu
            // 
            this.cbasyrfu.AutoSize = true;
            this.cbasyrfu.Location = new System.Drawing.Point(101, 42);
            this.cbasyrfu.Name = "cbasyrfu";
            this.cbasyrfu.Size = new System.Drawing.Size(72, 16);
            this.cbasyrfu.TabIndex = 5;
            this.cbasyrfu.Text = "保留字段";
            this.cbasyrfu.UseVisualStyleBackColor = true;
            // 
            // cbasytm
            // 
            this.cbasytm.AutoSize = true;
            this.cbasytm.Location = new System.Drawing.Point(10, 41);
            this.cbasytm.Name = "cbasytm";
            this.cbasytm.Size = new System.Drawing.Size(60, 16);
            this.cbasytm.TabIndex = 4;
            this.cbasytm.Text = "时间戳";
            this.cbasytm.UseVisualStyleBackColor = true;
            // 
            // cbasyfre
            // 
            this.cbasyfre.AutoSize = true;
            this.cbasyfre.Location = new System.Drawing.Point(258, 19);
            this.cbasyfre.Name = "cbasyfre";
            this.cbasyfre.Size = new System.Drawing.Size(48, 16);
            this.cbasyfre.TabIndex = 3;
            this.cbasyfre.Text = "频率";
            this.cbasyfre.UseVisualStyleBackColor = true;
            // 
            // cbasyantid
            // 
            this.cbasyantid.AutoSize = true;
            this.cbasyantid.Location = new System.Drawing.Point(185, 19);
            this.cbasyantid.Name = "cbasyantid";
            this.cbasyantid.Size = new System.Drawing.Size(60, 16);
            this.cbasyantid.TabIndex = 2;
            this.cbasyantid.Text = "天线ID";
            this.cbasyantid.UseVisualStyleBackColor = true;
            // 
            // cbasyrssi
            // 
            this.cbasyrssi.AutoSize = true;
            this.cbasyrssi.Location = new System.Drawing.Point(101, 20);
            this.cbasyrssi.Name = "cbasyrssi";
            this.cbasyrssi.Size = new System.Drawing.Size(48, 16);
            this.cbasyrssi.TabIndex = 1;
            this.cbasyrssi.Text = "RSSI";
            this.cbasyrssi.UseVisualStyleBackColor = true;
            // 
            // cbasyreadcnt
            // 
            this.cbasyreadcnt.AutoSize = true;
            this.cbasyreadcnt.Location = new System.Drawing.Point(10, 19);
            this.cbasyreadcnt.Name = "cbasyreadcnt";
            this.cbasyreadcnt.Size = new System.Drawing.Size(60, 16);
            this.cbasyreadcnt.TabIndex = 0;
            this.cbasyreadcnt.Text = "读次数";
            this.cbasyreadcnt.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(341, 137);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(137, 12);
            this.label13.TabIndex = 33;
            this.label13.Text = "TIMEREADWAIT（0－300）";
            // 
            // textBox_selwait
            // 
            this.textBox_selwait.Location = new System.Drawing.Point(471, 102);
            this.textBox_selwait.Name = "textBox_selwait";
            this.textBox_selwait.Size = new System.Drawing.Size(79, 21);
            this.textBox_selwait.TabIndex = 32;
            this.textBox_selwait.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(343, 108);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(131, 12);
            this.label10.TabIndex = 31;
            this.label10.Text = "TIMESELWAIT（0－300）";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(343, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 29;
            this.label11.Text = "读时长";
            // 
            // textBox_optime
            // 
            this.textBox_optime.Location = new System.Drawing.Point(471, 75);
            this.textBox_optime.Name = "textBox_optime";
            this.textBox_optime.Size = new System.Drawing.Size(79, 21);
            this.textBox_optime.TabIndex = 30;
            this.textBox_optime.Text = "1000";
            // 
            // cbb16opant
            // 
            this.cbb16opant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb16opant.FormattingEnabled = true;
            this.cbb16opant.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.cbb16opant.Location = new System.Drawing.Point(175, 124);
            this.cbb16opant.Name = "cbb16opant";
            this.cbb16opant.Size = new System.Drawing.Size(49, 20);
            this.cbb16opant.TabIndex = 28;
            this.cbb16opant.Visible = false;
            // 
            // lab16devtip
            // 
            this.lab16devtip.AutoSize = true;
            this.lab16devtip.Location = new System.Drawing.Point(95, 128);
            this.lab16devtip.Name = "lab16devtip";
            this.lab16devtip.Size = new System.Drawing.Size(77, 12);
            this.lab16devtip.TabIndex = 27;
            this.lab16devtip.Text = "8/16天线设备";
            this.lab16devtip.Visible = false;
            // 
            // cbbfilterrule
            // 
            this.cbbfilterrule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbfilterrule.FormattingEnabled = true;
            this.cbbfilterrule.Items.AddRange(new object[] {
            "匹配",
            "不匹配"});
            this.cbbfilterrule.Location = new System.Drawing.Point(230, 47);
            this.cbbfilterrule.Name = "cbbfilterrule";
            this.cbbfilterrule.Size = new System.Drawing.Size(79, 20);
            this.cbbfilterrule.TabIndex = 26;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(171, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 25;
            this.label12.Text = "匹配规则";
            // 
            // tbfilteraddr
            // 
            this.tbfilteraddr.Location = new System.Drawing.Point(80, 46);
            this.tbfilteraddr.Name = "tbfilteraddr";
            this.tbfilteraddr.Size = new System.Drawing.Size(79, 21);
            this.tbfilteraddr.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "起始地址";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "过滤bank";
            // 
            // cbbfilterbank
            // 
            this.cbbfilterbank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbfilterbank.FormattingEnabled = true;
            this.cbbfilterbank.Items.AddRange(new object[] {
            "EPCbank",
            "TIDbank",
            "USERbank"});
            this.cbbfilterbank.Location = new System.Drawing.Point(375, 46);
            this.cbbfilterbank.Name = "cbbfilterbank";
            this.cbbfilterbank.Size = new System.Drawing.Size(79, 20);
            this.cbbfilterbank.TabIndex = 21;
            // 
            // rbant4
            // 
            this.rbant4.AutoSize = true;
            this.rbant4.Enabled = false;
            this.rbant4.Location = new System.Drawing.Point(16, 127);
            this.rbant4.Name = "rbant4";
            this.rbant4.Size = new System.Drawing.Size(59, 16);
            this.rbant4.TabIndex = 20;
            this.rbant4.TabStop = true;
            this.rbant4.Text = "天线４";
            this.rbant4.UseVisualStyleBackColor = true;
            // 
            // rbant3
            // 
            this.rbant3.AutoSize = true;
            this.rbant3.Enabled = false;
            this.rbant3.Location = new System.Drawing.Point(175, 104);
            this.rbant3.Name = "rbant3";
            this.rbant3.Size = new System.Drawing.Size(59, 16);
            this.rbant3.TabIndex = 19;
            this.rbant3.TabStop = true;
            this.rbant3.Text = "天线３";
            this.rbant3.UseVisualStyleBackColor = true;
            // 
            // cbisaccesspasswd
            // 
            this.cbisaccesspasswd.AutoSize = true;
            this.cbisaccesspasswd.Location = new System.Drawing.Point(260, 104);
            this.cbisaccesspasswd.Name = "cbisaccesspasswd";
            this.cbisaccesspasswd.Size = new System.Drawing.Size(72, 16);
            this.cbisaccesspasswd.TabIndex = 16;
            this.cbisaccesspasswd.Text = "访问密码";
            this.cbisaccesspasswd.UseVisualStyleBackColor = true;
            // 
            // rbant2
            // 
            this.rbant2.AutoSize = true;
            this.rbant2.Location = new System.Drawing.Point(98, 104);
            this.rbant2.Name = "rbant2";
            this.rbant2.Size = new System.Drawing.Size(59, 16);
            this.rbant2.TabIndex = 18;
            this.rbant2.TabStop = true;
            this.rbant2.Text = "天线２";
            this.rbant2.UseVisualStyleBackColor = true;
            // 
            // cbisfilter
            // 
            this.cbisfilter.AutoSize = true;
            this.cbisfilter.Location = new System.Drawing.Point(260, 127);
            this.cbisfilter.Name = "cbisfilter";
            this.cbisfilter.Size = new System.Drawing.Size(72, 16);
            this.cbisfilter.TabIndex = 15;
            this.cbisfilter.Text = "数据过滤";
            this.cbisfilter.UseVisualStyleBackColor = true;
            // 
            // rbant1
            // 
            this.rbant1.AutoSize = true;
            this.rbant1.Location = new System.Drawing.Point(16, 104);
            this.rbant1.Name = "rbant1";
            this.rbant1.Size = new System.Drawing.Size(59, 16);
            this.rbant1.TabIndex = 17;
            this.rbant1.TabStop = true;
            this.rbant1.Text = "天线１";
            this.rbant1.UseVisualStyleBackColor = true;
            // 
            // tbkillpasswd
            // 
            this.tbkillpasswd.Location = new System.Drawing.Point(232, 75);
            this.tbkillpasswd.Name = "tbkillpasswd";
            this.tbkillpasswd.Size = new System.Drawing.Size(79, 21);
            this.tbkillpasswd.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(173, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "销毁密码";
            // 
            // tbfldata
            // 
            this.tbfldata.Location = new System.Drawing.Point(80, 18);
            this.tbfldata.Name = "tbfldata";
            this.tbfldata.Size = new System.Drawing.Size(476, 21);
            this.tbfldata.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "过滤数据";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "访问密码";
            // 
            // tbaccesspasswd
            // 
            this.tbaccesspasswd.Location = new System.Drawing.Point(80, 75);
            this.tbaccesspasswd.Name = "tbaccesspasswd";
            this.tbaccesspasswd.Size = new System.Drawing.Size(79, 21);
            this.tbaccesspasswd.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox_ledtime);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.button_readled);
            this.groupBox3.Location = new System.Drawing.Point(12, 355);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(556, 70);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "标签LED";
            // 
            // textBox_ledtime
            // 
            this.textBox_ledtime.Location = new System.Drawing.Point(498, 43);
            this.textBox_ledtime.Name = "textBox_ledtime";
            this.textBox_ledtime.Size = new System.Drawing.Size(51, 21);
            this.textBox_ledtime.TabIndex = 14;
            this.textBox_ledtime.Text = "1000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(496, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "间隔毫秒";
            // 
            // button_readled
            // 
            this.button_readled.Location = new System.Drawing.Point(16, 20);
            this.button_readled.Name = "button_readled";
            this.button_readled.Size = new System.Drawing.Size(462, 36);
            this.button_readled.TabIndex = 12;
            this.button_readled.Text = "读";
            this.button_readled.UseVisualStyleBackColor = true;
            this.button_readled.Click += new System.EventHandler(this.button_readled_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(105, 26);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.clearToolStripMenuItem.Text = "clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1_status
            // 
            this.label1_status.AutoSize = true;
            this.label1_status.Location = new System.Drawing.Point(26, 436);
            this.label1_status.Name = "label1_status";
            this.label1_status.Size = new System.Drawing.Size(23, 12);
            this.label1_status.TabIndex = 20;
            this.label1_status.Text = "...";
            // 
            // lvTags
            // 
            this.lvTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.lvTags.ContextMenuStrip = this.contextMenuStrip1;
            this.lvTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lvTags.FullRowSelect = true;
            this.lvTags.GridLines = true;
            this.lvTags.Location = new System.Drawing.Point(591, 12);
            this.lvTags.Name = "lvTags";
            this.lvTags.Size = new System.Drawing.Size(448, 437);
            this.lvTags.TabIndex = 19;
            this.lvTags.UseCompatibleStateImageBehavior = false;
            this.lvTags.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "EPC ID";
            this.columnHeader3.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "读次数";
            this.columnHeader2.Width = 65;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "天线";
            this.columnHeader4.Width = 51;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "附加数据";
            this.columnHeader5.Width = 116;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "协议";
            this.columnHeader6.Width = 66;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "RSSI";
            this.columnHeader7.Width = 75;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "频率";
            this.columnHeader8.Width = 50;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "相位";
            this.columnHeader9.Width = 53;
            // 
            // Form_tagtemperled
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 457);
            this.Controls.Add(this.label1_status);
            this.Controls.Add(this.lvTags);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_tagtemperled";
            this.Text = "标签温度LED";
            this.Load += new System.EventHandler(this.Form_tagtemperled_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbstartaddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbopbank;
        private System.Windows.Forms.TextBox tbblocks;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnread;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbb16opant;
        private System.Windows.Forms.Label lab16devtip;
        private System.Windows.Forms.ComboBox cbbfilterrule;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbfilteraddr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbbfilterbank;
        private System.Windows.Forms.RadioButton rbant4;
        private System.Windows.Forms.RadioButton rbant3;
        private System.Windows.Forms.CheckBox cbisaccesspasswd;
        private System.Windows.Forms.RadioButton rbant2;
        private System.Windows.Forms.CheckBox cbisfilter;
        private System.Windows.Forms.RadioButton rbant1;
        private System.Windows.Forms.TextBox tbkillpasswd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbfldata;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbaccesspasswd;
        private System.Windows.Forms.TextBox textBox_selwait;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_optime;
        private System.Windows.Forms.TextBox textBox_readwait;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cbasyemd;
        private System.Windows.Forms.CheckBox cbasyrfu;
        private System.Windows.Forms.CheckBox cbasytm;
        private System.Windows.Forms.CheckBox cbasyfre;
        private System.Windows.Forms.CheckBox cbasyantid;
        private System.Windows.Forms.CheckBox cbasyrssi;
        private System.Windows.Forms.CheckBox cbasyreadcnt;
        private System.Windows.Forms.CheckBox cbasypro;
        private System.Windows.Forms.Button button_readled;
        private DoubleBufferListView lvTags;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox_ledtime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1_status;
        private System.Windows.Forms.TextBox textBox_ledlightt;
        private System.Windows.Forms.CheckBox checkBox_ledtime;
        private System.Windows.Forms.Button button_mulselect;
    }
}