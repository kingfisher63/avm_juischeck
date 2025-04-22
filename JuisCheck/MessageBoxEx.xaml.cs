using System;
using System.Drawing;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#pragma warning disable CA1819	// Properties should not return arrays

namespace JuisCheck
{
	public enum MessageBoxExImage
	{
		None		= 0,
		Question	= 1,
		Information	= 2,
		Warning		= 3,
		Error		= 4
	}

	public sealed class MessageBoxExParams
	{
		public string[]				ButtonText		{ get; set; } = { "OK" };
		public string				CaptionText		{ get; set; } = null;
		public string				CheckboxText	{ get; set; } = null;
		public bool					CheckboxValue	{ get; set; } = false;
		public int					DefaultButton	{ get; set; } = -1;
		public MessageBoxExImage	Image			{ get; set; } = MessageBoxExImage.None;
		public string				MessageText		{ get; set; } = null;
		public Window				Owner			{ get; set; } = null;
		public SystemSound			Sound			{ get; set; } = null;
	}

	public sealed partial class MessageBoxEx : Window
	{
		//
		// Static members
		//

		public static int Show(MessageBoxExParams msgParams)
		{
			return Show(msgParams, false, out bool _);
		}

		public static int Show(MessageBoxExParams msgParams, out bool cbValue)
		{
			return Show(msgParams, true, out cbValue);
		}

		private static int Show(MessageBoxExParams msgParams, bool cbShow, out bool cbValue)
		{
			if (msgParams == null) {
				throw new ArgumentNullException(nameof(msgParams));
			}

			if (msgParams.MessageText == null) {
				throw new ArgumentException($"{nameof(msgParams)}.{nameof(msgParams.MessageText)}");
			}
			if (msgParams.CheckboxText == null && cbShow) {
				throw new ArgumentException($"{nameof(msgParams)}.{nameof(msgParams.CheckboxText)}");
			}
			if (msgParams.ButtonText == null) {
				throw new ArgumentException($"{nameof(msgParams)}: {nameof(msgParams.ButtonText)}");
			}
			if (msgParams.ButtonText.Length == 0) {
				throw new ArgumentException($"{nameof(msgParams)}.{nameof(msgParams.ButtonText)}");
			}

			if (msgParams.Sound != null) {
				msgParams.Sound.Play();
			}

			MessageBoxEx msgBox = new MessageBoxEx() {
				Title = msgParams.CaptionText ?? string.Empty
			};

			if (msgParams.Owner != null) {
				try {
					msgBox.Owner                 = msgParams.Owner;
					msgBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				}
				catch (InvalidOperationException) {
					msgBox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				}
			} else {
				msgBox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			}

			msgBox.SetButtons(msgParams.ButtonText);
			msgBox.SetCheckbox(cbShow, msgParams.CheckboxText, msgParams.CheckboxValue);
			msgBox.SetImage(msgParams.Image);
			msgBox.SetMessage(msgParams.MessageText);
			msgBox.ShowDialog();

			cbValue = (bool)msgBox.cbCheckbox.IsChecked;

			return msgBox.result;
		}

		//
		// Other members
		//

		private int result = -1;

		private MessageBoxEx()
		{
			InitializeComponent();
		}

		private void AddButton(string text, int result)
		{
			Button button = new Button {
				Content = text,
				Tag     = result
			};
			button.Click += Button_Click_Handler;

			ugButtons.Children.Add(button);
			ugButtons.Columns++;
		}

		private void SetButtons(string[] buttonText)
		{
			for (int i = 0; i < buttonText.Length; i++) {
				AddButton(buttonText[i], i);
			}
		}

		private void SetCheckbox(bool cbShow, string cbText, bool cbValue)
		{
			dpCheckbox.Visibility = cbShow ? Visibility.Visible : Visibility.Collapsed;

			if (cbShow) {
				cbCheckbox.IsChecked = cbValue;

				foreach (string line in cbText.Split('\n')) {
					tbkCheckboxText.Inlines.Add(new Run(line));
					tbkCheckboxText.Inlines.Add(new LineBreak());
				}
			}
		}

		private void SetImage(MessageBoxExImage image)
		{
			Icon icon;

			switch (image) {
				case MessageBoxExImage.Error:		icon = SystemIcons.Error; break;
				case MessageBoxExImage.Information:	icon = SystemIcons.Information; break;
				case MessageBoxExImage.None:		icon = null; break;
				case MessageBoxExImage.Question:	icon = SystemIcons.Question; break;
				case MessageBoxExImage.Warning:		icon = SystemIcons.Warning; break;

				default:
					throw new ArgumentOutOfRangeException(nameof(image));
			}

			if (icon == null) {
				imgMessageIcon.Visibility = Visibility.Collapsed;
			} else {
				imgMessageIcon.Source     = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
				imgMessageIcon.Visibility = Visibility.Visible;
			}
		}

		private void SetMessage(string text)
		{
			foreach (string line in text.Split('\n')) {
				tbkMessageText.Inlines.Add(new Run(line));
				tbkMessageText.Inlines.Add(new LineBreak());
			}
		}

		//
		// Event handlers
		//

		private void Button_Click_Handler(object sender, RoutedEventArgs evt)
		{
			result = (int)(sender as Button).Tag;
			Close();
		}

		private void KeyUp_Handler(object sender, KeyEventArgs evt)
		{
			if (evt.Key == Key.Escape) {
				Close();
			}
		}
	}
}
