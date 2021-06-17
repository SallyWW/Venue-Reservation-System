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
        private const string SqlSelectAllVenues = "SELECT v.id, v.name, c.name, s.name, s.abbreviation, v.description " +
            "FROM venue v JOIN city c ON v.city_id = c.id JOIN state s ON c.state_abbreviation = s.abbreviation " +
            "ORDER BY v.name";
        private const string SqlSelectGetCategories = "SELECT v.id, v.name, c.name FROM venue v " +
                    "JOIN category_venue cv ON v.id = cv.venue_id " +
                    "JOIN category c ON c.id = cv.category_id" +
                    "ORDER BY v.name";


        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Venue> GetAllVenues()
        {
            IList<Venue> venues = new List<Venue>();
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
                        venue.Id = Convert.ToInt32(reader["v.id"]);
                        venue.Name = Convert.ToString(reader["v.name"]);
                        venue.Description = Convert.ToString(reader["v.description"]);
                        venue.StateName = Convert.ToString(reader["s.name"]);
                        venue.CityName = Convert.ToString(reader["c.name"]);
                        venue.StateCode = Convert.ToString(reader["s.abbreviation"]);
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
        public IList<string> GetCategoriesForVenues(Venue venue)
        {
            IList<string> categories = new List<string>();
            try
            {
                string currentID = Convert.ToString(venue.Id);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectGetCategories, conn);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string category = "";
                        string lineID = reader["id"].ToString();
                        if (currentID != lineID)
                        {
                            continue;
                        }
                        else if (currentID == lineID)
                        {
                            category = reader["name"].ToString();
                            categories.Add(category);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting venues: " + ex.Message);
            }


            return categories;

        }
    }
}
