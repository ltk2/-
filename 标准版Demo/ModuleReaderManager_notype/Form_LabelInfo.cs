using ModuleLibrary;
using ModuleTech;
using ModuleTech.Gen2;
using System;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    public partial class Form_LabelInfo : Form
    {
        ReaderParams rparam = null;
        Reader mordr = null;
        Form1 Mfrm = null;
        public Form_LabelInfo(Reader rdr, ReaderParams param, Form1 frm)
        {
            InitializeComponent();
            mordr = rdr;
            rparam = param;
            Mfrm = frm;
        }

        private void btnIcIdentify_Click(object sender, EventArgs e)
        {
            int ret;
            Gen2TagFilter filter = null;

            if (tbaccesspasswd.Text != "")
            {
                ret = Form1.IsValidPasswd(tbaccesspasswd.Text.Trim());
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
                {
                    return;
                }
                else
                {
                    uint passwd = uint.Parse(tbaccesspasswd.Text.Trim(), System.Globalization.NumberStyles.AllowHexSpecifier);
                    mordr.ParamSet("AccessPassword", passwd);
                }
            }
            else
            {
                mordr.ParamSet("AccessPassword", (uint)0);
            }

            ushort[] readdata = null;
            ushort[] epc2 = null;

            ushort[] pwd = null;

            ret = IsAntSet();
            if (ret == -1)
            {
                MessageBox.Show("请选择操作天线");
                return;
            }
            else if (ret == 1)
            {
                if (rparam.ischkant)
                {
                    DialogResult stat = DialogResult.OK;
                    stat = MessageBox.Show("在未检测到天线的端口执行操作，真的要执行吗?", "警告",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button2);
                    if (stat != DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            try
            {
                readdata = mordr.ReadTagMemWords(filter, MemBank.TID, 0, 2);
                epc2 = mordr.ReadTagMemWords(filter, MemBank.EPC, 1, 1);
                pwd = mordr.ReadTagMemWords(filter, MemBank.RESERVED, 0, 2);
            }
            catch (OpFaidedException notagexp)
            {
                if (notagexp.ErrCode == 0x400)
                {
                    MessageBox.Show("没法发现标签");
                }
                else
                {
                    MessageBox.Show("操作失败:" + notagexp.ToString());
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败:" + ex.ToString());
                return;
            }
            string epc2Tostring = TostringX4(epc2);
            string pwd2Tostring = TostringX4(pwd);

            string tid = TostringX4(readdata);

            var _EpcLength = setEpcLength(epc2Tostring);

            string ic = getIcString(tid);
            TidManufacturers.Text = ic;
            TidText.Text = tid;
            DestroyPassword.Text = pwd2Tostring;

           // EpcLength.Text = $"{_EpcLength * 16}bit({_EpcLength}块)";
            EpcLength.Text = (_EpcLength * 16)+"bit("+ _EpcLength + "块)";

            EpcText(filter, _EpcLength);
            UserText(filter);
            VisitPasswordText(filter);
        }

        public void EpcText(Gen2TagFilter filter, int EpcLength)
        {
            ushort[] epc = null;
            try
            {
                epc = mordr.ReadTagMemWords(filter, MemBank.EPC, 2, EpcLength);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败:" + ex.ToString());
                return;
            }
            var epctostring = TostringX4(epc);
            Epc.Text = epctostring;
        }

        public void VisitPasswordText(Gen2TagFilter filter)
        {
            ushort[] value = null;
            try
            {
                value = mordr.ReadTagMemWords(filter, MemBank.RESERVED, 2, 2);
            }
            catch (Exception)
            {
            }
            if (value != null)
            {
                var epctostring = TostringX4(value);
                VisitPassword.Text = epctostring;
            }
        }

        public void UserText(Gen2TagFilter filter)
        {
            int Ierror = 0;
            ushort[] value = null;
            //获取user最大 长度(位),
            for (int i = 1; i < 100; i++)
            {
                try
                {
                    value = mordr.ReadTagMemWords(filter, MemBank.USER, 0, i);
                }
                catch (Exception)
                {
                    Ierror = i - 1;
                    i = 100;
                }
            }
            //防止意外错误，重复几次
            for (int i = Ierror; i < 100; i++)
            {
                try
                {
                    value = mordr.ReadTagMemWords(filter, MemBank.USER, 0, i);
                }
                catch (Exception)
                {
                    Ierror = i - 1;
                    i = 100;
                }
            }
            //防止意外错误，重复几次
            for (int i = Ierror; i < 100; i++)
            {
                try
                {
                    value = mordr.ReadTagMemWords(filter, MemBank.USER, 0, i);
                }
                catch (Exception)
                {
                    Ierror = i - 1;
                    i = 100;
                }
            }
            if (Ierror > 0)
            {
                //UserLength.Text = $"{Ierror * 16}bit({Ierror}块)";
                UserLength.Text = (Ierror * 16)+"bit("+Ierror+"块)";
            }
        }

        //判断是否已经设置了操作单天线
        private int IsAntSet()
        {
            int ret = -1;
            if (Mfrm.readerantnumber < 8)
            {
                if (cbb16opant.SelectedIndex != -1)
                {
                    mordr.ParamSet("TagopAntenna", cbb16opant.SelectedIndex + 1);
                    ret = 0;
                }
            }
            else
            {
                mordr.ParamSet("TagopAntenna", cbb16opant.SelectedIndex);
                ret = 0;
            }
            return ret;
        }

        private string getIcString(string tid)
        {
            if (tid == null)
            {
                return "";
            }

            string ret = null;
            string manufactureid = tid.Substring(2, 3);
            switch (manufactureid)
            {
                case "006":
                    ret = "NXP";
                    break;
                case "003":
                    {
                        ret = "ALIEN";
                        string icmodle = tid.Substring(5, 3);
                        if (icmodle == "412")
                        {
                            ret += "-Higgs 3";
                        }
                        else if (icmodle == "411")
                        {
                            ret += "-Higgs 2";
                        }

                        break;
                    }
                case "001":
                    ret = "IMPINJ";
                    break;
                case "801":
                    {
                        ret = "IMPINJ";
                        string icmodle = tid.Substring(5, 3);
                        if (icmodle == "105")
                        {
                            ret += "-Monza 4";
                        }
                        else if (icmodle == "130")
                        {
                            ret += "-Monza 5";
                        }

                        break;
                    }
                case "00F":
                    ret = "坤锐";
                    break;
                default:
                    ret = "";
                    break;
            }
            return ret;
        }


        /// <summary>
        /// 获取Epc的长度
        /// </summary>
        /// <param name="Epc2">Epc第二块的数据(16进制)</param>
        /// <returns></returns>
        public int setEpcLength(string epc2)
        {
            var epcTo2 = HexString2BinString(epc2);//将Epc第二块转2进制
            var epcLength = Convert.ToInt32(epcTo2.Substring(0, 5), 2);//将epcTo2前5位转10进制

            return epcLength;
        }

        /// <summary>
        /// 16进制转2进制
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public string HexString2BinString(string hexString)
        {
            string result = string.Empty;
            foreach (char c in hexString)
            {
                int v = Convert.ToInt32(c.ToString(), 16);
                int v2 = int.Parse(Convert.ToString(v, 2));
                // 去掉格式串中的空格，即可去掉每个4位二进制数之间的空格，
                result += string.Format("{0:d4}", v2);
            }
            return result;
        }

        public string TostringX4(ushort[] value)
        {
            string valueTotsring = "";
            for (int i = 0; i < value.Length; ++i)
            {
                valueTotsring += value[i].ToString("X4");
            }

            return valueTotsring;
        }
    }
}
