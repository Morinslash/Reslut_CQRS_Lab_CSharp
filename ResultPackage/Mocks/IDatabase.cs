namespace ResultPackage;

public interface IDatabase
{
    Maybe<Customer> GetById(int customerId);
    Customer GetCustomerById(int customerId);
    Result Save(Customer customerValue);
}