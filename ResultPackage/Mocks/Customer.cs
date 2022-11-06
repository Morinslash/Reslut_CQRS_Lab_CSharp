namespace ResultPackage;

public class Customer
{
    public int BillingInfo { get; set; }
    public decimal Balance { get; set; }

    public Result AddBalance(MoneyToCharge value)
    {
        throw new NotImplementedException();
    }

}