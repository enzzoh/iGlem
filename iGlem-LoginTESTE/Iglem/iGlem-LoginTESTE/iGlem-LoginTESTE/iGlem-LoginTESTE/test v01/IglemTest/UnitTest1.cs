using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using test_v01.Controllers;
using test_v01.Repository.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using test_v01.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IglemTest
{
    public class DocumentosControllerTests
    {
        private Mock<SITEtccDbContext> _mockContext;
        private DocumentosController _controller;

        public DocumentosControllerTests()
        {
            _mockContext = new Mock<SITEtccDbContext>();
            _controller = new DocumentosController();
        }

        private ClaimsPrincipal GetAdminUser()
        {
            var claims = new List<Claim>()
            {
                new Claim("IsAdmin", "true")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public async Task Index()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = GetAdminUser() }
            };

            var mockSet = new Mock<DbSet<Documento>>();
            _mockContext.Setup(m => m.Documentos).Returns(mockSet.Object);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create()
        {
            // Arrange
            var documento = new Documento { Caminhodocumento = "TestPath", Documentonome = "TestName", Idusuario = 1 };
            _mockContext.Setup(m => m.Documentos.Add(documento)).Returns(Mock.Of<EntityEntry<Documento>>());
            _mockContext.Setup(m => m.SaveChangesAsync(default)).Returns(Task.FromResult(1));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = GetAdminUser() }
            };

            // Act
            var result = await _controller.Create(documento);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        public async Task Edit()
        {
            // Arrange
            var documento = new Documento { Documentoid = 1, Caminhodocumento = "TestPath", Documentonome = "TestName", Idusuario = 1 };
            var mockSet = new Mock<DbSet<Documento>>();

            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(documento); // Certifique-se de que o documento está retornando
            _mockContext.Setup(m => m.Documentos).Returns(mockSet.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = GetAdminUser() }
            };

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<ViewResult>(result); // Deve retornar um ViewResult se o documento for encontrado
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var documento = new Documento { Documentoid = 1, Caminhodocumento = "TestPath", Documentonome = "TestName", Idusuario = 1 };
            var mockSet = new Mock<DbSet<Documento>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(documento);
            _mockContext.Setup(m => m.Documentos).Returns(mockSet.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = GetAdminUser() }
            };

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            mockSet.Verify(m => m.Remove(documento), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
