using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RankItems.Pages.Shows
{
    public class IndexModel : PageModel
    {
        public List<ShowInfo> listShows = new List<ShowInfo>();

        public string ConnectionStatus { get; set; }

        // Add the SearchTerm property
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet(string searchTerm)
        {
            try
            {
                string connectionString = "Data Source=MTG;Initial Catalog=master;Integrated Security=True";

                if (IsDatabaseConnectionValid(connectionString))
                {
                    ConnectionStatus = "Connected";
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        RetrieveShowsBySearch(connectionString, searchTerm);
                    }
                    else
                    {
                        RetrieveShows(connectionString);
                    }
                }
                else
                {
                    ConnectionStatus = "Connection Failed";
                }
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"Exception: {ex.ToString()}";
            }
        }

        private bool IsDatabaseConnectionValid(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void RetrieveShows(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM shows";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ShowInfo showInfo = new ShowInfo();
                            showInfo.id = reader.GetInt32(0).ToString();
                            showInfo.name = reader.GetString(1);
                            showInfo.story = reader.GetString(2);
                            showInfo.VisualEffects = reader.GetString(3);
                            showInfo.Engagement = reader.GetString(4);
                            showInfo.created_at = reader.GetDateTime(5).ToString();

                            listShows.Add(showInfo);
                        }
                    }
                }
            }
        }

        private void RetrieveShowsBySearch(string connectionString, string searchTerm)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM shows WHERE name LIKE @searchTerm";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ShowInfo showInfo = new ShowInfo();
                            showInfo.id = reader.GetInt32(0).ToString();
                            showInfo.name = reader.GetString(1);
                            showInfo.story = reader.GetString(2);
                            showInfo.VisualEffects = reader.GetString(3);
                            showInfo.Engagement = reader.GetString(4);
                            showInfo.created_at = reader.GetDateTime(5).ToString();

                            listShows.Add(showInfo);
                        }
                    }
                }
            }
        }
    }

    public class ShowInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string story { get; set; }
        public string VisualEffects { get; set; }
        public string Engagement { get; set; }
        public string created_at { get; set; }
    }
}
