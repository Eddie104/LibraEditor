using libra.util;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace LibraEditor.mapEditor.view.newMap
{
    /// <summary>
    /// NewMap.xaml 的交互逻辑
    /// </summary>
    public partial class NewMap : MetroWindow
    {

        public NewMap()
        {
            InitializeComponent();
        }

        private void OnOpenDir(object sender, RoutedEventArgs e)
        {
            mapFloderTextBlock.Text = FileHelper.FindFolder();
            mapFloderTextBlock.ToolTip = mapFloderTextBlock.Text;
        }

        private void OnMapTypeChanged(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                bool isOblique = (bool)this.obliqueRadioButton.IsChecked;
                if (isOblique)
                {
                    this.tileHeightLabel.Visibility = Visibility.Hidden;
                    this.tileHeightNumeric.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.tileHeightLabel.Visibility = Visibility.Visible;
                    this.tileHeightNumeric.Visibility = Visibility.Visible;
                }
            }
        }

        private void OnCreateMap(object sender, RoutedEventArgs e)
        {
            string mapName = this.mapNameTextBox.Text;
            if (RegularHelper.IsLetterAndNumber(mapName))
            {

            }
            else
            {
                DialogManager.ShowMessageAsync(MainWindow.GetInstance(), "地图名错误", "命名应以字母开头，并且只能包含字母数字和下划线");
            }
        }
    }
}
