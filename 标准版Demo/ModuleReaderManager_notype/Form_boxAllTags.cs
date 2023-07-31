using ModuleLibrary;
using ModuleTech;
using ModuleTech.Gen2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static ModuleReaderManager.models;

namespace ModuleReaderManager
{
    public partial class Form_boxAllTags : Form
    {
        public Reader modulerdr;
        public ReaderParams rParms;
        Mutex mutex = new Mutex();
        Thread readThread = null;

        int roundcount = 0;
        int antForCount = 0;// 天线循环次数
        int abForCount = 0;//ab 循环次数
        int rfModelForCount = 0;//rfmodel 循环次数
        bool isInventory = false;
        bool isInventoryV2 = false;//内部循环的控制
        string currentRfMode = "";//当前rfModel
        int currentQval =0;//当前Qval
        int currentAntTime = 0;//当前天线停留时间
        List<Rfmodels> rfModellist = null;
        List<uint> htb = new List<uint>();//频点
        List<int> ants = new List<int>();//天线

        int stop_Time = 0;
        int stop_Count = 0;
        int power = 0;
        Dictionary<string, TagReadData> m_Tags = new Dictionary<string, TagReadData>();
        public List<ListViewItem> alist = new List<ListViewItem>();
        Target nextTarget;
        int tickCount=0;
        IAsyncResult iartest;

        private ListViewItem[] myCache; //array to cache items for the virtual list
        private int firstItem; //stores the index of the first item in the cache
        public Form_boxAllTags(Reader _modulerdr, ReaderParams _rParms)
        {
            InitializeComponent();
            modulerdr = _modulerdr;
            rParms = _rParms;

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

        private void Form_boxAllTags_Load(object sender, EventArgs e)
        {
            var powermax = modulerdr.ParamGet("RfPowerMax");
            Power.Text = powermax.ToString();

            this.WindowState = FormWindowState.Maximized;
            FrequencyHopTable();
            Gen2Target.Items.AddRange(new object[] {"A","B","A-B","B-A"});
            Gen2Target.SelectedIndex = 0;

            //区域
            try
            {
                int st = Environment.TickCount;
                ModuleTech.Region rg = (ModuleTech.Region)modulerdr.ParamGet("Region");
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
                    case ModuleTech.Region.CE_HIGH:
                        this.cbbregion.SelectedIndex = 11;
                        break;
                    case ModuleTech.Region.HK:
                        this.cbbregion.SelectedIndex = 12;
                        break;
                    case ModuleTech.Region.TAIWAN:
                        this.cbbregion.SelectedIndex = 13;
                        break;
                    case ModuleTech.Region.MALAYSIA:
                        this.cbbregion.SelectedIndex = 14;
                        break;
                    case ModuleTech.Region.SOUTH_AFRICA:
                        this.cbbregion.SelectedIndex = 15;
                        break;
                    case ModuleTech.Region.BRAZIL:
                        this.cbbregion.SelectedIndex = 16;
                        break;
                    case ModuleTech.Region.THAILAND:
                        this.cbbregion.SelectedIndex = 17;
                        break;
                    case ModuleTech.Region.SINGAPORE:
                        this.cbbregion.SelectedIndex = 18;
                        break;
                    case ModuleTech.Region.AUSTRALIA:
                        this.cbbregion.SelectedIndex = 19;
                        break;
                    case ModuleTech.Region.URUGUAY:
                        this.cbbregion.SelectedIndex = 20;
                        break;
                    case ModuleTech.Region.VIETNAM:
                        this.cbbregion.SelectedIndex = 21;
                        break;
                    case ModuleTech.Region.ISRAEL:
                        this.cbbregion.SelectedIndex = 22;
                        break;
                    case ModuleTech.Region.PHILIPPINES:
                        this.cbbregion.SelectedIndex = 23;
                        break;
                    case ModuleTech.Region.INDONESIA:
                        this.cbbregion.SelectedIndex = 24;
                        break;
                    case ModuleTech.Region.NEW_ZEALAND:
                        this.cbbregion.SelectedIndex = 25;
                        break;
                    case ModuleTech.Region.PERU:
                        this.cbbregion.SelectedIndex = 26;
                        break;
                    case ModuleTech.Region.RUSSIA:
                        this.cbbregion.SelectedIndex = 27;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取失败");
            }

            //天线信息初始化
            foreach (Control ctl in groupBox2.Controls)
            {
                if (!ctl.Name.StartsWith("Ant"))
                    continue;

                int antid = int.Parse(ctl.Name.Substring(3, ctl.Name.Length - 3));
                if (antid <= rParms.antcnt)
                    ctl.Enabled = true;
                else
                    ctl.Enabled = false;
            }

            int[] connectedants = (int[])modulerdr.ParamGet("ConnectedAntennas");
            for (int c = 0; c < connectedants.Length; ++c) { 
                var groupBox2_ant=(CheckBox)groupBox2.Controls["Ant" + connectedants[c]];
                groupBox2_ant.ForeColor = Color.Green;
                groupBox2_ant.Checked=true;
            }

            //RFmode参数配置 数据初始化
            foreach (Control ctl in groupBox1.Controls)
            {
                if (ctl.Name.IndexOf("_rfmod")>0) {
                    var comboBox = ((ComboBox)ctl);
                    if ((int)modulerdr.HwDetails.module >= 24)
                    {
                        comboBox.Items.AddRange(new object[] {
                        "FM0","M2","M4","M8","RF_MODE_1","RF_MODE_3","RF_MODE_5","RF_MODE_7","RF_MODE_11","RF_MODE_12","RF_MODE_13","RF_MODE_15"});
                    }
                    else
                    {
                        comboBox.Items.AddRange(new object[] {
                        "FM0","M2","M4","M8","PROFILE 0","PROFILE 1","PROFILE 2","PROFILE 3","PROFILE 4","PROFILE 5"});
                    }
                    comboBox.SelectedIndex = 0;
                }

                if (ctl.Name.IndexOf("_qval")>0) {
                    ((ComboBox)ctl).Items.AddRange(new object[] {
                    "自动",
                    "0",
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9",
                    "10",
                    "11",
                    "12",
                    "13",
                    "14",
                    "15"});
                    ((ComboBox)ctl).SelectedIndex = 0;
                }

                if (ctl.Name.IndexOf("_count") > 0)
                    ((TextBox)ctl).Text = "3";

                if (ctl.Name.IndexOf("_time") > 0)
                    ((TextBox)ctl).Text = "4000";

                if (!ctl.Name.StartsWith("RFmode"))
                continue;

                ctl.Enabled = false;
            }
            checkBox_1.Checked = true;
            checkBox_2.Checked = true;
            checkBox_3.Checked = true;
        }

        private void checkBox_A_CheckedChanged(object sender, EventArgs e)
        {
            ControlEnabled("RFmode" + ((CheckBox)sender).Name.Split('_')[1], ((CheckBox)sender).Checked);
        }

        private void checkBox_B_CheckedChanged(object sender, EventArgs e)
        {
            ControlEnabled("RFmode" + ((CheckBox)sender).Name.Split('_')[1], ((CheckBox)sender).Checked);
        }

        private void checkBox_C_CheckedChanged(object sender, EventArgs e)
        {
            ControlEnabled("RFmode" + ((CheckBox)sender).Name.Split('_')[1], ((CheckBox)sender).Checked);
        }

        private void checkBox_D_CheckedChanged(object sender, EventArgs e)
        {
            ControlEnabled("RFmode" + ((CheckBox)sender).Name.Split('_')[1], ((CheckBox)sender).Checked);
        }

        private void checkBox_E_CheckedChanged(object sender, EventArgs e)
        {
            ControlEnabled("RFmode" + ((CheckBox)sender).Name.Split('_')[1], ((CheckBox)sender).Checked);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isInventory)
            {
                rfModelForCount = 0;
                abForCount = 0;
                isInventory = false;
                timer1.Stop();

                if (iartest != null)
                    EndInvoke(iartest);

                if (readThread != null) { 
                    readThread.Join();
                }
               
                modulerdr.AsyncStopReading();
                button4.Text = "开始盘点";
            }
            else {
                if (ants == null|| ants.Count<=0) {
                    button1.PerformClick();
                }

                if (s2_a.Checked) {
                    //默认RF_MODE_7
                    modulerdr.ParamSet("gen2tagEncoding", getGen2val("RF_MODE_7"));
                    if (htb.Count <= 3)
                    {
                        for (int i = 0; i < htb.Count; i++)
                        {
                            if (SetFilterSessioninTargetA(ants.ToArray(), htb[i], Convert.ToInt32(power)) == -1)
                                return;
                        }
                    }
                    else {
                        //低频点
                        if (SetFilterSessioninTargetA(ants.ToArray(), htb[0], Convert.ToInt32(power)) == -1)
                            return;
                        //中频点
                        if (SetFilterSessioninTargetA(ants.ToArray(), htb[htb.Count/2], Convert.ToInt32(power)) == -1)
                            return;
                        //高频点
                        if (SetFilterSessioninTargetA(ants.ToArray(), htb[htb.Count-1], Convert.ToInt32(power)) == -1)
                            return;
                    }
                }
                

                modulerdr.ParamSet("Gen2Session", ModuleTech.Gen2.Session.Session2);//S2
                modulerdr.ParamSet("Ex10FastModeParams", null);

                abForCount = Gen2Target.SelectedIndex <=1 ? 1 : 2;

                //获取RFmode循环次数
                foreach (Control ctl in groupBox1.Controls)
                {
                if (!ctl.Name.StartsWith("checkBox"))
                    continue;

                if (((CheckBox)ctl).Checked)
                    rfModelForCount++;
                }

                //获取 rfmodel列表的配置信息
                rfModellist = new List<Rfmodels>();
                for (int i = 0; i < rfModelForCount; i++)
                {
                    Rfmodels rfmodel= new Rfmodels();
                    
                    ComboBox rfmode = this.Controls.Find("RFmode" + (i + 1) + "_rfmode", true)[0] as ComboBox;
                    rfmodel.rfModel = rfmode.Text;

                    ComboBox qval = this.Controls.Find("RFmode" + (i + 1) + "_qval", true)[0] as ComboBox;
                    rfmodel.qval = qval.SelectedIndex;

                    TextBox time = this.Controls.Find("RFmode" + (i + 1) + "_time", true)[0] as TextBox;
                    rfmodel.time = Convert.ToInt32(time.Text);

                    TextBox count = this.Controls.Find("RFmode" + (i + 1) + "_count", true)[0] as TextBox;
                    rfmodel.count = Convert.ToInt32(count.Text);
                    rfModellist.Add(rfmodel);
                }

                rtbopfailmsg.Text = "";
                if (this.Gen2Target.SelectedIndex < 2) { 
                    modulerdr.ParamSet("Gen2Target", (ModuleTech.Gen2.Target)this.Gen2Target.SelectedIndex);
                    rtbopfailmsg.Text += $"配置模块参数: Gen2Target-{(ModuleTech.Gen2.Target)this.Gen2Target.SelectedIndex} \n";
                    nextTarget = (ModuleTech.Gen2.Target)this.Gen2Target.SelectedIndex;
                }
                else if(this.Gen2Target.SelectedIndex==2){
                    modulerdr.ParamSet("Gen2Target", ModuleTech.Gen2.Target.A);
                    rtbopfailmsg.Text += $"配置模块参数: Gen2Target-A \n";
                    nextTarget = ModuleTech.Gen2.Target.B;
                }
                else if(this.Gen2Target.SelectedIndex == 3){
                    modulerdr.ParamSet("Gen2Target", ModuleTech.Gen2.Target.B);
                    rtbopfailmsg.Text += $"配置模块参数: Gen2Target-B \n";
                    nextTarget = ModuleTech.Gen2.Target.A;
                }

                stop_Time = int.Parse(stopTime.Text);
                stop_Count = int.Parse(stopCount.Text);

                alist.Clear();
                m_Tags.Clear();
                myCache = null;
                lvTags.VirtualListSize = alist.Count;

                tickCount = Environment.TickCount;
                label_tags.Text = "0";
                button4.Text = "停止盘点";
                timer1.Start();
                isInventory = true;
                isInventoryV2 = true;
                readThread = new Thread(ReadFunc);
                readThread.Start();
            }
        }

        private void cbbregion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModuleTech.Region rg = ModuleTech.Region.UNSPEC;
            bool is840_845 = false;
            bool is840_925 = false; ;
            if (this.cbbregion.SelectedIndex == -1)
            {
                MessageBox.Show("请选择区域");
                return;
            }
            switch (this.cbbregion.SelectedIndex)
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
                        if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100 ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1200 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1300 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5100 ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5200 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                        {
                            if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC ||
                                modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100 ||
                                 modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1200 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1300 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5100 ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5200 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                                rg = ModuleTech.Region.PRC2;
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
                        if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100)
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
                case 10:
                    rg = ModuleTech.Region.EU3;

                    break;
                case 11:
                    rg = ModuleTech.Region.CE_HIGH;
                    break;
                case 12:
                    rg = ModuleTech.Region.HK;
                    break;
                case 13:
                    rg = ModuleTech.Region.TAIWAN;
                    break;
                case 14:
                    rg = ModuleTech.Region.MALAYSIA;
                    break;
                case 15:
                    rg = ModuleTech.Region.SOUTH_AFRICA;
                    break;
                case 16:
                    rg = ModuleTech.Region.BRAZIL;
                    break;
                case 17:
                    rg = ModuleTech.Region.THAILAND;
                    break;
                case 18:
                    rg = ModuleTech.Region.SINGAPORE;
                    break;
                case 19:
                    rg = ModuleTech.Region.AUSTRALIA;
                    break;
                case 20:
                    rg = ModuleTech.Region.URUGUAY;
                    break;
                case 21:
                    rg = ModuleTech.Region.VIETNAM;
                    break;
                case 22:
                    rg = ModuleTech.Region.ISRAEL;
                    break;
                case 23:
                    rg = ModuleTech.Region.PHILIPPINES;
                    break;
                case 24:
                    rg = ModuleTech.Region.INDONESIA;
                    break;
                case 25:
                    rg = ModuleTech.Region.NEW_ZEALAND;
                    break;
                case 26:
                    rg = ModuleTech.Region.PERU;
                    break;
                case 27:
                    rg = ModuleTech.Region.RUSSIA;
                    break;
            }

            if (rParms.readertype == ReaderType.PR_ONEANT)
            {
                if (rg == ModuleTech.Region.OPEN || rg == ModuleTech.Region.CN)
                {
                    MessageBox.Show("不支持的区域");
                    return;
                }
            }
            try
            {
                modulerdr.ParamSet("Region", rg);
                rParms.isSpecfres = false;
                if (this.cbbregion.SelectedIndex == 10)
                {
                    List<uint> htab = new List<uint>();
                    htab.Add(867700);
                    modulerdr.ParamSet("FrequencyHopTable", htab.ToArray());
                    rParms.isSpecfres = true;
                }

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
                    modulerdr.ParamSet("FrequencyHopTable", htab.ToArray());
                    rParms.isSpecfres = true;
                }
                else
                {

                }
            }
            catch (Exception)
            {
                MessageBox.Show("设置失败 ");
            }

            try
            {
                int st = Environment.TickCount;
                uint[] htb = (uint[])modulerdr.ParamGet("FrequencyHopTable");

                if (htb != null)
                    Sort(ref htb);
               
                int cnt = 0;
                uint curchal = htb[htb.Length - 1];
                lvhoptb.Items.Clear();
                foreach (uint fre in htb)
                {
                    cnt++;
                    ListViewItem item = new ListViewItem(fre.ToString());
                    if (rParms.readertype == ReaderType.PR_ONEANT)
                    {
                        if (cnt == htb.Length)
                            break;
                        if (curchal == fre)
                            item.Checked = true;
                    }
                    else
                        item.Checked = true;

                    lvhoptb.Items.Add(item);
                }
                this.allcheckBox.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取失败");
            }
        }

        void ReadFunc()
        {
            try
            {
                mutex.WaitOne();
                for (int i = 0; i <= abForCount; i++)//这里是一个死循环，需要手动点停止才结束
                {
                    if (!isInventory)
                        return;

                    //a-b 状态互换
                    if (i != 0)
                    {
                        modulerdr.AsyncStopReading();
                        modulerdr.ParamSet("Gen2Target", nextTarget);
                        iartest = (IAsyncResult)Invoke(new Handletest(testhandle), new object[] { 4, new List<object>() { $"配置模块参数: Gen2Target-{nextTarget} \n\r" } });
                        nextTarget = nextTarget == Target.A ? Target.B : Target.A;
                    }
                    for (int j = 0; j < rfModelForCount; j++)
                    {
                        if (!isInventory)
                            return;

                        //配置 rfmodel
                        var rfmodel = rfModellist[j];

                        //判断是否需要修改 gen2tagEncoding
                        if (currentRfMode != rfmodel.rfModel)
                        {
                            currentRfMode = rfmodel.rfModel;
                            modulerdr.ParamSet("gen2tagEncoding", getGen2val(rfmodel.rfModel));
                            iartest = (IAsyncResult)Invoke(new Handletest(testhandle), new object[] { 4, new List<object>() { $"配置模块参数: gen2tagEncoding-{getGen2val(rfmodel.rfModel)}" } });
                        }

                        //判断是否需要修改 Gen2Qvalue
                        if (currentQval != (rfmodel.qval - 1))
                        {
                            currentQval = rfmodel.qval - 1;
                            modulerdr.ParamSet("Gen2Qvalue", rfmodel.qval - 1);
                            iartest = (IAsyncResult)Invoke(new Handletest(testhandle), new object[] { 4, new List<object>() { $"配置模块参数: Gen2Qvalue-{rfmodel.qval - 1}" } });
                        }

                        //判断是否需要修改 HopAntTime
                        if (currentAntTime != rfmodel.time)
                        {
                            currentAntTime = rfmodel.time;
                            modulerdr.ParamSet("HopAntTime", rfmodel.time);
                            iartest = (IAsyncResult)Invoke(new Handletest(testhandle), new object[] { 4, new List<object>() { $"配置模块参数: HopAntTime-{rfmodel.time}" } });
                        }

                        antForCount = rfmodel.count;
                        modulerdr.AsyncStartReading(0x20 | (0x0004 << 8));

                        while (isInventoryV2 && isInventory)
                        {
                            try
                            {
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

                                        if (roundcount < antForCount)
                                        {
                                            roundcount++;
                                            break;
                                        }
                                        roundcount = 0;
                                        if (isInventoryV2)
                                        {
                                            modulerdr.AsyncStopReading();
                                            isInventoryV2 = false;
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }
                                }

                                if(stop_Time>0&&(Environment.TickCount - tickCount - stop_Time)>0)
                                    iartest = BeginInvoke(new Handletest(testhandle), new object[] { 3, new List<object>() { 3 } });


                                if (ttrd.Count > 0)
                                {
                                    List<TagReadData> ltemp = new List<TagReadData>();
                                    ltemp.AddRange(ttrd);
                                    iartest = BeginInvoke(new Handletest(testhandle), new object[] { 5, new List<object>() { ltemp } });
                                }
                            }
                            catch (Exception ex) {
                                Console.WriteLine("错误"+ex.Message);
                            }
                            finally
                            {
                                Thread.Sleep(25);
                            }
                        }

                        if (!isInventoryV2)
                        {
                            //iartest = (IAsyncResult)Invoke(new Handletest(testhandle), new object[] { 4, new List<object>() { $"结束第{(j + 1)}轮rfModel \n" } });
                            if (abForCount == 1)
                                j = (j == (rfModelForCount - 1)) ? -1 : j;//如果是最后一轮rfModel 就重置(-1通过 j++ 会回到0)

                            isInventoryV2 = true;
                            continue;
                        }
                        Thread.Sleep(25);
                    }
                    i = abForCount == 1 ? -1 : 1;//0表示一直是A或者B不翻转，1表示来回反转(-1通过 i++ 会回到0)
                }
            }
            catch (Exception)
            {

                Console.WriteLine("错误");
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        public void ControlEnabled(string startsWith,bool enabled)
        {
            foreach (Control ctl in groupBox1.Controls)
            {
                if (!ctl.Name.StartsWith(startsWith))
                    continue;

                ctl.Enabled = enabled;
            }
        }

        public void FrequencyHopTable() {
            try
            {
                int st = Environment.TickCount;
                uint[] htb2 = (uint[])modulerdr.ParamGet("FrequencyHopTable");

                htb = htb2.ToList();
                int cnt = 0;
                uint curchal = htb2[htb2.Length - 1];
                lvhoptb.Items.Clear();
                foreach (uint fre in htb2)
                {
                    cnt++;
                    ListViewItem item = new ListViewItem(fre.ToString());
                    if (rParms.readertype == ReaderType.PR_ONEANT)
                    {
                        if (cnt == htb2.Length)
                            break;
                        if (curchal == fre)
                            item.Checked = true;

                    }
                    else
                        item.Checked = true;

                    lvhoptb.Items.Add(item);
                }
                this.allcheckBox.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取失败");
            }
        }

        public int SetFilterSessioninTargetA(int[] ants, uint fre, int power)
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
            catch (Exception ex)
            {
                MessageBox.Show("出现盘点错误,硬件不支持该功能!");
                rtbopfailmsg.Text += ex;
                return -1;
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

        /// <summary>
        /// 检查天线是否可用
        /// </summary>
        /// <returns></returns>
        private List<int> CheckAntsValid()
        {
            List<int> selants = new List<int>();

            for (int i = 0; i < groupBox2.Controls.Count; i++)
            {
                if (!groupBox2.Controls[i].Name.StartsWith("Ant"))
                    continue;

                var checkBox = (CheckBox)groupBox2.Controls[i];
                if (checkBox.Checked)
                {
                    var antId=checkBox.Name.Replace("Ant", "");
                    selants.Add(Convert.ToInt32(antId));
                }
            }
            return selants;
        }

        delegate void Handletest(int type, List<object> lobj);
        private void testhandle(int type, List<object> lobj)
        {
            Console.WriteLine("testhandle开始");
            try
            {
                switch (type)
                {
                    case 5:
                        List<TagReadData> ltd = (List<TagReadData>)(lobj[0]);

                        for (int i = 0; i < ltd.Count; i++)
                        {
                            if (!m_Tags.ContainsKey(ltd[i].EPCString))
                            {
                                m_Tags.Add(ltd[i].EPCString, ltd[i]);

                                var ltdTime = ltd[0].Time;
                                ListViewItem lvi = new ListViewItem(m_Tags.Count.ToString());
                                lvi.SubItems.Add(ltd[i].ReadCount.ToString());
                                lvi.SubItems.Add(ltd[i].EPCString);
                                lvi.SubItems.Add(ltd[i].Antenna.ToString());
                                alist.Add(lvi);
                            }
                        }

                        myCache = alist.ToArray();
                        lvTags.VirtualListSize = alist.Count;
                        label_tags.Text = alist.Count.ToString();
                        if (stop_Count > 0 && alist.Count >= stop_Count)
                            button4.PerformClick();

                        break;
                    case 4:
                        rtbopfailmsg.Text += lobj[0].ToString();
                        rtbopfailmsg.Text += "\n";
                        break;
                    case 3:
                        button4.PerformClick();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("testhandle错误");
            }
            finally {
                Console.WriteLine("testhandle退出");
            }
        }

        public int getGen2val(string name)
        {
            switch (name)
            {
                case "FM0":
                    return 0;
                case "M2":
                    return 1;
                case "M4":
                    return 2;
                case "M8":
                    return 3;
                case "PROFILE 0":
                    return 16;
                case "PROFILE 1":
                    return 17;
                case "PROFILE 2":
                    return 18;
                case "PROFILE 3":
                    return 19;
                case "PROFILE 4":
                    return 20;
                case "PROFILE 5":
                    return 21;
                case "RF_MODE_1":
                    return 101;
                case "RF_MODE_3":
                    return 103;
                case "RF_MODE_5":
                    return 105;
                case "RF_MODE_7":
                    return 107;
                case "RF_MODE_11":
                    return 111;
                case "RF_MODE_12":
                    return 112;
                case "RF_MODE_13":
                    return 113;
                case "RF_MODE_15":
                    return 115;
            }
            return 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labreadtime.Text = (Environment.TickCount - tickCount).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            power = Convert.ToInt32(Power.Text);
            ants = CheckAntsValid();
            AntPower[] apwrs = new AntPower[ants.Count];
            for (int v = 0; v < ants.Count; ++v)
            {
                apwrs[v].AntId = (byte)(ants[v]);
                apwrs[v].ReadPower = (ushort)(power);
                apwrs[v].WritePower = (ushort)(power);
            }
       
            modulerdr.ParamSet("AntPowerConf", apwrs);
            modulerdr.ParamSet("ReadPlan", new SimpleReadPlan(ants.ToArray()));//设置天线
        }

        private void btnsethtb_Click(object sender, EventArgs e)
        {
            htb.Clear();
            foreach (ListViewItem item in lvhoptb.Items)
            {
                if (item.Checked)
                    htb.Add(uint.Parse(item.SubItems[0].Text));
            }

            if (htb.Count <= 0)
            {
                MessageBox.Show("请至少设置一个盘点");
                return;
            }
            if (rParms.readertype == ReaderType.PR_ONEANT && htb.Count != 1)
            {
                MessageBox.Show("只能设置一个信道");
                return;
            }

            //将频点保存至模块
            modulerdr.ParamSet("FrequencyHopTable", htb.ToArray());
            //modulerdr.ParamSet("ModuleSave_Frenqency", htb.ToArray());
        }

        private void btnsetrg_Click(object sender, EventArgs e)
        {
            ModuleTech.Region rg = ModuleTech.Region.UNSPEC;
            bool is840_845 = false;
            bool is840_925 = false; ;
            if (this.cbbregion.SelectedIndex == -1)
            {
                MessageBox.Show("请选择区域");
                return;
            }
            switch (this.cbbregion.SelectedIndex)
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
                        if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100 ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1200 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1300 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5100 ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5200 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                        {
                            if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC ||
                                modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100 ||
                                 modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1200 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1300 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5100 ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5200 ||
                        modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                                rg = ModuleTech.Region.PRC2;
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
                        if (modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC ||
                            modulerdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100)
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
                case 10:
                    rg = ModuleTech.Region.EU3;

                    break;
                case 11:
                    rg = ModuleTech.Region.CE_HIGH;
                    break;
                case 12:
                    rg = ModuleTech.Region.HK;
                    break;
                case 13:
                    rg = ModuleTech.Region.TAIWAN;
                    break;
                case 14:
                    rg = ModuleTech.Region.MALAYSIA;
                    break;
                case 15:
                    rg = ModuleTech.Region.SOUTH_AFRICA;
                    break;
                case 16:
                    rg = ModuleTech.Region.BRAZIL;
                    break;
                case 17:
                    rg = ModuleTech.Region.THAILAND;
                    break;
                case 18:
                    rg = ModuleTech.Region.SINGAPORE;
                    break;
                case 19:
                    rg = ModuleTech.Region.AUSTRALIA;
                    break;
                case 20:
                    rg = ModuleTech.Region.URUGUAY;
                    break;
                case 21:
                    rg = ModuleTech.Region.VIETNAM;
                    break;
                case 22:
                    rg = ModuleTech.Region.ISRAEL;
                    break;
                case 23:
                    rg = ModuleTech.Region.PHILIPPINES;
                    break;
                case 24:
                    rg = ModuleTech.Region.INDONESIA;
                    break;
                case 25:
                    rg = ModuleTech.Region.NEW_ZEALAND;
                    break;
                case 26:
                    rg = ModuleTech.Region.PERU;
                    break;
                case 27:
                    rg = ModuleTech.Region.RUSSIA;
                    break;

            }

            if (rParms.readertype == ReaderType.PR_ONEANT)
            {
                if (rg == ModuleTech.Region.OPEN || rg == ModuleTech.Region.CN)
                {
                    MessageBox.Show("不支持的区域");
                    return;
                }
            }
            try
            {
                modulerdr.ParamSet("Region", rg);
                rParms.isSpecfres = false;
                if (this.cbbregion.SelectedIndex == 10)
                {
                    List<uint> htab = new List<uint>();
                    htab.Add(867700);
                    modulerdr.ParamSet("FrequencyHopTable", htab.ToArray());
                    rParms.isSpecfres = true;
                }

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
                    modulerdr.ParamSet("FrequencyHopTable", htab.ToArray());
                    rParms.isSpecfres = true;
                }
                else
                {

                }
            }
            catch (Exception)
            {
                MessageBox.Show("设置失败 ");
            }
        }

        private void btngetrg_Click(object sender, EventArgs e)
        {
            try
            {
                int st = Environment.TickCount;
                ModuleTech.Region rg = (ModuleTech.Region)modulerdr.ParamGet("Region");
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
                    case ModuleTech.Region.CE_HIGH:
                        this.cbbregion.SelectedIndex = 11;
                        break;
                    case ModuleTech.Region.HK:
                        this.cbbregion.SelectedIndex = 12;
                        break;
                    case ModuleTech.Region.TAIWAN:
                        this.cbbregion.SelectedIndex = 13;
                        break;
                    case ModuleTech.Region.MALAYSIA:
                        this.cbbregion.SelectedIndex = 14;
                        break;
                    case ModuleTech.Region.SOUTH_AFRICA:
                        this.cbbregion.SelectedIndex = 15;
                        break;
                    case ModuleTech.Region.BRAZIL:
                        this.cbbregion.SelectedIndex = 16;
                        break;
                    case ModuleTech.Region.THAILAND:
                        this.cbbregion.SelectedIndex = 17;
                        break;
                    case ModuleTech.Region.SINGAPORE:
                        this.cbbregion.SelectedIndex = 18;
                        break;
                    case ModuleTech.Region.AUSTRALIA:
                        this.cbbregion.SelectedIndex = 19;
                        break;
                    case ModuleTech.Region.URUGUAY:
                        this.cbbregion.SelectedIndex = 20;
                        break;
                    case ModuleTech.Region.VIETNAM:
                        this.cbbregion.SelectedIndex = 21;
                        break;
                    case ModuleTech.Region.ISRAEL:
                        this.cbbregion.SelectedIndex = 22;
                        break;
                    case ModuleTech.Region.PHILIPPINES:
                        this.cbbregion.SelectedIndex = 23;
                        break;
                    case ModuleTech.Region.INDONESIA:
                        this.cbbregion.SelectedIndex = 24;
                        break;
                    case ModuleTech.Region.NEW_ZEALAND:
                        this.cbbregion.SelectedIndex = 25;
                        break;
                    case ModuleTech.Region.PERU:
                        this.cbbregion.SelectedIndex = 26;
                        break;
                    case ModuleTech.Region.RUSSIA:
                        this.cbbregion.SelectedIndex = 27;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取失败");
            }
        }

        private void btngethtb_Click(object sender, EventArgs e)
        {
            try
            {
                int st = Environment.TickCount;
                uint[] _htb = (uint[])modulerdr.ParamGet("FrequencyHopTable");
                htb = _htb.ToList();

                if (_htb != null)
                    Sort(ref _htb);
               
                int cnt = 0;
                uint curchal = _htb[_htb.Length - 1];
                lvhoptb.Items.Clear();
                foreach (uint fre in _htb)
                {
                    cnt++;
                    ListViewItem item = new ListViewItem(fre.ToString());
                    if (rParms.readertype == ReaderType.PR_ONEANT)
                    {
                        if (cnt == _htb.Length)
                            break;
                        if (curchal == fre)
                            item.Checked = true;

                    }
                    else
                        item.Checked = true;

                    lvhoptb.Items.Add(item);
                }
                this.allcheckBox.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取失败");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            JObject jo = new JObject();
            JArray ja = new JArray();
            jo.Add(new JProperty("Gen2Target", Gen2Target.SelectedIndex));
            jo.Add(new JProperty("S2_AInstruct", s2_a.Checked));

            var _rfModelForCount = 0;
            //获取RFmode循环次数
            foreach (Control ctl in groupBox1.Controls)
            {
                if (!ctl.Name.StartsWith("checkBox"))
                    continue;

                if (((CheckBox)ctl).Checked)
                    _rfModelForCount++;
            }

            //获取 rfmodel列表的配置信息
            for (int i = 0; i < _rfModelForCount; i++)
            {
                JObject _jo = new JObject();

                ComboBox rfmode = this.Controls.Find("RFmode" + (i + 1) + "_rfmode", true)[0] as ComboBox;
                ComboBox qval = this.Controls.Find("RFmode" + (i + 1) + "_qval", true)[0] as ComboBox;
                TextBox time = this.Controls.Find("RFmode" + (i + 1) + "_time", true)[0] as TextBox;
                TextBox count = this.Controls.Find("RFmode" + (i + 1) + "_count", true)[0] as TextBox;

                _jo.Add(new JProperty("rfModel", rfmode.Text));
                _jo.Add(new JProperty("qval", qval.SelectedIndex));
                _jo.Add(new JProperty("time", Convert.ToInt32(time.Text)));
                _jo.Add(new JProperty("count", Convert.ToInt32(count.Text)));
                ja.Add(_jo);
            }
            jo.Add(new JProperty("list", ja));

            string filename = null;
            SaveFileDialog sfd = new SaveFileDialog();
            
            sfd.Filter = "Json(*.Json)| *.Json";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName;
                FileInfo fileInfo = new FileInfo(filename);
                fileInfo.Delete();
                StreamWriter streamWriter = fileInfo.CreateText();
                streamWriter.WriteLine(jo);

                streamWriter.Flush();
                streamWriter.Close();
            }

        }
    }

    public class models {

        public class Rfmodels {
            public string rfModel { get; set; }
            public int qval { get; set; }
            public int count { get; set; }
            public int time { get; set; }
        }

        public class boxAllTag
        {
            public int Gen2Target { get; set; }
            public bool S2_AInstruct { get; set; }
            public List<Rfmodels> list { get; set; }
        }

    }
}
