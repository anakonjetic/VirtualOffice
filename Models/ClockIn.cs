using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualOffice.Models
{
    public class ClockIn
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly ClockInTime { get; set; }
        public TimeOnly ClockOutTime { get; set; }
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
