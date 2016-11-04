using System;
using Android.App;
using Firebase.Iid;

namespace TryFirebaseXamarin.Droid
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
	public class FirebaseInstanceIdServiceImpl : FirebaseInstanceIdService
	{
		public override void OnTokenRefresh()
		{
			// Get updated InstanceID token.
			var refreshedToken = FirebaseInstanceId.Instance.Token;
			System.Diagnostics.Debug.WriteLine(refreshedToken);
		}
	}
}
