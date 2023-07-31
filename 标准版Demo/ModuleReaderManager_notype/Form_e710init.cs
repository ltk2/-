using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ModuleTech;
namespace ModuleReaderManager
{
    public partial class Form_e710init : Form
    {
        public Form_e710init(ReaderParams paras, Reader rd)
        {
            InitializeComponent();
            m_params = paras;
            modulerdr = rd;
        }
        ReaderParams m_params;
        Reader modulerdr;
        private void button_set_Click(object sender, EventArgs e)
        {
            StringBuilder sbd = new StringBuilder();
            if (comboBox_region3.SelectedIndex == -1 || comboBox_sort4.SelectedIndex == -1)
            {
                MessageBox.Show("请选择区域和序号");
                return;
            }

            if (comboBox_writerule.SelectedIndex == 0)
            {
                if (comboBox_chip1.SelectedIndex == -1 || comboBox_antport2.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择芯片和天线口");
                    return;
                }


                int chip = 0;
                switch (comboBox_chip1.SelectedIndex)
                {
                    case 0:
                        chip = 0x00;
                        break;
                    case 1:
                        chip = 0x31;
                        break;
                    case 2:
                        chip = 0x32;
                        break;
                    case 3:
                        chip = 0x33;
                        break;
                    case 4:
                        chip = 0x34;
                        break;
                }

                sbd.Append(chip.ToString("X2"));

                int port = 0;

                switch (comboBox_antport2.SelectedIndex)
                {
                    case 0x00:

                    case 0x01:

                    case 0x02:

                    case 0x03:

                    case 0x04:

                    case 0x05:
                        port = comboBox_antport2.SelectedIndex;
                        break;
                    case 6:
                        port = 0x10;
                        break;
                    case 7:
                        port = 0x20;
                        break;
                }
                sbd.Append(port.ToString("X2"));
            }
            else
            {
                if (comboBox_rtype.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择类型");
                    return;
                }


                switch (comboBox_rtype.SelectedIndex)
                {
                    case 0:
                        sbd.Append("0000");
                        break;
                    case 1:
                        sbd.Append("3100");
                        break;
                    case 2:
                        sbd.Append("3102");
                        break;
                    case 3:
                        sbd.Append("3103");
                        break;
                    case 4:
                        sbd.Append("3104");
                        break;
                    case 5:
                        sbd.Append("3110");
                        break;
                    case 6:
                        sbd.Append("3120");
                        break;
                    case 7:
                        sbd.Append("3300");
                        break;
                    case 8:
                        sbd.Append("3302");
                        break;
                    case 9:
                        sbd.Append("3303");
                        break;
                    case 10:
                        sbd.Append("3304");
                        break;
                    case 11:
                        sbd.Append("3310");
                        break;
                    case 12:
                        sbd.Append("3320");
                        break;
                    case 13:
                        sbd.Append("3200");
                        break;
                    case 14:
                        sbd.Append("3202");
                        break;
                    case 15:
                        sbd.Append("3203");
                        break;
                    case 16:
                        sbd.Append("3204");
                        break;
                    case 17:
                        sbd.Append("3210");
                        break;
                    case 18:
                        sbd.Append("3220");
                        break;
                }
            }

            int region = comboBox_region3.SelectedIndex;
            if (comboBox_region3.SelectedIndex == comboBox_region3.Items.Count - 1)
                region = 0xA1;
            sbd.Append(region.ToString("X2"));
            int sort = comboBox_sort4.SelectedIndex;
            sbd.Append(sort.ToString("X2"));

            int val = int.Parse(sbd.ToString(), System.Globalization.NumberStyles.HexNumber);
            ushort addr = 0x00000001;
            R2000_calibration r2000pcmd = new R2000_calibration();
            R2000_calibration.R2000cmd rcmdo = new R2000_calibration.R2000cmd();

            R2000_calibration.OEM_DATA r2000oem = null;

            rcmdo = R2000_calibration.R2000cmd.OEMwrite;
            r2000oem = new R2000_calibration.OEM_DATA(addr, val);
            try
            {
                byte[] senddata = null;

                senddata = r2000pcmd.GetSendCmd(rcmdo, r2000oem.ToByteData());

                byte[] revdata = modulerdr.SendandRev(senddata, 1000);

                MessageBox.Show("操作完成,请重新上电读写器");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("操作失败:" + ex.Message);
                return;
            }
        }

        private void button_get_Click(object sender, EventArgs e)
        {
            label_chiptype.Text = "";
            R2000_calibration r2000pcmd = new R2000_calibration();
            R2000_calibration.R2000cmd rcmdo = new R2000_calibration.R2000cmd();

            R2000_calibration.OEM_DATA r2000oem = null;

            rcmdo = R2000_calibration.R2000cmd.OEMread;
            r2000oem = new R2000_calibration.OEM_DATA(0x00000001);

            byte[] senddata = null;

            senddata = r2000pcmd.GetSendCmd(rcmdo, r2000oem.ToByteData());


            byte[] revdata = modulerdr.SendandRev(senddata, 1000);

            byte[] data = new byte[revdata.Length - 19];
            Array.Copy(revdata, 17, data, 0, data.Length);
            R2000_calibration.OEM_DATA r2000data = new R2000_calibration.OEM_DATA(data);

            R2000_calibration.OEM_DATA.Adpair[] adp = r2000data.GetAddr();

            label_chiptype.Text = adp[0].val.ToString("X4");
            if (label_chiptype.Text.Length < 8)
                label_chiptype.Text = label_chiptype.Text.PadLeft(8, '0');
            string bdata= adp[0].val.ToString("X4");
            if (bdata.Length < 8)
                bdata = bdata.PadLeft(8, '0');

            string rtype = bdata.Substring(0, 4);
            int chip = int.Parse(bdata.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int port = int.Parse(bdata.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int region = int.Parse(bdata.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            int sort = int.Parse(bdata.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            switch (chip)
            {
                case 0x00:
                    comboBox_chip1.SelectedIndex = 0;
                    break;
                case 0x31:
                    comboBox_chip1.SelectedIndex = 1;
                    break;
                case 0x32:
                    comboBox_chip1.SelectedIndex = 2;
                    break;
                case 0x33:
                    comboBox_chip1.SelectedIndex = 3;
                    break;
                case 0x34:
                    comboBox_chip1.SelectedIndex = 4;
                    break;
            }

            switch (port)
            {
                case 0x00:
                   
                case 0x01:
                   
                case 0x02:
                  
                case 0x03:
                   
                case 0x04:
                
                case 0x05:
                    comboBox_antport2.SelectedIndex = port;
                    break;
                case 0x10:
                    comboBox_antport2.SelectedIndex = 6;
                    break;
                case 0x20:
                    comboBox_antport2.SelectedIndex = 7;
                    break;
            }

            if(region<=23)
            comboBox_region3.SelectedIndex=region;


            var s = comboBox_sort4.Items.Count;
            comboBox_sort4.SelectedIndex = sort;

            switch (rtype)
            {
                case "0000":
                    comboBox_rtype.SelectedIndex = 0;
                    break;
                case "3100":
                    comboBox_rtype.SelectedIndex = 1;
                    break;
                case "3102":
                    comboBox_rtype.SelectedIndex = 2;
                    break;
                case "3103":
                    comboBox_rtype.SelectedIndex = 3;
                    break;
                case "3104":
                    comboBox_rtype.SelectedIndex = 4;
                    break;
                case "3110":
                    comboBox_rtype.SelectedIndex = 5;
                    break;
                case "3120":
                    comboBox_rtype.SelectedIndex = 6;
                    break;
                case "3300":
                    comboBox_rtype.SelectedIndex = 7;
                    break;
                case "3302":
                    comboBox_rtype.SelectedIndex = 8;
                    break;
                case "3303":
                    comboBox_rtype.SelectedIndex = 9;
                    break;
                case "3304":
                    comboBox_rtype.SelectedIndex = 10;
                    break;
                case "3310":
                    comboBox_rtype.SelectedIndex = 11;
                    break;
                case "3320":
                    comboBox_rtype.SelectedIndex = 12;
                    break;
                case "3200":
                    comboBox_rtype.SelectedIndex = 13;
                    break;
                case "3202":
                    comboBox_rtype.SelectedIndex = 14;
                    break;
                case "3203":
                    comboBox_rtype.SelectedIndex = 15;
                    break;
                case "3204":
                    comboBox_rtype.SelectedIndex = 16;
                    break;
                case "3210":
                    comboBox_rtype.SelectedIndex = 17;
                    break;
                case "3220":
                    comboBox_rtype.SelectedIndex = 18;
                    break;

            }

        }

        private void Form_e710init_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 256; i++)
            {
                comboBox_sort4.Items.Add(i.ToString("X2"));
            }

            //comboBox_chip1.SelectedIndex = 0;
            //comboBox_antport2.SelectedIndex = 0;
            //comboBox_region3.SelectedIndex = 0;
            //comboBox_sort4.SelectedIndex = 0;

            comboBox_writerule.SelectedIndex = 0;

            try
            {
                modulerdr.SendandRev(new byte[] {0xff,0x00,0x04,0x1d,0x0b}, 1000);
            }
            catch(Exception ex)
            {
                MessageBox.Show("未能启动固件:"+ex.Message+ex.StackTrace);

            }
        }
    }
}
