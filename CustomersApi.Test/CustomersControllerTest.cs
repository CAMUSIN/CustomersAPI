using CustomersAPI.Business.Abstractions;
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
    public class CustomersControllerTest
    {
        #region POST/ADDING

        [Fact]
        public void Post_ValidCustomer_PostCustomer()
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

            //Valid customer
            var validCustomerModel = A.Fake<CustomerModel>();
            validCustomerModel.Success = true;

            //Valid resultModel
            var validResultModel = A.Fake<ResultModel>();
            validResultModel.Success = true;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.AddCustomer(validCustomerModel)).Returns(validResultModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.Post(validCustomerModel);

            //Assert
            var okResult = result as OkObjectResult;
            var resultUserModel = okResult.Value as ResultModel;
            Assert.True(resultUserModel.Success);
        }

        [Fact]
        public void Post_InvalidCustomer_PostCustomer()
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

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.Post(null); //Invalid Customer

            //Assert
            var badResult = result as BadRequestObjectResult;
            Assert.Equal("Null userData", badResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }

        [Fact]
        public void Post_InvalidDB_PostCustomer()
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

            //Valid resultModel
            var validResultModel = A.Fake<ResultModel>();
            validResultModel.Success = false;
            validResultModel.Message = "Error Test";

            //Valid customer
            var validCustomerModel = A.Fake<CustomerModel>();
            validCustomerModel.Success = true;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.AddCustomer(validCustomerModel)).Returns(validResultModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.Post(validCustomerModel);

            //Assert
            var badResult = result as BadRequestObjectResult;
            Assert.Equal("Db Operation Failed.", badResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }

        #endregion

        #region GET/SELECT

        [Fact]
        public void Get_InvalidToken_GetCustomers()
        {
            //Arrange
            var identity = new GenericIdentity("toeknTest", "testUser");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            //Invalid Token
            var invalidToken = A.Fake<ResultModel>();
            invalidToken.Success = false;
            invalidToken.Message = "Invalid Token";
            invalidToken.Result = string.Empty;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(invalidToken);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var actionResult = controller.Get();

            //Assert
            var outResult = actionResult as BadRequestObjectResult;
            var resultModel = outResult.Value as ResultModel;
            Assert.Equal("Invalid Token", resultModel.Message);
            Assert.Equal(StatusCodes.Status400BadRequest, outResult.StatusCode);
        }

        [Fact]
        public void Get_Correct_GetCustomers()
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

            //Valid customers
            var validCustomersList = A.CollectionOfDummy<CustomerModel>(5).AsEnumerable();

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.GetCustomers()).Returns(validCustomersList.ToList());

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.Get();

            //Assert
            var okResult = result as OkObjectResult;
            var resultUserModel = okResult.Value as List<CustomerModel>;
            Assert.NotEmpty(resultUserModel);
        }

        [Fact]
        public void Get_InvalidDB_GetCustomers()
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

            //Valid customers
            var validCustomersList = A.CollectionOfDummy<CustomerModel>(0).AsEnumerable();

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.GetCustomers()).Returns(validCustomersList.ToList());

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.Get();

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.NotEqual("", okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact]
        public void Get_ValidId_GetCustomerById()
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

            //Valid customer
            var validCustomerModel = A.Fake<CustomerModel>();
            validCustomerModel.Success = true;

            //Valid Id
            var validId = 1;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.GetCustomerById(validId)).Returns(validCustomerModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.GetById(validId);

            //Assert
            var okResult = result as OkObjectResult;
            var resultUserModel = okResult.Value as CustomerModel;
            Assert.True(resultUserModel.Success);
        }

        [Fact]
        public void Get_InvalidId_GetCustomerById()
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

            //Valid customer
            var validCustomerModel = A.Fake<CustomerModel>();
            validCustomerModel.Success = true;

            //Valid Id
            var invalidId = 0;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.GetCustomerById(invalidId)).Returns(validCustomerModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.GetById(invalidId);

            //Assert
            var okResult = result as BadRequestObjectResult;
            Assert.Equal("Id not provided or invalid", okResult.Value);
        }

        [Fact]
        public void Get_InvalidDB_GetCustomerById()
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

            //Valid customer
            var invalidCustomerModel = A.Fake<CustomerModel>();
            invalidCustomerModel.Success = false;
            invalidCustomerModel.Message = "Error Test";

            //Valid Id
            var validId = 1;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.GetCustomerById(validId)).Returns(invalidCustomerModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.GetById(validId);

            //Assert
            var badResult = result as BadRequestObjectResult;
            Assert.Equal("Db Operation Failed.", badResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }

        #endregion

        #region PUT/UPDATE

        [Fact]
        public void Put_ValidId_PutCustomerById()
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

            //Valid customer
            var validCustomerModel = A.Fake<CustomerModel>();
            validCustomerModel.Success = true;

            //Valid resultModel
            var validResultModel = A.Fake<ResultModel>();
            validResultModel.Success = true;

            //Valid Id
            var validId = 1;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.UpdateCustomerById(validId, validCustomerModel)).Returns(validResultModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.PutById(validId, validCustomerModel);

            //Assert
            var okResult = result as OkObjectResult;
            var resultUserModel = okResult.Value as ResultModel;
            Assert.True(resultUserModel.Success);
        }

        [Fact]
        public void Put_InvalidId_PutCustomerById()
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

            //Valid customer
            var validCustomerModel = A.Fake<CustomerModel>();
            validCustomerModel.Success = true;

            //Valid resultModel
            var validResultModel = A.Fake<ResultModel>();
            validResultModel.Success = true;

            //Valid Id
            var invalidId = 0;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.UpdateCustomerById(invalidId, validCustomerModel)).Returns(validResultModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.PutById(invalidId, validCustomerModel);

            //Assert
            var badResult = result as BadRequestObjectResult;
            Assert.Equal("Null customerData", badResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }

        [Fact]
        public void Put_InvalidDB_PostCustomerById()
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

            //Valid customer
            var validCustomerModel = A.Fake<CustomerModel>();
            validCustomerModel.Success = true;

            //Valid resultModel
            var invalidResultModel = A.Fake<ResultModel>();
            invalidResultModel.Success = false;
            invalidResultModel.Message = "Error Test";

            //Valid Id
            var validId = 1;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.UpdateCustomerById(validId, validCustomerModel))
                .Returns(invalidResultModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.PutById(validId, validCustomerModel);

            //Assert
            var badResult = result as BadRequestObjectResult;
            Assert.Equal("Db Operation Failed.", badResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }

        #endregion

        #region DELETE

        [Fact]
        public void Delete_ValidId_DeleteCustomerById()
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

            //Valid resultModel
            var validResultModel = A.Fake<ResultModel>();
            validResultModel.Success = true;

            //Valid Id
            var validId = 1;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.DeleteCustomerById(validId))
                .Returns(validResultModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.DeleteById(validId);

            //Assert
            var okResult = result as OkObjectResult;
            var resultUserModel = okResult.Value as ResultModel;
            Assert.True(resultUserModel.Success);
        }

        [Fact]
        public void Delete_InvalidId_DeleteCustomerById()
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

            //Valid Id
            var invalidId = 0;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.DeleteById(invalidId);

            //Assert
            var badResult = result as BadRequestObjectResult;
            Assert.Equal("Id not provided or invalid", badResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }

        [Fact]
        public void Delete_InvalidDB_DeleteCustomerById()
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

            //Valid customer
            var validCustomerModel = A.Fake<CustomerModel>();
            validCustomerModel.Success = true;

            //Valid resultModel
            var invalidResultModel = A.Fake<ResultModel>();
            invalidResultModel.Success = false;
            invalidResultModel.Message = "Error Test";

            //Valid Id
            var validId = 1;

            var customerBusiness = A.Fake<ICustomerBusiness>();
            var securityBusiness = A.Fake<ISecurityBusiness>();

            A.CallTo(() => securityBusiness.ValidateToken(identity)).Returns(validToken);
            A.CallTo(() => customerBusiness.DeleteCustomerById(validId))
                .Returns(invalidResultModel);

            var controller = new CustomersController(securityBusiness, customerBusiness)
            {
                ControllerContext = controllerContext
            };

            //Act
            var result = controller.DeleteById(validId);

            //Assert
            var badResult = result as BadRequestObjectResult;
            Assert.Equal("Db Operation Failed.", badResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }

        #endregion
    }
}
