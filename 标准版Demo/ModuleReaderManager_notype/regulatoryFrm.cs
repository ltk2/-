using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ModuleTech;
using ModuleTech.Gen2;
using ModuleLibrary;
using System.Threading;

namespace ModuleReaderManager
{
    public partial class regulatoryFrm : Form
    {
        Reader modrdr = null;
        public regulatoryFrm(Reader rdr)
        {
            modrdr = rdr;
            InitializeComponent();
        }

        private void btnsetopfre_Click(object sender, EventArgs e)
        {
            if (this.tbopfre.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入频点");
                return;
            }
            try
            {
                modrdr.ParamSet("setOperatingFrequency", uint.Parse(this.tbopfre.Text.Trim()));
            }
            catch
            {
                MessageBox.Show("设置失败");
            }
        }

        private void btntransCW_Click(object sender, EventArgs e)
        {
            if (modrdr.HwDetails.module >= Reader.Module_Type.MODOULE_SLR1100)
            {
                if (this.tbopant.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("请先输入天线");
                    return;
                }
                if (this.tbopfre.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("请输入频点");
                    return;
                }

                CWTransmitOption cwop = new CWTransmitOption();
                cwop.offon = 1;
                cwop.antid = int.Parse(this.tbopant.Text.Trim());
                cwop.frequency = int.Parse(this.tbopfre.Text.Trim());
                AntPower[] apwrs2 = (AntPower[])modrdr.ParamGet("AntPowerConf");
                foreach (AntPower pwr in apwrs2)
                {
                    if (pwr.AntId == cwop.antid)
                    {
                        cwop.power = pwr.ReadPower;
                        break;
                    }
                }
                try
                {
                    modrdr.ParamSet("slr_transmitCWSignal", cwop);
                    this.btnstopCW.Enabled = true;
                    this.btntransCW.Enabled = false;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("发射失败:"+exp.ToString());
                }
                return;
            }
            if (this.btnsetopant.Enabled)
            {
                MessageBox.Show("请先输入天线");
                return;
            }
            try
            {
                modrdr.ParamSet("transmitCWSignal", 1);
                this.btnstopCW.Enabled = true;
                this.btntransCW.Enabled = false;
            }
            catch
            {
                MessageBox.Show("发射失败");
            }
        }

        private void btnstopCW_Click(object sender, EventArgs e)
        {
            if (modrdr.HwDetails.module >= Reader.Module_Type.MODOULE_SLR1100)
            {
                CWTransmitOption cwop = new CWTransmitOption();
                cwop.offon = 0;
                try
                {
                    modrdr.ParamSet("slr_transmitCWSignal", cwop);
                    this.btntransCW.Enabled = true;
                    this.btnstopCW.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("发射失败");
                }
                return;
            }
            try
            {
                modrdr.ParamSet("transmitCWSignal", 0);
                this.btntransCW.Enabled = true;
                this.btnstopCW.Enabled = false;
            }
            catch
            {
                MessageBox.Show("停止失败");
            }
        }

        private void btnPRBSOn_Click(object sender, EventArgs e)
        {
            if (this.btnsetopant.Enabled)
            {
                MessageBox.Show("请先输入天线");
                return;
            }
            if (this.tbdur.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入时长");
                return;
            }
            int dur = int.Parse(this.tbdur.Text.Trim());
            if (dur > 65535)
            {
                MessageBox.Show("时长必须小于65535");
                return;
            }
            ushort usdur = (ushort)dur;
            int aa = Environment.TickCount;
            try
            {
                this.btnPRBSOn.Enabled = false;
                modrdr.ParamSet("turnPRBSOn", usdur);
            }
            catch (Exception exx)
            {
                MessageBox.Show("发射失败"+exx.ToString());
            }
 //           int bb = (dur - (Environment.TickCount - aa));
 //           if (bb > 0)
  //              Thread.Sleep(bb+1500);
            this.btnPRBSOn.Enabled = true;
        }

        private void regulatoryFrm_Load(object sender, EventArgs e)
        {
            this.btnstopCW.Enabled = false;
            this.btntransCW.Enabled = true;
            
        }

        private void btnsetopant_Click(object sender, EventArgs e)
        {
            if (this.tbopant.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入天线");
                return;
            }
            int ant = int.Parse(this.tbopant.Text);
            modrdr.ParamSet("setRegulatoryOpAnt", ant);
            this.tbopant.Enabled = false;
            this.btnsetopant.Enabled = false;
        }
        public class Fre_Vswr
        {
            public int Fre { get; set; }
            public float Vswr { get; set; }
        }
        public class Fre_VswrComper : IComparer<Fre_Vswr>
        {
            public int Compare(Fre_Vswr x, Fre_Vswr y)
            {
                return x.Fre.CompareTo(y.Fre);
            }
        }

        private void filterfres()
        {
            float val = 5.0f;//指定驻波值
            VswrQueryParam vqp = new VswrQueryParam();
            vqp.AntId = (byte)1;//天线1
            vqp.Power = (ushort)2000;
            vqp.Rg = ModuleTech.Region.NA;
            modrdr.ParamSet("AntPowerVswr", vqp);
            Dictionary<int, int> antvs = null;
            try
            {
                antvs = (Dictionary<int, int>)modrdr.ParamGet("AntPowerVswr");
            }
            catch (Exception ex)
            {
                MessageBox.Show("测试失败:" + ex.ToString());
                return;
            }
        
            List<uint> filterfres=new List<uint>();
            foreach (int fre in antvs.Keys)
            {
                Fre_Vswr tmp = new Fre_Vswr();
                tmp.Fre = fre;
                float rl = (float)Math.Pow((double)10, (double)(((float)antvs[fre] / (float)10) / (float)20));
                tmp.Vswr = (1 + rl) / (rl - 1);
                
                if(tmp.Vswr>val)
                    filterfres.Add((uint)fre);
            }
            uint[] htb = (uint[])modrdr.ParamGet("FrequencyHopTable");

            List<uint> usefres = new List<uint>();

            for (int i = 0; i < htb.Length; i++)
            {
                if(!filterfres.Contains(htb[i]))
                usefres.Add(htb[i]);
            }

            modrdr.ParamSet("FrequencyHopTable", usefres.ToArray());
        }
        private void btntestvswr_Click(object sender, EventArgs e)
        {
            VswrQueryParam vqp = new VswrQueryParam(); 
            try 
            {
                vqp.AntId = byte.Parse(tbvswrantid.Text.Trim());
            }
            catch
            {
                MessageBox.Show("请输入合法的天线id");
                return;
            }
            try
            {
                vqp.Power = ushort.Parse(tbvswrpwr.Text.Trim());
            }
            catch
            {
                MessageBox.Show("请输入合法的输出功率");
                return;
            }

            try
            {
                if (checkBox_fixfre.Checked)
                {
                    uint[] htb = (uint[])modrdr.ParamGet("FrequencyHopTable");
                    vqp.Frequencies = new int[htb.Length];
                    for (int i = 0; i < htb.Length; i++)
                        vqp.Frequencies[i] = (int)htb[i];
                }
            }
            catch
            {
                MessageBox.Show("获取频点失败");
                return;
            }
           
            modrdr.ParamSet("AntPowerVswr", vqp);
            Dictionary<int, int> antvs = null;
            try
            {
                ModuleTech.Region rg = (ModuleTech.Region)modrdr.ParamGet("Region");
                vqp.Rg = rg;
                antvs = (Dictionary<int, int>)modrdr.ParamGet("AntPowerVswr");
            }
            catch (Exception ex)
            {
                MessageBox.Show("测试失败:" + ex.ToString());
                return;
            }
            string vswrstr = "";
            this.rtbVswrInfo.Text = "";
            List<Fre_Vswr> vswrlist = new List<Fre_Vswr>();
            foreach (int fre in antvs.Keys)
            {
                Fre_Vswr tmp = new Fre_Vswr();
                tmp.Fre = fre;
                float rl = (float)Math.Pow((double)10, (double)(((float)antvs[fre] / (float)10) / (float)20));
                tmp.Vswr = (1 + rl) / (rl - 1);
                vswrlist.Add(tmp);
            }
            vswrlist.Sort(new Fre_VswrComper());
            foreach (Fre_Vswr fv in vswrlist)
            {
                vswrstr += fv.Fre.ToString() + ":" + fv.Vswr.ToString("#.00") +" ";
            }
            this.rtbVswrInfo.Text = vswrstr;
        }
    }
}
