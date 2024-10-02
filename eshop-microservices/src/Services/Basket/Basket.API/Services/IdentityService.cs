
namespace Basket.API.Services
{
    public class IdentityService(IHttpContextAccessor context) : IIdentityService
    {
        private IHttpContextAccessor _context = context;

        public Customer GetUserIdentity()
        {
            var customer = new Customer() {
                CustomerId = _context.HttpContext?.User.FindFirst("userId")?.Value ?? string.Empty,
                UserName = _context.HttpContext?.User.FindFirst("preferred_username")?.Value ?? string.Empty,
            };
            return customer;
        }
    }
}
