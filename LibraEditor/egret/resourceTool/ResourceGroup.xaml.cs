using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace LibraEditor.egret.resourceTool
{
    /// <summary>
    /// ResourceGroup.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceGroup : UserControl
    {

        public string GroupName { get; set; }

        public List<Resource> ResourceList { get; set; }

        public ResourceGroup(string groupName, List<Resource> resourceList = null)
        {
            InitializeComponent();
            this.headerExpander.Header = groupName;

            GroupName = groupName;
            ResourceList = resourceList;

            //listBox.ItemsSource = ResourceList;
        }

        internal bool TryRemoveResource()
        {
            if (ResourceList != null)
            {
                foreach (var item in ResourceList)
                {
                    if (item.GroupName != GroupName)
                    {
                        ResourceList.Remove(item);
                        //listBox.ItemsSource = ResourceList;
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool TryAddResource(Resource resource)
        {
            if (resource.GroupName == GroupName)
            {
                ResourceList.Add(resource);
                //listBox.ItemsSource = ResourceList;
                return true;
            }
            return false;
        }
    }
}
