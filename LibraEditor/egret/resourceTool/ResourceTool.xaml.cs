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
    /// <summary>
    /// ResourceTool.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceTool : MetroWindow
    {

        public List<Resource> ResourceList { get; set; }

        public List<ResourceDataGroup> ResourceDataGroups { get; set; }

        public List<ResourceGroup> ResourceGroupList { get; set; }

        public ResJson ResJson { get; set; }

        public static string projectPath;

        public ResourceTool()
        {
            InitializeComponent();

            ResourceGroupList = new List<ResourceGroup>();
            ResourceList = new List<Resource>();
            ResourceDataGroups = new List<ResourceDataGroup>();
            ResourceDataGroups.Add(new ResourceDataGroup("no group"));
        }

        private void OnFindProjectPath(object sender, System.Windows.RoutedEventArgs e)
        {
            projectPath = FileHelper.FindFolder(Config.GetFirstRecentEgretResourcePath());
            if (!string.IsNullOrEmpty(projectPath))
            {
                Config.AddRecentEgretResourcePath(projectPath);
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
                                    if (fileNameWithoutExtension == "default.res")
                                    {
                                        this.AnalyticalResJson(filePath);
                                        continue;
                                    }
                                    else if ("defaultTest.res" == fileNameWithoutExtension)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        string sheetImageName = GetSheetImageName(filePath);
                                        if (string.IsNullOrEmpty(sheetImageName))
                                        {
                                            ResourceList.Add(new Resource() { Name = fileNameWithoutExtension, Path = filePath, Type = ResourceType.json });
                                        }
                                        else
                                        {
                                            string[] a = sheetImageName.Split(new char[] { '.' });
                                            List<string> aa = new List<string>(filePath.Split(new char[] { '\\' }));
                                            aa.RemoveAt(aa.Count - 1);
                                            ResourceList.Add(new Resource()
                                            {
                                                Name = fileNameWithoutExtension,
                                                Path = filePath,
                                                Json = new Resource()
                                                {
                                                    Name = fileNameWithoutExtension,
                                                    Path = filePath,
                                                    Type = ResourceType.json
                                                },
                                                Image = new Resource()
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
                        //将sheet中的img资源剔除
                        var tmp = new List<Resource>(ResourceList.ToArray());
                        foreach (Resource item in tmp)
                        {
                            if (item.Type == ResourceType.sheet)
                            {
                                foreach (var i in ResourceList)
                                {
                                    if (i.Path == item.Image.Path)
                                    {
                                        ResourceList.Remove(i);
                                        break;
                                    }
                                }
                            }
                        }

                        //初始化配置中的group
                        foreach (ResGroup group in ResJson.groups)
                        {
                            List<Resource> groupRes = new List<Resource>();
                            string[] keys = group.keys.Split(new char[] { ',' });
                            foreach (string key in keys)
                            {
                                ResItem resItem = ResJson.GetResItemByName(key);
                                foreach (Resource res in ResourceList)
                                {
                                    if (res.Path.Contains(resItem.url))
                                    {
                                        res.ResName = resItem.name;
                                        res.GroupName = group.name;
                                        groupRes.Add(res);
                                        break;
                                    }
                                }
                            }
                            AddGroup(group.name, groupRes);
                        }

                        //剔除resJson中有但是却不存在的资源
                        bool b = false;
                        List<ResItem> tmpList = new List<ResItem>(ResJson.resources.ToArray());
                        foreach (ResItem item in tmpList)
                        {
                            b = false;
                            foreach (Resource res in ResourceList)
                            {
                                if (res.Path.Contains(item.url))
                                {
                                    b = true;
                                    break;
                                }
                            }
                            if (!b)
                            {
                                ResJson.resources.Remove(item);
                            }
                        }

                        //将新的资源加入resJson
                        foreach (Resource res in ResourceList)
                        {
                            ResJson.AddRes(res);
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

        private void AnalyticalResJson(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string jsonTxt = sr.ReadToEnd();
                ResJson = (ResJson)JsonConvert.DeserializeObject(jsonTxt, typeof(ResJson));
            }
        }

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

        private void OnResourceSelected(object sender, SelectedCellsChangedEventArgs e)
        {
            Resource resource = resourceDataGrid.SelectedItem as Resource;
            if (resource != null)
            {
                if (resource.Type == ResourceType.image)
                {
                    img.Source = new BitmapImage(new Uri(resource.Path, UriKind.Absolute));
                }
                else if (resource.Type == ResourceType.sheet)
                {
                    img.Source = new BitmapImage(new Uri(resource.Image.Path, UriKind.Absolute));
                }
            }
        }

        private async void OnAddGroup(object sender, System.Windows.RoutedEventArgs e)
        {
            string name = await DialogManager.ShowInputAsync(this, "资源组名", "请输入资源组名");
            if (!string.IsNullOrEmpty(name))
            {
                AddGroup(name);
            }
        }

        private void AddGroup(string name, List<Resource> resourceList = null)
        {
            ResourceGroup resourceGroup = new ResourceGroup(name, resourceList);
            this.ResourceGroupList.Add(resourceGroup);
            groupPanel.Children.Add(resourceGroup);
            ResourceDataGroup group = new ResourceDataGroup(name);
            if (resourceList != null)
            {
                group.ResourceList = resourceList;
            }
            this.ResourceDataGroups.Add(group);
        }

        private void OnGroupChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;
            if (index > -1)
            {
                Resource resource = resourceDataGrid.SelectedItem as Resource;
                string oldGroupName = resource.GroupName;
                resource.GroupName = this.ResourceDataGroups[index].GroupName;
                //foreach (ResourceGroup group in ResourceGroupList)
                //{
                //    if (group.TryRemoveResource())
                //    {
                //        break;
                //    }
                //}
                //foreach (ResourceGroup group in ResourceGroupList)
                //{
                //    if (group.TryAddResource(resource))
                //    {
                //        break;
                //    }
                //}
                ResJson.OnGroupChanged(resource, oldGroupName);
            }
        }

        private void OnExport(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(projectPath))
            {
                string strSerializeJSON = JsonConvert.SerializeObject(ResJson);
                using (StreamWriter sw = new StreamWriter(projectPath + "\\resource\\defaultTest.res.json"))
                {
                    sw.Write(strSerializeJSON);
                    DialogManager.ShowMessageAsync(this, "导出成功", "default.res.json文件已导出");
                }
            }
        }
    }

    public class Resource
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (string.IsNullOrEmpty(ResName))
                {
                    ResName = name;
                }
            }
        }

        public string ResName { get; set; }

        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                path = value.Replace("\\", "/");
            }
        }

        public string GroupName { get; set; }

        public ResourceType Type { get; set; }

        public Resource Json { get; set; }

        public Resource Image { get; set; }

        public Resource()
        {
            GroupName = "no group";
        }

        public override string ToString()
        {
            return this.ResName;
        }
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

    public class ResourceDataGroup
    {
        public string GroupName { get; set; }

        public List<Resource> ResourceList { get; set; }

        public ResourceDataGroup(string name)
        {
            GroupName = name;
            ResourceList = new List<Resource>();
        }

        public override string ToString()
        {
            return this.GroupName;
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

        internal void AddRes(Resource res)
        {
            if (!HasRes(res))
            {
                resources.Add(new ResItem()
                {
                    name = res.ResName,
                    type = Enum.GetName(typeof(ResourceType), res.Type),
                    url = res.Path.Replace(ResourceTool.projectPath.Replace("\\", "/") + "/resource/", "")
                });
            }
        }

        private bool HasRes(Resource res)
        {
            foreach (ResItem item in resources)
            {
                if (res.Path.Contains(item.url))
                {
                    return true;
                }
            }
            return false;
        }

        public ResItem GetResItemByName(string name)
        {
            foreach (ResItem item in resources)
            {
                if (item.name == name)
                {
                    return item;
                }
            }
            return null;
        }

        private ResItem GetResItemByPath(string path)
        {
            foreach (ResItem item in resources)
            {
                if (path.Contains(item.url))
                {
                    return item;
                }
            }
            return null;
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

        internal void OnGroupChanged(Resource resource, string oldGroupName)
        {
            ResItem resItem = GetResItemByPath(resource.Path);
            if (resItem != null)
            {
                ResGroup oldGroup = GetResGroup(oldGroupName);
                if (oldGroup != null)
                {
                    oldGroup.RemoveKey(resItem.name);
                }
                if (resource.GroupName != "no group")
                {
                    ResGroup newGroup = GetResGroup(resource.GroupName);
                    newGroup.AddKey(resItem.name);
                }
            }
        }
    }

    public class ResItem
    {
        public string name { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }

    public class ResGroup
    {
        public string name { get; set; }

        private List<string> _keyList;
        private string _keys;
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

        internal void RemoveKey(string name)
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

        internal void AddKey(string name)
        {
            if (!_keyList.Contains(name))
            {
                _keyList.Add(name);
                this._keys = string.Join(",", _keyList);
            }
        }
    }
}
