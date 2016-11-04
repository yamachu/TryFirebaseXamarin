using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase.Analytics;

namespace TryFirebaseXamarin.Droid
{
	[Activity(Label = "TryFirebaseXamarin.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		private FirebaseAnalytics firebaseAnalytics;

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			// Initialize firebase instance
			firebaseAnalytics = FirebaseAnalytics.GetInstance(this);

			var analyticsData = new Bundle();
			analyticsData.PutString("Event_type", "App_open");
			firebaseAnalytics.LogEvent("CustomEvent", analyticsData);

			LoadApplication(new App());
		}
	}
}
