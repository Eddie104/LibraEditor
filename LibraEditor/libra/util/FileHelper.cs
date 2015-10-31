using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace libra.util
{
    class FileHelper
    {

        public static string FindFolder(string selectedPath = null)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                dialog.SelectedPath = selectedPath;
            }
            return dialog.ShowDialog() == DialogResult.OK ? dialog.SelectedPath : null;
        }

        /// <summary>
        /// 查找文件
        /// </summary>
        /// <param name="filter">文件格式过滤器 比如 "*.png;*.jpg"</param>
        /// <param name="initialDirectory">初始文件夹，可选</param>
        /// <returns>选择的文件路径数组</returns>
        public static string[] FindFile(string filter, string initialDirectory = null)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            //检查文件是否存在
            openFile.CheckFileExists = true;
            //检查路径是否存在
            openFile.CheckPathExists = true;
            //是否允许多选，false表示单选
            openFile.Multiselect = true;
            if (initialDirectory != null)
            {
                openFile.InitialDirectory = initialDirectory;
            }
            
            openFile.Filter = "files (" + filter + ")|" + filter + "|All files (*.*)|*.*";//这里设置的是文件过滤器，比如选了txt文件，那别的文件就看不到了
            if (openFile.ShowDialog() == DialogResult.OK)//打开文件选择器，并按下选择按钮
            {
                return openFile.FileNames;
            }
            return null;
        }

        /// <summary>
        /// 查找指定目录下的所有文件
        /// </summary>
        /// <param name="dir">指定的目录</param>
        /// <returns></returns>
        public static List<string> FindFile(string dir)  
        {
            List<string> result = new List<string>();
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            //查找子目录
            foreach (DirectoryInfo d in dirInfo.GetDirectories())
            {
                result.AddRange(FindFile(dir + "\\" + d.ToString()));
            }
            //查找文件
            foreach (FileInfo f in dirInfo.GetFiles("*.*"))
            {
                result.Add(dirInfo + "\\" + f.ToString());
            }
            return result;
        }

    }
}
