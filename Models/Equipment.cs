using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualOffice.Models
{
    public class Equipment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(Equipment))]
        public int CategoryId { get; set; }
        public virtual EquipmentCategory? EquipmentCategory { get; set; }

    }
}
