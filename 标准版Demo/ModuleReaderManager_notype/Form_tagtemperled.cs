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
    public partial class Form_tagtemperled : Form
    {
 
        public Form_tagtemperled(Reader rdr, ReaderParams param, Form1 frm)
        {
            InitializeComponent();
            mordr = rdr;
            rparam = param;
            Mfrm = frm;
        }
        ReaderParams rparam = null;
        Reader mordr = null;
        Form1 Mfrm = null;
        public List<string> lepcs = new List<string>();
        //判断是否有效16进制字符串
        public static int IsValidHexstr(string str, int len)
        {
            if (str == "")
                return -3;
            if (str.Length % 4 != 0)
                return -2;
            if (str.Length > len)
                return -4;
            string lowstr = str.ToLower();
            byte[] hexchars = Encoding.ASCII.GetBytes(lowstr);

            foreach (byte a in hexchars)
            {
                if (!((a >= 48 && a <= 57) || (a >= 97 && a <= 102)))
                    return -1;
            }
            return 0;
        }

        //判断是否有效地址
        private bool IsValidAddr(string addr, int bank, int rorw)
        {
            if (addr == "")
                return false;

            int addr_;
            try
            {
                addr_ = int.Parse(addr,System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception exxx)
            {
                return false;
            }

            switch (bank)
            {
                case 0:
                    {
                        if (addr_ >= 0 && addr_ <= 3)
                            return true;
                        break;
                    }
                case 1:
                    {
                        if (rorw == 2)
                        {
                            if (addr_ >= 2 && addr_ <= 32)
                                return true;
                        }
                        else if (rorw == 1)
                        {
                            if (addr_ >= 0 && addr_ <= 32)
                                return true;
                        }
                        break;
                    }
                case 2:
                    {
                        if (addr_ >= 0)
                            return true;
                        break;
                    }
                case 3:
                    {
                        if (addr_ >= 0 && addr_ <= 8000)
                            return true;
                        break;
                    }

            }

            return false;
        }

        //判断是否有效块数
        private bool IsValidCnt(string cnt, int bank, string addr)
        {
            if (cnt == "")
                return false;

            int addr_;
            int cnt_;
            try
            {
                addr_ = int.Parse(addr, System.Globalization.NumberStyles.HexNumber);
                cnt_ = int.Parse(cnt);
            }
            catch (Exception exx)
            {
                return false;
            }

            int sum = addr_ + cnt_;
            switch (bank)
            {
                case 0:
                    {
                        if (sum <= 4)
                            return true;
                        break;
                    }
                case 1:
                    {
                        if (sum <= 64)
                            return true;
                        break;
                    }
                case 2:
                    {
                        if (sum <= 16)
                            return true;
                        break;
                    }
                case 3:
                    {
                        if (sum <= 8000)
                            return true;
                        break;
                    }
            }
            return false;
        }
        Dictionary<int, RadioButton> allants = new Dictionary<int, RadioButton>();
        //判断是否已经设置了操作单天线
        private int IsAntSet()
        {
            int ret = -1;
            if (Mfrm.readerantnumber >= 8)
            {
                if (cbb16opant.SelectedIndex != -1)
                {
                    mordr.ParamSet("TagopAntenna", cbb16opant.SelectedIndex + 1);
                    ret = 0;
                }
            }
            else
            {
                for (int i = 1; i <= allants.Count; ++i)
                {
                    if (allants[i].Checked)
                    {
                        mordr.ParamSet("TagopAntenna", i);
                        //if (isSetRp)
                        //{
                        //    mordr.ParamSet("ReadPlan", new SimpleReadPlan(new int[] { i }));
                        //}
                        if (allants[i].ForeColor == Color.Red)
                            ret = 1;
                        else
                            ret = 0;
                    }
                }
            }
            return ret;
        }

        private void btnread_Click(object sender, EventArgs e)
        {
            int ret;
            Gen2TagFilter filter = null;
           
            if (this.cbbopbank.SelectedIndex == -1)
            {
                MessageBox.Show("请选择bank");
                return;
            }

            if (!IsValidAddr(this.tbstartaddr.Text.Trim(), this.cbbopbank.SelectedIndex, 1))
            {
                MessageBox.Show("所设置的起始地址超过了对应bank的范围");
                return;
            }

            if (!IsValidCnt(this.tbblocks.Text.Trim(), this.cbbopbank.SelectedIndex, this.tbstartaddr.Text))
            {
                MessageBox.Show("所设置的块数超过了对应的bank和起始地址的范围");
                return;
            }

            if (this.cbisaccesspasswd.Checked)
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
                        return;
                }
            }

            if (this.cbisfilter.Checked)
            {

                ret = Form1.IsValidBinaryStr(this.tbfldata.Text.Trim());
                switch (ret)
                {
                    case -3:
                        MessageBox.Show("匹配数据不能为空");
                        break;
                    case -1:
                        MessageBox.Show("匹配数据只能是二进制字符串");
                        break;

                }

                if (ret != 0)
                    return;
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

                byte[] filterbytes = new byte[(this.tbfldata.Text.Trim().Length - 1) / 8 + 1];
                for (int c = 0; c < filterbytes.Length; ++c)
                    filterbytes[c] = 0;

                int bitcnt = 0;
                foreach (Char ch in this.tbfldata.Text.Trim())
                {
                    if (ch == '1')
                        filterbytes[bitcnt / 8] |= (byte)(0x01 << (7 - bitcnt % 8));
                    bitcnt++;

                }

                filter = new Gen2TagFilter(this.tbfldata.Text.Trim().Length, filterbytes,
                    (MemBank)this.cbbfilterbank.SelectedIndex + 1, bitaddr,
                    this.cbbfilterrule.SelectedIndex == 0 ? false : true);
            }

            ushort metaflag = 0;
            if (cbasyreadcnt.Checked)
                metaflag |= 0x1;
            if (cbasyrssi.Checked)
                metaflag |= 0x1 << 1;
            if (cbasyantid.Checked)
                metaflag |= 0x1 << 2;
            if (cbasyfre.Checked)
                metaflag |= 0x1 << 3;
            if (cbasytm.Checked)
                metaflag |= 0x1 << 4;
            if (cbasyrfu.Checked)
                metaflag |= 0x1 << 5;
            if (cbasypro.Checked)
                metaflag |= 0x1 << 6;
            if (cbasyemd.Checked)
                metaflag |= 0x1 << 7;
            try
            {
                //读温度数据的区，地址，起始块，块数，总超时时间，select超时（温度标签厂商的超时），读标签等待超时，metaflag标志，这个总超时等于后面两个超时加上通信等待时间，
                int alltime = int.Parse(this.textBox_optime.Text) + int.Parse(this.textBox_selwait.Text) + int.Parse(textBox_readwait.Text);
               R2000_calibration.Tagtemperture_DATA readdata = mordr.ReadTagTemperture(filter,
                    (MemBank)this.cbbopbank.SelectedIndex, int.Parse(this.tbstartaddr.Text.Trim(),System.Globalization.NumberStyles.HexNumber)
                    , int.Parse(this.tbblocks.Text.Trim()), alltime, int.Parse(this.textBox_selwait.Text), int.Parse(textBox_readwait.Text),metaflag);

               bool ishave = false;
               for (int i = 0; i < this.lvTags.Items.Count; i++)
               {
                   if (this.lvTags.Items[i].SubItems[0].Text == ByteFormat.ToHex(readdata.TagEpc))
                   {
                       this.lvTags.Items[i].SubItems[1].Text = (int.Parse(this.lvTags.Items[i].SubItems[1].Text) + readdata.ReadCount).ToString();
                       this.lvTags.Items[i].SubItems[2].Text = readdata.Antenna.ToString();

                       string tempet = "";
                       if ((readdata.Data[0] & 0x80) == 0)
                       {
                          tempet=(readdata.Data[0]-30) + "." + readdata.Data[1] * 100 / 255;
                       }
                       else
                       {
                           readdata.Data[0] = (byte)(~readdata.Data[0]); readdata.Data[1] = (byte)(~readdata.Data[1] + 1);
                           tempet = "-"+(readdata.Data[0]-30) + "." + readdata.Data[1] * 100 / 255;
                       }
                       this.lvTags.Items[i].SubItems[3].Text = ByteFormat.ToHex(readdata.Data) + "(" + tempet + "℃)";
                       this.lvTags.Items[i].SubItems[4].Text = "Gen2";
                       this.lvTags.Items[i].SubItems[5].Text = readdata.Lqi.ToString();
                       this.lvTags.Items[i].SubItems[6].Text = readdata.Frequency.ToString();
                       this.lvTags.Items[i].SubItems[7].Text = readdata.Phase.ToString();
                                        
                       ishave = true;
                       break;
                   }
               }

               if (!ishave)
               {
                   ListViewItem lvi = new ListViewItem(ByteFormat.ToHex(readdata.TagEpc));
                   lvi.SubItems.Add(readdata.ReadCount.ToString());
                   lvi.SubItems.Add(readdata.Antenna.ToString());
                   string tempet = "";
                   if ((readdata.Data[0] & 0x80) == 0)
                   {
                       tempet = (readdata.Data[0] - 30) + "." + readdata.Data[1] * 100 / 255;
                   }
                   else
                   {
                       readdata.Data[0] = (byte)(~readdata.Data[0]); readdata.Data[1] = (byte)(~readdata.Data[1] + 1);
                       tempet = "-" + (readdata.Data[0]-30) + "." + readdata.Data[1] * 100 / 255;
                   }
                   lvi.SubItems.Add(ByteFormat.ToHex(readdata.Data) + "(" + tempet + "℃)");
                   lvi.SubItems.Add("Gen2");
                   lvi.SubItems.Add(readdata.Lqi.ToString());
                   lvi.SubItems.Add(readdata.Frequency.ToString());
                   lvi.SubItems.Add(readdata.Phase.ToString());
                   this.lvTags.Items.Add(lvi);
                   this.lvTags.Refresh();
               }

               for (int i = 0; i < this.lvTags.Items.Count; i++)
               {
                   if(!lepcs.Contains(this.lvTags.Items[i].Text))
                       lepcs.Add(this.lvTags.Items[i].Text);
               }
            }
            catch (OpFaidedException notagexp)
            {
                if (notagexp.ErrCode == 0x400)
                    MessageBox.Show("没法发现标签");
                else
                    MessageBox.Show("操作失败:" + notagexp.ErrCode.ToString("X4"));

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败:" + ex.Message);
                return;
            }
 
        }

        private void Form_tagtemperled_Load(object sender, EventArgs e)
        {
            if (Mfrm.readerantnumber >= 8)
            {
                cbb16opant.Items.Clear();
                for (int i = 0; i < Mfrm.readerantnumber; ++i)
                    cbb16opant.Items.Add((i + 1).ToString());
                lab16devtip.Visible = true;
                cbb16opant.Visible = true;
            }
            allants.Add(1, rbant1);
            allants.Add(2, rbant2);
            allants.Add(3, rbant3);
            allants.Add(4, rbant4);

            bool isselect = false;
            for (int i = 1; i <= allants.Count; ++i)
                allants[i].Enabled = false;
            for (int j = 0; j < rparam.AntsState.Count; ++j)
            {
                allants[rparam.AntsState[j].antid].Enabled = true;
                if (rparam.AntsState[j].isConn)
                {
                    allants[rparam.AntsState[j].antid].ForeColor = Color.Green;
                    if (!isselect)
                        allants[rparam.AntsState[j].antid].Checked = true;
                }
                else
                    allants[rparam.AntsState[j].antid].ForeColor = Color.Red;
            }

            if (rparam.readertype == ReaderType.PR_ONEANT)
            {
                this.tbfilteraddr.Enabled = false;
                this.tbfldata.Enabled = false;
                this.cbbfilterbank.Enabled = false;
                this.cbbfilterrule.Enabled = false;
                this.cbisfilter.Enabled = false;
            }

            mordr.ParamSet("TagopProtocol", TagProtocol.GEN2);
            cbbopbank.SelectedIndex = 3;
        }
 
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.lvTags.Items.Clear();
            this.lepcs.Clear();
        }

        ushort metaflag = 0;
        Gen2TagFilter filter = null;
        bool isctrlledtime = false;
        int t_ivl;
        private void button_readled_Click(object sender, EventArgs e)
        {
            if (button_readled.Text == "读")
            {
                int ret;
                filter = null;

                if (this.cbisaccesspasswd.Checked)
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
                            return;
                    }
                }

                if (this.cbisfilter.Checked)
                {

                    ret = Form1.IsValidBinaryStr(this.tbfldata.Text.Trim());
                    switch (ret)
                    {
                        case -3:
                            MessageBox.Show("匹配数据不能为空");
                            break;
                        case -1:
                            MessageBox.Show("匹配数据只能是二进制字符串");
                            break;

                    }

                    if (ret != 0)
                        return;
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

                    byte[] filterbytes = new byte[(this.tbfldata.Text.Trim().Length - 1) / 8 + 1];
                    for (int c = 0; c < filterbytes.Length; ++c)
                        filterbytes[c] = 0;

                    int bitcnt = 0;
                    foreach (Char ch in this.tbfldata.Text.Trim())
                    {
                        if (ch == '1')
                            filterbytes[bitcnt / 8] |= (byte)(0x01 << (7 - bitcnt % 8));
                        bitcnt++;

                    }

                    filter = new Gen2TagFilter(this.tbfldata.Text.Trim().Length, filterbytes,
                        (MemBank)this.cbbfilterbank.SelectedIndex + 1, bitaddr,
                        this.cbbfilterrule.SelectedIndex == 0 ? false : true);
                }

               
                metaflag = 0;
                if (cbasyreadcnt.Checked)
                    metaflag |= 0x1;
                if (cbasyrssi.Checked)
                    metaflag |= 0x1 << 1;
                if (cbasyantid.Checked)
                    metaflag |= 0x1 << 2;
                if (cbasyfre.Checked)
                    metaflag |= 0x1 << 3;
                if (cbasytm.Checked)
                    metaflag |= 0x1 << 4;
                if (cbasyrfu.Checked)
                    metaflag |= 0x1 << 5;
                if (cbasypro.Checked)
                    metaflag |= 0x1 << 6;
                if (cbasyemd.Checked)
                    metaflag |= 0x1 << 7;
               

                isctrlledtime = checkBox_ledtime.Checked;

                if (isctrlledtime)
                {
                    //指定控制亮灯时间，配置metaflag|=0x8000;
                    metaflag |= 0x8000;

                    mordr.ParamSet("OpTimeout", (ushort)(int.Parse(this.textBox_ledlightt.Text) + int.Parse(this.textBox_optime.Text)));
                }
                else
                    mordr.ParamSet("OpTimeout", (ushort)1000);
                System.Diagnostics.Debug.WriteLine("start Time:" + DateTime.Now.Second.ToString());

                t_ivl = int.Parse(textBox_ledtime.Text);

                //timer1.Start();
                if (isctrlledtime)//指定控制亮灯时间时，超时计算方法
                    t_timeout = ((int.Parse(this.textBox_ledlightt.Text) / 100) << 8) | int.Parse(this.textBox_optime.Text) / 100;
                else
                    t_timeout = int.Parse(this.textBox_optime.Text);

                t_isrun = true;
                t_thread = new System.Threading.Thread(new System.Threading.ThreadStart(thread_run));
                t_thread.Start();
               

                button_readled.Text = "停";
            }
            else
            {
                //timer1.Stop();
                t_isrun = false;
                if (iar != null)
                    this.EndInvoke(iar);

                if (t_thread != null)
                    t_thread.Join();

                button_readled.Text = "读";
            }
        }

        System.Threading.Thread t_thread;
        bool t_isrun;
        int t_timeout;
        delegate void handleshow1(R2000_calibration.TagLED_DATA readdata);
        delegate void handleshow2(string mess);
        IAsyncResult iar;
        private void showmess1(R2000_calibration.TagLED_DATA readdata)
        {
            bool ishave = false;
            for (int i = 0; i < this.lvTags.Items.Count; i++)
            {
                if (this.lvTags.Items[i].SubItems[0].Text == ByteFormat.ToHex(readdata.TagEpc))
                {
                    this.lvTags.Items[i].SubItems[1].Text = (int.Parse(this.lvTags.Items[i].SubItems[1].Text) + readdata.ReadCount).ToString();
                    this.lvTags.Items[i].SubItems[2].Text = readdata.Antenna.ToString();

                    if (readdata.Data != null)
                        this.lvTags.Items[i].SubItems[3].Text = ByteFormat.ToHex(readdata.Data);
                    else
                        this.lvTags.Items[i].SubItems[3].Text = "";
                    this.lvTags.Items[i].SubItems[4].Text = "Gen2";
                    this.lvTags.Items[i].SubItems[5].Text = readdata.Lqi.ToString();
                    this.lvTags.Items[i].SubItems[6].Text = readdata.Frequency.ToString();
                    this.lvTags.Items[i].SubItems[7].Text = readdata.Phase.ToString();

                    ishave = true;
                    break;
                }
            }

            if (!ishave)
            {
                ListViewItem lvi = new ListViewItem(ByteFormat.ToHex(readdata.TagEpc));
                lvi.SubItems.Add(readdata.ReadCount.ToString());
                lvi.SubItems.Add(readdata.Antenna.ToString());
                if (readdata.Data != null)
                    lvi.SubItems.Add(ByteFormat.ToHex(readdata.Data));
                else
                    lvi.SubItems.Add("");
                lvi.SubItems.Add("Gen2");
                lvi.SubItems.Add(readdata.Lqi.ToString());
                lvi.SubItems.Add(readdata.Frequency.ToString());
                lvi.SubItems.Add(readdata.Phase.ToString());
                this.lvTags.Items.Add(lvi);
                this.lvTags.Refresh();
            }

            for (int i = 0; i < this.lvTags.Items.Count; i++)
            {
                if (!lepcs.Contains(this.lvTags.Items[i].Text))
                    lepcs.Add(this.lvTags.Items[i].Text);
            }
        }
        private void showmess2(string mess)
        {
            label1_status.Text = mess;
        }
        private void thread_run()
        {
            while (t_isrun)
            {
                try
                {

                    R2000_calibration.TagLED_DATA readdata = mordr.ReadTagLED(filter, t_timeout, metaflag);
                    if(readdata.TagEpc!=null)
                    iar=this.BeginInvoke(new handleshow1(showmess1), new object[] { readdata });
                }
                catch (OpFaidedException notagexp)
                {
                    if (notagexp.ErrCode == 0x400)
                        iar=this.BeginInvoke(new handleshow2(showmess2), new object[] { DateTime.Now.ToString() + "  没法发现标签" }); 
                    else
                        iar = this.BeginInvoke(new handleshow2(showmess2), new object[] { DateTime.Now.ToString() + "   操作失败:" + notagexp.ErrCode.ToString("X4") });

                    return;
                }
                catch (Exception ex)
                {
          
                    iar = this.BeginInvoke(new handleshow2(showmess2), new object[] { DateTime.Now.ToString() + "操作失败:" + ex.Message });
                    return;
                }
                System.Threading.Thread.Sleep(t_ivl);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
               
                int timeout;
                if (isctrlledtime)
                    timeout = ((int.Parse(this.textBox_ledlightt.Text) / 100) << 8) | int.Parse(this.textBox_optime.Text) / 100;
                else
                    timeout = int.Parse(this.textBox_optime.Text);

                System.Diagnostics.Debug.WriteLine("Time:"+DateTime.Now.Second.ToString());
                R2000_calibration.TagLED_DATA readdata = mordr.ReadTagLED(filter, timeout, metaflag);

                bool ishave = false;
                for (int i = 0; i < this.lvTags.Items.Count; i++)
                {
                    if (this.lvTags.Items[i].SubItems[0].Text == ByteFormat.ToHex(readdata.TagEpc))
                    {
                        this.lvTags.Items[i].SubItems[1].Text = (int.Parse(this.lvTags.Items[i].SubItems[1].Text) + readdata.ReadCount).ToString();
                        this.lvTags.Items[i].SubItems[2].Text = readdata.Antenna.ToString();

                        if (readdata.Data != null)
                            this.lvTags.Items[i].SubItems[3].Text = ByteFormat.ToHex(readdata.Data);
                        else
                            this.lvTags.Items[i].SubItems[3].Text = "";
                        this.lvTags.Items[i].SubItems[4].Text = "Gen2";
                        this.lvTags.Items[i].SubItems[5].Text = readdata.Lqi.ToString();
                        this.lvTags.Items[i].SubItems[6].Text = readdata.Frequency.ToString();

                        if (mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7100 ||
           mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7200 ||
           mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7300 ||
           mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7400 ||
           mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7500)
                        {

                            float xw = (float)(readdata.Phase / 256.0 * 360);
                            this.lvTags.Items[i].SubItems[7].Text = Math.Round(xw, 2).ToString();
                        }
                        else
                        this.lvTags.Items[i].SubItems[7].Text = readdata.Phase.ToString();

                        ishave = true;
                        break;
                    }
                }

                if (!ishave)
                {
                    ListViewItem lvi = new ListViewItem(ByteFormat.ToHex(readdata.TagEpc));
                    lvi.SubItems.Add(readdata.ReadCount.ToString());
                    lvi.SubItems.Add(readdata.Antenna.ToString());
                    if (readdata.Data != null)
                        lvi.SubItems.Add(ByteFormat.ToHex(readdata.Data));
                    else
                        lvi.SubItems.Add("");
                    lvi.SubItems.Add("Gen2");
                    lvi.SubItems.Add(readdata.Lqi.ToString());
                    lvi.SubItems.Add(readdata.Frequency.ToString());
                    if (mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7100 ||
          mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7200 ||
          mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7300 ||
          mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7400 ||
          mordr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7500)
                    {

                        float xw = (float)(readdata.Phase / 256.0 * 360);
                        lvi.SubItems.Add(Math.Round(xw, 2).ToString());
                    }
                    else
                    lvi.SubItems.Add(readdata.Phase.ToString());
                    this.lvTags.Items.Add(lvi);
                    this.lvTags.Refresh();
                }

                
               for (int i = 0; i < this.lvTags.Items.Count; i++)
               {
                   if(!lepcs.Contains(this.lvTags.Items[i].Text))
                       lepcs.Add(this.lvTags.Items[i].Text);
               }

                int ivt = int.Parse(textBox_ledtime.Text);
                if (ivt == 0)
                    timer1.Interval = 1;
                else
                    timer1.Interval = ivt;
            }
            catch (OpFaidedException notagexp)
            {
                if (notagexp.ErrCode == 0x400)
                   label1_status.Text=DateTime.Now.ToString()+"  没法发现标签";
                else
                    label1_status.Text=DateTime.Now.ToString()+"   操作失败:" + notagexp.ErrCode.ToString("X4");  

                return;
            }
            catch (Exception ex)
            {
                label1_status.Text = DateTime.Now.ToString() + "操作失败:" + ex.Message;
                return;
            }
        }

        private void button_mulselect_Click(object sender, EventArgs e)
        { 
            Form_mulselect frm = new Form_mulselect(mordr, rparam, lepcs);
            frm.ShowDialog();
        }
    }
}
