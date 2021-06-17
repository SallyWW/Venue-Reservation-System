using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueDAO
    {
        private readonly string connectionString;
        private const string SqlSelectAllVenues = "SELECT id, name, city_id, description FROM venue ORDER BY name";

        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Venue> GetAllVenues()
        {
            List<Venue> venues = new List<Venue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAllVenues, conn);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Venue venue = new Venue();
                        venue.Id = Convert.ToInt32(reader["id"]);
                        venue.Name = Convert.ToString(reader["name"]);
                        venue.CityId = Convert.ToInt32(reader["city_id"]);
                        venue.Description = Convert.ToString(reader["description"]);
                        venues.Add(venue);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting venues: " + ex.Message);
            }

            
            return venues;
        }
    }

}
