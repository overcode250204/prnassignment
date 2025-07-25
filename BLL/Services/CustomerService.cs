using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepository;
        public CustomerService(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public bool CheckCustomerExist(int id)
        {
            _customerRepository.GetCustomerById(id);
            return _customerRepository.GetCustomerById(id) != null;
        }

        public List<Customer> GetAll()
        {
            var res = _customerRepository.GetAll();
            return res.ToList();
        }
        public void Remove(Customer c)
        {
            _customerRepository.Remove(c);
        }

        public void Add(Customer customer)
        {
            _customerRepository.Add(customer);
        }

        public void Update(Customer customer)
        {
            _customerRepository.Update(customer);
        }

        public List<Customer> Search(string keyword)
        {
            var all = _customerRepository.GetAll();
            if (string.IsNullOrWhiteSpace(keyword)) return all.ToList();
            keyword = keyword.ToLower();
            return all.Where(c => (c.CustomerFullName != null && c.CustomerFullName.ToLower().Contains(keyword))
                               || (c.EmailAddress != null && c.EmailAddress.ToLower().Contains(keyword))
                               || (c.Telephone != null && c.Telephone.ToLower().Contains(keyword)))
                      .ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return _customerRepository.GetCustomerById(id);
        }
    }
}
