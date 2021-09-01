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
        public async Task<IEnumerable<FilmOrderTitle>> GetAllFilmOrdersAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var query = context
                    .FilmOrders
                    .Where(fo => fo.UserId == _userId)
                    .Select(fo => new FilmOrderTitle
                    {
                        FilmTitle = fo.Film.Title
                    }
                    );

                return await query.ToArrayAsync();
            }
        }

        // GET CUSTOMER BY ID / READ
        public FilmOrderTitle GetFilmOrderById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .FilmOrders
                    .Single(c => c.UserId == _userId && c.Id == id);

                return new FilmOrderTitle
                {
                    FilmTitle = entity.Film.Title
                };
            }
        }

        public FilmOrderDetail GetFilmOrderByFilmAndOrderIds(int filmId, int orderId)
        {
            using (var context = new ApplicationDbContext())
            {
                var entity = context
                    .FilmOrders
                    .Single(fo => fo.UserId == _userId && fo.FilmId == filmId && fo.OrderId == orderId);

                return new FilmOrderDetail
                {
                    FilmOrderId = entity.Id,
                    FilmId = entity.FilmId,
                    OrderId = entity.OrderId

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
