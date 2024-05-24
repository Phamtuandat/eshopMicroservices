
namespace Basket.API.Services
{
    public class IdentityService(IHttpContextAccessor context) : IIdentityService
    {
        private IHttpContextAccessor _context = context ?? throw new ArgumentNullException(nameof(context));

        public Customer? GetUserIdentity()
        {
            var customer = new Customer() {

                CustomerId = _context.HttpContext.User.FindFirst("userId")?.Value,
                UserName = _context.HttpContext.User.FindFirst("preferred_username")?.Value

            };
            return customer;
        }
    }
}
