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
using System.Diagnostics;

namespace InitTagWrite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Reader rdr = null;
        void OpenReader()
        {
            /*
             * 当使用设备的网口进行连接的时候，Create函数的第一个参数也可能是设备的
             * IP地址,当设备仅有一个天线端口时（例如一体机或者发卡器），Create
             * 函数的第三个参数为1,请根据设备的实际天线端口数确定第三个参数的值
             * */
            int antnum = 1;
            rdr = Reader.Create("com1", ModuleTech.Region.NA, antnum);

            #region 必须要设置的参数

            #region 设置读写器的发射功率
            //设置读写器发射功率为19dbm，对于初始化标签类应用不宜将发射功率
            //设置太大,一般来说发射功率越大读写距离越远
            AntPower[] pwrs = new AntPower[antnum];
            for (int i = 0; i < antnum; ++i)
            {
                pwrs[i].AntId = (byte)(i + 1);
                pwrs[i].ReadPower = (ushort)1900;
                pwrs[i].WritePower = (ushort)1900;
            }
            rdr.ParamSet("AntPowerConf", pwrs);
            #endregion

            #region 设置盘存标签使用的天线
            //如果要使用其它天线可以在数组useants中放置其它多个天线编号，本
            //例中是使用天线1
            int[] useants = new int[] { 1 };
            rdr.ParamSet("ReadPlan", new SimpleReadPlan(TagProtocol.GEN2, useants));
            #endregion

            #region 设置读，写，器锁等标签操作使用的天线
            //此例设置使用天线1做除盘存外的其它标签操作
            rdr.ParamSet("TagopAntenna", 1);
            #endregion

            #endregion

        }
        //用于在具体标签操作前检测是否只有一个标签在天线场内
        private TagReadData PreTagOp()
        {
            try
            {
                TagReadData[] tags = rdr.Read(250);
                if (tags.Length != 1)
                {
                    MessageBox.Show("请保证有且只有一个标签在天线场内");
                    return null;
                }
                return tags[0];
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("检测标签失败:"+ex.ToString());
            }
            return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                OpenReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接读写器失败:"+ex.ToString());
                Close();
                return;
            }
        }
        /*
         * 向标签写入EPC码，此例将以16进制字符串000000000000000000000001表示
         * 的数据写入EPC区。
         * 但请注意：WriteTag和WriteTagMemWords方法都可以实现修改标签EPC区数
         * 据，EPC区的PC段中有几个bit位是用于指明当进行盘存操作（对应Read方法）
         * 时返回给读写器的EPC码的长度，也就是说返回的EPC码的长度是可以小于等
         * 于实际的EPC区容量的，WriteTag方法会从块地址2开始将数据写入EPC区，
         * 同时将PC段中指示盘存操作的EPC码长度的bit位设置为写入数据的长度，也
         * 即WriteTag方法写入的数据会和Read方法读出的数据完全一致。
         * WriteTagMemWords方法则可以修改EPC区任何可写的存储区域。
         * */
        private void btnwriteepc_Click(object sender, EventArgs e)
        {
            TagReadData target = PreTagOp();
            if (target == null)
                return;
            Gen2TagFilter filter = new Gen2TagFilter(target.EPC.Length * 8, target.EPC, MemBank.EPC, 32, false);
            try
            {
                //访问密码的设置分为两种情况：
                //1，当所操作区域被锁定时，必须设置和标签一致的访问密码操作才能成功
                //2，当所操作区域未被锁定时，可以设置访问密码为0，或则不设置访问密码，
                //读写器默认采用0作为访问密码
                uint accesspwd = (uint)0;
                rdr.ParamSet("AccessPassword", accesspwd);
                rdr.WriteTag(filter, new TagData(ByteFormat.FromHex("000000000000000000000001")));
                MessageBox.Show("操作成功");
            }
            catch (ModuleException mex)
            {
                MessageBox.Show("操作失败:"+mex.ToString());
                return;
            }
        }
        /*
         * 向标签存储区中写入数据，本例向USER区写入两个块的数据
         * */
        private void btnwritebank_Click(object sender, EventArgs e)
        {
            TagReadData target = PreTagOp();
            if (target == null)
                return;
            Gen2TagFilter filter = new Gen2TagFilter(target.EPC.Length * 8, target.EPC, MemBank.EPC, 32, false);
            try
            {
                //访问密码的设置分为两种情况：
                //1，当所操作区域被锁定时，必须设置和标签一致的访问密码操作才能成功
                //2，当所操作区域未被锁定时，可以设置访问密码为0，或则不设置访问密码，
                //读写器默认采用0作为访问密码
                uint accesspwd = (uint)0;
                rdr.ParamSet("AccessPassword", accesspwd);
                ushort[] wdata = new ushort[2];
                wdata[0] = 0x1234;
                wdata[1] = 0x5678;
                rdr.WriteTagMemWords(filter, MemBank.USER, 0, wdata);
                MessageBox.Show("操作成功");
            }
            catch (ModuleException mex)
            {
                MessageBox.Show("操作失败:" + mex.ToString());
                return;
            }
        }

        /*
         * 读取标签存储区中的数据，本例读取USER区从地址0开始的2个块数据
         * */
        private void btnreadbank_Click(object sender, EventArgs e)
        {
            TagReadData target = PreTagOp();
            if (target == null)
                return;
            Gen2TagFilter filter = new Gen2TagFilter(target.EPC.Length * 8, target.EPC, MemBank.EPC, 32, false);
            try
            {
                //访问密码的设置分为两种情况：
                //1，当所操作区域被锁定时，必须设置和标签一致的访问密码操作才能成功
                //2，当所操作区域未被锁定时，可以设置方面密码为0，或则不设置访问密码，
                //读写器默认采用0作为访问密码
                uint accesspwd = (uint)0;
                rdr.ParamSet("AccessPassword", accesspwd);
                ushort[] rdata = rdr.ReadTagMemWords(filter, MemBank.USER, 0, 2);
                MessageBox.Show("操作成功");
            }
            catch (ModuleException mex)
            {
                MessageBox.Show("操作失败:" + mex.ToString());
                return;
            }
        }
        /*
         * 锁定标签的存储区域，本例暂时锁定标签的EPC区
         * 需要注意的是锁定标签存储区域前一般都需要先初始化标签的访问密码（本
         * 例假定标签的访问密码为0x12345678）为非0,在调用LockTag方法锁定
         * 存储区域前一定要设置AccessPassword参数，否则操作会失败。初始化标签
         * 的访问密码可以通过WriteTagMemWords方法进行操作。在完成LockTag后，
         * 请将AccessPassword为0，因为AccessPassword参数一旦设置过以后会一直
         * 保持有效，直到下次设置。一般刚刚采购回来的标签默认的访问密码都是0。
         * */
        private void btnlocktag_Click(object sender, EventArgs e)
        {
            TagReadData target = PreTagOp();
            if (target == null)
                return;
            Gen2TagFilter filter = new Gen2TagFilter(target.EPC.Length * 8, target.EPC, MemBank.EPC, 32, false);
            try
            {
                Gen2LockAct[] act = new Gen2LockAct[1];
                act[0] = Gen2LockAct.EPC_LOCK;
                uint accesspwd = 0x12345678;
                rdr.ParamSet("AccessPassword", accesspwd);
                rdr.LockTag(filter, new Gen2LockAction(act));
                accesspwd = (uint)0;
                rdr.ParamSet("AccessPassword", accesspwd);
                MessageBox.Show("操作成功");
            }
            catch (ModuleException mex)
            {
                MessageBox.Show("操作失败:" + mex.ToString());
                return;
            }
        }
        /*
         * 销毁标签，标签被销毁后就不再能使用了
         * 需要注意的是标签只有初始化了非零的销毁密码才能执行销毁操作
         * */
        private void btnkilltag_Click(object sender, EventArgs e)
        {
            TagReadData target = PreTagOp();
            if (target == null)
                return;
            Gen2TagFilter filter = new Gen2TagFilter(target.EPC.Length * 8, target.EPC, MemBank.EPC, 32, false);
            try
            {
                //假定标签的销毁密码为0x87654321，请根据标签实际写入的销毁密码进行设置
                uint killpwd = 0x87654321;
                rdr.KillTag(filter, killpwd);
                MessageBox.Show("操作成功");
            }
            catch (ModuleException mex)
            {
                MessageBox.Show("操作失败:" + mex.ToString());
                return;
            }
        }
        /*
         * 完成初始化访问密码、销毁密码、EPC码并锁定标签的访问密码、销毁密码
         * 和EPC区
         * 注：在实际的应用场景中，从安全性角度出发初始化了标签的EPC区或者
         * USER区后有可能需要将这些存储区域进行锁定，防止非授权的改写。如果
         * 是这类应用则必须要初始化标签的访问密码为非0才可以锁定标签的其它存
         * 储区域，如果考虑到以后还有可能需要销毁标签，则还必须初始化标签的
         * 销毁密码。访问密码和销毁密码也需要被锁定，这些密码才不能被非授权
         * 地读出和改写。
         * */
        private void btninittag_Click(object sender, EventArgs e)
        {
            TagReadData target = PreTagOp();
            if (target == null)
                return;
            Gen2TagFilter filter = new Gen2TagFilter(target.EPC.Length * 8, target.EPC, MemBank.EPC, 32, false);
            try
            {
                //标签出厂时默认的访问密码都是全0
                uint accesspwd = (uint)0;
                rdr.ParamSet("AccessPassword", accesspwd);

                byte[] wepc = ByteFormat.FromHex("000000000000000000000001");
                rdr.WriteTag(filter, new TagData(wepc));

                ushort[] killpwd = new ushort[2];
                killpwd[0] = 0x8765;
                killpwd[1] = 0x4321;
                rdr.WriteTagMemWords(filter, MemBank.RESERVED, 0, killpwd);

                ushort[] w_accesspwd = new ushort[2];
                w_accesspwd[0] = 0x1234;
                w_accesspwd[1] = 0x5678;
                rdr.WriteTagMemWords(filter, MemBank.RESERVED, 2, w_accesspwd);
                
                Gen2LockAct[] act = new Gen2LockAct[3];
                act[0] = Gen2LockAct.ACCESS_LOCK;
                act[1] = Gen2LockAct.KILL_LOCK;
                act[2] = Gen2LockAct.EPC_LOCK;
                accesspwd = 0x12345678;
                rdr.ParamSet("AccessPassword", accesspwd);
                Gen2TagFilter filter2 = new Gen2TagFilter(wepc.Length * 8, wepc, MemBank.EPC, 32, false);
                rdr.LockTag(filter2, new Gen2LockAction(act));
                //修改访问密码为全0，以免影响下次标签操作
                accesspwd = (uint)0;
                rdr.ParamSet("AccessPassword", accesspwd);
                MessageBox.Show("操作成功");
            }
            catch (ModuleException mex)
            {
                MessageBox.Show("操作失败:" + mex.ToString());
                return;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rdr != null)
                rdr.Disconnect();
        }
    }
}
