namespace Basket.API.Models;

public class ShoppingCart
{
    public string CustomerId { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);


    public ShoppingCart(string customerId)
    {
        CustomerId = customerId;
    }

    //Required for Mapping
    public ShoppingCart()
    {
    }
}