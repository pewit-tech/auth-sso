using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySQL.Data.Entity.Extensions;
using Microsoft.Extensions.Configuration;
using System.Reflection;

/// <summary>
/// Persisted grant extended
/// </summary>
public class ExtendedPersistedGrantDbContext : PersistedGrantDbContext
{
    private OperationalStoreOptions storeOptions;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">options</param>
    /// <param name="storeOptions">operational store options</param>
    public ExtendedPersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options, OperationalStoreOptions storeOptions)
        : base(options, storeOptions)
    {
        this.storeOptions = storeOptions;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // perform original build
        System.Console.WriteLine("ExtendedPersistentGrantDbContext OnModelCreating");
        modelBuilder.Entity<IdentityServer4.EntityFramework.Entities.PersistedGrant>(grant =>
        {
            grant.Property(x => x.Data).HasMaxLength(8196).IsRequired();
        });
        base.OnModelCreating(modelBuilder);
    }

    public class ExtendedPersistedGrantDbContextFactory : IDbContextFactory<ExtendedPersistedGrantDbContext>
    {
        public ExtendedPersistedGrantDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new ConfigurationBuilder()
				.SetBasePath(options.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile("connectionstrings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{options.EnvironmentName}.json", optional: true);

			var config = builder.Build();

			var mySqlConnectionString = config.GetConnectionString("DefaultConnection");

			var optionsBuilder = new DbContextOptionsBuilder<PersistedGrantDbContext>();

			var migrationsAssembly = typeof(ExtendedPersistedGrantDbContext).GetTypeInfo().Assembly.GetName().Name;

			optionsBuilder.UseMySQL(mySqlConnectionString, opts => opts.MigrationsAssembly(migrationsAssembly));

			return new ExtendedPersistedGrantDbContext(optionsBuilder.Options,
				new OperationalStoreOptions());
        }
    }
}