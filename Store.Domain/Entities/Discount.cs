namespace Store.Domain.Entities;

public class Discount : Entity
{
	public Discount(decimal amount, DateTime expireDate)
	{
		Amount = amount;
		ExpireDate = expireDate;
	}

	public decimal Amount { get; private set; }
	public DateTime ExpireDate { get; private set; }

	#region Methods
	public bool IsValid() => DateTime.Compare(DateTime.Now, ExpireDate) < 0;

	public decimal Value() => IsValid() ? Amount : 0;

	#endregion
}
