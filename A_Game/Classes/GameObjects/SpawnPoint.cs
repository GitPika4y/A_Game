using A_Game.Classes.Helpers;
using A_Game.Classes.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace A_Game.Classes.GameObjects
{
    [Serializable]
    public class SpawnPoint : IGameObject, ICollider, IInputable
    {

		public Image GameObject => _gameObject;
        public Vector Position => _position;
        public Dictionary<string, double> ColliderBounds => _colliderBounds;


        public (int x, int y) CurrentScene { get; }
        public event Action UpdateSpawnPoint;


        private Dictionary<string, double> _colliderBounds;
		[NonSerialized] private Image _gameObject;
        private Vector _position;

        public SpawnPoint(ImageSource gameObject,Vector position, (int x, int y) currentScene)
        {
            _position = position;
            _gameObject = new Image
            {
                Source = gameObject,
                Width = gameObject.Width,
                Height = gameObject.Height,

            };

            CurrentScene = currentScene;

            GameObject.UpdateColllisionBounds(Position, out _colliderBounds);

        }

		public override bool Equals(object obj)
		{
			if (obj is SpawnPoint other)
			{
				return Position == other.Position &&
					   CurrentScene == other.CurrentScene; // Не сравниваем Image
			}
			return false;
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash = hash * 31 + Position.GetHashCode();
			hash = hash * 31 + CurrentScene.GetHashCode();
			return hash;
		}


		public void OnKeyDown(Key key)
        {
            if(key == Key.Enter)
            {
                UpdateSpawnPoint?.Invoke(); //Обновляем spawn point
                InteractableHelper.ConfirmInteraction();
            }
        }

        public void OnKeyUp(Key key) {}
    }
}
