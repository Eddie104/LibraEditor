using LibraEditor.mapEditor.events;
using LibraEditor.mapEditor.model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LibraEditor.mapEditor.view.mapLayer
{
    /// <summary>
    /// LayerItem.xaml 的交互逻辑
    /// </summary>
    public partial class LayerItem : UserControl
    {

        public event EventHandler VisibleChangedEvent;

        public bool IsCanVisible { get; set; }

        public LayerData LayerData { get; set; }

        public LayerItem(LayerData layerData)
        {
            InitializeComponent();

            IsCanVisible = true;
            LayerData = layerData;
            this.nameLabel.Content = LayerData.Name;
        }

        private void OnVisibleChanged(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                IsCanVisible = !IsCanVisible;
                eyeImage.Source = new BitmapImage(new Uri(IsCanVisible ? "/LibraEditor;component/Resources/eye_24.png" : "/LibraEditor;component/Resources/x_24.png", UriKind.RelativeOrAbsolute));
                VisibleChangedEvent(this, new VisibleChangedEventArgs(IsCanVisible, LayerData.Name));
            }
        }
    }
}
