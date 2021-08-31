using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisneyFilmStore.Services
{
    public class FilmOrderService
    {
        // THIS IS NOT DONE, JUST COPIED FROM OTHER SERVICE, NOT CORRECT OR COMPLETE AT ALL
        private readonly Guid _userId;

        public FilmOrderService(Guid userId)
        {
            _userId = userId;
        }

        // CREATE / POST
        public async Task<bool> CreateFilmOrderAsync(FilmOrderCreate model)
        {
            var entity = new FilmOrder
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
                context.FilmOrders.Add(entity);
                return await context.SaveChangesAsync() == 1;
            }
        }

        // GET ALL / READ
        public async Task<IEnumerable<FilmOrderListItem>> GetAllFilmOrdersAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var query = context
                    .FilmOrders
                    .Where(c => c.UserId == _userId)
                    .Select(c => new FilmOrderListItem
                    {
                        Id = c.Id,
                        FullName = $"{c.FirstName} {c.LastName}"
                    }
                    );

                return await query.ToArrayAsync();
            }
        }

        // GET CUSTOMER BY ID / READ
        public FilmOrderDetail GetFilmOrderById(int id)
        {
            var orderService = new OrderService(_userId);
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .FilmOrders
                    .Single(c => c.UserId == _userId && c.Id == id);

                var orders = orderService.GetOrders(); // getting all orders with userId

                return new FilmOrderDetail
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
        public async Task<bool> UpdateFilmOrderByIdAsync(int id, FilmOrderEdit model)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .FilmOrders
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
        public async Task<bool> DeleteFilmOrderByIdAsync(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .FilmOrders
                    .Single(c => c.UserId == _userId && c.Id == id);

                context.FilmOrders.Remove(entity);
                return await context.SaveChangesAsync() == 1;
            };
        }
    }
}
