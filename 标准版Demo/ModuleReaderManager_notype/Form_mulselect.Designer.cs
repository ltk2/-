namespace ModuleReaderManager
{
    partial class Form_mulselect
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
            this.allcheckBox = new System.Windows.Forms.CheckBox();
            this.btnsetmulseltag = new System.Windows.Forms.Button();
            this.lvSelecttags = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // allcheckBox
            // 
            this.allcheckBox.AutoSize = true;
            this.allcheckBox.Location = new System.Drawing.Point(23, 203);
            this.allcheckBox.Name = "allcheckBox";
            this.allcheckBox.Size = new System.Drawing.Size(48, 16);
            this.allcheckBox.TabIndex = 36;
            this.allcheckBox.Text = "全选";
            this.allcheckBox.UseVisualStyleBackColor = true;
            this.allcheckBox.CheckedChanged += new System.EventHandler(this.allcheckBox_CheckedChanged);
            // 
            // btnsetmulseltag
            // 
            this.btnsetmulseltag.Location = new System.Drawing.Point(13, 225);
            this.btnsetmulseltag.Name = "btnsetmulseltag";
            this.btnsetmulseltag.Size = new System.Drawing.Size(177, 23);
            this.btnsetmulseltag.TabIndex = 35;
            this.btnsetmulseltag.Text = "设置";
            this.btnsetmulseltag.UseVisualStyleBackColor = true;
            this.btnsetmulseltag.Click += new System.EventHandler(this.btnsetmulseltag_Click);
            // 
            // lvSelecttags
            // 
            this.lvSelecttags.CheckBoxes = true;
            this.lvSelecttags.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvSelecttags.GridLines = true;
            this.lvSelecttags.LabelEdit = true;
            this.lvSelecttags.Location = new System.Drawing.Point(13, 12);
            this.lvSelecttags.Name = "lvSelecttags";
            this.lvSelecttags.Size = new System.Drawing.Size(184, 185);
            this.lvSelecttags.TabIndex = 33;
            this.lvSelecttags.UseCompatibleStateImageBehavior = false;
            this.lvSelecttags.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "条目";
            this.columnHeader1.Width = 174;
            // 
            // Form_mulselect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 262);
            this.Controls.Add(this.allcheckBox);
            this.Controls.Add(this.btnsetmulseltag);
            this.Controls.Add(this.lvSelecttags);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_mulselect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "多标签选择";
            this.Load += new System.EventHandler(this.Form_mulselect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox allcheckBox;
        private System.Windows.Forms.Button btnsetmulseltag;
        private System.Windows.Forms.ListView lvSelecttags;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}