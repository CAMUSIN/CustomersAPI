using CustomersAPI.Business.Adtractions;
using CustomersAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityBusiness _securityBussiness;

        public SecurityController(ISecurityBusiness securityBusiness)
        {
            _securityBussiness = securityBusiness;
        }

        [AllowAnonymous]
        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin) 
        {
            var IdUser = _securityBussiness.IsValidCredentials(userLogin);
            if (IdUser > 0)
            {
                return new OkObjectResult(await GenerateToken(userLogin, IdUser));
            }
            else 
            {
                return BadRequest("Invalid Credentials.");
            }
        }

        [HttpPost]
        [Route("Admin/AddRole")]
        public Task AsignRole()
        {
            //TODO: Implement Roles management logic
            return null;
        }

        [HttpGet]
        [Route("Admin/GetAllRoles")]
        public string GetAllRoles()
        {
            //TODO: Implement Roles management logic
            return null;
        }

        private async Task<LoginResult> GenerateToken(UserLogin userLogin, int IdUser) {
            var userRoles = await _securityBussiness.UserRoles(userLogin);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userLogin.UserName),
                    new Claim(ClaimTypes.NameIdentifier, IdUser.ToString()),
                    new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
                    new Claim("Roles", userRoles),
                    new Claim("IdUser", IdUser.ToString()),
                    new Claim("User", userLogin.UserName)
                };

            JwtSecurityToken token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BallastLaneChallengeKey-BallastLaneChallengeKey-BallastLaneChallengeKey-BallastLaneChallengeKey")),
                        SecurityAlgorithms.HmacSha256)),
                    new JwtPayload(claims)
                );

            var output = new LoginResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = userLogin.UserName
            };

            return output;
        }
    }
}
