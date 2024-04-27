using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ngo_One.Models
{
    public class AdminModel
    {
        // Slider Variables

        public string SId { get; set; }
        public string SName { get; set; }
        public string SImage { get; set; }
        public string CreatedAt3 { get; set; }
        public string UpdatedAt3 { get; set; }

        public string SliderStatus { get; set; }

        //Ngo Staff Variables

        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string StaffDesignation { get; set; }
        public string Staff_Image { get; set; }
        public string SeniorityLevel { get; set; }
        public string Staff_Status { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        //Ngo video Variables

        public string VId { get; set; }
        public string VName { get; set; }
        public string VDescription { get; set; }
        public string VStatus { get; set; }
        public string VLink { get; set; }
        public string VCreatedAt { get; set; }
        public string VUpdatedAt { get; set; }

        // Ngo Video Category
        public string VCategoryId { get; set; }
        public string VCategoryName { get; set; }
        public string VCategoryImage { get; set; }

        // Ngo Image Categories variables
        public string Ngo_Category_Id { get; set; }
        public string Ngo_Category_Name { get; set; }
        public string Ngo_Category_Image { get; set; }
        public string CreatedAt1 { get; set; }
        public string UpdatedAt1 { get; set; }

        //Gallery Images variables

        public string Image_Id { get; set; }
        public string Image_Name { get; set; }
        public string Gallery_Image { get; set; }
        public string Gallery_Image_Category { get; set; }
        public string SelectedCategory {  get; set; }
        public string Image_Status { get; set; }
        public string Created_At { get; set; }
        public string Updated_At { get; set; }

        //Registration Record Variables
        public string Registration_Record_Id { get; set; }
        public string Registration_Record_RgId { get; set; }
        public string Registration_Record_Course { get; set; }
        public string Registration_Record_Semester { get; set; }
        public string Registration_Record_Installment_Type { get; set; }
        public string Registration_Record_Session { get; set; }
        public string Registration_Record_AccountNo { get; set; }
        public string Registration_Record_Amount { get; set; }
        public string Registration_Record_Atom_Txn_Id { get; set; }
        public string Registration_Record_Merchant_Txn_Id { get; set; }
        public string Registration_Record_Txn_Amount { get; set; }
        public string Registration_Record_Product_Id { get; set; }
        public string Registration_Record_Date { get; set; }
        public string Registration_Record_Bank_Txn_Id { get; set; }
        public string Registration_Record_Status { get; set; }
        public string Registration_Record_Bank_Name { get; set; }
        public string Registration_Record_Discriminator { get; set; }

        //Payment Gateway

        public string Doctor_Username { get; set; }
        public string Doctor_Email { get; set; }
        public string Doctor_Contact_No { get; set; }
        public string Payment_Record_Session { get; }
        public string Payment_Record_Amount { get; set; }

        //User Variables
        public string User_Id { get; set; }
        public string Full_Name { get; set; }
        public string Mobile_Number { get; set; }
        public string Id_Proof { get; set; }
        public string Id_Proof_Number { get; set; }
        public string Email_Id { get; set; }
        public string Amount { get; set; }
        public string Address { get; set; }
        public string Donation_Mode { get; set; }
        public string Donation_Type { get; set; }
        public string Section_Code { get; set; }
        public string Status { get; set; }
        public string Created_Date { get; set; }

        //Payment Variables
        public string PaymentRecordId { get; set; }
        public string StudentId { get; set; }
        public string Session { get; set; }
        public string Payment_Amount { get; set; }
        public string AtomTransactionId { get; set; }
        public string MerchantTransactionId { get; set; }
        public string TransactionAmount { get; set; }
        public string Payment_ProductId { get; set; }
        public string Date { get; set; }
        public string BankTransactionId { get; set; }
        public string BankStatus { get; set; }
        public string BankName { get; set; }
        public string Discriminator { get; set; }
        public string Payment_Status { get; set; }
        public string PunchDate { get; set; }

        // User Id Proof Variables

        public string Proof_Id { get; set; }
        public string Proof_Type { get; set; }

        //  Section Code Variable

        public string Section_Id { get; set; }
        public string Section_Name { get; set; }

        // Donation Type

        public string Donation_ID { get; set; }
        public string Donation_Name { get; set; }

        // Donation Mode

        public string Mode_ID { get; set; }
        public string Donation_Mode_Name { get; set; }
    }
}