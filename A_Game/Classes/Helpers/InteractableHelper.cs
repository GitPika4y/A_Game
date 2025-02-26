using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using A_Game.Classes.Interfaces;
using A_Game.Classes.SceneControls;

namespace A_Game.Classes.Helpers
{
	public static class InteractableHelper
	{
		private static TextBlock _messageText;
		private static bool _isMessageWindowActive = false;
		private static TaskCompletionSource<bool> _messageTaskCompletion;

		static InteractableHelper()
		{
			_messageText = new TextBlock
			{
				Visibility = Visibility.Collapsed,
				FontSize = 20,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};

			SceneManager.SceneChanged += ConfirmInteraction;
		}

		public static async Task ShowMessageWindow(IGameObject gameObject, string message, Vector offset)
		{
			if (_isMessageWindowActive) return;

			_isMessageWindowActive = true;
			_messageText.Text = message;
			_messageText.Visibility = Visibility.Visible;

			Canvas.SetLeft(_messageText, gameObject.Position.X + offset.X);
			Canvas.SetBottom(_messageText, gameObject.Position.Y + gameObject.GameObject.Height + offset.Y);

			CanvasParameters.Instance.Children.Add(_messageText);
			_messageTaskCompletion = new TaskCompletionSource<bool>();

			await Task.WhenAny(_messageTaskCompletion.Task, Task.Delay(1200));

			_isMessageWindowActive = false;
			_messageText.Visibility = Visibility.Collapsed;
			CanvasParameters.Instance.Children.Remove(_messageText);
			_messageTaskCompletion = null;
		}

		public static void ConfirmInteraction()
		{
			_messageTaskCompletion?.TrySetResult(true);
		}
	}
}
