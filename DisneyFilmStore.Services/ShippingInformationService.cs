using DisneyFilmStore.Data;
using DisneyFilmStore.Models.ShippingInformationModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Services
{
    public class ShippingInformationService
    {
        private readonly Guid _userId;

        public ShippingInformationService(Guid userId)
        { 
            _userId = userId;
        }

        // CREATE / POST
        public async Task<bool> CreateShippingInfoAsync(ShippingInfoCreate model)
        {
            var entity = new ShippingInformation
            {
                UserId = _userId,
                OrderId = model.OrderId,
                CustomerId = model.CustomerId
            };

            using (var context = new ApplicationDbContext())
            {
                context.Shipments.Add(entity);
                return await context.SaveChangesAsync() == 1;
            }
        }

        // GET ALL / READ
        public async Task<IEnumerable<ShippingInfoListItem>> GetAllShippingInfosAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var query = context
                    .Shipments
                    .Where(s => s.UserId == _userId)
                    .Select(s => new ShippingInfoListItem
                    {
                        Id = s.Id,
                        OrderId = s.OrderId,
                        CustomerId = s.CustomerId,                        
                    }
                    );

                return await query.ToArrayAsync();
            }
        }

        // GET CUSTOMER BY ID / READ
        public ShippingInfoDetail GetShippingInfoById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .Shipments
                    .Single(c => c.UserId == _userId && c.Id == id);

                return new ShippingInfoDetail
                {
                    Id = entity.Id,
                    OrderId = entity.OrderId,
                    CustomerId = entity.CustomerId,
                    OrderDate = entity.Order.OrderDate,
                    ShippingAddress = entity.Customer.Address
                };
            }
        }

        // PUT BY ID / UPDATE
        public async Task<bool> UpdateShippingInfoByIdAsync(int id, ShippingInfoEdit model)
        {
            var customerService = new CustomerService(_userId);
            var orderService = new OrderService(_userId);
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .Shipments
                    .Single(s => s.UserId == _userId && s.Id == id);

                entity.Customer.Address = model.ShippingAddress;
                entity.Order.OrderDate = DateTime.Now;

                return await context.SaveChangesAsync() == 2;
            }
        }

        // DELETE
        public async Task<bool> DeleteShippingInfoByIdAsync(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .Shipments
                    .Single(s => s.UserId == _userId && s.Id == id);

                context.Shipments.Remove(entity);
                return await context.SaveChangesAsync() == 1;
            };
        }

        // DELETE
        public async Task<int> DeleteShippingInfoByOrderIdAsync(int orderId)
        {
            using (var context = new ApplicationDbContext())
            {
                int changesCount = 0;
                var entity = context
                    .Shipments
                    .Single(s => s.UserId == _userId && s.OrderId == orderId);

                if (await DeleteShippingInfoByIdAsync(entity.Id))
                {
                    changesCount++;
                }
                return changesCount;
            };
        }

        // DELETE
        public async Task<int> DeleteShippingInfoByCustomerIdAsync(int customerId)
        {
            int changeCount = 0;
            List<ShippingInformation> shipmentsToDelete = new List<ShippingInformation>();
            using (var context = new ApplicationDbContext())
            {
                int changesCount = 0;
                var query = context
                    .Shipments
                    .Where(s => s.UserId == _userId && s.CustomerId == customerId);

                shipmentsToDelete = await query.ToListAsync();
                foreach (var s in shipmentsToDelete)
                {
                    await DeleteShippingInfoByIdAsync(s.Id);
                    changeCount++;
                }
                return changesCount;
            };
        }

    }
}
