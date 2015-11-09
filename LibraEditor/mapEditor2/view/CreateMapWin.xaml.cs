using MahApps.Metro.Controls;
using System.Windows;

namespace LibraEditor.mapEditor2.view
{
    /// <summary>
    /// CreateMapWin.xaml 的交互逻辑
    /// </summary>
    public partial class CreateMapWin : MetroWindow
    {
        public delegate void CreateMap(string name, int rows, int cols);
        public event CreateMap CreateMapHandler;

        public CreateMapWin()
        {
            InitializeComponent();
        }

        private void OnCreateMap(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            if (!string.IsNullOrEmpty(name))
            {
                int rows = (int)rowsNumeric.Value;
                int cols = (int)colsNumeric.Value;
                CreateMapHandler(name, rows, cols);
            }
        }
    }
}
