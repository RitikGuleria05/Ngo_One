using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Ngo_One.Models
{
    public class DeleteData
    {
        //Indian Time Standard Datetime
        public static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);

        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        MySqlCommand cmd;

        // Total Vacancy
        public void Delete_Staff(string id)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_staff_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_Id", id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Video Disable - For Case In which we need enable/disable feature
        public void Disable_Video(string id)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_videos_disable", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_VId", id);
                    cmd.Parameters.AddWithValue("@S_Status", id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        //Video Delete

        public void Delete_Video(string id)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_video_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_VideoId", id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Total Vacancy
        public void Delete_Video_Category(string id)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_category_videos_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_CategoryId", id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Gallery Category
        public void Delete_Gallery_Category(string id)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_gallery_categories_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_id", id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Gallery image disable & delete
        public void Disable_Image(string id,string status )
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_gallery_disable", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_Id", id);
                    cmd.Parameters.AddWithValue("@S_Status", status);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Gallery Category
        public void Delete_Gallery_Image(string id)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_gallery_delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parameters
                    cmd.Parameters.AddWithValue("@p_id", id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}