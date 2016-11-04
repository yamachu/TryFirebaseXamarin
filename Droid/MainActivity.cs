using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase.Messaging;
using Firebase.Iid;
using System.Threading.Tasks;

namespace TryFirebaseXamarin.Droid
{
	[Activity(Label = "TryFirebaseXamarin.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			// 不明点
			// Debugビルドだと以下のようにDeleteInstanceId()を呼んでInstanceIdをリフレッシュしないと再起動後通知が受け取れない
			// Releaseビルドだと以下の作業は不要でSubscribeするだけでそのトピックへの通知を受け取ることができる
#if DEBUG
			Task.Run(() =>
			{
				var instanceID = FirebaseInstanceId.Instance;
				instanceID.DeleteInstanceId();
				var iid1 = instanceID.Token;
				var iid2 = instanceID.GetToken(GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope);
				FirebaseMessaging.Instance.SubscribeToTopic("all");
			});
#else
			FirebaseMessaging.Instance.SubscribeToTopic("all");
#endif			    

			LoadApplication(new App());
		}
	}
}
