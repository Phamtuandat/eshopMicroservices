
using Ordering.Domain.Abstractions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models
{
    public class Customer : Entity<CustomerId>
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        
        public static Customer Create(CustomerId id, string email, string name) 
        {
            var customer = new Customer() 
            {
                Id = id,
                Name = name,
                Email = email,
            };
            return customer;
        }
    }
}
