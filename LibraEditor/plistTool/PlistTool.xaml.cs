using LibraEditor.libra.util;
using MahApps.Metro.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;

namespace LibraEditor.plistTool
{
    /// <summary>
    /// PlistTool.xaml 的交互逻辑
    /// </summary>
    public partial class PlistTool : MetroWindow
    {
        public PlistTool()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = @"E:\vsProjects\LibraEditor\test\2.plist";
            PlistHelper t = new PlistHelper();
            t.XMLparser(path);
            PlistData data = t.CreatePlistData();

            Cut(@"E:\vsProjects\LibraEditor\test", "2.png", data);
        }

        private void Cut(string imgDir, string imgName, PlistData plistData)
        {
            // 加载图片
            System.Drawing.Image image = new System.Drawing.Bitmap(imgDir + "/" + imgName);
            // 目标区域
            Rectangle destRect = new Rectangle();
            foreach (var item in plistData.Frames)
            {
                // 源图区域
                Rectangle srcRect = item.GetTextureRect();
                destRect.Width = srcRect.Width;
                destRect.Height = srcRect.Height;

                // 新建Graphics对象
                Bitmap newImage = new Bitmap(destRect.Width, destRect.Height);
                Graphics g = Graphics.FromImage(newImage);
                // 绘图平滑程序
                g.SmoothingMode = SmoothingMode.HighQuality;
                // 图片输出质量
                g.CompositingQuality = CompositingQuality.HighQuality;
                // 输出到newImage对象
                g.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                // 释放绘图对象
                g.Dispose();

                string strDestFile = string.Format("{0}\\{1}", imgDir, item.PngName);
                newImage.Save(strDestFile);
                newImage.Dispose();
            }
            // 释放图像资源
            image.Dispose();
        }
    }
}
