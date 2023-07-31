using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ModuleTech;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

namespace ModuleReaderManager
{
    public partial class updatefrm : Form
    {
        public updatefrm()
        {
            InitializeComponent();
            this.progressBar1.Maximum = 100;
            this.progressBar1.Minimum = 0;
            this.progressBar2.Maximum = 100;
            this.progressBar2.Minimum = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
                MessageBox.Show("请先输入读写器地址");

            OpenFileDialog of = new OpenFileDialog();
            if(Common.IsIPAddress(textBox1.Text.Trim()))
                of.Filter = "固件|*.slfw";
            else
                of.Filter = "固件|*.bin";


            if (of.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = of.FileName;
            }
        }

        delegate void updateprog(float prg);
        delegate void EnableQuit();
        void EnableQuitbtn()
        {
            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.button3.Enabled = true;
            this.button4.Enabled = true;
        }

        void EnableQuitbtnforsuc()
        {
            this.button1.Enabled = true;
            this.button2.Enabled = true;
            this.button3.Enabled = true;
            this.button4.Enabled = true;

            this.button5.Enabled = true;
            this.button6.Enabled = true;
            this.comboBox1.Enabled = true;

            this.labsoftver.Text = (string)rd.ParamGet("SoftwareVersion");
        }

        void updateprogbar(float prg)
        {
            this.Invoke(new updateprog(updateui), prg);
        }

        void updateui(float prog)
        {
            int curstep = (int)(prog * 100);
            this.progressBar1.Value = curstep;
            this.progressBar2.Value = curstep;
        }

        Reader rd = null;
        Thread updatehread;
        int readertype = -1;
        bool isBootFirmware = false;
        DateTime dtst;

        string type = "";

        void updatefirmware(object text)
        {
            Debug.WriteLine("开始升级");
            try
            {
                rd.FirmwareLoad(text.ToString(), new Reader.FirmwareUpdate(updateprogbar));
                isBootFirmware = true;
                TimeSpan ts = DateTime.Now - dtst;
                MessageBox.Show("升级成功:"+(int)(ts.TotalSeconds)+"s");
                this.Invoke(new EnableQuit(EnableQuitbtnforsuc));

            }
            catch (Exception eex)
            {
                isBootFirmware = false;
                MessageBox.Show("升级失败: " + eex.ToString());
                this.Invoke(new EnableQuit(EnableQuitbtn));
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text.Trim() == "")
            {
                MessageBox.Show("请输入升级文件路径");
                return;
            }
            if (this.textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入读写器地址");
                return;
            }

            string fwpath = this.textBox2.Text.Trim();
            char[] dep = new char[1];
            dep[0] = '\\';
            string[] strs = fwpath.Split(dep);
            string fwfilename = strs[strs.Length - 1];

            if (rd == null)
            {
                try
                {
                    rd = Reader.Create(this.textBox1.Text.Trim(), 6);
                    readertype =0;

                }
                catch (Exception exx)
                {
                    MessageBox.Show("连接读写器失败：" + exx.ToString());
                    this.Invoke(new EnableQuit(EnableQuitbtn));
                    return;
                }
            }


            if (updatehread != null)
            {
                updatehread.Join();

            }
            dtst = DateTime.Now;
            //updatehread = new Thread(updatefirmware);
            //updatehread.Start();
            updatehread = new Thread(new ParameterizedThreadStart(updatefirmware));
            updatehread.Start(this.textBox2.Text.Trim());
            this.button1.Enabled = false;
            this.button3.Enabled = false;
            this.button2.Enabled = false;
            this.button4.Enabled = false;
        }

        private void updatefrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rd != null)
                rd.Disconnect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请输入读写器地址");
                return;
            }

            try
            {
                if (rd == null)
                {
                    rd = Reader.Create(this.textBox1.Text.Trim(), 6);
                    readertype = 6;
                }
                if (readertype == 5)
                {
                    MessageBox.Show("不支持该操作");
                    return;
                }
                this.labsoftver.Text = (string)rd.ParamGet("SoftwareVersion");
            }
            catch (Exception exx)
            {
                isBootFirmware = false;
                MessageBox.Show("连接读写器失败：" + exx.ToString());
                return;
            }
        }

        private void updatefrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (updatehread != null)
                updatehread.Join();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (rd != null)
            {
                if (isBootFirmware && (readertype == 1 || readertype == 2 || readertype == 4))
                    rd.ParamSet("BootFirmware", true);
            }
            this.Close();
        }

        private void updatefrm_Load(object sender, EventArgs e)
        {
            textBox1.Items.Clear();
            textBox1.Text = "";

            comboBox1.Items.Clear();
            comboBox1.Text = "";

            string[] coms = Find_WIN32_Com();
            if (coms != null && coms.Length > 0)
            {
                for (int i = 0; i < coms.Length; i++)
                {
                    textBox1.Items.Add(coms[i]);
                    comboBox1.Items.Add(coms[i]);
                }
            }

            textBox1.Items.Add("192.168.1.100");
            textBox1.SelectedIndex = 0;

            if (comboBox1.Items.Count>0)
                comboBox1.SelectedIndex = 0;

            comboBox2.Items.Clear();
            comboBox2.Items.Add("E系列");
            comboBox2.Items.Add("R2000系列");
            comboBox2.SelectedIndex = 0;
        }

        public string[] Find_WIN32_Com()
        {
            List<string> coms = new List<string>();
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();

                for (int i = 0; i < sSubKeys.Length; i++)
                {
                    if (!sSubKeys[i].Contains("BthModem"))
                    {
                        coms.Add((string)keyCom.GetValue(sSubKeys[i]));
                    }
                }
            }
            return coms.ToArray();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage2)
            {
                type = comboBox2.SelectedIndex == 0 ? "SoftwareVersion_E" : "SoftwareVersion_R";
                lvhoptbAdd(type);
            }
        }

        public void lvhoptbAdd(string fileName) {
            lvhoptb.Items.Clear();
            var ftpUrl = ConfigHelper.GetAppConfig("ftpUrl");
            FtpHelper.path = "ftp://" + ftpUrl;

            var list = FtpHelper.GetFtpFileInfos("", fileName);
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].FileName == "." || list[i].FileName == "..")
                    continue;

                ListViewItem item = new ListViewItem();
                item.SubItems[0].Text = list[i].FileName;
                item.SubItems.Add(Convert.ToDateTime(list[i].LastModifiedDate).ToString());
                lvhoptb.Items.Add(item);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (lvhoptb.SelectedItems.Count > 0)
            {
                var name = lvhoptb.SelectedItems[0].Text;
                //.Parent.FullName + "/download
                string str = System.Environment.CurrentDirectory + "/download";
                var sss = FtpHelper.DownLoadDirectory(type + "/" + name, str);

                string fwpath = System.Environment.CurrentDirectory + @"\download\" + sss[0];
                char[] dep = new char[1];
                dep[0] = '\\';
                string[] strs = fwpath.Split(dep);
                string fwfilename = strs[strs.Length - 1];

                if (rd == null)
                {
                    try
                    {
                        rd = Reader.Create(this.comboBox1.Text.Trim(), 6);
                        readertype = 0;
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show("连接读写器失败：" + exx.ToString());
                        this.Invoke(new EnableQuit(EnableQuitbtn));
                        return;
                    }
                }

                if (updatehread != null)
                {
                    updatehread.Join();
                }

                dtst = DateTime.Now;
                //updatehread = new Thread(updatefirmware);
                updatehread = new Thread(new ParameterizedThreadStart(updatefirmware));
                updatehread.Start(fwpath);
                this.button5.Enabled = false;
                this.button6.Enabled = false;
                this.comboBox1.Enabled = false;
            }
            else {
                MessageBox.Show("请选择一个固件再点击升级。");
            }
         }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            type = comboBox2.SelectedIndex == 0 ? "SoftwareVersion_E" : "SoftwareVersion_R";
            lvhoptbAdd(type);
        }
    }
}