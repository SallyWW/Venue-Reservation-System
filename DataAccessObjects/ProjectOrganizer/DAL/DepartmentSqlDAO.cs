using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private readonly string connectionString;
        private const string SqlInsertDepartment = "INSERT INTO department (department_id, name) VALUES (@department_id, @name);";
        private const string SqlSelectAllDepartments = "SELECT department_id, name FROM department";

        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public ICollection<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAllDepartments, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        departments.Add(GetDepartmentFromDataReader(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("problems getting Departments " + ex.Message);
            }
            return departments;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlInsertDepartment, conn);
                    command.Parameters.AddWithValue("@department_id", newDepartment.Id);
                    command.Parameters.AddWithValue("@name", newDepartment.Name);

                    //command.ExecuteNonQuery();
                    int id = Convert.ToInt32(command.ExecuteScalar());
                    return id;
                   // Console.WriteLine("Just created department " + id);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("problems creating Department " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            throw new NotImplementedException();
        }

        private Department GetDepartmentFromDataReader(SqlDataReader reader)
        {
            Department department = new Department();

            department.Id = Convert.ToInt32(reader["department_id"]);
            department.Name = Convert.ToString(reader["name"]);

            return department;
        }
    }
}
