using CustomersAPI.Business.Adtractions;
using CustomersAPI.Controllers;
using CustomersAPI.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace CustomersApi.Test
{
    public class UsersControllerTest
    {
        #region POST/ADDING

        [Fact]
        public void Post_InvalidToken_AddUser()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext(){ User = contextUser };
            var controllerContext = new ControllerContext(){ HttpContext = httpContext };

            //Invalid Token
            var invalidToken = A.Fake<ResultModel>();
            invalidToken.Success = false;
            invalidToken.Message = "Invalid Token";
            invalidToken.Result = string.Empty;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(invalidToken);

            var controller = new UsersController(userBusiness, securityBusiness) {
                ControllerContext = controllerContext
            };

            var userLogin = A.Fake<UserLogin>();
            userLogin.UserName = "Test";
            userLogin.Password = "Test";

            //Act
            var actionResult = controller.Post(userLogin);  

            //Assert
            var outResult = actionResult as BadRequestObjectResult;
            var resultModel = outResult.Value as ResultModel;
            Assert.Equal("Invalid Token", resultModel.Message);
        }

        [Fact]
        public void Post_Correct_AddUser() 
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

               //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

               //Valid BD Response
            var validDBResponse = A.Fake<ResultModel>();
            validDBResponse.Success = true;
            validDBResponse.Message = string.Empty;
            validDBResponse.Result = "-1";

            //Valid UserLogin
            var userLogin = A.Fake<UserLogin>();
            userLogin.UserName="Test";
            userLogin.Password="Test";

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => userBusiness.AddUser(userLogin)).Returns(validDBResponse);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.Post(userLogin);

            //Assert
            var okResult = result as OkObjectResult;
            var resultModel = okResult.Value as ResultModel;
            Assert.NotEmpty(resultModel.Result);
            Assert.True(resultModel.Success);
        }

        [Fact]
        public void Post_NullUserData_AddUser()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Valid UserLogin
            var userLogin = A.Fake<UserLogin>();
            userLogin.UserName = "Test";
            userLogin.Password = "Test";

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.Post(null); //Null userData

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.Equal("Null userData", okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact]
        public void Post_InvalidDB_AddUser()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            var invalidDBResponse = A.Fake<ResultModel>();
            invalidDBResponse.Success = false;
            invalidDBResponse.Message = "testError";
            invalidDBResponse.Result = string.Empty;

            //Valid UserLogin
            var userLogin = A.Fake<UserLogin>();
            userLogin.UserName = "Test";
            userLogin.Password = "Test";

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => userBusiness.AddUser(userLogin)).Returns(invalidDBResponse);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.Post(userLogin);

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.Equal("Insertion Failed.", okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        #endregion

        #region GET/SELECT

        [Fact]
        public void Get_CorrectId_GetUserById()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Valid usermodel
            var validUserModel = A.Fake<UserModel>();
            validUserModel.Success = true;
            validUserModel.UserName = "Test";
            validUserModel.Id = 1;

            //Valid Id
            var validId = 1;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => userBusiness.GetUserById(validId)).Returns(validUserModel);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.GetById(validId);

            //Assert
            var okResult = result as OkObjectResult;
            var resultUserModel = okResult.Value as UserModel;
            Assert.True(resultUserModel.Success);
            Assert.Equal("Test", resultUserModel.UserName);
        }

        [Fact]
        public void Get_IncorrectId_GetUserById()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Invalid Id
            var invalidId = 0;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);


            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.GetById(invalidId);

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.Equal("Id not provided or invalid", okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact]
        public void Get_InvalidDB_GetUserById()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Valid usermodel
            var invalidUserModel = A.Fake<UserModel>();
            invalidUserModel.Success = false;
            invalidUserModel.Message = "No results";

            //Valid Id
            var validId = 1;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => userBusiness.GetUserById(validId)).Returns(invalidUserModel);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.GetById(validId);

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.NotEqual("",okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        #endregion

        #region PUT/UPDATE

        [Fact]
        public void Put_ValidId_PutUserById()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Valid BD Response
            var validDBResponse = A.Fake<ResultModel>();
            validDBResponse.Success = true;
            validDBResponse.Message = string.Empty;
            validDBResponse.Result = "-1";

            //Valid usermodel
            var validUserLogin = A.Fake<UserLogin>();
            validUserLogin.Password = "Test";
            validUserLogin.UserName = "Test";

            //Valid Id
            var validId = 1;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => userBusiness.UpdateUserById(validId, validUserLogin)).Returns(validDBResponse);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.PutById(validId, validUserLogin);

            //Assert
            var okResult = result as OkObjectResult;
            var resultUserModel = okResult.Value as ResultModel;
            Assert.True(resultUserModel.Success);
            Assert.NotEqual("", resultUserModel.Result);
        }

        [Fact]
        public void Put_InvalidId_PutUserById()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Valid usermodel
            var validUserLogin = A.Fake<UserLogin>();
            validUserLogin.Password = "Test";
            validUserLogin.UserName = "Test";

            //Invalid Id
            var invalidId = 0;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.PutById(invalidId, validUserLogin);

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.Equal("Id not provided or invalid", okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact]
        public void Put_InvalidDB_PutUserById()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Valid BD Response
            var invalidDBResponse = A.Fake<ResultModel>();
            invalidDBResponse.Success = false;
            invalidDBResponse.Message = "Test Error";

            //Valid usermodel
            var validUserLogin = A.Fake<UserLogin>();
            validUserLogin.Password = "Test";
            validUserLogin.UserName = "Test";

            //Valid Id
            var validId = 1;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => userBusiness.UpdateUserById(validId, validUserLogin)).Returns(invalidDBResponse);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.PutById(validId, validUserLogin);

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.NotEqual("", okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        #endregion

        #region DELETE

        [Fact]
        public void Delete_ValidId_DeleteUserById()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Valid BD Response
            var validDBResponse = A.Fake<ResultModel>();
            validDBResponse.Success = true;
            validDBResponse.Message = string.Empty;
            validDBResponse.Result = "-1";

            //Valid Id
            var validId = 1;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => userBusiness.DeleteUserById(validId)).Returns(validDBResponse);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.DeleteById(validId);

            //Assert
            var okResult = result as OkObjectResult;
            var resultUserModel = okResult.Value as ResultModel;
            Assert.True(resultUserModel.Success);
            Assert.NotEqual("", resultUserModel.Result);
        }

        [Fact]
        public void Delete_InvalidId_DeleteUserById()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Invalid Id
            var invalidId = 0;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.DeleteById(invalidId);

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.Equal("Id not provided or invalid", okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact]
        public void Delete_InvalidDB_DeleteUserById()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Valid Token
            var validToken = A.Fake<ResultModel>();
            validToken.Success = true;
            validToken.Message = string.Empty;
            validToken.Result = string.Empty;

            //Valid BD Response
            var invalidDBResponse = A.Fake<ResultModel>();
            invalidDBResponse.Success = false;
            invalidDBResponse.Message = "Test Error";

            //Valid Id
            var validId = 1;

            var userBusiness = A.Fake<IUserBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => userBusiness.DeleteUserById(validId)).Returns(invalidDBResponse);

            var controller = new UsersController(userBusiness, securityBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.DeleteById(validId);

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.NotEqual("", okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        #endregion
    }
}
