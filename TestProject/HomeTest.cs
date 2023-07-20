using System.Threading.Tasks;
using BusinessObject;
using DataAccess.Repository;
using eStore.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace eStore.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _homeController;
        private Mock<IMemberRepository> _memberRepositoryMock;

        [SetUp]
        public void Setup()
        {
            // Thiết lập đối tượng IMemberRepository giả
            _memberRepositoryMock = new Mock<IMemberRepository>();

            // Khởi tạo HomeController với đối tượng repository giả
            _homeController = new HomeController(
                Mock.Of<ILogger<HomeController>>(),
                Mock.Of<IHttpContextAccessor>(),
                _memberRepositoryMock.Object
            );
        }

        [Test]
        public void Profile_Update_Successful()
        {
            // Sắp xếp
            var memberId = 1;
            var member = new Member
            {
                MemberId = memberId,
                Email = "test@example.com",
                CompanyName = "Công ty Test",
                City = "Thành phố Test",
                Country = "Quốc gia Test",
                /* Thêm các thuộc tính thành viên khác ở đây */
            };

            // Thiết lập đối tượng IMemberRepository giả cho cập nhật thành công
            _memberRepositoryMock.Setup(m => m.Update(It.IsAny<Member>())).Verifiable();

            // Thiết lập đối tượng giả ITempDataDictionary
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.SetupSet(t => t["Error"] = It.IsAny<string>());

            _homeController.TempData = tempDataMock.Object;

            // Cài đặt ModelState.IsValid thành true
            _homeController.ModelState.Clear();

            // Hành động
            var result = _homeController.Profile(memberId, member) as ViewResult;

            // Khẳng định
            _memberRepositoryMock.Verify(); // Đảm bảo rằng phương thức Update đã được gọi

            // Kiểm tra TempData
            tempDataMock.VerifySet(t => t["Error"] = "Update successful!", Times.Once);

            // Kiểm tra ViewResult
            Assert.IsNotNull(result);
            Assert.AreEqual(member, result.Model as Member);
        }

        [Test]
        public void Profile_Update_Unsuccessful_InvalidModelState()
        {
            // Sắp xếp
            var memberId = 1;
            var member = new Member
            {
                MemberId = memberId,
                Email = "test@example.com",
                CompanyName = "Công ty Test",
                City = "Thành phố Test",
                Country = "Quốc gia Test",
                /* Thêm các thuộc tính thành viên khác ở đây */
            };

            // Thiết lập đối tượng giả ITempDataDictionary
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.SetupSet(t => t["Error"] = It.IsAny<string>());

            _homeController.TempData = tempDataMock.Object;

            // Cài đặt ModelState.IsValid thành false (mô phỏng ModelState không hợp lệ)
            _homeController.ModelState.AddModelError("Email", "Email is required");

            // Hành động
            var result = _homeController.Profile(memberId, member) as ViewResult;

            // Khẳng định
            // Kiểm tra TempData
            tempDataMock.VerifySet(t => t["Error"] = "Wrong: please try again!", Times.Once);

            // Kiểm tra ViewResult
            Assert.IsNotNull(result);
            Assert.AreEqual(member, result.Model as Member);
        }
    }
}
