using System;
using System.ComponentModel.DataAnnotations;

namespace APIWebManagement.Data.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double SalePrice { get; set; }
        [Required]
        public double OriginalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
