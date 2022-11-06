namespace ResultPackage;

public interface IPaymentGateway
{
    void RollbackLastTransaction();
    Result ChargePayment(int billingInfo, MoneyToCharge value);
    void ChargePayment(int billingInfo, decimal value);
}