namespace ResultPackage;

public class RailwayCustomerService
{
    private readonly IDatabase _database;
    private readonly ILogger _logger;
    private readonly IPaymentGateway _paymentGateway;

    public RailwayCustomerService(IDatabase database, ILogger logger, IPaymentGateway paymentGateway)
    {
        _database = database;
        _logger = logger;
        _paymentGateway = paymentGateway;
    }

    public string RefillBalance(int customerId, decimal moneyAmount)
    {
        Result<MoneyToCharge> moneyToCharge = MoneyToCharge.Create(moneyAmount);
        Result<Customer> customer = _database.GetById(customerId).ToResult("Customer is not found");

        return Result.Combine(moneyToCharge, customer)
            .OnSuccess(() => customer.Value.AddBalance(moneyToCharge.Value))
            .OnSuccess(() => _paymentGateway.ChargePayment(customer.Value.BillingInfo, moneyToCharge.Value))
            .OnSuccess(
                () => _database.Save(customer.Value)
                    .OnFailure(() => _paymentGateway.RollbackLastTransaction()))
            .OnBoth(Log)
            .OnBoth(result => result.IsSuccess ? "OK" : result.Error);
    }

    private void Log(Result result)
    {
        _logger.Log(result.IsFailure ? result.Error : "OK");
    }
}