using libra.util;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LibraEditor.egret.resourceTool
{
    
    public enum ResourceType
    {
        image,
        json,
        font,
        sheet,
        sound,
        bin
    }

    public enum SoundType
    {
        music, effect
    }

    /// <summary>
    /// ResourceTool.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class ResourceTool : MetroWindow
    {

        internal static string projectPath;

        public ResJson ResJson { get; set; }

        public ResourceTool()
        {
            InitializeComponent();
        }

        private void OnFindProjectPath(object sender, System.Windows.RoutedEventArgs e)
        {
            projectPath = FileHelper.FindFolder(Config.GetInstance().GetFirstEgretProject());
            if (!string.IsNullOrEmpty(projectPath))
            {
                //判断路径是否正确，若包含egretProperties.json文件则说明正确
                string egretJsonPath = projectPath + "\\egretProperties.json";
                if (File.Exists(egretJsonPath))
                {
                    Config.GetInstance().EgretProjects.Insert(0, projectPath);
                    pathTextBlock.Text = projectPath;

                    string resourceDir = projectPath + "\\resource";
                    if (Directory.Exists(resourceDir))
                    {
                        //ResourceList.Clear();

                        //遍历resource目录下的所有文件，生成一个TmpFile放进fileList中，以便之后的逻辑处理
                        List<TmpFile> fileList = new List<TmpFile>();
                        List<string> files = FileHelper.FindFile(resourceDir);
                        foreach (string filePath in files)
                        {
                            //文件名  “Default.aspx”
                            //string filename = Path.GetFileName(fullPath);
                            //扩展名 “.aspx”
                            string extension = Path.GetExtension(filePath);
                            //没有扩展名的文件名 “Default”
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                            switch (extension.ToLower())
                            {
                                case ".png":
                                case ".jpg":
                                    fileList.Add(new TmpFile() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.image });
                                    break;
                                case ".json":
                                    if (fileNameWithoutExtension == "default.res")
                                    {
                                        // 这是资源的配置文件，进行反序列化
                                        using (StreamReader sr = new StreamReader(filePath))
                                        {
                                            string jsonTxt = sr.ReadToEnd();
                                            ResJson = JsonConvert.DeserializeObject(jsonTxt, typeof(ResJson)) as ResJson;
                                        }
                                        continue;
                                    }
                                    else if ("defaultTest.res" == fileNameWithoutExtension)
                                    {
                                        //测试的，直接跳过
                                        continue;
                                    }
                                    else
                                    {
                                        string sheetImageName = GetSheetImageName(filePath);
                                        if (string.IsNullOrEmpty(sheetImageName))
                                        {
                                            fileList.Add(new TmpFile() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.json });
                                        }
                                        else
                                        {
                                            string[] a = sheetImageName.Split(new char[] { '.' });
                                            List<string> aa = new List<string>(filePath.Split(new char[] { '\\' }));
                                            aa.RemoveAt(aa.Count - 1);
                                            fileList.Add(new TmpFile()
                                            {
                                                Name = fileNameWithoutExtension,
                                                Path = filePath,
                                                Json = new TmpFile()
                                                {
                                                    Name = fileNameWithoutExtension,
                                                    Path = filePath,
                                                    Type = ResourceType.json
                                                },
                                                Image = new TmpFile()
                                                {
                                                    Name = a[0],
                                                    Path = string.Join("\\", aa) + "\\" + sheetImageName,
                                                    Type = ResourceType.image
                                                },
                                                Type = ResourceType.sheet
                                            });
                                        }
                                    }
                                    break;
                                case ".fnt":
                                    fileList.Add(new TmpFile() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.font });
                                    break;
                                case ".mp3":
                                    fileList.Add(new TmpFile() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.sound });
                                    break;
                                default:
                                    fileList.Add(new TmpFile() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.bin });
                                    break;
                            }
                        }

                        //遍历fileList，把sheet中的图片资源删除
                        List<TmpFile> tmpFileList = new List<TmpFile>(fileList.ToArray());
                        foreach (var item in tmpFileList)
                        {
                            if (item.Type == ResourceType.sheet)
                            {
                                fileList.RemoveAll(p => { return item.Image.Path == p.Path; });
                            }
                        }

                        //遍历resJson.resources，把不存在的资源删除
                        if (ResJson != null)
                        {
                            List<ResItem> tmpResItemList = new List<ResItem>(ResJson.resources.ToArray());
                            foreach (var item in tmpResItemList)
                            {
                                if (!fileList.Exists(p =>
                                {
                                    if (p.Path == item.url)
                                    {
                                        return true;
                                    }
                                    return false;
                                }))
                                {
                                    ResJson.resources.Remove(item);
                                }
                            }
                        }
                        else
                        {
                            ResJson = new ResJson();
                        }
                        //添加一个空组
                        ResJson.AddNullGroup();

                        //遍历fileList，把新加入的资源放进resJson
                        foreach (var item in fileList)
                        {
                            if (!ResJson.resources.Exists(p=> { return item.Path == p.url; }))
                            {
                                ResJson.resources.Add(new ResItem() { name = item.Name, url = item.Path, type = Enum.GetName(typeof(ResourceType), item.Type) });
                            }
                            if (item.Type == ResourceType.sheet)
                            {
                                ResJson.resources.ForEach(p =>
                                {
                                    if (p.url == item.Path)
                                    {
                                        p.Json = new ResItem() { name = item.Json.Name, url = item.Json.Path, type = Enum.GetName(typeof(ResourceType), item.Json.Type) };
                                        p.Image = new ResItem() { name = item.Image.Name, url = item.Image.Path, type = Enum.GetName(typeof(ResourceType), item.Image.Type) };
                                    }
                                });
                            }
                        }

                        //初始化配置中的group
                        foreach (ResGroup group in ResJson.groups)
                        {
                            AddGroup(group);
                        }

                        resourceDataGrid.ItemsSource = ResJson.resources;
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

        private void AddGroup(ResGroup group)
        {
            ResJson.resources.ForEach(p =>
            {
                if (group.HasKey(p.name))
                {
                    p.groupName = group.name;
                }
            });
            groupListBox.Items.Add(group.name);
        }

        /// <summary>
        /// 读取json，判断是否是sheet的json
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>如果是sheet的json，返回png名字，否则返回null</returns>
        private string GetSheetImageName(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsonTxt = sr.ReadToEnd();
                Match math = Regex.Match(jsonTxt, "^{\"file\":\"\\w+\\.png\"");
                if (math.Groups.Count > 0)
                {
                    return math.ToString().Replace("\"", "").Replace("{file:", "");
                }
            }
            return null;
        }

        /// <summary>
        /// 导出json文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExport(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(projectPath))
            {
                //如果resJson中有重名的resItem，提示一下
                string itemName = null;
                for (int i = 0; i < ResJson.resources.Count; i++)
                {
                    itemName = ResJson.resources[i].name;
                    for (int j = i + 1; j < ResJson.resources.Count; j++)
                    {
                        if (itemName == ResJson.resources[j].name)
                        {
                            DialogManager.ShowMessageAsync(this, "资源重名错误", "有重名的资源了:" + itemName);
                            return;
                        }
                    }
                }
                //移除多余的group
                ResJson.RemoveRedundantGroup();
                string strSerializeJSON = JsonConvert.SerializeObject(ResJson, Formatting.Indented);
                using (StreamWriter sw = new StreamWriter(projectPath + "\\resource\\defaultTest.res.json"))
                {
                    sw.Write(strSerializeJSON);
                    DialogManager.ShowMessageAsync(this, "导出成功", "default.res.json文件已导出");
                }
            }
        }

        /// <summary>
        /// 资源库中资源选中状态变化了，预览窗口也跟着变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResourceSelected(object sender, SelectedCellsChangedEventArgs e)
        {
            ResItem item = resourceDataGrid.SelectedItem as ResItem;
            BitmapImage bi = null;
            switch (item.ResType)
            {
                case ResourceType.image:
                    bi = new BitmapImage(new Uri(projectPath + "/resource/" + item.url, UriKind.Absolute));
                    break;
                case ResourceType.sheet:
                    bi = new BitmapImage(new Uri(projectPath + "/resource/" + item.Image.url, UriKind.Absolute));
                    break;
                default:
                    break;
            }
            img.Source = bi;
            if (bi != null)
            {
                img.Width = bi.PixelWidth;
                img.Height = bi.PixelHeight;
            }
        }
        
        /// <summary>
        /// 资源组变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGroupChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;
            if (index > -1)
            {
                ResItem item = resourceDataGrid.SelectedItem as ResItem;

                string oldGroupName = item.groupName;
                item.groupName = ResJson.groups[index].name;
                ResJson.OnGroupChanged(item, oldGroupName);
            }
        }

        /// <summary>
        /// 增加资源组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddGroup(object sender, System.Windows.RoutedEventArgs e)
        {
            string name = await DialogManager.ShowInputAsync(this, "新建资源组", "请输入资源组名");
            if (!string.IsNullOrEmpty(name))
            {
                AddGroup(name);
            }
        }

        private void AddGroup(string name)
        {
            ResJson.groups.Add(new ResGroup() { name = name });
            groupListBox.Items.Add(name);
        }

        /// <summary>
        /// 删除资源组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveGroup(object sender, System.Windows.RoutedEventArgs e)
        {
            if (groupListBox.SelectedItem != null)
            {
                string groupName = groupListBox.SelectedItem.ToString();
                if (groupName != "noGroup")
                {
                    ResJson.groups.RemoveAll(p=> 
                    {
                        return p.name == groupName;
                    });
                    groupListBox.Items.Remove(groupName);
                    ResJson.resources.ForEach(p =>
                    {
                        if (p.groupName == groupName)
                        {
                            p.groupName = "noGroup";
                        }
                    });
                }
                else
                {
                    DialogManager.ShowMessageAsync(this, "无法删除", "noGroup资源组是无法删除！");
                }
            }
        }
    }

    public class ResJson
    {

        public List<ResItem> resources { get; set; }

        public List<ResGroup> groups { get; set; }

        public ResJson()
        {
            resources = new List<ResItem>();
            groups = new List<ResGroup>();
        }

        public void AddNullGroup()
        {
            groups.Insert(0, new ResGroup() { name = "noGroup" });
        }

        private ResGroup GetResGroup(string name)
        {
            foreach (var item in groups)
            {
                if (item.name == name)
                {
                    return item;
                }
            }
            return null;
        }

        internal void OnGroupChanged(ResItem resItem, string oldGroupName)
        {
            ResGroup oldGroup = GetResGroup(oldGroupName);
            if (oldGroup != null)
            {
                oldGroup.RemoveKey(resItem.name);
            }
            ResGroup newGroup = GetResGroup(resItem.groupName);
            if (newGroup != null)
            {
                newGroup.AddKey(resItem.name);
            }
        }

        internal void RemoveRedundantGroup()
        {
            groups.RemoveAll(p=> 
            {
                return string.IsNullOrEmpty(p.keys) || p.name == "noGroup";
            });
        }
    }

    public class ResItem
    {
        public string name { get; set; }

        private string _type;
        public string type
        {
            get { return _type; }
            set
            {
                _type = value;
                ResType = (ResourceType)Enum.Parse(typeof(ResourceType), value, true);
            }
        }

        public string url { get; set; }

        [JsonIgnore]
        public string groupName { get; set; }

        [JsonIgnore]
        public ResourceType ResType { get; set; }

        [JsonIgnore]
        public ResItem Json { get; set; }

        [JsonIgnore]
        public ResItem Image { get; set; }

    }

    public class ResGroup
    {
        public string name { get; set; }

        private List<string> _keyList = new List<string>();
        private string _keys = "";
        public string keys
        {
            get
            {
                return _keys;
            }
            set
            {
                _keys = value;
                _keyList = new List<string>(keys.Split(new char[] { ',' }));
            }
        }

        public void RemoveKey(string name)
        {
            foreach (string item in _keyList)
            {
                if (item == name)
                {
                    _keyList.Remove(item);
                    this._keys = string.Join(",", _keyList);
                    break;
                }
            }
        }

        public void AddKey(string name)
        {
            if (!_keyList.Contains(name))
            {
                _keyList.Add(name);
                this._keys = string.Join(",", _keyList);
            }
        }

        public bool HasKey(string name)
        {
            return _keyList.Contains(name);
        }

        public override string ToString()
        {
            return name;
        }
    }

    internal class TmpFile
    {
        public ResourceType Type { get; set; }

        public string Name { get; set; }

        private string path;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value.Replace(ResourceTool.projectPath, "").Replace("\\", "/").Replace("/resource/", "");
            }
        }

        public TmpFile Json { get; set; }

        public TmpFile Image { get; set; }

    }
}
