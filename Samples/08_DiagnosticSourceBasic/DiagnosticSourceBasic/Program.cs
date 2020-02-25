using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace DiagnosticSourceBasic
{
	class Program
	{
		static void Main(string[] args)
		{
			Subscribe();
			new MyLibrary().CalculateAverage(new List<double> { 1.2, 2.4, 2.34 });
			Console.WriteLine("Hello World!");
		}

		private static void Subscribe()
		{
			DiagnosticListener.AllListeners.Subscribe(new Subscriber());
		}
	}

	class Subscriber : IObserver<DiagnosticListener>
	{
		public void OnCompleted() { }

		public void OnError(Exception error) { }

		public void OnNext(DiagnosticListener listener)
		{
			if (listener.Name == typeof(MyLibrary).FullName)
			{
				listener.Subscribe(new MyLibraryObserver());
			}
		}
	}

	class MyLibraryObserver : IObserver<KeyValuePair<string, object>>
	{
		public void OnCompleted() { }

		public void OnError(Exception error) { }

		public void OnNext(KeyValuePair<string, object> kv)
		{
			switch (kv.Key)
			{
				case "CalculateAverageStarted":
					if (kv.Value.GetType().GetTypeInfo().GetDeclaredProperty("Items").GetValue(kv.Value)
						is IEnumerable<double> items)
					{
						Console.WriteLine("CalculateAverageFinished - items:");
						foreach (var item in items)
						{
							Console.WriteLine(item);
						}
					}
					break;

				case "CalculateAverageFinished":
					if (kv.Value.GetType().GetTypeInfo().GetDeclaredProperty("Result").GetValue(kv.Value)
						is double result)
					{
						Console.WriteLine($"CalculateAverageFinished - result: {result}");
					}
					break;
				default:
					break;
			}
		}
	}
}
