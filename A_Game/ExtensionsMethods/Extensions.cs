using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace A_Game
{
    internal static class Extensions
    {
        public static ImageSource GetImageSource(this string mainPath, string imgPath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(mainPath+imgPath, UriKind.Absolute); // Путь к файлу
            bitmap.EndInit();
            return bitmap;
        }
    }
}
