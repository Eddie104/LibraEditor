using System.Windows.Controls;

namespace LibraEditor.egret.resourceTool
{
    /// <summary>
    /// ResourceGroup.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceGroup : UserControl
    {
        public ResourceGroup(string header)
        {
            InitializeComponent();
            this.headerExpander.Header = header;
        }
    }
}
