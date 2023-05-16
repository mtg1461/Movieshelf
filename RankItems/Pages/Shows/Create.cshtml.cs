using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace RankItems.Pages.Shows
{
    public class CreateModel : PageModel
    {
        public ShowInfo showInfo = new ShowInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            showInfo.name = Request.Form["name"];
            showInfo.story = Request.Form["story"];
            showInfo.VisualEffects = Request.Form["VisualEffects"];
            showInfo.Engagement = Request.Form["Engagement"];

            if (showInfo.name.Length == 0 || showInfo.story.Length == 0 || showInfo.VisualEffects.Length == 0 || showInfo.Engagement.Length == 0)
            {
                errorMessage = "All fields are required!";
                return;
            }

            try
            {
                string connectionString = "Data Source=MTG;Initial Catalog=master;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO shows " +
                                 "(name, story, VisualEffects, Engagement) VALUES" +
                                 "(@name, @story, @VisualEffects, @Engagement);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", showInfo.name);
                        command.Parameters.AddWithValue("@story", showInfo.story);
                        command.Parameters.AddWithValue("@VisualEffects", showInfo.VisualEffects);
                        command.Parameters.AddWithValue("@Engagement", showInfo.Engagement);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            successMessage = "Data added successfully!";
        }
    }
}
