using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsButton.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomButton : ContentView
	{
        public event EventHandler Clicked;

        public ImageSource SourceDisabled
        {
            get { return ImageDisabled.Source; }
            set { ImageDisabled.Source = value; }
        }

        public ImageSource SourcePressed
        {
            get { return ImagePressed.Source; }
            set { ImagePressed.Source = value; }
        }

        public ImageSource Source
        {
            get { return ImageNormal.Source; }
            set { ImageNormal.Source = value; }
        }

        public string Text
        {
            get { return LabelText.Text; }
            set { LabelText.Text = value; }
        }

        public Color TextColor
        {
            get { return LabelText.TextColor; }
            set { LabelText.TextColor = value; }
        }

        public CustomButton ()
		{
			InitializeComponent ();

            // デフォルト配色をコードで設定する
            ImageNormal.Source = ImageSource.FromResource("FormsButton.Resources.button_normal.png", typeof(CustomButton).GetTypeInfo().Assembly);
            ImagePressed.Source = ImageSource.FromResource("FormsButton.Resources.button_pressed.png", typeof(CustomButton).GetTypeInfo().Assembly);
            ImageDisabled.Source = ImageSource.FromResource("FormsButton.Resources.button_disabled.png", typeof(CustomButton).GetTypeInfo().Assembly);
        }
        /// <summary>
        /// クリックイベント.
        /// Press/Releaseイベントが無いため押下時間は固定となる.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TapGestureRecognizer_TappedAsync(object sender, EventArgs e)
        {
            await ExecuteClickEffectAsync();
            Clicked?.Invoke(sender, e);
        }

        private async Task ExecuteClickEffectAsync()
        {
            await ImagePressed.FadeTo(1, 50);
            await ImagePressed.FadeTo(0.01, 200);
        }
    }
}