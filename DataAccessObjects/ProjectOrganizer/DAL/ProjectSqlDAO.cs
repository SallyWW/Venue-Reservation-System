using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class ProjectSqlDAO : IProjectDAO
    {
        private readonly string connectionString;
        private const string SqlSelectAllProjects = "SELECT project_id, name, from_date, to_date FROM project";
        private const string SqlInsertEmployeeToProject = "INSERT INTO project_employee (project_id, employee_id) VALUES (@project_id, @employee_id)";
        private const string SqlRemoveEmployeeFromProject = "DELETE FROM project_employee WHERE project_id = @project_id AND employee_id = @employee_id";
        private const string SqlCreateProject = "INSERT INTO project (name, from_date, to_date) VALUES (@name, @from_date, @to_date); SELECT @@IDENTITY;";

        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAllProjects, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        projects.Add(GetProjectFromDataReader(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("problems getting Projects " + ex.Message);
            }
            return projects;
        }

        /// <summary>
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlInsertEmployeeToProject, conn);
                    command.Parameters.AddWithValue("@project_id", projectId);
                    command.Parameters.AddWithValue("@employee_id", employeeId);

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem assigning employee to project: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlRemoveEmployeeFromProject, conn);
                    command.Parameters.AddWithValue("@project_id", projectId);
                    command.Parameters.AddWithValue("@employee_id", employeeId);

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem removing employee from project: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlCreateProject, conn);
                    command.Parameters.AddWithValue("@name", newProject.Name);
                    command.Parameters.AddWithValue("@from_date", newProject.StartDate);
                    command.Parameters.AddWithValue("@to_date", newProject.EndDate);

                    int id = Convert.ToInt32(command.ExecuteScalar());
                    return id;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("problems creating Project " + ex.Message);
                return 0;
            }
        }

        private Project GetProjectFromDataReader(SqlDataReader reader)
        {
            Project project = new Project();

            project.ProjectId = Convert.ToInt32(reader["project_id"]);
            project.Name = Convert.ToString(reader["name"]);
            project.StartDate = Convert.ToDateTime(reader["from_date"]);
            project.EndDate = Convert.ToDateTime(reader["to_date"]);
           

            return project;
        }
    }
}
