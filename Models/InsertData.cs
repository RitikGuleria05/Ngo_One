using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Ngo_One.Models
{
    public class InsertData
    {
        //Indian Time Standard Datetime
        public static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);

        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        MySqlCommand cmd;

        // Ngo Slider
        public void Insert_Slider(string name = null, string description2 = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_banner_insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    cmd.Parameters.AddWithValue("@p_name", name);
                    cmd.Parameters.AddWithValue("@p_image", description2);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Ngo Staff
        public void Insert_Staff(string name = null, string description2 = null,string image = null, string rank = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_staff_insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    cmd.Parameters.AddWithValue("@p_Name", name);
                    cmd.Parameters.AddWithValue("@p_Designation", description2);
                    cmd.Parameters.AddWithValue("@p_Image", image);
                    cmd.Parameters.AddWithValue("@p_Seniority", rank);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Ngo Video
        public void Insert_Video(string name = null, string description2 = null, string link = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_videos_insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    cmd.Parameters.AddWithValue("@p_Title", name);
                    cmd.Parameters.AddWithValue("@p_Description", description2);
                    cmd.Parameters.AddWithValue("@p_YoutubeLink", link);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Ngo Video
        public void Insert_Video_Category(string name = null, string image = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_category_videos_insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    cmd.Parameters.AddWithValue("@p_CategoryName", name);
                    cmd.Parameters.AddWithValue("@p_Categoryimage", image);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Ngo Gallery Category
        public void Insert_Gallery_Category(string name = null,string image = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_gallery_categories_insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    cmd.Parameters.AddWithValue("@p_name", name);
                    cmd.Parameters.AddWithValue("@p_image", image);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        // Ngo gallery
        public void Insert_Gallery_Image(string name = null, string image = null, string category = null)
        {
            using (conn)
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("ngo_gallery_insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    cmd.Parameters.AddWithValue("@p_name", name);
                    cmd.Parameters.AddWithValue("@p_image", image);
                    cmd.Parameters.AddWithValue("@p_image_category", category);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        //Payment Insert

        public void Insert_User_And_Get_Id(string name, string mobile, string proof, string proof_number, string email, string amount, string address, string donationmode, string donationtype, string sectionmode, MySqlCommand cmd)
        {
            using (conn)
            {
                conn.Open();
                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "Insert_User_And_Get_Id";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters
                    cmd.Parameters.AddWithValue("@p_Full_Name", name);
                    cmd.Parameters.AddWithValue("@p_Mobile_Number", mobile);
                    cmd.Parameters.AddWithValue("@p_id_proof", proof);
                    cmd.Parameters.AddWithValue("@p_id_proof_number", proof_number);
                    cmd.Parameters.AddWithValue("@p_Email_Id", email);
                    cmd.Parameters.AddWithValue("@p_Amount", amount);
                    cmd.Parameters.AddWithValue("@p_Address", address);
                    cmd.Parameters.AddWithValue("@p_donation_mode", donationmode);
                    cmd.Parameters.AddWithValue("@p_donation_type", donationtype);
                    cmd.Parameters.AddWithValue("@p_section_mode", sectionmode);

                    // Output parameter for User_Id
                    var userIdParam = new MySqlParameter("@p_User_Id", MySqlDbType.Int32);
                    userIdParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(userIdParam);

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }
    }
}