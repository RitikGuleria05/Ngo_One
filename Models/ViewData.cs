using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Ngo_One.Models
{
    public class ViewData
    {
        public string connection = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;

        //admin data

        public List<SuperAdminModel> admin_data(string username = null, string pass = null)
        {
            List<SuperAdminModel> bb = new List<SuperAdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("admin_login_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    SuperAdminModel bb1 = new SuperAdminModel();
                    bb1.Admin_Id = dr["admin_id"].ToString();
                    bb1.Admin_Username = dr["admin_username"].ToString();
                    bb1.Admin_Password = dr["admin_password"].ToString();
                    bb1.Admin_Type = dr["admin_type"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        // Slider View

        public List<AdminModel> Slider_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("ngo_banner_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.SId = dr["ngo_id"].ToString();
                    bb1.SName = dr["ngo_name"].ToString();
                    bb1.SImage = dr["ngo_image"].ToString();
                    bb1.CreatedAt3 = dr["ngo_created_at"].ToString();
                    bb1.UpdatedAt3 = dr["ngo_updated_at"].ToString();
                    bb1.SliderStatus = dr["ngo_status"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        // Staff View

        public List<AdminModel> Staff_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("ngo_staff_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.StaffId = dr["Staff_id"].ToString();
                    bb1.StaffName = dr["Staff_name"].ToString();
                    bb1.StaffDesignation = dr["Staff_designation"].ToString();
                    bb1.Staff_Image = dr["Staff_image"].ToString();
                    bb1.SeniorityLevel = dr["Staff_seniority"].ToString();
                    bb1.Staff_Status = dr["Staff_status"].ToString();
                    bb1.CreatedAt = dr["Staff_created_at"].ToString();
                    bb1.UpdatedAt = dr["Staff_updated_at"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        // Video View

        public List<AdminModel> Video_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("ngo_videos_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.VId = dr["video_id"].ToString();
                    bb1.VName = dr["title"].ToString();
                    bb1.VDescription = dr["DESCRIPTION1"].ToString();
                    bb1.VStatus = dr["status1"].ToString();
                    bb1.VLink = dr["youtube_link"].ToString();
                    bb1.VCreatedAt = dr["created_at"].ToString();
                    bb1.VUpdatedAt = dr["updated at"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        // Video Category View

        public List<AdminModel> Video_Category_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("ngo_category_videos_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.VCategoryId = dr["Category_id"].ToString();
                    bb1.VCategoryName = dr["Category_name"].ToString();
                    bb1.VCategoryImage = dr["Category_image"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }


        // Gallery Category View

        public List<AdminModel> Gallery_Category_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("ngo_gallery_categories_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.Ngo_Category_Id = dr["ngo_category_id"].ToString();
                    bb1.Ngo_Category_Name = dr["ngo_category_name"].ToString();
                    bb1.Ngo_Category_Image = dr["ngo_category_image"].ToString();
                    bb1.CreatedAt1 = dr["created_at"].ToString();
                    bb1.UpdatedAt1 = dr["updated_at"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        // Gallery Category View

        public List<AdminModel> Gallery_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("ngo_gallery_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.Image_Id = dr["gallery_id"].ToString();
                    bb1.Image_Name = dr["gallery_name"].ToString();
                    bb1.Gallery_Image = dr["gallery_image"].ToString();
                    bb1.Gallery_Image_Category = dr["gallery_image_category"].ToString();
                    bb1.SelectedCategory = dr["gallery_image_category"].ToString();
                    bb1.Image_Status = dr["gallery_status"].ToString();
                    bb1.Created_At = dr["created_at"].ToString(); 
                    bb1.Updated_At = dr["updated_at"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }


        public List<AdminModel> User_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("User_Details_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.User_Id = dr["User_Id"].ToString();
                    bb1.Full_Name = dr["Full_Name"].ToString();
                    bb1.Mobile_Number = dr["Mobile_Number"].ToString();
                    bb1.Id_Proof = dr["Id_Proof"].ToString();
                    bb1.Id_Proof_Number = dr["Id_Proof_Number"].ToString();
                    bb1.Email_Id = dr["Email_Id"].ToString();
                    bb1.Amount = dr["Amount"].ToString();
                    bb1.Address = dr["Address"].ToString();
                    bb1.Donation_Mode = dr["Donation_Mode1"].ToString();
                    bb1.Donation_Type = dr["Donation_type1"].ToString();
                    bb1.Section_Code = dr["Section_Mode"].ToString();
                    bb1.Status = dr["Status"].ToString();
                    bb1.Created_Date = dr["Created_Date"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        public List<AdminModel> Id_Proof_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("user_id_proof_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.Proof_Id = dr["id"].ToString();
                    bb1.Proof_Type = dr["id_proof_type"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        public List<AdminModel> Section_Code_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("section_code_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.Section_Id = dr["id"].ToString();
                    bb1.Section_Name = dr["section_code_type"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        public List<AdminModel> Donation_Type_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("Donation_type_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.Donation_ID = dr["id"].ToString();
                    bb1.Donation_Name = dr["donation_type"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        public List<AdminModel> Donation_Mode_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("donation_mode_view", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.Donation_ID = dr["id"].ToString();
                    bb1.Donation_Mode_Name = dr["Mode_Type"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }

        // Payment Data

        public List<AdminModel> Payment_data()
        {
            List<AdminModel> bb = new List<AdminModel>();
            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter("payment_record_read", conn);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                adp.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    AdminModel bb1 = new AdminModel();
                    bb1.PaymentRecordId = dr["payment_record_id"].ToString();
                    bb1.StudentId = dr["payment_record_student_id"].ToString();
                    bb1.Session = dr["payment_record_session"].ToString();
                    bb1.Payment_Amount = dr["payment_record_amount"].ToString();
                    bb1.AtomTransactionId = dr["payment_record_atom_txn_id"].ToString();
                    bb1.MerchantTransactionId = dr["payment_record_merchant_txn_id"].ToString();
                    bb1.TransactionAmount = dr["payment_record_txn_amount"].ToString();
                    bb1.Payment_ProductId = dr["payment_record_product_id"].ToString();
                    bb1.Date = dr["payment_record_date"].ToString();
                    bb1.BankTransactionId = dr["payment_record_bank_txn_id"].ToString();
                    bb1.BankStatus = dr["payment_record_bank_status"].ToString();
                    bb1.BankName = dr["payment_record_bank_name"].ToString();
                    bb1.Discriminator = dr["payment_record_discriminator"].ToString();
                    bb1.Payment_Status = dr["payment_record_status"].ToString();
                    bb1.PunchDate = dr["payment_record_punchdate"].ToString();

                    bb1.User_Id = dr["User_Id"].ToString();
                    bb1.Full_Name = dr["Full_Name"].ToString();
                    bb1.Mobile_Number = dr["Mobile_Number"].ToString();
                    bb1.Id_Proof = dr["Id_Proof"].ToString();
                    bb1.Id_Proof_Number = dr["Id_Proof_Number"].ToString();
                    bb1.Email_Id = dr["Email_Id"].ToString();
                    bb1.Amount = dr["Amount"].ToString();
                    bb1.Address = dr["Address"].ToString();
                    bb1.Donation_Mode = dr["Donation_Mode1"].ToString();
                    bb1.Donation_Type = dr["Donation_type1"].ToString();
                    bb1.Section_Code = dr["Section_Mode"].ToString();
                    bb1.Status = dr["Status"].ToString();
                    bb1.Created_Date = dr["Created_Date"].ToString();
                    bb.Add(bb1);
                }
                conn.Close();
                return bb;
            }
        }
    }
}