using ModuleTech;
using ModuleTech.Gen2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    public partial class Form_inicard : Form
    {

        public Form_inicard(Reader rdr, ReaderParams param, Form1 frm)
        {
            InitializeComponent();
            MTreader = rdr;
            rparam = param;
            Mfrm = frm;
        }
        ReaderParams rparam = null;

        Form1 Mfrm = null;
        ModuleTech.Reader MTreader;
        struct OPTION
        {
            public bool MT;
            public bool BOX;
            public int ST;
            public int ED;
            public int POW;
            public int VAL;
            public string fstr;
            public string after;
            public int LEN;
        }
        OPTION Option;
        bool isRun;
        Thread InitThread;
        List<string> lhaveinit;
        IAsyncResult Iar;
        int readlong, readstop;


        private void startbutton_Click(object sender, EventArgs e)
        {
            try
            {

                if (textBox_before.Text.Length >= int.Parse(Len_textBox.Text))
                {
                    MessageBox.Show("前缀字符串长度大于总长度");
                    return;
                }


                if (int.Parse(Len_textBox.Text) % 4 != 0)
                {
                    MessageBox.Show("长度为4或4倍数");
                    return;
                }

                if (rparam.antcnt < 1)
                {
                    MessageBox.Show("先连接读写器");
                    return;
                }

                if (Option.MT)
                {
                    AntPower[] antpwr = new AntPower[rparam.antcnt];
                    byte antid;
                    for (int i = 0; i < antpwr.Length; i++)
                    {

                        antid = (byte)(i + 1);
                        antpwr[i] = new AntPower(antid, (ushort)int.Parse(pow_textBox.Text), (ushort)int.Parse(pow_textBox.Text));
                    }

                    MTreader.ParamSet("AntPowerConf", antpwr);

                    int opant = 0;
                    if (radioButton1.Checked)
                    {
                        opant = 1;
                    }

                    if (radioButton2.Checked)
                    {
                        opant = 2;
                    }

                    if (radioButton3.Checked)
                    {
                        opant = 3;
                    }

                    if (radioButton4.Checked)
                    {
                        opant = 4;
                    }

                    if (opant == 0)
                    {
                        throw new Exception("mt 天线没选择");
                    }

                    MTreader.ParamSet("TagopAntenna", opant);

                    // MTreader.ParamSet("CheckAntConnection", false);

                    MTreader.ParamSet("ReadPlan", new SimpleReadPlan(new int[] { opant }));
                }


                Option.ST = int.Parse(starttextBox.Text);

                if (endtextBox.Text == string.Empty)
                {
                    Option.ED = 0;
                }
                else
                {
                    Option.ED = int.Parse(endtextBox.Text);
                }

                Option.LEN = int.Parse(Len_textBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            Option.VAL = Option.ST;

            Option.fstr = epc_label.Text = textBox_before.Text;
            Option.after = textBox_after.Text;

            nextepc = getnextepc();
            epc_label.Text = nextepc;

            isRun = true;
            lhaveinit.Clear();
            InitThread = new Thread(new ThreadStart(Running));
            InitThread.Start();
            startbutton.Enabled = false;
            stopbutton.Enabled = true;
            checkbutton.Enabled = false;
            button4.Enabled = false;

        }

        private void stopbutton_Click(object sender, EventArgs e)
        {
            isRun = false;
            if (Iar != null)
            {
                EndInvoke(Iar);
            }

            if (InitThread != null)
            {
                InitThread.Join();
            }

            startbutton.Enabled = true;
            stopbutton.Enabled = false;
            checkbutton.Enabled = true;
            button4.Enabled = true;

        }
        private void HandleMess(int type, string mess)
        {
            switch (type)
            {
                case -1:
                    decri_label.Text = mess;
                    isRun = false;
                    if (Iar != null)
                    {
                        EndInvoke(Iar);
                    }

                    if (InitThread != null)
                    {
                        InitThread.Join();
                    }

                    startbutton.Enabled = true;
                    stopbutton.Enabled = false;

                    checkbutton.Enabled = true;
                    button4.Enabled = true;
                    break;
                case 0:
                    decri_label.Text = mess;
                    break;
                case 1:
                    string[] ss = mess.Split(new char[] { '&' });
                    decri_label.Text = "MT 写数据成功:" + ss[0] + " \n\r";
                    epc_label.Text = "将要初始化下一个&" + ss[1];
                    listBox1.Items.Add(ss[0] + "--->" + ss[1]);
                    break;

            }
        }
        delegate void DeleHanle(int type, string mess);
        DeleHanle dh;

        public string getnextepc()
        {
            string epc = string.Empty;
            if (radioButton5.Checked)
            {

                epc = Option.fstr;
                epc += Option.VAL.ToString().PadLeft(Option.LEN - Option.fstr.Length - Option.after.Length, '0');
                epc += Option.after;

            }
            else if (radioButton6.Checked)
            {
                //epc = Option.fstr;
                //DateTime dt2 = new DateTime(1970, 1, 1);
                //DateTime dt = DateTime.Now;
                //TimeSpan ts = dt - dt2;
                //string ss = Math.Round(ts.TotalMilliseconds, 0).ToString();
                //if (ss.Length > Option.LEN - Option.fstr.Length - Option.after.Length)
                //    ss = ss.Substring(ss.Length - Option.LEN + Option.fstr.Length + Option.after.Length);

                string date = "000000000" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("d2") + DateTime.Now.Day.ToString("d2") +
                    DateTime.Now.Hour.ToString("d2") + DateTime.Now.Minute.ToString("d2") + DateTime.Now.Second.ToString("d2") +
                    DateTime.Now.Millisecond.ToString("d3");

                int nowl = date.Length + Option.fstr.Length + Option.after.Length;
                if (nowl > Option.LEN)
                {
                    date = date.Substring(date.Length - (Option.LEN - Option.fstr.Length - Option.after.Length));
                }

                epc = Option.fstr + date + Option.after;

            }
            else
            {
                if (Option.BOX)
                {
                    epc = "BBBB";
                }
                else
                {
                    epc = "FFFF";
                }
                epc += Option.VAL.ToString().PadLeft(Option.LEN - 4, '0');
            }

            return epc;
        }

        string nextepc = string.Empty;
        private void Running()
        {
            string lastepc = string.Empty;

            while (isRun)
            {
                if (Option.MT)
                {
                    TagReadData[] trds = null;

                    try
                    {
                        trds = MTreader.Read(readlong);
                    }
                    catch (ModuleLibrary.ModuleException mmex)
                    {
                        Iar = BeginInvoke(dh, new object[] { -1, "MT 写数据异常:" + mmex.ErrCode.ToString("X4") + "\n\r将停止工作" });
                        return;
                    }

                    if (lhaveinit.Count > 0 && trds.Length > 0)
                        trds = trds.Where(r => !lhaveinit.Select(j => j).Contains(r.EPCString)).ToArray();

                    if (trds.Length < 1)
                        Iar = BeginInvoke(new DeleHanle(HandleMess), new object[] { 0, "没有初始化的标签" });
                    else if (trds.Length > 1)
                        Iar = BeginInvoke(dh, new object[] { 0, "标签太多了" });
                    else if (Option.ED != 0 && Option.VAL > Option.ED)
                    {
                        isRun = false;
                        Iar = BeginInvoke(dh, new object[] { -1, "初始化标签达到上限数" });
                    }
                    else if (!lhaveinit.Contains(trds[0].EPCString))
                    {
                        string epc = string.Empty;
                        if (lastepc == string.Empty)
                        {
                            if (nextepc != string.Empty)
                            {
                                epc = nextepc;
                            }
                            else
                            {
                                epc = getnextepc();
                            }
                        }
                        else
                        {
                            epc = lastepc;
                        }


                        try
                        {
                            var item = trds[0].EPCString;
                            int flen = item.Length * 4;
                            byte[] fdata = ByteFormat.FromHex(item);
                            Gen2TagFilter gtf= new Gen2TagFilter(flen, fdata, MemBank.EPC, 32, false);

                            MTreader.WriteTag(gtf, new ModuleTech.TagData(epc));
                        }
                        catch (ModuleLibrary.FatalInternalException fiex)
                        {
                            Iar = BeginInvoke(dh, new object[] { -1, "MT 写数据异常:" + fiex.ErrCode.ToString("X4") + "\n\r将停止工作" });

                        }
                        catch (ModuleLibrary.HardwareAlertException hex)
                        {
                            Iar = BeginInvoke(dh, new object[] { -1, "MT 写数据异常:" + hex.ErrCode.ToString("X4") + "\n\r将停止工作" });

                        }
                        catch (ModuleLibrary.ModuleException mex)
                        {
                            lastepc = epc;
                            Iar = BeginInvoke(dh, new object[] { 0, "MT 写数据异常:" + mex.ErrCode.ToString("X4") + "\n\r将重新写:" + epc });

                            continue;
                        }
                        lastepc = string.Empty;

                        Option.VAL++;
                        nextepc = getnextepc();
                        lhaveinit.Add(epc.ToUpper());
                        System.Console.Beep();

                        GPOSet();

                        //Iar = this.BeginInvoke(dh, new object[] { 1, "MT 写数据成功:" + epc + " \n\r将要初始化下一个&" + nextepc });

                        Iar = BeginInvoke(dh, new object[] { 1, epc + "&" + nextepc });
                    }
                    else
                    {
                        Iar = BeginInvoke(dh, new object[] { 0, trds[0].EPCString + "已经初始化" });
                    }
                }

                Thread.Sleep(readstop);
            }
        }
        private void Form_initcard_Load(object sender, EventArgs e)
        {
            cbgpo1.Enabled = false;
            cbgpo2.Enabled = false;
            cbgpo3.Enabled = false;
            cbgpo4.Enabled = false;

            if (rparam.readertype == ReaderType.MT_TWOANTS || rparam.readertype == ReaderType.MT_ONEANT || rparam.readertype == ReaderType.MT100)
            {
                cbgpo1.Enabled = true;
                cbgpo2.Enabled = true;
            }
            else if (rparam.readertype == ReaderType.MT_FOURANTS)
            {

                cbgpo1.Enabled = true;
                cbgpo2.Enabled = true;
            }
            else if (rparam.readertype == ReaderType.MT_A7_TWOANTS ||
                rparam.readertype == ReaderType.MT_A7_FOURANTS ||
                rparam.readertype == ReaderType.SL_FOURANTS ||
                rparam.readertype == ReaderType.M6_A7_FOURANTS ||
                rparam.readertype == ReaderType.M56_A7_FOURANTS ||
                rparam.readertype == ReaderType.SL_FOURANTS)
            {
                if (MTreader.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM7 ||
                    MTreader.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_WIFI)
                {
                    cbgpo1.Enabled = true;
                    cbgpo2.Enabled = true;
                    cbgpo3.Enabled = true;
                    cbgpo4.Enabled = true;
                }
                else if (MTreader.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9
                    || MTreader.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI)
                {
                    cbgpo1.Enabled = true;
                    cbgpo2.Enabled = true;
                    cbgpo3.Enabled = true;
                    cbgpo4.Enabled = true;
                }
            }


            Option.MT = true;
            Option.POW = 0;
            Option.ST = 0;
            Option.ED = 0;
            Option.BOX = true;
            lhaveinit = new List<string>();

            dh = HandleMess;

            startbutton.Enabled = false;
            stopbutton.Enabled = false;
            try
            {

                readlong = int.Parse(textBox2.Text);
                readstop = int.Parse(textBox3.Text);
            }
            catch (ModuleLibrary.ModuleException mex)
            {
                MessageBox.Show(mex.Message + mex.ErrCode.ToString("X4"));
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            startbutton.Enabled = true;
            stopbutton.Enabled = false;
            checkbutton.Enabled = true;
            button4.Enabled = true;

        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void checkbutton_Click(object sender, EventArgs e)
        {
            startbutton.Enabled = false;

            if (Option.MT)
            {
                AntPower[] antpwr = new AntPower[rparam.antcnt];
                byte antid;
                for (int i = 0; i < antpwr.Length; i++)
                {

                    antid = (byte)(i + 1);
                    antpwr[i] = new AntPower(antid, (ushort)int.Parse(pow_textBox.Text), (ushort)int.Parse(pow_textBox.Text));
                }

                MTreader.ParamSet("AntPowerConf", antpwr);

                int opant = 0;
                if (radioButton1.Checked)
                {
                    opant = 1;
                }

                if (radioButton2.Checked)
                {
                    opant = 2;
                }

                if (radioButton3.Checked)
                {
                    opant = 3;
                }

                if (radioButton4.Checked)
                {
                    opant = 4;
                }

                if (opant == 0)
                {
                    throw new Exception("mt 天线没选择");
                }

                MTreader.ParamSet("TagopAntenna", opant);

                MTreader.ParamSet("ReadPlan", new SimpleReadPlan(new int[] { opant }));
                TagReadData[] trds = MTreader.Read(1000);
                for (int i = 0; i < trds.Length; i++)
                {
                    listBox1.Items.Add(trds[i].EPCString);
                }
            }

            startbutton.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (Option.MT)
                {
                    AntPower[] antpwr = new AntPower[1];
                    byte antid;
                    for (int i = 0; i < antpwr.Length; i++)
                    {

                        antid = (byte)(i + 1);
                        antpwr[i] = new AntPower(antid, ushort.Parse(pow_textBox.Text), ushort.Parse(pow2_textBox.Text));
                    }

                    MTreader.ParamSet("AntPowerConf", antpwr);

                    MTreader.ParamSet("OpTimeout", ushort.Parse(textBox_wt.Text));
                }
                readlong = int.Parse(textBox2.Text);
                readstop = int.Parse(textBox3.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("已经设置");
        }

        private void btngetrg_Click(object sender, EventArgs e)
        {
            try
            {
                int st = Environment.TickCount;
                ModuleTech.Region rg = (ModuleTech.Region)MTreader.ParamGet("Region");

                switch (rg)
                {
                    case ModuleTech.Region.CN:
                        cbbregion.SelectedIndex = 6;
                        break;
                    case ModuleTech.Region.EU:
                    case ModuleTech.Region.EU2:
                    case ModuleTech.Region.EU3:
                        cbbregion.SelectedIndex = 4;
                        break;
                    case ModuleTech.Region.IN:
                        cbbregion.SelectedIndex = 5;
                        break;
                    case ModuleTech.Region.JP:
                        cbbregion.SelectedIndex = 2;
                        break;
                    case ModuleTech.Region.KR:
                        cbbregion.SelectedIndex = 3;
                        break;
                    case ModuleTech.Region.NA:
                        cbbregion.SelectedIndex = 1;
                        break;
                    case ModuleTech.Region.PRC:
                        cbbregion.SelectedIndex = 0;
                        break;
                    case ModuleTech.Region.OPEN:
                        cbbregion.SelectedIndex = 7;
                        break;
                    case ModuleTech.Region.PRC2:
                        cbbregion.SelectedIndex = 8;
                        break;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("获取失败");
            }
        }

        private void btnsetrg_Click(object sender, EventArgs e)
        {
            ModuleTech.Region rg = ModuleTech.Region.UNSPEC;
            bool is840_845 = false;
            bool is840_925 = false; ;
            if (cbbregion.SelectedIndex == -1)
            {
                MessageBox.Show("请选择区域");
                return;
            }
            switch (cbbregion.SelectedIndex)
            {
                case 0:
                    rg = ModuleTech.Region.PRC;
                    break;
                case 1:
                    rg = ModuleTech.Region.NA;
                    break;
                case 2:
                    rg = ModuleTech.Region.JP;
                    break;
                case 3:
                    rg = ModuleTech.Region.KR;
                    break;
                case 4:
                    rg = ModuleTech.Region.EU3;
                    break;
                case 5:
                    rg = ModuleTech.Region.IN;
                    break;
                case 6:
                    rg = ModuleTech.Region.CN;
                    break;
                case 7:
                    rg = ModuleTech.Region.OPEN;
                    break;
                case 8:
                    {
                        if (MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E ||
                            MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E_PRC)
                        {
                            if (MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E_PRC)
                            {
                                rg = ModuleTech.Region.PRC2;
                            }
                            else
                            {
                                is840_845 = true;
                                rg = ModuleTech.Region.OPEN;
                            }
                            break;
                        }
                        else
                        {
                            MessageBox.Show("不支持的区域");
                            return;
                        }
                    }
                case 9:
                    {
                        if (MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E ||
                            MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E_PRC)
                        {
                            rg = ModuleTech.Region.OPEN;
                            is840_925 = true;
                            break;
                        }
                        else
                        {
                            MessageBox.Show("不支持的区域");
                            return;
                        }
                    }

            }


            try
            {
                MTreader.ParamSet("Region", rg);
                if (is840_845 || is840_925)
                {
                    List<uint> htab = new List<uint>();
                    if (is840_845)
                    {
                        htab.Add(841375);
                        htab.Add(842625);
                        htab.Add(840875);
                        htab.Add(843625);
                        htab.Add(841125);
                        htab.Add(840625);
                        htab.Add(843125);
                        htab.Add(841625);
                        htab.Add(842125);
                        htab.Add(843875);
                        htab.Add(841875);
                        htab.Add(842875);
                        htab.Add(844125);
                        htab.Add(843375);
                        htab.Add(844375);
                        htab.Add(842375);
                    }
                    else if (is840_925)
                    {
                        htab.Add(841375);
                        htab.Add(921375);

                        htab.Add(842625);
                        htab.Add(922625);

                        htab.Add(840875);
                        htab.Add(920875);

                        htab.Add(843625);
                        htab.Add(923625);

                        htab.Add(841125);
                        htab.Add(921125);

                        htab.Add(840625);
                        htab.Add(920625);

                        htab.Add(843125);
                        htab.Add(923125);

                        htab.Add(841625);
                        htab.Add(921625);

                        htab.Add(842125);
                        htab.Add(922125);

                        htab.Add(843875);
                        htab.Add(923875);

                        htab.Add(841875);
                        htab.Add(921875);

                        htab.Add(842875);
                        htab.Add(922875);

                        htab.Add(844125);
                        htab.Add(924125);

                        htab.Add(843375);
                        htab.Add(923375);

                        htab.Add(844375);
                        htab.Add(924375);

                        htab.Add(842375);
                        htab.Add(922375);
                    }
                    MTreader.ParamSet("FrequencyHopTable", htab.ToArray());
                }
                else
                {

                }
            }
            catch (Exception)
            {
                MessageBox.Show("设置失败 ");
                return;
            }
            MessageBox.Show("设置成功");
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            Random r = new Random(Environment.TickCount + DateTime.Now.Millisecond);

            string b = string.Empty, a = string.Empty;

            for (int i = 0; i < 4; i++)
            {
                int n = r.Next(0, 16);
                b += n.ToString("X");
            }

            for (int i = 0; i < 4; i++)
            {
                int n = r.Next(0, 16);
                a += n.ToString("X");
            }

            textBox_before.Text = b;
            textBox_after.Text = a;
        }

        private void Form_inicard_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRun = false;
            if (Iar != null)
            {
                EndInvoke(Iar);
            }

            if (InitThread != null)
            {
                InitThread.Join();
            }
        }

        public void GPOSet()
        {
            if (cbgpo1.Enabled && cbgpo1.Checked)
            {
                MTreader.GPOSet(1, true);
                Thread.Sleep(200);
                MTreader.GPOSet(1, false);
            }
            else if (cbgpo2.Enabled && cbgpo2.Checked) {
                MTreader.GPOSet(2, true);
                Thread.Sleep(200);
                MTreader.GPOSet(2, false);
            }
            else if (cbgpo3.Enabled && cbgpo3.Checked)
            {
                MTreader.GPOSet(3, true);
                Thread.Sleep(200);
                MTreader.GPOSet(3, false);
            }
            else if (cbgpo4.Enabled && cbgpo4.Checked)
            {
                MTreader.GPOSet(4, true);
                Thread.Sleep(200);
                MTreader.GPOSet(4, false);
            }


        }
    }
}
