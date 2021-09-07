using DisneyFilmStore.Data;
using DisneyFilmStore.Models.FilmOrderModels;
using DisneyFilmStore.Models.OrderModels;
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

        //// PUT BY ID / UPDATE
        //public async Task<bool> UpdateFilmOrderByIdAsync(int id, FilmOrderEdit model)
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var entity = context
        //            .FilmOrders
        //            .Single(fo => fo.UserId == _userId && fo.Id == id);

        //        entity.OrderId = model.OrderId;
        //        entity.FilmId = model.FilmId;

        //        return await context.SaveChangesAsync() == 1;
        //    }
        //}

        public async Task<int> UpdateFilmOrderFromOrderUpdateAsync(OrderEdit model)
        {
            int changesCount = 0;
            using (var context = new ApplicationDbContext())
            {
                var query =
                    context
                        .FilmOrders
                        .Where(fo => fo.OrderId == model.OrderId && fo.UserId == _userId);

                var currentFilmOrders = await query.ToArrayAsync();

                // old films: 1 2 3
                // updated films: 3 7 8 9

                List<int> currentFilmIds = new List<int>(); // list of current film Ids for order
                foreach (var filmOrder in currentFilmOrders)
                {
                    currentFilmIds.Add(filmOrder.FilmId);
                }

                foreach (var filmId in currentFilmIds) // deleting current films references no longer in the edited order
                {
                    if (!(model.FilmIds.Contains(filmId)))
                    {
                        await DeleteFilmOrderByIdAsync(filmId);
                        changesCount++;
                    }
                }
                foreach (var filmId in model.FilmIds) // adding films references that were not previously in the order
                {
                    if (!(currentFilmIds.Contains(filmId)))
                    {
                        await CreateFilmOrderAsync(
                            new FilmOrderCreate
                            {
                                FilmId = filmId,
                                OrderId = model.OrderId
                            });
                        changesCount++;
                    }
                }
            }
            return changesCount;
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

        // DELETE
        public async Task<int> DeleteFilmOrderByFilmId(int filmID)
        {
            int changeCount = 0;
            List<FilmOrder> filmOrdersToDelete = new List<FilmOrder>();
            using (var context = new ApplicationDbContext())
            {
                var query = context
                    .FilmOrders
                    .Where(fo => fo.UserId == _userId && fo.FilmId == filmID);

                filmOrdersToDelete = await query.ToListAsync();
            }
            foreach (var fo in filmOrdersToDelete)
            {
                await DeleteFilmOrderByIdAsync(fo.Id);
                changeCount++;
            }
            return changeCount;
        }
    }
}
