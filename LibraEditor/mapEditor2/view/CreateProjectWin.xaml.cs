using Libra.helper;
using LibraEditor.mapEditor2.model.data;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// CreateMapWin.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProjectWin : MetroWindow
    {

        public event EventHandler CreateMapHandler;

        public CreateProjectWin()
        {
            InitializeComponent();

            foreach (var item in Config.GetInstance().MapPropjects)
            {
                this.mapListBox.Items.Add(item);
            }
        }

        /// <summary>
        /// 创建新地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreateMap(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(mapFloderTextBlock.Text))
            {
                string mapName = mapNameTextBox.Text;
                if (RegularHelper.IsLetterAndNumber(mapName))
                {
                    GameData gameData = GameData.GetInstance();
                    gameData.Name = mapName;
                    gameData.Path = mapFloderTextBlock.Text + "\\" + gameData.Name;
                    gameData.ViewType = (bool)obliqueRadioButton.IsChecked ? MapViewType.iso : MapViewType.tile;
                    gameData.CellWidth = (int)tileWidthNumeric.Value;
                    gameData.CellHeight = (int)tileHeightNumeric.Value;
                    gameData.CellRows = (int)tileRowsNumeric.Value;
                    gameData.CellCols = (int)tileColsNumeric.Value;

                    InitHelper();
                    Config.GetInstance().MapPropjects.Add(gameData.Path);

                    //创建map的文件夹
                    if (!Directory.Exists(gameData.Path))
                    {
                        Directory.CreateDirectory(gameData.Path);
                    }
                    gameData.NeedSave = true;
                    gameData.Save();

                    CreateMapHandler(this, null);
                    this.Close();
                }
                else
                {
                    DialogManager.ShowMessageAsync(this, "地图名错误", "地图名只能包含字母和数字");
                }
            }
            else
            {
                DialogManager.ShowMessageAsync(this, "存储路径有误", "请选择正确的地图存储路径");
            }
        }

        private void InitHelper()
        {
            GameData gameData = GameData.GetInstance();
            ICoordinateHelper helper;
            if (gameData.ViewType == MapViewType.iso)
            {
                helper = new ISOHelper(gameData.CellWidth, gameData.CellHeight);
                if (gameData.CellWidth % 2 != 0)
                {
                    DialogManager.ShowMessageAsync(this, "地图宽度有误", "斜视角地图中，格子宽度应为2的倍数");
                    return;
                }
                gameData.CellHeight = gameData.CellWidth / 2;
                helper.Height = helper.Width / 2;
            }
            else
            {
                helper = new RectangularHelper();
                helper.Width = gameData.CellWidth;
                helper.Height = gameData.CellHeight;
            }
            MapEditor.GetInstance().CoordinateHelper = helper;
        }

        /// <summary>
        /// 打开指定的地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOpenMap(object sender, RoutedEventArgs e)
        {
            if (this.mapListBox.SelectedItem != null)
            {
                string mapPath = mapListBox.SelectedItem.ToString();
                GameData.CreateWithJson(string.Format("{0}\\{1}.json", mapPath, Path.GetFileName(mapPath)));
                InitHelper();
                CreateMapHandler(this, null);
                this.Close();
            }
        }

        private void OnMapTypeChanged(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {   
                bool isOblique = (bool)this.obliqueRadioButton.IsChecked;
                if (isOblique)
                {
                    //斜视角
                    tileHeightLabel.Visibility = Visibility.Hidden;
                    tileHeightNumeric.Visibility = Visibility.Hidden;

                    tileWidthNumeric.Value = 32;
                    tileWidthNumeric.Interval = 2;
                }
                else
                {
                    tileWidthNumeric.Value = 30;
                    tileWidthNumeric.Interval = 1;
                    tileHeightLabel.Visibility = Visibility.Visible;
                    tileHeightNumeric.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// 找到新地图的存储路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOpenDir(object sender, RoutedEventArgs e)
        {
            mapFloderTextBlock.Text = FileHelper.FindFolder();
            mapFloderTextBlock.ToolTip = mapFloderTextBlock.Text;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //移除事件监听
            EventHelper.RemoveEvent<CreateProjectWin>(this, "CreateMapHandler");
        }
    }
}
