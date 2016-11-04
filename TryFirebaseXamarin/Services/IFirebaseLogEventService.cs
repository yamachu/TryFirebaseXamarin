using System;
using System.Collections.Generic;
namespace TryFirebaseXamarin.Services
{
	public interface IFirebaseLogEventService
	{
		void LogEvent(string key, Dictionary<string, object> values);
	}
}
