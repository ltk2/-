using ModuleLibrary;
using ModuleTech;
using ModuleTech.Gen2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    public partial class gen2opForm2 : Form
    {
        ReaderParams rparam = null;
        Reader mordr = null;
        Form1 Mfrm = null;
        public gen2opForm2(Reader rdr, ReaderParams param, Form1 frm)
        {
            InitializeComponent();
            mordr = rdr;
            rparam = param;
            Mfrm = frm;
        }

        private void btnWriteEpc_Click(object sender, EventArgs e)
        {
            if (cbbopbankType.SelectedIndex == 0) {
                readClick();
            }





        }

        private void button1_Click(object sender, EventArgs e)
        {
            gen2opForm frm = new gen2opForm(mordr, rparam, Mfrm);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void gen2opForm2_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= rparam.antcnt; i++)
            {
                cbb16opant.Items.Add(i);
            }
        }

        public void readClick() {

            int ret;
            Gen2TagFilter filter = null;

            if (this.cbbopbank.SelectedIndex == -1)
            {
                MessageBox.Show("请选择操作区域");
                return;
            }

            if (tbstartaddr.Text == "")
            {
                MessageBox.Show("请输入起始地址");
                return;
            }
            if (tbblocks.Text == "")
            {
                MessageBox.Show("请输入读块数");
                return;
            }

            if (this.tbaccesspasswd.Text!="")
            {
                ret = Form1.IsValidPasswd(this.tbaccesspasswd.Text.Trim());
                {
                    switch (ret)
                    {
                        case -3:
                            MessageBox.Show("访问密码不能为空");
                            break;
                        case -2:
                        case -4:
                            MessageBox.Show("访问密码必须是8个16进制数");
                            break;
                        case -1:
                            MessageBox.Show("访问密码只能是16进制数字");
                            break;

                    }
                }
                if (ret != 0)
                    return;
                else
                {
                    uint passwd = uint.Parse(this.tbaccesspasswd.Text.Trim(), System.Globalization.NumberStyles.AllowHexSpecifier);
                    mordr.ParamSet("AccessPassword", passwd);
                }
            }
            else
                mordr.ParamSet("AccessPassword", (uint)0);

            ushort[] readdata = null;

            //ret = cbb16opant.SelectedIndex+1;
            mordr.ParamSet("TagopAntenna", cbb16opant.SelectedIndex + 1);
            if (this.tbfldata.Text!="")
            {
                bool isHex = Form1.IsHexString(tbfldata.Text.ToString());
                if (!isHex)
                {
                    MessageBox.Show("匹配数据只能是16进制字符串");
                    return;
                }
                
                if (this.cbbfilterbank.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择过滤bank");
                    return;
                }

                if (this.cbbfilterrule.SelectedIndex == -1)
                {
                    MessageBox.Show("请输入过滤规则");
                    return;
                }

                int bitaddr = 0;
                if (this.tbfilteraddr.Text.Trim() == "")
                {
                    MessageBox.Show("请输入过滤bank的起始地址,以字为最小单位");
                    return;
                }
                else
                {
                    try
                    {
                        bitaddr = int.Parse(this.tbfilteraddr.Text.Trim());
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("起始地址请输入数字");
                        return;
                    }
                    if (bitaddr < 0)
                    {
                        MessageBox.Show("地址必须大于零");
                        return;
                    }
                }

                var Mask = tbfldata.Text.Trim();
                string hexstr = Mask.Trim();
                if (hexstr.Length % 2 != 0)
                    hexstr += "0";

                var filterbytes = ByteFormat.FromHex(hexstr);
                var bitlen = Mask.Trim().Length * 4;

                filter = new Gen2TagFilter(bitlen, filterbytes,(MemBank)cbbfilterbank.SelectedIndex + 1,
                    bitaddr,cbbfilterrule.SelectedIndex == 0 ? false : true);
                // filter = new Gen2TagFilter(bitlen, filterbytes, (MemBank)cbbfilterbank.SelectedIndex + 1,
                //bitaddr, cbbfilterrule.SelectedIndex == 0 ? false : true);
            }

            try
            {
                int st = Environment.TickCount;
                readdata = mordr.ReadTagMemWords(filter,
                    (MemBank)this.cbbopbank.SelectedIndex, int.Parse(this.tbstartaddr.Text.Trim())
                    , int.Parse(this.tbblocks.Text.Trim()));
                if (rparam.setGPO1)
                {
                    mordr.GPOSet(1, true);
                    System.Threading.Thread.Sleep(20);
                    mordr.GPOSet(1, false);
                }

            }
            catch (OpFaidedException notagexp)
            {
                if (notagexp.ErrCode == 0x400)
                    MessageBox.Show("没法发现标签");
                else
                    MessageBox.Show("操作失败:" + notagexp.ToString());

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败:" + ex.ToString());
                return;
            }

            string readdatastr = "";
            for (int i = 0; i < readdata.Length; ++i)
                readdatastr += readdata[i].ToString("X4");

            this.rtbdata.Text = readdatastr;
        }

        private void cbbopbankType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbopbankType.SelectedIndex == 2)
            {
                cbblocktype.Enabled = true;
                label5.Enabled = true;
            }
            else if (cbbopbankType.SelectedIndex == 3)
            {
                tbkillpasswd.Enabled = true;
                label1.Enabled = true;
            }
            else {
                cbblocktype.Enabled = false;
                tbkillpasswd.Enabled = false;
                label5.Enabled = false;
                label1.Enabled = false;
            }
        }

        private void cbbfilterbank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbfilterbank.SelectedIndex == 0) {
                tbfilteraddr.Text = "32";
            }


        }

        private void cbbopbank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbopbankType.SelectedIndex == 0) {
                if (cbbopbank.SelectedIndex == 1) {

                }




            }


        }
    }
}
