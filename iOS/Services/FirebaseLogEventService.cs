using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(TryFirebaseXamarin.iOS.FirebaseLogEventService))]
namespace TryFirebaseXamarin.iOS
{
	public class FirebaseLogEventService : TryFirebaseXamarin.Services.IFirebaseLogEventService
	{
		public void LogEvent(string key, Dictionary<string, object> values)
		{
			var nsdict = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(
				values.Values.Select(v => NSObject.FromObject(v)).ToArray(),
				values.Keys.Select(k => new NSString(k)).ToArray());

			Firebase.Analytics.Analytics.LogEvent(key, nsdict);
		}
	}
}
