using System.ComponentModel.DataAnnotations;

namespace VirtualOffice.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Employee>? Employee { get; set; }
    }
}
