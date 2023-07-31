using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ModuleTech;
using System.IO;

namespace ModuleReaderManager
{
    public partial class Form_chipupdate : Form
    {
        R2000_calibration.UploadDatas updata;
        R2000_calibration r2000;
        Reader reader;
        int wtotlen;
        byte[] buffer;
        public Form_chipupdate(Reader rdr)
        {
            InitializeComponent();
            reader = rdr;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog of = new OpenFileDialog();
            
                of.Filter = "bin|*.bin";
            

            if (of.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = of.FileName;
                  FileInfo fi = new FileInfo(of.FileName);
                 wtotlen = (int)fi.Length;
                 textBox1.Text += "file:len:" + wtotlen;
                BinaryReader fwr = new System.IO.BinaryReader((new System.IO.StreamReader(of.FileName)).BaseStream);
                buffer = new byte[wtotlen];
                int getc = fwr.Read(buffer, 0, wtotlen);
              
            }
        }

        string dmsg = "";
        bool cmdstream;
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (this.textBox2.Text.Trim() == "")
            {
                MessageBox.Show("请输入升级文件路径");
                return;
            }

            updata = new R2000_calibration.UploadDatas(buffer, 228);
            r2000 = new R2000_calibration();

            cmdstream = checkBox_datastream.Checked;
            string fwpath = this.textBox2.Text.Trim();
            char[] dep = new char[1];
            dep[0] = '\\';
            string[] strs = fwpath.Split(dep);
            string fwfilename = strs[strs.Length - 1];
  
            if (updatehread != null)
            {
                updatehread.Join();

            }
            dtst = DateTime.Now;
            dmsg = "";
            updatehread = new Thread(updatefirmware);
            updatehread.Start();

            this.button1.Enabled = false;
            this.button2.Enabled = false;
        }

        delegate void Updateprog(byte[] bys,byte[] rev,int p);
        delegate void EnableQuit();
        void EnableQuitbtn()
        {
            this.button1.Enabled = true;
            this.button2.Enabled = true;
           
        }

        void EnableQuitbtnforsuc()
        {
            this.button1.Enabled = true;
            this.button2.Enabled = true;
 
        }

        void updateprogbar(byte[] bys, byte[] rev,int p)
        {
            if (cmdstream)
            {
                if (bys != null)
                    textBox1.Text += "Send:P=" + p + " " + ByteFormat.ToHex(bys) + "\r\n";
                if (rev != null)
                    textBox1.Text += "Revd:" + ByteFormat.ToHex(rev) + "\r\n";
                textBox1.Select(textBox1.Text.Length - 1, 1);
                textBox1.ScrollToCaret();
            }

           progressBar1.Value = (int)(p*1.0 / wtotlen * 100);
        }
        void updateui(float prog)
        {
            int curstep = (int)(prog * 100);
            this.progressBar1.Value = curstep;
        }

        DateTime dtst;

        Thread updatehread;
        void updatefirmware()
        {
           
            try
            {
                //int st = Environment.TickCount;
                //rd.FirmwareLoad(this.textBox2.Text.Trim(), new Reader.FirmwareUpdate(updateprogbar));
                dmsg += "1->";
                do
                {
                    if (updata.isSend)
                    {
                        byte[] bys = r2000.GetSendCmd(R2000_calibration.R2000cmd.ChipUpdate, updata.ToByteData());
                        //System.Diagnostics.Debug.WriteLine("cost1:" + (Environment.TickCount - st));
                        byte[] rev = null;
                        try
                        {
                            
                            rev = reader.SendandRev(bys, 2000);
                            //System.Diagnostics.Debug.WriteLine("cost2:" + (Environment.TickCount - st));
                           
                        }
                        catch (Exception ex)
                        {
                            dmsg += "ex->";
                            throw ex;
                        }
                        this.Invoke(new Updateprog(updateprogbar), new object[] { bys, rev, updata.Pos });
                    }
                    else
                    { dmsg += "fin->"; break; }

                    //System.Diagnostics.Debug.WriteLine("cost3:" + (Environment.TickCount - st));
                } while (true);
               
                TimeSpan ts = DateTime.Now - dtst;
                MessageBox.Show("升级成功a:" + (int)(ts.TotalSeconds) + "s");
                this.Invoke(new EnableQuit(EnableQuitbtnforsuc));
                return;
            }
            catch (Exception eex)
            {

                MessageBox.Show("升级失败: " + eex.ToString() + dmsg);
                this.Invoke(new EnableQuit(EnableQuitbtn));
                return;
            }


        }

    }
}
