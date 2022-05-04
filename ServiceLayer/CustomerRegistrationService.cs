using EntityLayer;
using Repository;
using System.Collections.Generic;

namespace ServiceLayer
{
    public class CustomerRegistrationService
    {
        CustomerRegistrationRepo registrationRepo = null;
        public CustomerRegistrationService()
        {
            registrationRepo = new CustomerRegistrationRepo();
        }

        public string GetCustomerCode()
        {
            return registrationRepo.GetCustomerCode();
        }

        public int AddCustomer(CustomerRegistrationEntity entity, string IPAddress, string Browser, string BrowserVersion, string Password)
        {
            return registrationRepo.AddRegisterCustomer(entity, IPAddress, Browser, BrowserVersion, Password);
        }

        public CustomerRegistrationEntity GetCustRegisted(string Id)
        {
            return registrationRepo.GetCustRegisted(Id);
        }

        public List<CustomerInfoEntity> GetCustomers(string CustomerId)
        {
            return registrationRepo.GetCustomers(CustomerId);
        }

        public int GetEmailDuplicate(string Email)
        {
            return registrationRepo.GetEmailDuplicate(Email);
        }
    }
}
