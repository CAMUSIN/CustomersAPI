using CustomersAPI.Business.Adtractions;
using CustomersAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness _userBussiness;
        private readonly ISecurityBusiness _securityBussiness;

        public UsersController( 
            IUserBusiness userBusiness,
            ISecurityBusiness securityBusiness) 
        {
            _userBussiness = userBusiness;
            _securityBussiness = securityBusiness;
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] UserLogin userData)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var validation = _securityBussiness.ValidateToken(identity);
            if (!validation.Success) 
                return new BadRequestObjectResult(validation);

            if (userData == null)
                return BadRequest("Null userData");

            var result = _userBussiness.AddUser(userData);

            if (!result.Success)
                return BadRequest("Insertion Failed.");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var validation = _securityBussiness.ValidateToken(identity);
            if (!validation.Success)
                return new BadRequestObjectResult(validation);

            if (id < 1)
                return BadRequest("Id not provided or invalid");

            var user = _userBussiness.GetUserById(id);

            if (!user.Success)
                return BadRequest(user.Message);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult PutById(int id, [FromBody] UserLogin user) 
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var validation = _securityBussiness.ValidateToken(identity);
            if (!validation.Success)
                return new BadRequestObjectResult(validation);

            if (id < 1)
                return BadRequest("Id not provided or invalid");

            var result = _userBussiness.UpdateUserById(id, user);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id) 
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var validation = _securityBussiness.ValidateToken(identity);
            if (!validation.Success)
                return new BadRequestObjectResult(validation);

            if (id < 1)
                return BadRequest("Id not provided or invalid");

            var result = _userBussiness.DeleteUserById(id);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
