using DisneyFilmStore.Models.CustomerModels;
using DisneyFilmStore.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DisneyFilmStore.Controllers
{
    [Authorize]
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        public CustomerService CreateCustomerService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var customerService = new CustomerService(userId);
            return customerService;
        }

        [HttpPost]
        [Route("api/Customer")]
        public async Task<IHttpActionResult> PostCustomerAsync([FromBody] CustomerCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateCustomerService();

            if (!(await service.CreateCustomerAsync(model)))
                return InternalServerError();

            return Ok();
        }

        [HttpGet]
        [Route("api/Customer")]
        public async Task<IHttpActionResult> GetAllCustomersAsync()
        {
            var service = CreateCustomerService();
            var customerList = await service.GetAllCustomersAsync();
            return Ok(customerList);
        }

        [HttpGet]
        [Route("api/Customer/{id}")]
        public async Task<IHttpActionResult> GetCustomerByIdAsync([FromUri] int id)
        {
            var service = CreateCustomerService();
            var customerDetail = service.GetCustomerById(id); // what will happen if the customer doesn't exist?
            return Ok(customerDetail);
        }

        [HttpPut]
        [Route("api/Customer/{id}")]
        public async Task<IHttpActionResult> PutCustomerByIdAsync([FromUri] int id, [FromBody] CustomerEdit model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateCustomerService();

            if (!(await service.UpdateCustomerByIdAsync(id, model)))
                return InternalServerError();

            var customerDetail = service.GetCustomerById(id);

            return Ok(customerDetail);
        }

        [HttpDelete]
        [Route("api/Customer/{id}")]
        public async Task<IHttpActionResult> DeteteCustomerByIdAsync([FromUri] int id)
        {
            var service = CreateCustomerService();
            if (!(await service.DeleteCustomerByIdAsync(id)))
                return NotFound();

            return Ok("Customer Deleted");

        }
    }
}
