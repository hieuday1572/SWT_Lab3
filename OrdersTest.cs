using System.Collections.Generic;
using BusinessObject;
using DataAccess.Repository;
using eStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace YourNUnitProjectNamespace
{
    [TestFixture]
    public class OrdersTest
    {
        private OrdersController controller; 
        private Mock<IOrderRepository> mockOrderRepository;

        [SetUp]
        public void Setup()
        {
            // Create a mock instance of IOrderRepository
            mockOrderRepository = new Mock<IOrderRepository>();

            // Create an instance of the controller using the mock repository (you can use DI in a real project)
            controller = new OrdersController(mockOrderRepository.Object);
        }

        [Test]
        public async Task Index_Returns_ViewResult_With_OrdersList()
        {
            // Arrange
            List<Order> ordersList = new List<Order>
    {
        new Order { OrderId = 1, MemberId = 1, OrderDate = DateTime.Now, Freight = 10 },
        new Order { OrderId = 2, MemberId = 2, OrderDate = DateTime.Now, Freight = 15 }
    };

            // Set up the mock repository to return the list of orders when GetOrders is called
            mockOrderRepository.Setup(repo => repo.GetOrders()).Returns(ordersList);

            // Act
            var controller = new OrdersController(mockOrderRepository.Object);
            IActionResult result = await controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            var model = viewResult.Model as List<Order>;
            Assert.AreEqual(ordersList.Count, model.Count);
        }

        [Test]
        public async Task Create_With_Valid_MemberId_Calls_Create_On_Repository_And_Returns_Redirect()
        {
            // Arrange
            Order order = new Order { MemberId = 1, OrderDate = DateTime.Now, Freight = 10 }; // Setting MemberId to a valid value

            // Act
            IActionResult result = await controller.Create(order);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);

            // Verify that the Create method of the mock repository is called with the correct Order object
            mockOrderRepository.Verify(repo => repo.Create(order), Times.Once);
        }

        [Test]
        public async Task Edit_With_Valid_Model_Returns_RedirectToAction()
        {
            // Arrange
            int validId = 1;
            Order validOrder = new Order { OrderId = validId, MemberId = 1, OrderDate = DateTime.Now, Freight = 10 };

            // Mock the Update method of the repository
            mockOrderRepository.Setup(repo => repo.Update(validOrder));

            // Act
            IActionResult result = await controller.Edit(validId, validOrder);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }



    }
}
