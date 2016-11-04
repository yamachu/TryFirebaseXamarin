using System;
using System.Collections.Generic;
using Android.OS;

[assembly: Xamarin.Forms.Dependency(typeof(TryFirebaseXamarin.Droid.FirebaseLogEventService))]
namespace TryFirebaseXamarin.Droid
{
	public class FirebaseLogEventService : TryFirebaseXamarin.Services.IFirebaseLogEventService
	{
		public void LogEvent(string key, Dictionary<string, object> values)
		{
			Bundle bundle = new Bundle();
			foreach (var v in values)
			{
				bundle.PutString(v.Key, v.Value.ToString());
			}

			FirebaseInstanceManager.Instance.SendUserLogEvent(key, bundle);
		}
	}
}
