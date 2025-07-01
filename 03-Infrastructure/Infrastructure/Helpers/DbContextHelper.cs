using System;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbContextHelper
    {
        public static void UpdateAuditableEntityProperties<TDbContext>(TDbContext dbContext, long userId, string ipAddress)
           where TDbContext : DbContext
        {
            foreach (var entry in dbContext.ChangeTracker.Entries<AuditableEntity>())
            {
                var entity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entity.RefId == Guid.Empty)
                            entity.RefId = Guid.NewGuid();

                        entity.CreatedBy = userId;
                        entity.CreatedOn = DateTime.UtcNow;
                        entity.CreatedIpAddress = ipAddress ?? "127.0.0.1";
                        break;

                    case EntityState.Modified:
                        entry.Property(x => x.CreatedBy).IsModified = false;
                        entry.Property(x => x.CreatedOn).IsModified = false;
                        entry.Property(x => x.CreatedIpAddress).IsModified = false;
                        entity.LastModifiedBy = userId;
                        entity.LastModifiedOn = DateTime.UtcNow;
                        entity.LastModifiedIpAddress = ipAddress;
                        entry.Property(x => x.IsDeleted).IsModified = false;
                        entry.Property(x => x.DeletedBy).IsModified = false;
                        entry.Property(x => x.DeletedOn).IsModified = false;
                        entry.Property(x => x.DeletedIpAddress).IsModified = false;
                        break;

                    case EntityState.Deleted:
                        // https://www.ryansouthgate.com/2019/01/07/entity-framework-core-soft-delete/
                        // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                        entry.State = EntityState.Unchanged;

                        // Only update the IsDeleted flag - only this will get sent to the Db
                        entry.Property(x => x.CreatedBy).IsModified = false;
                        entry.Property(x => x.CreatedOn).IsModified = false;
                        entry.Property(x => x.CreatedIpAddress).IsModified = false;
                        entry.Property(x => x.LastModifiedBy).IsModified = false;
                        entry.Property(x => x.LastModifiedOn).IsModified = false;
                        entry.Property(x => x.LastModifiedIpAddress).IsModified = false;
                        entity.IsActive = false;
                        entity.IsDeleted = true;
                        entity.DeletedBy = userId;
                        entity.DeletedOn = DateTime.UtcNow;
                        entity.DeletedIpAddress = ipAddress;
                        break;
                }
            }
        }
    }
}
