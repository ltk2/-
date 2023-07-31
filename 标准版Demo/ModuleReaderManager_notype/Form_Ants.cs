using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    public partial class Form_Ants : Form
    {
        public Form_Ants()
        {
            InitializeComponent();
        }

        private void Form_Ants_Load(object sender, EventArgs e)
        {
            this.Location= new Point(380, 150);
            label1.Text = this.Location.ToString();
        }
    }
}
