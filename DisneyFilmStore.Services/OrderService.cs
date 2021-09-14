using DisneyFilmStore.Data;
using DisneyFilmStore.Models.FilmModels;
using DisneyFilmStore.Models.FilmOrderModels;
using DisneyFilmStore.Models.OrderModels;
using DisneyFilmStore.Models.ShippingInformationModels;
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
            int changeCount = 0;
            var entity =
                new Order()
                {
                    TotalOrderCost = GetTotalCostOfOrder(model.FilmIds, model.CustomerId), // write some calc for this based on films, prices, and member status
                    CustomerId = model.CustomerId,
                    OrderDate = DateTime.Now,
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Orders.Add(entity);
                if (await ctx.SaveChangesAsync() == 1)
                    changeCount++;
            }

            var shippingService = new ShippingInformationService(_userId);
            ShippingInfoCreate shippingInfo = new ShippingInfoCreate { OrderId = entity.OrderId, CustomerId = entity.CustomerId };
            if (await shippingService.CreateShippingInfoAsync(shippingInfo))
                changeCount++;

            var filmOrderService = new FilmOrderService(_userId);
            foreach (var filmId in model.FilmIds)
            {
                var filmOrderCreate = new FilmOrderCreate { FilmId = filmId, OrderId = entity.OrderId };
                if (await filmOrderService.CreateFilmOrderAsync(filmOrderCreate))
                    changeCount++;
            }
            return (changeCount == 1 + 1 + model.FilmIds.Count());
            
        }

        private decimal GetTotalCostOfOrder(IEnumerable<int> filmIds, int customerId)
        {
            var customerService = new CustomerService(_userId);
            var filmService = new FilmService(_userId);

            decimal totalOrderCost = 0m;
            bool isMember = customerService.GetCustomerById(customerId).Member;
            foreach (var filmId in filmIds)
            {
                var filmDetail = filmService.GetFilmById(filmId);
                if (isMember)
                {
                    totalOrderCost += filmDetail.MemberCost;
                }
                else
                {
                    totalOrderCost += filmDetail.NonMemberCost;
                }
            }
            return totalOrderCost;
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
                                    CreatedUtc = e.OrderDate,
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
                await filmOrderService.UpdateFilmOrderFromOrderUpdateAsync(model);
                entity.TotalOrderCost = GetTotalCostOfOrder(model.FilmIds, entity.CustomerId);
                entity.OrderDate = DateTime.Now;

                return await ctx.SaveChangesAsync() == 1;
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

                await filmOrderService.UpdateFilmOrderFromOrderUpdateAsync(
                    new OrderEdit
                    {
                        OrderId = entity.OrderId,
                        FilmIds = new List<int>() // use blank list to compare for updates
                    });
                // one shipment
                await shippingService.DeleteShippingInfoByOrderIdAsync(entity.OrderId);

                ctx.Orders.Remove(entity);

                return ctx.SaveChanges() == (foChanges + shipChanges + 1);
            }
        }

        
    }
}

