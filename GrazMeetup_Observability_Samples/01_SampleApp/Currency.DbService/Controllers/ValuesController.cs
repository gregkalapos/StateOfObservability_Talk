using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Currency.DbService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Currency.DbService.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private SampleDbContext _context;
		private Random _random = new Random();
		ILogger<ValuesController> _logger;

		public ValuesController(ILogger<ValuesController> logger, SampleDbContext dbContext)
		{
			_context = dbContext;
			_logger = logger;

			_context.Database.EnsureCreated();

			if (_context.Sample.Count() == 0)
			{
				_context.Sample.Add(new Sample { Value = 10000000 });
			}

			_context.SaveChanges();
		}

		// GET api/values
		[HttpGet]
		public async Task<ActionResult<long>> Get()
		{
			var rnd = _random.Next(5);
			_logger.LogInformation("Generated random number: {randomnumber}", rnd);

			if (rnd == 0)
			{
				//randomly do work to generate high CPU usage
				Prime(100000);
			}

			if (rnd == 1)
			{
				//randomly throw exceptions to generate errors
				throw new Exception("Whuuuuuu");
			}

			var val = (await _context.Sample.FirstAsync()).Value;
			return val;
		}

		private List<long> Prime(long num)
		{
			var retVal = new List<long>();

			for (long i = 0; i <= num; i++)
			{
				bool isPrime = true;
				for (long j = 2; j < i; j++)
				{
					if (i % j == 0)
					{
						isPrime = false;
						break;
					}
				}
				if (isPrime)
				{
					retVal.Add(i);
				}
			}

			return retVal;
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return "value";
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
