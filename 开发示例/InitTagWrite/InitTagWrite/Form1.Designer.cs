namespace InitTagWrite
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
            this.btnwriteepc = new System.Windows.Forms.Button();
            this.btnwritebank = new System.Windows.Forms.Button();
            this.btnreadbank = new System.Windows.Forms.Button();
            this.btnlocktag = new System.Windows.Forms.Button();
            this.btnkilltag = new System.Windows.Forms.Button();
            this.btninittag = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnwriteepc
            // 
            this.btnwriteepc.Location = new System.Drawing.Point(57, 21);
            this.btnwriteepc.Name = "btnwriteepc";
            this.btnwriteepc.Size = new System.Drawing.Size(75, 23);
            this.btnwriteepc.TabIndex = 0;
            this.btnwriteepc.Text = "写EPC";
            this.btnwriteepc.UseVisualStyleBackColor = true;
            this.btnwriteepc.Click += new System.EventHandler(this.btnwriteepc_Click);
            // 
            // btnwritebank
            // 
            this.btnwritebank.Location = new System.Drawing.Point(57, 52);
            this.btnwritebank.Name = "btnwritebank";
            this.btnwritebank.Size = new System.Drawing.Size(75, 23);
            this.btnwritebank.TabIndex = 1;
            this.btnwritebank.Text = "写存储区";
            this.btnwritebank.UseVisualStyleBackColor = true;
            this.btnwritebank.Click += new System.EventHandler(this.btnwritebank_Click);
            // 
            // btnreadbank
            // 
            this.btnreadbank.Location = new System.Drawing.Point(57, 83);
            this.btnreadbank.Name = "btnreadbank";
            this.btnreadbank.Size = new System.Drawing.Size(75, 23);
            this.btnreadbank.TabIndex = 2;
            this.btnreadbank.Text = "读存储区";
            this.btnreadbank.UseVisualStyleBackColor = true;
            this.btnreadbank.Click += new System.EventHandler(this.btnreadbank_Click);
            // 
            // btnlocktag
            // 
            this.btnlocktag.Location = new System.Drawing.Point(57, 114);
            this.btnlocktag.Name = "btnlocktag";
            this.btnlocktag.Size = new System.Drawing.Size(75, 23);
            this.btnlocktag.TabIndex = 3;
            this.btnlocktag.Text = "锁标签";
            this.btnlocktag.UseVisualStyleBackColor = true;
            this.btnlocktag.Click += new System.EventHandler(this.btnlocktag_Click);
            // 
            // btnkilltag
            // 
            this.btnkilltag.Location = new System.Drawing.Point(57, 145);
            this.btnkilltag.Name = "btnkilltag";
            this.btnkilltag.Size = new System.Drawing.Size(75, 23);
            this.btnkilltag.TabIndex = 4;
            this.btnkilltag.Text = "销毁标签";
            this.btnkilltag.UseVisualStyleBackColor = true;
            this.btnkilltag.Click += new System.EventHandler(this.btnkilltag_Click);
            // 
            // btninittag
            // 
            this.btninittag.Location = new System.Drawing.Point(155, 65);
            this.btninittag.Name = "btninittag";
            this.btninittag.Size = new System.Drawing.Size(80, 63);
            this.btninittag.TabIndex = 5;
            this.btninittag.Text = "初始化标签";
            this.btninittag.UseVisualStyleBackColor = true;
            this.btninittag.Click += new System.EventHandler(this.btninittag_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 206);
            this.Controls.Add(this.btninittag);
            this.Controls.Add(this.btnkilltag);
            this.Controls.Add(this.btnlocktag);
            this.Controls.Add(this.btnreadbank);
            this.Controls.Add(this.btnwritebank);
            this.Controls.Add(this.btnwriteepc);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnwriteepc;
        private System.Windows.Forms.Button btnwritebank;
        private System.Windows.Forms.Button btnreadbank;
        private System.Windows.Forms.Button btnlocktag;
        private System.Windows.Forms.Button btnkilltag;
        private System.Windows.Forms.Button btninittag;
    }
}

