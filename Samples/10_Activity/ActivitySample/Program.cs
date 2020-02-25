using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ActivitySample
{

	class Program
	{
		static async Task Main(string[] args)
		{
			Subscribe();

			await new MyLibrary().CalculateAverageAsync(new List<double> { 1.2, 2.3 });
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
			if (listener.Name == "HttpHandlerDiagnosticListener")
			{
				listener.Subscribe(new HttpClientObserver());
			}
		}
	}

	class HttpClientObserver : IObserver<KeyValuePair<string, object>>
	{
		Stopwatch _stopwatch = new Stopwatch();

		public void OnCompleted() { }

		public void OnError(Exception error) { }

		public void OnNext(KeyValuePair<string, object> receivedEvent)
		{
			switch (receivedEvent.Key)
			{
				case "System.Net.Http.HttpRequestOut.Start":
					_stopwatch.Start();

					if (receivedEvent.Value.GetType().GetTypeInfo().GetDeclaredProperty("Request")
						?.GetValue(receivedEvent.Value) is HttpRequestMessage requestMessage)
					{
						Console.WriteLine($"System.Net.Http.HttpRequestOut.Start: {requestMessage.Method} -" +
							$" {requestMessage.RequestUri} - activity id: {Activity.Current?.Id}, parentId: {Activity.Current?.ParentId}");
					}

					break;
				case "System.Net.Http.HttpRequestOut.Stop":
					_stopwatch.Stop();

					if (receivedEvent.Value.GetType().GetTypeInfo().GetDeclaredProperty("Response")
						?.GetValue(receivedEvent.Value) is HttpResponseMessage responseMessage)
					{
						Console.WriteLine($"System.Net.Http.HttpRequestOut.Stop: took " +
							$"{_stopwatch.ElapsedMilliseconds}ms, status code:" +
								$" {responseMessage.StatusCode} - activity id: {Activity.Current?.Id}, parentId: {Activity.Current?.ParentId}");
					}

					break;
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
				case "CalculateAverageAsync.Start":
					Console.WriteLine($"CalculateAverageAsync.Start - activity id: {Activity.Current?.Id}");
					break;
				case "CalculateAverageAsync.Stop":
					Console.WriteLine($"CalculateAverageAsync.Stop - activity id: {Activity.Current?.Id}");

					if (Activity.Current != null)
					{
						Console.WriteLine("Tags:");
						foreach (var tag in Activity.Current.Tags)
						{
							Console.WriteLine($"{tag.Key} - {tag.Value}");
						}
					}
					break;
				default:
					break;
			}
		}
	}
}
