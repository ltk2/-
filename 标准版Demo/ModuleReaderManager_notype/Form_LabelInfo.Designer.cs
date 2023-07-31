namespace ModuleReaderManager
{
    partial class Form_LabelInfo
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
            this.btnIcIdentify = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.UserLength = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.DestroyPassword = new System.Windows.Forms.Label();
            this.VisitPassword = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Epc = new System.Windows.Forms.Label();
            this.EpcLength = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TidManufacturers = new System.Windows.Forms.Label();
            this.TidText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbb16opant = new System.Windows.Forms.ComboBox();
            this.tbaccesspasswd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnIcIdentify
            // 
            this.btnIcIdentify.Location = new System.Drawing.Point(278, 24);
            this.btnIcIdentify.Name = "btnIcIdentify";
            this.btnIcIdentify.Size = new System.Drawing.Size(71, 21);
            this.btnIcIdentify.TabIndex = 31;
            this.btnIcIdentify.Text = "读取标签信息";
            this.btnIcIdentify.UseVisualStyleBackColor = true;
            this.btnIcIdentify.Click += new System.EventHandler(this.btnIcIdentify_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.UserLength);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.DestroyPassword);
            this.groupBox3.Controls.Add(this.VisitPassword);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.Epc);
            this.groupBox3.Controls.Add(this.EpcLength);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.TidManufacturers);
            this.groupBox3.Controls.Add(this.TidText);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(10, 68);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(347, 370);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "标签信息";
            // 
            // UserLength
            // 
            this.UserLength.AutoSize = true;
            this.UserLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserLength.Location = new System.Drawing.Point(243, 130);
            this.UserLength.Name = "UserLength";
            this.UserLength.Size = new System.Drawing.Size(48, 17);
            this.UserLength.TabIndex = 51;
            this.UserLength.Text = "NULL";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(178, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 50;
            this.label6.Text = "User容量:";
            // 
            // DestroyPassword
            // 
            this.DestroyPassword.AutoSize = true;
            this.DestroyPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DestroyPassword.Location = new System.Drawing.Point(65, 101);
            this.DestroyPassword.Name = "DestroyPassword";
            this.DestroyPassword.Size = new System.Drawing.Size(48, 17);
            this.DestroyPassword.TabIndex = 49;
            this.DestroyPassword.Text = "NULL";
            // 
            // VisitPassword
            // 
            this.VisitPassword.AutoSize = true;
            this.VisitPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VisitPassword.Location = new System.Drawing.Point(243, 101);
            this.VisitPassword.Name = "VisitPassword";
            this.VisitPassword.Size = new System.Drawing.Size(48, 17);
            this.VisitPassword.TabIndex = 48;
            this.VisitPassword.Text = "NULL";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(178, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 47;
            this.label9.Text = "访问密码:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 46;
            this.label10.Text = "销毁密码:";
            // 
            // Epc
            // 
            this.Epc.AutoSize = true;
            this.Epc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Epc.Location = new System.Drawing.Point(65, 163);
            this.Epc.Name = "Epc";
            this.Epc.Size = new System.Drawing.Size(48, 17);
            this.Epc.TabIndex = 45;
            this.Epc.Text = "NULL";
            // 
            // EpcLength
            // 
            this.EpcLength.AutoSize = true;
            this.EpcLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EpcLength.Location = new System.Drawing.Point(65, 134);
            this.EpcLength.Name = "EpcLength";
            this.EpcLength.Size = new System.Drawing.Size(48, 17);
            this.EpcLength.TabIndex = 44;
            this.EpcLength.Text = "NULL";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 43;
            this.label5.Text = "EPC容量:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 42;
            this.label4.Text = "EPC:";
            // 
            // TidManufacturers
            // 
            this.TidManufacturers.AutoSize = true;
            this.TidManufacturers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TidManufacturers.Location = new System.Drawing.Point(65, 69);
            this.TidManufacturers.Name = "TidManufacturers";
            this.TidManufacturers.Size = new System.Drawing.Size(48, 17);
            this.TidManufacturers.TabIndex = 41;
            this.TidManufacturers.Text = "NULL";
            // 
            // TidText
            // 
            this.TidText.AutoSize = true;
            this.TidText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TidText.Location = new System.Drawing.Point(243, 69);
            this.TidText.Name = "TidText";
            this.TidText.Size = new System.Drawing.Size(48, 17);
            this.TidText.TabIndex = 39;
            this.TidText.Text = "NULL";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(178, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "TID内容:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "IC型号:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 33;
            this.label1.Text = "天线";
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
            this.cbb16opant.Location = new System.Drawing.Point(54, 24);
            this.cbb16opant.Name = "cbb16opant";
            this.cbb16opant.Size = new System.Drawing.Size(71, 20);
            this.cbb16opant.TabIndex = 34;
            // 
            // tbaccesspasswd
            // 
            this.tbaccesspasswd.Location = new System.Drawing.Point(186, 23);
            this.tbaccesspasswd.Name = "tbaccesspasswd";
            this.tbaccesspasswd.Size = new System.Drawing.Size(71, 21);
            this.tbaccesspasswd.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(127, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 35;
            this.label8.Text = "访问密码";
            // 
            // Form_LabelInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbb16opant);
            this.Controls.Add(this.tbaccesspasswd);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnIcIdentify);
            this.Name = "Form_LabelInfo";
            this.Text = "标签信息";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnIcIdentify;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbb16opant;
        private System.Windows.Forms.TextBox tbaccesspasswd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label TidManufacturers;
        private System.Windows.Forms.Label TidText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label EpcLength;
        private System.Windows.Forms.Label Epc;
        private System.Windows.Forms.Label DestroyPassword;
        private System.Windows.Forms.Label VisitPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label UserLength;
        private System.Windows.Forms.Label label6;
    }
}