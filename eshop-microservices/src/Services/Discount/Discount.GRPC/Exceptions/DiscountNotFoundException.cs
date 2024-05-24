using BuildingBlocks.Exceptions;

namespace Discount.GRPC.Exceptions
{
    public class DiscountNotFoundException : NotFoundException
    {
        public DiscountNotFoundException(string id) : base($"Can't not find the ID: {id}")
        {
        }
    }
}
