namespace ModuleReaderManager
{
    partial class Form_e710init
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_chip1 = new System.Windows.Forms.ComboBox();
            this.comboBox_antport2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_region3 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_sort4 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_get = new System.Windows.Forms.Button();
            this.button_set = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox_writerule = new System.Windows.Forms.ComboBox();
            this.label_chiptype = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox_rtype = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "芯片";
            // 
            // comboBox_chip1
            // 
            this.comboBox_chip1.FormattingEnabled = true;
            this.comboBox_chip1.Items.AddRange(new object[] {
            "未定义",
            "E710",
            "E510",
            "E310",
            "E910"});
            this.comboBox_chip1.Location = new System.Drawing.Point(107, 21);
            this.comboBox_chip1.Name = "comboBox_chip1";
            this.comboBox_chip1.Size = new System.Drawing.Size(121, 20);
            this.comboBox_chip1.TabIndex = 1;
            // 
            // comboBox_antport2
            // 
            this.comboBox_antport2.FormattingEnabled = true;
            this.comboBox_antport2.Items.AddRange(new object[] {
            "ANTPORT_1",
            "ANTPORT_2",
            "ANTPORT_4",
            "ANTPORT_8",
            "ANTPORT_16",
            "ANTPORT_32",
            "ANTPORT_1_贴片式28X28",
            "ANTPORT_1_贴片式21X21"});
            this.comboBox_antport2.Location = new System.Drawing.Point(107, 63);
            this.comboBox_antport2.Name = "comboBox_antport2";
            this.comboBox_antport2.Size = new System.Drawing.Size(121, 20);
            this.comboBox_antport2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "天线口";
            // 
            // comboBox_region3
            // 
            this.comboBox_region3.FormattingEnabled = true;
            this.comboBox_region3.Items.AddRange(new object[] {
            "CHINA",
            "FCC",
            "JAPAN",
            "CE LOW",
            "KOREA",
            "CE HIGH",
            "HK",
            "TAIWAN",
            "MALAYSIA",
            "SOUTH_AFRICA",
            "BRAZIL",
            "THAILAND",
            "SINGAPORE",
            "AUSTRALIA",
            "INDIA",
            "URUGUAY",
            "VIETNAM",
            "ISRAEL",
            "PHILIPPINES",
            "INDONESIA",
            "NEW_ZEALAND",
            "PERU",
            "RUSSIA",
            "CE LOW AND HIGH",
            "FCC_CUSTOM"});
            this.comboBox_region3.Location = new System.Drawing.Point(56, 26);
            this.comboBox_region3.Name = "comboBox_region3";
            this.comboBox_region3.Size = new System.Drawing.Size(121, 20);
            this.comboBox_region3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "区域";
            // 
            // comboBox_sort4
            // 
            this.comboBox_sort4.FormattingEnabled = true;
            this.comboBox_sort4.Location = new System.Drawing.Point(56, 62);
            this.comboBox_sort4.Name = "comboBox_sort4";
            this.comboBox_sort4.Size = new System.Drawing.Size(121, 20);
            this.comboBox_sort4.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "序号";
            // 
            // button_get
            // 
            this.button_get.Location = new System.Drawing.Point(93, 227);
            this.button_get.Name = "button_get";
            this.button_get.Size = new System.Drawing.Size(134, 23);
            this.button_get.TabIndex = 8;
            this.button_get.Text = "读取";
            this.button_get.UseVisualStyleBackColor = true;
            this.button_get.Click += new System.EventHandler(this.button_get_Click);
            // 
            // button_set
            // 
            this.button_set.Location = new System.Drawing.Point(268, 227);
            this.button_set.Name = "button_set";
            this.button_set.Size = new System.Drawing.Size(130, 23);
            this.button_set.TabIndex = 9;
            this.button_set.Text = "写入";
            this.button_set.UseVisualStyleBackColor = true;
            this.button_set.Click += new System.EventHandler(this.button_set_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(24, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 108);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "芯片";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox_writerule);
            this.groupBox2.Controls.Add(this.label_chiptype);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.comboBox_region3);
            this.groupBox2.Controls.Add(this.comboBox_sort4);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(24, 121);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 100);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "区域";
            // 
            // comboBox_writerule
            // 
            this.comboBox_writerule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_writerule.FormattingEnabled = true;
            this.comboBox_writerule.Items.AddRange(new object[] {
            "按芯片天线口写入",
            "按型号写入"});
            this.comboBox_writerule.Location = new System.Drawing.Point(235, 58);
            this.comboBox_writerule.Name = "comboBox_writerule";
            this.comboBox_writerule.Size = new System.Drawing.Size(121, 20);
            this.comboBox_writerule.TabIndex = 10;
            // 
            // label_chiptype
            // 
            this.label_chiptype.AutoSize = true;
            this.label_chiptype.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_chiptype.ForeColor = System.Drawing.Color.Red;
            this.label_chiptype.Location = new System.Drawing.Point(328, 22);
            this.label_chiptype.Name = "label_chiptype";
            this.label_chiptype.Size = new System.Drawing.Size(42, 20);
            this.label_chiptype.TabIndex = 9;
            this.label_chiptype.Text = "...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Maroon;
            this.label6.Location = new System.Drawing.Point(233, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "选择芯片或型号";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox_rtype);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(259, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 108);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "型号";
            // 
            // comboBox_rtype
            // 
            this.comboBox_rtype.FormattingEnabled = true;
            this.comboBox_rtype.Items.AddRange(new object[] {
            "未定义",
            "SIM7100",
            "SIM7200",
            "SIM7300",
            "SIM7400",
            "SIM7500",
            "SIM7600",
            "SIM3100",
            "SIM3200",
            "SIM3300",
            "SIM3400",
            "SIM3500",
            "SIM3600",
            "SIM5100",
            "SIM5200",
            "SIM5300",
            "SIM5400",
            "SIM5500",
            "SIM5600"});
            this.comboBox_rtype.Location = new System.Drawing.Point(76, 44);
            this.comboBox_rtype.Name = "comboBox_rtype";
            this.comboBox_rtype.Size = new System.Drawing.Size(121, 20);
            this.comboBox_rtype.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "型号";
            // 
            // Form_e710init
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 262);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_set);
            this.Controls.Add(this.button_get);
            this.Controls.Add(this.comboBox_antport2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_chip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_e710init";
            this.Text = "Form_e710init";
            this.Load += new System.EventHandler(this.Form_e710init_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_chip1;
        private System.Windows.Forms.ComboBox comboBox_antport2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_region3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_sort4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_get;
        private System.Windows.Forms.Button button_set;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox_rtype;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_chiptype;
        private System.Windows.Forms.ComboBox comboBox_writerule;
    }
}