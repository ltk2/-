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
    public partial class Form_onekey : Form
    {
        public Form_onekey(ReaderParams paras, Reader rd)
        {
            InitializeComponent();
            m_params = paras;
            modulerdr = rd;
            VisSet = false;
        }
        ReaderParams m_params;
        Reader modulerdr;
        public bool VisSet;
        private byte[] buildM5eBootBootloader()
        {
            return new byte[] { 0xFF, 0x00, 0x09, 0x1D, 0x06 };
        }
        private void button_stdwacanlce_Click(object sender, EventArgs e)
        {
            try
            {
                R2000_calibration r2000pcmd = new R2000_calibration();
                R2000_calibration.R2000cmd rcmdo = new R2000_calibration.R2000cmd();

                R2000_calibration.OEM_DATA r2000oem = null;

                rcmdo = R2000_calibration.R2000cmd.OEMwrite;
                r2000oem = new R2000_calibration.OEM_DATA(0xc60, 0xb05);

                byte[] senddata = r2000pcmd.GetSendCmd(rcmdo, r2000oem.ToByteData());


                byte[] revdata = modulerdr.SendandRev(senddata, 1000);
                if (revdata[3] + revdata[4] != 0)
                {
                    throw new Exception("S1:"+revdata[3].ToString("X2")+revdata[4].ToString("X2"));
                }

                revdata = modulerdr.SendandRev(buildM5eBootBootloader(), 1000);
                if (revdata[3] + revdata[4] != 0)
                {
                    throw new Exception("B1:" + revdata[3].ToString("X2") + revdata[4].ToString("X2"));
                }
                modulerdr.Disconnect();
                VisSet = true;
                MessageBox.Show("设置完毕，请重连接读写器");
            }
            catch (System.Exception ex)
            {
                VisSet = false;
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                R2000_calibration r2000pcmd = new R2000_calibration();
                R2000_calibration.R2000cmd rcmdo = new R2000_calibration.R2000cmd();

                R2000_calibration.OEM_DATA r2000oem = null;

                rcmdo = R2000_calibration.R2000cmd.OEMwrite;
                r2000oem = new R2000_calibration.OEM_DATA(0xc60, 0);

                byte[] senddata = r2000pcmd.GetSendCmd(rcmdo, r2000oem.ToByteData());


                byte[] revdata = modulerdr.SendandRev(senddata, 1000);
                if (revdata[3] + revdata[4] != 0)
                {
                    throw new Exception("S1:" + revdata[3].ToString("X2") + revdata[4].ToString("X2"));
                }

                revdata = modulerdr.SendandRev(buildM5eBootBootloader(), 1000);
                if (revdata[3] + revdata[4] != 0)
                {
                    throw new Exception("B1:" + revdata[3].ToString("X2") + revdata[4].ToString("X2"));
                }
                modulerdr.Disconnect();
                VisSet = true;
                MessageBox.Show("设置完毕，请重连接读写器");
            }
            catch (System.Exception ex)
            {
                VisSet = false;
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void button_getcurvscheck_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("未连接读写器");
                return;
            }
            label_vscheck.Text = "";
            Application.DoEvents();
            try
            {
                R2000_calibration r2000pcmd = new R2000_calibration();
                R2000_calibration.R2000cmd rcmdo = new R2000_calibration.R2000cmd();

                R2000_calibration.OEM_DATA r2000oem = null;

                rcmdo = R2000_calibration.R2000cmd.OEMread;
                r2000oem = new R2000_calibration.OEM_DATA(0xc60);

                byte[] senddata = r2000pcmd.GetSendCmd(rcmdo, r2000oem.ToByteData());


                byte[] revdata = modulerdr.SendandRev(senddata, 1000);
                if (revdata[3] + revdata[4] != 0)
                {
                    throw new Exception("S1:" + revdata[3].ToString("X2") + revdata[4].ToString("X2"));
                }

                byte[] data = new byte[revdata.Length - 19];
                Array.Copy(revdata, 17, data, 0, data.Length);
                R2000_calibration.OEM_DATA r2000data = new R2000_calibration.OEM_DATA(data);

                R2000_calibration.OEM_DATA.Adpair[] adp = r2000data.GetAddr();

                if (adp[0].addr == 0xc60)
                {
                    if (adp[0].val == 0)
                    {
                        label_vscheck.Text = "开启";
                    }
                    else if (adp[0].val == 0xb05)
                    {
                        label_vscheck.Text = "关闭";
                    }
                    else
                        label_vscheck.Text = "未知";
                }

            }
            catch (System.Exception ex)
            {
               
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int val115 = 0;

                 R2000_calibration r2000pcmd = new R2000_calibration();
                short val = 0;
                byte[] data = new byte[3];
                data[0] = 0x07;
                data[1] = (byte)((0x115 & 0xff00) >> 8);
                data[2] = (byte)(0x115 & 0x00ff);
                byte[] senddata = r2000pcmd.GetSendCmd(R2000_calibration.R2000cmd.Regop, data);
                byte[] revdata = modulerdr.SendandRev(senddata, 1000);
                
                val=(short)(revdata[revdata.Length - 4]<<8|revdata[revdata.Length - 3]);

                data = new byte[5];
                data[0] = 0x07;
                data[1] = (byte)((0x114 & 0xff00) >> 8);
                data[2] = (byte)(0x114 & 0x00ff);
                val = (short)(val | 0x100);
                data[3] = (byte)((val & 0xff00) >> 8);
                data[4] = (byte)(val & 0x00ff);
                senddata = r2000pcmd.GetSendCmd(R2000_calibration.R2000cmd.Regop, data);
                revdata = modulerdr.SendandRev(senddata, 1000);

                if (revdata[3] + revdata[4] != 0)
                {
                    throw new Exception("S1:" + revdata[3].ToString("X2") + revdata[4].ToString("X2"));
                }

                revdata = modulerdr.SendandRev(buildM5eBootBootloader(), 1000);
                if (revdata[3] + revdata[4] != 0)
                {
                    throw new Exception("B1:" + revdata[3].ToString("X2") + revdata[4].ToString("X2"));
                }
                modulerdr.Disconnect();
  
                VisSet = true;
                MessageBox.Show("设置完毕，请重连接读写器");
            }
            catch (System.Exception ex)
            {
                VisSet = false;
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                R2000_calibration r2000pcmd = new R2000_calibration();
                byte[] data = new byte[5];
                data[0] = 0x07;
                data[1] = (byte)((0x114 & 0xff00) >> 8);
                data[2] = (byte)(0x114 & 0x00ff);
                data[3] = (byte)((0 & 0xff00) >> 8);
                data[4] = (byte)(0 & 0x00ff);
                byte[] senddata = r2000pcmd.GetSendCmd(R2000_calibration.R2000cmd.Regop, data);
                byte[] revdata = modulerdr.SendandRev(senddata, 1000);
 
                if (revdata[3] + revdata[4] != 0)
                {
                    throw new Exception("S1:" + revdata[3].ToString("X2") + revdata[4].ToString("X2"));
                }

                revdata = modulerdr.SendandRev(buildM5eBootBootloader(), 1000);
                if (revdata[3] + revdata[4] != 0)
                {
                    throw new Exception("B1:" + revdata[3].ToString("X2") + revdata[4].ToString("X2"));
                }
                modulerdr.Disconnect();
                VisSet = true;
                MessageBox.Show("设置完毕，请重连接读写器");
            }
            catch (System.Exception ex)
            {
                VisSet = false;
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (modulerdr == null)
            {
                MessageBox.Show("未连接读写器");
                return;
            }
            label_vscheck.Text = "";
            Application.DoEvents();
            try
            {
                R2000_calibration r2000pcmd = new R2000_calibration();
                short val = (short)(0x114);
                byte[] data = new byte[3];
                data[0] = 0x07;
                data[1] = (byte)((val & 0xff00) >> 8);
                data[2] = (byte)(val & 0x00ff);
                byte[] senddata = r2000pcmd.GetSendCmd(R2000_calibration.R2000cmd.Regop, data);
                byte[] revdata = modulerdr.SendandRev(senddata, 1000);
              

                if (revdata[revdata.Length - 4] + revdata[revdata.Length - 3] == 0)
                    {
                        label_vscheck.Text = "动态";
                    }
                    else
                        label_vscheck.Text = "锁定";

              
            }
            catch (System.Exception ex)
            {
             
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }
    }
}
