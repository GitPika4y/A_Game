using A_Game.Data.ProgressEvents;
using System;
using System.Collections.Generic;

[Serializable]
public class ProgressEventsData
{
	public static event Action OnLoadEventFlags;

	public static Dictionary<Events, bool> EventFlags = new Dictionary<Events, bool>();

	public static bool GetEvent(Events eventName) =>
		EventFlags.TryGetValue(eventName, out bool value) && value;

	public static void SetEvent(Events eventName, bool value) =>
		EventFlags[eventName] = value;

	public static void LoadEventFlags(Dictionary<Events, bool> eventFlags)
	{
		EventFlags = eventFlags;
		OnLoadEventFlags?.Invoke();
	}
}