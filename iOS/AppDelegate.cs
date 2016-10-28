using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using UserNotifications;

using Firebase.CloudMessaging;

namespace TryFirebaseXamarin.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			// get permission for notification
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				// iOS 10
				var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
				UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
				{
					Console.WriteLine(granted);
				});

				// For iOS 10 display notification (sent via APNS)
				UNUserNotificationCenter.Current.Delegate = this;
			}
			else {
				// iOS 9 <=
				var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
				var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			}

			UIApplication.SharedApplication.RegisterForRemoteNotifications();

			Firebase.Analytics.App.Configure();

			Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) => {
				var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
				// if you want to send notification per user, use this token
				System.Diagnostics.Debug.WriteLine(newToken);

				connectFCM();
			});

			return base.FinishedLaunching(app, options);
		}

		public override void DidEnterBackground(UIApplication uiApplication)
		{
			Messaging.SharedInstance.Disconnect();
		}

		public override void OnActivated(UIApplication uiApplication)
		{
			connectFCM();
			base.OnActivated(uiApplication);
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
#if DEBUG
			Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Sandbox);
#endif
#if RELEASE
			Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Prod);
#endif
		}

		// iOS 9 <=, fire when recieve notification foreground
		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

			if (application.ApplicationState == UIApplicationState.Active) {
				System.Diagnostics.Debug.WriteLine(userInfo);
				var aps_d = userInfo["aps"] as NSDictionary;
				var alert_d = aps_d["alert"] as NSDictionary;
				var body = alert_d["body"] as NSString;
				var title = alert_d["title"] as NSString;
				debugAlert(title, body);
			}
		}

		// iOS 10, fire when recieve notification foreground
		[Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
		public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			var title = notification.Request.Content.Title;
			var body = notification.Request.Content.Body;
			debugAlert(title, body);
		}

		private void connectFCM()
		{
			Messaging.SharedInstance.Connect((error) =>
			{
				if (error == null)
				{
					Messaging.SharedInstance.Subscribe("/topics/all");
				}
				System.Diagnostics.Debug.WriteLine(error != null ? "error occured" : "connect success");
			});
		}

		private void debugAlert(string title, string message) 
		{
			var alert = new UIAlertView(title ?? "Title", message ?? "Message", null, "Cancel", "OK");
			alert.Show();
		}
	}
}
