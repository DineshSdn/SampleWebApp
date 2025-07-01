using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using App.Common.Abstractions.DbDrivers;
using App.Common.Abstractions.Utilities;
using App.Common.Adapters.Data;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Adapters.Data;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.DbDrivers
{
    public sealed class AppDbContext : DbContext, IAppDbContext
    {
        private readonly ILoggedinUserService _loggedinUserService;

        public AppDbContext(DbContextOptions dbContextOptions, ILoggedinUserService loggedinUserService)
            : base(dbContextOptions)
        {
            _loggedinUserService = loggedinUserService;
        }

        public new IDbSetAdapter<TEntity> Set<TEntity>()
            where TEntity : class
            => new DbSetAdapter<TEntity>(base.Set<TEntity>());

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            UpdateDeleteBehavior(builder, DeleteBehavior.NoAction);
            base.OnModelCreating(builder);
        }

        public IDbContextTransactionAdapter BeginTransaction()
            => new DbContextTransactionAdapter(Database.BeginTransaction());

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DbContextHelper.UpdateAuditableEntityProperties(this, _loggedinUserService.UserId, _loggedinUserService.IpAddress);
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        private static void UpdateDeleteBehavior(ModelBuilder builder, DeleteBehavior deleteBehavior)
        {
            var cascadeForeignKeys = builder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetForeignKeys())
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var foreignKey in cascadeForeignKeys)
                foreignKey.DeleteBehavior = deleteBehavior;

            SetIndises<Employee>(builder);
        }

        public static void SetIndises<TModel>(ModelBuilder builder)
            where TModel : AuditableEntity
        {
            builder.Entity<TModel>()
               .HasIndex(b => b.RefId)
               .IsUnique();

            builder.Entity<TModel>()
                .HasIndex(b => b.IsActive);

            builder.Entity<TModel>()
                .HasIndex(b => b.IsDeleted);
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
