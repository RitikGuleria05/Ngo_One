using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Ngo_One.Models
{
    public class UpdateData
    {
        //Indian Time Standard Datetime
        public static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);

        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        MySqlCommand cmd;

        // Slider
        public void Update_Slider(string id = null, string content2 = null, string image = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_banner_update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_id", id);
                    cmd.Parameters.AddWithValue("@p_name", content2);
                    cmd.Parameters.AddWithValue("@p_image", image);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public void Slider_Status_Update(string id = null, string status = null)
        {
            using (conn)
            {
                conn.Open();
                cmd = new MySqlCommand("ngo_banner_disable", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_Id", id);
                cmd.Parameters.AddWithValue("@S_Status", status);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        // Staff
        public void Update_Staff(string id = null, string content2 = null, string designation = null, string image = null, string seniority = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_staff_update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_Id", id);
                    cmd.Parameters.AddWithValue("@p_name", content2);
                    cmd.Parameters.AddWithValue("@p_Designation", designation);
                    cmd.Parameters.AddWithValue("@p_image",image);
                    cmd.Parameters.AddWithValue("@p_seniority", seniority);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Video
        public void Update_Video(string id = null, string titile = null, string description = null, string video = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_videos_update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_VideoId", id);
                    cmd.Parameters.AddWithValue("@p_Title", titile);
                    cmd.Parameters.AddWithValue("@p_Description", description);
                    cmd.Parameters.AddWithValue("@p_YoutubeLink", video);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Video Category
        public void Update_Video_Category(string id = null, string titile = null,string image = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_category_videos_update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_CategoryId", id);
                    cmd.Parameters.AddWithValue("@p_CategoryName", titile);
                    cmd.Parameters.AddWithValue("@p_Categoryimage", image);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Gallery Category
        public void Update_Gallery_Category(string id = null, string titile = null, string image = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_gallery_categories_update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_id", id);
                    cmd.Parameters.AddWithValue("@p_name", titile);
                    cmd.Parameters.AddWithValue("@p_image", image);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Gallery Image
        public void Update_Gallery_Image(string id = null, string titile = null, string image = null,string category = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_gallery_update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_id", id);
                    cmd.Parameters.AddWithValue("@p_name", titile);
                    cmd.Parameters.AddWithValue("@p_image", image);
                    cmd.Parameters.AddWithValue("@p_image_category", category);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}