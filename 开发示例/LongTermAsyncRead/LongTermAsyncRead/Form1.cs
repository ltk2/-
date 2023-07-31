using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ModuleTech;
using ModuleTech.Gen2;
using ModuleLibrary;
using System.Threading;
using System.Diagnostics;

namespace LongTermAsyncRead
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Reader rdr = null;
        ReaderExceptionChecker rechecker = new ReaderExceptionChecker(3, 60);

        delegate void UpdateTagsRead(TagReadData[] tags);
        delegate void UpdateUi(string tip);
        void OnResumeFailed(string tip)
        {
            this.btnstart.Enabled = true;
            this.btnstop.Enabled = false;            
            MessageBox.Show(tip);
        }

        void ShowTags(TagReadData[] tags)
        {            
            foreach (TagReadData tag in tags)
            {
                bool isnew = true;
                for (int j = 0; j < listBox1.Items.Count; ++j)
                {
                    string epcstr = listBox1.Items[j].ToString();
                    if (epcstr.StartsWith(tag.EPCString))
                    {
                        char[] sep = new char[1];
                        sep[0] = ' ';
                        string[] strs = epcstr.Split(sep, StringSplitOptions.None);
                        listBox1.Items[j] = tag.EPCString + " " + (int.Parse(strs[1]) + tag.ReadCount).ToString();
                        isnew = false;
                        break;
                    }
                }
                if (isnew)
                    this.listBox1.Items.Add(tag.EPCString+" 1");
            }
        }

        void OnTagsRead(object sender, Reader.TagsReadEventArgs tagsArgs)
        {
            Reader rdrtmp = (Reader)sender;
            #region 示例在盘存标签事件中做其它标签访问操作
            /*
            //可以对读到的标签做进一步的读，写，锁和GPIO等操作等操作
            //注：只有在非高速盘存模式下才可以进行其它的标签操作，高速盘存模式下禁
            //止再进行其它标签操作，否则盘存操作将被终止，其它的标签操作也不会成功。
            //但GPIO的操作是例外，不论哪种盘存模式都可以在此处操作GPIO
             * */
            /*
            //以下示例写入两个块的数据到盘存到的标签的USER区中
            foreach (TagReadData tag in tagsArgs.Tags)
            {
                ushort[] wdata = new ushort[2];
                wdata[0] = 0x1234;
                wdata[1] = 0x5678;
                Gen2TagFilter filter = new Gen2TagFilter(tag.EPC.Length * 8, tag.EPC, MemBank.EPC, 32, false);
                try
                {
                    //必须设置操作使用的天线，一般可设置为标签被盘存到时的天线
                    rdrtmp.ParamSet("TagopAntenna", tag.Antenna);
                    rdrtmp.WriteTagMemWords(filter, MemBank.USER, 0, wdata);
                }
                catch (System.Exception ex)
                {
                //操作有可能失败（很多原因可能造成操作失败，常见的比如标签速度过快，已经不在天线场内）
                }
            }
            */
            #endregion

            #region 示例在盘存标签事件中操作GPO
            /*
            //设置GPO1为1
            try
            {
                rdrtmp.GPOSet(1, true);
            }
            catch (System.Exception ex)
            {
                //操作有可能失败,比如由于断网
            }
            */
            #endregion

            this.BeginInvoke(new UpdateTagsRead(ShowTags), new object[] { tagsArgs.Tags });
        }
        //GPI触发事件的处理函数
        /*
        void GpiStateOnTrigger(object sender, Reader.GpiTriggerEventArgs gpiArgs)
        {
            Reader rdrtmp = (Reader)sender;
            Debug.WriteLine("reader " + rdrtmp.Address);
            GPIState[] gstates = gpiArgs.GpiStates;
            foreach (GPIState gs in gstates)
            {
                Debug.WriteLine("gpiid:" + gs.GpiId + ", state:" + gs.State);
            }
        }
        */
        void TryResumeReading(object sender, Reader.ReadExceptionEventArgs expArgs)
        {
            Reader rdrtmp = (Reader)sender;
            //如果需要可在此处记录异常日志
            Debug.WriteLine(rdrtmp.Address + "--异常信息:" + expArgs.ReaderException.ToString());
            //判断是否异常发生过于频繁
            if (rechecker.IsTrigger())
            {
                this.BeginInvoke(new UpdateUi(OnResumeFailed),
                    new object[] { "读写器异常太过频繁,错误信息:" + expArgs.ReaderException.ToString()});
                return;
            }
            else
                rechecker.AddErr();

            //尝试重新连接读写器并启动盘存操作，如果5次均失败则弹出提示
            for (int i = 0; i < 5; ++i)
            {
                try
                {
                    OpenReader();
                    rdr.StartReading();
                    break;
                }
                catch (System.Exception connex)
                {
                    if (i == 4)
                    {
                        //如果需要可在此处记录异常日志
                        Debug.WriteLine("重连读写器恢复盘存失败:" + connex.ToString());
                        this.BeginInvoke(new UpdateUi(OnResumeFailed), 
                            new object[] { "重连读写器恢复盘存失败:" + connex.ToString() });
                        return;
                    }
                }
                Thread.Sleep(4000);
            }
        }


        void OpenReader()
        {
            if (rdr != null)
            {
                rdr.Disconnect();
                rdr = null;
            }

            /*
             * 当使用设备的串口进行连接的时候，Create函数的第一个参数也可能是串口号
             * （例如com1）当设备仅有一个天线端口时（例如一体机或者发卡器），Create
             * 函数的第三个参数也可能为1,读写器出厂默认IP为192.168.1.100 
             * */
            int antnum = 1;
            rdr = Reader.Create("COM23", ModuleTech.Region.NA, antnum);

            #region 必须要设置的参数

            #region 设置读写器的发射功率
            //获取读写器最大发射功率
            int maxp = (int)rdr.ParamGet("RfPowerMax");
            AntPower[] pwrs = new AntPower[antnum];
            for (int i = 0; i < antnum; ++i)
            {
                pwrs[i].AntId = (byte)(i + 1);
                pwrs[i].ReadPower = (ushort)maxp;
                pwrs[i].WritePower = (ushort)maxp;
            }
            //设置读写器发射功率,本例设置为最大发射功率，可根据实际情况调整,
            //一般来说，功率越大则识别距离越远
            rdr.ParamSet("AntPowerConf", pwrs);
            #endregion

            #region 设置盘存标签使用的天线
            //如果要使用其它天线可以在数组useants中放置其它多个天线编号，本例中是使用天线1
            int[] useants = new int[] { 1 };
            rdr.ParamSet("ReadPlan", new SimpleReadPlan(TagProtocol.GEN2, useants));
            #endregion

            #region 设置盘存操作的其它细节选项
            BackReadOption bro = new BackReadOption();
            BackReadOption.FastReadTagMetaData frtmdata = new BackReadOption.FastReadTagMetaData();
            /*是否采用高速模式（目前只有slr11xx和slr12xx系列读写器才支持）,对于
            *一般标签数量不大，速度不快的应用没有必要使用高速模式,本例没有设置
            *使用高速模式
            * */
            bro.IsFastRead = false;
            #region 选择使用高速模式才起作用的选项
            //参照如下设置即可,如果使能高速模式，则只需取消以下被注释的代码即可
            /*
            //标签信息是否携带识别天线的编号
            frtmdata.IsAntennaID = true;
            //标签信息是否携带标签识别次数
            frtmdata.IsReadCnt = false;
            //标签信息是否携带识别标签时的信号强度
            frtmdata.IsRSSI = false;
            //标签信息是否携带时间戳
            frtmdata.IsTimestamp = false;
            //标签信息是否携带识别标签时的工作频点
            frtmdata.IsFrequency = false;
            //标签信息是否携带识别标签时同时读取的其它bank数据信息,如果要获取在
            //盘存时同时读取其它bank的信息还必须设置EmbededCmdOfInventory参数,
            //（目前只有slr11xx和slr12xx系列读写器才支持）
            frtmdata.IsEmdData = false;
            //保留字段，可始终设置为false
            frtmdata.IsRFU = false;
            //高速模式下为取得最佳效果设置为0即可
            bro.FastReadDutyRation = 0;
            */
            #endregion

            #region 非高速模式才起作用的选项
            //盘存周期,单位为ms，可根据实际使用的天线个数按照每个天线需要200ms
            //的方式计算得出,如果启用高速模式则此选项没有任何意义，可以设置为
            //任意值，或者干脆不设置
            bro.ReadDuration = (ushort)(200 * useants.Length);
            //盘存周期间的设备不工作时间,单位为ms,一般可设置为0，增加设备不工作
            //时间有利于节电和减少设备发热（针对某些使用电池供电或空间结构不利
            //于散热的情况会有帮助）
            bro.ReadInterval = 0;
            #endregion

            bro.FRTMetadata = frtmdata;
            rdr.ParamSet("BackReadOption", bro);
            #endregion

            #endregion

            #region 必须要设置的事件处理函数
            //识别到标签事件的处理函数
            rdr.ReadException += TryResumeReading;
            //读写器异常发生事件的处理函数
            rdr.TagsRead += OnTagsRead;
            #endregion

            #region 根据应用需求可能要设置的参数和事件
            //
            #region 设置Gen2Session
            /*
            //标签数量比较大，且移动速度缓慢或静止不动,设置为Session1可取得更
            //好效果,反之标签数量较少移动速度较快应设置为Session0,读写器默认
            //为Session0
            rdr.ParamSet("Gen2Session", Session.Session1);
            */
            #endregion

            #region 设置GPI触发控制读写器工作
            /*
            //共有三种触发模式：
            //1  预定义两种GPI状态，一种作为触发读写器启动盘存的条件，另一种
            //   作为停止盘存的条件
            //2  预定义一种GPI状态，触发读写器启动盘存，指定一段时间(必须大于
            //   5秒)后自动停止盘存标签
            //3  预定义两种GPI状态，满足任何一种GPI状态都触发读写器启动盘存，
            //   指定一段时间(必须大于5秒)后自动停止盘存标签

            //详见"读写器GPI触发工作模式.pdf"
            //以下示例为模式1，Trigger1State为启动工作时的GPI状态，且可以指
            //定多个GPI的状态，此例指定当GPI1为false时读写器开始盘存,当GPI2
            //为false时读写器停止盘存
            GPITrigger GpiTriiger = new GPITrigger();
            GPIState[] startstates = new GPIState[1];
            startstates[0] = new GPIState(1, false);
            GpiTriiger.Trigger1State = startstates;
            GpiTriiger.TriggerType = GPITriggerType.GPITrigger_Tri1Start_Tri2Stop;

            GPIState[] stopstates = new GPIState[1];
            stopstates[0] = new GPIState(2, false);
            GpiTriiger.Trigger2State = stopstates;
            rdr.ParamSet("BackReadGPITrigger", GpiTriiger);
            //在GPI触发工作时，当触发条件发生时的处理函数，可通过此事件获取触
            //发时刻的GPI状态，如果不关心触发时刻的GPI状态以及时序关系，可以
            //不添加此事件，详见文档"读写器GPI触发工作模式.pdf"
            rdr.GpiTrigger += GpiStateOnTrigger;
            */
            #endregion

            #region 设置盘存标签时的过滤条件
            /*
            //对于某些应用可能需要读写器只盘存符合某些数据特征的标签，
            //可以通过设置过滤条件来实现，这样对于不符合特征的标签就
            //不会被采集，以下代码实现只盘存EPC码以0011（二进制）开头
            //的标签
            string binstr = "0011";
            byte[] fiterdata = ByteFormat.FromBin(binstr);
            Gen2TagFilter filter = new Gen2TagFilter(binstr.Length, fiterdata, MemBank.EPC, 32, false);
            rdr.ParamSet("Singulation", filter);
            */
            #endregion

            #region 设置盘存过程中同时读其它bank数据
            /*
            //有些应用除了快速盘存标签的EPC码外还想同时抓取某一个bank内的
            //数据，可以通过设置此参数实现（目前只有slr11xx和slr12xx系列
            //读写器才支持），以下代码实现抓取tid区从0块开始的12个字节
            //的数据,需要注意的是抓取的字节数必须是2的倍数,如果成功获得其
            //它bank的数据则TagReadData类的成员EbdData则为获得的数据，如果
            //失败的话则EbdData为null
            EmbededCmdData ecd = new EmbededCmdData(MemBank.TID, 0, 12);
            rdr.ParamSet("EmbededCmdOfInventory", ecd);
            */
            #endregion

            #endregion

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                OpenReader();
                this.btnstart.Enabled = true;
                this.btnstop.Enabled = false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("连接读写器失败:" + ex.ToString());
                this.Close();
            }
        }

        private void btnstart_Click(object sender, EventArgs e)
        {
            try
            {
                rdr.StartReading();
                this.btnstart.Enabled = false;
                this.btnstop.Enabled = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("启动盘存失败:" + ex.ToString());
                this.Close();
            }
            
        }

        private void btnstop_Click(object sender, EventArgs e)
        {
            try
            {
                rdr.StopReading();
                this.btnstart.Enabled = true;
                this.btnstop.Enabled = false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("停止盘存失败:" + ex.ToString());
                this.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rdr != null)
            {
                if (this.btnstop.Enabled)
                    this.btnstop_Click(null, null);

                rdr.Disconnect();
            }
        }
    }
    /*
     * 用于统计读写器发生断线和异常的频率，读写器工作的时候由于网络连接和外部环境（比
     * 如有金属物体紧贴天线或者有外物撞击了天线导致天线连接断开）都有可能造成工作中断，
     * 如果引起异常的外部因素只是偶尔出现，重新初始化读写器一般都能继续工作。如果发生
     * 的频率很高则应该检查网络或读写器安装部署环境是否有问题。此类的构造函数包含两个
     * 参数，参数1是发生异常的最大次数，参数2则规定了一个时间段（单位为秒），例如参数1
     * 设置为3，参数2设置为60，则具体含义为：如果读写器在60秒内发生了3次异常则IsTrigger
     * 方法则会返回true，表示读写器发生异常的频率过高，超过了构造函数设置的门限值。
     * */
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
        int maxdursec;
        int index;
        public bool IsTrigger()
        {
            DateTime dt = DateTime.Now;
            if (index == dts.Length - 1)
            {
                if (dt.Subtract(dts[0]).TotalSeconds < maxdursec)
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
                return false;
        }
    }
}
