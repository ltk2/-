using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ModuleTech;
using ModuleTech.Gen2;
using ModuleTech.ISO180006b;
using ModuleLibrary;
using System.Diagnostics;

namespace ModuleReaderManager
{
    public partial class readerParaform : Form
    {
        ReaderParams m_params;
        Reader rdr;
        public readerParaform(ReaderParams paras, Reader rd)
        {
            InitializeComponent();
            m_params = paras;
            rdr = rd;
        }

        Dictionary<int, TextBox> rants = new Dictionary<int, TextBox>();
        Dictionary<int, TextBox> wants = new Dictionary<int, TextBox>();
        private void readerParaform_Load(object sender, EventArgs e)
        {
            this.tbMacAddr.Enabled = false;
            this.cbgpi1.Enabled = false;
            this.cbgpi2.Enabled = false;
            this.cbgpi3.Enabled = false;
            this.cbgpi4.Enabled = false;

            this.cbgpo1.Enabled = false;
            this.cbgpo2.Enabled = false;
            this.cbgpo3.Enabled = false;
            this.cbgpo4.Enabled = false;

            if (m_params.readertype == ReaderType.MT_THREEANTS)
            {
                this.cbgpi1.Enabled = true;
                this.cbgpi2.Enabled = true;
                this.cbgpo2.Enabled = true;
            }
            else if (m_params.readertype == ReaderType.MT_TWOANTS || m_params.readertype == ReaderType.MT_ONEANT||m_params.readertype==ReaderType.MT100)
            {
                this.cbgpi1.Enabled = true;
                this.cbgpi2.Enabled = true;
                this.cbgpo1.Enabled = true;
                this.cbgpo2.Enabled = true;
            }
            else if (m_params.readertype == ReaderType.MT_FOURANTS)
            {
                this.cbgpi1.Enabled = true;
                this.cbgpi2.Enabled = true;
                this.cbgpo1.Enabled = true;
                this.cbgpo2.Enabled = true;
            }
            else if (m_params.readertype == ReaderType.MT_A7_TWOANTS ||
                m_params.readertype == ReaderType.MT_A7_FOURANTS ||
                m_params.readertype == ReaderType.SL_FOURANTS ||
                m_params.readertype == ReaderType.M6_A7_FOURANTS ||
                m_params.readertype == ReaderType.M56_A7_FOURANTS ||
                m_params.readertype == ReaderType.SL_FOURANTS)
            {
                if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM7 ||
                    rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_WIFI)
                {
                    this.cbgpi1.Enabled = true;
                    this.cbgpi2.Enabled = true;
                    this.cbgpi3.Enabled = true;
                    this.cbgpi4.Enabled = true;

                    this.cbgpo1.Enabled = true;
                    this.cbgpo2.Enabled = true;
                    this.cbgpo3.Enabled = true;
                    this.cbgpo4.Enabled = true;
                }
                else if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9
                    || rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI)
                {
                    this.cbgpi1.Enabled = true;
                    this.cbgpi2.Enabled = true;

                    this.cbgpo1.Enabled = true;
                    this.cbgpo2.Enabled = true;
                    this.cbgpo3.Enabled = true;
                    this.cbgpo4.Enabled = true;
                }
            }
            this.labMoudevir.Text = m_params.hardvir;
            this.labfirmware.Text = m_params.softvir;

            if (m_params.hasIP)
            {
                if (!(rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI ||
                    rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI_V2))
                {
                    this.rbnettypeeth.Enabled = false;
                    this.rbnettypewifi.Enabled = false;
                }
                this.btnipget_Click(null, null);
            }
            else
                this.gpipinfo.Enabled = false;

            if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM7)
                this.labmainboard.Text = "arm7 主板";
            else if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_SERIAL)
                this.labmainboard.Text = "串口主板";
            else if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_WIFI)
                this.labmainboard.Text = "wifi 主板";
            else if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9)
                this.labmainboard.Text = "arm9 主板";
            else if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI)
                this.labmainboard.Text = "arm9_wifi 主板";
            else if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_V2)
                this.labmainboard.Text = "arm9_v2 主板";
            else if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI_V2)
                this.labmainboard.Text = "arm9_wifi_v2 主板";


            if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_M5E)
                this.labmodule.Text = "m5e";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100)
                this.labmodule.Text = "slr1100";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5900)
                this.labmodule.Text = "slr5900";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5800)
                this.labmodule.Text = "slr5800";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR6000)
                this.labmodule.Text = "slr6000";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR6100)
                this.labmodule.Text = "slr6100";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1200)
                this.labmodule.Text = "slr1200";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1300)
                this.labmodule.Text = "slr1300";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5100)
                this.labmodule.Text = "slr5100";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5200)
                this.labmodule.Text = "slr5200";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                this.labmodule.Text = "slr5300";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7100)
                this.labmodule.Text = "sim7100";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7200)
                this.labmodule.Text = "sim7200";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7300)
                this.labmodule.Text = "sim7300";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7400)
                this.labmodule.Text = "sim7400";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7500)
                this.labmodule.Text = "sim7500";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7600)
                this.labmodule.Text = "sim7600";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM3100)
                this.labmodule.Text = "sim3100";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM3200)
                this.labmodule.Text = "sim3200";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM3300)
                this.labmodule.Text = "sim3300";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM3400)
                this.labmodule.Text = "sim3400";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM3500)
                this.labmodule.Text = "sim3500";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM3600)
                this.labmodule.Text = "sim3600";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM5100)
                this.labmodule.Text = "sim5100";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM5200)
                this.labmodule.Text = "sim5200";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM5300)
                this.labmodule.Text = "sim5300";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM5400)
                this.labmodule.Text = "sim5400";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM5500)
                this.labmodule.Text = "sim5500";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM5600)
                this.labmodule.Text = "sim5600";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR3000)
                this.labmodule.Text = "slr3000";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR3100)
                this.labmodule.Text = "slr3100";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR3200)
                this.labmodule.Text = "slr3200";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_M5E_PRC)
                this.labmodule.Text = "m5e-prc";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_M5E_C)
                this.labmodule.Text = "m5e-c";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E)
                this.labmodule.Text = "m6e";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC)
                this.labmodule.Text = "m6e-prc";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_R902_MT100)
                this.labmodule.Text = "mt100";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_R902_MT200)
                this.labmodule.Text = "mt200";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_R2000)
                this.labmodule.Text = "r2000";
            else if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_MICRO)
                this.labmodule.Text = "m6e-micro";
            else
                this.labmodule.Text = "未知";

            this.label25.Text = (string)rdr.ParamGet("SoftwareVersion");

            byte temperature = (byte)rdr.ParamGet("Temperature");
            this.label14.Text = temperature.ToString();

            Gen2get.PerformClick();
            btngethtb.PerformClick();
            btngetgpi.PerformClick();
            button2.PerformClick();
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

        int getRn = 0;
        uint[] gethtb = null;
        private void btngethtb_Click(object sender, EventArgs e)
        {
            try
            {
                int st = Environment.TickCount;
                ModuleTech.Region rg = (ModuleTech.Region)rdr.ParamGet("Region");
                Debug.WriteLine((Environment.TickCount - st).ToString());

                getRn = getRegion(rg);
                cbbregion.SelectedIndex= getRn;

            }
            catch (Exception ex)
            {
                MessageBox.Show("获取失败");
            }

            try
            {
                int st = Environment.TickCount;
                uint[] htb = (uint[])rdr.ParamGet("FrequencyHopTable");

                if (htb != null)
                    Sort(ref htb);
                gethtb = htb;
               
                int cnt = 0;
                uint curchal = htb[htb.Length - 1];
                lvhoptb.Items.Clear();
                foreach (uint fre in htb)
                {
                    cnt++;
                    ListViewItem item = new ListViewItem(fre.ToString());
                    if (m_params.readertype == ReaderType.PR_ONEANT)
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

        private void btnsethtb_Click(object sender, EventArgs e)
        {
            try
            {
                
                rdr.ParamSet("Region", getRegion(cbbregion.SelectedIndex));

                if(getRn == cbbregion.SelectedIndex){
                    List<uint> htb = new List<uint>();
                    foreach (ListViewItem item in lvhoptb.Items)
                    {
                        if (item.Checked)
                            htb.Add(uint.Parse(item.SubItems[0].Text));
                    }

                    if (m_params.readertype == ReaderType.PR_ONEANT)
                    {
                        if (htb.Count != 1)
                        {
                            MessageBox.Show("只能设置一个信道");
                            return;
                        }
                    }

                    if (htb.Count > 0)
                        rdr.ParamSet("FrequencyHopTable", htb.ToArray());
                }

                btngethtb.PerformClick();
            }
            catch(Exception)
            {
                MessageBox.Show("设置失败 ");
            }
        }

        private void btngetrg_Click(object sender, EventArgs e)
        {
            try
            {
                int st = Environment.TickCount;
                ModuleTech.Region rg = (ModuleTech.Region)rdr.ParamGet("Region");
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
                        if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E ||
                            rdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC ||
                            rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100 ||
                            rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1200 ||
                        rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1300||
                        rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5100 ||
                            rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5200 ||
                        rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
                        {
                            if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC ||
                                rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100||
                                 rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1200 ||
                        rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1300||
                        rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5100 ||
                            rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5200 ||
                        rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
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
                        if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E ||
                            rdr.HwDetails.module == Reader.Module_Type.MODOULE_M6E_PRC ||
                            rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR1100)
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
                //case 11:
                //    rg = ModuleTech.Region.KOREA;
                //    break;
                //case 12:
                //    rg = ModuleTech.Region.JAPAN;
                //    break;
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

            if (m_params.readertype == ReaderType.PR_ONEANT)
            {
                if (rg == ModuleTech.Region.OPEN || rg == ModuleTech.Region.CN)
                {
                    MessageBox.Show("不支持的区域");
                    return;
                }
            }
            try
            {
                rdr.ParamSet("Region", rg);
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
                    rdr.ParamSet("FrequencyHopTable", htab.ToArray());
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

        private void btngetgpi_Click(object sender, EventArgs e)
        {
            if (this.cbgpi1.Enabled)
            {
                if (rdr.GPIGet(1))
                    this.cbgpi1.Checked = true;
                else
                    this.cbgpi1.Checked = false;
            }
            if (this.cbgpi2.Enabled)
            {
                if (rdr.GPIGet(2))
                    this.cbgpi2.Checked = true;
                else
                    this.cbgpi2.Checked = false;
            }
            if (this.cbgpi3.Enabled)
            {
                if (rdr.GPIGet(3))
                    this.cbgpi3.Checked = true;
                else
                    this.cbgpi3.Checked = false;
            }
            if (this.cbgpi4.Enabled)
            {
                if (rdr.GPIGet(4))
                    this.cbgpi4.Checked = true;
                else
                    this.cbgpi4.Checked = false;
            }
        }

        private void btnsetgpo_Click(object sender, EventArgs e)
        {
            if (this.cbgpo1.Enabled)
            {
                if (this.cbgpo1.Checked)
                    rdr.GPOSet(1, true);
                else
                    rdr.GPOSet(1, false);
            }
            if (this.cbgpo2.Enabled)
            {
                if (this.cbgpo2.Checked)
                    rdr.GPOSet(2, true);
                else
                    rdr.GPOSet(2, false);
            }
            if (this.cbgpo3.Enabled)
            {
                if (this.cbgpo3.Checked)
                    rdr.GPOSet(3, true);
                else
                    rdr.GPOSet(3, false);
            }
            if (this.cbgpo4.Enabled)
            {
                if (this.cbgpo4.Checked)
                    rdr.GPOSet(4, true);
                else
                    rdr.GPOSet(4, false);
            }
        }

        private void btnGetWriteMode_Click(object sender, EventArgs e)
        {

            WriteMode wm = WriteMode.WORD_ONLY;
            try
            {
                wm = (WriteMode)rdr.ParamGet("Gen2WriteMode");
            }
            catch
            {
                MessageBox.Show("操作失败");
                return;
            }
            this.cbbwritemode.SelectedIndex = (int)wm;
        }

        private void btnSetWriteMode_Click(object sender, EventArgs e)
        {
            if (this.cbbwritemode.SelectedIndex == -1)
                MessageBox.Show("请选择写模式");
            else
            {
                try
                {
                    rdr.ParamSet("Gen2WriteMode", (WriteMode)this.cbbwritemode.SelectedIndex);
                }
                catch
                {
                    MessageBox.Show("操作失败");
                }
            }
        }

        private void cbmacset_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbmacset.Checked)
                this.tbMacAddr.Enabled = true;
            else
                this.tbMacAddr.Enabled = false;
        }

        private void btngetgen2encode_Click(object sender, EventArgs e)
        {
            this.cbbgen2encode.Items.Clear();
            if ((int)rdr.HwDetails.module >= 24)
            {
                this.cbbgen2encode.Items.AddRange(new object[] {
                "FM0","M2","M4","M8","RF_MODE_1","RF_MODE_3","RF_MODE_5","RF_MODE_7",
                "RF_MODE_11","RF_MODE_12","RF_MODE_13","RF_MODE_15"});
            }
            else {
                this.cbbgen2encode.Items.AddRange(new object[] {
                "FM0","M2","M4","M8","PROFILE 0","PROFILE 1","PROFILE 2","PROFILE 3","PROFILE 4","PROFILE 5",});
            }

            try
            {
                int enc = (int)rdr.ParamGet("gen2tagEncoding");
                if (enc <= 3)
                    this.cbbgen2encode.SelectedIndex = enc;
                else if (enc > 100)
                {
                    if (enc == 101)
                        cbbgen2encode.SelectedIndex=4;
                    if (enc == 103)
                        cbbgen2encode.SelectedIndex=5;
                    if (enc == 105)
                        cbbgen2encode.SelectedIndex=6;
                    if (enc == 107)
                        cbbgen2encode.SelectedIndex=7;
                    if (enc == 111)
                        cbbgen2encode.SelectedIndex=8;
                    if (enc == 112)
                        cbbgen2encode.SelectedIndex=9;
                    if (enc == 113)
                        cbbgen2encode.SelectedIndex=10;
                    if (enc == 115)
                        cbbgen2encode.SelectedIndex=11;
                }
                else
                    this.cbbgen2encode.SelectedIndex = 4 + enc - 0x10;
            }
            catch
            {
                MessageBox.Show("操作失败");
            }
        }

        private void btnsetgen2encode_Click(object sender, EventArgs e)
        {
            if (this.cbbgen2encode.SelectedIndex == -1)
            {
                MessageBox.Show("请选择编码方式");
                return;
            }

            try
            {
                //if (this.cbbgen2encode.SelectedIndex <= 3)
                //    rdr.ParamSet("gen2tagEncoding", this.cbbgen2encode.SelectedIndex);
                //else if (this.cbbgen2encode.SelectedIndex > 3 && this.cbbgen2encode.SelectedIndex <= 9)
                //    rdr.ParamSet("gen2tagEncoding", 0x10 + this.cbbgen2encode.SelectedIndex - 4);
                //else
                //{
                //    int val = this.cbbgen2encode.SelectedIndex;
                //    if (val == 10)
                //        val = 101;
                //    else if (val == 11)
                //        val = 103;
                //    else if (val == 12)
                //        val = 105;
                //    else if (val == 13)
                //        val = 107;
                //    else if (val == 14)
                //        val = 111;
                //    else if (val == 15)
                //        val = 112;
                //    else if (val == 16)
                //        val = 113;
                //    else if (val == 17)
                //        val = 115;
                //    rdr.ParamSet("gen2tagEncoding", val);
                //}

                rdr.ParamSet("gen2tagEncoding", getGen2val(cbbgen2encode.Text)); 
            }
            catch(Exception exx)
            {
                MessageBox.Show("操作失败:"+exx.ToString());
            }
        }

        private void btngetgen2target_Click(object sender, EventArgs e)
        {
            try
            {
                ModuleTech.Gen2.Target tt = (ModuleTech.Gen2.Target)rdr.ParamGet("Gen2Target");
                this.cbbgen2target.SelectedIndex = (int)tt;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取失败:" + ex.ToString());
            }
        }

        private void btnsetgen2target_Click(object sender, EventArgs e)
        {
            if (this.cbbgen2target.SelectedIndex == -1)
            {
                MessageBox.Show("请选择Target");
                return;
            }
            try
            {
                rdr.ParamSet("Gen2Target", (ModuleTech.Gen2.Target)this.cbbgen2target.SelectedIndex);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("设置失败:" + ex.ToString());
            }
        }

        private void btnge2sessget_Click(object sender, EventArgs e)
        {
            try
            {
                ModuleTech.Gen2.Session sess = (ModuleTech.Gen2.Session)rdr.ParamGet("Gen2Session");
                this.cbbsession.SelectedIndex = (int)sess;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取失败:" + ex.ToString());
            }
            
        }

        private void btngen2sessset_Click(object sender, EventArgs e)
        {
            if (this.cbbsession.SelectedIndex == -1)
            {
                MessageBox.Show("请选择session");
                return;
            }
            try
            {
                rdr.ParamSet("Gen2Session", (ModuleTech.Gen2.Session)this.cbbsession.SelectedIndex);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("设置失败:" + ex.ToString());
            }
        }
        int gen2Qvalue = 0;
        int gen2Session = 0;
        int gen2Target = 0;
        int gen2tagEncoding = 0;
        int gen2WriteMode = 0;
        private void Gen2get_Click(object sender, EventArgs e)
        {
            try
            {
                gen2Qvalue = (int)rdr.ParamGet("Gen2Qvalue");
                this.cbbGen2Q.SelectedIndex = gen2Qvalue+1;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取Gen2Qvalue失败" + ex.ToString());
            }

            try
            {
                gen2Session = (int)rdr.ParamGet("Gen2Session");
                this.cbbsession.SelectedIndex = gen2Session;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取Gen2Session失败:" + ex.ToString());
            }
            try
            {
                gen2Target = (int)rdr.ParamGet("Gen2Target");
                this.cbbgen2target.SelectedIndex = gen2Target;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取Gen2Target失败:" + ex.ToString());
            }

            this.cbbgen2encode.Items.Clear();
            if ((int)rdr.HwDetails.module >= 24)
            {
                this.cbbgen2encode.Items.AddRange(new object[] {
                "FM0","M2","M4","M8","RF_MODE_1","RF_MODE_3","RF_MODE_5","RF_MODE_7",
                "RF_MODE_11","RF_MODE_12","RF_MODE_13","RF_MODE_15","RF_MODE_103","RF_MODE_120","RF_MODE_345"});
            }
            else
            {
                this.cbbgen2encode.Items.AddRange(new object[] {
                "FM0","M2","M4","M8","PROFILE 0","PROFILE 1","PROFILE 2","PROFILE 3","PROFILE 4","PROFILE 5",});
            }

            try
            {
                gen2tagEncoding = (int)rdr.ParamGet("gen2tagEncoding");
                if (gen2tagEncoding <= 3)
                    this.cbbgen2encode.SelectedIndex = gen2tagEncoding;
                else if (gen2tagEncoding == 45)
                    cbbgen2encode.SelectedIndex = 14;
                else if (gen2tagEncoding > 100)
                {
                    if (gen2tagEncoding == 101)
                        cbbgen2encode.SelectedIndex = 4;
                    if (gen2tagEncoding == 103)
                        cbbgen2encode.SelectedIndex = 5;
                    if (gen2tagEncoding == 105)
                        cbbgen2encode.SelectedIndex = 6;
                    if (gen2tagEncoding == 107)
                        cbbgen2encode.SelectedIndex = 7;
                    if (gen2tagEncoding == 111)
                        cbbgen2encode.SelectedIndex = 8;
                    if (gen2tagEncoding == 112)
                        cbbgen2encode.SelectedIndex = 9;
                    if (gen2tagEncoding == 113)
                        cbbgen2encode.SelectedIndex = 10;
                    if (gen2tagEncoding == 115)
                        cbbgen2encode.SelectedIndex = 11;
                    if (gen2tagEncoding == 203)
                        cbbgen2encode.SelectedIndex = 12;
                    if (gen2tagEncoding == 220)
                        cbbgen2encode.SelectedIndex = 13;
                   
                }
                else
                    this.cbbgen2encode.SelectedIndex = 4 + gen2tagEncoding - 0x10;
            }
            catch
            {
                MessageBox.Show("操作失败");
            }

          
            try
            {
                gen2WriteMode = (int)rdr.ParamGet("Gen2WriteMode");
                this.cbbwritemode.SelectedIndex = gen2WriteMode;
            }
            catch
            {
                MessageBox.Show("操作WriteMode失败");
                return;
            }
        }

        private void btngen2qset_Click(object sender, EventArgs e)
        {
            try
            {
                if(gen2Qvalue!= this.cbbGen2Q.SelectedIndex - 1)
                    rdr.ParamSet("Gen2Qvalue", this.cbbGen2Q.SelectedIndex-1);

                if(gen2Session!= this.cbbsession.SelectedIndex)
                    rdr.ParamSet("Gen2Session", (ModuleTech.Gen2.Session)this.cbbsession.SelectedIndex);

                if(gen2Target != this.cbbgen2target.SelectedIndex)
                    rdr.ParamSet("Gen2Target", (ModuleTech.Gen2.Target)this.cbbgen2target.SelectedIndex);

                if(gen2tagEncoding!= this.cbbgen2encode.SelectedIndex)
                    rdr.ParamSet("gen2tagEncoding", getGen2val(cbbgen2encode.Text));

                if(gen2WriteMode!= this.cbbwritemode.SelectedIndex)
                    rdr.ParamSet("Gen2WriteMode", (WriteMode)this.cbbwritemode.SelectedIndex);

                Gen2get.PerformClick();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("设置失败:" + ex.ToString());
            }
        }

        private void btnpwrget_Click(object sender, EventArgs e)
        {
            AntPower[] apwrs2 = (AntPower[])rdr.ParamGet("AntPowerConf");
            foreach (AntAndBoll a in m_params.AntsState)
            {
                foreach (AntPower b in apwrs2)
                {
                    if (a.antid == b.AntId)
                    {
                        a.rpower = b.ReadPower;
                        a.wpower = b.WritePower;
                        break;
                    }
                }
            }
            if (m_params.AntsState.Count> 4)
                return;

            foreach (AntAndBoll a in m_params.AntsState)
            {
                if (rants[a.antid].Enabled)
                {
                    rants[a.antid].Text = a.rpower.ToString();
                    wants[a.antid].Text = a.wpower.ToString();
                }
            }
        }

        private void btnpwrset_Click(object sender, EventArgs e)
        {
            int maxp = (int)rdr.ParamGet("RfPowerMax");
            int minp = (int)rdr.ParamGet("RfPowerMin");
            //maxp = 2500;
            foreach (AntAndBoll a in m_params.AntsState)
            {
                if (rants[a.antid].Enabled == true)
                {
                    int rpwr;
                    int wpwr;
                    try
                    {
                        rpwr = int.Parse(rants[a.antid].Text);
                        wpwr = int.Parse(wants[a.antid].Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("功率输入错误，请输入整数");
                        return;
                    }

                    //if (rdr.HwDetails.module != Reader.Module_Type.MODOULE_M6E_MICRO)
                    //{
                    //    if (rpwr < minp || rpwr > maxp || wpwr < minp || wpwr > maxp)
                    //    {
                    //        MessageBox.Show("功率值只能在" + minp.ToString() + "到" + maxp.ToString() + "之间");
                    //        return;
                    //    }
                    //}

                    a.rpower = (ushort)(rpwr);
                    a.wpower = (ushort)(wpwr);

                    if (m_params.readertype == ReaderType.PR_ONEANT)
                    {
                        if (a.rpower != a.wpower)
                        {
                            MessageBox.Show("此类型读写器要求读写功率相同");
                            return;
                        }
                    }
                }
            }
            List<AntPower> antspwr = new List<AntPower>();

            if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5300)
            {
                antspwr.Add(new AntPower((byte)m_params.AntsState[0].antid, (ushort)(m_params.AntsState[0].rpower),
                        (ushort)(m_params.AntsState[0].wpower)));
            }
            else {
                foreach (AntAndBoll at in m_params.AntsState)
                {
                    antspwr.Add(new AntPower((byte)at.antid, (ushort)(at.rpower),
                        (ushort)(at.wpower)));
                }
            }

            try
            {
                rdr.ParamSet("AntPowerConf", antspwr.ToArray());
            }
            catch (System.Exception exx)
            {
                MessageBox.Show("设置失败：" + exx.ToString());
            }
        }

        private void btnipget_Click(object sender, EventArgs e)
        {
            if (!(rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI ||
                rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI_V2))
            {
                this.tbipaddr.Text = m_params.ip;
                this.tbsubnet.Text = m_params.subnet;
                this.tbgateway.Text = m_params.gateway;
                if (m_params.macstr != null)
                {
                    this.tbMacAddr.Text = m_params.macstr;
                }
            }
            else
            {
                try
                {
                    ReaderIPInfo_Ex ipinfo = (ReaderIPInfo_Ex)rdr.ParamGet("IPAddressEx");
                    this.tbipaddr.Text = ipinfo.IPInfo.IP;
                    this.tbsubnet.Text = ipinfo.IPInfo.SUBNET;
                    this.tbgateway.Text = ipinfo.IPInfo.GATEWAY;

                    if (ipinfo.NType == ReaderIPInfo_Ex.NetType.NetType_Ethernet)
                        this.rbnettypeeth.Checked = true;
                    else if (ipinfo.NType == ReaderIPInfo_Ex.NetType.NetType_Wifi)
                    {                       
                        this.cbbwifiauth.SelectedIndex = (int)(ipinfo.Wifi.Auth-1);
                        this.tbwifissid.Text = ipinfo.Wifi.SSID;
                        if (ipinfo.Wifi.KEY != null)
                            this.tbwifikey.Text = ipinfo.Wifi.KEY;
                        else
                            this.tbwifikey.Text = "";
                        this.rbnettypewifi.Checked = true;
                        this.cbbkeytype.SelectedIndex = (int)(ipinfo.Wifi.KType - 1);
                    }

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("获取失败:" + ex.ToString());
                }
                
            }
        }

        private void btnipset_Click(object sender, EventArgs e)
        {
            if (this.tbipaddr.Text.Trim() == string.Empty || this.tbsubnet.Text.Trim() == string.Empty
                || this.tbgateway.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入ip地址相关项");
                return;
            }
            ReaderIPInfo ipinfo = null;
            try
            {
                ipinfo = ReaderIPInfo.Create(this.tbipaddr.Text.Trim(), this.tbsubnet.Text.Trim(),
                    this.tbgateway.Text.Trim());
                if (this.cbmacset.Checked)
                {
                    if (this.tbMacAddr.Text.Trim().Length != 12)
                    {
                        MessageBox.Show("MAC地址格式错误");
                        return;
                    }
                    else
                    {
                        try
                        {
                            byte[] macb = ByteFormat.FromHex(this.tbMacAddr.Text.Trim());
                            ipinfo.MACADDR = macb;
                        }
                        catch
                        {
                            MessageBox.Show("MAC地址格式错误");
                            return;
                        }
                    }

                }
            }
            catch (OpFaidedException exp)
            {
                MessageBox.Show("ip 地址设置有错误:" + exp.ToString());
                return;
            }

            if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI ||
                rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI_V2)
            {   
                if ((!this.rbnettypewifi.Checked) && (!this.rbnettypeeth.Checked))
                {
                    MessageBox.Show("请选择是以太网，还是wifi");
                    return;
                }

                ReaderIPInfo_Ex.NetType type = ReaderIPInfo_Ex.NetType.NetType_None;
                ReaderIPInfo_Ex.WifiSetting wifi = null;
                if (this.rbnettypewifi.Checked)
                {
                    if (this.cbbwifiauth.SelectedIndex == -1 || this.tbwifissid.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("请输入wifi配置相关项");
                        return;
                    }

                    ReaderIPInfo_Ex.WifiSetting.AuthMode auth = (ReaderIPInfo_Ex.WifiSetting.AuthMode)(this.cbbwifiauth.SelectedIndex + 1);

                    if (this.cbbwifiauth.SelectedIndex == 0)
                    {
                        wifi = new ReaderIPInfo_Ex.WifiSetting(auth, this.tbwifissid.Text.Trim(),
                            ReaderIPInfo_Ex.WifiSetting.KeyType.KeyType_NONE, null);
                    }
                    else
                    {
                        if (this.tbwifikey.Text.Trim() == string.Empty || this.cbbkeytype.SelectedIndex == -1)
                        {
                            MessageBox.Show("请输入wifi配置相关项");
                            return;
                        }
                        if (this.cbbwifiauth.SelectedIndex == 3 || this.cbbwifiauth.SelectedIndex == 4)
                        {
                            if (this.cbbkeytype.SelectedIndex == 1)
                            {
                                MessageBox.Show("密钥类型只能是ASC2码");
                                return;
                            }
                        }

                        wifi = new ReaderIPInfo_Ex.WifiSetting(auth, this.tbwifissid.Text.Trim(), 
                            (ReaderIPInfo_Ex.WifiSetting.KeyType)(this.cbbkeytype.SelectedIndex+1),
                            this.tbwifikey.Text.Trim());
                    }
            
                    type = ReaderIPInfo_Ex.NetType.NetType_Wifi;
                }
                else
                    type = ReaderIPInfo_Ex.NetType.NetType_Ethernet;
          
                ReaderIPInfo_Ex ininfoex = new ReaderIPInfo_Ex(ipinfo, type, wifi);
                try
                {
                    rdr.ParamSet("IPAddressEx", ininfoex);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("设置失败:" + ex.ToString());
                }
                
            }
            else
            {
                try
                {
                    rdr.ParamSet("IPAddress", ipinfo);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("设置失败:" + ex.ToString());
                }
            }
            if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI ||
                rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM9_WIFI_V2)
                MessageBox.Show("设置成功，给读写器重新上电后生效");
            else
                MessageBox.Show("ip设置成功，请重新连接读写器");
        }

        private void rbnettypeeth_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnettypeeth.Checked)
            {
                //this.gpwificonf.Enabled = false;
            }
        }

        private void rbnettypewifi_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnettypewifi.Checked)
            {
               //this.gpwificonf.Enabled = true;
            }
        }

        private void btnset915_Click(object sender, EventArgs e)
        {
            int[] hoptb = new int[] { 915750, 915250, 917750, 914750, 913750, 917250, 914250, 916250, 916750, 913250 };
            rdr.ParamSet("FrequencyHopTable", hoptb);
        }

        private void allcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (allcheckBox.Checked)
            {
                for (int i = 0; i < lvhoptb.Items.Count; i++)
                {
                    lvhoptb.Items[i].Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < lvhoptb.Items.Count; i++)
                {
                    lvhoptb.Items[i].Checked = false;
                }
            }
        }

        private void button_mpfrm_Click(object sender, EventArgs e)
        {
            try
            {
                Session sn = (Session)cbbsession.SelectedIndex;
                rdr.ParamSet("ModuleSave_ProtocolConfig_Gen2_Session", sn);

                Target tg = (Target)cbbgen2target.SelectedIndex;
                rdr.ParamSet("ModuleSave_ProtocolConfig_Gen2_Target", tg);

                int val = cbbGen2Q.SelectedIndex - 1;
                rdr.ParamSet("ModuleSave_ProtocolConfig_Gen2_Q", val);

                if (this.cbbgen2encode.SelectedIndex <= 3)
                    rdr.ParamSet("ModuleSave_ProtocolConfig_Gen2_M", this.cbbgen2encode.SelectedIndex);
                else if (this.cbbgen2encode.SelectedIndex > 3 && this.cbbgen2encode.SelectedIndex <= 9)
                    rdr.ParamSet("ModuleSave_ProtocolConfig_Gen2_M", 0x10 + this.cbbgen2encode.SelectedIndex - 4);
                else
                {
                    rdr.ParamSet("ModuleSave_ProtocolConfig_Gen2_M", getGen2val(cbbgen2encode.Text));
                }

                var rg= getRegion(cbbregion.SelectedIndex);
                rdr.ParamSet("ModuleSave_Region", rg);

                if(textBox_fretime.Enabled)
                    rdr.ParamSet("ModuleSave_Frenqency_time", Convert.ToInt32(textBox_fretime.Text));

                uint[] shtb = null;
                List<uint> htb = new List<uint>();
                foreach (ListViewItem item in lvhoptb.Items)
                {
                    if (item.Checked)
                        htb.Add(uint.Parse(item.SubItems[0].Text));
                }
                if (htb.Count < 1)
                {
                    MessageBox.Show("请选择其中一个");
                    return;
                }
                shtb = htb.ToArray();
                rdr.ParamSet("ModuleSave_Frenqency", shtb);

                if (baud != comboBox_mpbaud.SelectedIndex|| autoApp!= radioButton_mpautoyes.Checked) {
                    if (baud != comboBox_mpbaud.SelectedIndex&& comboBox_mpbaud.Enabled) { 
                        int baudRate = int.Parse(comboBox_mpbaud.Text);
                        rdr.ParamSet("ModuleSave_Baudrate", baudRate);
                    }

                    if (autoApp != radioButton_mpautoyes.Checked&& radioButton_mpautoyes.Enabled) {
                        rdr.ParamSet("ModuleSave_AutoApp", radioButton_mpautoyes.Checked);
                    }

                    MessageBox.Show("设置完毕，关闭程序，请断电5秒后再重新打开程序连接读写器");
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int getGen2val(string name) {
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
                case "RF_MODE_103":
                    return 203;
                case "RF_MODE_120":
                    return 220;
                case "RF_MODE_345":
                    return 45;
            }
            return 0;
        }


        public int getRegion(ModuleTech.Region rg) {
            var val = 0;
            switch ((ModuleTech.Region)rg)
            {
                case ModuleTech.Region.CN:
                    val=6;
                    break;
                case ModuleTech.Region.EU:
                case ModuleTech.Region.EU2:
                case ModuleTech.Region.EU3:
                    val=4;
                    break;
                case ModuleTech.Region.IN:
                    val = 5;
                    break;
                case ModuleTech.Region.JP:
                    val = 2;
                    break;
                case ModuleTech.Region.KR:
                    val = 3;
                    break;
                case ModuleTech.Region.NA:
                    val = 1;
                    break;
                case ModuleTech.Region.PRC:
                    val = 0;
                    break;
                case ModuleTech.Region.OPEN:
                    val = 7;
                    break;
                case ModuleTech.Region.PRC2:
                    val = 8;
                    break;

                case ModuleTech.Region.CE_HIGH:
                    val = 11;
                    break;
                case ModuleTech.Region.HK:
                    val = 12;
                    break;
                case ModuleTech.Region.TAIWAN:
                    val = 13;
                    break;
                case ModuleTech.Region.MALAYSIA:
                    val = 14;
                    break;
                case ModuleTech.Region.SOUTH_AFRICA:
                    val = 15;
                    break;
                case ModuleTech.Region.BRAZIL:
                    val = 16;
                    break;
                case ModuleTech.Region.THAILAND:
                    val = 17;
                    break;
                case ModuleTech.Region.SINGAPORE:
                    val = 18;
                    break;
                case ModuleTech.Region.AUSTRALIA:
                    val = 19;
                    break;
                case ModuleTech.Region.URUGUAY:
                    val = 20;
                    break;
                case ModuleTech.Region.VIETNAM:
                    val = 21;
                    break;
                case ModuleTech.Region.ISRAEL:
                    val = 22;
                    break;
                case ModuleTech.Region.PHILIPPINES:
                    val = 23;
                    break;
                case ModuleTech.Region.INDONESIA:
                    val = 24;
                    break;
                case ModuleTech.Region.NEW_ZEALAND:
                    val = 25;
                    break;
                case ModuleTech.Region.PERU:
                    val = 26;
                    break;
                case ModuleTech.Region.RUSSIA:
                    val = 27;
                    break;
            }
            return val;
        }

        public ModuleTech.Region getRegion(int val)
        {
            ModuleTech.Region rg=ModuleTech.Region.UNSPEC;
            switch (val)
            {
                case 0:
                    rg = ModuleTech.Region.PRC;//中国
                    break;
                case 1:
                    rg = ModuleTech.Region.NA;//北美
                    break;
                case 2:
                    rg = ModuleTech.Region.JP;//日本
                    break;
                case 3:
                    rg = ModuleTech.Region.KR;//韩国
                    break;
                case 4:
                    rg = ModuleTech.Region.EU3;//欧洲
                    break;
                case 5:
                    rg = ModuleTech.Region.IN;//印度
                    break;
                //case 6:
                //    {
                //        return rg;
                //    }
                //    break;
                case 7:
                    rg = ModuleTech.Region.OPEN;//全频段
                    break;
                case 8:
                    rg = ModuleTech.Region.PRC2;//中国2
                    break;
            }
            return rg;
        }

        private void tbwifissid_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_mpfrm_MouseHover(object sender, EventArgs e)
        {
            ForeColors(Color.Red);
        }

        private void button_mpfrm_MouseLeave(object sender, EventArgs e)
        {
            ForeColors(SystemColors.ControlText);
        }
        public void ForeColors(Color color) {
            label2.ForeColor = color;
            label11.ForeColor = color;
            label10.ForeColor = color;
            label12.ForeColor = color;
            label12.ForeColor = color;
            label15.ForeColor = color;
            label17.ForeColor = color;

            cbbregion.ForeColor = color;
            lvhoptb.ForeColor = color;
        }

        int hopanttime = 0;
        int baud = 0;
        int isdet = 0;
        int frenqencyTime = 0;
        bool autoApp = false;
        int HopFrequencyMode = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                hopanttime = (int)rdr.ParamGet("HopAntTime");
                this.tbhopanttime.Text = hopanttime.ToString();
            }
            catch (Exception)
            {
                this.tbhopanttime.Enabled = false;
            }

            try
            {
                var _isdet = (bool)rdr.ParamGet("CheckAntConnection");
                isdet = _isdet ? 0 : 1;
                cbbdetectants.SelectedIndex = isdet;
            }
            catch (Exception)
            {
            }


            try
            {
                bool isR2000 = rdr.HwDetails.module.ToString().IndexOf("SLR") != -1;//是否是 R2000
                if (isR2000)
                    cbbhopfremode.Enabled = true;
                else
                    cbbhopfremode.Enabled = false;
            }
            catch (Exception)
            {
            }

            try
            {
                if (rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_ARM7 ||
                    rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_SERIAL ||
                    rdr.HwDetails.board == Reader.MaindBoard_Type.MAINBOARD_WIFI)
                {
                    HopFrequencyMode = (int)rdr.ParamGet("HopFrequencyMode");
                    cbbhopfremode.SelectedIndex = HopFrequencyMode;

                    baud = (int)rdr.ParamGet("ModuleSave_Baudrate");
                    comboBox_mpbaud.SelectedIndex = comboBox_mpbaud.Items.IndexOf(baud.ToString());

                    frenqencyTime = (int)rdr.ParamGet("ModuleSave_Frenqency_time");
                    textBox_fretime.Text = frenqencyTime != -1 ? frenqencyTime.ToString() : "400";

                    autoApp = (bool)rdr.ParamGet("ModuleSave_AutoApp");
                    if (autoApp)
                        radioButton_mpautoyes.Checked = true;
                    else
                        radioButton_mpautono.Checked = true;
                }
                else
                {
                    groupBox3.Enabled = false;

                    cbbhopfremode.Enabled = false;
                    comboBox_mpbaud.Enabled = false;
                    textBox_fretime.Enabled = false;

                    radioButton_mpautono.Enabled = false;
                    radioButton_mpautoyes.Enabled = false;
                }
            }
            catch (Exception)
            {
            }
        }

        bool isSorting = false;
        private void lvhoptb_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (isSorting)
            {
                
            }
            else {

            }

        }

        private void button_default_Click(object sender, EventArgs e)
        {
            try
            {
                rdr.ParamSet("ModuleSave_default", true);
                MessageBox.Show("恢复出厂默认完成，关闭程序，请断电5秒后再重新打开程序连接读写器");
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception mex)
            {
                MessageBox.Show("保存失败 " + mex.Message);
                return;
            }

            MessageBox.Show("保存成功");
        }

        private void button_readytoupdate_Click(object sender, EventArgs e)
        {
            try
            {
                rdr.ParamSet("ModuleSave_readytoupdate", true);

            }
            catch (Exception mex)
            {
                MessageBox.Show("保存失败 " + mex.Message);
                return;
            }

            MessageBox.Show("保存成功");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(hopanttime!= int.Parse(this.tbhopanttime.Text.Trim())&& tbhopanttime.Enabled)
                rdr.ParamSet("HopAntTime", int.Parse(this.tbhopanttime.Text.Trim()));

            if (isdet!= this.cbbdetectants.SelectedIndex&& cbbdetectants.Enabled)
                rdr.ParamSet("CheckAntConnection", this.cbbdetectants.SelectedIndex==0?true:false);

            if(HopFrequencyMode != cbbhopfremode.SelectedIndex&& cbbhopfremode.Enabled)
                rdr.ParamSet("HopFrequencyMode", cbbhopfremode.SelectedIndex);

            button2.PerformClick();
        }
    }
}