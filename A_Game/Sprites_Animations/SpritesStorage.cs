using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace A_Game.Classes
{
    internal class SpritesStorage
    {
        public static ImageSource Player => _playerSpritePath.GetImageSource();
        public static ImageSource Platform => _platformSpritePath.GetImageSource();
        public static ImageSource MainPlatform => _mainPlatformSpritePath.GetImageSource();
        public static ImageSource DeathZone => _deathZoneSpritePath.GetImageSource();
        public static ImageSource SpawnPoint => _spawnPointSpritePath.GetImageSource();
        public static ImageSource MovingPlatform => _movingPlatformSpritePath.GetImageSource();
        public static ImageSource MeleeAttack => _meleeAttackSpritePath.GetImageSource();
        public static ImageSource RangeAttack => _rangeAttackSpritePath.GetImageSource();
        public static ImageSource BowPickup => _bowPickUpSpritePath.GetImageSource();
        public static ImageSource SwordPickup => _swordPickUpSpritePath.GetImageSource();


        private static string _playerSpritePath = "sprites/Player/Player.png";
        private static string _platformSpritePath = "sprites/Platforms/Platform.png";
        private static string _mainPlatformSpritePath = "sprites/Platforms/FullPlatform.png";
        private static string _deathZoneSpritePath = "sprites/Enemies/DeathZone.png";
        private static string _spawnPointSpritePath = "sprites/SpawnPoints/SpawnPoint.png";
        private static string _movingPlatformSpritePath = "sprites/Platforms/MovingPlatform.png";
        private static string _meleeAttackSpritePath = "sprites/Attacks/MeleeAttack.png";
        private static string _rangeAttackSpritePath = "sprites/Attacks/RangeAttack.png";
        private static string _bowPickUpSpritePath = "sprites/Pickups/Bow.png";
        private static string _swordPickUpSpritePath = "sprites/Pickups/Sword.png";
	}
}
