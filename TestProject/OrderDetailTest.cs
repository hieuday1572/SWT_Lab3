using BusinessObject;
using DataAccess.Repository;
using eStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace eStore

{
    public class OrderDetailTest
    {
        private OrderDetailsController controller;
        private Mock<IOrderDetailRepository> mockOrderDetailRepository;
        [SetUp]
        public void Setup()
        {
            mockOrderDetailRepository = new Mock<IOrderDetailRepository>();
            // Khởi tạo controller với mock repository (có thể sử dụng DI trong thực tế)
            controller = new OrderDetailsController(mockOrderDetailRepository.Object);
        }

        [Test]
        public void Test_Index_Returns_View_With_001_Non_Empty_OrderDetails()
        {
            // Arrange: Chuẩn bị dữ liệu
            List<OrderDetail> orderDetails = new List<OrderDetail>
            {
                new OrderDetail{ OrderId=1, ProductId=1, UnitPrice=10000, Quantity=1, Discount=20},
                new OrderDetail{ OrderId=2, ProductId=2, UnitPrice=20000, Quantity=2, Discount=30}
            };

            // Thiết lập giả định cho mock repository
            mockOrderDetailRepository.Setup(repo => repo.GetOrderDetails()).Returns(orderDetails);

            // Act: Thực thi hàm Index
            IActionResult result = controller.Index().Result;

            // Assert: Kiểm tra kết quả
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            var model = viewResult.Model as IEnumerable<OrderDetail>;
            Assert.AreEqual(orderDetails.Count, model.Count());

            // Kiểm tra liệu GetMembers của MemberRepository đã được gọi hay chưa
            mockOrderDetailRepository.Verify(repo => repo.GetOrderDetails(), Times.Once);
        }
        [Test]
        public void Test_Index_Returns_View_With_010_Empty_OrderDetails()
        {
            // Arrange: Chuẩn bị dữ liệu rỗng
            List<OrderDetail> members = new List<OrderDetail>();

            // Thiết lập giả định cho mock repository
            mockOrderDetailRepository.Setup(repo => repo.GetOrderDetails()).Returns(members);

            // Act: Thực thi hàm Index
            IActionResult result = controller.Index().Result;

            // Assert: Kiểm tra kết quả
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            var model = viewResult.Model as IEnumerable<OrderDetail>;
            Assert.AreEqual(members.Count, model.Count());

            // Kiểm tra liệu GetMembers của MemberRepository đã được gọi hay chưa
            mockOrderDetailRepository.Verify(repo => repo.GetOrderDetails(), Times.Once);
        }

        [Test]
        public void Test_Details_With_Null_Id_Returns_NotFound()
        {
            // Arrange
            int id = 0;
            int proId = 1;
            // Act
            IActionResult result = controller.Details(id, proId).Result;

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Test_Details_With_Null_OrderDetail_Returns_NotFound()
        {
            // Arrange
            int id = 1;
            int proId = 1;

            // Giả định hàm GetOrderDetail trả về null
            mockOrderDetailRepository.Setup(repo => repo.GetOrderDetail(id, proId)).Returns((OrderDetail)null);

            // Act
            IActionResult result = controller.Details(id, proId).Result;

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Test_Details_With_Valid_Data_Returns_ViewResult()
        {
            // Arrange
            int id = 1;
            int proId = 1;

            // Giả định hàm GetOrderDetail trả về một order detail hợp lệ
            OrderDetail validOrderDetail = new OrderDetail { OrderId = 1, ProductId = 1, UnitPrice = 1, Quantity = 1, Discount = 1 };
            mockOrderDetailRepository.Setup(repo => repo.GetOrderDetail(id, proId)).Returns(validOrderDetail);

            // Act
            IActionResult result = controller.Details(id, proId).Result;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(validOrderDetail, viewResult.Model);
        }
    }
}