using System.ComponentModel.DataAnnotations;
namespace GrpcDemo.Model
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Qty { get; set; }

    }
}
