using System;
using Android.Content;
using Android.OS;

namespace TryFirebaseXamarin.Droid
{
	public class FirebaseInstanceManager
	{
		private static FirebaseInstanceManager instance;
		private Firebase.Analytics.FirebaseAnalytics firebaseAnalytics;

		private FirebaseInstanceManager() {}

		public static void Init(Context context) 
		{
			if (instance == null) {
				instance = new FirebaseInstanceManager();
				instance.firebaseAnalytics = Firebase.Analytics.FirebaseAnalytics.GetInstance(context);
			}
		}

		public static FirebaseInstanceManager Instance
		{
			get
			{
				if (instance == null) {
					throw new Exception("You must call FirebaseInstanceManager.Init(context) before use instance");
				}

				return instance;
			}
		}

		public void SendUserLogEvent(string key, Bundle values) => firebaseAnalytics.LogEvent(key, values);
	}
}
