using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Form_box_html : Form
    {
        JObject jObject = new JObject();
        JArray table = new JArray();
        bool timer1Type = true;
        public Form_box_html(JObject _jObject, JArray _table)
        {
            InitializeComponent();
            jObject = _jObject;
            table = _table;
            
            this.webBrowser1.ObjectForScripting = this;
            string path = Application.StartupPath + @"\html\charts.html";
            this.webBrowser1.Url = new System.Uri(path, System.UriKind.Absolute);
        }

        private void Form_box_html_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            this.webBrowser1.Width = this.Width;
            this.webBrowser1.Height = this.Height;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(timer1Type)
                webBrowser1.Document.InvokeScript("DoAdd", new object[] { jObject.ToString(), table.ToString() });

            timer1Type = false;
            timer1.Enabled = false;
        }


        public void ADSA()
        {
            //弹出保存文件对话框。
            SaveFileDialog saveFileExcel = new SaveFileDialog();

            saveFileExcel.Filter = "Excel 文档(*.xls)|*.xls|所有文件(*.*)|*.*";

            saveFileExcel.ShowDialog();

            //如果用户选择了路径则生成excel
            if (saveFileExcel.FileName != "")
            {
                FileStream filestreamExcel = new FileStream(saveFileExcel.FileName, FileMode.Create, FileAccess.Write);//生成文件

                StreamWriter streamwriterExcel = new StreamWriter(filestreamExcel, System.Text.Encoding.GetEncoding("gb2312"));//生成可以向刚刚生成的文件里写内容的可写流
                StringBuilder sb = new StringBuilder();

                sb.Append("次数\t");
                sb.Append("已读标签\t");
                sb.Append("耗时(毫秒)\t");
                sb.Append("存在标签\t");
                sb.Append("记录时间\t");
                sb.Append("状态\t");
                //streamwriterExcel.Write(sb);

                if (table.Count > 0)
                {
                    foreach (JObject lstvItem in table)
                    {
                        sb.Append("\n\r");
                        var tags = Convert.ToInt32(lstvItem["tags"].ToString());
                        var count = Convert.ToInt32(lstvItem["count"].ToString());

                        sb.Append(lstvItem["name"] + "\t");
                        sb.Append(lstvItem["tags"] + "\t");
                        sb.Append(lstvItem["mis"] + "\t");
                        sb.Append(lstvItem["count"] + "\t");
                        sb.Append(DateTime.Now.ToString()+ "\t");
                        if (tags>= count)
                            sb.Append("已读全\t");
                        else
                            sb.Append("未读全\t");

                    }
                }
                streamwriterExcel.Write(sb);
                streamwriterExcel.Close();

                streamwriterExcel.Dispose();

                filestreamExcel.Close();

                filestreamExcel.Dispose();
            }
        }

    }
}
