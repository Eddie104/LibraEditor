using libra.log4CSharp;
using libra.util;
using MahApps.Metro.Controls.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// MapLayer.xaml 的交互逻辑
    /// </summary>
    public partial class MapLayer : UserControl
    {
        public MapLayer()
        {
            InitializeComponent();
        }

        private async void OnAddLayerHandler(object sender, RoutedEventArgs e)
        {
            string name = await DialogManager.ShowInputAsync(MainWindow.GetInstance(), "新建图层", "请输入图层名");
            if (!string.IsNullOrEmpty(name))
            {
                if (RegularHelper.IsLetterAndNumber(name))
                {
                    layerListBox.Items.Add(new LayerItem(name));
                }
                else
                {
                    await DialogManager.ShowMessageAsync(MainWindow.GetInstance(), "图层名错误", "图层名只能以字母开头并且只能包含字母、数字和下划线");
                }
            }
        }
    }
}
