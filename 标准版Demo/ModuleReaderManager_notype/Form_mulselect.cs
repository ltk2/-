using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ModuleTech;
using ModuleTech.Gen2;
using ModuleTech.CustomCmd;
using ModuleLibrary;

namespace ModuleReaderManager
{
    public partial class Form_mulselect : Form
    {
        public Form_mulselect(Reader rdr, ReaderParams param, List<string> lltags)
        {
            InitializeComponent();
            mordr = rdr;
            rparam = param;
            lepcs = new List<string>();
            lepcs.AddRange(lltags);
        }
        ReaderParams rparam = null;
        Reader mordr = null;
        List<string> lepcs = null;

        private void btnsetmulseltag_Click(object sender, EventArgs e)
        {
            Gen2TagFilter[] gtf2;
            int count=0;
            for (int i = 0; i < lvSelecttags.Items.Count; i++)
                {
                    if(lvSelecttags.Items[i].Checked)
                        count++;
                }
            if(count>16)
            {
                MessageBox.Show("不能超过16个过滤标签");
                return;
            }

            gtf2=new Gen2TagFilter[count];
            int p=0;
              for (int i = 0; i < lvSelecttags.Items.Count; i++)
                {
                    if(lvSelecttags.Items[i].Checked)
                    {
                        int flen=lvSelecttags.Items[i].Text.Length*4;
                        byte[] fdata=ByteFormat.FromHex(lvSelecttags.Items[i].Text);
                        gtf2[p++]=new Gen2TagFilter(flen,fdata,MemBank.EPC,32,false);
                    }
                }
              try
              {
                  if(count>0)
                      mordr.ParamSet("MultiTagFilters", gtf2);
                  else
                      mordr.ParamSet("MultiTagFilters", null);
              }
              catch (ModuleException mex)
              {
                  MessageBox.Show(mex.Message);
                  return;
              }
              MessageBox.Show("OK");
              this.Close();
        }

        private void allcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (allcheckBox.Checked)
            {

                for (int i = 0; i < lvSelecttags.Items.Count; i++)
                {
                    lvSelecttags.Items[i].Checked = true;
                }

            }
            else
            {
                for (int i = 0; i < lvSelecttags.Items.Count; i++)
                {
                    lvSelecttags.Items[i].Checked = false;
                }
            }
        }

        private void Form_mulselect_Load(object sender, EventArgs e)
        {
            lvSelecttags.Items.Clear();
            for (int i = 0; i < lepcs.Count; i++)
            {

                ListViewItem item = new ListViewItem(lepcs[i].ToString());
                lvSelecttags.Items.Add(item);
                
            }
        }
    }
}
