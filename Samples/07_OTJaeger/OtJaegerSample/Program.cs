using System;

namespace OtJaegerSample
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using OpenTelemetry.Exporter.Jaeger;
	using OpenTelemetry.Trace;
	using OpenTelemetry.Trace.Configuration;

	class Program
	{
		static TracerFactory _tracerFactory;
		static void Main(string[] args)
		{
			// Configure the tracer
			using (_tracerFactory = TracerFactory.Create(
				
								// set up jaeger exporter
								builder => builder.UseJaeger(o =>
								{
									o.ServiceName = "jaeger-test";
									o.AgentHost = "localhost";
								})))
			{
				// Get the tracer
				var tracer = _tracerFactory.GetTracer("jaeger-test");

				// Start an active span. It will end automatically when using statement ends
				using (tracer.StartActiveSpan("Main", out var span))
				{
					Console.WriteLine("About to do a busy work");
					for (int i = 0; i < 10; i++)
					{
						DoWork(i);
					}
				}
			}

			// Gracefully shutdown the exporter so it'll flush queued traces to Jaeger.
			_tracerFactory.Dispose();

			Console.WriteLine("Press any key to exit!");
			Console.ReadKey();
		}

		private static void DoWork(int i)
		{
			// Get the tracer instance
			var tracer = _tracerFactory.GetTracer("jaeger-test");

			// Start another span. If another span was already started, it'll use that span as the parent span.
			// In this example, the main method already started a span, so that'll be the parent span, and this will be
			// a child span.
			var span = tracer.StartSpan("DoWork");

			try
			{
				// Simulate some work.
				Console.WriteLine("Doing busy work");
				Thread.Sleep(1000);
			}
			catch (ArgumentOutOfRangeException e)
			{
				// . Set status upon error
				span.Status = Status.Internal.WithDescription(e.ToString());
			}

			//  Annotate our span to capture metadata about our operation
			span.SetAttribute("SampleAttibute", 42);
			
			// End span
			span.End();
		}
	}
}
