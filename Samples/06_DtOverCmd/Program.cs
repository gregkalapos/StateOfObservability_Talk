using Elastic.Apm;
using Elastic.Apm.Api;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace DistributedTracingOverCmdParameter
{
    class Program
    {
        static void Main(string[] args)
        {
			if (args.Length == 1) //in case it's started with an argument we try to parse the argument as a DistributedTracingData
			{
				Console.WriteLine($"Process started - continuing trace with distributed tracing data: {args[0]}");

				var distributedTracingData = DistributedTracingData.TryDeserializeFromString(args[0]);
				var transaction2 = Agent.Tracer.StartTransaction("Transaction2", "TestTransaction", distributedTracingData);

				try
				{
					transaction2.CaptureSpan("TestSpan", "TestSpanType", () => Thread.Sleep(200));
				}
				finally
				{
					transaction2.End();
				}

				Thread.Sleep(2000);

				Console.WriteLine("About to exit - press any key...");
				Console.ReadKey();
			}
			else
			{
				Console.WriteLine("Started");

				Console.WriteLine("Capturing a transaction");
				var transaction = Agent.Tracer.StartTransaction("Transaction1", "TestTransaction");

				try
				{
					var outgoingDistributedTracingData = Agent.Tracer.CurrentTransaction?.OutgoingDistributedTracingData?.SerializeToString();

					Console.WriteLine($"The value of the distributed tracing data: {outgoingDistributedTracingData}");
					Console.WriteLine($"Waiting 20 sec to continue the trace");

					Thread.Sleep(20000);
				}
				finally
				{
					transaction.End();
				}

				//WIP: if the process terminates the agent
				//potentially does not have time to send the transaction to the server.


				Console.WriteLine("About to exit - press any key...");
				Console.ReadKey();
			}
		}
	}
}
