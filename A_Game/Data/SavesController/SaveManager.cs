using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using A_Game.Classes;
using System.IO;
using System.Runtime.Serialization;
using A_Game.Classes.GameObjects;
using A_Game.Data.ProgressEvents;

namespace A_Game.Data.Saves
{

	
	public class SaveManager
	{
		string path = "";

		public static void SaveGame((double x, double y) playerPosition, (int x, int y) scene)
		{

			SaveData saveData = new SaveData
			{
				DateTime = DateTime.Now,
				SpawnPoint = CanvasParameters.SpawnPoint,
				EventFlags = ProgressEventsData.EventFlags,
		};

			using(FileStream fs = new FileStream("save.dat", FileMode.Create))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(fs, saveData);
			}
		}


		public static SaveData LoadGame() 
		{
			if(!File.Exists("save.dat"))
				return null;

			try
			{
				using (FileStream fs = new FileStream("save.dat", FileMode.Open))
				{
					BinaryFormatter formatter = new BinaryFormatter();
					return (SaveData)formatter.Deserialize(fs);
				}
			}
			catch (SerializationException ex)
			{
				Console.WriteLine("SerializationException: " + ex.Message);
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
				return null;
			}

		}

	}


	[Serializable]
	public class SaveData 
	{
		public DateTime DateTime;
		public SpawnPoint SpawnPoint;
		public Dictionary<Events, bool> EventFlags;
	}

}
