using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using A_Game.Classes.Interfaces;

namespace A_Game
{
    internal static class Extensions
    {
        public static ImageSource GetImageSource(this string imgPath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imgPath, UriKind.Relative); // Путь к файлу
            bitmap.EndInit();
            return bitmap;
        }
        public static bool TryGetObject<T>(this IEnumerable<IGameObject> gameObjects, out T result) where T : class
        {
            result = gameObjects.OfType<T>().FirstOrDefault();
            return result != null;
        }
    }
}
