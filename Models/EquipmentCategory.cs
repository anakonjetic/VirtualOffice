using System.ComponentModel.DataAnnotations;

namespace VirtualOffice.Models
{
    public class EquipmentCategory
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Equipment>? Equipment { get; set; }
    }
}
