using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private readonly string connectionString;
        private const string SqlSelectAllEmployees = "SELECT employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date FROM employee";
        private const string SqlEmployeeSearchFirstLast = "SELECT employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date FROM employee WHERE first_name LIKE @first_name AND last_name LIKE @last_name";
        private const string SqlEmployeeSearchActiveProject = "SELECT e.last_name, e.first_name, e.job_title, e.birth_date, p.project_id, p.to_date " +
            "FROM employee e JOIN project_employee pe ON e.employee_id = pe.employee_id " +
            "JOIN project p ON pe.project_id = p.project_id " +
            "WHERE p.to_date < @today";

        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public ICollection<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAllEmployees, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(GetEmployeeFromDataReader(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("problems getting Employees " + ex.Message);
            }
            return employees;
        }

        /// <summary>
        /// Find all employees whose names contain the search strings.
        /// Returned employees names must contain *both* first and last names.
        /// </summary>
        /// <remarks>Be sure to use LIKE for proper search matching.</remarks>
        /// <param name="firstname">The string to search for in the first_name field</param>
        /// <param name="lastname">The string to search for in the last_name field</param>
        /// <returns>A list of employees that matches the search.</returns>
        public ICollection<Employee> Search(string firstname, string lastname)
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlEmployeeSearchFirstLast, conn);

                    command.Parameters.AddWithValue("@first_name", firstname);
                    command.Parameters.AddWithValue("@last_name", lastname);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        employees.Add(GetEmployeeFromDataReader(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("problems getting Employees " + ex.Message);
            }
            return employees;
        }

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlEmployeeSearchActiveProject, conn);

                    command.Parameters.AddWithValue("@today", DateTime.Now);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);

                        employees.Add(employee);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("problems getting Employees " + ex.Message);
            }
            return employees;
        }

        private Employee GetEmployeeFromDataReader(SqlDataReader reader)
        {
            Employee employee = new Employee();

            employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
            employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
            employee.FirstName = Convert.ToString(reader["first_name"]);
            employee.LastName = Convert.ToString(reader["last_name"]);
            employee.JobTitle = Convert.ToString(reader["job_title"]);
            employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
            employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

            return employee;
        }
    }
}
