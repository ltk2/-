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

using System.Threading;
using AppAlgoLib;

namespace ModuleReaderManager
{
    public partial class Form_anterea : Form
    {
        public Form_anterea(Reader rdr, ReaderParams param, Form1 frm)
        {
            InitializeComponent();
            mordr = rdr;
            rparam = param;
            Mfrm = frm;
        }
        ReaderParams rparam = null;
        Reader mordr = null;
        Form1 Mfrm = null;
        Dictionary<int, CheckBox> allAnts = new Dictionary<int, CheckBox>();
        bool isInventory = false;
        int starttm;
        Thread readThread = null;
        bool finish = false;
        delegate void OpFailedHandler(Exception ex);
        Thread timerControlThread = null;
        bool istimerct;
        int endatetime;
        delegate void TimerHandleDelg(int type, object paramslist);
        IAsyncResult iar_timer;
        AntArea AA = new AntArea();
        bool isantgroup;//是否启用天线组
        Dictionary<int, List<int>> dic_areatoants = new Dictionary<int, List<int>>();
        Dictionary<int, int> dic_anttoarea = new Dictionary<int, int>();
        bool islog = false;
        Mutex Lock_thread = new Mutex();

        /************************************************************************/
        /* 测试1 功率变化测试                                                                     */
        /************************************************************************/
        bool test_pwch_is;
        int test_pwch_cyc;
        int test_pwch_stpw;
        int test_pwch_edpw;
        int[] allantpow;
        int test_pwch_adpw;
        DateTime test_pwch_dt;
        Dictionary<int, List<string>> dils;
        int option;
        void ShowOpFailedMsg(Exception ex)
        {
            string mex = "";
            if (ex is ModuleException)
                mex = ((ModuleException)ex).ErrCode.ToString("X4");
            richTextBox_results.Text = ex.Message+"\r\n"+ex.StackTrace + " errcode:" + mex + "\r\n";

           button_antereajuge.PerformClick();
        }
        private void Form_anterea_Load(object sender, EventArgs e)
        {
           
            allAnts.Add(1, cbant1);
            allAnts.Add(2, cbant2);
            allAnts.Add(3, cbant3);
            allAnts.Add(4, cbant4);
            allAnts.Add(5, cbant5);
            allAnts.Add(6, cbant6);
            allAnts.Add(7, cbant7);
            allAnts.Add(8, cbant8);
            allAnts.Add(9, cbant9);
            allAnts.Add(10, cbant10);
            allAnts.Add(11, cbant11);
            allAnts.Add(12, cbant12);
            allAnts.Add(13, cbant13);
            allAnts.Add(14, cbant14);
            allAnts.Add(15, cbant15);
            allAnts.Add(16, cbant16);

            for (int f = 1; f <= allAnts.Count; ++f)
                allAnts[f].Enabled = false;
           
                for (int aa = 1; aa <= Mfrm.readerantnumber; ++aa)
                {
                    allAnts[aa].Enabled = true;
                    allAnts[aa].ForeColor = Color.Red;
                }


                try
                {
                    //获取连接天线
                    //if (readerantnumber != 16)
                    //{
                    int[] connectedants = (int[])mordr.ParamGet("ConnectedAntennas");
                    for (int c = 0; c < connectedants.Length; ++c)
                        allAnts[connectedants[c]].ForeColor = Color.Green;

                    for (int ff = 1; ff <= allAnts.Count; ++ff)
                    {
                        if (allAnts[ff].Enabled)
                        {
                            if (allAnts[ff].ForeColor == Color.Green)
                            {
                                allAnts[ff].Checked = true;

                            }

                        }
                    }
                }
                catch (System.Exception ex)
                {

                }
                dils = new Dictionary<int, List<string>>();
        }
        /// <summary>
        /// 检查天线是否可用
        /// </summary>
        /// <returns></returns>
        private List<AntAndBoll> CheckAntsValid()
        {
            List<AntAndBoll> selants = new List<AntAndBoll>();

            for (int cc = 1; cc <= allAnts.Count; ++cc)
            {
                if (allAnts[cc].Checked)
                {
                    if (allAnts[cc].ForeColor == Color.Red)
                        selants.Add(new AntAndBoll(cc, false));
                    else
                        selants.Add(new AntAndBoll(cc, true));
                }
            }

            return selants;
        }
        private void button_antereajuge_Click(object sender, EventArgs e)
        {
             
            if (button_antereajuge.Text == "开始识别")
            {
               
                dils.Clear();

                richTextBox_results.Text = "";
                AA.Clear();
                if (checkBox_firstrssi.Checked)
                    AA.FirstCompare = AA.First_Cmp_Rssi;
                else
                    AA.FirstCompare = AA.First_Cmp_Count;

                islog=false;
                if (checkBox_log.Checked)
                    islog = true;

                List<AntAndBoll> selants = null;

                selants = CheckAntsValid();
                if (selants.Count == 0)
                {
                    MessageBox.Show("请选择天线");
                    return;
                }
                List<int> antsExe = new List<int>();
                for (int i = 0; i < selants.Count; ++i)
                {
                    antsExe.Add(selants[i].antid);
                }
                if (checkBox_antgroup.Checked)
                {
                    isantgroup = true;
                    try
                    {
                        dic_anttoarea.Clear();
                        dic_areatoants.Clear();

                        string[] areaant = textBox_antgroup.Text.Split(new char[] { ';' });

                        for (int i = 0; i < areaant.Length; i++)
                        {
                            if (areaant[i] != string.Empty)
                            {
                                string[] area_ant = areaant[i].Split(new char[] { ':' });
                                string[] ants = area_ant[1].Split(new char[] { ',' });
                                List<int> lants = new List<int>();
                                for (int k = 0; k < ants.Length; k++)
                                    lants.Add(int.Parse(ants[k]));
                                dic_areatoants.Add(int.Parse(area_ant[0]), lants);

                            }
                        }

                        foreach (KeyValuePair<int, List<int>> kvp in dic_areatoants)
                        {
                            for (int i = 0; i < kvp.Value.Count; i++)
                            {
                                dic_anttoarea.Add(kvp.Value[i], kvp.Key);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("天线组合配置错误:" + ex.Message);
                        return;
                    }
                }
                else
                    isantgroup = false;
                try
                {
                    AA.PCount = double.Parse(textBox_pcount.Text);
                    AA.PRssi = double.Parse(textBox_prssi.Text);
                    mordr.ParamSet("ReadPlan", new SimpleReadPlan(TagProtocol.GEN2, antsExe.ToArray()));
                    endatetime = int.Parse(textBox_antereatime.Text);
                    AA.SetMaxantareaMaxcount(int.Parse(textBox_maxc.Text));
                    
                    test_pwch_is = checkBox_powchtest.Checked;

                    if (test_pwch_is)
                    {
                        try
                        {
                            test_pwch_stpw = int.Parse(textBox_powsttval.Text);
                            test_pwch_cyc = int.Parse(textBox_antereatime.Text);
                            test_pwch_adpw = int.Parse(textBox_powchval.Text);
                            test_pwch_edpw = int.Parse(textBox_powendval.Text);
                            test_pwch_dt = DateTime.Now;

                            allantpow = new int[Mfrm.readerantnumber];
                            for (int i = 0; i < Mfrm.readerantnumber; i++)
                                allantpow[i] = test_pwch_stpw;

                            AntPower[] dicpwrs = new AntPower[Mfrm.readerantnumber];
                           
                            for (int i = 0; i < Mfrm.readerantnumber; i++)
                            {
                                AntPower tmppwr = new AntPower();
                                tmppwr.AntId = (byte)(i + 1);

                                tmppwr.ReadPower = (ushort)allantpow[i];
                                tmppwr.WritePower = (ushort)allantpow[i];
                                dicpwrs[i] = tmppwr;
                            }
                           
                            mordr.ParamSet("AntPowerConf", dicpwrs);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("功率测试参数有误");
                            return;
                        }
                      
                    }

                    if (Mfrm.rParms.isFastRead)
                    {
                        int metaflag = 0;
                         option = 0;
                        if (Mfrm.rParms.FRTMeta.IsAntennaID)
                            metaflag |= 0X0004;
                        if (Mfrm.rParms.FRTMeta.IsEmdData)
                            metaflag |= 0X0080;
                        if (Mfrm.rParms.FRTMeta.IsTimestamp)
                            metaflag |= 0X0010;
                        if (Mfrm.rParms.FRTMeta.IsFrequency)
                            metaflag |= 0X0008;
                        if (Mfrm.rParms.FRTMeta.IsRFU)
                            metaflag |= 0X0020;
                        //if (Mfrm.rParms.FRTMeta.IsRSSI)
                            metaflag |= 0X0002;
                        //if (Mfrm.rParms.FRTMeta.IsReadCnt)
                            metaflag |= 0X0001;

                        option = metaflag << 8;
                        if (Mfrm.rParms.is1200hdstmd)
                            option |= 0x10; ;

                        System.Console.WriteLine("AsyncStartReading》》》");
                        mordr.AsyncStartReading(option);
                      
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("启动读取失败:" + ex.ToString());
                    return;
                }

                isInventory = true;
                starttm = Environment.TickCount;
                readThread = new Thread(ReadFunc);
                readThread.Start();

                istimerct = true;
                timerControlThread = new Thread(timerControl);

                timerControlThread.Start();
                button_antereajuge.Text = "停止";
            }
            else
            {
             
                try
                {
                    System.Console.WriteLine("AsyncStopReading");
                    if (Mfrm.rParms.isFastRead)
                    {
                        Lock_thread.WaitOne();
                        mordr.AsyncStopReading();
                    }
                }
                catch (ModuleException mex)
                {
                    richTextBox_results.Text += mex.ErrCode.ToString("X4") + mex.Message + "\r\n";
                }
                finally
                {
                    if (Mfrm.rParms.isFastRead)
                    Lock_thread.ReleaseMutex();
                }
                istimerct = false;
                isInventory = false;

                if (iar_timer != null)
                    this.EndInvoke(iar_timer);

                readThread.Join();

                try
                {
                    if (!checkBox_powchtest.Checked)
                        dils = AA.AnysisData();
                    
                        AA.ClearFilterTags();
                        AA.ClearFilterAnts();

                    foreach (KeyValuePair<int, List<string>> kvp in dils)
                    {
                        if (isantgroup)
                        {
                            string antstr="(";
                            for(int i=0;i<dic_areatoants[kvp.Key].Count;i++)
                                antstr+=dic_areatoants[kvp.Key][i]+" ";
                            antstr += ")";
                            string temp = "区域:" + kvp.Key+antstr + " 标签:" + kvp.Value.Count + "\r\n";
                            for (int i = 0; i < kvp.Value.Count; i++)
                                temp += kvp.Value[i] + "\r\n";
                            temp += "\r\n";
                            richTextBox_results.Text += temp;
                        }
                        else
                        {
                            string temp = "天线:" + kvp.Key + " 标签:" + kvp.Value.Count + "\r\n";
                            for (int i = 0; i < kvp.Value.Count; i++)
                                temp += kvp.Value[i] + "\r\n";
                            temp += "\r\n";
                            richTextBox_results.Text += temp;
                        }
 
                    }
                    if(checkBox_log.Checked)
                    richTextBox_results.Text += "\r\n" + AA.Log;
                }
                catch (Exception ex)
                {
                    string mex = "";
                    if (ex is ModuleException)
                        mex = ((ModuleException)ex).ErrCode.ToString("X4");
                    richTextBox_results.Text = ex.Message+ex.StackTrace + " " + mex + "\r\n"; 
                }

                button_antereajuge.Text = "开始识别";

                if (checkBox_repeate.Checked)
                { System.Threading.Thread.Sleep(1000); 
                    button_antereajuge.PerformClick();
                }
            }
        }

        void ReadFunc()
        {

            while (isInventory)
            {
                finish = false;
                try
                {
                    TagReadData[] reads = null;
                    System.Console.WriteLine("AsyncGetTags");
                    if (Mfrm.rParms.isFastRead)
                    {
                        try
                        {
                            Lock_thread.WaitOne();
                            reads = mordr.AsyncGetTags();
                        }
                        catch
                        { }
                        finally
                        { Lock_thread.ReleaseMutex(); }
                    }
                    else
                        reads = mordr.Read(Mfrm.rParms.readdur);

                    finish = true;
                    if (reads!=null)
                    {
                        foreach (TagReadData read in reads)
                        {
                            // Console.WriteLine("ReadCount: " + read.ReadCount);
                            TagDInfo td = new TagDInfo();
                            if (isantgroup)
                                td.Antenna = dic_anttoarea[read.Antenna];
                            else
                                td.Antenna = read.Antenna;
                            td.CRC = read.CRC;
                            td.EMDDataString = read.EMDDataString;
                            td.EPCString = read.EPCString;
                            td.Frequency = read.Frequency;
                            td.PC = read.PC;
                            td.Phase = read.Phase;
                            td.ReadCount = read.ReadCount;
                            td.ReadOffset = read.ReadOffset;
                            td.Rssi = read.Rssi;
                            td.Time = read.Time;
                            AA.AddTagTo(td);
                        }
                    }
                    Thread.Sleep(Mfrm.rParms.sleepdur);
                }
                catch (Exception ex)
                {
                    if (istimerct)
                        this.BeginInvoke(new OpFailedHandler(ShowOpFailedMsg), ex);
                    return;
                }
            }
        }

       

        void TimerHandle(int type, object paramslist)
        {
           
                if (type == 0)
                {
                    richTextBox_results.Text += paramslist.ToString() + "\r\n";
                }
                else if (type == 1)
                {
                    button_antereajuge.PerformClick();
                }
            
        }
        void timerControl()
        {
            string debugmes = "";
            try
            {
                while (istimerct)
                {
                    debugmes = "";
                    if (test_pwch_is)
                    {
                        TimeSpan ts = DateTime.Now - test_pwch_dt;
                        if (ts.TotalMilliseconds > test_pwch_cyc)
                        {
                            
                            isInventory = false;
                            while (!finish)
                                Thread.Sleep(10);

                            if (Mfrm.rParms.isFastRead)
                            {
                                try
                                {
                                    debugmes += "AsyncStopReading>";
                                    Lock_thread.WaitOne();
                                    mordr.AsyncStopReading();
                                }
                                catch (ModuleException mex) { throw mex; }
                                finally
                                {
                                    Lock_thread.ReleaseMutex();
                                }
                            }

                            readThread.Join();


                            Dictionary<int, List<string>> temp = AA.AnysisData();
                            Dictionary<string, bool> dsb = AA.Dic_diffTags;
                            List<int> lants = new List<int>();
                            lants.AddRange(AA.L_diffAnts.ToArray());

                            foreach (KeyValuePair<int, List<string>> kvp in temp)
                            {
                                for (int i = 0; i < kvp.Value.Count; i++)
                                {
                                    if (dsb.ContainsKey(kvp.Value[i]))
                                    {

                                        if (dsb[kvp.Value[i]])
                                        {
                                            if (!this.dils.ContainsKey(kvp.Key))
                                                this.dils.Add(kvp.Key, new List<string>());
                                            if (!dils[kvp.Key].Contains(kvp.Value[i]) && dils[kvp.Key].Count < AA.AntMaxcount)
                                            {
                                                dils[kvp.Key].Add(kvp.Value[i]); AA.AddFilterTag(kvp.Value[i]);
                                                if (dils[kvp.Key].Count == AA.AntMaxcount)
                                                    AA.AddFilterAnt(kvp.Key);
                                            }
                                        }
                                    }
                                }
                            }


                            if (islog)
                                iar_timer = this.BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 0, AA.Log });


                            AA.Clear();

                            //找出需要


                            if (test_pwch_stpw >= test_pwch_edpw)
                            {
                                iar_timer = this.BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 1, (Environment.TickCount - starttm).ToString() });
                                istimerct = false;
                                continue;
                            }

                            AntPower[] dicpwrs = new AntPower[Mfrm.readerantnumber];
                            debugmes += "lants count:" + lants.Count + " ";
                            if (lants.Count > 0)
                            {
                                if (isantgroup)
                                {
                                    for (int m = 0; m < lants.Count; m++)
                                    {
                                        List<int> redcpwants = dic_areatoants[m];

                                        for (int i = 0; i < Mfrm.readerantnumber; i++)
                                        {

                                            if (redcpwants.Contains(i + 1))
                                            {
                                                allantpow[i] -= test_pwch_adpw;
                                                //debugmes += " -pow:" +(i+1)+" "+ allantpow[i];
                                                if (allantpow[i] < 500)
                                                {
                                                    throw new Exception("未能辨别标签，请修改辨别参数");

                                                }
                                            }

                                            AntPower tmppwr = new AntPower();
                                            tmppwr.AntId = (byte)(i + 1);

                                            tmppwr.ReadPower = (ushort)allantpow[i];
                                            tmppwr.WritePower = (ushort)allantpow[i];
                                            dicpwrs[i] = tmppwr;
                                        }
                                    } 
                                 
                                }
                                else
                                {
                                    for (int i = 0; i < Mfrm.readerantnumber; i++)
                                    {
                                        if (lants.Contains(i + 1))
                                        {
                                            allantpow[i] -= test_pwch_adpw;
                                            //debugmes += " -pow:" +(i+1)+" "+ allantpow[i];
                                            if (allantpow[i] < 500)
                                            {
                                                throw new Exception("未能辨别标签，请修改辨别参数");

                                            }
                                        }

                                        AntPower tmppwr = new AntPower();
                                        tmppwr.AntId = (byte)(i + 1);

                                        tmppwr.ReadPower = (ushort)allantpow[i];
                                        tmppwr.WritePower = (ushort)allantpow[i];
                                        dicpwrs[i] = tmppwr;
                                    }
                                }
                            }
                            else
                            {
                                test_pwch_stpw += test_pwch_adpw;
                                
                                if (test_pwch_stpw > test_pwch_edpw)
                                {
                                    test_pwch_stpw = test_pwch_edpw;
                                }

                                for (int i = 0; i < Mfrm.readerantnumber; i++)
                                {

                                    allantpow[i] = test_pwch_stpw;

                                    AntPower tmppwr = new AntPower();
                                    tmppwr.AntId = (byte)(i + 1);

                                    tmppwr.ReadPower = (ushort)allantpow[i];
                                    tmppwr.WritePower = (ushort)allantpow[i];
                                    dicpwrs[i] = tmppwr;
                                }
                            }

                            try
                            {
                                debugmes += "AntPowerConf> " + dicpwrs.Length + " allpw:";
                                for (int m = 0; m < dicpwrs.Length; m++)
                                    debugmes += dicpwrs[m].ReadPower.ToString() + ",";
                                
                                Lock_thread.WaitOne();
                                mordr.ParamSet("AntPowerConf", dicpwrs);

                                if (Mfrm.rParms.isFastRead && istimerct)
                                {
                                    debugmes += "AsyncStartReading>";
                                    mordr.AsyncStartReading(option);
                                }
                            }
                            catch (ModuleException mex) { throw mex; }
                            finally
                            {
                                Lock_thread.ReleaseMutex();

                            }

                            if (istimerct)
                            {
                                isInventory = true;

                                readThread = new Thread(ReadFunc);
                                readThread.Start();
                                test_pwch_dt = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        if (Environment.TickCount - starttm > endatetime)
                        {
                            iar_timer = this.BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 1, (Environment.TickCount - starttm).ToString() });
                            istimerct = false;
                        }
                    }

                    Thread.Sleep(50);
                }
            }
            catch (ModuleException mex)
            {
                string mexstr = "MEX in timectl " + mex.ErrCode.ToString("X4") + mex.Message + mex.StackTrace + debugmes;
                 iar_timer = this.BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 0, mexstr});
            }
            catch (Exception ex)
            {
                string exstr = "EX in timectl " + ex.Message + ex.StackTrace + debugmes;
                iar_timer = this.BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 0, exstr });
            }
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            bool istest = true;
            richTextBox_results.Text = "";
            AA.Clear();
            AA.SetMaxantareaMaxcount(int.Parse(textBox_maxc.Text));
            TagDInfo td = new TagDInfo();
            td.Antenna = 11;
            td.EPCString = "B0000002";
            td.ReadCount = 9;
            td.Rssi = -318;
            DateTime dt = new DateTime(2020, 8, 26, 17, 5, 9, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 14;
            td.EPCString = "B0000002";
            td.ReadCount = 6;
            td.Rssi = -347;
            dt = new DateTime(2020, 8, 26, 17, 5, 13, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 16;
            td.EPCString = "B0000002";
            td.ReadCount = 7;
            td.Rssi = -412;
            dt = new DateTime(2020, 8, 26, 17, 5, 2, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 8;
            td.EPCString = "B0000002";
            td.ReadCount = 6;
            td.Rssi = -350;
            dt = new DateTime(2020, 8, 26, 17, 5, 18, 0);
            td.Time = dt;
            AA.AddTagTo(td);


            td = new TagDInfo();
            td.Antenna = 11;
            td.EPCString = "D0000004";
            td.ReadCount = 9;
            td.Rssi = -388;
            dt = new DateTime(2020, 8, 26, 17, 5, 20, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 8;
            td.EPCString = "D0000004";
            td.ReadCount = 7;
            td.Rssi = -376;
            dt = new DateTime(2020, 8, 26, 17, 5, 20, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 14;
            td.EPCString = "D0000004";
            td.ReadCount = 9;
            td.Rssi = -345;
            dt = new DateTime(2020, 8, 26, 17, 5, 20, 0);
            td.Time = dt;
            AA.AddTagTo(td);


            td = new TagDInfo();
            td.Antenna = 16;
            td.EPCString = "D0000004";
            td.ReadCount = 7;
            td.Rssi = -272;
            dt = new DateTime(2020, 8, 26, 17, 5, 28, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 14;
            td.EPCString = "C0000003";
            td.ReadCount = 8;
            td.Rssi = -298;
            dt = new DateTime(2020, 8, 26, 17, 5, 26, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 16;
            td.EPCString = "C0000003";
            td.ReadCount = 8;
            td.Rssi = -361;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 8;
            td.EPCString = "C0000003";
            td.ReadCount = 7;
            td.Rssi = -261;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 11;
            td.EPCString = "A0000001";
            td.ReadCount = 11;
            td.Rssi = -426;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 8;
            td.EPCString = "A0000001";
            td.ReadCount = 7;
            td.Rssi = -259;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 16;
            td.EPCString = "A0000001";
            td.ReadCount = 6;
            td.Rssi = -343;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 14;
            td.EPCString = "A0000001";
            td.ReadCount = 9;
            td.Rssi = -394;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);



            td = new TagDInfo();
            td.Antenna = 16;
            td.EPCString = "00B07A136C026DC800001A6E";
            td.ReadCount = 7;
            td.Rssi = -483;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 8;
            td.EPCString = "00B07A136C026DC800001A6E";
            td.ReadCount = 6;
            td.Rssi = -394;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 8;
            td.EPCString = "A0000028";
            td.ReadCount = 4;
            td.Rssi = -285;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 8;
            td.EPCString = "E20020180905007689000238";
            td.ReadCount = 1;
            td.Rssi = -74;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);

            td = new TagDInfo();
            td.Antenna = 8;
            td.EPCString = "E28011700000020A16C084AD";
            td.ReadCount = 2;
            td.Rssi = -141;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);


            td = new TagDInfo();
            td.Antenna = 8;
            td.EPCString = "300033B2DDD9010000000509";
            td.ReadCount = 2;
            td.Rssi = -137;
            dt = new DateTime(2020, 8, 26, 17, 5, 27, 0);
            td.Time = dt;
            AA.AddTagTo(td);
            Dictionary<int, List<string>> dils = AA.AnysisData();



            foreach (KeyValuePair<int, List<string>> kvp in dils)
            {
                string temp = "天线:" + kvp.Key + "\r\n";
                for (int i = 0; i < kvp.Value.Count; i++)
                    temp += kvp.Value[i] + "\r\n";
                temp += "\r\n";
                richTextBox_results.Text += temp;
            }

            if (checkBox_log.Checked)
                richTextBox_results.Text += "\r\n" + AA.Log;
            if (istest)
                return;
        }

        private void button_reanysis_Click(object sender, EventArgs e)
        {
            try
            {
                AA.SetMaxantareaMaxcount(int.Parse(textBox_maxc.Text));

                if (checkBox_firstrssi.Checked)
                    AA.FirstCompare = AA.First_Cmp_Rssi;
                else
                    AA.FirstCompare = AA.First_Cmp_Count;


                richTextBox_results.Text = "";
                AA.PreReAnysisData();
                Dictionary<int, List<string>> dils = AA.AnysisData();
                foreach (KeyValuePair<int, List<string>> kvp in dils)
                {
                    string temp = "天线:" + kvp.Key + "\r\n";
                    for (int i = 0; i < kvp.Value.Count; i++)
                        temp += kvp.Value[i] + "\r\n";
                    temp += "\r\n";
                    richTextBox_results.Text += temp;

                }
                if (checkBox_log.Checked)
                    richTextBox_results.Text += "\r\n" + AA.Log;
            }
            catch (Exception ex)
            {
                string mex = "";
                if (ex is ModuleException)
                    mex = ((ModuleException)ex).ErrCode.ToString("X4");
                richTextBox_results.Text = ex.Message + ex.StackTrace + " " + mex + "\r\n";
            }
        }
    }
}
