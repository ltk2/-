using ModuleTech;
using ModuleTech.Gen2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    public partial class InventoryParasform : Form
    {
        Form1 frm1 = null;
        public InventoryParasform(Form1 frm)
        {
            frm1 = frm;
            InitializeComponent();
        }

        public class ComBoBoxItem
        {
            public ComBoBoxItem(string _Name, int _Value)
            {
                Name = _Name;
                Value = _Value;
            }
            public string Name { get; set; }
            public int Value { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int rdur = 0;
            int sdur = 0;
            int reconcnt = 0;
            int reconinterval = 0;
            //added on 3-26
           
            //frm1.rParms.isUniByEmd = cbisunibynullemd.Checked;
            //frm1.rParms.isChangeColor = cbischgcolor.Checked;
            //frm1.rParms.isUniByAnt = cbisunibyant.Checked;
            try
            {
                rdur = int.Parse(tbreaddur.Text.Trim());
                sdur = int.Parse(tbsleepdur.Text.Trim());
            }
            catch (Exception)
            {
                MessageBox.Show("读时长和读间隔只能输入整数，单位为毫秒");
                return;
            }
            
            //try
            //{
            //    if (textBox_stopsec.Text!="")
            //    {
            //        frm1.rParms.stopsec = int.Parse(textBox_stopsec.Text);
            //    }
            //}
            //catch (System.Exception)
            //{
            //    MessageBox.Show("停止时间参数有误");
            //    return;
            //}

            try
            {
                reconcnt = int.Parse(tbreconcnt.Text.Trim());
                reconinterval = int.Parse(tbreconinterval.Text.Trim());
                if (AntForTime.Text != "")
                    frm1.rParms.antForTime = Convert.ToInt32(AntForTime.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("重连次数和连接间隔只能输入整数");
                return;
            }

            frm1.rParms.readdur = rdur;
            frm1.rParms.sleepdur = sdur;

            frm1.rParms.reconnectcnt = reconcnt;
            frm1.rParms.connectinterval = reconinterval;

            //是否启用过滤
            if (cbisfilter.Checked)
            {
                if (tbfilterdata.Text != "") { 
                    bool ret = Form1.IsHexString(tbfilterdata.Text.ToString());
                    if (!ret)
                    {
                        MessageBox.Show("匹配数据只能是16进制字符串");
                        return;
                    }
                }

                if (cbbfilterbank.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择过滤bank");
                    return;
                }

                if (cbbfilterrule.SelectedIndex == -1)
                {
                    MessageBox.Show("请输入过滤规则");
                    return;
                }

                int bitaddr = 0;
                if (tbfilteraddr.Text.Trim() == "")
                {
                    MessageBox.Show("请输入过滤bank的起始地址,以字为最小单位");
                    return;
                }
                else
                {
                    try
                    {
                        bitaddr = int.Parse(tbfilteraddr.Text.Trim());
                    }
                    catch (Exception)
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
                var Mask = tbfilterdata.Text.Trim();
                     string hexstr = Mask.Trim();
                if (hexstr.Length % 2 != 0)
                    hexstr += "0";

                var  filterbytes = ByteFormat.FromHex(hexstr);
                var  bitlen = Mask.Trim().Length * 4;

                //设置过滤项
                //Gen2TagFilter filter = new Gen2TagFilter(tbfilterdata.Text.Trim().Length, filterbytes,
                //    (MemBank)cbbfilterbank.SelectedIndex + 1, bitaddr,
                //    cbbfilterrule.SelectedIndex == 0 ? false : true);

                Gen2TagFilter filter = new Gen2TagFilter(bitlen, filterbytes, (MemBank)cbbfilterbank.SelectedIndex + 1,
                bitaddr, cbbfilterrule.SelectedIndex == 0 ? false : true);

                frm1.modulerdr.ParamSet("Singulation", filter);
            }
            else
            {
                frm1.modulerdr.ParamSet("Singulation", null);//清空过滤项
            }

            //设置附加数据项
            //|| frm1.modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_R902_MT100
            if (!(frm1.rParms.readertype == ReaderType.PR_ONEANT))
            {
                if (cbisaddiondata.Checked)
                {
                    uint wordaddr = 0;
                    int ebbytescnt;

                    if (tbebstartaddr.Text.Trim() == "")
                    {
                        MessageBox.Show("请输入附加数据bank的起始地址,以字为最小单位");
                        return;
                    }
                    else
                    {
                        try
                        {
                            wordaddr = uint.Parse(tbebstartaddr.Text.Trim());
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("起始地址请输入数字");
                            return;
                        }
                        if (wordaddr < 0)
                        {
                            MessageBox.Show("地址必须大于零");
                            return;
                        }
                    }

                    if (tbebbytescnt.Text.Trim() == "")
                    {
                        MessageBox.Show("请输入附加数据bank的起始地址,以字为最小单位");
                        return;
                    }
                    else
                    {
                        try
                        {
                            ebbytescnt = int.Parse(tbebbytescnt.Text.Trim());
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("附加数据字节数请输入数字");
                            return;
                        }
                        if (ebbytescnt < 0 || ebbytescnt > 128)
                        {
                            MessageBox.Show("附加数据字节数必须大于零且小于等于128");
                            return;
                        }
                    }

                    if (cbbebbank.SelectedIndex == -1)
                    {
                        MessageBox.Show("请选择附加数据的bank");
                        return;
                    }

                    EmbededCmdData ecd = new EmbededCmdData((MemBank)cbbebbank.SelectedIndex, wordaddr,
                        (byte)ebbytescnt);

                    frm1.modulerdr.ParamSet("EmbededCmdOfInventory", ecd);
                }
                else
                {
                    frm1.modulerdr.ParamSet("EmbededCmdOfInventory", null);
                }

                //设置密码项
                if (cbispwd.Checked)
                {
                    int ret = Form1.IsValidPasswd(tbacspwd.Text.Trim());
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
                        uint passwd = uint.Parse(tbacspwd.Text.Trim(), System.Globalization.NumberStyles.AllowHexSpecifier);
                        frm1.modulerdr.ParamSet("AccessPassword", passwd);
                    }
                }
                else
                {
                    frm1.modulerdr.ParamSet("AccessPassword", (uint)0);
                }
                //     if (frm1.modulerdr.HwDetails.board != Reader.MaindBoard_Type.MAINBOARD_ARM9)
                //     {
            }

            //设置gpi触发模式
            if (cbenabletrigger.Checked)
            {
                if (cbbtriggermode.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择触发模式");
                    return;
                }

                if (cbbtriggermode.SelectedIndex != 0)
                {
                    if (tbtimeoutstop.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("请输入超时停止时间");
                        return;
                    }
                }
                List<GPIState> gpitris1 = new List<GPIState>();
                if (cbcon1gpi1.Checked)
                {
                    gpitris1.Add(new GPIState(1, true));
                }
                else
                {
                    gpitris1.Add(new GPIState(1, false));
                }

                if (cbcon1gpi2.Checked)
                {
                    gpitris1.Add(new GPIState(2, true));
                }
                else
                {
                    gpitris1.Add(new GPIState(2, false));
                }

                if (cbcon1gpi3.Checked)
                {
                    gpitris1.Add(new GPIState(3, true));
                }
                //else
                //{
                //    gpitris1.Add(new GPIState(3, false));
                //}

                if (cbcon1gpi4.Checked)
                {
                    gpitris1.Add(new GPIState(4, true));
                }
                //else
                //{
                //    gpitris1.Add(new GPIState(4, false));
                //}

                GPITrigger gpitrigger = new GPITrigger();
                gpitrigger.TriggerType = (GPITriggerType)(cbbtriggermode.SelectedIndex + 1);
                gpitrigger.Trigger1State = gpitris1.ToArray();

                if (cbbtriggermode.SelectedIndex != 1)
                {
                    List<GPIState> gpitris2 = new List<GPIState>();
                    if (cbcon2gpi1.Checked)
                    {
                        gpitris2.Add(new GPIState(1, true));
                    }
                    else
                    {
                        gpitris2.Add(new GPIState(1, false));
                    }

                    if (cbcon2gpi2.Checked)
                    {
                        gpitris2.Add(new GPIState(2, true));
                    }
                    else
                    {
                        gpitris2.Add(new GPIState(2, false));
                    }

                    if (cbcon2gpi3.Checked)
                    {
                        gpitris2.Add(new GPIState(3, true));
                    }
                    //else
                    //{
                    //    gpitris2.Add(new GPIState(3, false));
                    //}

                    if (cbcon2gpi4.Checked)
                    {
                        gpitris2.Add(new GPIState(4, true));
                    }
                    //else
                    //{
                    //    gpitris2.Add(new GPIState(4, false));
                    //}

                    gpitrigger.Trigger2State = gpitris2.ToArray();
                }

                if (cbbtriggermode.SelectedIndex != 0)
                {
                    gpitrigger.ReadTimeout = int.Parse(tbtimeoutstop.Text.Trim());
                }

                frm1.rParms.GpiTriiger = gpitrigger;
            }
            else
            {
                frm1.rParms.GpiTriiger = null;
            }

            //设置异步高速读取返回标签项
            frm1.rParms.FRTMeta.IsAntennaID = cbasyantid.Checked;
            frm1.rParms.FRTMeta.IsEmdData = cbasyemd.Checked;
            frm1.rParms.FRTMeta.IsTimestamp = cbasytm.Checked;
            frm1.rParms.FRTMeta.IsFrequency = cbasyfre.Checked;
            frm1.rParms.FRTMeta.IsRFU = cbasyrfu.Checked;
            frm1.rParms.FRTMeta.IsRSSI = cbasyrssi.Checked;
            frm1.rParms.FRTMeta.IsReadCnt = cbasyreadcnt.Checked;
            //frm1.rParms.isFastRead = cbfastread.Checked;
            frm1.rParms.is1200hdstmd = checkBox_1200handsetmode.Checked;
            frm1.rParms.istagfoucs = checkBox_tagfoucs.Checked;
            frm1.rParms.isfastid = checkBox_fastid.Checked;

            //frm1.rParms.isLogTime = checkBox1.Checked;
            //frm1.rParms.logTime = Convert.ToInt32(textBox1.Text);
            
            //frm1.rParms.isEx10 = Ex10Celerity.Checked;
            //ComBoBoxItem item = this.CelerityType.SelectedItem as ComBoBoxItem;
            //frm1.rParms.isCelerityType = item.Value;
            //Ex10FastModeParams ex10 = Ex10Celerity.Checked ? new Ex10FastModeParams() : null;
            //frm1.modulerdr.ParamSet("Ex10FastModeParams", ex10);//清空过滤项
            Close();
        }

        private void InventoryParasform_Load(object sender, EventArgs e)
        {
            if (frm1.rParms.GpiTriiger != null)
            {
                cbenabletrigger.Checked = true;
                cbcon1gpi1.Checked = (frm1.rParms.GpiTriiger.Trigger1State[0].State);
                cbcon1gpi2.Checked = (frm1.rParms.GpiTriiger.Trigger1State[1].State);
                if (frm1.rParms.GpiTriiger.Trigger1State.Length > 2) {
                    cbcon1gpi3.Checked = (frm1.rParms.GpiTriiger.Trigger1State[2].State);
                    cbcon1gpi4.Checked = (frm1.rParms.GpiTriiger.Trigger1State[3].State);
                }
                
                cbbtriggermode.SelectedIndex = (int)frm1.rParms.GpiTriiger.TriggerType - 1;
                if (frm1.rParms.GpiTriiger.TriggerType != GPITriggerType.GPITrigger_Tri1Start_TimeoutStop)
                {
                    cbcon2gpi1.Checked = (frm1.rParms.GpiTriiger.Trigger2State[0].State);
                    cbcon2gpi2.Checked = (frm1.rParms.GpiTriiger.Trigger2State[1].State);
                    if (frm1.rParms.GpiTriiger.Trigger2State.Length > 2)
                    {
                        cbcon2gpi3.Checked = (frm1.rParms.GpiTriiger.Trigger2State[2].State);
                        cbcon2gpi4.Checked = (frm1.rParms.GpiTriiger.Trigger2State[3].State);
                    }
                    tbtimeoutstop.Text = string.Empty;
                }
                if (frm1.rParms.GpiTriiger.TriggerType != GPITriggerType.GPITrigger_Tri1Start_Tri2Stop)
                {
                    tbtimeoutstop.Text = frm1.rParms.GpiTriiger.ReadTimeout.ToString();
                }
            }

            cbasyantid.Checked = frm1.rParms.FRTMeta.IsAntennaID;
            cbasyemd.Checked = frm1.rParms.FRTMeta.IsEmdData;
            cbasytm.Checked = frm1.rParms.FRTMeta.IsTimestamp;
            cbasyfre.Checked = frm1.rParms.FRTMeta.IsFrequency;
            cbasyrfu.Checked = frm1.rParms.FRTMeta.IsRFU;
            cbasyrssi.Checked = frm1.rParms.FRTMeta.IsRSSI;
            cbasyreadcnt.Checked = frm1.rParms.FRTMeta.IsReadCnt;
           
            object obj = null;
            if (!(frm1.modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E ||
                frm1.modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC))
            {
                //gbpotlweight.Enabled = false;
            }
           

            obj = frm1.modulerdr.ParamGet("EmbededCmdOfInventory");
            if (obj != null)
            {
                EmbededCmdData ecd = (EmbededCmdData)obj;
                tbebstartaddr.Text = ecd.StartAddr.ToString();
                tbebbytescnt.Text = ecd.ByteCnt.ToString();
                cbbebbank.SelectedIndex = (int)ecd.Bank;
                cbisaddiondata.Checked = true;
            }
            uint pwd = (uint)frm1.modulerdr.ParamGet("AccessPassword");
            if (pwd != 0)
            {
                cbispwd.Checked = true;
                tbacspwd.Text = pwd.ToString("X8");
            }

            obj = frm1.modulerdr.ParamGet("Singulation");
            if (obj != null)
            {
                Gen2TagFilter filter = (Gen2TagFilter)obj;
                cbbfilterbank.SelectedIndex = filter.FilterBank - 1;
                tbfilteraddr.Text = filter.FilterAddress.ToString();
                //string binarystr = "";
                var binarystr = BytesToHexString(filter.FilterData);
                
                tbfilterdata.Text = binarystr;

                if (filter.IsInvert)
                    cbbfilterrule.SelectedIndex = 1;
                else
                    cbbfilterrule.SelectedIndex = 0;

                cbisfilter.Checked = true;
            }
            tbreaddur.Text = frm1.rParms.readdur.ToString();
            tbsleepdur.Text = frm1.rParms.sleepdur.ToString();

            tbreconcnt.Text = frm1.rParms.reconnectcnt.ToString();
            tbreconinterval.Text = frm1.rParms.connectinterval.ToString();

            checkBox_1200handsetmode.Checked = frm1.rParms.is1200hdstmd;
            checkBox_tagfoucs.Checked = frm1.rParms.istagfoucs;
            checkBox_fastid.Checked = frm1.rParms.isfastid;
            //checkBox_stoptime.Checked = frm1.rParms.istopbytime;
            //textBox_stopsec.Text = frm1.rParms.stopsec.ToString();
            AntForTime.Text = frm1.rParms.antForTime.ToString();

        }
        
        private void button_mulselect_Click(object sender, EventArgs e)
        {
            List<string> epcs = new List<string>();

            Form_mulselect frm = new Form_mulselect(frm1.modulerdr, frm1.rParms, frm1.Epcs);
            frm.ShowDialog();
        }
        
        public static string BytesToHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
                return "";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //bool _checked = checkBox1.Checked;
            //label16.Visible = _checked;
            //label17.Visible = _checked;
            //textBox1.Visible = _checked;
        }

        private void cbbtriggermode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbtriggermode.SelectedIndex == 0)
            {
                tbtimeoutstop.Enabled = false;
                cbcon2gpi1.Enabled = true;
                cbcon2gpi2.Enabled = true;
                cbcon2gpi3.Enabled = true;
                cbcon2gpi4.Enabled = true;
            }
            else if (cbbtriggermode.SelectedIndex == 1)
            {
                tbtimeoutstop.Enabled = true;
                cbcon2gpi1.Enabled = false;
                cbcon2gpi2.Enabled = false;
                cbcon2gpi3.Enabled = false;
                cbcon2gpi4.Enabled = false;
            }
            else if (cbbtriggermode.SelectedIndex == 2)
            {
                tbtimeoutstop.Enabled = true;
                cbcon2gpi1.Enabled = true;
                cbcon2gpi2.Enabled = true;
                cbcon2gpi3.Enabled = true;
                cbcon2gpi4.Enabled = true;
            }
        }
    }
}