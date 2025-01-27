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


        private static string _playerSpritePath = "sprites/Player.png";
        private static string _platformSpritePath = "sprites/Platform.png";
        private static string _mainPlatformSpritePath = "sprites/FullPlatform.png";
        private static string _deathZoneSpritePath = "sprites/DeathZone.png";
        private static string _spawnPointSpritePath = "sprites/SpawnPoint.png";
    }
}
