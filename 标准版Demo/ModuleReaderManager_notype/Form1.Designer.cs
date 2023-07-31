namespace ModuleReaderManager
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnconnect = new System.Windows.Forms.Button();
            this.btnstart = new System.Windows.Forms.Button();
            this.btnstop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BaudRate = new System.Windows.Forms.ComboBox();
            this.tbip = new System.Windows.Forms.ComboBox();
            this.btndisconnect = new System.Windows.Forms.Button();
            this.cbbreadertype = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnInvParas = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.readparamenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tagopmenu = new System.Windows.Forms.ToolStripMenuItem();
            this.gen2tagopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iso183k6btagopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CountTagMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemmultibankwrite = new System.Windows.Forms.ToolStripMenuItem();
            this.标签信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menutest = new System.Windows.Forms.ToolStripMenuItem();
            this.MsgDebugMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemBox = new System.Windows.Forms.ToolStripMenuItem();
            this.柜子应用1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.柜子应用2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标签温度及LEDToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.其它ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.驻波ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.天线区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自动发卡ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lBTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标签特殊指令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pSAMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ex10系列初始化ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemlog = new ModuleReaderManager.CustomToolStripMenuItem();
            this.updatemenu = new System.Windows.Forms.ToolStripMenuItem();
            this.btn16antset = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.拷贝当前TIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.过滤标签ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除过滤ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏显示附加数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏显示天线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏显示协议ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏显示RSSLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏显示频率ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏显示相位ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.labtoatotims = new System.Windows.Forms.Label();
            this.labtotalcnt = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labreadtime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox_stopsec = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pattern = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cbant_4 = new System.Windows.Forms.CheckBox();
            this.cbant_3 = new System.Windows.Forms.CheckBox();
            this.cbant_2 = new System.Windows.Forms.CheckBox();
            this.cbant_1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbisunibyant = new System.Windows.Forms.CheckBox();
            this.cbischgcolor = new System.Windows.Forms.CheckBox();
            this.cbisunibynullemd = new System.Windows.Forms.CheckBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.lvTags = new ModuleReaderManager.DoubleBufferListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnconnect
            // 
            this.btnconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnconnect.Location = new System.Drawing.Point(6, 87);
            this.btnconnect.Name = "btnconnect";
            this.btnconnect.Size = new System.Drawing.Size(64, 30);
            this.btnconnect.TabIndex = 4;
            this.btnconnect.Text = "连接";
            this.btnconnect.UseVisualStyleBackColor = true;
            this.btnconnect.Click += new System.EventHandler(this.btnconnect_Click);
            // 
            // btnstart
            // 
            this.btnstart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnstart.ForeColor = System.Drawing.Color.Black;
            this.btnstart.Location = new System.Drawing.Point(16, 378);
            this.btnstart.Name = "btnstart";
            this.btnstart.Size = new System.Drawing.Size(70, 30);
            this.btnstart.TabIndex = 7;
            this.btnstart.Text = "开始";
            this.btnstart.UseVisualStyleBackColor = true;
            this.btnstart.Click += new System.EventHandler(this.btnstart_Click);
            // 
            // btnstop
            // 
            this.btnstop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnstop.Location = new System.Drawing.Point(181, 378);
            this.btnstop.Name = "btnstop";
            this.btnstop.Size = new System.Drawing.Size(69, 30);
            this.btnstop.TabIndex = 8;
            this.btnstop.Text = "停止";
            this.btnstop.UseVisualStyleBackColor = true;
            this.btnstop.Click += new System.EventHandler(this.btnstop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "连接地址";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(181, 414);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 30);
            this.button1.TabIndex = 9;
            this.button1.Text = "清空";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 700;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 570);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1097, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.BaudRate);
            this.groupBox3.Controls.Add(this.tbip);
            this.groupBox3.Controls.Add(this.btndisconnect);
            this.groupBox3.Controls.Add(this.cbbreadertype);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnconnect);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 33);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(257, 129);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "连接地址设置";
            // 
            // BaudRate
            // 
            this.BaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BaudRate.FormattingEnabled = true;
            this.BaudRate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.BaudRate.Location = new System.Drawing.Point(212, 22);
            this.BaudRate.Name = "BaudRate";
            this.BaudRate.Size = new System.Drawing.Size(25, 25);
            this.BaudRate.TabIndex = 32;
            this.BaudRate.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            // 
            // tbip
            // 
            this.tbip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbip.FormattingEnabled = true;
            this.tbip.Location = new System.Drawing.Point(95, 20);
            this.tbip.Name = "tbip";
            this.tbip.Size = new System.Drawing.Size(106, 23);
            this.tbip.TabIndex = 31;
            this.tbip.DropDown += new System.EventHandler(this.tbip_DropDown);
            this.tbip.SelectedIndexChanged += new System.EventHandler(this.tbip_SelectedIndexChanged);
            // 
            // btndisconnect
            // 
            this.btndisconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btndisconnect.Location = new System.Drawing.Point(173, 89);
            this.btndisconnect.Name = "btndisconnect";
            this.btndisconnect.Size = new System.Drawing.Size(64, 30);
            this.btndisconnect.TabIndex = 16;
            this.btndisconnect.Text = "断开";
            this.btndisconnect.UseVisualStyleBackColor = true;
            this.btndisconnect.Click += new System.EventHandler(this.btndisconnect_Click);
            // 
            // cbbreadertype
            // 
            this.cbbreadertype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbreadertype.FormattingEnabled = true;
            this.cbbreadertype.Items.AddRange(new object[] {
            "单端口/发卡机/一体机设备",
            "双端口设备",
            "四端口设备",
            "八端口设备",
            "十六端口设备"});
            this.cbbreadertype.Location = new System.Drawing.Point(95, 58);
            this.cbbreadertype.Name = "cbbreadertype";
            this.cbbreadertype.Size = new System.Drawing.Size(142, 25);
            this.cbbreadertype.TabIndex = 14;
            this.cbbreadertype.DropDown += new System.EventHandler(this.cbbreadertype_DropDown);
            this.cbbreadertype.SelectedIndexChanged += new System.EventHandler(this.cbbreadertype_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "设备类型";
            // 
            // btnInvParas
            // 
            this.btnInvParas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInvParas.Location = new System.Drawing.Point(191, 41);
            this.btnInvParas.Name = "btnInvParas";
            this.btnInvParas.Size = new System.Drawing.Size(65, 59);
            this.btnInvParas.TabIndex = 17;
            this.btnInvParas.Text = "盘存参数";
            this.btnInvParas.UseVisualStyleBackColor = true;
            this.btnInvParas.Click += new System.EventHandler(this.btnInvParas_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readparamenu,
            this.tagopmenu,
            this.menutest,
            this.MsgDebugMenu,
            this.ToolStripMenuItemBox,
            this.标签温度及LEDToolStripMenuItem1,
            this.其它ToolStripMenuItem,
            this.menuitemlog,
            this.updatemenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1097, 28);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // readparamenu
            // 
            this.readparamenu.Name = "readparamenu";
            this.readparamenu.Size = new System.Drawing.Size(106, 24);
            this.readparamenu.Text = "读写器参数";
            this.readparamenu.Click += new System.EventHandler(this.readparamenu_Click);
            // 
            // tagopmenu
            // 
            this.tagopmenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gen2tagopMenuItem,
            this.iso183k6btagopToolStripMenuItem,
            this.CountTagMenuItem,
            this.menuitemmultibankwrite,
            this.标签信息ToolStripMenuItem});
            this.tagopmenu.Name = "tagopmenu";
            this.tagopmenu.Size = new System.Drawing.Size(89, 24);
            this.tagopmenu.Text = "标签操作";
            this.tagopmenu.Click += new System.EventHandler(this.tagopmenu_Click);
            // 
            // gen2tagopMenuItem
            // 
            this.gen2tagopMenuItem.Name = "gen2tagopMenuItem";
            this.gen2tagopMenuItem.Size = new System.Drawing.Size(218, 24);
            this.gen2tagopMenuItem.Text = "Gen2标签操作";
            this.gen2tagopMenuItem.Click += new System.EventHandler(this.gen2tagopMenuItem_Click);
            // 
            // iso183k6btagopToolStripMenuItem
            // 
            this.iso183k6btagopToolStripMenuItem.Name = "iso183k6btagopToolStripMenuItem";
            this.iso183k6btagopToolStripMenuItem.Size = new System.Drawing.Size(218, 24);
            this.iso183k6btagopToolStripMenuItem.Text = "180006B标签操作";
            this.iso183k6btagopToolStripMenuItem.Click += new System.EventHandler(this.iso183k6btagopToolStripMenuItem_Click);
            // 
            // CountTagMenuItem
            // 
            this.CountTagMenuItem.Name = "CountTagMenuItem";
            this.CountTagMenuItem.Size = new System.Drawing.Size(218, 24);
            this.CountTagMenuItem.Text = "标签点数";
            this.CountTagMenuItem.Click += new System.EventHandler(this.CountTagMenuItem_Click);
            // 
            // menuitemmultibankwrite
            // 
            this.menuitemmultibankwrite.Name = "menuitemmultibankwrite";
            this.menuitemmultibankwrite.Size = new System.Drawing.Size(218, 24);
            this.menuitemmultibankwrite.Text = "多bank连续写入";
            this.menuitemmultibankwrite.Click += new System.EventHandler(this.menuitemmultibankwrite_Click);
            // 
            // 标签信息ToolStripMenuItem
            // 
            this.标签信息ToolStripMenuItem.Name = "标签信息ToolStripMenuItem";
            this.标签信息ToolStripMenuItem.Size = new System.Drawing.Size(218, 24);
            this.标签信息ToolStripMenuItem.Text = "标签信息";
            this.标签信息ToolStripMenuItem.Click += new System.EventHandler(this.标签信息ToolStripMenuItem_Click);
            // 
            // menutest
            // 
            this.menutest.Name = "menutest";
            this.menutest.Size = new System.Drawing.Size(89, 24);
            this.menutest.Text = "射频测试";
            this.menutest.Click += new System.EventHandler(this.menutest_Click);
            // 
            // MsgDebugMenu
            // 
            this.MsgDebugMenu.Name = "MsgDebugMenu";
            this.MsgDebugMenu.Size = new System.Drawing.Size(89, 24);
            this.MsgDebugMenu.Text = "信令调试";
            this.MsgDebugMenu.Click += new System.EventHandler(this.MsgDebugMenu_Click);
            // 
            // ToolStripMenuItemBox
            // 
            this.ToolStripMenuItemBox.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.柜子应用1ToolStripMenuItem,
            this.柜子应用2ToolStripMenuItem});
            this.ToolStripMenuItemBox.Name = "ToolStripMenuItemBox";
            this.ToolStripMenuItemBox.Size = new System.Drawing.Size(89, 24);
            this.ToolStripMenuItemBox.Text = "柜子应用";
            // 
            // 柜子应用1ToolStripMenuItem
            // 
            this.柜子应用1ToolStripMenuItem.Name = "柜子应用1ToolStripMenuItem";
            this.柜子应用1ToolStripMenuItem.Size = new System.Drawing.Size(156, 24);
            this.柜子应用1ToolStripMenuItem.Text = "柜子应用1";
            this.柜子应用1ToolStripMenuItem.Click += new System.EventHandler(this.柜子应用1ToolStripMenuItem_Click);
            // 
            // 柜子应用2ToolStripMenuItem
            // 
            this.柜子应用2ToolStripMenuItem.Name = "柜子应用2ToolStripMenuItem";
            this.柜子应用2ToolStripMenuItem.Size = new System.Drawing.Size(156, 24);
            this.柜子应用2ToolStripMenuItem.Text = "柜子应用2";
            this.柜子应用2ToolStripMenuItem.Click += new System.EventHandler(this.柜子应用2ToolStripMenuItem_Click_1);
            // 
            // 标签温度及LEDToolStripMenuItem1
            // 
            this.标签温度及LEDToolStripMenuItem1.Name = "标签温度及LEDToolStripMenuItem1";
            this.标签温度及LEDToolStripMenuItem1.Size = new System.Drawing.Size(141, 24);
            this.标签温度及LEDToolStripMenuItem1.Text = "标签温度及LED";
            this.标签温度及LEDToolStripMenuItem1.Click += new System.EventHandler(this.标签温度及LEDToolStripMenuItem1_Click);
            // 
            // 其它ToolStripMenuItem
            // 
            this.其它ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.驻波ToolStripMenuItem,
            this.天线区域ToolStripMenuItem,
            this.自动发卡ToolStripMenuItem,
            this.lBTToolStripMenuItem,
            this.关于ToolStripMenuItem,
            this.标签特殊指令ToolStripMenuItem,
            this.pSAMToolStripMenuItem1,
            this.ex10系列初始化ToolStripMenuItem1});
            this.其它ToolStripMenuItem.Name = "其它ToolStripMenuItem";
            this.其它ToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
            this.其它ToolStripMenuItem.Text = "其它功能";
            // 
            // 驻波ToolStripMenuItem
            // 
            this.驻波ToolStripMenuItem.Name = "驻波ToolStripMenuItem";
            this.驻波ToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.驻波ToolStripMenuItem.Text = "驻波";
            this.驻波ToolStripMenuItem.Click += new System.EventHandler(this.驻波ToolStripMenuItem_Click);
            // 
            // 天线区域ToolStripMenuItem
            // 
            this.天线区域ToolStripMenuItem.Name = "天线区域ToolStripMenuItem";
            this.天线区域ToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.天线区域ToolStripMenuItem.Text = "天线区域";
            this.天线区域ToolStripMenuItem.Click += new System.EventHandler(this.天线区域ToolStripMenuItem_Click);
            // 
            // 自动发卡ToolStripMenuItem
            // 
            this.自动发卡ToolStripMenuItem.Name = "自动发卡ToolStripMenuItem";
            this.自动发卡ToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.自动发卡ToolStripMenuItem.Text = "自动发卡";
            this.自动发卡ToolStripMenuItem.Click += new System.EventHandler(this.自动发卡ToolStripMenuItem_Click);
            // 
            // lBTToolStripMenuItem
            // 
            this.lBTToolStripMenuItem.Name = "lBTToolStripMenuItem";
            this.lBTToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.lBTToolStripMenuItem.Text = "LBT";
            this.lBTToolStripMenuItem.Click += new System.EventHandler(this.lBTToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.关于ToolStripMenuItem.Text = "日志";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 标签特殊指令ToolStripMenuItem
            // 
            this.标签特殊指令ToolStripMenuItem.Name = "标签特殊指令ToolStripMenuItem";
            this.标签特殊指令ToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.标签特殊指令ToolStripMenuItem.Text = "标签特殊指令";
            this.标签特殊指令ToolStripMenuItem.Click += new System.EventHandler(this.标签特殊指令ToolStripMenuItem_Click);
            // 
            // pSAMToolStripMenuItem1
            // 
            this.pSAMToolStripMenuItem1.Name = "pSAMToolStripMenuItem1";
            this.pSAMToolStripMenuItem1.Size = new System.Drawing.Size(203, 24);
            this.pSAMToolStripMenuItem1.Text = "PSAM";
            this.pSAMToolStripMenuItem1.Click += new System.EventHandler(this.pSAMToolStripMenuItem1_Click);
            // 
            // ex10系列初始化ToolStripMenuItem1
            // 
            this.ex10系列初始化ToolStripMenuItem1.Name = "ex10系列初始化ToolStripMenuItem1";
            this.ex10系列初始化ToolStripMenuItem1.Size = new System.Drawing.Size(203, 24);
            this.ex10系列初始化ToolStripMenuItem1.Text = "Ex10系列初始化";
            this.ex10系列初始化ToolStripMenuItem1.Click += new System.EventHandler(this.ex10系列初始化ToolStripMenuItem1_Click);
            // 
            // menuitemlog
            // 
            this.menuitemlog.Name = "menuitemlog";
            this.menuitemlog.Size = new System.Drawing.Size(89, 24);
            this.menuitemlog.Text = "软件版本";
            this.menuitemlog.Click += new System.EventHandler(this.menuitemlog_Click);
            // 
            // updatemenu
            // 
            this.updatemenu.Name = "updatemenu";
            this.updatemenu.Size = new System.Drawing.Size(89, 24);
            this.updatemenu.Text = "固件更新";
            this.updatemenu.Click += new System.EventHandler(this.updatemenu_Click);
            // 
            // btn16antset
            // 
            this.btn16antset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn16antset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn16antset.Location = new System.Drawing.Point(167, 46);
            this.btn16antset.Name = "btn16antset";
            this.btn16antset.Size = new System.Drawing.Size(87, 27);
            this.btn16antset.TabIndex = 26;
            this.btn16antset.Text = "天线功率设置";
            this.btn16antset.UseVisualStyleBackColor = true;
            this.btn16antset.Click += new System.EventHandler(this.btn16antset_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.拷贝当前TIDToolStripMenuItem,
            this.过滤标签ToolStripMenuItem,
            this.清除过滤ToolStripMenuItem,
            this.隐藏显示附加数据ToolStripMenuItem,
            this.隐藏显示天线ToolStripMenuItem,
            this.隐藏显示协议ToolStripMenuItem,
            this.隐藏显示RSSLToolStripMenuItem,
            this.隐藏显示频率ToolStripMenuItem,
            this.隐藏显示相位ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(178, 224);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 22);
            this.toolStripMenuItem2.Text = "拷贝当前EPC";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // 拷贝当前TIDToolStripMenuItem
            // 
            this.拷贝当前TIDToolStripMenuItem.Name = "拷贝当前TIDToolStripMenuItem";
            this.拷贝当前TIDToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.拷贝当前TIDToolStripMenuItem.Text = "拷贝当前TID";
            this.拷贝当前TIDToolStripMenuItem.Click += new System.EventHandler(this.拷贝当前TIDToolStripMenuItem_Click);
            // 
            // 过滤标签ToolStripMenuItem
            // 
            this.过滤标签ToolStripMenuItem.Name = "过滤标签ToolStripMenuItem";
            this.过滤标签ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.过滤标签ToolStripMenuItem.Text = "过滤标签";
            this.过滤标签ToolStripMenuItem.Click += new System.EventHandler(this.过滤标签ToolStripMenuItem_Click);
            // 
            // 清除过滤ToolStripMenuItem
            // 
            this.清除过滤ToolStripMenuItem.Name = "清除过滤ToolStripMenuItem";
            this.清除过滤ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.清除过滤ToolStripMenuItem.Text = "清除过滤";
            this.清除过滤ToolStripMenuItem.Click += new System.EventHandler(this.清除过滤ToolStripMenuItem_Click);
            // 
            // 隐藏显示附加数据ToolStripMenuItem
            // 
            this.隐藏显示附加数据ToolStripMenuItem.Name = "隐藏显示附加数据ToolStripMenuItem";
            this.隐藏显示附加数据ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.隐藏显示附加数据ToolStripMenuItem.Text = "隐藏/显示附加数据";
            this.隐藏显示附加数据ToolStripMenuItem.Click += new System.EventHandler(this.隐藏显示附加数据ToolStripMenuItem_Click);
            // 
            // 隐藏显示天线ToolStripMenuItem
            // 
            this.隐藏显示天线ToolStripMenuItem.Name = "隐藏显示天线ToolStripMenuItem";
            this.隐藏显示天线ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.隐藏显示天线ToolStripMenuItem.Text = "隐藏/显示天线";
            this.隐藏显示天线ToolStripMenuItem.Click += new System.EventHandler(this.隐藏显示天线ToolStripMenuItem_Click);
            // 
            // 隐藏显示协议ToolStripMenuItem
            // 
            this.隐藏显示协议ToolStripMenuItem.Name = "隐藏显示协议ToolStripMenuItem";
            this.隐藏显示协议ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.隐藏显示协议ToolStripMenuItem.Text = "隐藏/显示协议";
            this.隐藏显示协议ToolStripMenuItem.Click += new System.EventHandler(this.隐藏显示协议ToolStripMenuItem_Click);
            // 
            // 隐藏显示RSSLToolStripMenuItem
            // 
            this.隐藏显示RSSLToolStripMenuItem.Name = "隐藏显示RSSLToolStripMenuItem";
            this.隐藏显示RSSLToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.隐藏显示RSSLToolStripMenuItem.Text = "隐藏/显示RSSL";
            this.隐藏显示RSSLToolStripMenuItem.Click += new System.EventHandler(this.隐藏显示RSSLToolStripMenuItem_Click);
            // 
            // 隐藏显示频率ToolStripMenuItem
            // 
            this.隐藏显示频率ToolStripMenuItem.Name = "隐藏显示频率ToolStripMenuItem";
            this.隐藏显示频率ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.隐藏显示频率ToolStripMenuItem.Text = "隐藏/显示频率";
            this.隐藏显示频率ToolStripMenuItem.Click += new System.EventHandler(this.隐藏显示频率ToolStripMenuItem_Click);
            // 
            // 隐藏显示相位ToolStripMenuItem
            // 
            this.隐藏显示相位ToolStripMenuItem.Name = "隐藏显示相位ToolStripMenuItem";
            this.隐藏显示相位ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.隐藏显示相位ToolStripMenuItem.Text = "隐藏/显示相位";
            this.隐藏显示相位ToolStripMenuItem.Click += new System.EventHandler(this.隐藏显示相位ToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(16, 414);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 30);
            this.button2.TabIndex = 27;
            this.button2.Text = "导出";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // labtoatotims
            // 
            this.labtoatotims.AutoSize = true;
            this.labtoatotims.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.labtoatotims.ForeColor = System.Drawing.Color.Red;
            this.labtoatotims.Location = new System.Drawing.Point(154, 500);
            this.labtoatotims.Name = "labtoatotims";
            this.labtoatotims.Size = new System.Drawing.Size(27, 29);
            this.labtoatotims.TabIndex = 20;
            this.labtoatotims.Text = "0";
            // 
            // labtotalcnt
            // 
            this.labtotalcnt.AutoSize = true;
            this.labtotalcnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.labtotalcnt.ForeColor = System.Drawing.Color.Red;
            this.labtotalcnt.Location = new System.Drawing.Point(154, 470);
            this.labtotalcnt.Name = "labtotalcnt";
            this.labtotalcnt.Size = new System.Drawing.Size(27, 29);
            this.labtotalcnt.TabIndex = 149;
            this.labtotalcnt.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(72, 479);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 148;
            this.label3.Text = "标签数量：";
            // 
            // labreadtime
            // 
            this.labreadtime.AutoSize = true;
            this.labreadtime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.labreadtime.ForeColor = System.Drawing.Color.Red;
            this.labreadtime.Location = new System.Drawing.Point(154, 526);
            this.labreadtime.Name = "labreadtime";
            this.labreadtime.Size = new System.Drawing.Size(27, 29);
            this.labreadtime.TabIndex = 151;
            this.labreadtime.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(72, 535);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 17);
            this.label6.TabIndex = 150;
            this.label6.Text = "耗时(ms)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(74, 509);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 152;
            this.label2.Text = "读取次数：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 56);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(95, 12);
            this.label16.TabIndex = 158;
            this.label16.Text = "指定时间停止(s)";
            // 
            // textBox_stopsec
            // 
            this.textBox_stopsec.Location = new System.Drawing.Point(121, 47);
            this.textBox_stopsec.Name = "textBox_stopsec";
            this.textBox_stopsec.Size = new System.Drawing.Size(60, 21);
            this.textBox_stopsec.TabIndex = 157;
            this.textBox_stopsec.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 159;
            this.label5.Text = "盘存模式";
            // 
            // pattern
            // 
            this.pattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pattern.FormattingEnabled = true;
            this.pattern.Items.AddRange(new object[] {
            "普通模式",
            "快速模式",
            "EX10快速模式"});
            this.pattern.Location = new System.Drawing.Point(77, 77);
            this.pattern.Name = "pattern";
            this.pattern.Size = new System.Drawing.Size(104, 23);
            this.pattern.TabIndex = 160;
            this.pattern.SelectedIndexChanged += new System.EventHandler(this.pattern_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.cbant_4);
            this.groupBox1.Controls.Add(this.cbant_3);
            this.groupBox1.Controls.Add(this.cbant_2);
            this.groupBox1.Controls.Add(this.cbant_1);
            this.groupBox1.Controls.Add(this.btn16antset);
            this.groupBox1.Location = new System.Drawing.Point(9, 177);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 80);
            this.groupBox1.TabIndex = 200;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "天线设置";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label7.AutoSize = true;
            this.label7.Enabled = false;
            this.label7.Font = new System.Drawing.Font("宋体", 16F);
            this.label7.Location = new System.Drawing.Point(104, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 22);
            this.label7.TabIndex = 160;
            this.label7.Text = "﹀";
            this.label7.Visible = false;
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.checkBox1.Location = new System.Drawing.Point(5, 54);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 20);
            this.checkBox1.TabIndex = 27;
            this.checkBox1.Text = "全选";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cbant_4
            // 
            this.cbant_4.AutoSize = true;
            this.cbant_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.cbant_4.Location = new System.Drawing.Point(196, 20);
            this.cbant_4.Name = "cbant_4";
            this.cbant_4.Size = new System.Drawing.Size(52, 20);
            this.cbant_4.TabIndex = 21;
            this.cbant_4.Text = "ant4";
            this.cbant_4.UseVisualStyleBackColor = true;
            // 
            // cbant_3
            // 
            this.cbant_3.AutoSize = true;
            this.cbant_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.cbant_3.Location = new System.Drawing.Point(130, 20);
            this.cbant_3.Name = "cbant_3";
            this.cbant_3.Size = new System.Drawing.Size(52, 20);
            this.cbant_3.TabIndex = 20;
            this.cbant_3.Text = "ant3";
            this.cbant_3.UseVisualStyleBackColor = true;
            // 
            // cbant_2
            // 
            this.cbant_2.AutoSize = true;
            this.cbant_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.cbant_2.Location = new System.Drawing.Point(68, 20);
            this.cbant_2.Name = "cbant_2";
            this.cbant_2.Size = new System.Drawing.Size(52, 20);
            this.cbant_2.TabIndex = 19;
            this.cbant_2.Text = "ant2";
            this.cbant_2.UseVisualStyleBackColor = true;
            // 
            // cbant_1
            // 
            this.cbant_1.AutoSize = true;
            this.cbant_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.cbant_1.Location = new System.Drawing.Point(5, 20);
            this.cbant_1.Name = "cbant_1";
            this.cbant_1.Size = new System.Drawing.Size(52, 20);
            this.cbant_1.TabIndex = 18;
            this.cbant_1.Text = "ant1";
            this.cbant_1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbisunibyant);
            this.groupBox2.Controls.Add(this.cbischgcolor);
            this.groupBox2.Controls.Add(this.cbisunibynullemd);
            this.groupBox2.Controls.Add(this.textBox_stopsec);
            this.groupBox2.Controls.Add(this.btnInvParas);
            this.groupBox2.Controls.Add(this.pattern);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Location = new System.Drawing.Point(9, 263);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 109);
            this.groupBox2.TabIndex = 201;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "盘点参数";
            // 
            // cbisunibyant
            // 
            this.cbisunibyant.AutoSize = true;
            this.cbisunibyant.Location = new System.Drawing.Point(184, 19);
            this.cbisunibyant.Name = "cbisunibyant";
            this.cbisunibyant.Size = new System.Drawing.Size(72, 16);
            this.cbisunibyant.TabIndex = 163;
            this.cbisunibyant.Text = "天线唯一";
            this.cbisunibyant.UseVisualStyleBackColor = true;
            this.cbisunibyant.CheckedChanged += new System.EventHandler(this.cbisunibyant_CheckedChanged);
            // 
            // cbischgcolor
            // 
            this.cbischgcolor.AutoSize = true;
            this.cbischgcolor.Checked = true;
            this.cbischgcolor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbischgcolor.Location = new System.Drawing.Point(108, 19);
            this.cbischgcolor.Name = "cbischgcolor";
            this.cbischgcolor.Size = new System.Drawing.Size(72, 16);
            this.cbischgcolor.TabIndex = 162;
            this.cbischgcolor.Text = "颜色变化";
            this.cbischgcolor.UseVisualStyleBackColor = true;
            this.cbischgcolor.CheckedChanged += new System.EventHandler(this.cbischgcolor_CheckedChanged);
            // 
            // cbisunibynullemd
            // 
            this.cbisunibynullemd.AutoSize = true;
            this.cbisunibynullemd.Location = new System.Drawing.Point(6, 20);
            this.cbisunibynullemd.Name = "cbisunibynullemd";
            this.cbisunibynullemd.Size = new System.Drawing.Size(96, 16);
            this.cbisunibynullemd.TabIndex = 161;
            this.cbisunibynullemd.Text = "附加数据唯一";
            this.cbisunibynullemd.UseVisualStyleBackColor = true;
            this.cbisunibynullemd.CheckedChanged += new System.EventHandler(this.cbisunibynullemd_CheckedChanged);
            // 
            // lvTags
            // 
            this.lvTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
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
            this.lvTags.HideSelection = false;
            this.lvTags.Location = new System.Drawing.Point(277, 42);
            this.lvTags.Name = "lvTags";
            this.lvTags.Size = new System.Drawing.Size(833, 532);
            this.lvTags.TabIndex = 29;
            this.lvTags.UseCompatibleStateImageBehavior = false;
            this.lvTags.View = System.Windows.Forms.View.Details;
            this.lvTags.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTags_ColumnClick_1);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "编号";
            this.columnHeader1.Width = 57;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "读次数";
            this.columnHeader2.Width = 71;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "EPC ID";
            this.columnHeader3.Width = 155;
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
            this.columnHeader9.Text = "相位(弧度)";
            this.columnHeader9.Width = 74;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 592);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labreadtime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labtotalcnt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labtoatotims);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lvTags);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnstop);
            this.Controls.Add(this.btnstart);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ModuleReaderManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnconnect;
        private System.Windows.Forms.Button btnstart;
        private System.Windows.Forms.Button btnstop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbbreadertype;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btndisconnect;
        private System.Windows.Forms.Button btnInvParas;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem readparamenu;
        private System.Windows.Forms.ToolStripMenuItem tagopmenu;
        private System.Windows.Forms.ToolStripMenuItem updatemenu;
        private System.Windows.Forms.ToolStripMenuItem menutest;
        private System.Windows.Forms.ToolStripMenuItem gen2tagopMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iso183k6btagopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CountTagMenuItem;
        private ModuleReaderManager.CustomToolStripMenuItem menuitemlog;
        private System.Windows.Forms.ToolStripMenuItem MsgDebugMenu;
        private System.Windows.Forms.ToolStripMenuItem menuitemmultibankwrite;
        private System.Windows.Forms.Button btn16antset;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 其它ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 驻波ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 天线区域ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 自动发卡ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lBTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 拷贝当前TIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 过滤标签ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除过滤ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏显示附加数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏显示天线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏显示协议ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏显示RSSLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏显示频率ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏显示相位ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标签信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox tbip;
        private System.Windows.Forms.ComboBox BaudRate;
        private System.Windows.Forms.Label labtoatotims;
        private DoubleBufferListView lvTags;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label labtotalcnt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labreadtime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox_stopsec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox pattern;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbant_4;
        private System.Windows.Forms.CheckBox cbant_3;
        private System.Windows.Forms.CheckBox cbant_2;
        private System.Windows.Forms.CheckBox cbant_1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbisunibyant;
        private System.Windows.Forms.CheckBox cbischgcolor;
        private System.Windows.Forms.CheckBox cbisunibynullemd;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripMenuItem 标签特殊指令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pSAMToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemBox;
        private System.Windows.Forms.ToolStripMenuItem 柜子应用1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 柜子应用2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标签温度及LEDToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ex10系列初始化ToolStripMenuItem1;
    }
}

