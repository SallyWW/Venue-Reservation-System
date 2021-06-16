using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class EmployeeDAOtests : UnitTestBase
    {
        [TestMethod]
        public void GetEmployeeShouldReturnAllEmployees()
        {
            //Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);
            int expectedResults = GetRowCount("employee");

            //Act
            ICollection<Employee> results = dao.GetAllEmployees();

            //Assert
            Assert.IsTrue(results.Count > 0);
            Assert.AreEqual(expectedResults, results.Count);
        }

        [TestMethod]
        public void SearchEmployeeFirstNameLastName_Should_Return1Count()
        {
            //Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            //Act
            ICollection<Employee> results = dao.Search("bilbo", "baggins");

            //Assert
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void SearchEmployeesNotOnProject_Should_ReturnCount1()
        {
            //Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            //Act
            ICollection<Employee> results = dao.GetEmployeesWithoutProjects();

            //Assert
            Assert.AreEqual(1, results.Count);
        }
    }
}
