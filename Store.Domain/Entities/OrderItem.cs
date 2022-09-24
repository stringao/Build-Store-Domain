using Flunt.Validations;

namespace Store.Domain.Entities;

public class OrderItem : Entity
{
	public OrderItem(Product product, int quantity)
	{
		AddNotifications(
			new Contract()
				.Requires()
                .IsNotNull(product, "Product", "Produto Inválido")
				.IsGreaterThan(quantity, 0, "DeliveryFee", "A quantidade deve ser maior que zero")
        );

		Product = product;
		Price = product is not null ? product.Price : 0;
		Quantity = quantity;
	}

	public Product Product { get; private set; }
	public decimal Price { get; private set; }
	public int Quantity { get; private set; }

	#region Methods

	public decimal Total()
	{
		return Price * Quantity;
	}

	#endregion
}
