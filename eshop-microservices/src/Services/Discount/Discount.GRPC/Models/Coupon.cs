namespace Discount.GRPC.Models
{
    public class Coupon
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int DiscountPercentage { get; set; }
        public int Quantity { get; set; }

        public string Code { get; set; }
    }
}
