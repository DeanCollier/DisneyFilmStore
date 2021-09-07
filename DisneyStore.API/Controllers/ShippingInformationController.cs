using DisneyFilmStore.Models.ShippingInformationModels;
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
    public class ShippingInformationController : ApiController
    {
        [Authorize]
        public class CustomerController : ApiController
        {
            public ShippingInformationService CreateShippingInformationService()
            {
                var userId = Guid.Parse(User.Identity.GetUserId());
                var shippingService = new ShippingInformationService(userId);
                return shippingService;
            }

            //[HttpPost]  // Created in OrderService with new order
            //[Route("api/ShippingInformation")]
            //public async Task<IHttpActionResult> PostShippingAsync([FromBody] ShippingInfoCreate model)
            //{
            //    if (!ModelState.IsValid)
            //        return BadRequest(ModelState);

            //    var service = CreateShippingInformationService();

            //    if (!(await service.CreateShippingInfoAsync(model)))
            //        return InternalServerError();

            //    return Ok();
            //}

            [HttpGet]
            [Route("api/ShippingInformation")]
            public async Task<IHttpActionResult> GetAllCustomersAsync()
            {
                var service = CreateShippingInformationService();
                var shipmentList = await service.GetAllShippingInfosAsync();
                return Ok(shipmentList);
            }

            [HttpGet]
            [Route("api/ShippingInformation/{id}")]
            public IHttpActionResult GetCustomerById([FromUri] int id)
            {
                var service = CreateShippingInformationService();
                var shipmentDetail = service.GetShippingInfoById(id); // what will happen if the customer doesn't exist?
                return Ok(shipmentDetail);
            }

            [HttpPut]
            [Route("api/ShippingInformation/{id}")]
            public async Task<IHttpActionResult> PutShippingByIdAsync([FromUri] int id, [FromBody] ShippingInfoEdit model)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var service = CreateShippingInformationService();

                if (!(await service.UpdateShippingInfoByIdAsync(id, model)))
                    return InternalServerError();

                var shipmentDetail = service.GetShippingInfoById(id);

                return Ok(shipmentDetail);
            }
        }
    }
}
