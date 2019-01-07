using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebAPI.DAL
{
	public class MainDbContext : DbContext
	{
		public DbSet<Flight> Flights { get; set; }

		public MainDbContext(DbContextOptions options) : base(options)
		{
		}

		//Fluent API
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Flight>()
				.Property(f => f.AircraftType)
				.IsRequired();

			modelBuilder.Entity<Flight>()
				.Property(f => f.DepartureTime)
				.IsRequired()
				.HasColumnType("smalldatetime")
				.ValueGeneratedNever();

			modelBuilder.Entity<Flight>()
				.Property(f => f.ArrivalTime)
				.IsRequired()
				.HasColumnType("smalldatetime")
				.ValueGeneratedNever();

			modelBuilder.Entity<Flight>()
				.Property(f => f.FromLocation)
				.IsRequired();

			modelBuilder.Entity<Flight>()
				.Property(f => f.ToLocation)
				.IsRequired();
		}
	}
}
