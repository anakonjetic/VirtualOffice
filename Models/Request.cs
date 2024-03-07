using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualOffice.Models
{
    public class Request
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ManagerId { get; set; }
        public virtual Employee? Manager { get; set; }

        [ForeignKey(nameof(Status))]
        public int StatusId { get; set; }

        public virtual Status? Status { get; set; }

        [ForeignKey(nameof(RequestType))]
        public int RequestTypeID { get; set; }

        public virtual RequestType? RequestType { get; set; }

    }

    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Request>? Request{ get; set; }
    }

    public class RequestType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Request>? Request { get; set; }
    }

 }
