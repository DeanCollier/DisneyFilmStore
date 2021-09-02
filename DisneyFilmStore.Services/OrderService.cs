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
                    .Select(fo => new FilmOrderTitle { FilmTitle = fo.Film.Title });

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

                entity.OrderDate = DateTime.Now;
                int changeCount = await filmOrderService.UpdateFilmOrderFromOrderUpdateAsync(model);

                return await ctx.SaveChangesAsync() == (changeCount + 1);
            }
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var filmOrderService = new FilmOrderService(_userId);
            var shippingService = new ShippingInformationService(_userId);
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Orders
                        .Single(e => e.OrderId == orderId && e.Customer.UserId == _userId);

                // delete possibly multiple FilmOrders
                int foChanges = await filmOrderService.UpdateFilmOrderFromOrderUpdateAsync(
                    new OrderEdit
                    {
                        OrderId = entity.OrderId,
                        FilmIds = new List<int>() // use blank list to compare for updates
                    });
                // one shipment
                int shipChanges = await shippingService.DeleteShippingInfoByOrderIdAsync(entity.OrderId);

                ctx.Orders.Remove(entity);

                return ctx.SaveChanges() == (foChanges + shipChanges + 1);
            }
        }

        
    }
}

