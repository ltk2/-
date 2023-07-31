using ModuleTech;
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
    public partial class Form_Password : Form
    {
        ReaderParams m_params;
        Reader rdr;
        public Form_Password(ReaderParams _m_params, Reader _rdr)
        {
            InitializeComponent();
            m_params = _m_params;
            rdr = _rdr;
        }

        private void btnipset_Click(object sender, EventArgs e)
        {
            var pas = passe.Text;

            if (pas != "2022")
            {
                MessageBox.Show("密码错误!");
                return;
            }

            this.Close();

            ModuleSaveParamsFrm mspf = new ModuleSaveParamsFrm(m_params, rdr);
            mspf.ShowDialog();
        }
    }
}
