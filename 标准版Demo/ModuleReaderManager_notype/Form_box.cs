using FileComponent;
using ModuleLibrary;
using ModuleTech;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    public partial class Form_box : Form
    {
        JArray table = new JArray();
        JArray labels = new JArray();
        JArray data1 = new JArray();
        JArray data2 = new JArray();
        public List<string> lvTagsList = new List<string>();
        public List<string> unreadLvTagsList = new List<string>();

        private ListViewItem[] myCache; //array to cache items for the virtual list
        private int firstItem; //stores the index of the first item in the cache
        public Form_box(Reader rdr, ReaderParams param, Form1 frm)
        {
            InitializeComponent();
            modulerdr = rdr;
            rparam = param;
            Mfrm = frm;

            //ListView listView1 = new ListView();
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

        ReaderParams rparam = null;
        Reader modulerdr = null;
        Form1 Mfrm = null;
        Dictionary<int, CheckBox> allAnts = new Dictionary<int, CheckBox>();

        int timerTime = 0;
        public static DateTime startTime;
        const int Box_Step1 = 0;
        const int Box_Step2 = 1;
        const int Box_Step3 = 2;
        int BoxMode_step = Box_Step1;
        int rfmode = 0;
        int defautlmode = 0;
        uint lowfre;
        uint cenfre;
        uint higfre;
        uint low2fre;
        uint hig2fre;
        int[] uants;
        bool isSetA;
        int roundcount = 0;
        int SKIPC = 2;
        bool isInventory = false;
        LogFile logf;
        int starttm;
        int starttm_V;
        int powermax;
        int powermin;
        int cbbgen2encoden = 0;
        List<int> antsExe = new List<int>();
        Thread readThread = null;
        Mutex opmutex = new Mutex();
        Dictionary<string, TagReadData> m_Tags = new Dictionary<string, TagReadData>();
        private void LogTo(string msg)
        {
            if (logf != null)
            {
                logf.To_Log(msg);
            }
        }
        int SetFilterSessioninTargetA(int[] ants, uint fre, int power)
        {
            try
            {
                R2000_calibration.FilterS2inA_DATA fsa = new R2000_calibration.FilterS2inA_DATA(ants, fre, power);
                R2000_calibration r2cb = new R2000_calibration();
                byte[] data = r2cb.GetSendCmd(R2000_calibration.R2000cmd.S2TA, fsa.ToByteData());
                //japi.DataTransportSend(hReader[0],data,data.length,1000);
                byte[] badata = modulerdr.SendandRev(data, 1000);
                return (badata[3] << 8) | badata[4];
            }
            catch (Exception)
            {
                MessageBox.Show("出现盘点错误,硬件不支持该功能!");
                return 0;
            }
        }
        public void Sort(ref uint[] array)
        {
            uint tmpIntValue = 0;
            for (int xIndex = 0; xIndex < array.Length; xIndex++)
            {
                for (int yIndex = 0; yIndex < array.Length; yIndex++)
                {
                    if (array[xIndex] < array[yIndex])
                    {
                        tmpIntValue = array[xIndex];
                        array[xIndex] = array[yIndex];
                        array[yIndex] = tmpIntValue;
                    }
                }
            }
        }

        private void Form_box_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
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

            checkBox_taglist.Checked = true;
            

            for (int f = 1; f <= allAnts.Count; ++f)
            {
                allAnts[f].Enabled = false;
            }

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
                int[] connectedants = (int[])modulerdr.ParamGet("ConnectedAntennas");
                for (int c = 0; c < connectedants.Length; ++c)
                {
                    allAnts[connectedants[c]].ForeColor = Color.Green;
                }

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
           
            powermax = (int)modulerdr.ParamGet("RfPowerMax");

            //AntPower[] apwrs = new AntPower[Mfrm.readerantnumber];
            //for (int v = 0; v < Mfrm.readerantnumber; ++v)
            //{
            //    apwrs[v].AntId = (byte)(v + 1);
            //    apwrs[v].ReadPower = (ushort)(powermax);
            //    apwrs[v].WritePower = (ushort)(powermax);

            //    //apwrs[v].ReadPower = (ushort)(3000);
            //    //apwrs[v].WritePower = (ushort)(3000);

            //}
            //modulerdr.ParamSet("AntPowerConf", apwrs);
            powermin = (int)modulerdr.ParamGet("RfPowerMin");

            // int gen2session = (int)modulerdr.ParamGet("Gen2Session");

            //modulerdr.ParamSet("HopAntTime", 2000);
            //modulerdr.ParamSet("Gen2Session", ModuleTech.Gen2.Session.Session2);

            if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7100 ||
                  modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7200 ||
                  modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7300 ||
                  modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7400)
            {
                defautlmode = 107;
            }
            else
            {
                defautlmode = 0x11;
            }

            logf = new LogFile("record.txt");
        }


        private void btnstart()
        {
            m_Tags.Clear();
            lvTags.Items.Clear();
            Console.WriteLine("start button-----AAAAA ");
            modulerdr.ParamSet("ReadPlan", new SimpleReadPlan(antsExe.ToArray()));

            try
            {

                ModuleTech.Region rg = (ModuleTech.Region)modulerdr.ParamGet("Region");
                modulerdr.ParamSet("Ex10FastModeParams", null);
                int hatv = (int)modulerdr.ParamGet("HopAntTime");

                uint[] htb = (uint[])modulerdr.ParamGet("FrequencyHopTable");
                if (htb != null)
                {
                    Sort(ref htb);
                }

                lowfre = htb[0];
                cenfre = htb[htb.Length / 2];
                higfre = htb[htb.Length - 1];
                low2fre = htb[htb.Length / 4];
                hig2fre = htb[(htb.Length - 1 + htb.Length / 2) / 2];

               

                //this.rtbopfailmsg.Text += hatv.ToString() + " ";

                starttm_V = Environment.TickCount;
               
                //this.rtbopfailmsg.Text += "fres " + e7mode.ToString() + " gen2code " + defautlmode + "\n";
                modulerdr.ParamSet("gen2tagEncoding", defautlmode);

                //this.rtbopfailmsg.Text += "set target A\n";
                modulerdr.ParamSet("Gen2Target", ModuleTech.Gen2.Target.A);


                if (cbbgen2encoden <= 3)
                {
                    rfmode = cbbgen2encoden;
                }
                else if (cbbgen2encoden > 3 && cbbgen2encoden <= 9)
                {
                    rfmode = 0x10 + cbbgen2encoden - 4;
                }
                else
                {
                    int val = cbbgen2encoden;
                    if (val == 10)
                    {
                        rfmode = 101;
                    }
                    else if (val == 11)
                    {
                        rfmode = 103;
                    }
                    else if (val == 12)
                    {
                        rfmode = 105;
                    }
                    else if (val == 13)
                    {
                        rfmode = 107;
                    }
                    else if (val == 14)
                    {
                        rfmode = 111;
                    }
                    else if (val == 15)
                    {
                        rfmode = 112;
                    }
                    else if (val == 16)
                    {
                        rfmode = 113;
                    }
                    else if (val == 17)
                    {
                        rfmode = 115;
                    }
                }
                modulerdr.ParamSet("gen2tagEncoding", rfmode);
                //this.rtbopfailmsg.Text += "gen2code " + rfmode.ToString() + "\n";

                isSetA = false;
                roundcount = 0;
                starttm = Environment.TickCount;
                //starttm_V = Environment.TickCount;
                //Console.WriteLine("start reading+++++111111");
                modulerdr.AsyncStartReading(0x20 | (0x0004 << 8));
                //Console.WriteLine("start reading+++++222222");
                isInventory = true;
                BoxMode_step = Box_Step1;
                readThread = new Thread(ReadFunc);

                readThread.Start();
            }
            catch (System.Exception)
            {
                return;
            }
            isInventory = true;

            Console.WriteLine("start button-----BBBBBB " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());
        }

        private void btnstartV2()
        {

            m_Tags.Clear();
            alist.Clear();
            lvTags.Items.Clear();
            lvTags.VirtualListSize = 0;
            lvTags.Invalidate();

            uint[] htb = (uint[])modulerdr.ParamGet("FrequencyHopTable");
            if (htb != null)
            {
                Sort(ref htb);
            }
            cenfre = htb[htb.Length / 2];
            
            SetFilterSessioninTargetA(uants, cenfre, powermax);
            
            Console.WriteLine("start button-----AAAAA ");
            
            modulerdr.ParamSet("ReadPlan", new SimpleReadPlan(antsExe.ToArray()));
            modulerdr.ParamSet("gen2tagEncoding", defautlmode);
            modulerdr.ParamSet("Gen2Target", ModuleTech.Gen2.Target.A);
            starttm = Environment.TickCount;
            starttm_V = Environment.TickCount;

            modulerdr.AsyncStartReading(0x20 | (0x0004 << 8));
            //modulerdr.AsyncStartReading(0x20 | (0x0004 << 8) | (0x0002 << 8));

            istimerct = true;
            timerControlThread = new Thread(ReadFunc);
            timerControlThread.Start();
        }

        private void btnstop(bool isupdate)
        {
            Console.WriteLine("stop button-----333333 " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());
            isInventory = false;
            // LogTo("thread before stop===:" + (Environment.TickCount - starttm_V - maxtestreaddur).ToString() + " " + threadmsg);
            opmutex.WaitOne();
            int ct = 0;
            try
            {
                Console.WriteLine("stop button-stopread-----333 " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());

                //LogTo("thread before stop2===:" + (Environment.TickCount - starttm_V - maxtestreaddur).ToString() + " " + threadmsg);

                modulerdr.AsyncStopReading();
                Console.WriteLine("stop button-stopread----444 " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());
                ct = Environment.TickCount - starttm_V;

                if (ct - maxtestreaddur > 500)
                {
                    LogTo(" time cost more than:" + (ct - maxtestreaddur));
                }

                TagReadData[] trdl = modulerdr.AsyncGetTags();
                if (trdl.Length > 0)
                {
                    Console.WriteLine("last tags:" + trdl.Length.ToString());
                    List<TagReadData> ttrd = new List<TagReadData>();
                    foreach (TagReadData read in trdl)
                    {
                        // Console.WriteLine("ReadCount: " + read.ReadCount);
                        if (read.Antenna != 105)
                        {
                            ttrd.Add(read);
                        }
                    }
                    if (ttrd.Count > 0)
                    {
                        iartest = BeginInvoke(new Handletest(testhandle), new object[] { 5, new List<object>() { ttrd } });
                    }
                }
            }
            catch (ModuleException mex)
            {
                Console.WriteLine(mex.Message + mex.ErrCode.ToString("X4"));
                iartest = BeginInvoke(new Handletest(testhandle), new object[] { 4, new List<object>() { "stop mex:" + mex.Message + mex.ErrCode.ToString("X4") + "\r\n" } });
            }
            catch (Exception ex)
            {
                iartest = BeginInvoke(new Handletest(testhandle), new object[] { 4, new List<object>() { "stop ex:" + ex.Message + "\r\n" } });
                Console.WriteLine("testhandle:" + ex.Message);
            }
            finally
            {
                opmutex.ReleaseMutex();
            }

            try
            {

                logf.To_Log((testtime + 1).ToString() + " " + ct.ToString() + " " + m_Tags.Count.ToString() + "\r\n");
                testtime++;
                alltimemill += ct;

                if (isupdate)
                {
                    iartest = BeginInvoke(new Handletest(testhandle), new object[] { 6, new List<object>() { testtime.ToString(), (alltimemill / testtime).ToString(), ct.ToString() } });
                }
            }
            catch (Exception)
            {
                return;
            }

            Console.WriteLine("stop button-----444444 " + testtime.ToString() + " " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());

            if (ct - maxtestreaddur > 4000 && isupdate)
            {
                iartest = BeginInvoke(new Handletest(testhandle), new object[] { 2, new List<object>() });
            }
        }

        Thread threadtest;
        bool istest;
        IAsyncResult iartest;
        int testcount;
        long alltimemill;
        int testtime;
        int maxtestreaddur;
        int testinterv = 10;
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
                    {
                        selants.Add(new AntAndBoll(cc, false));
                    }
                    else
                    {
                        selants.Add(new AntAndBoll(cc, true));
                    }
                }
            }

            return selants;
        }
        Thread timerControlThread = null;
        private void button_startest_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now;
            if (button_startest.Text == "开始测试")
            {
                timerTime = 0;
                label_timerTime.Text = "0";
                table = new JArray();
                labels = new JArray();
                data1 = new JArray();
                data2 = new JArray();
                alist = new List<ListViewItem>();

                label_tags.Text = "0";
                labreadtime.Text = "0";
                label_testcount.Text = "0";
                label_pjtime.Text = "0";
                labmaxdur.Text = "0";
                label_less.Text = "0";
                List<AntAndBoll> selants = null;
                antsExe.Clear();
                lvTags.Items.Clear();
                lvTags.VirtualListSize = 0;
                lvTags.Invalidate();
                selants = CheckAntsValid();
                if (selants.Count == 0)
                {
                    MessageBox.Show("请选择天线");
                    return;
                }

                for (int i = 0; i < selants.Count; ++i)
                {
                    antsExe.Add(selants[i].antid);
                }
                uants = antsExe.ToArray();
               
                testinterv = int.Parse(textBox_tci.Text);
                istest = true;
                rtbopfailmsg.Text = "";
                testcount = int.Parse(textBox_testcount.Text);
                maxtestreaddur = int.Parse(tbMaxReaddur.Text.Trim());
                testtime = 0;
                alltimemill = 0;
                timer1.Enabled = true;
                unreadLvTagsList = new List<string>();
                isInventory = true;
                
                btnstartV2();

                if (isInventory)
                {
                    testart = true;
                    logf.To_Log("次数:" + label_testcount.Text + " 平均:" + label_pjtime.Text + "\r\n");
                    threadtest = new Thread(run_test);
                    threadtest.Start();
                }
                rtbopfailmsg.Text = "start:" + DateTime.Now.ToString() + "\r\n";
                button_startest.Text = "停止测试";
               
            }
            else
            {
                // Application.DoEvents();
                Console.WriteLine("button================111");

                istest = false;
                timer1.Enabled = false;
                timerTime = 0;
                if (iartest != null)
                {
                    EndInvoke(iartest);
                }

                Console.WriteLine("button================222");
                if (threadtest != null)
                {
                    threadtest.Join();
                }

                btnstop(false);

                Console.WriteLine("button================333");
                logf.To_Log("次数:" + label_testcount.Text + " 平均:" + label_pjtime.Text + "\r\n");
                button_startest.Text = "开始测试";
            }
        }
        bool istimerct = true;
        IAsyncResult iar_timer, iar_test;
        int stgettemp, statenvtick;
        delegate void TimerHandleDelg(int type, object paramslist);

        void TimerHandle(int type, object paramslist)
        {
            labreadtime.Text = paramslist.ToString();
            if (type == 0)
            {

            }
            else if (type == 3)
            {
                labreadtime.Text = "0";
                button1.PerformClick();
            }
            else if (type == 2)
            {

            }
            else
            {
                if (modulerdr != null)
                {
                    modulerdr.StopReading();

                    TagReadData[] trds = modulerdr.AsyncGetTags();
                    if (trds.Length > 0)
                    {
                        List<object> list = new List<object>();
                        foreach (TagReadData tag in trds)
                        {
                            list.Add(tag);
                        }
                        testhandle(5, list);
                    }
                }
                //btnstop.PerformClick();
            }
        }

        delegate void Handletest(int type, List<object> lobj);
        bool testart;
        public  List<ListViewItem> alist = new List<ListViewItem>();
        private void testhandle(int type, List<object> lobj)
        {
            try
            {
                switch (type)
                {
                    case 5:
                        {
                            List<TagReadData> ltd = (List<TagReadData>)(lobj[0]);
                            //DateTime
                            
                            for (int i = 0; i < ltd.Count; i++)
                            {
                                if (!unreadLvTagsList.Contains(ltd[i].EPCString))
                                {
                                    unreadLvTagsList.Add(ltd[i].EPCString);
                                }

                                if (!m_Tags.ContainsKey(ltd[i].EPCString))
                                {
                                    m_Tags.Add(ltd[i].EPCString, ltd[i]);

                                    var ltdTime = ltd[0].Time;
                                    TimeSpan tss = ltdTime.Subtract(startTime);
                                    ListViewItem lvi = new ListViewItem(m_Tags.Count.ToString());
                                    lvi.SubItems.Add(ltd[i].ReadCount.ToString());
                                    lvi.SubItems.Add(ltd[i].EPCString);
                                    lvi.SubItems.Add(ltd[i].Antenna.ToString());
                                    lvi.SubItems.Add(ltd[i].EMDDataString);
                                    lvi.SubItems.Add("Gen2");
                                    lvi.SubItems.Add(ltd[i].Rssi.ToString());
                                    lvi.SubItems.Add(ltd[i].Frequency.ToString());
                                    lvi.SubItems.Add(ltd[i].Phase.ToString());
                                    //lvi.SubItems.Add($"{tss.Hours}时{tss.Minutes}分{tss.Seconds}秒{tss.Milliseconds}毫秒");
                                    lvi.SubItems.Add( tss.Hours+"时"+tss.Minutes+"分"+tss.Seconds+"秒"+tss.Milliseconds+"毫秒");
                                    if (istaglist)
                                    {
                                        //lvTags.Items.Add(lvi);
                                        alist.Add(lvi);
                                        //firstItem++;
                                    }
                                }
                            }
                            myCache = alist.ToArray();
                            lvTags.VirtualListSize = alist.Count;
                            label_tags.Text = m_Tags.Count.ToString();
                            //if(istaglist)
                            //this.lvTags.Refresh();
                            //Application.DoEvents();
                        }
                        break;
                    case 6:
                        labels.Add("次数" + lobj[0].ToString());
                        data1.Add(Convert.ToInt32(lobj[2].ToString()));
                        data2.Add(Convert.ToInt32(label_tags.Text));

                        JObject jArrayTable = new JObject();
                        jArrayTable.Add(new JProperty("name", "次数" + lobj[0].ToString()));
                        jArrayTable.Add(new JProperty("tags", label_tags.Text));
                        jArrayTable.Add(new JProperty("mis", lobj[2].ToString()));
                        jArrayTable.Add(new JProperty("count", textBox_testcount.Text));
                        jArrayTable.Add(new JProperty("time", DateTime.Now.ToString()));

                        var unreadList = lvTagsList.Except(unreadLvTagsList).ToList();
                        jArrayTable.Add(new JProperty("unread", unreadList));

                        unreadLvTagsList = new List<string>();
                        
                        table.Add(jArrayTable);

                        int ct = int.Parse(lobj[2].ToString());
                        labreadtime.Text = lobj[2].ToString();
                        label_testcount.Text = lobj[0].ToString();
                        label_pjtime.Text = lobj[1].ToString();
                        if (int.Parse(label_tags.Text) < testcount)
                        {
                            label_less.Text = (int.Parse(label_less.Text) + 1).ToString();
                        }

                        if (int.Parse(labmaxdur.Text) < ct)
                        {
                            labmaxdur.Text = ct.ToString();
                        }

                        timer1.Enabled = false;
                        timerTime = 0;
                        label_timerTime.Text = "0";

                        int intLess = int.Parse(label_less.Text);
                        int inttestcount = int.Parse(label_testcount.Text);
                        label_succeedRatio.Text = GetPercent((inttestcount - intLess), inttestcount);
                        int count = int.Parse(textBox_count.Text);
                        
                        firstItem = 0;
                        if (inttestcount >= count)
                            button_startest.PerformClick();

                        break;
                    case 0:
                        {
                            timer1.Enabled = true;
                            
                            isInventory = true;
                            btnstartV2();
                           
                            testart = true;
                        }
                        break;
                    case 1:
                        {
                            //Console.WriteLine("button stop :" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());
                            //if(Environment.TickCount - starttm_V-maxtestreaddur>90)
                            //Console.WriteLine(DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond + " time up2 " + (Environment.TickCount - starttm_V).ToString());
                            if (Environment.TickCount - starttm_V - maxtestreaddur > 90)
                            {
                                LogTo(" time up2 " + (Environment.TickCount - starttm_V).ToString() + " " + m_Tags.Count.ToString() + "\r\n");
                            }

                            btnstop(true);
                            testart = false;
                        }
                        break;
                    case 2:
                        {
                            button_startest.PerformClick();
                        }
                        break;
                    case 3:
                    case 4:
                        rtbopfailmsg.Text += DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond.ToString() + "  " + lobj[0].ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("testhandle:" + ex.Message);
                if (ex is ModuleException)
                {
                    rtbopfailmsg.Text += DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond.ToString() + "  " + ((ModuleException)ex).ErrCode.ToString("X4") + ex.Message;
                }
                else
                {
                    rtbopfailmsg.Text += DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond.ToString() + "  " + ex.Message;
                }
            }
        }
        public static string GetPercent(double value, double total)
        {
            if (total == 0)
            {
                return "0%";
            }
            var ss = Math.Round(value / total, 5);
            return Math.Round(value / total, 5).ToString("P");
        }

        private void run_test()
        {
            while (istest)
            {
                if (!testart)
                {
                    if (istest)
                    {
                        iartest = (IAsyncResult)Invoke(new Handletest(testhandle), new object[] { 0, new List<object>() });
                    }
                }

                bool isStop = false;
                if (testcount == 0)
                {

                    if (Environment.TickCount - starttm_V > maxtestreaddur)
                    {
                        isStop = true;
                    }
                }
                else
                {
                    if (m_Tags.Count >= testcount || Environment.TickCount - starttm_V > maxtestreaddur)
                    {
                        isStop = true;
                    }
                }

                if (isStop)
                {
                    if (istest && isInventory)
                    {

                        Console.WriteLine(DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond + " time up " + (Environment.TickCount - starttm_V).ToString());
                        isInventory = false;
                        iartest = (IAsyncResult)Invoke(new Handletest(testhandle), new object[] { 1, new List<object>() });

                    }

                    //if (istest)
                    //    Thread.Sleep(10000);

                    for (int m = 0; m < testinterv; m++)
                    {
                        if (istest)
                        {
                            Thread.Sleep(1000);
                            startTime = DateTime.Now;//重新记录开始时间
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                // tagmutex.ReleaseMutex();
                //System.Diagnostics.Debug.WriteLine("333333333333");
                if (istest)
                {
                    Thread.Sleep(5);
                }
            }

            Console.WriteLine("run_test is over");
        }

        void ReadFunc()
        {
            while (isInventory)
            {
                Console.WriteLine("Readfun------AAAAAA " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());
                // threadmsg = "";
                opmutex.WaitOne();
                try
                {
                    // threadmsg += "1>";
                    Console.WriteLine("Readfun---AsyncGetTags " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());
                    TagReadData[] reads = modulerdr.AsyncGetTags();
                    
                    List<TagReadData> ttrd = new List<TagReadData>();
                    foreach (TagReadData read in reads)
                    {
                        if (read.Antenna != 105)
                        {
                            ttrd.Add(read);
                        }
                        else
                        {
                            if (ttrd.Count > 0)
                            {
                                List<TagReadData> ltemp = new List<TagReadData>();
                                ltemp.AddRange(ttrd);
                                iartest = BeginInvoke(new Handletest(testhandle), new object[] { 5, new List<object>() { ltemp } });
                                ttrd.Clear();
                            }

                            if (roundcount < SKIPC - 1)
                            {
                                //threadmsg += "A>";
                                roundcount++;
                                break;
                            }

                            roundcount = 0;
                            Console.WriteLine("Readfun-stop reading-----777777 " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());
                            if (isInventory)
                            {
                                modulerdr.AsyncStopReading();
                            }
                            else
                            {
                                return;
                            }
                            if (BoxMode_step == Box_Step1 && isInventory)
                            {
                                Console.WriteLine("Readfun---gen2tagEncoding " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());
                                if (isInventory)
                                    modulerdr.ParamSet("gen2tagEncoding", 113);
                                else
                                    return;

                                BoxMode_step = Box_Step2;
                            }
                            else if (BoxMode_step == Box_Step2 && isInventory)
                            {
                                if (isSetA)
                                {
                                    Console.WriteLine("Readfun---gen2tagEncoding2 " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());

                                    if (isInventory)
                                        modulerdr.ParamSet("gen2tagEncoding", defautlmode);
                                    else
                                        return;

                                    Console.WriteLine("Readfun---Gen2TargetA RFmode " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());

                                    if (isInventory)
                                        modulerdr.ParamSet("Gen2Target", ModuleTech.Gen2.Target.A);
                                    else
                                        return;

                                    if (isInventory)
                                        modulerdr.ParamSet("gen2tagEncoding", rfmode);
                                    else
                                        return;

                                    isSetA = false;
                                }
                                else
                                {
                                    if (isInventory)
                                        modulerdr.ParamSet("Gen2Target", ModuleTech.Gen2.Target.B);
                                    else
                                        return;
                                   

                                    if (isInventory)
                                        modulerdr.ParamSet("gen2tagEncoding", rfmode);
                                    else
                                        return;

                                    isSetA = true;
                                }

                                BoxMode_step = Box_Step1;
                            }
                            starttm = Environment.TickCount;
                            if (isInventory)
                            {
                                Console.WriteLine("Readfun-start reading-----6666666 " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());

                                if (BoxMode_step == Box_Step3)
                                {
                                    modulerdr.AsyncStartReading((0x0004 << 8));
                                }
                                else
                                {
                                    modulerdr.AsyncStartReading(0x20 | (0x0080 << 8));
                                }
                            }
                        }
                    }

                    if (ttrd.Count > 0)
                    {
                        List<TagReadData> ltemp = new List<TagReadData>();
                        ltemp.AddRange(ttrd);
                        iartest = BeginInvoke(new Handletest(testhandle), new object[] { 5, new List<object>() { ltemp } });
                    }
                }
                catch (OpFaidedException exxx)
                {
                    iartest = BeginInvoke(new Handletest(testhandle), new object[] { 3, new List<object>() { exxx.ToString() } });
                }
                catch (Exception ex)
                {
                    iartest = BeginInvoke(new Handletest(testhandle), new object[] { 4, new List<object>() { ex.StackTrace+"--"+ex.ToString() } });
                    return;
                }
                finally
                {
                    opmutex.ReleaseMutex();
                }
                Console.WriteLine("Readfun------BBBBBB " + (Environment.TickCount - starttm_V - maxtestreaddur).ToString());
                Thread.Sleep(25);
            }
        }

        private void Form_box_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (logf != null)
            {
                logf.Dispose();
            }
        }

        bool istaglist = false;
        private void checkBox_taglist_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_taglist.Checked)
            {
                istaglist = true;
            }
            else
            {
                istaglist = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_timerTime.Text = (Environment.TickCount - starttm_V).ToString();
        }

        private void 设为对比数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvTagsList = new List<string>();
            for (int i = 0; i < lvTags.Items.Count; i++)
            {
                lvTagsList.Add(lvTags.Items[i].SubItems[2].Text);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Ex10FastModeParams ex10 = new Ex10FastModeParams();
            ex10.ex10_type = 0x00;
            modulerdr.ParamSet("Ex10FastModeParams", ex10);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Ex10FastModeParams ex10 = new Ex10FastModeParams();
            ex10.ex10_type = 0x01;
            modulerdr.ParamSet("Ex10FastModeParams", ex10);
        }

        private void lvTags_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            myCache =alist.OrderBy(r => r.SubItems[e.Column].Text).ToArray();
            lvTags.VirtualListSize = alist.Count;
            lvTags.Invalidate();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            modulerdr.ParamSet("Ex10FastModeParams", null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filename = null;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "csv(*.csv)|*.csv|txt(*.txt)| *.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName;
                FileInfo fileInfo = new FileInfo(filename);
                fileInfo.Delete();
                StreamWriter streamWriter = fileInfo.CreateText();
                string headline = "读取次数,EPC ID,天线,附加数据,协议,RSSI,频率,相位,首次读取时间";
                streamWriter.WriteLine(headline);

                //streamWriter.WriteLine("格式：epc，读次数，天线，附加数据，协议,RSSI,读取次数");
                string wline = "";
                foreach (TagReadData viewitem in m_Tags.Values)
                {
                    wline = viewitem.ReadCount + "," + viewitem.EPCString + "," +
                       viewitem.Antenna + "," + viewitem.EMDDataString + ",GEN2,"
                       + viewitem.Rssi + "," + viewitem.Frequency + ","
                       + viewitem.Phase + "," + viewitem.Time.ToString();
                    streamWriter.WriteLine(wline);
                }
                //foreach (ListViewItem viewitem in lvTags.Items)
                //{
                //     wline = viewitem.SubItems[1].Text + "," + viewitem.SubItems[2].Text + "," +
                //        viewitem.SubItems[3].Text + "," + viewitem.SubItems[4].Text + "," +
                //        viewitem.SubItems[5].Text + "," + viewitem.SubItems[6].Text + "," +
                //        viewitem.SubItems[7].Text + "," + viewitem.SubItems[8].Text + "," + viewitem.SubItems[9].Text;
                //    streamWriter.WriteLine(wline);
                //}
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            JObject data = new JObject();
            JArray datasets = new JArray();

            JObject jObject1 = new JObject();
            JObject jObject2 = new JObject();

            jObject1.Add(new JProperty("label", "时间"));
            jObject1.Add(new JProperty("backgroundColor", "rgba(117, 193, 129, 0.8)"));
            jObject1.Add(new JProperty("hoverBackgroundColor", "rgba(117,193,129,1)"));
            jObject1.Add(new JProperty("data", data1));

            jObject2.Add(new JProperty("label", "标签数"));
            jObject2.Add(new JProperty("backgroundColor", "rgba(91,153,234,0.8)"));
            jObject2.Add(new JProperty("hoverBackgroundColor", "rgba(91,153,234,1)"));
            jObject2.Add(new JProperty("data", data2));

            datasets.Add(jObject1);
            datasets.Add(jObject2);

            data.Add(new JProperty("labels", labels));
            data.Add(new JProperty("datasets", datasets));

            Form_box_html form_Box_Html = new Form_box_html(data, table);
            form_Box_Html.Show();
        }

    }
}
