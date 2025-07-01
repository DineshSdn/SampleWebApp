using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public abstract class AuditableEntity
    {
        public Guid RefId { get; set; } = Guid.NewGuid();

        public long CreatedBy { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string CreatedIpAddress { get; set; }

        public long? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

        [MaxLength(20)]
        public string LastModifiedIpAddress { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public long? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

        [MaxLength(20)]
        public string DeletedIpAddress { get; set; }
    }
}
