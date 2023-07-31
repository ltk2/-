namespace ModuleReaderManager
{
    partial class Form_Password
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
            this.label7 = new System.Windows.Forms.Label();
            this.passe = new System.Windows.Forms.TextBox();
            this.btnipset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(49, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "请输入密码";
            // 
            // passe
            // 
            this.passe.Location = new System.Drawing.Point(120, 36);
            this.passe.Name = "passe";
            this.passe.Size = new System.Drawing.Size(97, 21);
            this.passe.TabIndex = 15;
            // 
            // btnipset
            // 
            this.btnipset.Location = new System.Drawing.Point(93, 94);
            this.btnipset.Name = "btnipset";
            this.btnipset.Size = new System.Drawing.Size(85, 34);
            this.btnipset.TabIndex = 16;
            this.btnipset.Text = "确定";
            this.btnipset.UseVisualStyleBackColor = true;
            this.btnipset.Click += new System.EventHandler(this.btnipset_Click);
            // 
            // Form_Password
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 168);
            this.Controls.Add(this.btnipset);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.passe);
            this.Name = "Form_Password";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "密码验证";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox passe;
        private System.Windows.Forms.Button btnipset;
    }
}