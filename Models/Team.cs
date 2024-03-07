using System.ComponentModel.DataAnnotations;

namespace VirtualOffice.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Employee>? Employee { get; set; }
    }
}
