using CustomersAPI.Business.Adtractions;
using CustomersAPI.Controllers;
using CustomersAPI.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace CustomersApi.Test
{
    public class SecurityControllerTest
    {
        [Fact]
        public void Login_Returns_CorrectToken()
        {
            //Arrange
            var userLogin = A.Fake<UserLogin>();
            userLogin.UserName = "Test";
            userLogin.Password = "Test";

            var securityBusiness = A.Fake<ISecurityBusiness>();
            A.CallTo(() => securityBusiness.IsValidCredentials(userLogin)).Returns(1);
            var controller = new SecurityController(securityBusiness);

            //Act
            var actionResult = controller.Login(userLogin);

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var tkn = result.Value as LoginResult;
            Assert.NotNull(tkn.AccessToken);
        }

        [Fact]
        public void Login_InvalidCredentials()
        {
            //Arrange
            var userLogin = A.Fake<UserLogin>();
            userLogin.UserName = "Test";
            userLogin.Password = "Test";

            var securityBusiness = A.Fake<ISecurityBusiness>();
            A.CallTo(() => securityBusiness.IsValidCredentials(userLogin)).Returns(0);
            var controller = new SecurityController(securityBusiness);

            //Act
            var actionResult = controller.Login(userLogin);

            //Assert
            var result = actionResult.Result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Credentials.", result.Value);
            
        }
    }
}