using BusinessObject;
using DataAccess.Repository;
using eStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestProject
{
    [TestFixture]
    public class MembersTest
    {
        private MembersController _controller;
        private Mock<IMemberRepository> _mockMemberRepository;

        [SetUp]
        public void Setup()
        {
            _mockMemberRepository = new Mock<IMemberRepository>();

            // Khởi tạo controller với mock repository.
            _controller = new MembersController(_mockMemberRepository.Object);
        }

        [Test]
        public void Index_ReturnsViewWithMembers()
        {
            // Arrange
            var expectedMembers = new List<Member>
        {
            new Member { MemberId = 1, CompanyName = "fpt", Email="member1@gmail.com",City="hanoi",Country="vietnam",Password="12345"},
            new Member { MemberId = 2, CompanyName = "fptt", Email="member2@gmail.com",City="hanoii",Country="vietnamm",Password="123456" }
        };

            // Thiết lập hành vi cho mock repository.
            _mockMemberRepository.Setup(x => x.GetMembers()).Returns(expectedMembers);

            // Act
            var result = _controller.Index().Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedMembers, result.Model);
        }
        [Test]
        public async Task Details_WithValidEmail_ReturnsViewResultWithCorrectModel()
        {
            // Arrange
            var email = "example@example.com";
            var expectedMember = new Member
            {
                MemberId = 1,
                CompanyName = "Example Company",
                Email = email,
                City = "Example City",
                Country = "Example Country",
                Password = "examplepassword"
            };
            _mockMemberRepository.Setup(x => x.GetMember(email)).Returns(expectedMember);

            // Act
            var result = await _controller.Details(email);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.AreEqual(expectedMember, viewResult.Model);
        }

        [Test]
        public async Task Details_WithNullEmail_ReturnsNotFoundResult()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_WithInvalidEmail_ReturnsNotFoundResult()
        {
            // Arrange
            var email = "invalid@example.com";
            _mockMemberRepository.Setup(x => x.GetMember(email)).Returns((Member)null);

            // Act
            var result = await _controller.Details(email);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task Create_ValidMember_RedirectsToIndex()
        {
            // Arrange
            var validMember = new Member
            {
                MemberId = 1,
                CompanyName = "Example Company",
                Email = "example@example.com",
                City = "Example City",
                Country = "Example Country",
                Password = "examplepassword"
            };

            // Act
            var result = await _controller.Create(validMember);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToAction.ActionName);
        }

        [Test]
        public async Task Create_InvalidMember_ReturnsViewResultWithModel()
        {
            // Arrange
            var invalidMember = new Member(); // Invalid member with ModelState errors

            // Simulate ModelState invalid state
            _controller.ModelState.AddModelError("Email", "Email is required.");

            // Act
            var result = await _controller.Create(invalidMember);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.AreEqual(invalidMember, viewResult.Model);
        }
        [Test]
        public async Task Edit_ValidModel_RedirectsToIndex()
        {
            // Arrange
            int memberId = 1;
            var member = new Member
            {
                MemberId = memberId,
                Email = "example@example.com",
                CompanyName = "Example Company",
                City = "Example City",
                Country = "Example Country",
                Password = "examplepassword"
            };
            _mockMemberRepository.Setup(x => x.Update(member));

            // Act
            var result = await _controller.Edit(memberId, member);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToAction.ActionName);
        }

        [Test]
        public async Task Edit_InvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            int memberId = 1;
            var member = new Member
            {
                // Set invalid properties of the member to trigger ModelState.IsValid failure
            };
            _controller.ModelState.AddModelError("Email", "Email is required.");

            // Act
            var result = await _controller.Edit(memberId, member);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.AreEqual(member, viewResult.Model);
        }
        [Test]
        public async Task DeleteConfirmed_ExistingEmail_RedirectsToIndex()
        {
            // Arrange
            string email = "example@example.com";
            var existingMember = new Member
            {
                Email = email,
                // Set other properties of the member as needed
            };
            _mockMemberRepository.Setup(x => x.GetMember(email))
                .Returns(await Task.FromResult(existingMember)); // Sử dụng Task.FromResult

            // Act
            var result = await _controller.DeleteConfirmed(email);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToAction.ActionName);

            // Verify that Delete method of repository is called
            _mockMemberRepository.Verify(x => x.Delete(existingMember.Email), Times.Once);
        }

        [Test]
        public async Task DeleteConfirmed_NonExistingEmail_RedirectsToIndex()
        {
            // Arrange
            string email = "nonexistent@example.com";
            _mockMemberRepository.Setup(x => x.GetMember(email))
                .Returns(await Task.FromResult<Member>(null)); // Sử dụng Task.FromResult

            // Act
            var result = await _controller.DeleteConfirmed(email);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToAction = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectToAction.ActionName);

            // Verify that Delete method of repository is not called
            _mockMemberRepository.Verify(x => x.Delete(It.IsAny<string>()), Times.Never);
        }
    }
}



