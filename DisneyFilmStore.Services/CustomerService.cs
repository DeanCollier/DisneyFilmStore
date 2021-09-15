using DisneyFilmStore.Data;
using DisneyFilmStore.Models.CustomerModels;
using DisneyFilmStore.Models.OrderModels;
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
                            FullName = c.FirstName + " " + c.LastName   
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

                var query = context
                    .Orders
                    .Where(o => o.CustomerId == entity.Id) // getting all orders with userId and customerId
                    .Select(o => new OrderListItem
                    {
                        OrderId = o.OrderId,
                        TotalOrderCost = o.TotalOrderCost,
                        CreatedUtc = o.OrderDate
                    });

                var orders = query.ToArray();

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

                if (model.FirstName != null) // if included in model, true
                    entity.FirstName = model.FirstName;  

                if (model.LastName != null)
                    entity.LastName = model.LastName;

                if (model.Email != null)
                    entity.Email = model.Email;

                if (model.Address != null)
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
            var shippingService = new ShippingInformationService(_userId);
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .Customers
                    .Single(c => c.UserId == _userId && c.Id == id);

                int sChanges = await shippingService.DeleteShippingInfoByCustomerIdAsync(entity.Id);

                context.Customers.Remove(entity);
                return await context.SaveChangesAsync() == (sChanges + 1);
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
