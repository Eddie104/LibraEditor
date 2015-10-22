using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraEditor.mapEditor.events
{
    class VisibleChangedEventArgs : EventArgs
    {

        public bool IsVisible { get; set; }

        public VisibleChangedEventArgs(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}
