using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace A_Game.Classes
{
    internal class Camera : IUpdateHandler
    {
        private Player _target;//player
        private CanvasParameters _canvas;

        private Vector _cameraPosition;
        private float _cameraSpeed = 10f;

        public Camera(Player target, CanvasParameters canvas) 
        {
            _target = target;
            _canvas = canvas;
        }

        public void Update()
        {
            UpdateCameraPosition();
        }
        private void UpdateCameraPosition()
        {
            // Вычисляем отдаление игрока от центра
            double offsetX = _target.Position.X - _canvas.Center.X;
            double offsetY = _target.Position.Y - _canvas.Center.Y;

            // Если игрок отдалился на больше чем 100px, сдвигаем камеру
            if (Math.Abs(offsetX) > 100)
            {
                _cameraPosition.X += Math.Sign(offsetX) * _cameraSpeed; // Перемещаем камеру по оси X
            }

            if (Math.Abs(offsetY) > 100)
            {
                _cameraPosition.Y += Math.Sign(offsetY) * _cameraSpeed; // Перемещаем камеру по оси Y
            }

            // Ограничиваем движение камеры границами
            _cameraPosition.X = Math.Max(0, Math.Min(_cameraPosition.X, _canvas.Instance.ActualWidth - _canvas.Center.X * 2));
            _cameraPosition.Y = Math.Max(0, Math.Min(_cameraPosition.Y, _canvas.Instance.ActualHeight - _canvas.Center.Y * 2));
        }
    }
}
