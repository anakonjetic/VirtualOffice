using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualOffice.Models
{
    public class EvaluationForm
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        [ForeignKey(nameof(Manager))]
        public int ManagerId { get; set; }
        public virtual Employee? Manager { get; set; }

        public string FormTitle { get; set; }
        public string FormDescription { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(nameof(EvaluationType))]
        public int EvaluationTypeId { get; set; }
        public virtual EvaluationType? EvaluationType { get; set; }
    }

    public class EvaluationType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<EvaluationForm>? EvaluationForm { get; set; }
    }
}
