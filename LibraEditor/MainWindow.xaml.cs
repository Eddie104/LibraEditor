using Libra.helper;
using LibraEditor.egret.resourceTool;
using LibraEditor.mapEditor.model;
using LibraEditor.mapEditor.view.newMap;
using LibraEditor.plistTool;
using MahApps.Metro.Controls;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Interop;

namespace LibraEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private static MainWindow instance;

        public Timer Timer { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Config.GetInstance();
            instance = this;

            Timer = new Timer(10000);
            Timer.AutoReset = true;
            Timer.Start();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper wih = new WindowInteropHelper(this);
            HwndSource hWndSource = HwndSource.FromHwnd(wih.Handle);
            //添加处理程序 
            hWndSource.AddHook(MainWindowProc);
            alts = HotKey.GlobalAddAtom("Alt-S");
            altd = HotKey.GlobalAddAtom("Alt-D");
            HotKey.RegisterHotKey(wih.Handle, alts, HotKey.KeyModifiers.Alt, (int)System.Windows.Forms.Keys.S);
            HotKey.RegisterHotKey(wih.Handle, altd, HotKey.KeyModifiers.Alt, (int)System.Windows.Forms.Keys.D);
        }

        int alts, altd;

        private IntPtr MainWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case HotKey.WM_HOTKEY:
                    int sid = wParam.ToInt32();
                    if (sid == alts)
                    {
                        Console.WriteLine("按下Alt+S");
                        MapData.GetInstance().Save();
                    }
                    else if (sid == altd)
                    {
                        Console.WriteLine("按下Alt+D");
                    }
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

        private void NewMapButton_Click(object sender, RoutedEventArgs e)
        {
            NewMap newMap = new NewMap();
            newMap.ShowDialog();
        }

        private void OnShowPlistTool(object sender, RoutedEventArgs e)
        {
            PlistTool plistTool = new PlistTool();
            plistTool.ShowDialog();
        }

        public static MainWindow GetInstance()
        {
            return instance;
        }

        private void OnShowEgretResTool(object sender, RoutedEventArgs e)
        {
            ResourceTool win = new ResourceTool();
            win.ShowDialog();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Config.Save();
        }
    }
}
