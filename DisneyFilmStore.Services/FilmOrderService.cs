using DisneyFilmStore.Data;
using DisneyFilmStore.Models.FilmOrderModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                OrderId = model.OrderId,
                FilmId = model.FilmId
            };

            using (var context = new ApplicationDbContext())
            {
                context.FilmOrders.Add(entity);
                return await context.SaveChangesAsync() == 1;
            }
        }

        // GET ALL / READ
        public async Task<IEnumerable<FilmOrderDetail>> GetAllFilmOrdersAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var query = context
                    .FilmOrders
                    .Where(fo => fo.UserId == _userId)
                    .Select(fo => new FilmOrderDetail
                    {
                        Id = fo.Id,
                        OrderId = fo.OrderId,
                        FilmId = fo.FilmId
                    }
                    );

                return await query.ToArrayAsync();
            }
        }

        // GET CUSTOMER BY ID / READ
        public FilmOrderDetail GetFilmOrderById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .FilmOrders
                    .Single(c => c.UserId == _userId && c.Id == id);

                return new FilmOrderDetail
                {
                    Id = entity.Id,
                    OrderId = entity.OrderId,
                    FilmId = entity.FilmId
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
                    .Single(fo => fo.UserId == _userId && fo.Id == id);

                entity.OrderId = model.OrderId;
                entity.FilmId = model.FilmId;

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
                    .Single(fo => fo.UserId == _userId && fo.Id == id);

                context.FilmOrders.Remove(entity);
                return await context.SaveChangesAsync() == 1;
            };
        }
    }
}
