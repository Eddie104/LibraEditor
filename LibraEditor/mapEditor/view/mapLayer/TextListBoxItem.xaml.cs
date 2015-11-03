using System;
using System.Windows.Controls;

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// TextListBoxItem.xaml 的交互逻辑
    /// </summary>
    public partial class TextListBoxItem : ListBoxItem
    {

        public event EventHandler OnEdit;

        public event EventHandler OnDel;

        public object Data { get; set; }

        public TextListBoxItem(object data)
        {
            InitializeComponent();

            Data = data;
            nameLabel.Content = Data.ToString();

            ContextMenu menu = new ContextMenu();
            MenuItem editItem = new MenuItem();
            editItem.Header = "编辑";
            editItem.Click += EditItem_Click;
            menu.Items.Add(editItem);

            MenuItem delItem = new MenuItem();
            delItem.Header = "删除";
            delItem.Click += DelItem_Click;
            menu.Items.Add(delItem);

            ContextMenu = menu;
        }

        private void DelItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnDel(this, null);
        }

        private void EditItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnEdit(this, null);
        }
    }
}
