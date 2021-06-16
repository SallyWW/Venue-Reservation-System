using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class DepartmentDAOtests : UnitTestBase
    {
        [TestMethod]
        public void GetDepartmentsShouldReturnAllDepartments()
        {
            //Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            int expectedResults = GetRowCount("department");

            //Act
            ICollection<Department> results = dao.GetDepartments();

            //Assert
            Assert.IsTrue(results.Count > 0);
            Assert.AreEqual(expectedResults, results.Count);
        }

        [TestMethod]
        public void CreateDepartmentsShouldIncreaseCountBy1()
        {
            //Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            Department testDept = new Department();
            testDept.Name = "doesntmatter";

            int oldRowCount = GetRowCount("department");

            //Act
            int newDeptId = dao.CreateDepartment(testDept);

            //Assert
            Assert.IsTrue(newDeptId > 0);
            Assert.AreEqual(oldRowCount + 1, GetRowCount("department"));
        }

        [TestMethod]
        public void UpdateDepartmentShouldChangeNameOfDepartment()
        {
            //Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            Department expectedDepartment = new Department();
            expectedDepartment.Name = "project x";
            expectedDepartment.Id = 1;

            //Act
            bool success = dao.UpdateDepartment(expectedDepartment);

            //Assert
            Assert.IsTrue(success);
        }
    }
}
