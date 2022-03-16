using System.ComponentModel.DataAnnotations;

namespace RbsAPI.Models
{
    public class Orders
    {
        [Key]
        public int OrderUniqueId { get; set; }
        [Required(ErrorMessage = "Order Id is Required")]
        public string OrderId { get; set; }
        [Required(ErrorMessage = "ItemName is Required")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "CustomerName is Required")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Quantity is Required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "UnitPrice is Required")]
        public int UnitPrice { get; set; }
        [Required(ErrorMessage = "TotalPrice is Required")]
        public int TotalPrice { get; set; }
        [Required(ErrorMessage = "Created By is Required")]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsOrderApproved { get; set; }
    }
}
