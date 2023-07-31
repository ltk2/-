using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ModuleTech;
using System.Threading;
using System.Runtime.InteropServices;

namespace ModuleReaderManager
{
    public partial class Form_print : Form
    {
        public Form_print(Reader rdr, ReaderParams param, Form1 frm)
        {
            InitializeComponent();
            MTreader = rdr;
            rparam = param;
            Mfrm = frm;
        }

        /*
        //枚举设备
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern string DSEnumPrinter();

        //打开设备
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSOpenPrinter(string buf);

        //关闭设备
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSClosePrinter();

        //获取状态
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSNewGetDeviceStatus(byte[] tatus);

        //清除rfid状态
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSNewGetRFIDStatus(byte[] tatus);

        //获取机号
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void DSGetPrID_std(byte[] buf, int bufsize);

        //旋转
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSZPLRotation(int rotation);

        //打印文本
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSZPLPrintTextEx(int byCommand, string fontFace, bool italic, bool bold, int byDarkness, double dXPos, double dYPos, int byHScale, int byWScale, string data);

        //设置页长
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSZPLSetPageLength(double inch);

        //打二维码
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSZPLPrintQrCodeExA(int byCommand, int byDarkness, double dXPos, double dYPos, double QrSize, int byErrLevel, StringBuilder data);

        //打印一维码
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSZPLPrintCode128(int byCommand, int byDarkness, double dXPos, double dYPos, int byHScaleHumanRead, int byWScaleHumanRead, double dBarcodeHeight, bool bFlagHumanRead, bool bPosHumanRead, string data);

        //设置切刀
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSDLEnableCutter(bool cut);

        //打印图片
        [DllImport("DSPrinterLib.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DSZPLPrintImageExA_SizeScalable(int byCommand, double dXPos, double dYPos, double widthTimes, double heightTimes, string path, int Threshold2);

        //打印图片1
        [DllImport("DSPrinterLib.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int DSZPLPrintImageScaleA(int byCommand, double dXPos, double dYPos, double xScale, double yScale, string picPath);

        //设置DPI
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void DSSetDpi(int dpi);

        //获取DPI
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSGetDpi();

        //读TidEPC（动态）
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFIDTag_Read_TidEPCID(byte[] buffer, ref int c);

        //写EPC（动态）
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFIDTag_Write_EPCID(byte[] buff, int bufflen);

        //检索标签个数
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFIDTag_Search(ref uint leng);

        //标签前移
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFID_TagForward(UInt32 leng);

        //标签后移
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFID_TagBackward(UInt32 leng);

        //RFID芯片调整
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFID_Position_Adjustment(Byte leng);

        //RFID自动定位
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFID_Automatic_Location();

        //设置读功率
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFIDTag_SetReadPower(UInt16 dbm);

        //获取读功率
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFIDTag_FetchReadPower(ref UInt16 dbm);

        //获取写功率
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFIDTag_SetWritePower(UInt16 dbm);

        //获取写功率
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFIDTag_FetchWritePower(ref UInt16 dbm);

        //黑标定位
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSDLAlignToBlackMark();

        //打印PDF
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSZPLPrintPdf(string pdfPath, double xpos, double ypos, double xscale, double yscale, double height, int pageNumber, int rotation, bool UsingDriver, int ThresholdA);

        //判断打印是否可以开始
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSPrintStatusBegin();
        //判断打印是否完成
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSPrintStatusEnd();
        //结束判断打印是否可以开始
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSPrintBeginStop();
        //结束判断打印是否完成
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSPrintEndStop();

        //读Tid+EPC
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_RFIDTag_Read_TidEPCIDA(byte[] buffer, ref int c);


        //写EPC字符（静态）
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_HNRFIDTag_Write_EPCID(string buff, int bufflen);

        //读EPC（静态）
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_HNRFIDTag_Read_EPCID(IntPtr buffer);

        //读TID（静态）
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_HNRFID_ReadTid(IntPtr buffer);

        //写USER（静态）
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_HNRFIDTag_Write_User(string buff, int bufflen);

        //读USER（静态）
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DAS_HNRFIDTag_Read_User(IntPtr buffer, int bufflen);

        //开启补打
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSOpenReprint();

        //关闭补打
        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSCloseReprint();

        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSPrintData(string str, bool hexMode);

        [DllImport("DSPrinterLib.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int DSPrintDataA(string str, bool hexMode);
        */


        Form1 Mfrm = null;
        ModuleTech.Reader MTreader;
        ReaderParams rparam = null;

        bool UhfPrintSync = true;//读写同步
        bool Failedskip = true;//写失败跳号
        bool FilterTID = true;
        public class Tags
        {
            public string Tid;
            public string oldepcid;
            public string newepcid;
 
        }
        public bool Is_ContainOldEpcid(string oldepckey)
        {
            foreach(KeyValuePair<string,Tags> kvp in InitTags)
            {
                if (kvp.Value.oldepcid == oldepckey)
                    return true;
            }

            return false;
        }
        public bool Is_ContainNewEpcid(string newepckey)
        {
            foreach (KeyValuePair<string, Tags> kvp in InitTags)
            {
                if (kvp.Value.newepcid == newepckey)
                    return true;
            }

            return false;
        }
        public string GetTagsTidbyNewepcid(string newepckey)
        {
            foreach (KeyValuePair<string, Tags> kvp in InitTags)
            {
                if (kvp.Value.newepcid == newepckey)
                    return kvp.Key;
            }
            return "";
        }

        Dictionary<string, Tags> InitTags = new Dictionary<string, Tags>();
        Dictionary<string, string> InitEpcToReadyEpc = new Dictionary<string, string>();
        Dictionary<string, int> WrtieResults = new Dictionary<string, int>();
        
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
        bool isRun,isRun2;
        Thread InitThread,PrintThread;
        List<string> lhaveinit;
        List<string> readyinit;
        IAsyncResult Iar;
        int readlong, readstop;

        private void button4_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    ModuleTech.Region rg = ModuleTech.Region.UNSPEC;
            //    bool is840_845 = false;
            //    bool is840_925 = false; ;
            //    if (this.cbbregion.SelectedIndex == -1)
            //    {
            //        MessageBox.Show("请选择区域");
            //        return;
            //    }
            //    switch (this.cbbregion.SelectedIndex)
            //    {
            //        case 0:
            //            rg = ModuleTech.Region.PRC;
            //            break;
            //        case 1:
            //            rg = ModuleTech.Region.NA;
            //            break;
            //        case 2:
            //            rg = ModuleTech.Region.JP;
            //            break;
            //        case 3:
            //            rg = ModuleTech.Region.KR;
            //            break;
            //        case 4:
            //            rg = ModuleTech.Region.EU3;
            //            break;
            //        case 5:
            //            rg = ModuleTech.Region.IN;
            //            break;
            //        case 6:
            //            rg = ModuleTech.Region.CN;
            //            break;
            //        case 7:
            //            rg = ModuleTech.Region.OPEN;
            //            break;
            //        case 8:
            //            {
            //                if (MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E ||
            //                    MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E_PRC)
            //                {
            //                    if (MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E_PRC)
            //                        rg = ModuleTech.Region.PRC2;
            //                    else
            //                    {
            //                        is840_845 = true;
            //                        rg = ModuleTech.Region.OPEN;
            //                    }
            //                    break;
            //                }
            //                else
            //                {
            //                    MessageBox.Show("不支持的区域");
            //                    return;
            //                }
            //            }
            //        case 9:
            //            {
            //                if (MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E ||
            //                    MTreader.HwDetails.module == ModuleTech.Reader.Module_Type.MODOULE_M6E_PRC)
            //                {
            //                    rg = ModuleTech.Region.OPEN;
            //                    is840_925 = true;
            //                    break;
            //                }
            //                else
            //                {
            //                    MessageBox.Show("不支持的区域");
            //                    return;
            //                }
            //            }

            //    }


               
            //        MTreader.ParamSet("Region", rg);
            //        if (is840_845 || is840_925)
            //        {
            //            List<uint> htab = new List<uint>();
            //            if (is840_845)
            //            {
            //                htab.Add(841375);
            //                htab.Add(842625);
            //                htab.Add(840875);
            //                htab.Add(843625);
            //                htab.Add(841125);
            //                htab.Add(840625);
            //                htab.Add(843125);
            //                htab.Add(841625);
            //                htab.Add(842125);
            //                htab.Add(843875);
            //                htab.Add(841875);
            //                htab.Add(842875);
            //                htab.Add(844125);
            //                htab.Add(843375);
            //                htab.Add(844375);
            //                htab.Add(842375);
            //            }
            //            else if (is840_925)
            //            {
            //                htab.Add(841375);
            //                htab.Add(921375);

            //                htab.Add(842625);
            //                htab.Add(922625);

            //                htab.Add(840875);
            //                htab.Add(920875);

            //                htab.Add(843625);
            //                htab.Add(923625);

            //                htab.Add(841125);
            //                htab.Add(921125);

            //                htab.Add(840625);
            //                htab.Add(920625);

            //                htab.Add(843125);
            //                htab.Add(923125);

            //                htab.Add(841625);
            //                htab.Add(921625);

            //                htab.Add(842125);
            //                htab.Add(922125);

            //                htab.Add(843875);
            //                htab.Add(923875);

            //                htab.Add(841875);
            //                htab.Add(921875);

            //                htab.Add(842875);
            //                htab.Add(922875);

            //                htab.Add(844125);
            //                htab.Add(924125);

            //                htab.Add(843375);
            //                htab.Add(923375);

            //                htab.Add(844375);
            //                htab.Add(924375);

            //                htab.Add(842375);
            //                htab.Add(922375);
            //            }
            //            MTreader.ParamSet("FrequencyHopTable", htab.ToArray());
            //        }
            //        else
            //        {

            //        }
                
            try
            {

                if (Option.MT)
                {
                    AntPower[] antpwr = new AntPower[1];
                    byte antid;
                    for (int i = 0; i < antpwr.Length; i++)
                    {

                        antid = (byte)(i + 1);
                        antpwr[i] = new AntPower(antid, ushort.Parse(pow_textBox.Text), ushort.Parse(poww_textBox.Text));
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
                        this.cbbregion.SelectedIndex = 6;
                        break;
                    case ModuleTech.Region.EU:
                    case ModuleTech.Region.EU2:
                    case ModuleTech.Region.EU3:
                        this.cbbregion.SelectedIndex = 4;
                        break;
                    case ModuleTech.Region.IN:
                        this.cbbregion.SelectedIndex = 5;
                        break;
                    case ModuleTech.Region.JP:
                        this.cbbregion.SelectedIndex = 2;
                        break;
                    case ModuleTech.Region.KR:
                        this.cbbregion.SelectedIndex = 3;
                        break;
                    case ModuleTech.Region.NA:
                        this.cbbregion.SelectedIndex = 1;
                        break;
                    case ModuleTech.Region.PRC:
                        this.cbbregion.SelectedIndex = 0;
                        break;
                    case ModuleTech.Region.OPEN:
                        this.cbbregion.SelectedIndex = 7;
                        break;
                    case ModuleTech.Region.PRC2:
                        this.cbbregion.SelectedIndex = 8;
                        break;
                }

                uint[] htb = (uint[])MTreader.ParamGet("FrequencyHopTable");

                textBox_fre.Text = htb[0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取失败");
            }
        }

        private void btnsetrg_Click(object sender, EventArgs e)
        {
           
        }

        private void stopbutton_Click(object sender, EventArgs e)
        {
            isRun = false;
            isRun2 = false;
            if (Iar != null)
                this.EndInvoke(Iar);
            if (InitThread != null)
                InitThread.Join();

            if (PrintThread != null)
                PrintThread.Join();

            startbutton.Enabled = true;
            stopbutton.Enabled = false;
            button4.Enabled = true;
           

             textBox_debuglog.Text +="\r\n\r\n盘点标签结果：\r\n";
            foreach (KeyValuePair<string, Tags> kvp in InitTags)
            {
                if (FilterTID)
                    textBox_debuglog.Text += "TID:" + kvp.Key + " EPC:" + kvp.Value.oldepcid + "\r\n";
                else
                    textBox_debuglog.Text += " EPC:" + kvp.Value.oldepcid + "\r\n";
            }

             textBox_debuglog.Text += "\r\n\r\n打印标签结果：\r\n";
            int ok = 0;
            foreach (KeyValuePair<string, int> kvp in WrtieResults)
            {
                if (FilterTID)
                {
                    string tidstr = GetTagsTidbyNewepcid(kvp.Key);
                    textBox_debuglog.Text += "EPC:" + kvp.Key + " TID:" + tidstr + (kvp.Value == 1 ? " 成功\r\n" : " 失败\r\n");
                }
                else
                    textBox_debuglog.Text += "EPC:" + kvp.Key + (kvp.Value == 1 ? " 成功\r\n" : " 失败\r\n");

                if (kvp.Value == 1)
                    ok++;
            }

            textBox_debuglog.Text += "成功率:" + (ok * 1.0 / WrtieResults.Count*100).ToString("f2")+"%";
            textBox_debuglog.Select(textBox_debuglog.Text.Length, 0);
            textBox_debuglog.ScrollToCaret();
        }

        int printtimes = 8;
        int printsleep = 100;
        private void startbutton_Click(object sender, EventArgs e)
        {
            try
            {
                InitTags.Clear();
                InitEpcToReadyEpc.Clear();
                WrtieResults.Clear();
                debugmsg = string.Empty;
                textBox_debuglog.Text = "";
                UhfPrintSync = false;
                Failedskip = checkBox_failedskip.Checked;
                FilterTID = checkBox_filtid.Checked;
                if (FilterTID)
                {
                    EmbededCmdData ecd = new EmbededCmdData(ModuleTech.Gen2.MemBank.TID, 0,(byte)12);
                    MTreader.ParamSet("EmbededCmdOfInventory", ecd);
                }
                else
                    MTreader.ParamSet("EmbededCmdOfInventory", null);

                printsleep = int.Parse(textBox_printtime.Text);
                printtimes = int.Parse(textBox_printcount.Text);

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
                    //AntPower[] antpwr = new AntPower[rparam.antcnt];
                    //byte antid;
                    //for (int i = 0; i < antpwr.Length; i++)
                    //{

                    //    antid = (byte)(i + 1);
                    //    antpwr[i] = new AntPower(antid, (ushort)int.Parse(pow_textBox.Text), (ushort)int.Parse(pow_textBox.Text));
                    //}

                    //MTreader.ParamSet("AntPowerConf", antpwr);

                    int opant = 0;
                    if (radioButton1.Checked)
                        opant = 1;
                    if (radioButton2.Checked)
                        opant = 2;
                    if (radioButton3.Checked)
                        opant = 3;
                    if (radioButton4.Checked)
                        opant = 4;

                    if (opant == 0)
                        throw new Exception("mt 天线没选择");
                    MTreader.ParamSet("TagopAntenna", opant);

                    // MTreader.ParamSet("CheckAntConnection", false);

                    MTreader.ParamSet("ReadPlan", new SimpleReadPlan(new int[] { opant }));
                }


                Option.ST = int.Parse(starttextBox.Text);

                if (endtextBox.Text == string.Empty)
                    Option.ED = 0;
                else
                    Option.ED = int.Parse(endtextBox.Text);

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

            //启动打印
            /*
            DSClosePrinter();
            string info = DSEnumPrinter();
            string port = info.Substring(info.Length - 6);
            if (string.IsNullOrEmpty(port))
            {
                MessageBox.Show("获取打印机信息失败!");
                return;
            }
            int ret = DSOpenPrinter(port);
            if (ret == 0)
            {
                MessageBox.Show("打开打印机失败");
                return;
            }
            */

            isRun = true;
            lhaveinit.Clear();
            readyinit.Clear();
            InitThread = new Thread(new ThreadStart(Running));
            InitThread.Start();

            if (!UhfPrintSync)
            {
                isRun2 = true;
                PrintThread = new Thread(new ThreadStart(Running2));
                PrintThread.Start();
            }

            startbutton.Enabled = false;
            stopbutton.Enabled = true;
            
            button4.Enabled = false;
        }

        private void HandleMess(int type, string mess)
        {
            switch (type)
            {
                case -1:
                    decri_label.Text = mess;
                    stopbutton.PerformClick();
                    break;
                case 0:
                    decri_label.Text = mess;
                    break;
                case 1:
                    string[] ss = mess.Split(new char[] { '&' });
                    decri_label.Text = ss[0];
                    epc_label.Text = ss[1];
                    break;
                case 2:
                    textBox_debuglog.Text += mess;
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
                    date = date.Substring(date.Length - (Option.LEN - Option.fstr.Length - Option.after.Length));
                epc = Option.fstr + date + Option.after;

            }
            else
            {
                if (Option.BOX)
                    epc = "BBBB";
                else
                    epc = "FFFF";
                epc += Option.VAL.ToString().PadLeft(Option.LEN - 4, '0');
            }

            return epc;
        }

        string nextepc = string.Empty;
        string debugmsg = string.Empty;
        
        private void Running()
        {
            string lastepc = string.Empty;
           
            int st = Environment.TickCount;
            while (isRun)
            {
                if (Option.MT)
                {

                    if(debugmsg!=string.Empty)
                    Iar = this.BeginInvoke(dh, new object[] { 2, debugmsg });
                    TagReadData[] trds = null;
                    debugmsg = string.Empty;
                    try
                    {
                        trds = MTreader.Read(readlong);
                        List<string> curtag = new List<string>();
                      
                       // System.Diagnostics.Debug.WriteLine("read all tag: "+trds.Length);
                        debugmsg += "read all tag: " + trds.Length + "\r\n";

                        if (trds.Length > 0)
                        {
                            for (int i = 0; i < trds.Length; i++)
                            {
                                string key = "";
                                string epcid = trds[i].EPCString;
                                string rssi = trds[i].Rssi.ToString();
                                string fre=trds[i].Frequency.ToString();
                                if (FilterTID)
                                {
                                    if (string.IsNullOrEmpty(trds[i].EMDDataString))
                                    {
                                        if (lhaveinit.Count > 0)
                                        {
                                            if (InitTags.Count > 0)
                                            {
                                                if (InitTags[lhaveinit[0]].newepcid.Substring(0, 4) != trds[i].EPCString.Substring(0, 4))
                                                {
                                                    debugmsg += "read tag " + (i + 1) + " epc " + epcid + " rssi " + rssi + " may be new tag not tid\r\n"; 
                                                }
                                            }
                                        }

                                        continue;
                                    }
                                    else
                                        key = trds[i].EMDDataString;
                                }
                                else
                                    key = trds[i].EPCString;

                                System.Diagnostics.Debug.WriteLine("read tag " + (i + 1) + " " + key+" rssi "+rssi);
                                debugmsg += "read tag " + (i + 1) + " " + key +" epc "+epcid+" rssi "+rssi+" fre "+fre+ "\r\n";

                                if (!InitTags.ContainsKey(key))
                                {
                                    Tags t = new Tags();
                                    if (FilterTID)
                                        t.Tid = key;
                                    else
                                        t.Tid = string.Empty;

                                    t.oldepcid = epcid;
                                    t.newepcid = "";
                                    InitTags.Add(key, t);
                                }
                            }

                        }

                        for (int i = 0; i < trds.Length; i++)
                        {
                            string key = "";
                            string epcid = trds[i].EPCString;
                            if (FilterTID)
                            {
                                if (string.IsNullOrEmpty(trds[i].EMDDataString))
                                    continue;
                                else
                                    key = trds[i].EMDDataString;
                            }
                            else
                                key = trds[i].EPCString;

                            if (!readyinit.Contains(key)&&!lhaveinit.Contains(key))
                            {
                                readyinit.Add(key);
                                System.Diagnostics.Debug.WriteLine("readyinit tag "+(i+1)+" "+key);
                                debugmsg += "readyinit tag " + (i + 1) + " " + key + " epc " + epcid + "\r\n";
                            }
                            curtag.Add(key);  
                        }

                        List<string> removes = new List<string>();
                        //当前读不到的准备写入的标签
                        for (int i = 0; i < readyinit.Count; i++)
                        {
                            if (!curtag.Contains(readyinit[i]))
                                removes.Add(readyinit[i]);
                        }
                        //移除读不到准备写入的标签
                        for (int i = 0; i < removes.Count; i++)
                        {
                            readyinit.Remove(removes[i]);
                            System.Diagnostics.Debug.WriteLine("remove tag " + (i+1) + " " + removes[i]);
                            debugmsg += "remove tag " + (i + 1) + " " + removes[i] + "\r\n";
                        }
                    }
                    catch (ModuleLibrary.ModuleException mmex)
                    {
                        Iar = this.BeginInvoke(dh, new object[] { -1, "MT 写数据异常:" + mmex.ErrCode.ToString("X4") + "\n\r将停止工作" });
                        return;
                    }


                    if (trds.Length < 1)
                        Iar = this.BeginInvoke(dh, new object[] { 0, "没有初始化的标签" });
                    else if (readyinit.Count > 0)
                    {
                        for (int i = 0; i < lhaveinit.Count; i++)
                        {
                            System.Diagnostics.Debug.WriteLine("have init tag " + (i + 1) + " " + lhaveinit[i]);
                            debugmsg += "have init tag " + (i + 1) + " " + lhaveinit[i] + "\r\n";
                        }

                        string epc = string.Empty;
                        if (!lhaveinit.Contains(readyinit[0]))
                        {

                            if (lastepc == string.Empty)
                            {
                                if (nextepc != string.Empty)
                                    epc = nextepc;
                                else
                                    epc = getnextepc();
                            }
                            else
                            {
                                if (!InitEpcToReadyEpc.ContainsKey(readyinit[0]))
                                {
                                    if (FilterTID)
                                    {
                                        System.Diagnostics.Debug.WriteLine("skip the tag");
                                        epc = getnextepc();
                                        debugmsg += "skip epc " + epc;

                                        if (!WrtieResults.ContainsKey(epc))
                                            WrtieResults.Add(epc, 0);

                                        Option.VAL++;
                                        epc = getnextepc();
                                        debugmsg += " to epc " + epc + " ???????\r\n";
                                    }
                                    else
                                    {
                                        //判断是改了部分的标签还是下一个标签
                                        Dictionary<string, string>.Enumerator tte = InitEpcToReadyEpc.GetEnumerator();
                                        KeyValuePair<string, string> kvp = tte.Current;
                                        bool iskip = true;
                                        do
                                        {
                                            if (tte.MoveNext())
                                            {
                                                kvp = tte.Current;
                                                int ind = -1;

                                                if (readyinit[0].Length != kvp.Key.Length)
                                                    continue;

                                                for (int i = 0; i < readyinit[0].Length; i++)
                                                {

                                                    if (readyinit[0].Substring(i, 1).Equals(kvp.Key.Substring(1, 1)))
                                                        continue;
                                                    else
                                                    {
                                                        ind = i; break;
                                                    }
                                                }

                                                string toallstr = kvp.Key.Substring(0, ind) + kvp.Value.Substring(ind, kvp.Value.Length - ind);
                                                if (toallstr == readyinit[0])
                                                {
                                                    iskip = false; break;
                                                }

                                            }
                                            else
                                                break;
                                        } while (true);

                                        if (iskip)
                                        {
                                            System.Diagnostics.Debug.WriteLine("skip the tag");
                                            epc = getnextepc();
                                            debugmsg += "skip epc " + epc;

                                            if (!WrtieResults.ContainsKey(epc))
                                                WrtieResults.Add(epc, 0);

                                            Option.VAL++;

                                            epc = getnextepc();
                                            debugmsg += " to epc " + epc + " ???????\r\n";
                                        }
                                       epc = getnextepc();

                                    }
                                }
                                else
                                    epc = lastepc;
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("had init the tag");
                            debugmsg += "had init the tag \r\n";
                            continue;
                        }

                        try
                        {

                            ModuleTech.Gen2.Gen2TagFilter gen2tf = null;

                            if (FilterTID)
                                gen2tf = new ModuleTech.Gen2.Gen2TagFilter(readyinit[0].Length * 4, ByteFormat.FromHex(readyinit[0]),
                              ModuleTech.Gen2.MemBank.TID, 0, false);
                            else
                                gen2tf = new ModuleTech.Gen2.Gen2TagFilter(readyinit[0].Length * 4, ByteFormat.FromHex(readyinit[0]),
                                ModuleTech.Gen2.MemBank.EPC, 32, false);


                            // System.Diagnostics.Debug.WriteLine("ready to write tag:" + readyinit[0] + "  to " + epc);
                            debugmsg += "ready to write tag:" + readyinit[0] + "  to " + epc + "\r\n";

                            if (!InitEpcToReadyEpc.ContainsKey(readyinit[0]))
                                InitEpcToReadyEpc.Add(readyinit[0], epc);
                            MTreader.WriteTag(gen2tf, new ModuleTech.TagData(epc));
                            //System.Diagnostics.Debug.WriteLine("ok!!!!!!!!!!!!!!!!!!!\n");
                            debugmsg += "ok!!!!!!!!!!!!!!!!!!!\r\n";

                            //if (FilterTID)
                                InitTags[readyinit[0]].newepcid = epc;

                            WrtieResults.Add(epc, 1);
                            if (UhfPrintSync)
                            {
                                //DSPrintDataA("^XA^FO999,0^FDJoshua^FS^XZ", false);
                                Thread.Sleep(printsleep);
                            }
                        }
                        catch (ModuleLibrary.FatalInternalException fiex)
                        {

                            Iar = this.BeginInvoke(dh, new object[] { -1, "MT 写数据异常1:" + fiex.ErrCode.ToString("X4") + "\n\r将停止工作" });

                        }
                        catch (ModuleLibrary.HardwareAlertException hex)
                        {
                            Iar = this.BeginInvoke(dh, new object[] { -1, "MT 写数据异常2:" + hex.ErrCode.ToString("X4") + "\n\r将停止工作" });

                        }
                        catch (ModuleLibrary.ModuleException mex)
                        {
                            //System.Diagnostics.Debug.WriteLine("MT 写数据异常3:" + mex.ErrCode.ToString("X4"));
                            debugmsg += "MT 写数据异常3:" + mex.ErrCode.ToString("X4") + "\r\n";
                            lastepc = epc;
                            Iar = this.BeginInvoke(dh, new object[] { 0, "MT 写数据异常3:" + mex.ErrCode.ToString("X4") + "\n\r将重新写:" + epc });

                            continue;
                        }

                        lastepc = string.Empty;
                        Option.VAL++;
                        if (Option.ED != 0 && Option.VAL > Option.ED)
                        { isRun = false; isRun2 = false; Iar = this.BeginInvoke(dh, new object[] { -1, "初始化标签达到上限数" }); }


                        nextepc = getnextepc();

                        if (FilterTID)
                            lhaveinit.Add(readyinit[0]);
                        else
                            lhaveinit.Add(epc.ToUpper());

                        readyinit.Remove(readyinit[0]);

                        System.Console.Beep();
                        Iar = this.BeginInvoke(dh, new object[] { 1, "MT 写数据成功:" + epc + " \n\r将要初始化下一个&" + nextepc });


                    }
                    else
                    {
                        if (FilterTID)
                            Iar = this.BeginInvoke(dh, new object[] { 0, trds[0].EPCString + "|" + trds[0].EMDDataString + " 已经初始化" });
                        else
                            Iar = this.BeginInvoke(dh, new object[] { 0, trds[0].EPCString + " 已经初始化" });
                    }
                }

                Thread.Sleep(readstop);
            }
        }

        private void Running2()
        {
            int i = 0;
            int st = Environment.TickCount;
            //System.Console.WriteLine("begion print>>>>>>>>>>>>>>>>");
            debugmsg += "begion print>>>>>>>>>>>>>>>>\r\n";
            do
            {

               // DSPrintDataA("^XA^FO999,0^FDJoshua^FS^XZ", false);


                if (i < printtimes)
                { Thread.Sleep(printsleep); i++; debugmsg += "print================ " + i + "\r\n"; }
                else
                {
                    isRun2 = false;
                }

            } while (isRun2);

            //System.Console.WriteLine("End print<<<<<<<<<<<<<<<<<< "+(Environment.TickCount-st));
            debugmsg += "End print<<<<<<<<<<<<<<<<<< " + (Environment.TickCount - st)+"\r\n";
        }
        private void Form_print_Load(object sender, EventArgs e)
        {
            Option.MT = true;
            Option.POW = 0;
            Option.ST = 0;
            Option.ED = 0;
            Option.BOX = true;
            lhaveinit = new List<string>();
            readyinit = new List<string>();
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
            button4.Enabled = true;
            
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            Random r=new Random(Environment.TickCount+DateTime.Now.Millisecond);

            string b=string.Empty, a=string.Empty;

            for (int i = 0; i < 4; i++)
            {
                int n=r.Next(0, 16);
                b+=n.ToString("X");
            }

            for (int i = 0; i < 4; i++)
            {
                int n = r.Next(0, 16);
                a += n.ToString("X");
            }

            textBox_before.Text = b;
            textBox_after.Text = a;
        }
    }
}
