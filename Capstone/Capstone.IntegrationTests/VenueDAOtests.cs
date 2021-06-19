using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Data.SqlClient;
using System.Collections.Generic;



namespace Capstone.IntegrationTests
{
    [TestClass]
    public class VenueDAOtests : IntegrationTestBase
    {
        [TestMethod]
        public void GetVenuesShouldReturnOneRow()
        {
            // Arrange
            VenueDAO dao = new VenueDAO(ConnectionString);
            
            // Act
            IList<Venue> results = dao.GetAllVenues();
            // Assert
            Assert.IsTrue(results.Count > 0);
            Assert.AreEqual(1, results.Count);
        }
        [TestMethod]
        public void TestMethod2()
        {
            // Arrange

            // Act

            // Assert
        }
        [TestMethod]
        public void TestMethod3()
        {
            // Arrange

            // Act

            // Assert
        }
        [TestMethod]
        public void TestMethod4()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
