﻿using Flunt.Validations;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Order : Entity
{
	public Order(Customer customer, decimal deliveryFee, Discount discount)
	{
		AddNotifications(
			new Contract()
				.Requires()
				.IsNotNull(customer, "Customer", "Cliente Inválido")
				.IsGreaterThan(deliveryFee, 0, "DeliveryFee", "A Taxa de Entrega deve ser maior que zero")
				.IsNotNull(discount, "Discount", "Desconto Inválido")
		);

		Customer = customer;
		Date = DateTime.Now;
		Number = Guid.NewGuid().ToString().Substring(0, 8);
		Status = EOrderStatus.WaitingPayment;
		DeliveryFee = deliveryFee;
		Discount = discount;
		Items = new();
	}

	public Customer Customer { get; private set; }
	public DateTime Date { get; private set; }
	public string Number { get; private set; }
	public List<OrderItem> Items { get; private set; }
	public decimal DeliveryFee { get; private set; }
	public EOrderStatus Status { get; private set; }
	public Discount Discount { get; private set; }

	#region Methods
	public void AddItem(Product product, int quantity)
	{
		var item = new OrderItem(product, quantity);
		if (item.Valid)
			Items.Add(item);
	}

	public decimal Total()
	{
		decimal total = 0;
		foreach (var item in Items)
		{
			total += item.Total();
		}
		total += DeliveryFee;
		total -= Discount is not null ? Discount.Value() : 0;
		return total;
	}

	public void Pay(decimal amount)
	{
		if (amount == Total())
			Status = EOrderStatus.WaitingDelivery;
	}

    public void Cancel(decimal amount)
    {
        Status = EOrderStatus.Canceled;
    }
    #endregion
}
