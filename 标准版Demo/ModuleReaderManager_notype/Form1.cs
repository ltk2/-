//#define TEMPERATRATURE
//#define NOHIGHSPEED
//#define NORECON
//#define EMDDATA

using Microsoft.Win32;
using ModuleLibrary;
using ModuleTech;
using ModuleTech.Gen2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    public partial class Form1 : Form
    {
        //ProgramLog taginfolog = null;
        private ListViewItem[] myCache; //array to cache items for the virtual list
        private int firstItem; //stores the index of the first item in the cache
        public Form1()
        {
            InitializeComponent();
            
            lvTags.VirtualMode = true;
            lvTags.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(listView1_RetrieveVirtualItem);
            lvTags.CacheVirtualItems += new CacheVirtualItemsEventHandler(listView1_CacheVirtualItems);
            lvTags.SearchForVirtualItem += new SearchForVirtualItemEventHandler(listView1_SearchForVirtualItem);

            //Add ListView to the form.
            this.Controls.Add(lvTags);

            //Search for a particular virtual item.
            //Notice that we never manually populate the collection!
            //If you leave out the SearchForVirtualItem handler, this will return null.
            ListViewItem lvi = lvTags.FindItemWithText("111111");

            //Select the item found and scroll it into view.
            if (lvi != null)
            {
                lvTags.SelectedIndices.Add(lvi.Index);
                lvTags.EnsureVisible(lvi.Index);
            }
        }

        //检测版本更新
        public void detectionVersionUpdatingIs() {
            try
            {
               
                if (FtpHelper.IsInternetAvailable())
                {
                    var ftpUrl = ConfigHelper.GetAppConfig("ftpUrl");
                    FtpHelper.path = "ftp://" + ftpUrl;
                    var versions = ConfigHelper.GetAppConfig("versions");
                    var list = FtpHelper.GetFtpFileInfos("", "ModuleReaderManager");
                    list = list.OrderByDescending(r => r.FileName).ToArray();
                    if (list != null && list[0].FileName != "." && list[0].FileName != "..")
                    {
                        Version v1 = new Version(list[0].FileName);
                        Version v2 = new Version(versions);
                        if (v1 > v2)
                        {
                            menuitemlog.RedDotHint = true;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        //The basic VirtualMode function.  Dynamically returns a ListViewItem
        //with the required properties; in this case, the square of the index.
        void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            //Caching is not required but improves performance on large sets.
            //To leave out caching, don't connect the CacheVirtualItems event 
            //and make sure myCache is null.

            //check to see if the requested item is currently in the cache
            if (myCache != null && e.ItemIndex >= firstItem && e.ItemIndex < firstItem + myCache.Length)
            {
                //A cache hit, so get the ListViewItem from the cache instead of making a new one.
                e.Item = myCache[e.ItemIndex - firstItem];
            }
            else
            {
                //A cache miss, so create a new ListViewItem and pass it back.
                int x = e.ItemIndex * e.ItemIndex;
                e.Item = new ListViewItem(x.ToString());
            }
        }

        //Manages the cache.  ListView calls this when it might need a 
        //cache refresh.
        void listView1_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            //We've gotten a request to refresh the cache.
            //First check if it's really neccesary.
            if (myCache != null && e.StartIndex >= firstItem && e.EndIndex <= firstItem + myCache.Length)
            {
                //If the newly requested cache is a subset of the old cache, 
                //no need to rebuild everything, so do nothing.
                return;
            }

            //Now we need to rebuild the cache.
            firstItem = e.StartIndex;
            int length = e.EndIndex - e.StartIndex + 1; //indexes are inclusive
            myCache = new ListViewItem[length];

            //Fill the cache with the appropriate ListViewItems.
            int x = 0;
            for (int i = 0; i < length; i++)
            {
                x = (i + firstItem) * (i + firstItem);
                myCache[i] = new ListViewItem(x.ToString());
            }
        }

        //This event handler enables search functionality, and is called
        //for every search request when in Virtual mode.
        void listView1_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            //We've gotten a search request.
            //In this example, finding the item is easy since it's
            //just the square of its index.  We'll take the square root
            //and round.
            double x = 0;
            if (Double.TryParse(e.Text, out x)) //check if this is a valid search
            {
                x = Math.Sqrt(x);
                x = Math.Round(x);
                e.Index = (int)x;
            }
            //If e.Index is not set, the search returns null.
            //Note that this only handles simple searches over the entire
            //list, ignoring any other settings.  Handling Direction, StartIndex,
            //and the other properties of SearchForVirtualItemEventArgs is up
            //to this handler.
        }

        Dictionary<string, TagInfo> m_Tags = new Dictionary<string, TagInfo>();
        List<AntCount> antList = new List<AntCount>();
        Mutex tagmutex = new Mutex();
        public ReaderParams rParms = new ReaderParams(200, 0, 1);

        bool isInventory = false;
        bool isConnect = false;
        int selantsAnt = 0;//5300启用时的天线id
        bool isStopanyerror = true;

        public AntCount setAntCount(AntCount model, string epcId, int antid, int count, out string text)
        {
            switch (antid)
            {
                case 1:
                    model.ant1 += count;
                    break;
                case 2:
                    model.ant2 += count;
                    break;
                case 3:
                    model.ant3 += count;
                    break;
                case 4:
                    model.ant4 += count;
                    break;
                case 5:
                    model.ant5 += count;
                    break;
                case 6:
                    model.ant6 += count;
                    break;
                case 7:
                    model.ant7 += count;
                    break;
                case 8:
                    model.ant8 += count;
                    break;
                case 9:
                    model.ant9 += count;
                    break;
                case 10:
                    model.ant10 += count;
                    break;
                case 11:
                    model.ant11 += count;
                    break;
                case 12:
                    model.ant12 += count;
                    break;
                case 13:
                    model.ant13 += count;
                    break;
                case 14:
                    model.ant14 += count;
                    break;
                case 15:
                    model.ant15 += count;
                    break;
                case 16:
                    model.ant16 += count;
                    break;
            }
            //自动补充空格
            string ant1 = model.ant1.ToString().PadRight(3);
            string ant2 = model.ant2.ToString().PadRight(3);
            string ant3 = model.ant3.ToString().PadRight(3);
            string ant4 = model.ant4.ToString().PadRight(3);
            //自动补充空格
            string ant5 = model.ant5.ToString().PadRight(3);
            string ant6 = model.ant6.ToString().PadRight(3);
            string ant7 = model.ant7.ToString().PadRight(3);
            string ant8 = model.ant8.ToString().PadRight(3);
            //自动补充格
            string ant9 = model.ant9.ToString().PadRight(3);
            string ant10 = model.ant10.ToString().PadRight(3);
            string ant11 = model.ant11.ToString().PadRight(3);
            string ant12 = model.ant12.ToString().PadRight(3);
            //自动补充空格
            string ant13 = model.ant13.ToString().PadRight(3);
            string ant14 = model.ant14.ToString().PadRight(3);
            string ant15 = model.ant15.ToString().PadRight(3);
            string ant16 = model.ant16.ToString().PadRight(3);

            //text = $"{ant1}/{ant2}/{ant3}/{ant4}/{ant5}/{ant6}/{ant7}/{ant8}/{ant9}/{ant10}/{ant11}/{ant12}/{ant13}/{ant14}/{ant15}/{ant16}";
            text = ant1 + "/" + ant2 + "/" + ant3 + "/" + ant4 + "/" + ant5 + "/" + ant6 + "/" + ant7 + "/" + ant8 + "/" + ant9 + "/" + ant10
                + "/" + ant11 + "/" + ant12 + "/" + ant13 + "/" + ant14 + "/" + ant15 + "/" + ant16;
            return model;
        }

        public List<string> Epcs
        {
            get
            {
                List<string> epcs = new List<string>();
                foreach (ListViewItem viewitem in taglvdic.Values)
                {
                    string wline = viewitem.SubItems[2].Text;
                    epcs.Add(wline);
                }
                return epcs;
            }
        }

        /// <summary>
        /// 将每次盘点到的新标签加入本地缓冲m_Tags中，已经盘点次数累加
        /// </summary>
        /// <param name="tag"></param>
        void AddTagToDic(TagReadData tag)
        {
            //System.Diagnostics.Debug.WriteLine("tag:" + tag.EPCString + " count:" + tag.ReadCount +" ant:" + tag.Antenna + " Rssi:" + tag.Rssi);
            TagInfo tmptag = null;
            if (rParms.isUniByEmd && (tag.Tag.Protocol == TagProtocol.GEN2))
            {
                if (tag.EMDDataString == string.Empty)
                {
                    return;
                }
            }
            //
            tagmutex.WaitOne();

            string keystr = tag.EPCString;

            if (rParms.isUniByEmd)
            {
                keystr += tag.EMDDataString;
            }

            if (rParms.isUniByAnt)
            {
                keystr += tag.Antenna.ToString();
            }

            if (m_Tags.ContainsKey(keystr))
            {
                tmptag = m_Tags[keystr];
                if (rParms.isOneReadOneTime)
                {
                    tmptag.readcnt += 1;
                }
                else
                {
                    tmptag.readcnt += tag.ReadCount;
                }

                tmptag.Frequency = tag.Frequency;
                tmptag.RssiRaw = tag.Rssi;
                tmptag.Phase = tag.Phase;
                tmptag.antid = tag.Antenna;

                if (!rParms.isUniByEmd)
                {
                    if (tmptag.emddatastr != tag.EMDDataString)
                    {
                        if (tag.EMDDataString != string.Empty)
                        {
                            tmptag.emddatastr = tag.EMDDataString;
                        }
                    }
                }

                //added on 3-26
                if (rParms.isIdtAnts)
                {
                    TimeSpan span = tag.Time - tmptag.timestamp;
                    if (tag.Rssi <= 0)
                        tmptag.RssiSum += (tag.Rssi + 120) * tag.ReadCount * ((int)span.TotalMilliseconds / 80);
                    else
                        tmptag.RssiSum += tag.Rssi * tag.ReadCount * ((int)span.TotalMilliseconds / 80);
                }

                tmptag.timestamp = tag.Time;
            }
            else
            {
                TagInfo newtag = null;
                if (rParms.isOneReadOneTime)
                {
                    newtag = new TagInfo(tag.EPCString, 1, tag.Antenna, tag.Time,
                        tag.Tag.Protocol, tag.EMDDataString);
                    newtag.RssiRaw = tag.Rssi;
                    newtag.Phase = tag.Phase;
                    newtag.Frequency = tag.Frequency;
                }
                else
                {
                    newtag = new TagInfo(tag.EPCString, tag.ReadCount, tag.Antenna, tag.Time,
                        tag.Tag.Protocol, tag.EMDDataString);
                    newtag.RssiRaw = tag.Rssi;
                    newtag.Phase = tag.Phase;
                    newtag.Frequency = tag.Frequency;
                }

                newtag.Phase = tag.Phase;
                m_Tags.Add(keystr, newtag);

                //added on 3-26
                if (rParms.isIdtAnts)
                {
                    if (tag.Rssi <= 0)
                    {
                        newtag.RssiSum += (tag.Rssi + 120) * tag.ReadCount;
                    }
                    else
                    {
                        newtag.RssiSum += tag.Rssi * tag.ReadCount;
                    }
                }
            }

            tagmutex.ReleaseMutex();
        }

        public Reader modulerdr = null;

        delegate void ReconnectHandler(int reason, Exception ex);

        string logstr = "";

#if NORECON
       ReaderExceptionChecker rechecker = new ReaderExceptionChecker(100, 60);
#else
        ReaderExceptionChecker rechecker = new ReaderExceptionChecker(4, 60);
#endif

        Thread timerControlThread = null;
        bool istimerct;

        delegate void TimerHandleDelg(int type, object paramslist);
        IAsyncResult iar_timer;

        void TimerHandle(int type, object paramslist)
        {
            if (type == 0)
            {
                //rParms.endatetime > 0 && Environment.TickCount - rParms.endatetime > 0
                //rtbopfailmsg.Text+=$"endatetime:{rParms.endatetime}------{Timestamp() - rParms.endatetime}";
                //rtbopfailmsg.Text += "\n\r";
                labreadtime.Text = paramslist.ToString();
            }
            else if (type == 1)
            {
                labreadtime.Text = paramslist.ToString();
                modulerdr.StopReading();
                btnstop.PerformClick();
            }
            else if (type == 2) {
                selantsAnt = Convert.ToInt32(paramslist);
            }
        }

        void timerControl()
        {
            int ant = 0;
            long starttmV2 = starttm;
            while (istimerct)
            {
                iar_timer = BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 0, (Timestamp() - starttm).ToString() });
                if (rParms.endatetime > 0 && Timestamp() - rParms.endatetime > 0)
                {
                    iar_timer = BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 1, (Timestamp() - starttm).ToString() });
                    istimerct = false;
                }

                else if (rParms.antForTime > 0 && Timestamp() - (starttmV2 + rParms.antForTime) > 0)
                {
                    UiMux.WaitOne();
                    modulerdr.StopReading();
                    if (iar_timer != null)
                    {
                        EndInvoke(iar_timer);
                    }
                    UiMux.ReleaseMutex();

                    if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                    {
                        iar_timer = BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] {2, selants[ant].antid });
                        if (selants[ant].antid == 4)
                        {
                            modulerdr.GPOSet(1, false);
                            modulerdr.GPOSet(2, false);
                        }
                        else if (selants[ant].antid == 3)
                        {
                            modulerdr.GPOSet(1, true);
                            modulerdr.GPOSet(2, false);
                        }
                        else if (selants[ant].antid == 2)
                        {
                            modulerdr.GPOSet(1, true);
                            modulerdr.GPOSet(2, true);
                        }
                        else if (selants[ant].antid == 1)
                        {
                            modulerdr.GPOSet(1, false);
                            modulerdr.GPOSet(2, true);
                        }
                    }
                    else { 
                        List<SimpleReadPlan> readplans = new List<SimpleReadPlan>();
                        readplans.Add(new SimpleReadPlan(TagProtocol.GEN2, new int[] { selants[ant].antid }, 30));
                        modulerdr.ParamSet("ReadPlan", readplans[0]);
                    }

                    if (ant == (selants.Count - 1))
                        ant = 0;
                    else
                        ant++;

                    modulerdr.StartReading();
                    starttmV2 = Timestamp();
                }
                Thread.Sleep(50);
            }
        }

        void Reconnect(int reason, Exception ex)
        {
            //rtbopfailmsg.Text = logstr;
            if (reason > 0)
            {
                cbisunibynullemd.Enabled = false;
                cbischgcolor.Enabled = false;
                cbisunibyant.Enabled = false;
                btnconnect.Enabled = true;
                btnstart.Enabled = false;
                btnstop.Enabled = false;
                readparamenu.Enabled = false;
                tagopmenu.Enabled = false;
                MsgDebugMenu.Enabled = false;
                ToolStripMenuItemBox.Enabled = false;
                标签温度及LEDToolStripMenuItem1.Enabled = false;
                menutest.Enabled = false;
                timer1.Enabled = false;
                btnInvParas.Enabled = false;
                btndisconnect.Enabled = true;
               
                pattern.Enabled = false;
                textBox_stopsec.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                label7.Enabled = false;
                label7.Visible = false;
                for (int f = 1; f <= allAnts.Count; ++f)
                {
                    allAnts[f].Checked = false;
                    allAnts[f].Enabled = false;
                    allAnts[f].ForeColor = antdefaulcolor;
                }

                timer1.Enabled = false;
                toolStripStatusLabel1.Text = "断开";
                if (modulerdr != null)
                {

                    modulerdr.Disconnect();
                    modulerdr = null;
                    isConnect = false;
                    istimerct = false;
                }
                if (reason == 1)
                {
                    MessageBox.Show("重新连接读写器失败");
                }
                else if (reason == 2)
                {
                    MessageBox.Show("读写器异常频率过高:" + ex.ToString());
                }

                if (iar_timer != null)
                {
                    EndInvoke(iar_timer);
                }

                AntsHide();
                groupBox1.Height = 80;
                btn16antset.Enabled = false;
                btn16antset.Visible = true;
                checkBox1.Enabled = false;
                checkBox1.Visible = true;
                istimerct = false;
            }
        }

        string sourceip;
        Color antdefaulcolor;

        public string serialcommunicationmsg = "";

        void AddTagsToDic(object sender, Reader.TagsReadEventArgs tagsArgs)
        {
            foreach (TagReadData tag in tagsArgs.Tags)
            {
                Console.WriteLine("Count:"+tag.ReadCount);
                AddTagToDic(tag);
            }
        }

        delegate void StartReadingByBtn(object sender, EventArgs e);

        Mutex UiMux = new Mutex();

        /// <summary>
        /// 盘点过程中，出现错误处理。判断是否需要重连，重连处理将重新设置相关函数，参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="expArgs"></param>
        void ReadingErrHandler(object sender, Reader.ReadExceptionEventArgs expArgs)
        {
            UiMux.WaitOne();
            Exception ex = expArgs.ReaderException;
            Debug.WriteLine("读写器" + ((Reader)sender).Address + "错误:" + ex.ToString());
            logstr += ex.ToString();
            logstr += "\n";
            Reader expreader = (Reader)sender;
            if (rechecker.IsTrigger())
            {
                BeginInvoke(new ReconnectHandler(Reconnect), new object[] { 2, expArgs.ReaderException });
                UiMux.ReleaseMutex();
                return;
            }
            else
            {
                rechecker.AddErr();
            }

            if (ex is ModuleException)
            {
                System.Diagnostics.Debug.WriteLine("error:" + ((ModuleException)ex).ErrCode.ToString("X4"));
                MessageBox.Show("error:" + ((ModuleException)ex).ErrCode.ToString("X4"));
            }
            /*
            if (((ModuleException)ex).ErrCode != 0x900c)
            {
                expreader.Disconnect();
                isConnect = false;
                modulerdr = null;
                this.BeginInvoke(new ReconnectHandler(Reconnect), new object[] { 2, expArgs.ReaderException });
                UiMux.ReleaseMutex();
                return;
            }*/

            string rdraddress = expreader.Address;
            expreader.Disconnect();
            //modulerdr = null;
            isConnect = false;

            int reason = 0;
            if (isStopanyerror)
            {
                rParms.reconnectcnt = 0;
                reason = 3;
            }

            for (int i = 0; i < rParms.reconnectcnt; ++i)
            {
                try
                {
                    modulerdr = Reader.Create(rdraddress, ModuleTech.Region.NA, readerantnumber);
                    modulerdr.TagsRead += AddTagsToDic;
                    modulerdr.ReadException += ReadingErrHandler;
                    isConnect = true;
                    BeginInvoke(new StartReadingByBtn(btnstart_Click), new object[] { null, null });
                    break;
                }
                catch
                {
                    if (i == rParms.reconnectcnt - 1)
                    {
                        BeginInvoke(new ReconnectHandler(Reconnect), new object[] { 1, null });
                        UiMux.ReleaseMutex();

                        istimerct = false;
                        return;
                    }
                    else
                    {
                        Thread.Sleep(rParms.connectinterval * 1000);
                    }
                }
            }
            //btnstop.PerformClick();
            BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 1, "" });
            UiMux.ReleaseMutex();
        }

        public int readerantnumber;
        private void btnconnect_Click(object sender, EventArgs e)
        {
            int initstep = 0;
            if (cbbreadertype.SelectedIndex == -1)
            {
                MessageBox.Show("请选择天线端口数");
                return;
            }

            if (modulerdr != null)
            {
                modulerdr.Disconnect();
                isConnect = false;
                modulerdr = null;
            }

            rParms.resetParams();

            sourceip = tbip.Text.Trim();
            //if (!tbip.Text.Trim().ToLower().Contains("com"))
            if (Common.IsIPAddress(tbip.Text.Trim().ToLower()))
            {
                rParms.hasIP = true;
            }
            else
            {
                rParms.hasIP = false;
                sourceip = BaudRate.Text.Trim()=="115200"? sourceip:(sourceip + ":" + BaudRate.Text.Trim());
            }

            // 连接读写器类型 有1天线口，2天线口，3天线口，4天线口以及16天线口，地址为串口地址或者ip地址
            try
            {
                starttm = Timestamp();
                timer2.Enabled = true;

                if (cbbreadertype.SelectedIndex ==0) 
                    readerantnumber = 1;
                else if (cbbreadertype.SelectedIndex == 1)
                    readerantnumber = 2;
                else if (cbbreadertype.SelectedIndex == 2)
                    readerantnumber = 4;
                else if (cbbreadertype.SelectedIndex == 4)
                    readerantnumber = 16;
                else if (cbbreadertype.SelectedIndex == 3)
                    readerantnumber = 8;

                modulerdr = Reader.Create(sourceip, ModuleTech.Region.NA, readerantnumber);
                rParms.antcnt = readerantnumber;

                Debug.WriteLine("connect time:" + (Timestamp() - starttm).ToString());
                rParms.AntsState.Clear();

                rParms.readertype = modulerdr.HwDetails.logictype;
                if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC
                    || modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_MICRO)
                {
                    rParms.isMultiPotl = true;
                }

                if (modulerdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_WIFI)
                {
                    rParms.hasIP = false;
                }

                //判断是否支持多协议
                if (!rParms.isMultiPotl)
                {
                    iso183k6btagopToolStripMenuItem.Enabled = false;
                }
                else
                {
                    iso183k6btagopToolStripMenuItem.Enabled = true;
                }
               
                if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7100 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7200 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7300 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7400 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7500)
                    IsEchipPhase = true;
                else
                    IsEchipPhase = false;
                
                rParms.hardvir = (string)modulerdr.ParamGet("HardwareVersion");
                initstep = 2;
                Debug.WriteLine("before SoftwareVersion");
                rParms.softvir = (string)modulerdr.ParamGet("SoftwareVersion");
                initstep = 3;
                char[] sep = new char[1];
                sep[0] = '.';
                string[] verfields = rParms.softvir.Split(sep, StringSplitOptions.None);
                int verint = int.Parse(verfields[0] + verfields[1], System.Globalization.NumberStyles.HexNumber);
                rParms.isFastRead = false;


                pattern.SelectedIndex = 0;
                if ((modulerdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM7 ||
                    modulerdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_SERIAL) &&
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100 &&
                    verint >= 1612)
                {
                    pattern.SelectedIndex = 1;
                }

                if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1200 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7100 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7200 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7300 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7400 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7500)
                {
                    pattern.SelectedIndex = 1;
                }

                if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5900 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5800 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR6000 ||
                    modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR6100)
                {
                    modulerdr.ParamSet("HopFrequencyMode", 1);
                    pattern.SelectedIndex = 1;
                }


#if EMDDATA
                EmbededCmdData ecd = new EmbededCmdData(MemBank.TID, 0,(byte)12);
                modulerdr.ParamSet("EmbededCmdOfInventory", ecd);
#endif

#if  NOHIGHSPEED
                rParms.isFastRead = false;
#endif

                //获取最大，最小功率值
                rParms.powermax = ((int)modulerdr.ParamGet("RfPowerMax")) / 100;
                initstep = 4;
                rParms.powermin = ((int)modulerdr.ParamGet("RfPowerMin")) / 100;
                initstep = 5;
                rParms.gen2session = (int)modulerdr.ParamGet("Gen2Session");
                initstep = 6;
                rParms.fisrtLoad = true;
                //设置回调函数，标签回调和异常回调
                modulerdr.TagsRead += AddTagsToDic;
                modulerdr.ReadException += ReadingErrHandler;
                isConnect = true;


                cbisunibynullemd.Enabled = true;
                cbischgcolor.Enabled = true;
                cbisunibyant.Enabled = true;
                btnstart.Enabled = true;
                readparamenu.Enabled = true;
                tagopmenu.Enabled = true;
                MsgDebugMenu.Enabled = true;
                ToolStripMenuItemBox.Enabled = true;
                标签温度及LEDToolStripMenuItem1.Enabled = true;
                menutest.Enabled = true;
                //menuoutputtags.Enabled = true;
                btnconnect.Enabled = false;
                btnInvParas.Enabled = true;
                btndisconnect.Enabled = true;
                updatemenu.Enabled = false;
               
                pattern.Enabled = true;
                textBox_stopsec.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;

                //labelTopToolStripMenuItem.Enabled = true;

                btn16antset.Enabled = true;
                checkBox1.Enabled = true;
                //添加天线
                if (readerantnumber >= 8)
                {
                    groupBox1.Height = 62;
                    invants16setting.Clear();
                    label7.Enabled = true;
                    label7.Visible = true;
                    btn16antset.Visible = false;
                    checkBox1.Visible = false;
                    for (int i = 5; i <= readerantnumber; i++)
                    {
                        var ControlsId = this.Controls.Find("cbant_" + i, true);
                        if (ControlsId.Length > 0)
                            continue;

                        CheckBox _checkBox = this.Controls.Find("cbant_" + (i - 4), true)[0] as CheckBox;

                        CheckBox checkBox = new CheckBox();
                        checkBox.Name = "cbant_" + i;
                        checkBox.Location = new Point(_checkBox.Location.X, _checkBox.Location.Y + 35);
                        checkBox.Size = _checkBox.Size;
                        checkBox.Font = _checkBox.Font;
                        checkBox.Visible = false;
                        checkBox.Text = "ant" + i;
                        if (i >= 10)
                        {
                            checkBox.Width = 60;
                            checkBox.Height = 21;
                        }
                        groupBox1.Controls.Add(checkBox);

                    }
                }
                else {
                    groupBox1.Height = 80;
                }

                //渲染颜色
                allAnts.Clear();
                foreach (Control ctl in groupBox1.Controls)
                {
                    if (ctl.Name.IndexOf("cbant") == -1)
                        continue;

                    var antId = int.Parse(ctl.Name.Split('_')[1]);
                    allAnts.Add(antId, (CheckBox)ctl);

                    //if (readerantnumber == 1)
                    //    break;
                }

                for (int aa = 1; aa <=readerantnumber; aa++)
                {
                    allAnts[aa].Enabled = true;
                    allAnts[aa].ForeColor = Color.Red;
                }

                //获取连接天线
                try
                {
                    int[] connectedants = (int[])modulerdr.ParamGet("ConnectedAntennas");
                    initstep = 1;
                    for (int c = 0; c < connectedants.Length; ++c)
                    {
                        allAnts[connectedants[c]].ForeColor = Color.Green;
                        allAnts[connectedants[c]].Checked = true;
                    }

                    for (int ff = 1; ff <= allAnts.Count; ++ff)
                    {
                        if (allAnts[ff].Enabled)
                        {
                            if (allAnts[ff].ForeColor == Color.Green)
                            {
                                rParms.AntsState.Add(new AntAndBoll(ff, true));
                            }
                            else
                            {
                                rParms.AntsState.Add(new AntAndBoll(ff, false));
                            }
                        }
                    }
                }
                catch { };

                toolStripStatusLabel1.Text = "连接成功";

            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败，请检查读写器地址是否正确" + ex.ToString() + " step:" + initstep);
                toolStripStatusLabel1.Text = "连接失败";
                return;
            }

            try
            {
             modulerdr.ParamSet("SerialCommMsgDebuger", new Reader.SerialCommOutput(MsgLogHandler));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 检查天线是否可用
        /// </summary>
        /// <returns></returns>
        public List<AntAndBoll> CheckAntsValid()
        {
            List<AntAndBoll> selants = new List<AntAndBoll>();

            for (int cc = 1; cc <= allAnts.Count; ++cc)
            {
                if (!allAnts[cc].Checked)
                    continue;

                if (allAnts[cc].ForeColor == Color.Red)
                    selants.Add(new AntAndBoll(cc, false));
                else
                    selants.Add(new AntAndBoll(cc, true));
            }
            return selants;
        }

        /// <summary>
        /// 判断是否有效二进制字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int IsValidBinaryStr(string str)
        {
            if (str == "")
            {
                return -3;
            }

            foreach (Char a in str)
            {
                if (!((a == '1') || (a == '0')))
                {
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 判断是否为16进制字符串
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static bool IsHexString(string hexString)
        {
            //十六进制发送时，发送框数据进行十六进制数据正则校验
            if (Regex.IsMatch(hexString, "^[0-9A-Fa-f]+$"))
            {
                //校验成功
                return true;
            }
            //校验失败
            return false;
        }

        /// <summary>
        /// 检查密码是否有效，长度为8的16进制字符串
        /// </summary>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public static int IsValidPasswd(string passwd)
        {
            int ret = IsValidHexstr(passwd, 8);
            if (ret == 0)
            {
                if (passwd.Length != 8)
                {
                    return -4;
                }
            }
            return ret;
        }

        /// <summary>
        /// 判断是否合法的16进制字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int IsValidHexstr(string str, int len)
        {
            if (str == "")
            {
                return -3;
            }

            if (str.Length % 4 != 0)
            {
                return -2;
            }

            if (str.Length > len)
            {
                return -4;
            }

            string lowstr = str.ToLower();
            byte[] hexchars = Encoding.ASCII.GetBytes(lowstr);

            foreach (byte a in hexchars)
            {
                if (!((a >= 48 && a <= 57) || (a >= 97 && a <= 102)))
                {
                    return -1;
                }
            }
            return 0;
        }

        long starttm;
        long listTimesStarttm;
        public List<AntAndBoll> invants16setting = new List<AntAndBoll>();
        public bool is16AntRevert = false;
        public List<AntAndBoll> selants;

        void MsgLogHandler(string outmsg)
        {
            serialcommunicationmsg += outmsg;
        }

        /// <summary>
        /// 开始盘点操作，检查必要的参数，然后启动后台盘点线程，设置标签以及异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnstart_Click(object sender, EventArgs e)
        {
            //rtbopfailmsg.Text = "";
            if (rParms.isStandby) {
                var data = new byte[] { 0xFF, 0x00, 0x04, 0x1D, 0x0B };
                modulerdr.SendandRev(data, 1000);
            }

            labtotalcnt.Text = "0";
            labtoatotims.Text = "0";
            selants = null;
            selants = CheckAntsValid();
            if (selants.Count == 0)
            {
                MessageBox.Show("请选择天线");
                return;
            }

            try
            {
                if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                {
                    rParms.ischkant = true;
                }
                else {
                    bool isdet = (bool)modulerdr.ParamGet("CheckAntConnection");
                    rParms.ischkant = isdet;
                }
            }
            catch { }

            if (rParms.ischkant && sender != null)
            {
                for (int i = 0; i < selants.Count; ++i)
                {
                    if (selants[i].isConn == false)
                    {
                        DialogResult stat = DialogResult.OK;
                        stat = MessageBox.Show("在未检测到天线的端口执行搜索，真的要执行吗?", "警告",
                                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button2);
                        if (stat == DialogResult.OK)
                        {
                            break;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }

            List<int> antsExe = new List<int>();
            if (rParms.antForTime > 0)
                antsExe.Add(selants[0].antid);
            else
            {
                for (int i = 0; i < selants.Count; ++i)
                    antsExe.Add(selants[i].antid);
            }

            // 设置ReadPlan 必要项，指定协议和天线组
            UiMux.WaitOne();
            List<SimpleReadPlan> readplans = new List<SimpleReadPlan>();
            if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
            {
                selantsAnt = selants[0].antid;
                if (selants[0].antid == 4)
                {
                    modulerdr.GPOSet(1, false);
                    modulerdr.GPOSet(2, false);
                }
                else if (selants[0].antid == 3)
                {
                    modulerdr.GPOSet(1, true);
                    modulerdr.GPOSet(2, false);
                }
                else if (selants[0].antid == 2)
                {
                    modulerdr.GPOSet(1, true);
                    modulerdr.GPOSet(2, true);
                }
                else if (selants[0].antid == 1)
                {
                    modulerdr.GPOSet(1, false);
                    modulerdr.GPOSet(2, true);
                }
                readplans.Add(new SimpleReadPlan(TagProtocol.GEN2, new int[] { 1 }, rParms.weightgen2));
            }
            else { 
                readplans.Add(new SimpleReadPlan(TagProtocol.GEN2, antsExe.ToArray(), rParms.weightgen2));
            }
            if (readplans.Count > 1)
                modulerdr.ParamSet("ReadPlan", new MultiReadPlan(readplans.ToArray()));
            else
                modulerdr.ParamSet("ReadPlan", readplans[0]);


            m_Tags.Clear();
            if (rParms.isFastRead)
                rParms.readdur = 50;

            if (rParms.isIdtAnts && rParms.IdtAntsType == 1)
            {
                timer1.Interval = rParms.DurIdtval;
            }
            else
            {
                if (rParms.readdur == 0)
                {
                    timer1.Interval = 200;
                }
                else if (rParms.readdur < 1000)
                {
                    timer1.Interval = rParms.readdur;
                }
                else
                {
                    timer1.Interval = 1000;
                }
            }

            try
            {
                BackReadOption bro = new BackReadOption();
                bro.IsFastRead = rParms.isFastRead;

                if (rParms.isFastRead)
                {
                    Ex10FastModeParams ex10 = pattern.SelectedIndex == 2 ? new Ex10FastModeParams() : null;
                    modulerdr.ParamSet("Ex10FastModeParams", ex10);
                }

                if (bro.IsFastRead && rParms.is1200hdstmd)
                {
                    bro.FastReadDutyRation |= 0x10;
                }
                
                if (modulerdr.HwDetails.module != Reader.Module_Type.MODOULE_M6E &&
                    modulerdr.HwDetails.module != Reader.Module_Type.MODOULE_M6E_MICRO &&
                    modulerdr.HwDetails.module != Reader.Module_Type.MODOULE_M6E_PRC &&
                    modulerdr.HwDetails.module != Reader.Module_Type.MODOULE_M5E &&
                    modulerdr.HwDetails.module != Reader.Module_Type.MODOULE_M5E_C &&
                    modulerdr.HwDetails.module != Reader.Module_Type.MODOULE_M5E_PRC)
                {
                    try
                    {
                        if (rParms.istagfoucs)
                        {
                            modulerdr.ParamSet("TagFoucs", true);
                        }
                        else
                        {
                            modulerdr.ParamSet("TagFoucs", false);
                        }

                        if (rParms.isfastid)
                        {
                            modulerdr.ParamSet("FastId", true);
                        }
                        else
                        {
                            modulerdr.ParamSet("FastId", false);
                        }
                    }
                    catch { };
                }

                bro.ReadDuration = (ushort)rParms.readdur;
                bro.ReadInterval = (uint)rParms.sleepdur;
                bro.FRTMetadata = rParms.FRTMeta;
                modulerdr.ParamSet("BackReadOption", bro);
                if (rParms.GpiTriiger != null)
                {
                    modulerdr.ParamSet("BackReadGPITrigger", rParms.GpiTriiger);
                }
                else
                {
                    modulerdr.ParamSet("BackReadGPITrigger", null);
                }

                modulerdr.StartReading();
                starttm = Timestamp();
                rParms.isStandby = false;
                rParms.stopsec = Convert.ToInt32(textBox_stopsec.Text)*1000;
                if (rParms.stopsec > 0)
                    rParms.endatetime = starttm + (long)rParms.stopsec;
                else
                    rParms.endatetime = 0;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("启动读取失败:" + ex.ToString());
                UiMux.ReleaseMutex();
                return;
            }
            isInventory = true;
            labreadtime.Text = "0";

            try
            {
                istimerct = true;
                timerControlThread = new Thread(timerControl);

                timerControlThread.Start();
            }
            catch (System.Exception)
            {

            }

            timer2.Enabled = true;
            //Thread.Sleep(100);
            timer1.Enabled = true;

            btndisconnect.Enabled = false;
            btnstop.Enabled = true;
            readparamenu.Enabled = false;
            btn16antset.Enabled = false;
            //btn16antset.Visible = false;
            menutest.Enabled = false;
            tagopmenu.Enabled = false;
            MsgDebugMenu.Enabled = false;
            ToolStripMenuItemBox.Enabled = false;
            标签温度及LEDToolStripMenuItem1.Enabled = false;
            menutest.Enabled = false;
            //menuoutputtags.Enabled = false;
            btnstart.Enabled = false;
            btnInvParas.Enabled = false;
            toolStripStatusLabel1.Text = "Inventory";
            UiMux.ReleaseMutex();
        }

        /// <summary>
        /// 获取当前的毫秒时间戳
        /// </summary>
        /// <returns></returns>
        public static long Timestamp()
        {
            long ts = ConvertDateTimeToInt(DateTime.Now);
            return ts;
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            //long t = (time.Ticks - 621356256000000000) / 10000;
            return t;
        }

        /// <summary>
        /// 停止盘点，刷新界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnstop_Click(object sender, EventArgs e)
        {
            istimerct = false;
            //lvTags.VirtualMode = false;
            UiMux.WaitOne();
            timer1.Enabled = false;
            isInventory = false;
            try
            {
                modulerdr.StopReading();
                labreadtime.Text = (Timestamp() - starttm).ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show("停止读取失败:" + exp.ToString());
                UiMux.ReleaseMutex();
                return;
            }

            if (iar_timer != null)
            {
                EndInvoke(iar_timer);
            }

            istimerct = false;

            timer1_Tick(null, null);
            btnstop.Enabled = false;
            readparamenu.Enabled = true;
            if (readerantnumber >= 8)
            {
                btn16antset.Enabled = true;
            }
            starttm = Timestamp();
            btn16antset.Enabled = true;
            menutest.Enabled = true;
            tagopmenu.Enabled = true;
            MsgDebugMenu.Enabled = true;
            ToolStripMenuItemBox.Enabled = true;
            标签温度及LEDToolStripMenuItem1.Enabled = true;
            menutest.Enabled = true;
            btnstart.Enabled = true;
            //menuoutputtags.Enabled = true;
            btndisconnect.Enabled = true;
            btnInvParas.Enabled = true;
            toolStripStatusLabel1.Text = "";

            UiMux.ReleaseMutex();
        }

        /// <summary>
        /// 清空操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            tagmutex.WaitOne();
            m_Tags.Clear();
            tagmutex.ReleaseMutex();
            lvTags.Items.Clear();
            taglvdic.Clear();
            antList.Clear();
            lvTags.VirtualListSize = 0;
        }
        List<TagInfo> tmplist = new List<TagInfo>();
		bool IsEchipPhase = false;
        Dictionary<string, ListViewItem> taglvdic = new Dictionary<string, ListViewItem>();
        bool taglvdicSorting = true;
        delegate void ListTasks();

        public void ListViewItemAdd() {
            myCache = taglvdic.Values.ToArray();
            lvTags.VirtualListSize = taglvdic.Count;

            if (myCache.Count() <= 200) {
                //lvTags.VirtualListSize = myCache.Count;
                lvTags.Invalidate();
            }
            //lvTags.Items.Add(item);
            //lvTagsCount++;
        }

        public int lvTagsCount=1;

        /// <summary>
        /// timer 事件用于刷新界面，显示标签盘点状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //standby
            labreadtime.Text = (Timestamp() - starttm).ToString();
            try
            {
                if (m_Tags.Values.Count > 0)
                {
                    var m_TagsValues = m_Tags.Values.ToList();
                    //labant1cnt.Text = m_TagsValues.Where(r=>r.antid==1).Count().ToString();
                    //labant2cnt.Text = m_TagsValues.Where(r=>r.antid==2).Count().ToString();
                    //labant3cnt.Text = m_TagsValues.Where(r=>r.antid==3).Count().ToString();
                    //labant4cnt.Text = m_TagsValues.Where(r=>r.antid==4).Count().ToString();
                    labtotalcnt.Text = m_TagsValues.Count().ToString();
                    labtoatotims.Text = m_TagsValues.Sum(r => r.readcnt).ToString();
                }
            }
            catch 
            {
               
            }
            ListTask();
        }

        private void ListTask()
        {
            tmplist.Clear();
            tagmutex.WaitOne();
#if(TEMPERATRATURE)
            label6.Text = tempture.ToString();
#endif
            tmplist = m_Tags.Values.ToList();
            tagmutex.ReleaseMutex();
            
            //added on 3-26
            if (rParms.isIdtAnts)
            {
                Dictionary<string, TagInfo> tmp_Tags = new Dictionary<string, TagInfo>();
                if (rParms.IdtAntsType == 2)
                {
                    List<TagInfo> tags_dy = new List<TagInfo>();
                    List<TagInfo> tags_xy = new List<TagInfo>();
                    List<TagInfo> tags_del = new List<TagInfo>();

                    foreach (TagInfo tag in tmplist)
                    {
                        TimeSpan span = DateTime.Now - tag.timestamp;
                        if (span.Seconds > rParms.AfterIdtWaitval)
                        {
                            tags_dy.Add(tag);
                        }
                        else
                        {
                            tags_xy.Add(tag);
                        }
                    }

                    foreach (TagInfo tag_dy in tags_dy)
                    {
                        foreach (TagInfo tag_xy in tags_xy)
                        {
                            bool isfind = false;
                            if (rParms.isUniByEmd)
                            {
                                if ((tag_dy.epcid + tag_dy.emddatastr) == (tag_xy.epcid + tag_xy.emddatastr))
                                {
                                    isfind = true;
                                }
                            }
                            else
                            {
                                if (tag_dy.epcid == tag_xy.epcid)
                                {
                                    isfind = true;
                                }
                            }
                            if (isfind)
                            {
                                tags_del.Add(tag_dy);
                                break;
                            }
                        }
                    }

                    foreach (TagInfo tag in tags_del)
                    {
                        tags_dy.Remove(tag);
                    }
                    tmplist = tags_dy;
                }

                foreach (TagInfo tag in tmplist)
                {
                    string unistr = null;
                    if (rParms.isUniByEmd)
                    {
                        unistr = tag.epcid + tag.emddatastr;
                    }
                    else
                    {
                        unistr = tag.epcid;
                    }

                    if (tmp_Tags.ContainsKey(unistr))
                    {
                        if (tmp_Tags[unistr].RssiSum < tag.RssiSum)
                        {
                            tmp_Tags.Remove(unistr);
                            tmp_Tags.Add(unistr, tag);
                        }
                    }
                    else
                    {
                        tmp_Tags.Add(unistr, tag);
                    }
                }

                tagmutex.WaitOne();
                foreach (TagInfo tag in tmplist)
                {
                    string keystr = tag.epcid;

                    if (rParms.isUniByEmd)
                    {
                        keystr += tag.emddatastr;
                    }

                    if (rParms.isUniByAnt)
                    {
                        keystr += tag.antid.ToString();
                    }

                    m_Tags.Remove(keystr);
                }

                tagmutex.ReleaseMutex();
                tmplist.Clear();
                foreach (TagInfo tag in tmp_Tags.Values)
                {
                    tmplist.Add(tag);
                }
            }
           
            int totaltims = 0;
            foreach (TagInfo tag in tmplist)
            {
                string epckeystr = tag.epcid;
                if (rParms.isUniByEmd)
                {
                    epckeystr += tag.emddatastr;
                }
                else if (rParms.isUniByAnt)
                {
                    epckeystr += tag.antid.ToString();
                }

                if (taglvdic.ContainsKey(epckeystr))
                {
                    ListViewItem viewitem = taglvdic[epckeystr];
                    int isupdatecolor = 0;
                    if (viewitem.SubItems[4].Text != tag.emddatastr)
                    {
                        isupdatecolor = 1;
                        viewitem.SubItems[4].Text = tag.emddatastr;
                    }

                    if (tag.readcnt != int.Parse(viewitem.SubItems[1].Text))
                    {
                        isupdatecolor = 1;
                        viewitem.SubItems[1].Text = tag.readcnt.ToString();
                    }
                    totaltims += tag.readcnt;

                    if (selantsAnt != int.Parse(viewitem.SubItems[3].Text)&&modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                    {
                        isupdatecolor = 1;
                        viewitem.SubItems[3].Text = selantsAnt.ToString();
                    }else if (tag.antid != int.Parse(viewitem.SubItems[3].Text)&& modulerdr.HwDetails.module != Reader.Module_Type.MODOULE_SLR5300) {
                        isupdatecolor = 1;
                        viewitem.SubItems[3].Text = tag.antid.ToString();
                    }

                    viewitem.SubItems[6].Text = tag.RssiRaw.ToString();
                    viewitem.SubItems[7].Text = tag.Frequency.ToString();
                    viewitem.SubItems[8].Text = tag.Phase.ToString();

					if (IsEchipPhase)
                    {

                        float xw1 = (float)(((tag.Phase&0xffff0000)>>16) / 4096.0 * 360);
                        float xw2 = (float)((tag.Phase & 0x0000ffff) / 4096.0 * 360);
                        viewitem.SubItems[8].Text = Math.Round(xw1, 2).ToString() + "/" + Math.Round(xw2, 2).ToString();
                    }
                    else
                    {
                        float xw = (tag.Phase & 0x3f) * 180 / 64;
                        viewitem.SubItems[8].Text = Math.Round(xw, 2).ToString();
                    }
                    if (rParms.isChangeColor)
                    {
                        if (isupdatecolor == 0)
                        {
                            TimeSpan span = DateTime.Now - tag.timestamp;
                            if (span.Seconds > 2 && span.Seconds < 4)
                            {
                                viewitem.BackColor = Color.Silver;
                            }
                            else if (span.Seconds >= 4)
                            {
                                viewitem.BackColor = Color.DimGray;
                            }
                        }
                        else
                        {
                            viewitem.BackColor = Color.White;
                        }
                    }
                }
                else if (!taglvdic.ContainsKey(epckeystr))
                {
                    //ListViewItem item = new ListViewItem();
                    ListViewItem item = new ListViewItem((taglvdic.Count+1).ToString());
                    item.SubItems.Add(tag.readcnt.ToString());
                    totaltims += tag.readcnt;
                    item.SubItems.Add(tag.epcid);

                    if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                        item.SubItems.Add(selantsAnt.ToString());
                    else 
                        item.SubItems.Add(tag.antid.ToString());
                        
                    item.Name = tag.epcid;
                    item.SubItems.Add(tag.emddatastr);
                    
                    if (tag.potl == TagProtocol.GEN2)
                    {
                        item.SubItems.Add("GEN2");
                    }
                    else if (tag.potl == TagProtocol.ISO180006B)
                    {
                        item.SubItems.Add("ISO180006B");
                    }
                    else if (tag.potl == TagProtocol.IPX256)
                    {
                        item.SubItems.Add("IPX256");
                    }
                    else if (tag.potl == TagProtocol.IPX64)
                    {
                        item.SubItems.Add("IPX64");
                    }
                    else
                    {
                        item.SubItems.Add("GEN2");
                    }

                    item.SubItems.Add(tag.RssiRaw.ToString());
                    item.SubItems.Add(tag.Frequency.ToString());
                    float xw = (tag.Phase & 0x3f) * 180 / 64;
                    item.SubItems.Add(Math.Round(xw, 2).ToString());
                    item.SubItems.Add("");
                    taglvdic.Add(epckeystr, item);
                    lvTagsCount++;
                    //BeginInvoke(new ListTasks(ListViewItemAdd), item);
                    //lvTags.Items.Add(item);
                }
            }

            Invoke(new ListTasks(ListViewItemAdd));
            //labtoatotims.Text = totaltims.ToString();
            //labant1cnt.Text = ant1cnt.ToString();
            //labant2cnt.Text = ant2cnt.ToString();
            //labant3cnt.Text = ant3cnt.ToString();
            //labant4cnt.Text = ant4cnt.ToString();
            //modify on 3-26
            //labtotalcnt.Text = lvTags.Items.Count.ToString();
            //labtotalcnt.Text = lvTagsCount.ToString();

            //lvTags.Items.Add(item);
            //lvTagsCount++;
        }

        [DllImport("user32.dll", EntryPoint = "GetScrollPos")]

        public static extern int GetScrollPos(IntPtr hwnd, int nBar);
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UiMux.WaitOne();
            timer1.Enabled = false;
            if (isConnect)
            {
                if (isInventory)
                {
                    modulerdr.StopReading();
                }

                modulerdr.Disconnect();
            }
            UiMux.ReleaseMutex();
        }

        Dictionary<int, CheckBox> allAnts = new Dictionary<int, CheckBox>();
        string cur_dir = null;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            cbisunibynullemd.Enabled = false;
            cbischgcolor.Enabled = false;
            cbisunibyant.Enabled = false;

            iso183k6btagopToolStripMenuItem.Enabled = false;
            readparamenu.Enabled = false;
            btn16antset.Enabled = false;
            checkBox1.Enabled = false;
            tagopmenu.Enabled = false;
            MsgDebugMenu.Enabled = false;
            ToolStripMenuItemBox.Enabled = false;
            标签温度及LEDToolStripMenuItem1.Enabled = false;
            menutest.Enabled = false;
            //menuoutputtags.Enabled = false;
            btnstart.Enabled = false;
            btnstop.Enabled = false;
            btnInvParas.Enabled = false;
            pattern.Enabled = false;
            textBox_stopsec.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            toolStripStatusLabel1.Text = "未连接";

            allAnts.Add(1, cbant_1);
            allAnts.Add(2, cbant_2);
            allAnts.Add(3, cbant_3);
            allAnts.Add(4, cbant_4);
            antdefaulcolor = cbant_1.ForeColor;

            for (int f = 1; f <= allAnts.Count; ++f)
            {
                allAnts[f].Enabled = false;
            }
            cur_dir = Environment.CurrentDirectory;
            cbbreadertype.SelectedIndex = 0;

            BaudRate.SelectedIndex = 4;
            pattern.SelectedIndex = 0;
            tbipAddItems();
            //isDetectionVersions

            if (ConfigHelper.GetAppConfig("isDetectionVersions")=="1")
                new Thread(detectionVersionUpdatingIs).Start();
        }

        private void readparamenu_Click(object sender, EventArgs e)
        {
            rParms.isIpModify = false;
            rParms.isM5eModify = false;

            if (rParms.hasIP)
            {
                if (!rParms.isGetIp)
                {
                    if (modulerdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9 ||
                        modulerdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_V2 ||
                          modulerdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI ||
                            modulerdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI_V2)
                    {
                        //get mac
                        byte[] rdl = (byte[])modulerdr.ParamGet("ReaderDetails");
                        System.Console.WriteLine(ByteFormat.ToHex(rdl));
                        rParms.isGetIp = true;
                    }

                    //else
                    {
                        ReaderIPInfo ipinfo = null;
                        try
                        {
                            ipinfo = (ReaderIPInfo)modulerdr.ParamGet("IPAddress");
                            rParms.ip = ipinfo.IP;
                            rParms.subnet = ipinfo.SUBNET;
                            rParms.gateway = ipinfo.GATEWAY;
                            if (ipinfo.MACADDR != null)
                            {
                                rParms.macstr = ByteFormat.ToHex(ipinfo.MACADDR);
                            }
                            rParms.isGetIp = true;
                        }
                        catch
                        {
                            rParms.hasIP = false;
                        }
                    }
                }
            }

            readerParaform frm = new readerParaform(rParms, modulerdr);
            frm.ShowDialog();
            //if (frm.ShowDialog() == DialogResult.Cancel)
            //{
            //    try
            //    {
            //        bool isdet = (bool)modulerdr.ParamGet("CheckAntConnection");
            //        rParms.ischkant = isdet;
            //    }
            //    catch { }
            //    return;
            //}
            //else
            //{
            //    try
            //    {
            //        bool isdet = (bool)modulerdr.ParamGet("CheckAntConnection");
            //        rParms.ischkant = isdet;
            //    }
            //    catch { }
            //}
        }

        //Gen2TagFilter filter = null;
        //EmbededCmdData embededdata = null;

        private void btnInvParas_Click(object sender, EventArgs e)
        {
            InventoryParasform frm = new InventoryParasform(this);
            frm.ShowDialog();
            if (rParms.GpiTriiger != null)
            {
                //modulerdr.ParamSet("BackReadGPITrigger", rParms.GpiTriiger);
                btnstart.PerformClick();
            }
            else {
                modulerdr.ParamSet("BackReadGPITrigger", null);
            }

        }

        private void updatemenu_Click(object sender, EventArgs e)
        {
            if (rParms.readertype == ReaderType.PR_ONEANT)
            {
                MessageBox.Show("此类型读写器不支持升级操作");
                return;
            }
            updatefrm frm = new updatefrm();
            frm.ShowDialog();
        }

        private void Custommenu_Click(object sender, EventArgs e)
        {
            if (rParms.readertype != ReaderType.PR_ONEANT)
            {
                CustomCmdFrm frm = new CustomCmdFrm(modulerdr, rParms);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("此类型读写器不支持标签特殊指令");
            }
        }

        private void gen2tagopMenuItem_Click(object sender, EventArgs e)
        {
            //gen2opForm2 frm = new gen2opForm2(modulerdr, rParms, this);
            //frm.StartPosition = FormStartPosition.CenterParent;
            //frm.ShowDialog();

            gen2opForm frm = new gen2opForm(modulerdr, rParms, this);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void iso183k6btagopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Iso186bopForm frm = new Iso186bopForm(modulerdr, rParms);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btndisconnect_Click(object sender, EventArgs e)
        {
            updatemenu.Enabled = true;
            Reconnect(3, null);

            //try
            //{
            //    Comm comm = new Comm();
            //    comm = new Comm();
            //    comm.serialPort.PortName = tbip.Text;
            //    //波特率
            //    comm.serialPort.BaudRate = Convert.ToInt32(BaudRate.Text);
            //    //数据位
            //    comm.serialPort.DataBits = 8;
            //    //两个停止位
            //    comm.serialPort.StopBits = System.IO.Ports.StopBits.One;
            //    //无奇偶校验位
            //    comm.serialPort.Parity = System.IO.Ports.Parity.None;
            //    comm.serialPort.ReadTimeout = 200;
            //    comm.serialPort.WriteTimeout = -1;
            //    comm.Open();
            //    if (comm.IsOpen)
            //    {
            //        comm.WritePort(new byte[] { 0x0c });
            //        comm.WritePort(new byte[] { 0x09 });
            //        comm.WritePort(new byte[] { 0x0c });
            //    }
            //    comm.Close();
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private void CountTagMenuItem_Click(object sender, EventArgs e)
        {
            CountTagsFrm frm = new CountTagsFrm(modulerdr, rParms);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void MulperlockMenuItem_Click(object sender, EventArgs e)
        {
            MulperlockFrm frm = new MulperlockFrm(modulerdr, rParms);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void menuabout_Click(object sender, EventArgs e)
        {
            AboutFrm frm = new AboutFrm();
            frm.ShowDialog();
        }

        private void menutest_Click(object sender, EventArgs e)
        {
            regulatoryFrm frm = new regulatoryFrm(modulerdr);
            frm.ShowDialog();
        }

        private void menuitemlog_Click(object sender, EventArgs e)
        {
            
            AboutFrm frm = new AboutFrm();
            frm.ShowDialog();
        }

        private void lvTags_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (!btnstart.Enabled)
                return;

            if (e.Column < 1 || e.Column > 8)
                return;
                
            myCache =taglvdic.Values.OrderBy(r =>r.SubItems[e.Column].Text).ToArray();
            lvTags.VirtualListSize = taglvdic.Count;
            lvTags.Invalidate();
        }

        /// <summary>
        /// 导出盘点标签数据文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuoutputtags_Click(object sender, EventArgs e)
        {
            string filename = null;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "txt(*.txt)| *.txt|csv(*.csv)|*.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName;
                FileInfo fileInfo = new FileInfo(filename);
                fileInfo.Delete();
                StreamWriter streamWriter = fileInfo.CreateText();
                //    streamWriter.WriteLine("格式：epc，读次数，天线，附加数据，协议,RSSI,读取次数");
                foreach (ListViewItem viewitem in lvTags.Items)
                {
                    string wline = viewitem.SubItems[2].Text + "," + viewitem.SubItems[1].Text + "," +
                        viewitem.SubItems[3].Text + "," + viewitem.SubItems[4].Text + "," +
                        viewitem.SubItems[5].Text + "," + viewitem.SubItems[6].Text + "," +
                        viewitem.SubItems[7].Text + "," + viewitem.SubItems[8].Text + ",";
                    //+ viewitem.SubItems[9].Text;
                    streamWriter.WriteLine(wline);
                }
                streamWriter.Flush();
                streamWriter.Close();
            }

        }

        private void MsgDebugMenu_Click(object sender, EventArgs e)
        {
            FrmMsgDebug frm = new FrmMsgDebug(this);
            frm.Show();
        }

        private void menuitemmultibankwrite_Click(object sender, EventArgs e)
        {
            MultiBankWriteFrm frm = new MultiBankWriteFrm(modulerdr);
            frm.ShowDialog();
        }

        private void pSAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPsam frm = new FrmPsam(modulerdr);
            frm.ShowDialog();
        }

        private void btn16antset_Click(object sender, EventArgs e)
        {
            Frm16AntSet frm = new Frm16AntSet(this);
            frm.ShowDialog();
        }

        private void lvTags_MouseClick_1(object sender, MouseEventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (lvTags.SelectedIndices!= null)
            {
                ListView.SelectedIndexCollection col = lvTags.SelectedIndices;
                string AllEpcs = "";
                foreach (int vi in col)
                {
                    AllEpcs += lvTags.Items[vi].SubItems[2].Text + "\r\n";
                }
                if (AllEpcs != string.Empty)
                {
                    Clipboard.SetDataObject(AllEpcs.Substring(0, AllEpcs.Length - 2), true);
                }
            }
        }

        private void 驻波ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modulerdr == null) {
                MessageBox.Show("请先连接设备后重试");
                return;
            }
            Form_onekey frm = new Form_onekey(rParms, modulerdr);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void 天线区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("请先连接设备后重试");
                return;
            }
            Form_anterea frm = new Form_anterea(modulerdr, rParms, this);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void 自动发卡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("请先连接设备后重试");
                return;
            }
            Form_inicard frm = new Form_inicard(modulerdr, rParms, this);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void lBTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("请先连接设备后重试");
                return;
            }
            Form_lbt frm = new Form_lbt(modulerdr, rParms, this);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void 拷贝当前TIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTags.SelectedItems.Count != 0)
            {
                string Alltids = "";
                foreach (ListViewItem vi in lvTags.SelectedItems)
                {
                    Alltids += vi.SubItems[4].Text + "\r\n";
                }
                if (Alltids != string.Empty)
                {
                    Clipboard.SetDataObject(Alltids.Substring(0, Alltids.Length - 2), true);
                }
            }
        }

        private void 拷贝全部TIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string AllTids = "";
            foreach (ListViewItem vi in lvTags.Items)
            {
                AllTids += vi.SubItems[4].Text + "\r\n";
            }
            if (AllTids != string.Empty)
            {
                Clipboard.SetDataObject(AllTids.Substring(0, AllTids.Length - 2), true);
            }
        }

        public string[] Find_WIN32_Com()
        {
            List<string> coms = new List<string>();
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();

                for (int i = 0; i < sSubKeys.Length; i++)
                {
                    if (!sSubKeys[i].Contains("BthModem"))
                    {
                        //var SS= keyCom.GET(sSubKeys[i]);
                        coms.Add((string)keyCom.GetValue(sSubKeys[i]));
                    }
                }

            }
            return coms.ToArray();
        }

        private void 柜子应用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("请先连接设备后重试");
                return;
            }
            Form_box frm = new Form_box(modulerdr, rParms, this);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void 过滤标签ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTags.SelectedIndices ==null)
            {
                MessageBox.Show("请至少选中一个标签");
                return;
            }
            ListView.SelectedIndexCollection col = lvTags.SelectedIndices;
            Gen2TagFilter[] gtf2 = new Gen2TagFilter[1];
            int p = 0;

            var text = lvTags.Items[col[0]].SubItems[2].Text;
            int flen = text.Length * 4;
            byte[] fdata = ByteFormat.FromHex(text);
            gtf2[p++] = new Gen2TagFilter(flen, fdata, MemBank.EPC, 32, false);
            try
            {
                modulerdr.ParamSet("MultiTagFilters", gtf2);
            }
            catch (ModuleException mex)
            {
                MessageBox.Show(mex.Message);
                return;
            }
        }

        private void 清除过滤ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modulerdr.ParamSet("MultiTagFilters", null);
        }

        private void 隐藏显示附加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTags.Columns[4].Width == 0)
            {
                lvTags.Columns[4].Width = 116;
            }
            else
            {
                lvTags.Columns[4].Width = 0;
            }
        }

        private void 隐藏显示天线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTags.Columns[3].Width == 0)
            {
                lvTags.Columns[3].Width = 51;
            }
            else
            {
                lvTags.Columns[3].Width = 0;
            }
        }

        private void 隐藏显示协议ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTags.Columns[5].Width == 0)
            {
                lvTags.Columns[5].Width = 66;
            }
            else
            {
                lvTags.Columns[5].Width = 0;
            }
        }

        private void 隐藏显示RSSLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTags.Columns[6].Width == 0)
            {
                lvTags.Columns[6].Width = 75;
            }
            else
            {
                lvTags.Columns[6].Width = 0;
            }
        }

        private void 隐藏显示频率ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTags.Columns[7].Width == 0)
            {
                lvTags.Columns[7].Width = 75;
            }
            else
            {
                lvTags.Columns[7].Width = 0;
            }
        }

        private void 隐藏显示相位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTags.Columns[8].Width == 0)
            {
                lvTags.Columns[8].Width = 75;
            }
            else
            {
                lvTags.Columns[8].Width = 0;
            }
        }

        private void 标签信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_LabelInfo frm = new Form_LabelInfo(modulerdr, rParms, this);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logFrm frm = new logFrm();
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filename = null;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "txt(*.txt)| *.txt|csv(*.csv)|*.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName;
                FileInfo fileInfo = new FileInfo(filename);
                fileInfo.Delete();
                StreamWriter streamWriter = fileInfo.CreateText();
                streamWriter.WriteLine("epc,读次数,天线,附加数据,协议,RSSI,频率,相位");
                string wline = "";
                
                foreach (TagInfo viewitem in m_Tags.Values)
                {
                    float xw = (viewitem.Phase & 0x3f) * 180 / 64;
                    wline = viewitem.epcid + "," + viewitem.readcnt + "," +
                       viewitem.antid + "," + viewitem.emddatastr + "," +
                       viewitem.potl.ToString()+ "," + viewitem.RssiRaw + "," +
                       viewitem.Frequency + "," + Math.Round(xw, 2).ToString() + ",";
                    streamWriter.WriteLine(wline);
                }
                
                //foreach (ListViewItem viewitem in lvTags.Items)
                //{
                //     wline = viewitem.SubItems[2].Text + "," + viewitem.SubItems[1].Text + "," +
                //        viewitem.SubItems[3].Text + "," + viewitem.SubItems[4].Text + "," +
                //        viewitem.SubItems[5].Text + "," + viewitem.SubItems[6].Text + "," +
                //        viewitem.SubItems[7].Text + "," + viewitem.SubItems[8].Text + ",";
                //        //+ viewitem.SubItems[9].Text;
                //    streamWriter.WriteLine(wline);
                //}
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        private void 标签统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_statistics frm = new Form_statistics(modulerdr, rParms, this);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void tbip_DropDown(object sender, EventArgs e)
        {
            int i = GetScrollPos(this.lvTags.Handle, 1);
            tbipAddItems();
            Common.DownListWidth(tbip);
        }

        public void tbipAddItems() {
            tbip.Items.Clear();
            tbip.Text = "";
            string[] coms = Find_WIN32_Com();
            if (coms != null && coms.Length > 0)
            {
                for (int i = 0; i < coms.Count(); i++)
                {
                    tbip.Items.Add(coms[i]);
                }
            }
            
            tbip.Items.Add("192.168.1.100");
            tbip.SelectedIndex = 0;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            Common.DownListWidth(BaudRate);
        }


        private void cbbreadertype_DropDown(object sender, EventArgs e)
        {
            Common.DownListWidth(cbbreadertype);
        }

        private void tbip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbip.Text.IndexOf("COM") != -1 || tbip.Text.IndexOf("com") != -1)
                tbip.Font = new Font(tbip.Font.Name, 11);
            else
                tbip.Font = new Font(tbip.Font.Name, 9);
        }

        private void 柜子应用2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void lvTags_ColumnClick_1(object sender, ColumnClickEventArgs e)
        {
            if (taglvdicSorting)
            {
                taglvdic = taglvdic.OrderBy(r => r.Value.SubItems[e.Column].Text).ToDictionary(r => r.Key, s => s.Value);
                taglvdicSorting = !taglvdicSorting;
            }
            else {
                taglvdic = taglvdic.OrderByDescending(r => r.Value.SubItems[e.Column].Text).ToDictionary(r => r.Key, s => s.Value);
                taglvdicSorting = !taglvdicSorting;
            }
           
            myCache = taglvdic.Values.ToArray();
            lvTags.VirtualListSize = taglvdic.Count;
            lvTags.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.D0)
            {
                Form_boxAllTags formV2 = new Form_boxAllTags(modulerdr, rParms);
                formV2.ShowDialog();
            }
        }

        private void cbbreadertype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modulerdr != null && modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300) {
                if (cbbreadertype.Text == "四天线") {
                    for (int aa = 1; aa <= 4; ++aa)
                    {
                        allAnts[aa].Enabled = true;
                        allAnts[aa].ForeColor = Color.Red;
                    }
                } else if (cbbreadertype.SelectedIndex == 0) {
                    for (int aa = 1; aa <= 4; ++aa)
                    {
                        allAnts[aa].Enabled = false;
                    }
                    allAnts[1].Enabled = true;
                }
            }
        }

        public void AntsShow() {
            if (readerantnumber < 8)
                return;

            groupBox1.BringToFront();
            if (readerantnumber == 8)
            {
                groupBox1.Height = 110;
            }

            if (readerantnumber == 16)
            {
                groupBox2.Visible = false;
                groupBox1.Height = 190;
            }
            foreach (Control ctl in groupBox1.Controls)
            {
                if (ctl.Name.IndexOf("cbant") == -1)
                    continue;

                ctl.Visible = true;
            }

            btn16antset.Visible = true;
            checkBox1.Visible = true;
            label7.Text = "︿";
        }
        public void AntsHide() {
            foreach (Control ctl in groupBox1.Controls)
            {
                var name = ctl.Name;
                if (name.IndexOf("cbant") == -1)
                    continue;
                if (Convert.ToInt32(name.Replace("cbant_", "")) <= 4)
                    continue;

                ctl.Visible = false;
            }

            groupBox1.Height = 62;
            btn16antset.Visible = false;
            checkBox1.Visible = false;
            groupBox2.Visible = true;
            label7.Text = "﹀";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control ctl in groupBox1.Controls) {
                if (ctl.Name.IndexOf("cbant") == -1|| !ctl.Enabled)
                    continue;

                ((CheckBox)ctl).Checked = checkBox1.Checked;
            }
        }

        private void pattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pattern.SelectedIndex == 0)
                rParms.isFastRead = false;
            else if (pattern.SelectedIndex == 1)
                rParms.isFastRead = true;
            else if (pattern.SelectedIndex == 2)
            {
                rParms.isFastRead = true;
            }
        }

        private void cbischgcolor_CheckedChanged(object sender, EventArgs e)
        {
            rParms.isChangeColor = cbischgcolor.Checked;
        }

        private void cbisunibynullemd_CheckedChanged(object sender, EventArgs e)
        {
            rParms.isUniByEmd = cbisunibynullemd.Checked;
            modulerdr.ParamSet("IsTagDataUniqueByEmddata",  cbisunibynullemd.Checked);
        }

        private void cbisunibyant_CheckedChanged(object sender, EventArgs e)
        {
            rParms.isUniByAnt = cbisunibyant.Checked;
            modulerdr.ParamSet("IsTagDataUniqueByAnt", cbisunibyant.Checked);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            if (label7.Text == "﹀") {
                AntsShow();
            }else {
                AntsHide();
            }
            
        }

        private void tagopmenu_Click(object sender, EventArgs e)
        {

        }

        private void 标签特殊指令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("请先连接设备后重试");
                return;
            }
            if (rParms.readertype != ReaderType.PR_ONEANT)
            {
                CustomCmdFrm frm = new CustomCmdFrm(modulerdr, rParms);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("此类型读写器不支持标签特殊指令");
            }
        }
        

        private void pSAMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmPsam frm = new FrmPsam(modulerdr);
            frm.ShowDialog();
        }

        private void 柜子应用1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("请先连接设备后重试");
                return;
            }
            Form_box frm = new Form_box(modulerdr, rParms, this);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void 柜子应用2ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("请先连接设备后重试");
                return;
            }
            Form_boxAllTagsV2 formV2 = new Form_boxAllTagsV2(modulerdr, rParms);
            formV2.ShowDialog();
        }

        private void ex10系列初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                sourceip = tbip.Text.Trim();
                try
                {
                    modulerdr = Reader.Create(sourceip, 6);
                }
                catch
                {
                    MessageBox.Show("连接失败！");
                    return;
                }
            }

            Form_e710init fe710 = new Form_e710init(rParms, modulerdr);
            fe710.ShowDialog();

            if (modulerdr != null)
            {
                modulerdr.Disconnect();
                isConnect = false;
                modulerdr = null;
            }
        }

        private void 标签温度及LEDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("请先连接设备后重试");
                return;
            }
            Form_tagtemperled frm = new Form_tagtemperled(modulerdr, rParms, this);
            frm.ShowDialog();
        }

        private void ex10系列初始化ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                sourceip = tbip.Text.Trim();
                try
                {
                    modulerdr = Reader.Create(sourceip, 6);
                }
                catch
                {
                    MessageBox.Show("连接失败！");
                    return;
                }
            }

            Form_e710init fe710 = new Form_e710init(rParms, modulerdr);
            fe710.ShowDialog();

            if (modulerdr != null)
            {
                modulerdr.Disconnect();
                isConnect = false;
                modulerdr = null;
            }
        }

        /*
        void restrf()
        {
            int cnt = 0;
            BackReadOption bro = new BackReadOption();
            bro.IsFastRead = true;

            bro.ReadDuration = (ushort)50;
            bro.ReadInterval = (uint)100;
            bro.FRTMetadata = new BackReadOption.FastReadTagMetaData();
            bro.FRTMetadata.IsAntennaID = true;
            bro.FRTMetadata.IsEmdData = false;
            bro.FRTMetadata.IsFrequency = false;
            bro.FRTMetadata.IsReadCnt = true;
            bro.FRTMetadata.IsRFU = false;
            bro.FRTMetadata.IsRSSI = false;
            bro.FRTMetadata.IsTimestamp = false;

            modulerdr.ParamSet("BackReadOption", bro);
            modulerdr.ParamSet("BackReadGPITrigger", null);
            modulerdr.ParamSet("ReadPlan",
                new SimpleReadPlan(TagProtocol.GEN2, new int[] { 1, 2 }));

            while (true)
            {
                modulerdr.StartReading();
                Thread.Sleep(500);
                modulerdr.StopReading();

                modulerdr.ParamSet("ResetRfidModule", (byte)0);
                Thread.Sleep(200);
                
                cnt++;
                Debug.WriteLine("ResetRfidModule " + cnt);
                if (cnt == 1000)
                    break;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(restrf);
            th.Start();
        }*/
    }

    public class AntAndBoll
    {
        public AntAndBoll(int ant, bool conn)
        {
            antid = ant;
            isConn = conn;
        }

        public int antid;
        public bool isConn;
        public UInt16 rpower;
        public UInt16 wpower;
    }

    public class AsyncTagMeta
    {
        public ushort ToFlags()
        {
            ushort ret = 0;
            if (IsReadCnt)
            {
                ret |= 0x1;
            }

            if (IsRSSI)
            {
                ret |= 0x1 << 1;
            }

            if (IsAntID)
            {
                ret |= 0x1 << 2;
            }

            if (IsFrequency)
            {
                ret |= 0x1 << 3;
            }

            if (IsTM)
            {
                ret |= 0x1 << 4;
            }

            if (IsRFU)
            {
                ret |= 0x1 << 5;
            }

            if (IsEmdData)
            {
                ret |= 0x1 << 7;
            }

            return ret;
        }
        public void Reset()
        {
            IsReadCnt = false;
            IsRSSI = false;
            IsAntID = true;
            IsFrequency = false;
            IsTM = false;
            IsRFU = false;
            IsEmdData = false;
        }
        public bool IsReadCnt { get; set; }
        public bool IsRSSI { get; set; }
        public bool IsAntID { get; set; }
        public bool IsFrequency { get; set; }
        public bool IsTM { get; set; }
        public bool IsRFU { get; set; }
        public bool IsEmdData { get; set; }
    }
    public class ReaderParams
    {
        public ReaderParams(int rdur, int sdur, int sess)
        {
            readdur = rdur;
            sleepdur = sdur;
            gen2session = sess;
            isIpModify = false;
            isM5eModify = false;
            fisrtLoad = true;

            ip = "";
            subnet = "";
            gateway = "";
            macstr = "";
            hasIP = false;
            isGetIp = false;
            Gen2Qval = -2;
            isCheckConnection = false;
            isMultiPotl = false;
            antcnt = -1;
            weightgen2 = 30;
            weight180006b = 30;
            weightipx64 = 30;
            weightipx256 = 30;

            isFastRead = false;
            isIdtAnts = false;
            IdtAntsType = 0;
            DurIdtval = 0;
            AfterIdtWaitval = 0;

            //           FixReadCount = 0;
            //           isReadFixCount = false;
            //           isOneReadOneTime = false;

            usecase_ishighspeedblf = false;
            usecase_tagcnt = -1;
            usecase_readperform = -1;
            usecase_antcnt = -1;
            FRTMeta = new BackReadOption.FastReadTagMetaData();

            FRTMeta.IsReadCnt = false;
            FRTMeta.IsRSSI = true;
            FRTMeta.IsAntennaID = true;
            FRTMeta.IsFrequency = false;
            FRTMeta.IsTimestamp = true;
            FRTMeta.IsRFU = false;
            FRTMeta.IsEmdData = false;
            GpiTriiger = null;

            reconnectcnt = 1;
            connectinterval = 5;
            istopbytime = false;
            stopsec = 0;
            ischkant = true;
            isEx10 = false;
        }

        public void resetParams()
        {
            isIpModify = false;
            isM5eModify = false;
            fisrtLoad = true;

            ip = "";
            subnet = "";
            gateway = "";
            macstr = "";
            hasIP = false;
            isGetIp = false;
            Gen2Qval = -2;
            isCheckConnection = false;
            isMultiPotl = false;
            antcnt = -1;
            weightgen2 = 30;
            weight180006b = 30;
            weightipx64 = 30;
            weightipx256 = 30;

            isChangeColor = true;
            isUniByEmd = false;
            isUniByAnt = false;

            isFastRead = false;
            isIdtAnts = false;
            IdtAntsType = 0;
            DurIdtval = 0;
            AfterIdtWaitval = 0;

            FixReadCount = 0;
            isReadFixCount = false;
            isOneReadOneTime = false;

            usecase_ishighspeedblf = false;
            usecase_tagcnt = -1;
            usecase_readperform = -1;
            usecase_antcnt = -1;

            FRTMeta.IsReadCnt = false;
            FRTMeta.IsRSSI = true;
            FRTMeta.IsAntennaID = true;
            FRTMeta.IsFrequency = false;
            FRTMeta.IsTimestamp = true;
            FRTMeta.IsRFU = false;
            FRTMeta.IsEmdData = false;

            GpiTriiger = null;
            reconnectcnt = 1;
            connectinterval = 5;
            istopbytime = false;
            stopsec = 0;
            ischkant = true;
            isEx10 = false;
        }
        //public Ex10FastModeParams Ex10;
        public BackReadOption.FastReadTagMetaData FRTMeta;//快速模式 后台盘点返回标签项标志位
        public GPITrigger GpiTriiger;//GPI 触发器
        public bool setGPO1;//是否设置gpo1
        public int gen2session;//gen2 协议 session项
        public int readdur; //盘点时长
        public int sleepdur;//休眠时间
        public int antcnt;//天线个数
        public string hardvir;//硬件版本
        public string softvir;//软件版本
        public ReaderType readertype;//读写器类型

        public List<AntAndBoll> AntsState = new List<AntAndBoll>();//天线状态
        public int ModuleReadervir;
        public string ip;//ip 地址
        public string subnet;//子网
        public string gateway;//网关
        public string macstr;//掩码
        public bool isGetIp;//是否获取ip
        public bool isIpModify;//ip是否更改
        public bool isM5eModify;//是否m5e更改
        public bool fisrtLoad;//首次加载
        public bool hasIP;//拥有ip地址
        public int powermin;//最大功率
        public int powermax;//最小功率
        public int Gen2Qval;//gen2 协议 q值
        public bool is1200hdstmd;//1200手持机模式
        public bool istagfoucs;//tagfoucs 模式
        public bool isfastid;
        public bool isFastRead;//是否快速模式
        public bool isEx10;//是否Ex10快速模式
        public int isCelerityType;
        public bool isCheckConnection;//是否检查连接
        public bool isMultiPotl;//是否多协议
        public SimpleReadPlan SixteenDevsrp = null;//简单盘点方式
        //public bool isRevertAnts;

        public int weightgen2;//gen2 权重
        public int weight180006b;//6b 权重
        public int weightipx64;//ipx64 权重
        public int weightipx256;//ipx256 权重

        public bool isChangeColor;//是否更改颜色
        public bool isUniByEmd;//是否附加数据唯一
        public bool isUniByAnt;//是否天线号唯一

        public bool isIdtAnts;//是否天线识别
        public int IdtAntsType;//识别天线类型
        public int DurIdtval;//判决时间
        public int AfterIdtWaitval;//等待时间

        public int FixReadCount;
        public bool isReadFixCount;
        public bool isOneReadOneTime;

        public bool usecase_ishighspeedblf;
        public int usecase_tagcnt;
        public int usecase_readperform;
        public int usecase_antcnt;

        public int reconnectcnt;//重连次数
        public int connectinterval;//重连间隔
        public bool istopbytime;
        public long endatetime;
        public int stopsec;
        public int antForTime;//单天线循环时间
        public int logTime;//日志统计间隔时间
        public bool isLogTime;//日志统计间隔时间

        public bool ischkant;

        public bool isSpecfres;//是否定频
       
        public bool isStandby; //待机时间
        //public int rssi;
        //public int 
    }

    public class TagInfoCompEPCId : IComparer<TagInfo>
    {
        public int Compare(TagInfo x, TagInfo y)
        {
            return x.epcid.CompareTo(y.epcid);
        }
    }

    public class TagInfoCompReadCnt : IComparer<TagInfo>
    {
        public int Compare(TagInfo x, TagInfo y)
        {
            return x.readcnt.CompareTo(y.readcnt);
        }
    }

    public class TagInfoCompPotl : IComparer<TagInfo>
    {
        public int Compare(TagInfo x, TagInfo y)
        {
            return x.potl.CompareTo(y.potl);
        }
    }

    public class TagInfoCompFreq : IComparer<TagInfo>
    {
        public int Compare(TagInfo x, TagInfo y)
        {
            return x.Frequency.CompareTo(y.Frequency);
        }
    }

    public class TagInfoCompPhase : IComparer<TagInfo>
    {
        public int Compare(TagInfo x, TagInfo y)
        {
            return x.Phase.CompareTo(y.Phase);
        }
    }

    public class TagInfoCompRssi : IComparer<TagInfo>
    {
        public int Compare(TagInfo x, TagInfo y)
        {
            return x.RssiRaw.CompareTo(y.RssiRaw);
        }
    }

    public class TagInfoCompEmdData : IComparer<TagInfo>
    {
        public int Compare(TagInfo x, TagInfo y)
        {
            return x.emddatastr.CompareTo(y.emddatastr);
        }
    }

    public class TagInfoCompAntId : IComparer<TagInfo>
    {
        public int Compare(TagInfo x, TagInfo y)
        {
            return x.antid.CompareTo(y.antid);
        }
    }

    public class AntCount
    {
        public string epcId { get; set; }

        public int ant1 { get; set; }

        public int ant2 { get; set; }

        public int ant3 { get; set; }

        public int ant4 { get; set; }

        public int ant5 { get; set; }

        public int ant6 { get; set; }

        public int ant7 { get; set; }

        public int ant8 { get; set; }

        public int ant9 { get; set; }

        public int ant10 { get; set; }

        public int ant11 { get; set; }

        public int ant12 { get; set; }

        public int ant13 { get; set; }

        public int ant14 { get; set; }

        public int ant15 { get; set; }

        public int ant16 { get; set; }
    }



    public class TagInfo
    {
        public TagInfo(string epc, int rcnt, int ant, DateTime time, TagProtocol potl_, string emdstr)
        {
            epcid = epc;
            readcnt = rcnt;
            antid = ant;
            timestamp = time;
            potl = potl_;
            emddatastr = emdstr;
            RssiSum = 0;
        }
        public string epcid;
        public int readcnt;
        public int antid;
        public TagProtocol potl;
        public DateTime timestamp;
        public string emddatastr;
        public int RssiSum;
        public int RssiRaw;
        public int Frequency;
        public int Phase;

        public bool Isshow;
    }

    class DoubleBufferListView : ListView
    {
        public DoubleBufferListView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }

    //日志类
    public class ProgramLog
    {
        readonly int logdayscnt;//日志文件保持数据的天数限制
        readonly string preffixname = null;//日志文件的固定前缀名 

        DateTime lastupdate = DateTime.Now.Date; //上一次创建新日志文件的时间
        StreamWriter curlogfile = null;//当前的日志文件

        public ProgramLog(int daycnts, string prefname)
        {
            logdayscnt = daycnts;
            preffixname = prefname;
            DelLogFile();
            curlogfile = File.CreateText(preffixname + "_" +
                DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        }
        //获取备份日志文件名字
        public string GetOldLog()
        {
            DirectoryInfo dir = null;
            dir = new DirectoryInfo(Environment.CurrentDirectory);
            FileInfo[] fls = dir.GetFiles();
            List<DateLogFile> logfiles = new List<DateLogFile>();
            foreach (FileInfo fl in fls)
            {
                if (fl.Name.StartsWith(preffixname))
                {
                    logfiles.Add(new DateLogFile(fl));
                }
            }
            return logfiles[0].logfi.Name;
        }
        //获取当前日志文件名 
        public string GetCurLog()
        {
            DirectoryInfo dir = null;
            dir = new DirectoryInfo(Environment.CurrentDirectory);
            FileInfo[] fls = dir.GetFiles();
            List<DateLogFile> logfiles = new List<DateLogFile>();
            foreach (FileInfo fl in fls)
            {
                if (fl.Name.StartsWith(preffixname))
                {
                    logfiles.Add(new DateLogFile(fl));
                }
            }
            return logfiles[logfiles.Count - 1].logfi.Name;
        }

        //删除多余日志，只保持日志文件不多于两个，且是最近创建的两个 
        private void DelLogFile()
        {
            DirectoryInfo dir = null;
            try
            {
                //根据文件名前缀，找出所有日志文件 
                dir = new DirectoryInfo(Environment.CurrentDirectory);
                FileInfo[] fls = dir.GetFiles();
                List<DateLogFile> logfiles = new List<DateLogFile>();
                foreach (FileInfo fl in fls)
                {
                    if (fl.Name.StartsWith(preffixname))
                    {
                        logfiles.Add(new DateLogFile(fl));
                    }
                }
                //根据日志文件创建的时间排序
                logfiles.Sort();
                //删除多余日子文件
                int delcnt = 0;
                if (logfiles.Count > 1)
                {
                    delcnt = logfiles.Count - 1;
                    for (int i = 0; i < delcnt; ++i)
                    {
                        logfiles[i].logfi.Delete();
                    }
                }
            }
            catch
            {
            }
            finally
            {
            }
        }
        //日志写入函数
        public void WriteLine(string line)
        {
            //首先判断是否需要建立新的日志文件，对某类日志文件都会规定一个期限，比如只存储三天的数据。一旦
            //当前日期大于文件创建日期三，则应该关闭当前日志文件，建立一个新的日志文件
            if (DateTime.Now.Date.Subtract(lastupdate).Days > logdayscnt)
            {
                //关闭当前日志文件
                curlogfile.Dispose();
                //删除多余日志文件
                DelLogFile();
                //创建新日志文件，命名规则为：固定前缀+'_'+日期字符串
                curlogfile = File.CreateText(preffixname + "_" +
                    DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                //更新前一次创建文件时间
                lastupdate = DateTime.Now.Date;
            }
            else //写入日志,一次写一行
            {
                curlogfile.WriteLine(DateTime.Now.ToString() + "--" + line);
                curlogfile.Flush();
            }

        }
        //内部类，用于日志文件按照创建日期进行排序
        class DateLogFile : IComparable<DateLogFile>
        {
            public DateLogFile(FileInfo fi)
            {
                logfi = fi;
            }
            public int CompareTo(DateLogFile other)
            {
                return logfi.CreationTime.CompareTo(other.logfi.CreationTime);
            }

            public FileInfo logfi;
        }
    }

    class ReaderExceptionChecker
    {
        public ReaderExceptionChecker(int maxerrcnt, int dursec)
        {
            dts = new DateTime[maxerrcnt];
            index = 0;
            maxdursec = dursec;
        }
        public void AddErr()
        {
            dts[index++] = DateTime.Now;
        }

        DateTime[] dts;
        readonly int maxdursec;
        int index;
        public bool IsTrigger()
        {
            DateTime now = DateTime.Now;
            if (index == dts.Length - 1)
            {
                if (now.Subtract(dts[0]).TotalSeconds < maxdursec)
                {
                    return true;
                }
                else
                {
                    index = 0;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}