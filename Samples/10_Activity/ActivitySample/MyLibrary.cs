using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ActivitySample
{
	class MyLibrary
	{
		private readonly static DiagnosticSource _diagnosticSource
			= new DiagnosticListener(typeof(MyLibrary).FullName);

		public async Task<double> CalculateAverageAsync(IEnumerable<double> items)
		{
			var activity = new Activity(nameof(CalculateAverageAsync));

			if (_diagnosticSource.IsEnabled(typeof(MyLibrary).FullName))
			{
				_diagnosticSource.StartActivity(activity, new { Items = items });
			}

			activity.AddTag("NumberOfItemsToAvg_InTag", items.Count().ToString());
			activity.AddBaggage("NumberOfItemsToAvg_InBaggage", items.Count().ToString());
			var avgValue = items.Average();

			try
			{
				var httpClient = new HttpClient();
				var res = await httpClient.GetAsync("http://localhost:5000/");
			}
			catch { }

			if (_diagnosticSource.IsEnabled(typeof(MyLibrary).FullName))
			{
				_diagnosticSource.StopActivity(activity, new { Items = items, Result = avgValue });
			}
			return avgValue;
		}
	}
}
