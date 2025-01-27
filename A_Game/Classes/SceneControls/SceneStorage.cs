using A_Game.Classes.Interfaces;
using A_Game.Classes.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace A_Game.Classes.SceneManager
{
    internal class SceneStorage
    {

        public static Dictionary<(int x, int y), List<IGameObject>> Scenes = new Dictionary<(int x, int y), List<IGameObject>>()
        {
            /// Y = 0
            [(0, 0)] = new List<IGameObject>
                {
                    new Platform(SpritesStorage.MainPlatform, new Vector(0,0)),
                },
            [(-1,0)] = new List<IGameObject> 
                { 
                    //new Platform(SpritesStorage.MainPlatform, new Vector(0,0)),
                    new DeathZone(SpritesStorage.DeathZone, new Vector(0,0)),
                    new Platform(SpritesStorage.Platform, new Vector(752,0)),
                    new Platform(SpritesStorage.Platform, new Vector(300, 160)),
                    new Platform(SpritesStorage.Platform, new Vector(400, 200)),
                    new Platform(SpritesStorage.Platform, new Vector(300, 250)),
                    new Platform(SpritesStorage.Platform, new Vector(400, 310)),
                },
            [(1,0)] = new List<IGameObject> 
                {
                    new DeathZone(SpritesStorage.DeathZone, new Vector(0,0)),
                    new SpawnPoint(SpritesStorage.SpawnPoint,new Vector(0, 32), (1,0)),
                    new Platform(SpritesStorage.Platform, new Vector(0,0)),
                },

            ///Y =1 (может быть сделать по локациям)
            [(-1,1)] = new List<IGameObject> 
                {
                    new Platform(SpritesStorage.Platform, new Vector(300,0)),
                },
        };



    }
}
