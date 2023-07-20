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
        private List<OrderDetail> orderDetails; 
[SetUp]
        public void Setup()
        {
            mockOrderDetailRepository = new Mock<IOrderDetailRepository>();
            // Khởi tạo controller với mock repository (có thể sử dụng DI trong thực tế)
            controller = new OrderDetailsController(mockOrderDetailRepository.Object);
            orderDetails = new List<OrderDetail>
            {
                new OrderDetail { OrderId=1, ProductId=1, UnitPrice=10000, Quantity=1, Discount=20},
                new OrderDetail { OrderId=2, ProductId=2, UnitPrice=20000, Quantity=2, Discount=30}
            };
            mockOrderDetailRepository.Setup(repo => repo.GetOrderDetail(1, 1)).Returns(orderDetails[0]);
            mockOrderDetailRepository.Setup(repo => repo.GetOrderDetail(2, 2)).Returns(orderDetails[1]);
            mockOrderDetailRepository.Setup(repo => repo.GetOrderDetails()).Returns(orderDetails);
        }

        [Test]
        public void Test_A_Index_Returns_View_With_001_Non_Empty_OrderDetails()
        {
            // Arrange: Chuẩn bị dữ liệu
            List<OrderDetail> orderDetails_Index = new List<OrderDetail>
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
        public void Test_A_Index_Returns_View_With_010_Empty_OrderDetails()
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
        public void Test_B_Details_With_Null_Id_Returns_NotFound()
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
        public void Test_B_Details_With_Null_OrderDetail_Returns_NotFound()
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
        public void Test_B_Details_With_Valid_Data_Returns_ViewResult()
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

        [Test]
        public void Test_C_Create_With_Existing_OrderDetail_Returns_View_With_OrderDetail()
        {
            // Arrange: Chọn một order detail có trong danh sách
            OrderDetail existingOrderDetail = orderDetails[0];

            // Act
            IActionResult result = controller.Create(existingOrderDetail).Result;

            // Assert: Hàm phải trả về một View với orderDetail
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(existingOrderDetail, viewResult.Model);
        }

        [Test]
        public void Test_C_Create_With_Non_Existing_OrderDetail_And_Valid_Model_Redirects_To_Index()
        {
            // Arrange: Chọn một order detail không có trong danh sách
            OrderDetail newOrderDetail = new OrderDetail { OrderId = 3, ProductId = 3, UnitPrice = 20000, Quantity = 2, Discount = 30 };

            // Act
            IActionResult result = controller.Create(newOrderDetail).Result;

            // Assert: Hàm phải thực hiện redirect tới action "Index"
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        

    [Test]
    public void Test_D_Edit_GET_With_Valid_Id_And_Valid_ProductId_Returns_View_With_OrderDetail()
    {
        // Arrange: Chọn một id và proId hợp lệ
        int id = 1;
        int proId = 1;

        // Act
        IActionResult result = controller.Edit(id, proId).Result;

        // Assert: Hàm phải trả về một View với orderDetail
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.AreEqual(orderDetails[0], viewResult.Model);
    }

    [Test]
    public void Test_D_Edit_GET_With_Invalid_OrderId_Returns_NotFoundResult()
    {
        // Arrange: Chọn một id không hợp lệ, proId hợp lệ
        int id = 3;
        int proId = 1;

        // Act
        IActionResult result = controller.Edit(id, proId).Result;

        // Assert: Hàm phải trả về NotFoundResult
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public void Test_D_Edit_GET_With_Invalid_ProductId_Returns_NotFoundResult()
    {
        // Arrange: Chọn một id hợp lệ, proId không hợp lệ,
        int id = 1;
        int proId = 3;

        // Act
        IActionResult result = controller.Edit(id, proId).Result;

        // Assert: Hàm phải trả về NotFoundResult
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public void Test_D_Edit_POST_With_Valid_Id_And_Valid_Model_Redirects_To_Index()
    {
        // Arrange: Chọn một id hợp lệ, và dữ liệu hợp lệ cho orderDetail
        OrderDetail validOrderDetail = new OrderDetail { OrderId = 1, ProductId = 1, UnitPrice = 10000, Quantity = 1, Discount = 50 };

        // Act
        IActionResult result = controller.Edit(validOrderDetail).Result;

        // Assert: Hàm phải thực hiện redirect tới action "Index"
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToActionResult = result as RedirectToActionResult;
        Assert.AreEqual("Index", redirectToActionResult.ActionName);

        // Hàm Update của repository được gọi đúng lần với đúng đối tượng validOrderDetail
        mockOrderDetailRepository.Verify(repo => repo.Update(validOrderDetail), Times.Once);
    }

        [Test]
        public void Test_E_Delete_With_Valid_Id_Returns_View_With_OrderDetail()
        {
            // Arrange: Chọn một id hợp lệ và proId hợp lệ
            int id = 1;
            int proId = 1;

            // Act
            IActionResult result = controller.Delete(id, proId).Result;

            // Assert: Hàm phải trả về một View với orderDetail
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(orderDetails[0], viewResult.Model);
        }

        [Test]
        public void Test_E_Delete_With_Invalid_Id_Returns_NotFoundResult()
        {
            // Arrange: Chọn một id không hợp lệ và proId không hợp lệ
            int id = 3;
            int proId = 1;

            // Act
            IActionResult result = controller.Delete(id, proId).Result;

            // Assert: Hàm phải trả về NotFoundResult
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Test_E_DeleteConfirmed_With_Valid_Id_And_Valid_ProductId_Redirects_To_Index()
        {
            // Arrange: Chọn một id hợp lệ và proId hợp lệ
            int id = 1;
            int proId = 1;

            // Act
            IActionResult result = controller.DeleteConfirmed(id, proId).Result;

            // Assert: Hàm phải thực hiện redirect tới action "Index"
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);

            // Hàm Delete của repository được gọi đúng lần với đúng id và proId
            mockOrderDetailRepository.Verify(repo => repo.Delete(id, proId), Times.Once);
        }

        [Test]
        public void Test_E_DeleteConfirmed_With_Non_Existing_OrderDetail_Returns_NotFoundResult()
        {
            // Arrange: Chọn một id không hợp lệ và proId không hợp lệ, _context.OrderDetails không có order detail nào
            int id = 3;
            int proId = 3;

            // Act
            IActionResult result = controller.DeleteConfirmed(id, proId).Result;

            // Assert: Hàm phải trả về NotFoundResult
            Assert.IsInstanceOf<NotFoundResult>(result);

            // Hàm Delete của repository không được gọi
            mockOrderDetailRepository.Verify(repo => repo.Delete(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
    
