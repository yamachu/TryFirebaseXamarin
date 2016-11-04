using Xamarin.Forms;

namespace TryFirebaseXamarin
{
	public partial class TryFirebaseXamarinPage : ContentPage
	{
		public TryFirebaseXamarinPage()
		{
			InitializeComponent();
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			var dictionary = new System.Collections.Generic.Dictionary<string, object>();
			dictionary.Add("Button", (sender as Button).Text);

			DependencyService.Get<Services.IFirebaseLogEventService>()
							 .LogEvent("Clicked", dictionary);
		}
	}
}
