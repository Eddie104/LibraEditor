using libra.util;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LibraEditor.egret.resourceTool
{
    /// <summary>
    /// ResourceTool.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceTool : MetroWindow
    {

        public List<Resource> ResourceList { get; set; }

        public ResourceTool()
        {
            InitializeComponent();
            ResourceList = new List<Resource>();
        }

        private void OnFindProjectPath(object sender, System.Windows.RoutedEventArgs e)
        {
            string projectPath = FileHelper.FindFolder();
            if (!string.IsNullOrEmpty(projectPath))
            {
                pathTextBlock.Text = projectPath;

                string egretJsonPath = projectPath + "\\egretProperties.json";
                if (File.Exists(egretJsonPath))
                {
                    string resourceDir = projectPath + "\\resource";
                    if (Directory.Exists(resourceDir))
                    {
                        ResourceList.Clear();

                        List<string> files = FileHelper.FindFile(resourceDir);
                        foreach (string filePath in files)
                        {
                            //string filename = Path.GetFileName(fullPath);//文件名  “Default.aspx”
                            string extension = Path.GetExtension(filePath);//扩展名 “.aspx”
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);// 没有扩展名的文件名 “Default”
                            switch (extension.ToLower())
                            {
                                case ".png":
                                case ".jpg":
                                    ResourceList.Add(new Resource() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.image });
                                    break;
                                case ".json":
                                    ResourceList.Add(new Resource() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.json });
                                    break;
                                case ".fnt":
                                    ResourceList.Add(new Resource() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.font });
                                    break;
                                case ".mp3":
                                    ResourceList.Add(new Resource() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.sound });
                                    break;
                                default:
                                    ResourceList.Add(new Resource() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.bin });
                                    break;
                            }
                        }
                        resourceDataGrid.ItemsSource = ResourceList;
                    }
                    else
                    {
                        DialogManager.ShowMessageAsync(this, "路径错误", "没有resource目录！");
                    }
                }
                else
                {
                    DialogManager.ShowMessageAsync(this, "路径错误", "白鹭引擎路径选择错误，请重新选择！");
                }
            }
        }

        private void OnResourceSelected(object sender, SelectedCellsChangedEventArgs e)
        {
            Resource resource = (sender as DataGrid).SelectedItem as Resource;
            if (resource.Type == ResourceType.image)
            {
                img.Source = new BitmapImage(new Uri(resource.Path, UriKind.Absolute));
            }
        }

        private async void OnAddGroup(object sender, System.Windows.RoutedEventArgs e)
        {
            string name = await DialogManager.ShowInputAsync(this, "资源组名", "请输入资源组名");
            if (!string.IsNullOrEmpty(name))
            {
                groupPanel.Children.Add(new ResourceGroup(name));
            }
        }
    }

    public class Resource
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string GroupName { get; set; }

        public ResourceType Type { get; set; }
    }

    public enum ResourceType
    {
        image,
        json,
        font,
        sheet,
        sound,
        bin
    }
}
