using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Currency.Frontend
{
	public class Program
	{
		public static void Main(string[] args)
		{

			Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
			.Enrich.WithElasticApmCorrelationInfo()
			.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
			{
				CustomFormatter = new EcsTextFormatter(),
				MinimumLogEventLevel = Serilog.Events.LogEventLevel.Verbose
			})
			.CreateLogger();

			try
			{
				Log.Information("Starting web host");
				CreateHostBuilder(args).Build().Run(); 
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Host terminated unexpectedly");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.UseSerilog();
	}
}
