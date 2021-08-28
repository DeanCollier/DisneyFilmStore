using DisneyFilmStore.Data;
using DisneyFilmStore.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Services
{
    public class CustomerService
    {
        private readonly Guid _userId;

        public CustomerService(Guid userId)
        {
            _userId = userId;
        }

        // CREATE / POST
        public async Task<bool> CreateCustomerAsync(CustomerCreate model)
        {
            var entity = new Customer
            {
                UserId = _userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                Member = model.Member
            };

            using (var context = new ApplicationDbContext())
            {
                context.Customers.Add(entity);
                return await context.SaveChangesAsync() == 1;
            }
        }

        // GET ALL / READ
        public async Task<IEnumerable<CustomerListItem>> GetAllCustomersAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var query = context
                    .Customers
                    .Where(c => c.UserId == _userId)
                    .Select(c => new CustomerListItem
                        {
                            Id = c.Id,
                            FullName = $"{c.FirstName} {c.LastName}"
                        }
                    );

                return await query.ToArrayAsync();
            }
        }

        // GET CUSTOMER BY ID / READ
        public CustomerDetail GetCustomerById(int id)
        {
            var orderService = new OrderService(_userId);
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .Customers
                    .Single(c => c.UserId == _userId && c.Id == id);

                var orders = orderService.GetOrders(); // getting all orders with userId

                return new CustomerDetail
                {
                    Id = entity.Id,
                    FullName = $"{entity.FirstName} {entity.LastName}",
                    Email = entity.Email,
                    Address = entity.Address,
                    Member = entity.Member,
                    Orders = orders
                };
            }
        }

        // PUT BY ID / UPDATE
        public async Task<bool> UpdateCustomerByIdAsync(int id, CustomerEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .Customers
                    .Single(c => c.UserId == _userId && c.Id == id);

                if (model.FirstName != null)
                    entity.FirstName = model.FirstName;  

                else if (model.LastName != null)
                    entity.LastName = model.LastName;

                else if (model.Email != null)
                    entity.Email = model.Email;

                else if (model.Address != null)
                    entity.Address = model.Address;

                entity.Member = model.Member;
                // this is problematic because if they don't change member status,
                // the bool will be false and Member status will automatically be set to false

                return await context.SaveChangesAsync() == 1;
            }
        }

        // DELETE
        public async Task<bool> DeleteCustomerByIdAsync(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .Customers
                    .Single(c => c.UserId == _userId && c.Id == id);

                context.Customers.Remove(entity);
                return await context.SaveChangesAsync() == 1;
            };
        }
        
        //// DELETE ACCOUNT - REMOVE ALL CUSTOMERS WITH CURRENT USERID
        //public bool DeleteAllCustomersOnAccount()
        //{
        //    IEnumerable<Customer> accountCustomers = new IEnumerable<Customer>();
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var query = context
        //            .Customers
        //            .Where(c => c.UserId == _userId);

        //        context.Customers.Remove(entity);
        //        return context.SaveChanges() == 1;
        //    };
        //}
    }
}
