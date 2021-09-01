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
    public class OrderService
    {
        private readonly Guid _userId;

        public OrderService(Guid userId)
        {
            _userId = userId;
        }

        public async Task<bool> CreateOrderAsync(OrderCreate model)
        {
            var entity =
                new Order()
                {
                    OrderDate = model.OrderDate,
                    TotalOrderCost = model.TotalOrderCost, // write some calc for this based on films, prices, and member status
                    CustomerId = model.CustomerId,
                };

            var filmOrderService = new FilmOrderService(_userId);
            foreach (var film in model.FilmIds)
            {
                var filmOrderCreate = new FilmOrderCreate { FilmId = film, OrderId = entity.OrderId };
                await filmOrderService.CreateFilmOrderAsync(filmOrderCreate);
            }

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Orders.Add(entity);
                return await ctx.SaveChangesAsync() == 1;
            }
        }

        public IEnumerable<OrderListItem> GetOrders()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Orders
                        .Where(e => e.Customer.UserId == _userId)
                        .Select(
                            e =>
                                new OrderListItem
                                {
                                    OrderId = e.OrderId,
                                    TotalOrderCost = e.TotalOrderCost,
                                }

                                );

                return query.ToArray();
            }
        }

        public OrderDetail GetOrderById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Orders
                        .Single(e => e.OrderId == id && e.Customer.UserId == _userId);

                var query = ctx
                    .FilmOrders
                    .Where(fo => fo.OrderId == entity.OrderId)
                    .Select(fo => new FilmOrderDetail { FilmTitle = fo.Film.Title });

                return
                    new OrderDetail
                    {
                        OrderId = entity.OrderId,
                        OrderDate = entity.OrderDate,
                        TotalOrderCost = entity.TotalOrderCost,
                        FilmTitles = query.ToArray(),
                        CustomerId = entity.CustomerId

                    };
            }
        }

        public async Task<bool> UpdateOrderAsync(OrderEdit model) // check for id in controller
        {
            var filmOrderService = new FilmOrderService(_userId);
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Orders
                        .Single(e => e.OrderId == model.OrderId && e.Customer.UserId == _userId);

                entity.OrderDate = model.OrderDate;
                //// ---------------------------------------------------- Dean got tired ------------------------
                var query =
                    ctx
                        .FilmOrders
                        .Where(fo => fo.OrderId == entity.OrderId);

                var currentFilmOrders = await query.ToArrayAsync();

                foreach (var oldFilm in currentFilmOrders)
                {
                    foreach (var newFilm in model.FilmIds)
                    {
                        if (!(model.FilmIds.Contains(oldFilm.FilmId)))
                        {
                            await filmOrderService.DeleteFilmOrderByIdAsync(item.Id);
                        }
                    }
                    // grab films with this order number and check to see if they are in the model
                    //   if they are, do nothing
                    //   if they aren't, delete
                    //   add any remaining films
                    
                    
                }
                // order number 55
                // before films 1,2,3
                // after films 3,7,8,9

                // delete FilmOrder for 55/1, 55/2
                // keep FilmOrder 55/3
                // add 55/7, 55/8, 55/9
                //// ---------------------------------------------------- Dean got tired ------------------------

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteOrder(int orderId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Orders
                        .Single(e => e.OrderId == orderId && e.Customer.UserId == _userId);

                ctx.Orders.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        
    }
}

