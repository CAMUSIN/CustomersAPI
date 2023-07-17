using CustomersAPI.Business.Abstractions;
using CustomersAPI.Data.Abstractions;
using CustomersAPI.Data.Models;

namespace CustomersAPI.Business
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly ICustomerData _customerData;

        public CustomerBusiness(ICustomerData customerData)
        {
            _customerData = customerData;
        }
        public ResultModel AddCustomer(CustomerModel customer)
        {
            var result = _customerData.AddCustomer(customer);
            return result;
        }

        public ResultModel DeleteCustomerById(int Id)
        {
            var result = _customerData.DeleteCustomerById(Id);
            return result;
        }

        public CustomerModel GetCustomerById(int Id)
        {
            var customer = _customerData.GetCustomerById(Id);
            return customer;
        }

        public List<CustomerModel> GetCustomers()
        {
            var customers = _customerData.GetCustomers();
            return customers;
        }

        public ResultModel UpdateCustomerById(int Id, CustomerModel customer)
        {
            var result = _customerData.UpdateCustomerById(Id, customer);
            return result;
        }
    }
}
