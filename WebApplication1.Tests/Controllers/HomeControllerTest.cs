using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Controllers;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using Moq;
using System.Web;
using System.Text;

namespace WebApplication1.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private ICreateRandomArray getCreateRandomArray() { return new CreateTwoDimensionalArray(); }
        private ICreateFile getCreateFile() { return new CreateCSV(); }
                
        [TestMethod]
        public void Index_ViewResult_NotNull()
        {
            // Arrange
            HomeController controller = new HomeController(getCreateRandomArray(), getCreateFile());
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
               
        [TestMethod]
        public void Index_ViewEqual_IndexCshtml()
        {
            // Arrange
            HomeController controller = new HomeController(getCreateRandomArray(), getCreateFile());
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void CreateRandomArray_ReturnType()
        {
            // Arrange
            var mock = new Mock<ICreateRandomArray>();
            mock.Setup(a => a.CreateRandomArray(0, 0, 0, 0)).Returns(new int[0, 0]);
            CreateTwoDimensionalArray arr = new CreateTwoDimensionalArray();
            // Act
            var result = arr.CreateRandomArray(0, 0, 0, 0);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new int[0, 0].GetType(), result.GetType());
        }
        
        [TestMethod]
        public void CreateRandomArray_CheckLength()
        {
            // Arrange
            var array = getCreateRandomArray();
            int col = 3, row = 5;
            // Act
            int[,] result = array.CreateRandomArray(col, row, 0, 0);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(col*row, result.Length);
        }

        [TestMethod]
        public void CreateRandomArray_CheckMinMaxRange()
        {
            // Arrange
            var array = getCreateRandomArray();
            int min = 5, max = 10;
            // Act
            int[,] result = array.CreateRandomArray(5, 5, min, max);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Cast<int>().Max() <= max && result.Cast<int>().Min() >= min);
        }

        [TestMethod]
        public void ExportToCSV_CheckFileName()
        {
            // Arrange
            string csv = "10,11,12,\r\n13,14,15,\r\n16,17,18,\r\n";
            var contex = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            session.SetupGet(s => s["matrix.csv"]).Returns(csv);
            contex.Setup(m => m.HttpContext.Session).Returns(session.Object);
            // Act
            HomeController controller = new HomeController(getCreateRandomArray(), getCreateFile());
            controller.ControllerContext = contex.Object;            
            // Assert
            Assert.AreEqual(csv, controller.Session["matrix.csv"]);
        }
        
        [TestMethod]
        public void DownloadFile_IsInstanceOfType()
        {
            // Arrange            
            var contex = new Mock<ControllerContext>();
            var session = new Mock<HttpSessionStateBase>();
            session.SetupGet(s => s["matrix.csv"]).Returns(new StringBuilder());
            contex.Setup(m => m.HttpContext.Session).Returns(session.Object);
            // Act
            HomeController controller = new HomeController(getCreateRandomArray(), getCreateFile());
            controller.ControllerContext = contex.Object;
            ActionResult result = controller.DownloadFile("matrix.csv");
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreNotEqual("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ((FileResult)result).ContentType);
            Assert.AreEqual("application/csv", ((FileResult)result).ContentType);
        }
    }
}
