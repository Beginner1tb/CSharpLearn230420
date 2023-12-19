using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _17.InterfaceChildView.IDepository
{
    public interface IChildView
    {
        void UpdateContent(string content);

        void ShowImg(BitmapImage bitmapimage);
    }

}
