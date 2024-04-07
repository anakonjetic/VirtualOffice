using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualOffice.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RemainingDaysOff { get; set; }
        public int SickLeaveDaysUsed { get; set; }
        public string EquipmentId { get; set; }

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public string UserId { get; set; }
        public virtual ICollection<Request>? Request { get; set; }
        public virtual ICollection<ClockIn>? ClockIn { get; set; }

        public virtual ICollection<EmployeeManager>? ManagedEmployees { get; set; }
        public virtual ICollection<EmployeeManager>? ManagingEmployees { get; set; }
    }

    public class EmployeeManager
    {
        public int ManagerId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee? Manager { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
