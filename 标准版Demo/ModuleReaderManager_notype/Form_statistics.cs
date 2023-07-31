using ModuleTech;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    public partial class Form_statistics : Form
    {
        public Reader modulerdr;
        public ReaderParams rParms;
        public Form1 form;
        public Form_statistics(Reader _modulerdr, ReaderParams _rParms, Form1 _form)
        {
            InitializeComponent();
            modulerdr = _modulerdr;
            rParms = _rParms;
            form = _form;
        }

        private void Form_statistics_Load(object sender, EventArgs e)
        {

        }
        Mutex opmutex = new Mutex();
        List<string> m_Tags = new List<string>();
       
        Thread timerControlThread = null;
        int starttm;
        delegate void TimerHandleDelg(int type, object paramslist);
        IAsyncResult iar_timer;

        bool istimerct;//每次循环的开光, false表示结束一次循环，等待下一次循环
        bool btnstartType = true;//读取的总开关, false表示停止
        bool timerControlTyep = true;
        private void btnstart_Click(object sender, EventArgs e)
        {
            if (btnstart.Text == "停止")
            {
                opmutex.WaitOne();
                btnstartType = false;
                btnstart.Text = "开始";
                labreadtime.Text = "0";
                opmutex.ReleaseMutex();
                return;
            }


            List<AntAndBoll> selants = null;
            if (form.readerantnumber < 8)
            {
                selants = form.CheckAntsValid();
                if (selants.Count == 0)
                {
                    MessageBox.Show("请选择天线");
                    return;
                }
            }
            else
            {
                selants = form.invants16setting;
                if (selants.Count == 0)
                {
                    MessageBox.Show("请点击'16天线设置'按钮选择天线");
                    return;
                }
            }

            try
            {
                bool isdet = (bool)modulerdr.ParamGet("CheckAntConnection");
                rParms.ischkant = isdet;
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
            for (int i = 0; i < selants.Count; ++i)
            {
                antsExe.Add(selants[i].antid);
            }

            // 设置ReadPlan 必要项，指定协议和天线组
            opmutex.WaitOne();
            List<SimpleReadPlan> readplans = new List<SimpleReadPlan>();
            readplans.Add(new SimpleReadPlan(TagProtocol.GEN2, antsExe.ToArray(), rParms.weightgen2));
            modulerdr.ParamSet("ReadPlan", readplans[0]);


            m_Tags.Clear();
            //added on 3-26
            if (rParms.isFastRead)
            {
                rParms.readdur = 50;
            }

            rParms.isFastRead = cbbreadertype.Text == "快速模式" ? true : false;
            Ex10FastModeParams ex10 = cbbreadertype.Text== "Ex10快速模式" ? new Ex10FastModeParams() : null;
            modulerdr.ParamSet("Ex10FastModeParams", ex10);

            //rParms.isFastRead  
            try
            {
                BackReadOption bro = new BackReadOption();
                bro.IsFastRead = rParms.isFastRead;
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
                //modulerdr.TagsRead;
                modulerdr.TagsRead += AddTagsToDic;

                modulerdr.StartReading();
                starttm = Environment.TickCount;
                rParms.endatetime = starttm + (Convert.ToInt32(time.Text)*1000);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("启动读取失败:" + ex.ToString());
                opmutex.ReleaseMutex();
                return;
            }

            try
            {
                istimerct = true;
                timerControlThread = new Thread(timerControl);

                timerControlThread.Start();
            }
            catch (System.Exception)
            {

            }
            btnstart.Text = "停止";
            btnstartType = true;
            timerControlTyep = true;
        }

        void timerControl()
        {
            while (timerControlTyep)
            {
                iar_timer = BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 0, (Environment.TickCount - starttm).ToString() });
                if (istimerct)
                {
                    var TickCount = Environment.TickCount - rParms.endatetime;
                    if (TickCount > 0)
                    {
                        iar_timer = BeginInvoke(new TimerHandleDelg(TimerHandle), new object[] { 1, (Environment.TickCount - starttm).ToString() });
                        istimerct = false;
                    }
                }
                if (istimerct)
                {
                    Thread.Sleep(50);
                }
                else {
                    Thread.Sleep(Convert.ToInt32(textBox2.Text) * 1000);
                    starttm = Environment.TickCount;
                    rParms.endatetime = starttm + (Convert.ToInt32(time.Text) * 1000);
                }
            }
        }

        void TimerHandle(int type, object paramslist)
        {
            if (type == 0)
            {
                labreadtime.Text = paramslist.ToString();
                labtotalcnt.Text = m_Tags.Count.ToString();
            }
            else if (type == 1)
            {
                labreadtime.Text = paramslist.ToString();

                ListViewItem item = new ListViewItem(lvTags.Items.Count.ToString());
                item.SubItems.Add(paramslist.ToString());
                item.SubItems.Add(textBox1.Text);
                item.SubItems.Add(m_Tags.Count.ToString());
                item.SubItems.Add(DateTime.Now.ToString());
                lvTags.Items.Add(item);

                //读取时间清零并暂停
                labreadtime.Text = "0";
                istimerct = true;
                m_Tags.Clear();
                labtotalcnt.Text = m_Tags.Count.ToString();
                

                //停止
                if (!btnstartType) { 
                    modulerdr.StopReading();
                    timerControlTyep = false;
                }
            }
        }

        void AddTagsToDic(object sender, Reader.TagsReadEventArgs tagsArgs)
        {
            if (!istimerct)
            {
                return;
            }

            foreach (TagReadData tag in tagsArgs.Tags)
            {
                AddTagToDic(tag);
            }
        }
        void AddTagToDic(TagReadData tag)
        {
            if (rParms.isUniByEmd && (tag.Tag.Protocol == TagProtocol.GEN2))
            {
                if (tag.EMDDataString == string.Empty)
                {
                    return;
                }
            }

            string keystr = tag.EPCString;
            if (rParms.isUniByEmd)
            {
                keystr += tag.EMDDataString;
            }
            if (rParms.isUniByAnt)
            {
                keystr += tag.Antenna.ToString();
            }

            if (!m_Tags.Contains(keystr))
            {
                m_Tags.Add(keystr);
            }

           
        }
    }
}
