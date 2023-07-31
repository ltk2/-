using ModuleTech;
using ModuleTech.Gen2;
using Newtonsoft.Json;
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
//using System.Windows.Forms.DataVisualization.Charting;
using static ModuleReaderManager.models;
using static System.Windows.Forms.ListView;

namespace ModuleReaderManager
{
    public partial class Form_boxAllTagsV2 : Form
    {
        public Reader modulerdr;
        public ReaderParams rParms;
        Mutex mutex = new Mutex();
        Thread readThread = null;
        Thread forCountThread = null;

        ColumnHeader[] ColumnHeader;

        //配置文件 
        public List<System.IO.FileInfo> fileInfo = new List<System.IO.FileInfo>();
        public boxAllTag boxAllTag;

        int roundcount = 0;
        int antForCount = 0;// 天线循环次数
        int abForCount = 0;//ab 循环次数
        int rfModelForCount = 0;//rfmodel 循环次数

        bool isForCount = false;
        bool isInventory = false;
        bool isInventoryV2 = false;//内部循环的控制
        string currentRfMode = "";//当前rfModel
        int currentQval = 0;//当前Qval
        int currentAntTime = 0;//当前天线停留时间
        List<Rfmodels> rfModellist = null;
        List<uint> htb = new List<uint>();//频点
        List<int> ants = new List<int>();//天线

        int stop_Time = 0;//读取时间
        int stop_Count = 0;//读取个数
        int Stop_ForCount = 0;//盘点次数
        int execute_Count = 0;//已经盘点次数
        int execute_CountV2 = 0;//已经盘点次数
        string fullName = "";

        public static DateTime startTime;

        ModuleTech.Region rg;
        
        List<StopModel> stop_Models = new List<StopModel>();

        int power = 0;
        Dictionary<string, TagReadData> m_Tags = new Dictionary<string, TagReadData>();
        public List<ListViewItem> alist = new List<ListViewItem>();
        Target nextTarget;
        int tickCount = 0;
        IAsyncResult iartest;
        

        private ListViewItem[] myCache; //array to cache items for the virtual list
        private int firstItem; //stores the index of the first item in the cache
        public Form_boxAllTagsV2(Reader _modulerdr, ReaderParams _rParms)
        {
            InitializeComponent();
            modulerdr = _modulerdr;
            rParms = _rParms;
            ColumnHeader =(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader6});

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
            //string fp = System.Windows.Forms.Application.StartupPath+ @"\Json";
            string fp = System.Windows.Forms.Application.StartupPath+ @"\json";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(fp);
           var _fileInfo = GetAllFileInfo(dir);
            for (int i = 0; i < _fileInfo.Count; i++)
            {
                var ss = _fileInfo[i].Extension;
                if (_fileInfo[i].Extension != ".Json" && _fileInfo[i].Extension != ".json")
                    continue;

                var type = "115200";
                if (modulerdr.Address.ToLower().Contains("com")&& modulerdr.Address.ToLower().Contains(":"))
                    type=modulerdr.Address.Split(':')[1];

                if (_fileInfo[i].Name.Contains(type)) { 
                    comboBox1.Items.Add(_fileInfo[i].Name);
                    fileInfo.Add(_fileInfo[i]);
                }
            }
            if (comboBox1.Items.Count > 0) { 
                comboBox1.SelectedIndex = 0;
                //comboBox1.Items.Add("测试所有配置方案");
                comboBox1.Items.Add("");
            }

            var powermax = modulerdr.ParamGet("RfPowerMax");
            Power.Text = powermax.ToString();

            this.WindowState = FormWindowState.Maximized;
            FrequencyHopTable();

            //区域
            try
            {
                int st = Environment.TickCount;
                rg = (ModuleTech.Region)modulerdr.ParamGet("Region");
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
            for (int c = 0; c < connectedants.Length; ++c)
            {
                var groupBox2_ant = (CheckBox)groupBox2.Controls["Ant" + connectedants[c]];
                groupBox2_ant.ForeColor = Color.Green;
                groupBox2_ant.Checked = true;
            }
        }

        public List<System.IO.FileInfo> GetAllFileInfo(System.IO.DirectoryInfo dir)
        {
            List<System.IO.FileInfo> FileList = new List<System.IO.FileInfo>();
            System.IO.FileInfo[] allFile = dir.GetFiles();
            foreach (System.IO.FileInfo file in allFile)
            {
                if (file.Name.IndexOf(".Json") != -1|| file.Name.IndexOf(".json")!= -1)
                    FileList.Add(file);
            }
            return FileList;
        }

        //业务上停止盘点，等待重新开始
        public void StopReading() {
            
            rfModelForCount = 0;
            abForCount = 0;
            isInventory = false;
            timer1.Stop();
            mutex.WaitOne();
            if (iartest != null)
                EndInvoke(iartest);

            if (readThread != null)
            {
                readThread.Join();
                readThread = null;
            }
            if (forCountThread != null)
            {
                forCountThread.Join();
                forCountThread = null;
            }

            modulerdr.AsyncStopReading();
            int execute = 0;
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
                execute = execute_CountV2==0? comboBox1.Items.Count - 2:execute_CountV2 -1;
            else 
                execute = comboBox1.SelectedIndex;

            StopModel stopModel = new StopModel();
            stopModel.jsonName = comboBox1.Items[execute].ToString();
            stopModel.timeConsuming = Convert.ToInt32(labreadtime.Text);
            stopModel.labelQuantity = Convert.ToInt32(label_tags.Text);
            stop_Models.Add(stopModel);

            var _Stop_ForCount = int.Parse(stopForCount.Text);
            if (_Stop_ForCount <= 0)
                _Stop_ForCount = 1;
            var _label_testcount = int.Parse(label_testcount.Text); 
            if (forCountThread == null && _Stop_ForCount > 0&& _Stop_ForCount > _label_testcount)
            {
                isForCount = true;
                forCountThread = new Thread(new ParameterizedThreadStart(ForCountFunc));
                forCountThread.Start(Stop_ForCount);
            }
            mutex.ReleaseMutex();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (isInventory)
                {
                    if (label_testcount.Text == "0")
                        label_testcount.Text = "1";

                    StopReading();
                    button4.Text = "开始盘点";

                    mutex.WaitOne();
                    isForCount = false;
                    if (forCountThread != null)
                    {
                        forCountThread.Join();
                        forCountThread = null;
                    }
                    mutex.ReleaseMutex();
                    if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
                    {
                        //Series series1 = new Series("时间");
                        //series1.ChartType = SeriesChartType.RangeBar;
                        //series1.IsValueShownAsLabel = true;//设置显示示数

                        //Series series2 = new Series("标签");
                        //series2.ChartType = SeriesChartType.RangeBar;

                        for (int i = 0; i < comboBox1.Items.Count - 1; i++)
                        {
                            var jsonName = comboBox1.Items[i].ToString();
                            //var averageLabelQuantity = stop_Models.Where(r => r.jsonName == jsonName).Average(r => r.labelQuantity);
                            var averageTimeConsuming = stop_Models.Where(r => r.jsonName == jsonName).Average(r => r.timeConsuming);
                            //series1.Points.Add(averageTimeConsuming);
                            //series2.Points.Add(averageLabelQuantity);
                        }
                        //chart1.Series.Add(series1);
                       
                        if(fullName.IndexOf("921600")!=-1)
                            chart1.ChartAreas[0].Axes[0].LabelStyle.Format = "921600 (#).json"; //设置X轴显示样式
                        else
                            chart1.ChartAreas[0].Axes[0].LabelStyle.Format = "115200 (#).json"; //设置X轴显示样式
                        //chart1.Series.Add(series2);
                    }
                    else {
                        rtbopfailmsg.Text += "\n\n";
                        for (int i = 0; i < stop_Models.Count; i++)
                        {
                            rtbopfailmsg.Text += $"第{i + 1}次盘点耗时:{stop_Models[i].timeConsuming}(ms),读取标签个数{stop_Models[i].labelQuantity}个 \n";
                            var s = stop_Models.Where(r => r.labelQuantity < Convert.ToInt32(stopCount.Text)).Count();
                            rtbopfailmsg.Text += $"盘点成功率: {GetPercent((stop_Models.Count - s), stop_Models.Count)} \n";
                            rtbopfailmsg.Text += "\n\n";
                        }

                        rtbopfailmsg.Text += $"平均每次盘点耗时:{stop_Models.Average(r => r.timeConsuming)}(ms)";
                        //平均每次盘点耗时:{stop_Models.Average(r => r.timeConsuming)}(ms),
                    }
                    Thread.Sleep(2000);
                    label_testcount.Text = "0";
                }
                else
                {
                    if (comboBox1.Text == "") {
                        MessageBox.Show("请选择一个配置文件");
                        return;
                    }
                    modulerdr.AsyncStopReading();
                    //label_testcount.Text = "0";
                    stop_Time = int.Parse(stopTime.Text)* 1000;
                    stop_Count = int.Parse(stopCount.Text);
                    Stop_ForCount = int.Parse(stopForCount.Text);
                    if (Stop_ForCount <= 0)
                        Stop_ForCount = 1;

                    if (comboBox1.SelectedIndex == comboBox1.Items.Count-1)
                    {
                        if (stopTime.Text == "0" || stopTime.Text == "")
                        {
                            MessageBox.Show("请输入指定时间");
                            return;
                        }

                        if (stopTime.Text == "0" || stopTime.Text == "")
                        {
                            MessageBox.Show("请输入指定时间");
                            return;
                        }
                        
                        fullName = fileInfo[execute_CountV2].FullName;
                    }
                    else {
                        fullName = fileInfo.Where(r => r.Name == comboBox1.Text).FirstOrDefault().FullName;
                    }
                    Console.WriteLine("当前文件:"+fullName);
                    boxAllTag boxAllTag = JsonConvert.DeserializeObject<boxAllTag>(File.ReadAllText(fullName));

                    if (ants == null || ants.Count <= 0)
                    {
                        button1.PerformClick();
                    }
                    rfModelForCount = boxAllTag.list.Count;

                    bool isR2000 = modulerdr.HwDetails.module.ToString().IndexOf("SLR") != -1;//是否是 R2000
                    //R2000模块 非FCC频段， 发送s2-a 固定发 902750 915250 927250频点
                    if (boxAllTag.S2_AInstruct && isR2000&& rg!= ModuleTech.Region.NA) {
                        //低频点
                        if (SetFilterSessioninTargetA(ants.ToArray(), (uint)902750, Convert.ToInt32(power)) == -1)
                            return;
                        //中频点
                        if (SetFilterSessioninTargetA(ants.ToArray(), (uint)915250, Convert.ToInt32(power)) == -1)
                            return;
                        //高频点
                        if (SetFilterSessioninTargetA(ants.ToArray(), (uint)927250, Convert.ToInt32(power)) == -1)
                            return;
                    }
                    else if (boxAllTag.S2_AInstruct)
                    {
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
                        else
                        {
                            //低频点
                            if (SetFilterSessioninTargetA(ants.ToArray(), htb[0], Convert.ToInt32(power)) == -1)
                                return;
                            //中频点
                            if (SetFilterSessioninTargetA(ants.ToArray(), htb[htb.Count / 2], Convert.ToInt32(power)) == -1)
                                return;
                            //高频点
                            if (SetFilterSessioninTargetA(ants.ToArray(), htb[htb.Count - 1], Convert.ToInt32(power)) == -1)
                                return;
                        }
                    }

                    modulerdr.ParamSet("Gen2Session", ModuleTech.Gen2.Session.Session2);//S2
                    modulerdr.ParamSet("Ex10FastModeParams", null);

                    abForCount = boxAllTag.Gen2Target <= 1 ? 1 : 2;

                    //获取 rfmodel列表的配置信息
                    rfModellist = new List<Rfmodels>();
                    for (int i = 0; i < boxAllTag.list.Count; i++)
                    {
                        Rfmodels rfmodel = new Rfmodels();
                        rfmodel.rfModel = boxAllTag.list[i].rfModel;
                        rfmodel.qval = boxAllTag.list[i].qval;
                        rfmodel.time = boxAllTag.list[i].time;
                        rfmodel.count = boxAllTag.list[i].count;
                        rfModellist.Add(rfmodel);
                    }

                    //如果 Stop_ForCount == execute_Count 说明是新的一轮盘点而非循环
                    if (Stop_ForCount == execute_Count|| Stop_ForCount<=0) {
                        execute_Count = 0;

                        rtbopfailmsg.Text = "";
                        label_testcount.Text = "0";
                        stop_Models = new List<StopModel>();
                    }
                    
                    //label_testcount.Text = execute_Count.ToString();
                    
                    if (boxAllTag.Gen2Target < 2)
                    {
                        modulerdr.ParamSet("Gen2Target", (ModuleTech.Gen2.Target)boxAllTag.Gen2Target);
                        Console.WriteLine($"1配置模块参数: Gen2Target-{(ModuleTech.Gen2.Target)boxAllTag.Gen2Target}");
                        nextTarget = (ModuleTech.Gen2.Target)boxAllTag.Gen2Target;
                    }
                    else if (boxAllTag.Gen2Target == 2)
                    {
                        modulerdr.ParamSet("Gen2Target", ModuleTech.Gen2.Target.A);
                        Console.WriteLine($"2配置模块参数: Gen2Target-A \n");
                        nextTarget = ModuleTech.Gen2.Target.B;
                    }
                    else if (boxAllTag.Gen2Target == 3)
                    {
                        modulerdr.ParamSet("Gen2Target", ModuleTech.Gen2.Target.B);
                        Console.WriteLine($"3配置模块参数: Gen2Target-B \n");
                        nextTarget = ModuleTech.Gen2.Target.A;
                    }

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
                    if (readThread == null) {
                        readThread = new Thread(ReadFunc);
                        readThread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
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

        void ForCountFunc(object ForCoun) {
            int i = 0;
            while (isForCount)
            {
                if (isInventory) {
                    Thread.Sleep(2000);
                    continue;
                }
                iartest = (IAsyncResult)Invoke(new Handletest(testhandle), new object[] { 4, new List<object>() { "" } });
                isForCount = false;
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
                        Console.WriteLine($"4配置模块参数: Gen2Target-{nextTarget} \n\r");
                        //iartest = (IAsyncResult)Invoke(new Handletest(testhandle), new object[] { 4, new List<object>() { $"配置模块参数: Gen2Target-{nextTarget} \n\r" } });
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
                            Console.WriteLine($"5配置模块参数: gen2tagEncoding-{getGen2val(rfmodel.rfModel)}");
                        }

                        //判断是否需要修改 Gen2Qvalue
                        if (currentQval != (rfmodel.qval - 1))
                        {
                            currentQval = rfmodel.qval - 1;
                            modulerdr.ParamSet("Gen2Qvalue", rfmodel.qval - 1);
                            Console.WriteLine($"6配置模块参数: Gen2Qvalue-{rfmodel.qval - 1}");
                        }

                        //判断是否需要修改 HopAntTime
                        if (currentAntTime != rfmodel.time)
                        {
                            currentAntTime = rfmodel.time;
                            modulerdr.ParamSet("HopAntTime", rfmodel.time);
                            Console.WriteLine($"7配置模块参数: HopAntTime-{rfmodel.time}");
                        }

                        antForCount = rfmodel.count;
                        startTime = DateTime.Now;
                        if (rParms.FRTMeta.IsEmdData)
                            modulerdr.AsyncStartReading(0x20 | (0x0004 << 8) | (0x0080 << 8));
                        else
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

                                if (stop_Time > 0 && (Environment.TickCount - tickCount - stop_Time) > 0) {
                                    //stop_Time = 0;
                                    iartest = BeginInvoke(new Handletest(testhandle), new object[] { 3, new List<object>() { 3 } });
                                }
                                if (ttrd.Count > 0)
                                {
                                    List<TagReadData> ltemp = new List<TagReadData>();
                                    ltemp.AddRange(ttrd);
                                    iartest = BeginInvoke(new Handletest(testhandle), new object[] { 5, new List<object>() { ltemp } });
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("错误" + ex.Message);
                            }
                            finally
                            {
                                Thread.Sleep(25);
                            }
                        }

                        if (!isInventoryV2)
                        {
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
            catch (Exception ex)
            {
                Console.WriteLine("错误"+ ex);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        public void FrequencyHopTable()
        {
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
                    var antId = checkBox.Name.Replace("Ant", "");
                    selants.Add(Convert.ToInt32(antId));
                }
            }
            return selants;
        }

        delegate void Handletest(int type, List<object> lobj);
        bool isTesthandle = false;
        private void testhandle(int type, List<object> lobj)
        {
            if (isTesthandle)
                return;

            isTesthandle = true;
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
                                TimeSpan tss = ltdTime.Subtract(startTime);
                                ListViewItem lvi = new ListViewItem(m_Tags.Count.ToString());
                                lvi.SubItems.Add(ltd[i].ReadCount.ToString());
                                lvi.SubItems.Add(ltd[i].EPCString);
                                lvi.SubItems.Add(ltd[i].Antenna.ToString());
                                lvi.SubItems.Add(ltd[i].EMDDataString.ToString());
                                //lvi.SubItems.Add("");
                                //lvi.SubItems.Add(tss.Hours + "时" + tss.Minutes + "分" + tss.Seconds + "秒" + tss.Milliseconds + "毫秒");
                                alist.Add(lvi);
                            }
                            else {
                                var ss=alist.Where(r => r.SubItems[2].Text == ltd[i].EPCString).Single();
                                ss.SubItems[1].Text =(Convert.ToInt32(ss.SubItems[1].Text)+ ltd[i].ReadCount).ToString();
                                ss.SubItems[2].Text = ltd[i].EPCString;
                                ss.SubItems[3].Text = ltd[i].Antenna.ToString();

                                if(ltd[i].EMDDataString!=""&& ss.SubItems[4].Text!= ltd[i].EMDDataString)
                                    ss.SubItems[4].Text = ltd[i].EMDDataString.ToString();
                            }
                        }
                        
                        myCache = alist.ToArray();
                        if (alist.Count() <= 400)
                            lvTags.Invalidate();

                        lvTags.VirtualListSize = alist.Count;
                        label_tags.Text = alist.Count.ToString();
                       
                        if (stop_Count > 0 && alist.Count >= stop_Count) {
                            isInventoryV2 = false;
                            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
                            {
                                var _fullName = comboBox1.Items[comboBox1.Items.Count - 2].ToString();
                                if (fullName.IndexOf(_fullName) != -1)
                                {
                                    execute_Count++;
                                    execute_CountV2 = 0;
                                }
                                else {
                                    execute_CountV2++;
                                }
                            }
                            else {
                                execute_Count++;
                            }
                            
                            label_testcount.Text = execute_Count.ToString();
                            startTime = DateTime.Now;
                            if (Stop_ForCount >0 && Stop_ForCount == execute_Count)
                                button4.PerformClick();
                            else
                                StopReading();//停止盘点
                        }

                        break;
                    case 4:
                        button4.PerformClick();
                        Console.WriteLine("重新开始");
                        break;
                    case 3:
                        isInventoryV2 = false;

                        if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
                        {
                            var _fullName = comboBox1.Items[comboBox1.Items.Count - 2].ToString();
                            if (fullName.IndexOf(_fullName) != -1)
                            {
                                execute_Count++;
                                execute_CountV2 = 0;
                            }
                            else {
                                execute_CountV2++;
                            }
                        }
                        else
                        {
                            execute_Count++;
                        }
                        label_testcount.Text = execute_Count.ToString();
                        startTime = DateTime.Now;
                        if (Stop_ForCount >0&& Stop_ForCount == execute_Count)
                            button4.PerformClick();
                        else
                            StopReading();//停止盘点

                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("testhandle错误"+ ex);
            }
            finally
            {
                Console.WriteLine("testhandle退出");
                isTesthandle = false;
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

        public static string GetPercent(double value, double total)
        {
            if (total == 0)
            {
                return "0%";
            }
            var ss = Math.Round(value / total, 5);
            return Math.Round(value / total, 5).ToString("P");
        }

        private void rtbopfailmsg_TextChanged(object sender, EventArgs e)
        {
            rtbopfailmsg.Focus();
            rtbopfailmsg.Select(rtbopfailmsg.Text.Length, 0);
            rtbopfailmsg.ScrollToCaret();
        }
        bool taglvdicSorting = true;
        private void lvTags_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (taglvdicSorting)
            {
                //var ss = m_Tags.Values.Where(r => r.EMDDataString == "").ToList();
                alist = alist.OrderBy(r => r.SubItems[e.Column].Text).ToList();
                taglvdicSorting = !taglvdicSorting;
            }
            else
            {
                alist = alist.OrderByDescending(r => r.SubItems[e.Column].Text).ToList();
                taglvdicSorting = !taglvdicSorting;
            }

            myCache = alist.ToArray();
            lvTags.VirtualListSize = alist.Count;
            lvTags.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboBox1.SelectedIndex == comboBox1.Items.Count-1)
            //{
            //    stopForCount.Text = (comboBox1.Items.Count - 1).ToString();
            //    lvTags.Visible = false;
            //    chart1.Visible = true;
            //    chart1.Height = 613;
            //    chart1.Width = 1113;
            //    chart1.Location = new Point(241, 68);
            //}
            //else {
            //    lvTags.Visible = true;
            //    chart1.Visible = false;
            //}
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            Common.DownListWidth(comboBox1);
        }
    }

    public class StopModel {
        public string jsonName { get; set; }
        public int timeConsuming { get; set; }
        public int labelQuantity { get; set; }
    }

}