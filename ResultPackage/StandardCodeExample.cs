namespace ResultPackage;

public class StandardCodeExample
{
    private ILogger _logger;
    private IDatabase _database;
    private IPaymentGateway _paymentGateway;

    public string RefillBalance(int customerId, decimal moneyAmount)
    {
        if (IsMoneyAmountValid(moneyAmount))
        {
            _logger.Log("Money amount is invalid");
            return "Money amount is invalid";
        }
    
        Customer customer = _database.GetCustomerById(customerId);
    
        if (customer is null)
        {
            _logger.Log("Customer not found");
            return "Customer not found";
        }
    
        customer.Balance += moneyAmount;
        try
        {
            _paymentGateway.ChargePayment(customer.BillingInfo, moneyAmount);
        }
        catch (InvalidOperationException)
        {
            _logger.Log("Unable to charge the credit card");
            return "Unable to charge the credit card";
        }
    
        try
        {
            _database.Save(customer);
        }
        catch (TimeoutException)
        {
            _paymentGateway.RollbackLastTransaction();
            _logger.Log("Unable to connect to the database");
            return "Unable to connect to the database";
        }
    
        _logger.Log("OK");
        return "OK";
    }
    
    private bool IsMoneyAmountValid(decimal moneyAmount) => moneyAmount > 0;
}