using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace RankItems.Pages.Shows
{
    public class EditModel : PageModel
    {
        public ShowInfo showInfo = new ShowInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public int? storyValue;
        public int? visualEffectsValue;
        public int? engagementValue;

        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                string connectionString = "Data Source=MTG;Initial Catalog=master;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM shows WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                showInfo.id = reader.GetInt32(0).ToString();
                                showInfo.name = reader.GetString(1);
                                showInfo.story = reader.GetString(2);
                                showInfo.VisualEffects = reader.GetString(3);
                                showInfo.Engagement = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public IActionResult OnPost()
        {
            showInfo.id = Request.Form["id"];
            showInfo.name = Request.Form["name"];

            int story;
            int visualEffects;
            int engagement;
            if (!int.TryParse(Request.Form["story"], out story) || story < 1 || story > 10 ||
                !int.TryParse(Request.Form["VisualEffects"], out visualEffects) || visualEffects < 1 || visualEffects > 10 ||
                !int.TryParse(Request.Form["Engagement"], out engagement) || engagement < 1 || engagement > 10)
            {
                errorMessage = "Invalid input for story, VisualEffects, or Engagement. Please enter a number between 1 and 10.";
                return Page();
            }

            showInfo.story = story.ToString();
            showInfo.VisualEffects = visualEffects.ToString();
            showInfo.Engagement = engagement.ToString();

            if (string.IsNullOrEmpty(showInfo.name) || string.IsNullOrEmpty(showInfo.story) ||
                string.IsNullOrEmpty(showInfo.VisualEffects) || string.IsNullOrEmpty(showInfo.Engagement))
            {
                errorMessage = "All fields are required!";
                return Page();
            }

            try
            {
                string connectionString = "Data Source=MTG;Initial Catalog=master;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE shows " +
                                 "SET name=@name, story=@story, VisualEffects=@VisualEffects, Engagement=@Engagement " +
                                 "WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", showInfo.name);
                        command.Parameters.AddWithValue("@story", showInfo.story);
                        command.Parameters.AddWithValue("@VisualEffects", showInfo.VisualEffects);
                        command.Parameters.AddWithValue("@Engagement", showInfo.Engagement);
                        command.Parameters.AddWithValue("@id", showInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page();
            }

            return RedirectToPage("/Shows/Index");
        }
    }
}