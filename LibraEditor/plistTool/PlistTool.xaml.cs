using libra.log4CSharp;
using LibraEditor.libra.util;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            string path = @"C:\Users\鸿杰\Desktop\plist\0.plist";
            PlistHelper t = new PlistHelper();
            t.XMLparser(path);
            PlistData data = t.CreatePlistData();
            var y = data.Frames[0].Width;
            //foreach (var item in t.DataList)
            //{
            //    Logger.Info(item.ToString());
            //}
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("aaaaaaaaaaaaaaaaaaa");
        }
    }
}
