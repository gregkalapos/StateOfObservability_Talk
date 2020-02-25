using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DiagnosticSourceBasic
{
	class MyLibrary
	{
		private readonly static DiagnosticSource _diagnosticSource
			= new DiagnosticListener(typeof(MyLibrary).FullName);

		public double CalculateAverage(IEnumerable<double> items)
		{
			if (_diagnosticSource.IsEnabled(typeof(MyLibrary).FullName))
			{
				_diagnosticSource.Write("CalculateAverageStarted", new { Items = items });
			}

			var avgValue = items.Average();

			if (_diagnosticSource.IsEnabled(typeof(MyLibrary).FullName))
			{
				_diagnosticSource.Write("CalculateAverageFinished", new { Items = items, Result = avgValue });
			}
			return avgValue;
		}
	}
}
