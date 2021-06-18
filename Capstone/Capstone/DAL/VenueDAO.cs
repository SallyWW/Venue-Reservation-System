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
        private const string SqlSelectAllVenues = "SELECT v.id, v.name, c.name AS cityName, s.name AS stateName, s.abbreviation, v.description " +
            "FROM venue v JOIN city c ON v.city_id = c.id JOIN state s ON c.state_abbreviation = s.abbreviation " +
            "ORDER BY v.name";
        private const string SqlSelectGetCategories = "SELECT c.name FROM venue v " +
                    "JOIN category_venue cv ON v.id = cv.venue_id " +
                    "JOIN category c ON c.id = cv.category_id " +
                    "WHERE v.id = @venueid " +
                    "ORDER BY v.name";
        private const string SqlGetVenueSpaces = "SELECT s.id, s.name, s.is_accessible, s.open_from, s.open_to, s.daily_rate, s.max_occupancy " +
            "FROM venue v JOIN space s ON v.id = s.venue_id " +
            "WHERE v.id = @id";
        private const string SqlSelectAvailableSpaces = "SELECT TOP 5 s.id, s.name, s.is_accessible, s.daily_rate, s.max_occupancy " +
            "FROM venue v JOIN space s ON v.id = s.venue_id JOIN reservation r ON r.space_id = s.id " +
            "WHERE s.max_occupancy >= @userOccupancy " +
            "AND s.open_from <= @startMonth " +
            "AND s.open_to >= @endMonth " +
            "AND r.start_date >= @startDate " +
            "AND r.end_date <= @endDate";


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
                        venue.Id = Convert.ToInt32(reader["id"]);
                        venue.Name = Convert.ToString(reader["name"]);
                        venue.Description = Convert.ToString(reader["description"]);
                        venue.StateName = Convert.ToString(reader["stateName"]);
                        venue.CityName = Convert.ToString(reader["cityName"]);
                        venue.StateCode = Convert.ToString(reader["abbreviation"]);
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

                    command.Parameters.AddWithValue("@venueid", venue.Id);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(reader.GetString(0));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting venues: " + ex.Message);
            }
            return categories;
        }

        public IList<Space> GetSpacesForVenue(string venue_id)
        {
            IList<Space> spaces = new List<Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetVenueSpaces, conn);

                    command.Parameters.AddWithValue("@id", venue_id);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Space space = new Space();
                        space.Id = Convert.ToInt32(reader["id"]);
                        space.Name = Convert.ToString(reader["name"]);
                        space.WheelchairAccessible = Convert.ToBoolean(reader["is_accessible"]);
                        space.OpenMonth = ConvertNullDate(reader, "open_from");
                        space.CloseMonth = ConvertNullDate(reader, "open_to");
                        space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
                        space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        spaces.Add(space);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting spaces: " + ex.Message);
            }
            return spaces;
        }

        private int ConvertNullDate(SqlDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return -1;
            }
            return Convert.ToInt32(reader[columnName]);
        }

        public IList<Space> GetSpacesForVenue(string venue_id, DateTime startDate, int numberOfDays, int occupancy)
        {
            IList<Space> spaces = new List<Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetVenueSpaces, conn);

                    command.Parameters.AddWithValue("@userOccupancy", occupancy);

                    command.Parameters.AddWithValue("@startMonth", startDate);
                    command.Parameters.AddWithValue("@endMonth", );
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", );

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Space space = new Space();
                        space.Id = Convert.ToInt32(reader["id"]);
                        space.Name = Convert.ToString(reader["name"]);
                        space.WheelchairAccessible = Convert.ToBoolean(reader["is_accessible"]);
                        space.OpenMonth = ConvertNullDate(reader, "open_from");
                        space.CloseMonth = ConvertNullDate(reader, "open_to");
                        space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
                        space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        spaces.Add(space);
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Problem returning available spaces:" + ex.Message);
            }
            return spaces;
        }
    }
}
