using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currency.DbService.Entities
{
	public class SampleDbContext : DbContext
	{
		public DbSet<Sample> Sample { get; set; }

		public SampleDbContext(DbContextOptions<SampleDbContext> contextOptions) : base(contextOptions) { }
	}
}