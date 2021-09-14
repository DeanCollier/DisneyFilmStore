using DisneyFilmStore.Models.OrderModels;
using DisneyFilmStore.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DisneyStore.API.Controllers
{
    public class OrderController : ApiController
    {
        private OrderService CreateOrderService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var orderService = new OrderService(userId);
            return orderService;
        }

        [HttpGet]
        [Route("api/Order")]
        public IHttpActionResult Get()
        {
            OrderService orderService = CreateOrderService();
            var orders = orderService.GetOrders();
            return Ok(orders);
        }

        [HttpGet]
        [Route("api/Order/{id}")]
        public IHttpActionResult Get(int id)
        {
            OrderService orderService = CreateOrderService();
            var order = orderService.GetOrderById(id);
            return Ok(order);
        }

        [HttpPost]
        [Route("api/Order")]
        public async Task<IHttpActionResult> OrderAsync(OrderCreate order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateOrderService();

            if (!(await service.CreateOrderAsync(order)))
                return InternalServerError();

            return Ok();
        }

        [HttpPut]
        [Route("api/Order/{id}")]
        public async Task<IHttpActionResult> Put(OrderEdit order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateOrderService();

            if (!(await service.UpdateOrderAsync(order)))
                return InternalServerError();

            return Ok();
        }

        [HttpDelete]
        [Route("api/Order/{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var service = CreateOrderService();

            if (!(await service.DeleteOrderAsync(id)))
                return InternalServerError();

            return Ok();
        }
    }

}
