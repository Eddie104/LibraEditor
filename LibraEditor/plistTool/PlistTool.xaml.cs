using libra.util;
using LibraEditor.libra.util;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

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

        private void OnCut(object sender, RoutedEventArgs e)
        {
            string[] pathArr = FileHelper.FindFile("*.plist", false, Config.GetRecentPlistPath());
            if (pathArr != null && pathArr.Length > 0)
            {
                resultContainer.Children.Clear();

                Config.SetRecentPlistPath(pathArr[0]);

                PlistHelper t = new PlistHelper();
                t.XMLparser(pathArr[0]);
                PlistData data = t.CreatePlistData();

                List<string> a = new List<string>(pathArr[0].Split(new char[] { '\\' }));
                string plistName = a[a.Count - 1];
                a.RemoveAt(a.Count - 1);
                Cut(string.Join("\\", a), plistName.Replace("plist", "png"), data);
            }
        }

        private void Cut(string imgDir, string imgName, PlistData plistData)
        {
            // 加载图片
            Bitmap image = new Bitmap(imgDir + "/" + imgName);

            //显示原图
            BitmapSource bi = Imaging.CreateBitmapSourceFromHBitmap(image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            sourceImg.Source = bi;
            sourceImg.Width = bi.PixelWidth;
            sourceImg.Height = bi.PixelHeight;

            // 目标区域
            Rectangle destRect = new Rectangle();
            foreach (var item in plistData.Frames)
            {
                // 源图区域
                Rectangle srcRect = new Rectangle(item.GetTextureRect().X, item.GetTextureRect().Y, item.GetTextureRect().Width, item.GetTextureRect().Height);
                if (item.IsRotated)
                {
                    int tmp = srcRect.Height;
                    srcRect.Height = srcRect.Width;
                    srcRect.Width = tmp;
                }
                
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

                if (item.IsRotated)
                {
                    newImage = ImageHelper.Rotate(newImage, 90);
                }

                bi = Imaging.CreateBitmapSourceFromHBitmap(newImage.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                System.Windows.Controls.Image ff = new System.Windows.Controls.Image();
                ff.Source = bi;
                ff.Width = bi.PixelWidth;
                ff.Height = bi.PixelHeight;
                resultContainer.Children.Add(ff);

                string strDestFile = string.Format("{0}\\{1}", imgDir, item.PngName);
                newImage.Save(strDestFile);
                newImage.Dispose();
            }
            // 释放图像资源
            image.Dispose();
        }

    }
}
