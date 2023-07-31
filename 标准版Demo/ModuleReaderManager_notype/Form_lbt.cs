using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ModuleTech;

namespace ModuleReaderManager
{
    public partial class Form_lbt : Form
    {
        public Form_lbt(Reader rdr, ReaderParams param, Form1 frm)
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

        private void Form_lbt_Load(object sender, EventArgs e)
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

            comboBox_rbregion.SelectedIndex = 1;
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
        private bool Testreturnlost_lbt(int v)
        {
            bool bl = true;
            List<int> lfre = new List<int>();
           
            List<AntAndBoll> selants = null;

            selants = CheckAntsValid();
            if (selants.Count == 0)
            {
                throw new Exception("请选择天线");
            }

            for (int i = 0; i < selants.Count; ++i)
            {
                List<int> lant = new List<int>();
                lant.Add(selants[i].antid);
  
                if (comboBox_rbregion.SelectedIndex == -1)
                    throw new Exception("选择区域");

                ThingMagic.Reader.Region trr = ThingMagic.Reader.Region.NA;
                switch (comboBox_rbregion.SelectedIndex)
                {
                    case 0:
                        trr = ThingMagic.Reader.Region.NA;
                        break;
                    case 1:
                        trr = ThingMagic.Reader.Region.PRC;
                        break;
                    case 2:
                        trr = ThingMagic.Reader.Region.EU3;
                        break;
                    case 3:
                        trr = ThingMagic.Reader.Region.PRC2;
                        break;
                    case 4:
                        trr = ThingMagic.Reader.Region.OPEN;
                        break;
                }

                if (!allcheckBox.Checked)
                {
                    for (int j = 0; j < lvhoptb.Items.Count; j++)
                    {
                        if (lvhoptb.Items[j].Checked)
                        {
                            lfre.Add(int.Parse(lvhoptb.Items[j].Text));
                        }
                    }
                }


                R2000_calibration.VSWRReturnloss_DATA et = new R2000_calibration.VSWRReturnloss_DATA(0, lfre.ToArray(), lant.ToArray(), trr);
                R2000_calibration r2000pcmd = new R2000_calibration();
                byte[] senddata = r2000pcmd.GetSendCmd(R2000_calibration.R2000cmd.LBT, et.ToByteData());
                byte[] revdata = mordr.SendandRev(senddata, 15000);

                byte[] data = new byte[revdata.Length - 19];
                Array.Copy(revdata, 17, data, 0, data.Length);
                R2000_calibration.VSWRReturnloss_DATA et2 = null;
                et2 = new R2000_calibration.VSWRReturnloss_DATA(data);


                for (int k = 0; k < et2.Llbt.Count; k++)
                    if (et2.Llbt[k] >= v)
                    { bl = false; break; }


                if (bl == false)
                    break;
            }

            return bl;
        }

        private void button_chklbt_Click(object sender, EventArgs e)
        {
            bool bl=true;
            try
            {
                int v=int.Parse(textBox_chkval.Text);
           
                bl = Testreturnlost_lbt(v);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("操作失败:" + ex.Message);
                return;
            }

            if(bl)
                MessageBox.Show("未检测到读写器信号");
            else
            MessageBox.Show("周围有读写器信号");
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

        int[] NAfrelist = new int[]{915750,915250,903250,926750,926250,904250,927250,920250,919250,909250, 
                        918750,917750,905250,904750,925250,921750,914750,906750,913750,922250, 
                911250,911750,903750,908750,905750,912250,906250,917250,914250,907250, 
                918250,916250,910250,910750,907750,924750,909750,919750,916750,913250, 
                923750,908250,925750,912750,924250,921250,920750,922750,902750,923250};
        //中国 1 跳频表： 
        int[] Chinafrelist1 = new int[]{921375,922625,920875,923625,921125,920625,923125,921625, 
                      922125,923875,921875,922875,924125,923375,924375,922375};
        //欧频跳频表： 
        int[] Eu3frelist = new int[] { 865700, 866300, 866900, 867500 };
        //中国 2 跳频表： 
        int[] Chinafrelist2 = new int[]{841375,842625,840875,843625,841125,840625,843125,841625, 
                      842125,843875,841875,842875,844125,843375,844375,842375};
        //全频段跳频表： 
        int[] Allfrelist = new int[]{840000,850000,860000,870000,880000,890000,900000, 
                910000,920000,930000,940000,950000,960000};



        public void Sort(ref int[] array)
        {
            int tmpIntValue = 0;
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

        private void comboBox_rbregion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_rbregion.SelectedIndex != -1)
            {
                int[] htb = null;
                switch (comboBox_rbregion.SelectedIndex)
                {
                    case 0:
                        htb = NAfrelist;
                        break;
                    case 1:
                        htb = Chinafrelist1;
                        break;
                    case 2:
                        htb = Eu3frelist;
                        break;
                    case 3:
                        htb = Chinafrelist2;
                        break;
                    case 4:
                        htb = Allfrelist;
                        break;
                }


                Sort(ref htb);

                int cnt = 0;
                int curchal = htb[htb.Length - 1];
                lvhoptb.Items.Clear();
                foreach (uint fre in htb)
                {
                    cnt++;
                    ListViewItem item = new ListViewItem(fre.ToString());
                    item.Checked = true;
                    lvhoptb.Items.Add(item);
                }
                allcheckBox.Checked = true;
            }
        }

        private void button_dwlist_Click(object sender, EventArgs e)
        {
            if (allcheckBox.Visible)
            {
                allcheckBox.Visible = false;
                lvhoptb.Visible = false;
            }
            else
            {
                allcheckBox.Visible = true;
                lvhoptb.Visible = true;
            }
        }
    }
}
