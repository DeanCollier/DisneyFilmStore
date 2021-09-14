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

        public async Task<int> UpdateFilmOrderFromOrderUpdateAsync(int orderId, OrderEdit model)
        {
            int changesCount = 0;
            using (var context = new ApplicationDbContext())
            {
                var query =
                    context
                        .FilmOrders
                        .DefaultIfEmpty()
                        .Where(fo => fo.OrderId == model.OrderId && fo.UserId == _userId);




                //    var currentfilmorders = await query.ToArrayAsync();

                //    //old films: 1 2 3
                //    //updated films: 3 7 8 9

                //    List<int> currentfilmids = new List<int>(); // list of current film ids for order
                //    foreach (var filmorder in currentfilmorders)
                //    {
                //        currentfilmids.Add(filmorder.FilmId);
                //    }

                //    foreach (var filmid in currentfilmids) // deleting current films references no longer in the edited order
                //    {
                //        if (!(model.FilmIds.Contains(filmid)))
                //        {
                //            changesCount++;
                //            await DeleteFilmOrderByIdAsync(filmid);
                //        }
                //    }
                //    foreach (var filmid in model.FilmIds) // adding films references that were not previously in the order
                //    {
                //        if (!(currentfilmids.Contains(filmid)))
                //        {
                //            await CreateFilmOrderAsync(
                //                new FilmOrderCreate
                //                {
                //                    FilmId = filmid,
                //                    OrderId = model.OrderId
                //                });
                //            changesCount++;
                //        }
                //    }
            }
            return changesCount;
        }

        // DELETES MOVIE INSIDE OF ORDER
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
