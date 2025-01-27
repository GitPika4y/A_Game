using System;
using System.Collections.Generic;
using A_Game.Classes.Interfaces;
using A_Game.Classes.GameObjects;

namespace A_Game.Classes
{
    internal class CollisionControl
    {
        public static List<ICollider> Colliders = new List<ICollider>(); // Список всех объектов, участвующих в коллизияхw
        private string _collisionDirection;
        public static event Action<(int, int)> OnPlayerDied;


        /// <summary>
        /// Проверяет столкновения игрока с другими объектами и обрабатывает их.
        /// </summary>
        /// <param name="player">Игрок, для которого проверяются столкновения</param>
        public void HandleCollisions(Player player)
        {
            if(Colliders.Count == 0 || Colliders == null) return;

            foreach (var collider in Colliders)
            {
                if (collider != player && CheckCollision(player, collider, out _collisionDirection)) //Произошло столкновение
                {
                    ProcessCollision(player, collider);
                    return;
                }
            }
        }

        /// <summary>
        /// Проверяет, пересекаются ли два объекта.
        /// </summary>
        private bool CheckCollision(ICollider a, ICollider b, out string collisionDirection)
        {
            collisionDirection = null;

            // Проверка на пересечение границ
            bool isColliding =
                a.ColliderBounds["Left"] < b.ColliderBounds["Right"] &&
                a.ColliderBounds["Right"] > b.ColliderBounds["Left"] &&
                a.ColliderBounds["Top"] > b.ColliderBounds["Bottom"] &&
                a.ColliderBounds["Bottom"] < b.ColliderBounds["Top"];

            if (isColliding == false) //Если нет коллизии
                return false;

            // Вычисление направления столкновения, с помощью минимального перекрытия (smallestOverlap), чтобы понять, какое направление доминирует в столкновении.
            double overlapTop = b.ColliderBounds["Bottom"] - a.ColliderBounds["Top"];
            double overlapBottom = a.ColliderBounds["Bottom"] - b.ColliderBounds["Top"];
            double overlapLeft = b.ColliderBounds["Right"] - a.ColliderBounds["Left"];
            double overlapRight = a.ColliderBounds["Right"] - b.ColliderBounds["Left"];

            double smallestOverlap = Math.Min(Math.Min(overlapTop, overlapBottom), Math.Min(overlapLeft, overlapRight));

            if (smallestOverlap == overlapTop)
            {
                collisionDirection = "Top";
            }
            else if (smallestOverlap == overlapBottom)
            {
                collisionDirection = "Bottom";
            }
            else if (smallestOverlap == overlapLeft)
            {
                collisionDirection = "Left";
            }
            else if (smallestOverlap == overlapRight)
            {
                collisionDirection = "Right";
            }

            return true;
        }

        /// <summary>
        /// Обрабатывает коллизию игрока с другим объектом.
        /// </summary>
        private void ProcessCollision(Player player, ICollider collidedObject)
        {

            switch(collidedObject)
            {
                case Platform platform:
                    HandlePlatformCollision(player, platform);
                    break;
                case DeathZone deathZone:
                    HandleDeathZoneCollision(player);
                    break;
            }
        }


        /// <summary>
        /// Логика обработки столкновения игрока с платформой.
        /// </summary>
        private void HandlePlatformCollision(Player player, Platform platform)
        {
            switch (_collisionDirection)
            {
                case "Top":
                    player.SetPosition(player.Position.X, platform.ColliderBounds["Top"]);
                    break;
                case "Bottom":
                    player.SetPosition(player.Position.X, platform.ColliderBounds["Bottom"]-player.GameObject.Height);
                    break;
                case "Left":
                    player.SetPosition(platform.ColliderBounds["Left"], player.Position.Y);
                    break;
                case "Right":
                    player.SetPosition(platform.ColliderBounds["Right"], player.Position.Y);
                    break;
            }
            player.IsOnGround = true;
        }
        private void HandleDeathZoneCollision(Player player)
        {
            OnPlayerDied?.Invoke(CanvasParameters.SpawnPoint.CurrentScene);
            player.SetPosition(CanvasParameters.SpawnPoint.Position.X, CanvasParameters.SpawnPoint.Position.Y);
        }


    }
}
