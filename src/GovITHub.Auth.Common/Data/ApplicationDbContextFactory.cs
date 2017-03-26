using System.Reflection;
using GovITHub.Auth.Common.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySQL.Data.Entity.Extensions;
using Localization.SqlLocalizer.DbStringLocalizer;
using System;
using Microsoft.Extensions.Configuration;

namespace GovITHub.Auth.Common.Data
{
	public abstract class GenericDbContextFactory<TContext> : IDbContextFactory<TContext>
		where TContext : DbContext
    {
		public TContext Create(DbContextFactoryOptions options)
        {
			var builder = new ConfigurationBuilder()
				.SetBasePath(options.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile("connectionstrings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{options.EnvironmentName}.json", optional: true);

			var config = builder.Build();

			var mySqlConnectionString = config.GetConnectionString("DefaultConnection");

			var optionsBuilder = new DbContextOptionsBuilder<TContext>();

			var migrationsAssembly = typeof(TContext).GetTypeInfo().Assembly.GetName().Name;

			optionsBuilder.UseMySQL(mySqlConnectionString, opts => opts.MigrationsAssembly(migrationsAssembly));

			return CreateInstance(optionsBuilder.Options);
        }

		protected abstract TContext CreateInstance(DbContextOptions<TContext> options);
    }

	public class ApplicationDbContextFactory : GenericDbContextFactory<ApplicationDbContext>
	{
		protected override ApplicationDbContext CreateInstance(DbContextOptions<ApplicationDbContext> options)
		{
			return new ApplicationDbContext(options);
		}
	}

	public class ConfigurationDbContextFactory : GenericDbContextFactory<ConfigurationDbContext>
    {
        protected override ConfigurationDbContext CreateInstance(DbContextOptions<ConfigurationDbContext> options)
		{
			return new ConfigurationDbContext(options, new ConfigurationStoreOptions());
		}
    }
	public class LocalizationDbContextFactory : GenericDbContextFactory<LocalizationModelContext>
    {
        protected override LocalizationModelContext CreateInstance(DbContextOptions<LocalizationModelContext> options)
		{
			return new LocalizationModelContext(options);
		}
    }
}
