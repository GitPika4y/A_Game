using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using A_Game.Classes.Interfaces;
using System.Windows;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Diagnostics;
using A_Game.Classes;

namespace A_Game
{
    internal static class Extensions
    {

        //Получить ImageSource (картинку для GameObject)
        public static ImageSource GetImageSource(this string imgPath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imgPath, UriKind.Relative); // Путь к файлу
            bitmap.EndInit();
            return bitmap;
        }

        //Попытка получить IGameObject (при true возвращает его)
        public static bool TryGetObject<T>(this IEnumerable<IGameObject> gameObjects, out T result) where T : class
        {
            result = gameObjects.OfType<T>().FirstOrDefault();
            return result != null;
        }

        //Движение объектов 
        public static async Task MoveTo(this IMovable gameObject, Vector distance, float time, bool isLooping = false)
        {
            if (gameObject.IsMoving) return;
            gameObject.IsMoving = true;

            Stopwatch stopwatch = Stopwatch.StartNew();
            Vector startPosition = gameObject.Position;
            float frameTime = 16; // Ограничение обновления позиции раз в 16 мс (~60 FPS)

            while (stopwatch.ElapsedMilliseconds < time * 1000)
            {
                float progress = stopwatch.ElapsedMilliseconds / (time * 1000f);
                gameObject.SetPosition(startPosition.X + distance.X * progress, startPosition.Y + distance.Y * progress);
                await Task.Delay((int)frameTime);
            }

            gameObject.SetPosition(startPosition.X + distance.X, startPosition.Y + distance.Y); // Гарантируем точное попадание

            if (isLooping)
            {
                gameObject.IsMoving = false;
                await gameObject.MoveTo(-distance, time, isLooping);
            }
        }

        //Обновление коллизии у GameObject
        public static void UpdateColllisionBounds(this Image gameObject, Vector position, out Dictionary<string, double> colliderBounds, double collisionOffSet = 0)
        {
            colliderBounds = new Dictionary<string, double>()
            {
                ["Top"] = position.Y + gameObject.Height,
                ["Bottom"] = position.Y,
                ["Left"] = position.X + collisionOffSet,
                ["Right"] = position.X + gameObject.Width - collisionOffSet,
            };
        }

        public static bool IsObjectOutOfScreen(this ICollider gameObject)
        {
            bool result = false;

			if (gameObject.ColliderBounds["Left"] < CanvasParameters.Bounds["Left"]) // Левый край экрана
			{
                result = true;
			}
			else if (gameObject.ColliderBounds["Right"] > CanvasParameters.Bounds["Right"]) // Правый край экрана
			{
				result = true;
			}
			// Проверяем по вертикали
			else if (gameObject.ColliderBounds["Bottom"] < CanvasParameters.Bounds["Bottom"]) // Нижний край экрана
			{
				result = true;
			}
			else if (gameObject.ColliderBounds["Top"] > CanvasParameters.Bounds["Top"]) // Верхний край экрана
			{
				result = true;
			}

            return result;
		}


	}
}
