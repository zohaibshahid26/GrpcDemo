using System.ComponentModel.DataAnnotations;
namespace gRPC_for_ASPNET_CORE.Model
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
