using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace libra.util
{
    class FileHelper
    {

        public static string FindFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            return dialog.ShowDialog() == DialogResult.OK ? dialog.SelectedPath : null;
        }

    }
}
