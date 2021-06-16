using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class ProjectDAOtests : UnitTestBase
    {
        [TestMethod]
        public void GetDepartmentsShouldReturnAllDepartments()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);
            int expectedResults = GetRowCount("project");

            //Act
            ICollection<Project> results = dao.GetAllProjects();

            //Assert
            Assert.IsTrue(results.Count > 0);
            Assert.AreEqual(expectedResults, results.Count);
        }

        [TestMethod]
        public void AssignEmployeeToProject_ShouldResultTrue()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);
            
            int oldCount = GetRowCount("project_employee");

            //Act
            bool addedEmployee = dao.AssignEmployeeToProject(1, 2);

            //Assert
            Assert.IsTrue(addedEmployee);
            Assert.AreEqual(oldCount + 1, GetRowCount("project_employee"));
        }

        [TestMethod]
        public void RemoveEmployeeFromProject_ShouldReturnTrue()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);

            //Act
            bool removedEmployee = dao.RemoveEmployeeFromProject(1, 1);

            //Assert
            Assert.IsTrue(removedEmployee);
        }

        [TestMethod]
        public void CreateProjectShouldIncreaseCountBy1()
        {
            //Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);
            Project testProject = new Project();
            testProject.Name = "doesntmatter";
            testProject.StartDate = Convert.ToDateTime("2021-01-01");
            testProject.EndDate = Convert.ToDateTime("2021-06-16");

            int oldRowCount = GetRowCount("project");

            //Act
            int newProjectId = dao.CreateProject(testProject);

            //Assert
            Assert.IsTrue(newProjectId > 1);
            Assert.AreEqual(oldRowCount + 1, GetRowCount("project"));
        }
    }
}
