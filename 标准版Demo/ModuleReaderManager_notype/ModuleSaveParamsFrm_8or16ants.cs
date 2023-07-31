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
    public partial class ModuleSaveParamsFrm_8or16ants : Form
    {
        public ModuleSaveParamsFrm_8or16ants(ReaderParams paras, Reader rd)
        {
            InitializeComponent();
            m_params = paras;
            rdr = rd;
        }
        ReaderParams m_params;
        Reader rdr;

        private void button_saveantport_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> li = new List<int>();
                if (checkBox_ant1.Checked)
                    li.Add(1);
                if (checkBox_ant2.Checked)
                    li.Add(2);
                if (checkBox_ant3.Checked)
                    li.Add(3);
                if (checkBox_ant4.Checked)
                    li.Add(4);
                if (checkBox_ant5.Checked)
                    li.Add(5);
                if (checkBox_ant6.Checked)
                    li.Add(6);
                if (checkBox_ant7.Checked)
                    li.Add(7);
                if (checkBox_ant8.Checked)
                    li.Add(8);

                if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5900 ||
            rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR6100)
                {
                    if (checkBox_ant9.Checked)
                        li.Add(9);
                    if (checkBox_ant10.Checked)
                        li.Add(10);
                    if (checkBox_ant11.Checked)
                        li.Add(11);
                    if (checkBox_ant12.Checked)
                        li.Add(12);
                    if (checkBox_ant13.Checked)
                        li.Add(13);
                    if (checkBox_ant14.Checked)
                        li.Add(14);
                    if (checkBox_ant15.Checked)
                        li.Add(15);
                    if (checkBox_ant16.Checked)
                        li.Add(16);
                }
                rdr.ParamSet("ModuleSave_Ant_port", li.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
                return;
            }

            MessageBox.Show("保存成功");
        }

        private void button_getantpw_Click(object sender, EventArgs e)
        {
            textBox_ant1rpow.Text = "";
            textBox_ant1wpow.Text = "";
            textBox_ant1time.Text = "";

            textBox_ant2rpow.Text = "";
            textBox_ant2wpow.Text = "";
            textBox_ant2time.Text = "";

            textBox_ant3rpow.Text = "";
            textBox_ant3wpow.Text = "";
            textBox_ant3time.Text = "";

            textBox_ant4rpow.Text = "";
            textBox_ant4wpow.Text = "";
            textBox_ant4time.Text = "";

            textBox_ant5rpow.Text = "";
            textBox_ant5wpow.Text = "";
            textBox_ant5time.Text = "";

            textBox_ant6rpow.Text = "";
            textBox_ant6wpow.Text = "";
            textBox_ant6time.Text = "";

            textBox_ant7rpow.Text = "";
            textBox_ant7wpow.Text = "";
            textBox_ant7time.Text = "";

            textBox_ant8rpow.Text = "";
            textBox_ant8wpow.Text = "";
            textBox_ant8time.Text = "";

            textBox_ant9rpow.Text = "";
            textBox_ant9wpow.Text = "";
            textBox_ant9time.Text = "";

            textBox_ant10rpow.Text = "";
            textBox_ant10wpow.Text = "";
            textBox_ant10time.Text = "";

            textBox_ant11rpow.Text = "";
            textBox_ant11wpow.Text = "";
            textBox_ant11time.Text = "";

            textBox_ant12rpow.Text = "";
            textBox_ant12wpow.Text = "";
            textBox_ant12time.Text = "";

            textBox_ant13rpow.Text = "";
            textBox_ant13wpow.Text = "";
            textBox_ant13time.Text = "";

            textBox_ant14rpow.Text = "";
            textBox_ant14wpow.Text = "";
            textBox_ant14time.Text = "";

            textBox_ant15rpow.Text = "";
            textBox_ant15wpow.Text = "";
            textBox_ant15time.Text = "";

            textBox_ant16rpow.Text = "";
            textBox_ant16wpow.Text = "";
            textBox_ant16time.Text = "";

            try
            {
                ushort[] vals = (ushort[])rdr.ParamGet("ModuleSave_Ant_Power_Time");
                if (vals[0]!=0x5a&&vals[5] != 0xffff)
                {
                    
                        textBox_ant1rpow.Text = vals[5].ToString();
                        textBox_ant1wpow.Text = vals[6].ToString();
                        textBox_ant1time.Text = vals[7].ToString();

                        textBox_ant2rpow.Text = vals[9].ToString();
                        textBox_ant2wpow.Text = vals[10].ToString();
                        textBox_ant2time.Text = vals[11].ToString();

                        textBox_ant3rpow.Text = vals[13].ToString();
                        textBox_ant3wpow.Text = vals[14].ToString();
                        textBox_ant3time.Text = vals[15].ToString();

                        textBox_ant4rpow.Text = vals[17].ToString();
                        textBox_ant4wpow.Text = vals[18].ToString();
                        textBox_ant4time.Text = vals[19].ToString();

                        textBox_ant5rpow.Text = vals[21].ToString();
                        textBox_ant5wpow.Text = vals[22].ToString();
                        textBox_ant5time.Text = vals[23].ToString();

                        textBox_ant6rpow.Text = vals[25].ToString();
                        textBox_ant6wpow.Text = vals[26].ToString();
                        textBox_ant6time.Text = vals[27].ToString();

                        textBox_ant7rpow.Text = vals[29].ToString();
                        textBox_ant7wpow.Text = vals[30].ToString();
                        textBox_ant7time.Text = vals[31].ToString();

                        textBox_ant8rpow.Text = vals[33].ToString();
                        textBox_ant8wpow.Text = vals[34].ToString();
                        textBox_ant8time.Text = vals[35].ToString();
                    
                    if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5900 ||
                        rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7400 ||
                        rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR6100)
                    {
                        
                            textBox_ant9rpow.Text = vals[37].ToString();
                            textBox_ant9wpow.Text = vals[38].ToString();
                            textBox_ant9time.Text = vals[39].ToString();

                            textBox_ant10rpow.Text = vals[41].ToString();
                            textBox_ant10wpow.Text = vals[42].ToString();
                            textBox_ant10time.Text = vals[43].ToString();

                            textBox_ant11rpow.Text = vals[45].ToString();
                            textBox_ant11wpow.Text = vals[46].ToString();
                            textBox_ant11time.Text = vals[47].ToString();

                            textBox_ant12rpow.Text = vals[49].ToString();
                            textBox_ant12wpow.Text = vals[50].ToString();
                            textBox_ant12time.Text = vals[51].ToString();

                            textBox_ant13rpow.Text = vals[53].ToString();
                            textBox_ant13wpow.Text = vals[54].ToString();
                            textBox_ant13time.Text = vals[55].ToString();

                            textBox_ant14rpow.Text = vals[57].ToString();
                            textBox_ant14wpow.Text = vals[58].ToString();
                            textBox_ant14time.Text = vals[59].ToString();

                            textBox_ant15rpow.Text = vals[61].ToString();
                            textBox_ant15wpow.Text = vals[62].ToString();
                            textBox_ant15time.Text = vals[63].ToString();

                            textBox_ant16rpow.Text = vals[65].ToString();
                            textBox_ant16wpow.Text = vals[66].ToString();
                            textBox_ant16time.Text = vals[67].ToString();
                        
                    }
                }
                else
                {

                    textBox_ant1rpow.Text = "2000";
                    textBox_ant1wpow.Text = "2000";
                    textBox_ant1time.Text = "0";

                    textBox_ant2rpow.Text = "2000";
                    textBox_ant2wpow.Text = "2000";
                    textBox_ant2time.Text = "0";

                    textBox_ant3rpow.Text = "2000";
                    textBox_ant3wpow.Text = "2000";
                    textBox_ant3time.Text = "0";

                    textBox_ant4rpow.Text = "2000";
                    textBox_ant4wpow.Text = "2000";
                    textBox_ant4time.Text = "0";

                    textBox_ant5rpow.Text = "2000";
                    textBox_ant5wpow.Text = "2000";
                    textBox_ant5time.Text = "0";

                    textBox_ant6rpow.Text = "2000";
                    textBox_ant6wpow.Text = "2000";
                    textBox_ant6time.Text = "0";

                    textBox_ant7rpow.Text = "2000";
                    textBox_ant7wpow.Text = "2000";
                    textBox_ant7time.Text = "0";

                    textBox_ant8rpow.Text = "2000";
                    textBox_ant8wpow.Text = "2000";
                    textBox_ant8time.Text = "0";

                    textBox_ant9rpow.Text = "2000";
                    textBox_ant9wpow.Text = "2000";
                    textBox_ant9time.Text = "0";

                    textBox_ant10rpow.Text = "2000";
                    textBox_ant10wpow.Text = "2000";
                    textBox_ant10time.Text = "0";

                    textBox_ant11rpow.Text = "2000";
                    textBox_ant11wpow.Text = "2000";
                    textBox_ant11time.Text = "0";

                    textBox_ant12rpow.Text = "2000";
                    textBox_ant12wpow.Text = "2000";
                    textBox_ant12time.Text = "0";

                    textBox_ant13rpow.Text = "2000";
                    textBox_ant13wpow.Text = "2000";
                    textBox_ant13time.Text = "0";

                    textBox_ant14rpow.Text = "2000";
                    textBox_ant14wpow.Text = "2000";
                    textBox_ant14time.Text = "0";

                    textBox_ant15rpow.Text = "2000";
                    textBox_ant15wpow.Text = "2000";
                    textBox_ant15time.Text = "0";

                    textBox_ant16rpow.Text = "2000";
                    textBox_ant16wpow.Text = "2000";
                    textBox_ant16time.Text = "0";
                }

                checkBox_ant1.Checked = false;
                checkBox_ant2.Checked = false;
                checkBox_ant3.Checked = false;
                checkBox_ant4.Checked = false;

                checkBox_ant5.Checked = false;
                checkBox_ant6.Checked = false;
                checkBox_ant7.Checked = false;
                checkBox_ant8.Checked = false;

                checkBox_ant9.Checked = false;
                checkBox_ant10.Checked = false;
                checkBox_ant11.Checked = false;
                checkBox_ant12.Checked = false;

                checkBox_ant13.Checked = false;
                checkBox_ant14.Checked = false;
                checkBox_ant15.Checked = false;
                checkBox_ant16.Checked = false;

                if (vals.Length >= 36)
                {
                    //if (vals[3] != 0xff)
                    {
                        byte bt = (byte)vals[3];
                        if ((bt & 0x01) != 0)
                            checkBox_ant1.Checked = true;
                        if ((bt & 0x02) != 0)
                            checkBox_ant2.Checked = true;
                        if ((bt & 0x04) != 0)
                            checkBox_ant3.Checked = true;
                        if ((bt & 0x08) != 0)
                            checkBox_ant4.Checked = true;
                        if ((bt & 0x10) != 0)
                            checkBox_ant5.Checked = true;
                        if ((bt & 0x20) != 0)
                            checkBox_ant6.Checked = true;
                        if ((bt & 0x40) != 0)
                            checkBox_ant7.Checked = true;
                        if ((bt & 0x80) != 0)
                            checkBox_ant8.Checked = true;
                    }

                    if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5900 ||
            rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR6100)
                    {
                       
                        //if (vals[2] != 0xff)
                        {
                            byte bt = (byte)(vals[2]);
                            if ((bt & 0x01) != 0)
                                checkBox_ant9.Checked = true;
                            if ((bt & 0x02) != 0)
                                checkBox_ant10.Checked = true;
                            if ((bt & 0x04) != 0)
                                checkBox_ant11.Checked = true;
                            if ((bt & 0x08) != 0)
                                checkBox_ant12.Checked = true;
                            if ((bt & 0x10) != 0)
                                checkBox_ant13.Checked = true;
                            if ((bt & 0x20) != 0)
                                checkBox_ant14.Checked = true;
                            if ((bt & 0x40) != 0)
                                checkBox_ant15.Checked = true;
                            if ((bt & 0x80) != 0)
                                checkBox_ant16.Checked = true;
                        }

                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取失败:" + ex.Message);
                return;
            }
        }

        private void button_antpwdef_Click(object sender, EventArgs e)
        {
             try
            {
                ushort[] vals = null;
                rdr.ParamSet("ModuleSave_Ant_Power_Time", vals);

                textBox_ant1rpow.Text = "2000";
                textBox_ant1wpow.Text = "2000";
                textBox_ant1time.Text = "0";

                textBox_ant2rpow.Text = "2000";
                textBox_ant2wpow.Text = "2000";
                textBox_ant2time.Text = "0";

                textBox_ant3rpow.Text = "2000";
                textBox_ant3wpow.Text = "2000";
                textBox_ant3time.Text = "0";

                textBox_ant4rpow.Text = "2000";
                textBox_ant4wpow.Text = "2000";
                textBox_ant4time.Text = "0";

                textBox_ant5rpow.Text = "2000";
                textBox_ant5wpow.Text = "2000";
                textBox_ant5time.Text = "0";

                textBox_ant6rpow.Text = "2000";
                textBox_ant6wpow.Text = "2000";
                textBox_ant6time.Text = "0";

                textBox_ant7rpow.Text = "2000";
                textBox_ant7wpow.Text = "2000";
                textBox_ant7time.Text = "0";

                textBox_ant8rpow.Text = "2000";
                textBox_ant8wpow.Text = "2000";
                textBox_ant8time.Text = "0";

                textBox_ant9rpow.Text = "2000";
                textBox_ant9wpow.Text = "2000";
                textBox_ant9time.Text = "0";

                textBox_ant10rpow.Text = "2000";
                textBox_ant10wpow.Text = "2000";
                textBox_ant10time.Text = "0";

                textBox_ant11rpow.Text = "2000";
                textBox_ant11wpow.Text = "2000";
                textBox_ant11time.Text = "0";

                textBox_ant12rpow.Text = "2000";
                textBox_ant12wpow.Text = "2000";
                textBox_ant12time.Text = "0";

                textBox_ant13rpow.Text = "2000";
                textBox_ant13wpow.Text = "2000";
                textBox_ant13time.Text = "0";

                textBox_ant14rpow.Text = "2000";
                textBox_ant14wpow.Text = "2000";
                textBox_ant14time.Text = "0";

                textBox_ant15rpow.Text = "2000";
                textBox_ant15wpow.Text = "2000";
                textBox_ant15time.Text = "0";

                textBox_ant16rpow.Text = "2000";
                textBox_ant16wpow.Text = "2000";
                textBox_ant16time.Text = "0";

                checkBox_ant1.Checked = false;
                checkBox_ant2.Checked = false;
                checkBox_ant3.Checked = false;
                checkBox_ant4.Checked = false;

                checkBox_ant5.Checked = false;
                checkBox_ant6.Checked = false;
                checkBox_ant7.Checked = false;
                checkBox_ant8.Checked = false;

                checkBox_ant9.Checked = false;
                checkBox_ant10.Checked = false;
                checkBox_ant11.Checked = false;
                checkBox_ant12.Checked = false;

                checkBox_ant13.Checked = false;
                checkBox_ant14.Checked = false;
                checkBox_ant15.Checked = false;
                checkBox_ant16.Checked = false;
            }
             catch (Exception ex)
             {
                 MessageBox.Show("保存失败:" + ex.Message);
                 return;
             }
             MessageBox.Show("保存成功");
        }

        private void button_setantpw_Click(object sender, EventArgs e)
        {
            try
            {
                ushort[] vals = null;

                if (textBox_ant1rpow.Text == "" || textBox_ant1wpow.Text == "" || textBox_ant1time.Text == "")
                    throw new Exception("无效值");
                if (textBox_ant2rpow.Text == "" || textBox_ant2wpow.Text == "" || textBox_ant2time.Text == "")
                    throw new Exception("无效值");
                if (textBox_ant3rpow.Text == "" || textBox_ant3wpow.Text == "" || textBox_ant3time.Text == "")
                    throw new Exception("无效值");
                if (textBox_ant4rpow.Text == "" || textBox_ant4wpow.Text == "" || textBox_ant4time.Text == "")
                    throw new Exception("无效值");
                if (textBox_ant5rpow.Text == "" || textBox_ant5wpow.Text == "" || textBox_ant5time.Text == "")
                    throw new Exception("无效值");
                if (textBox_ant6rpow.Text == "" || textBox_ant6wpow.Text == "" || textBox_ant6time.Text == "")
                    throw new Exception("无效值");
                if (textBox_ant7rpow.Text == "" || textBox_ant7wpow.Text == "" || textBox_ant7time.Text == "")
                    throw new Exception("无效值");
                if (textBox_ant8rpow.Text == "" || textBox_ant8wpow.Text == "" || textBox_ant8time.Text == "")
                    throw new Exception("无效值");

                if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5900 ||
               rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR6100)
                {
                    if (textBox_ant9rpow.Text == "" || textBox_ant9wpow.Text == "" || textBox_ant9time.Text == "")
                        throw new Exception("无效值");
                    if (textBox_ant10rpow.Text == "" || textBox_ant10wpow.Text == "" || textBox_ant10time.Text == "")
                        throw new Exception("无效值");
                    if (textBox_ant11rpow.Text == "" || textBox_ant11wpow.Text == "" || textBox_ant11time.Text == "")
                        throw new Exception("无效值");
                    if (textBox_ant12rpow.Text == "" || textBox_ant12wpow.Text == "" || textBox_ant12time.Text == "")
                        throw new Exception("无效值");
                    if (textBox_ant13rpow.Text == "" || textBox_ant13wpow.Text == "" || textBox_ant13time.Text == "")
                        throw new Exception("无效值");
                    if (textBox_ant14rpow.Text == "" || textBox_ant14wpow.Text == "" || textBox_ant14time.Text == "")
                        throw new Exception("无效值");
                    if (textBox_ant15rpow.Text == "" || textBox_ant15wpow.Text == "" || textBox_ant15time.Text == "")
                        throw new Exception("无效值");
                    if (textBox_ant16rpow.Text == "" || textBox_ant16wpow.Text == "" || textBox_ant16time.Text == "")
                        throw new Exception("无效值");
                }

                if (m_params.antcnt == 16)
                    vals = new ushort[48];
                else
                    vals = new ushort[24];
                vals[0] = ushort.Parse(textBox_ant1rpow.Text);
                vals[1] = ushort.Parse(textBox_ant1wpow.Text);
                vals[2] = ushort.Parse(textBox_ant1time.Text);

                vals[3] = ushort.Parse(textBox_ant2rpow.Text);
                vals[4] = ushort.Parse(textBox_ant2wpow.Text);
                vals[5] = ushort.Parse(textBox_ant2time.Text);

                vals[6] = ushort.Parse(textBox_ant3rpow.Text);
                vals[7] = ushort.Parse(textBox_ant3wpow.Text);
                vals[8] = ushort.Parse(textBox_ant3time.Text);

                vals[9] = ushort.Parse(textBox_ant4rpow.Text);
                vals[10] = ushort.Parse(textBox_ant4wpow.Text);
                vals[11] = ushort.Parse(textBox_ant4time.Text);

                vals[12] = ushort.Parse(textBox_ant5rpow.Text);
                vals[13] = ushort.Parse(textBox_ant5wpow.Text);
                vals[14] = ushort.Parse(textBox_ant5time.Text);

                vals[15] = ushort.Parse(textBox_ant6rpow.Text);
                vals[16] = ushort.Parse(textBox_ant6wpow.Text);
                vals[17] = ushort.Parse(textBox_ant6time.Text);

                vals[18] = ushort.Parse(textBox_ant7rpow.Text);
                vals[19] = ushort.Parse(textBox_ant7wpow.Text);
                vals[20] = ushort.Parse(textBox_ant7time.Text);

                vals[21] = ushort.Parse(textBox_ant8rpow.Text);
                vals[22] = ushort.Parse(textBox_ant8wpow.Text);
                vals[23] = ushort.Parse(textBox_ant8time.Text);


                if (rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR5900 ||
                    rdr.HwDetails.module == Reader.Module_Type.MODOULE_SIM7400 ||
                    rdr.HwDetails.module == Reader.Module_Type.MODOULE_SLR6100)
                {
                    vals[24] = ushort.Parse(textBox_ant9rpow.Text);
                    vals[25] = ushort.Parse(textBox_ant9wpow.Text);
                    vals[26] = ushort.Parse(textBox_ant9time.Text);

                    vals[27] = ushort.Parse(textBox_ant10rpow.Text);
                    vals[28] = ushort.Parse(textBox_ant10wpow.Text);
                    vals[29] = ushort.Parse(textBox_ant10time.Text);

                    vals[30] = ushort.Parse(textBox_ant11rpow.Text);
                    vals[31] = ushort.Parse(textBox_ant11wpow.Text);
                    vals[32] = ushort.Parse(textBox_ant11time.Text);

                    vals[33] = ushort.Parse(textBox_ant12rpow.Text);
                    vals[34] = ushort.Parse(textBox_ant12wpow.Text);
                    vals[35] = ushort.Parse(textBox_ant12time.Text);

                    vals[36] = ushort.Parse(textBox_ant13rpow.Text);
                    vals[37] = ushort.Parse(textBox_ant13wpow.Text);
                    vals[38] = ushort.Parse(textBox_ant13time.Text);

                    vals[39] = ushort.Parse(textBox_ant14rpow.Text);
                    vals[40] = ushort.Parse(textBox_ant14wpow.Text);
                    vals[41] = ushort.Parse(textBox_ant14time.Text);

                    vals[42] = ushort.Parse(textBox_ant15rpow.Text);
                    vals[43] = ushort.Parse(textBox_ant15wpow.Text);
                    vals[44] = ushort.Parse(textBox_ant15time.Text);

                    vals[45] = ushort.Parse(textBox_ant16rpow.Text);
                    vals[46] = ushort.Parse(textBox_ant16wpow.Text);
                    vals[47] = ushort.Parse(textBox_ant16time.Text);
                }

                rdr.ParamSet("ModuleSave_Ant_Power_Time", vals);


            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
                return;
            }

            MessageBox.Show("保存成功");
        }

        private void ModuleSaveParamsFrm_8or16ants_Load(object sender, EventArgs e)
        {
            button_getantpw.PerformClick();
        }
    }
}
