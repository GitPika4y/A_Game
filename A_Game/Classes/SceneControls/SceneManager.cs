using A_Game.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Game.Classes.SceneManager
{
    internal class SceneManager : IUpdateHandler
    {

        private CanvasParameters _canvas;
        private MainWindow _mainWindow;
        private Player _player;
        private (int x, int y) _currentScene = (0,0);

        public SceneManager(CanvasParameters canvas, Player player)
        {
            _canvas = canvas;
            _player = player;
            _mainWindow = MainWindow.Instance;
        }

        public void Update()
        {
            if (IsPlayerOutOfScene(out int xDirection, out int yDirection))
            {
                TryLoadScene(xDirection, yDirection);
            }
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
                _currentScene = targetScene;
                LoadScene(_currentScene); // Загружаем новую сцену
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
            _canvas.RemoveGameObjects(); //Удаляем элементы текущей сцена
            _mainWindow.RemoveHandlersAndColliders(); //Удаляем handlers и Colliders


            var sceneGameObjects = SceneStorage.Scenes[scene];

            _canvas.AddGameObjects(sceneGameObjects); //Добавляем новую сцену(элементы на сцену)
            AddToHandlersAndColliders(sceneGameObjects); //Добавляем Handlers и Colliders
        }


        private void AddToHandlersAndColliders(List<IGameObject> gameObjects)
        {

            foreach (var gameObject in gameObjects)
            {
                if(gameObject is ICollider collider)
                    CollisionControl.Colliders.Add(collider);
                if(gameObject is IUpdateHandler updateHandler)
                    _mainWindow.AddUpdateHandler(updateHandler);
                if(gameObject is IInputHandler inputHandler)
                    _mainWindow.AddInputHandler(inputHandler);

            }
        }


    }
}
