using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace ModuleReaderManager
{
    public partial class AboutFrm : Form
    {
        public static string versions = "";//当前版本
        public static string newVersion = "";//最新版
        public static string fileCatalog = "ModuleReaderManager";//请求的文件目录
        public static string ftpUrl = "";//请求的ftp地址
        public static string exe = "ModuleReaderManager.exe";//退出后打开的exe
        public AboutFrm()
        {
            InitializeComponent();
            versions = ConfigHelper.GetAppConfig("versions");
            ftpUrl = ConfigHelper.GetAppConfig("ftpUrl");
            FtpHelper.path = "ftp://" + ftpUrl;
        }

        private void AboutFrm_Load(object sender, EventArgs e)
        {
            button1.Visible = false;
            labVersion.Text = "ModuleReaderManager  v" + versions;

            if (!FtpHelper.IsInternetAvailable())
                return;

            //获取最新版本，判断是否显示更新按钮
            var list = FtpHelper.GetFtpFileInfos("", fileCatalog);
            list = list.OrderByDescending(r => r.FileName).ToArray();
            if (list != null && list[0].FileName != "." && list[0].FileName != "..")
            {
                newVersion = list[0].FileName;
                Version v1 = new Version(list[0].FileName);
                Version v2 = new Version(versions);
                if (v1 > v2)
                {
                    //var txtx = list.Where(r => r.FileName == "0.txt").SingleOrDefault();
                    //Label label = new Label();
                    //label.Text = "3.16.2版本已上线，点击下方按钮完成更新!";
                    //label.Location = new System.Drawing.Point(100, 85);
                    //label.Size = new System.Drawing.Size(220, 130);
                    //label.Visible = true;
                    //this.Controls.Add(label);
                    button1.Visible = true;
                    this.Height += 50;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string arguments = $"{fileCatalog}+{newVersion}+{exe}+{ftpUrl}";
            var url = AppDomain.CurrentDomain.BaseDirectory + "/versionUpdating/ReaderUpgrade.exe";
            ProcessStartInfo info = new ProcessStartInfo(url, arguments.ToString());
            Process process = new Process();
            process.StartInfo = info;
            process.Start();

            Application.Exit();
        }

        private bool CrackFireWall()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\AuthorizedApplications\List", true);
                string filename = Application.ExecutablePath;
                string exename = Path.GetFileNameWithoutExtension(filename);
                key.SetValue(filename, filename + ":*:Enabled:" + exename, RegistryValueKind.String);//设置这里
            }
            catch (Exception)
            {
                return (false);
            }
            return (true);
            //就是在注册表的相应位置，添加上自己要穿越的程序！！！
        }
    }
}