namespace Discount.GRPC.Models
{
    public class Coupon
    {
        public Guid Id { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int DiscountPercentage { get; set; } = default!;
        public int Quantity { get; set; } = default!;

        public string Code { get; set; } = default!;
    }
}
