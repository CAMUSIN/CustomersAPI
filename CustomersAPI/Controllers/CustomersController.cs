using CustomersAPI.Business.Abstractions;
using CustomersAPI.Business.Adtractions;
using CustomersAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ISecurityBusiness _securityBusiness;
        private readonly ICustomerBusiness _customerBusiness;

        public CustomersController(
            ISecurityBusiness securityBusiness,
            ICustomerBusiness customerBusiness)
        {
            _securityBusiness = securityBusiness;
            _customerBusiness = customerBusiness;
        }

        // GET: api/<CustomersController>
        [HttpGet]
        public IActionResult Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var validation = _securityBusiness.ValidateToken(identity);

            if (!validation.Success) 
                return BadRequest(validation);

            var customers = _customerBusiness.GetCustomers();

            if (customers.Count <= 0)
                return BadRequest("Db Operation Failed.");

            return Ok(customers);
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var validation = _securityBusiness.ValidateToken(identity);

            if (!validation.Success)
                return BadRequest(validation);

            if(id < 1)
                return BadRequest("Id not provided or invalid");

            var customer= _customerBusiness.GetCustomerById(id);

            if (!customer.Success)
                return BadRequest("Db Operation Failed.");

            return Ok(customer);
        }

        // POST api/<CustomersController>
        [HttpPost]
        public IActionResult Post([FromBody] CustomerModel customer)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var validation = _securityBusiness.ValidateToken(identity);

            if (!validation.Success)
                return BadRequest(validation);

            if (customer == null)
                return BadRequest("Null userData");

            var result = _customerBusiness.AddCustomer(customer);

            if (!result.Success)
                return BadRequest("Db Operation Failed.");

            return Ok(result);
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public IActionResult PutById(int id, [FromBody] CustomerModel customer)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var validation = _securityBusiness.ValidateToken(identity);

            if (!validation.Success)
                return BadRequest(validation);

            if (customer == null || id <= 0)
                return BadRequest("Null customerData");

            var result = _customerBusiness.UpdateCustomerById(id, customer);

            if (!result.Success)
                return BadRequest("Db Operation Failed.");

            return Ok(result);
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var validation = _securityBusiness.ValidateToken(identity);

            if (!validation.Success)
                return BadRequest(validation);

            if (id < 1)
                return BadRequest("Id not provided or invalid");

            var result = _customerBusiness.DeleteCustomerById(id);

            if (!result.Success)
                return BadRequest("Db Operation Failed.");

            return Ok(result);
        }
    }
}
