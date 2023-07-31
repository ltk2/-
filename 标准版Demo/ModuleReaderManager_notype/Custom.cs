using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModuleReaderManager
{
    public class CustomToolStripMenuItem : ToolStripMenuItem
    {
        public bool RedDotHint;
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent); //Call base for creating default Button 
            if (!RedDotHint)
                return;

            using (Pen pen = new Pen(Color.Red, 3)) //Create small Red Pen 
            {
                Rectangle rectangle = new Rectangle(pevent.ClipRectangle.X + pevent.ClipRectangle.Width - 10, 2, 6, 6);
                // 提升显示效果 高清显示
                pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
                pevent.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
                pevent.Graphics.CompositingQuality = CompositingQuality.HighQuality;//再加一点

                //画圆
                pevent.Graphics.DrawEllipse(pen, rectangle); 
                pevent.Graphics.FillEllipse(new SolidBrush(Color.Red), rectangle); 
                
            }
        }
    }
}
