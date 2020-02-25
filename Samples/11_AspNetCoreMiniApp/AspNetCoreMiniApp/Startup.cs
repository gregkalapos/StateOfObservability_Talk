using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreMiniApp
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGet("/", async context =>
				{
					Console.WriteLine($"activity id: {Activity.Current?.Id}," +
						$" parentId: {Activity.Current?.ParentId}");

					if (Activity.Current != null)
					{
						Console.WriteLine("Activity.Baggega items:");
						foreach (var item in Activity.Current.Baggage)
						{
							Console.WriteLine($"{item.Key}-{item.Value}");
						}
					}

					await context.Response.WriteAsync("Hello World!");
				});
			});
		}
	}
}
