using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Employee : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(250)]
        public string FirstName { get; set; }

        [MaxLength(250)]
        public string LastName { get; set; }

        [MaxLength(250)]
        public string Email { get; set; }
        public DateTime HireDate { get; set; }

        [MaxLength(250)]
        public string JobTitle { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
