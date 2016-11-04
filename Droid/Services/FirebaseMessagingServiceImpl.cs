using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Firebase.Messaging;

namespace TryFirebaseXamarin.Droid
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
	public class FirebaseMessagingServiceImpl : FirebaseMessagingService
	{
		// application is in the foreground, this method will fire.
		public override void OnMessageReceived(RemoteMessage message)
		{
			System.Diagnostics.Debug.WriteLine(message.GetNotification().Body);
			showNotification(message);
		}

		void showNotification(RemoteMessage message) 
		{
			var intent = new Intent(this, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.ClearTop);
			var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

			var notificationBuilder = new NotificationCompat.Builder(this)
			                                                .SetSmallIcon(Resource.Drawable.icon)
			                                                .SetContentTitle(message.GetNotification().Title ?? "from Firebase")
			                                                .SetContentText(message.GetNotification().Body)
			                                                .SetAutoCancel(true)
			                                                .SetContentIntent(pendingIntent);

			var notificationManager = NotificationManager.FromContext(this);

			notificationManager.Notify(0, notificationBuilder.Build());
		}
	}
}
