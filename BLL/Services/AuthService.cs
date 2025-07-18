using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthService
    {
        private readonly CustomerRepository _customerRepository;

        private readonly string _adminEmail;
        private readonly string _adminPassword;
        public AuthService(CustomerRepository customerRepository, string adminEmail, string adminPassword)
        {
            _customerRepository = customerRepository;
            _adminEmail = adminEmail;
            _adminPassword = adminPassword;

        }

        public bool IsAdmin(string email, string password)
        {
            return email == _adminEmail && password == _adminPassword;
        }

        public Customer AuthenticateCustomer(string email, string password)
        {
            return _customerRepository.GetByEmailAndPassword(email, password);
        }
    }
}
