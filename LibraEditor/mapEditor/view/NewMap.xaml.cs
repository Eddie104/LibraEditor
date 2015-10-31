using libra.util;
using LibraEditor.libra.util;
using LibraEditor.mapEditor.model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
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

            foreach (var item in Config.GetInstance().MapPropjects)
            {
                this.mapListBox.Items.Add(item);
            }
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
            if (!string.IsNullOrEmpty(mapFloderTextBlock.Text))
            {
                MapData mapData = MapData.GetInstance();
                string mapName = this.mapNameTextBox.Text;
                if (RegularHelper.IsLetterAndNumber(mapName))
                {
                    mapData.Name = mapName;
                    mapData.Path = mapFloderTextBlock.Text + "\\" + mapData.Name;
                    mapData.ProjectType = (ProjectType)Enum.GetValues(typeof(ProjectType)).GetValue(projectTypeComboBox.SelectedIndex);
                    mapData.ViewType = (bool)obliqueRadioButton.IsChecked ? ViewType.iso : ViewType.tile;
                    mapData.CellWidth = (int)tileWidthNumeric.Value;
                    mapData.CellHeight = (int)tileHeightNumeric.Value;
                    mapData.CellRows = (int)tileRowsNumeric.Value;
                    mapData.CellCols = (int)tileColsNumeric.Value;

                    InitHelper();

                    mapData.Created();
                    Config.GetInstance().MapPropjects.Add(mapData.Path);

                    CreateMapHandler(this, null);
                    this.Close();
                }
                else
                {
                    DialogManager.ShowMessageAsync(this, "地图名错误", "命名应以字母开头，并且只能包含字母数字和下划线");
                }
            }
            else
            {
                DialogManager.ShowMessageAsync(this, "存储路径有误", "请选择正确的地图存储路径");
            }
        }

        private void OnOpenMap(object sender, RoutedEventArgs e)
        {
            if (this.mapListBox.SelectedItem != null)
            {
                string mapPath = mapListBox.SelectedItem.ToString();
                List<string> t = new List<string>(mapPath.Split(new char[] { '\\' }));
                string mapJsonPath = mapPath + "\\" + t[t.Count - 1] + ".json";
                MapData.CreateWithJson(mapJsonPath);
                InitHelper();
                CreateMapHandler(this, null);
                this.Close();
            }
        }

        private void InitHelper()
        {
            MapData mapData = MapData.GetInstance();
            if (mapData.ViewType == ViewType.iso)
            {
                ISOHelper.Width = mapData.CellWidth;
                ISOHelper.Height = mapData.CellHeight;

                if (mapData.CellWidth % 4 != 0)
                {
                    DialogManager.ShowMessageAsync(this, "地图宽度有误", "斜视角地图中，格子宽度应为4的倍数");
                    return;
                }
                mapData.CellHeight = mapData.CellWidth / 2;
                ISOHelper.Height = ISOHelper.Width / 2;
            }
            else
            {
                RectangularHelper.Width = mapData.CellWidth;
                RectangularHelper.Height = mapData.CellHeight;
            }
        }
    }
}
