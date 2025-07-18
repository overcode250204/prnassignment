using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerRepository
    {
        private readonly FuminiHotelManagementContext _context;
        public CustomerRepository(FuminiHotelManagementContext context) 
        {
            _context = context;
        }
        public Customer GetByEmailAndPassword(string email, string password)
        {
            return _context.Customers.FirstOrDefault(cus => cus.EmailAddress == email && cus.Password == password);
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(cus => cus.CustomerId == id);
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void Remove(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        public List<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }
    }
}
