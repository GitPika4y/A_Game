using A_Game.Classes.Collisions;
using A_Game.Classes.Interfaces;
using A_Game.Pages;
using System;
using System.Collections.Generic;

namespace A_Game.Classes.SceneControls
{
    internal class SceneManager : IUpdateble
    {

        private CanvasParameters _canvas;
        private Player _player;
        private static (int x, int y) _currentScene = (0,0);

        public static event Action SceneChanged;

        public SceneManager(CanvasParameters canvas, Player player)
        {
            _canvas = canvas;
            _player = player;
            PlayerCollisionHandler.OnPlayerDied += LoadSceneOnDied;
        }

        public void Update()
        {
			//Отслеживание позиции игрока за выход экрана
			if (IsPlayerOutOfScene(out int xDirection, out int yDirection)) 
            {
                TryLoadScene(xDirection, yDirection);
            }
        }

        public static (int x, int y) GetCurrentScene()
        {
            return _currentScene;
        }

        private bool IsPlayerOutOfScene(out int xDirection, out int yDirection)
        {
            xDirection = 0; //С какой x границы игрок вышел
            yDirection = 0;//С какой y границы игрок вышел

            // Проверяем по горизонтали
            if (_player.ColliderBounds["Left"] < CanvasParameters.Bounds["Left"]) // Левый край экрана
            {
                xDirection = -1;
            }
            else if (_player.ColliderBounds["Right"] > CanvasParameters.Bounds["Right"]) // Правый край экрана
            {
                xDirection = 1;
            }

            // Проверяем по вертикали
            if (_player.ColliderBounds["Bottom"] < CanvasParameters.Bounds["Bottom"]) // Нижний край экрана
            {
                yDirection = -1;
            }
            else if (_player.ColliderBounds["Top"] > CanvasParameters.Bounds["Top"]) // Верхний край экрана
            {
                yDirection = 1;
            }

            // Определяем, вышел ли игрок за границы 
            return xDirection != 0 || yDirection != 0;
        }

        private void TryLoadScene(int xDirection, int yDirection)
        {
            // Рассчитываем целевую сцену
            var targetScene = (_currentScene.x + xDirection, _currentScene.y + yDirection);

            if (SceneStorage.Scenes.ContainsKey(targetScene)) // Если сцена существует
            {
                LoadScene(targetScene); // Загружаем новую сцену
                SetPlayerInNewScene(xDirection, yDirection);
            }
            else // Если сцены нет, не даем выйти за пределы текущей сцены
            {
                SetPlayerInNewScene(-xDirection, -yDirection);//Инвертируем край чтобы не выйти
            }
        }

        private void SetPlayerInNewScene(int xDirection, int yDirection) //Из какого края
        {
            if (xDirection == 1) // Из правого в Левый край
            {
                _player.SetPosition(CanvasParameters.Bounds["Left"], _player.Position.Y);
            }
            else if (xDirection == -1) //Из Левого в Правый край
            {
                _player.SetPosition(CanvasParameters.Bounds["Right"] - _player.GameObject.Width, _player.Position.Y);
            }

            if (yDirection == 1) // из Верхнего в Нижний край
            {
                _player.SetPosition(_player.Position.X, CanvasParameters.Bounds["Bottom"]);
            }
            else if (yDirection == -1) //из Нижнего в Верхний край
            {
                _player.SetPosition(_player.Position.X, CanvasParameters.Bounds["Top"] - _player.GameObject.Height);
            }
        }

        public void LoadScene( (int x, int y) scene)
        {
            _currentScene = scene;

            _canvas.RemoveGameObjects(); //Удаляем элементы текущей сцена

            var sceneGameObjects = SceneStorage.Scenes[scene];

            _canvas.AddGameObjects(sceneGameObjects); //Добавляем новую сцену(элементы на сцену)

            SceneChanged?.Invoke();
        }

        public void LoadSceneOnDied()
        {
            LoadScene(CanvasParameters.SpawnPoint.CurrentScene);
        }


    }
}
