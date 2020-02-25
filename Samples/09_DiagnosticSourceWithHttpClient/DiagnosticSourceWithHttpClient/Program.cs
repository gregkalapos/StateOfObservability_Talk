using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace DiagnosticSourceWithHttpClient
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Subscribe();
			var httpClient = new HttpClient();
			await httpClient.GetAsync("https://kalapos.net");
		}

		private static void Subscribe()
		{
			DiagnosticListener.AllListeners.Subscribe(new Subscriber());
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
						Console.WriteLine($"HTTP Request start: {requestMessage.Method} -" +
							$" {requestMessage.RequestUri}");
					}

					break;
				case "System.Net.Http.HttpRequestOut.Stop":
					_stopwatch.Stop();

					if (receivedEvent.Value.GetType().GetTypeInfo().GetDeclaredProperty("Response")
						?.GetValue(receivedEvent.Value) is HttpResponseMessage responseMessage)
					{
						Console.WriteLine($"HTTP Request finished: took " +
							$"{_stopwatch.ElapsedMilliseconds}ms, status code:" +
								$" {responseMessage.StatusCode}");
					}

					break;
			}
		}
	}

	class Subscriber : IObserver<DiagnosticListener>
	{
		public void OnCompleted() { }

		public void OnError(Exception error) { }

		public void OnNext(DiagnosticListener listener)
		{
			if (listener.Name == "HttpHandlerDiagnosticListener")
			{
				listener.Subscribe(new HttpClientObserver());
			}
		}
	}
}
