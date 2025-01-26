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
        public static ImageSource PlayerSprite => _playerSpritePath.GetImageSource();
        public static ImageSource PlatformSprite => _platformSpritePath.GetImageSource();
        public static ImageSource MainPlatformSprite => _mainPlatformSpritePath.GetImageSource();

        private static string _playerSpritePath = "sprites/Player.png";
        private static string _platformSpritePath = "sprites/Platform.png";
        private static string _mainPlatformSpritePath = "sprites/FullPlatform.png";
    }
}
