using libra.util;
using LibraEditor.libra.util;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;

namespace LibraEditor.mapEditor.view.newMap
{
    /// <summary>
    /// NewMap.xaml 的交互逻辑
    /// </summary>
    public partial class NewMap : MetroWindow
    {

        public static event EventHandler CreateMapHandler;

        public NewMap()
        {
            InitializeComponent();

            this.projectTypeComboBox.ItemsSource = Enum.GetNames(typeof(ProjectType));
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

                    this.tileWidthNumeric.Value = 32;
                    this.tileWidthNumeric.Interval = 4;
                }
                else
                {
                    this.tileWidthNumeric.Value = 30;
                    this.tileWidthNumeric.Interval = 1;
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
                Config.ProjectType = (ProjectType)Enum.GetValues(typeof(ProjectType)).GetValue(projectTypeComboBox.SelectedIndex);
                Config.ViewType = (bool)obliqueRadioButton.IsChecked ? ViewType.iso : ViewType.tile;
                Config.CellWidth = (int)tileWidthNumeric.Value;
                Config.CellHeight = (int)tileHeightNumeric.Value;
                Config.CellRows = (int)tileRowsNumeric.Value;
                Config.CellCols = (int)tileColsNumeric.Value;

                if (Config.ViewType == ViewType.iso)
                {
                    ISOHelper.Width = Config.CellWidth;
                    ISOHelper.Height = Config.CellHeight;

                    if (Config.CellWidth % 4 != 0)
                    {
                        DialogManager.ShowMessageAsync(this, "地图宽度有误", "斜视角地图中，格子宽度应为4的倍数");
                        return;
                    }
                    Config.CellHeight = Config.CellWidth / 2;
                    ISOHelper.Height = ISOHelper.Width / 2;
                }
                else
                {
                    RectangularHelper.Width = Config.CellWidth;
                    RectangularHelper.Height = Config.CellHeight;
                }

                CreateMapHandler(this, null);
                this.Close();
            }
            else
            {
                DialogManager.ShowMessageAsync(this, "地图名错误", "命名应以字母开头，并且只能包含字母数字和下划线");
            }
        }
    }
}
