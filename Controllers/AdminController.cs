using MySql.Data.MySqlClient;
using Ngo_One.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ngo_One.Controllers
{
    public class AdminController : Controller
    {
        public string connection = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        ViewData data = new ViewData();
        InsertData in_data = new InsertData();
        UpdateData up_data = new UpdateData();
        DeleteData dl_data = new DeleteData();

        //Indian Time Standard Datetime
        public static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        //Sign_in
        public ActionResult Sign_In()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Sign_In(string username, string password)
        {
            try
            {
                // Check the database for the provided username and password
                List<SuperAdminModel> adminData = data.admin_data(username, password);

                if (adminData.Count > 0)
                {
                    SuperAdminModel user = adminData[0];

                    if (username == user.Admin_Username && password == user.Admin_Password)
                    {
                        // Store user data in session
                        Session["UserId"] = user.Admin_Id;
                        Session["Username"] = user.Admin_Username;
                        Session["UserType"] = user.Admin_Type;

                        return RedirectToAction("Index", "Admin"); // Redirect to the home page or another desired page
                    }
                    else
                    {
                        // Show an alert for invalid user type
                        ViewBag.ErrorMessage = "Invalid user type. Only admin users are allowed.";
                    }
                }
                else
                {
                    // Show an alert for invalid username or password
                    ViewBag.ErrorMessage = "Invalid username or password";
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                // Log additional information for debugging
                // Logger.LogError("An error occurred in Sign_In action", ex);
            }

            return View();
        }

        //Sign Out

        public ActionResult SignOut()
        {
            try
            {
                // Clear user data from session
                Session.Clear();
                Session.Abandon();

                // Redirect to the login page or another desired page
                return RedirectToAction("Sign_In");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                ViewBag.ErrorMessage = $"An error occurred during sign-out: {ex.Message}";
                // Log additional information for debugging
                // Logger.LogError("An error occurred in SignOut action", ex);
            }

            // If an error occurs, redirect to the login page with an error message
            return RedirectToAction("Sign_In", new { errorMessage = "Error during sign-out." });
        }


        //Manage_Banner

        public ActionResult Manage_Banner()
        {
            return View();
        }

        public ActionResult Manage_Banner_Disable(string SId = null)
        {
            try
            {
                up_data.Slider_Status_Update(SId, "0");
                TempData["Success"] = "SUCCESS! RECORD DISABLED SUCCESSFULLY.";
            }
            catch (Exception ex)
            {
                TempData["Failed"] = ex;
            }
            return RedirectToAction("Manage_Banner");
        }

        public ActionResult Manage_Banner_Enable(string SId = null)
        {
            try
            {
                up_data.Slider_Status_Update(SId, "1");
                TempData["Success"] = "SUCCESS! RECORD DISABLED SUCCESSFULLY.";
            }
            catch (Exception ex)
            {
                TempData["Failed"] = ex;
            }
            return RedirectToAction("Manage_Banner");
        }

        [HttpPost]
        public ActionResult Manage_Banner(AdminModel model, HttpPostedFileBase SImage)
        {
            try
            {
                // Check if the file is uploaded
                if (SImage != null && SImage.ContentLength > 0)
                {
                    // Get the file extension
                    string fileExtension = Path.GetExtension(SImage.FileName).ToLower();

                    // Define the allowed file extensions
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                    // Validate file extension
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        // Generate a unique filename based on the current timestamp
                        string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;

                        // Save the main file to a specific path
                        string mainFilePath = Path.Combine(Server.MapPath("~/Content/Images/BannerImages"), uniqueFileName);
                        SImage.SaveAs(mainFilePath);

                        // Insert data into the database
                        in_data.Insert_Slider(model.SName, uniqueFileName);

                        TempData["SuccessMessage"] = "Data inserted successfully.";
                        return RedirectToAction("Manage_Banner"); // Redirect to a success page or another action
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid file extension. Allowed extensions: .jpg, .jpeg, .png.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload a file.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            // If the model is not valid or file upload fails, return to the form with validation errors
            return View("Manage_Banner");
        }

        [HttpPost]
        public ActionResult Manage_Banner_Update(AdminModel model, HttpPostedFileBase SImage)
        {
            try
            {
                // Check if a new file is provided
                if (SImage != null && SImage.ContentLength > 0)
                {
                    // Extract file extension
                    string fileExtension = Path.GetExtension(SImage.FileName).ToLower();

                    // Validate file extension
                    if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg" ||
                        fileExtension == ".pdf" || fileExtension == ".docx")
                    {
                        // Generate a unique file name using the original file name + timestamp
                        string originalFileName = Path.GetFileNameWithoutExtension(SImage.FileName);
                        string uniqueFileName = $"{originalFileName}_{DateTime.Now.Ticks}{fileExtension}";

                        // Specify the directory to save the file
                        string uploadPath = Server.MapPath("~/Content/Images/BannerImages"); // Change this path as needed

                        // Combine the directory and file name
                        string filePath = Path.Combine(uploadPath, uniqueFileName);

                        // Save the file
                        SImage.SaveAs(filePath);

                        // Update data in the database with the new file
                        up_data.Update_Slider(model.SId, model.SName, uniqueFileName);

                        TempData["SuccessMessage"] = "Data updated successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid file format. Only png, jpg, jpeg, pdf, docx files are allowed.";
                    }
                }
                else
                {
                    // No new file provided, update other information without changing the file
                    up_data.Update_Slider(model.SId, model.SName, model.SImage);

                    TempData["SuccessMessage"] = "Data updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            // Redirect back to the form
            return RedirectToAction("Manage_Banner");
        }

        //Manage Ngo Staff

        public ActionResult Manage_Ngo_Staff()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult Manage_Ngo_Staff(AdminModel model, HttpPostedFileBase Staff_Image)
        {
            try
            {
                // Check if the file is uploaded
                if (Staff_Image != null && Staff_Image.ContentLength > 0)
                {
                    // Get the file extension
                    string fileExtension = Path.GetExtension(Staff_Image.FileName).ToLower();

                    // Define the allowed file extensions
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                    // Validate file extension
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        // Generate a unique filename based on the current timestamp
                        string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;

                        // Save the main file to a specific path
                        string mainFilePath = Path.Combine(Server.MapPath("~/Content/Images/StaffImages"), uniqueFileName);
                        Staff_Image.SaveAs(mainFilePath);

                        // Insert data into the database
                        in_data.Insert_Staff(model.StaffName,model.StaffDesignation, uniqueFileName, model.SeniorityLevel);

                        TempData["SuccessMessage"] = "Data inserted successfully.";
                        return RedirectToAction("Manage_Ngo_Staff"); // Redirect to a success page or another action
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid file extension. Allowed extensions: .jpg, .jpeg, .png.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload a file.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            // If the model is not valid or file upload fails, return to the form with validation errors
            return View("Manage_Ngo_Staff");
        }

        [HttpPost]
        public ActionResult Manage_Ngo_Staff_Update(AdminModel model, HttpPostedFileBase Staff_Image)
        {
            try
            {
                // Check if a new file is provided
                if (Staff_Image != null && Staff_Image.ContentLength > 0)
                {
                    // Extract file extension
                    string fileExtension = Path.GetExtension(Staff_Image.FileName).ToLower();

                    // Validate file extension
                    if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg" ||
                        fileExtension == ".pdf" || fileExtension == ".docx")
                    {
                        // Generate a unique file name using the original file name + timestamp
                        string originalFileName = Path.GetFileNameWithoutExtension(Staff_Image.FileName);
                        string uniqueFileName = $"{originalFileName}_{DateTime.Now.Ticks}{fileExtension}";

                        // Specify the directory to save the file
                        string uploadPath = Server.MapPath("~/Content/Images/StaffImages"); // Change this path as needed

                        // Combine the directory and file name
                        string filePath = Path.Combine(uploadPath, uniqueFileName);

                        // Save the file
                        Staff_Image.SaveAs(filePath);

                        // Update data in the database with the new file
                        up_data.Update_Staff(model.StaffId, model.StaffName, model.StaffDesignation, uniqueFileName, model.SeniorityLevel);

                        TempData["SuccessMessage"] = "Data updated successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid file format. Only png, jpg, jpeg, pdf, docx files are allowed.";
                    }
                }
                else
                {
                    // No new file provided, update other information without changing the file
                    up_data.Update_Staff(model.StaffId, model.StaffName, model.StaffDesignation, model.Staff_Image, model.SeniorityLevel);

                    TempData["SuccessMessage"] = "Data updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            // Redirect back to the form
            return RedirectToAction("Manage_Ngo_Staff");
        }

        public ActionResult Delete_Ngo_Staff(string StaffId)
        {
            try
            {
                dl_data.Delete_Staff(StaffId);
                TempData["Success"] = "SUCCESS! RECORD DELETED SUCCESSFULLY.";
            }
            catch (Exception ex)
            {
                TempData["Failed"] = ex.Message; // Use ex.Message to get the error message
            }
            return RedirectToAction("Manage_Ngo_Staff");
        }

        //Manage Ngo Videos

        public ActionResult Manage_Ngo_Videos_Category()
        { 
            return View(); 
        }

        [HttpPost]
        public ActionResult Manage_Ngo_Videos_Category(AdminModel model, HttpPostedFileBase VCategoryImage)
        {
            try
            {
                // Check if the file is uploaded
                if (VCategoryImage != null && VCategoryImage.ContentLength > 0)
                {
                    // Get the file extension
                    string fileExtension = Path.GetExtension(VCategoryImage.FileName).ToLower();

                    // Define the allowed file extensions
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                    // Validate file extension
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        // Generate a unique filename based on the current timestamp
                        string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;

                        // Save the main file to a specific path
                        string mainFilePath = Path.Combine(Server.MapPath("~/Content/Images/VideosCategoryImages"), uniqueFileName);
                        VCategoryImage.SaveAs(mainFilePath);

                        // Insert data into the database
                        in_data.Insert_Video_Category(model.VCategoryName, uniqueFileName);

                        TempData["SuccessMessage"] = "Data inserted successfully.";
                        return RedirectToAction("Manage_Ngo_Videos_Category"); // Redirect to a success page or another action
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid file extension. Allowed extensions: .jpg, .jpeg, .png.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload a file.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            // If the model is not valid or file upload fails, return to the form with validation errors
            return View("Manage_Ngo_Videos_Category");
        }

        [HttpPost]
        public ActionResult Update_Ngo_Videos_Category(AdminModel model, HttpPostedFileBase VCategoryImage)
        {
            try
            {
                // Check if a new file is provided
                if (VCategoryImage != null && VCategoryImage.ContentLength > 0)
                {
                    // Extract file extension
                    string fileExtension = Path.GetExtension(VCategoryImage.FileName).ToLower();

                    // Validate file extension
                    if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg" ||
                        fileExtension == ".pdf" || fileExtension == ".docx")
                    {
                        // Generate a unique file name using the original file name + timestamp
                        string originalFileName = Path.GetFileNameWithoutExtension(VCategoryImage.FileName);
                        string uniqueFileName = $"{originalFileName}_{DateTime.Now.Ticks}{fileExtension}";

                        // Specify the directory to save the file
                        string uploadPath = Server.MapPath("~/Content/Images/VideosCategoryImages"); // Change this path as needed

                        // Combine the directory and file name
                        string filePath = Path.Combine(uploadPath, uniqueFileName);

                        // Save the file
                        VCategoryImage.SaveAs(filePath);

                        // Update data in the database with the new file
                        up_data.Update_Video_Category(model.VCategoryId, model.VCategoryName, uniqueFileName);

                        TempData["SuccessMessage"] = "Data updated successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid file format. Only png, jpg, jpeg, pdf, docx files are allowed.";
                    }
                }
                else
                {
                    // No new file provided, update other information without changing the file
                    up_data.Update_Video_Category(model.VCategoryId, model.VCategoryName, model.VCategoryImage);

                    TempData["SuccessMessage"] = "Data updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            // Redirect back to the form
            return RedirectToAction("Manage_Ngo_Videos_Category");
        }

        public ActionResult Delete_Ngo_Videos_Category(string VideoCategoryId)
        {
            try
            {
                dl_data.Delete_Video_Category(VideoCategoryId);
                TempData["Success"] = "SUCCESS! RECORD DELETED SUCCESSFULLY.";
            }
            catch (Exception ex)
            {
                TempData["Failed"] = ex.Message; // Use ex.Message to get the error message
            }
            return RedirectToAction("Manage_Ngo_Videos_Category");
        }

        // Manage Videos

        public ActionResult Manage_Ngo_Videos()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Manage_Ngo_Videos(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Use the GetEmbeddedYouTubeUrl method to get the embedded link
                    string embeddedLink = GetEmbeddedYouTubeUrl(model.VLink);

                    // Now store the embedded link in the database
                    in_data.Insert_Video(model.VName, model.VDescription, embeddedLink);

                    TempData["Success"] = "SUCCESS! RECORD INSERTED SUCCESSFULLY.";

                    return RedirectToAction("Manage_Ngo_Videos");
                }
                catch (ArgumentException ex)
                {
                    TempData["Failed"] = "Error inserting video. " + ex.Message;
                    // Handle the case where the video ID couldn't be extracted
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Error inserting video. " + ex.Message;
                    // Handle other exceptions
                }
            }

            return View("Manage_Ngo_Videos");
        }

        [HttpPost]
        public ActionResult Manage_Ngo_Videos_Update(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Use the GetEmbeddedYouTubeUrl method to get the embedded link
                    string embeddedLink = GetEmbeddedYouTubeUrl(model.VLink);

                    // Now store the embedded link in the database
                    up_data.Update_Video(model.VId,model.VName, model.VDescription, embeddedLink);

                    TempData["Success"] = "SUCCESS! RECORD INSERTED SUCCESSFULLY.";

                    return RedirectToAction("Manage_Ngo_Videos");
                }
                catch (ArgumentException ex)
                {
                    TempData["Failed"] = "Error inserting video. " + ex.Message;
                    // Handle the case where the video ID couldn't be extracted
                }
                catch (Exception ex)
                {
                    TempData["Failed"] = "Error inserting video. " + ex.Message;
                    // Handle other exceptions
                }
            }

            return View("Manage_Ngo_Videos");
        }

        public ActionResult Delete_Ngo_Videos(string VideoId)
        {
            try
            {
                dl_data.Delete_Video(VideoId);
                TempData["Success"] = "SUCCESS! RECORD DELETED SUCCESSFULLY.";
            }
            catch (Exception ex)
            {
                TempData["Failed"] = ex.Message; // Use ex.Message to get the error message
            }
            return RedirectToAction("Manage_Ngo_Videos");
        }

        // Manage Gallery Category 

        public ActionResult Manage_Gallery_Category()
        { 
            return View(); 
        }

        [HttpPost]
        public ActionResult Manage_Gallery_Category(AdminModel model, HttpPostedFileBase Ngo_Category_Image)
        {
            try
            {
                // Check if the file is uploaded
                if (Ngo_Category_Image != null && Ngo_Category_Image.ContentLength > 0)
                {
                    // Get the file extension
                    string fileExtension = Path.GetExtension(Ngo_Category_Image.FileName).ToLower();

                    // Define the allowed file extensions
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                    // Validate file extension
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        // Generate a unique filename based on the current timestamp
                        string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;

                        // Save the main file to a specific path
                        string mainFilePath = Path.Combine(Server.MapPath("~/Content/Images/CategoryImages"), uniqueFileName);
                        Ngo_Category_Image.SaveAs(mainFilePath);

                        // Insert data into the database
                        in_data.Insert_Gallery_Category(model.Ngo_Category_Name, uniqueFileName);

                        TempData["SuccessMessage"] = "Data inserted successfully.";
                        return RedirectToAction("Manage_Gallery_Category"); // Redirect to a success page or another action
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid file extension. Allowed extensions: .jpg, .jpeg, .png.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload a file.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            // If the model is not valid or file upload fails, return to the form with validation errors
            return View("Manage_Gallery_Category");
        }

        [HttpPost]
        public ActionResult Update_Manage_Gallery_Category(AdminModel model, HttpPostedFileBase Ngo_Category_Image)
        {
            try
            {
                // Check if a new file is provided
                if (Ngo_Category_Image != null && Ngo_Category_Image.ContentLength > 0)
                {
                    // Extract file extension
                    string fileExtension = Path.GetExtension(Ngo_Category_Image.FileName).ToLower();

                    // Validate file extension
                    if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg" ||
                        fileExtension == ".pdf" || fileExtension == ".docx")
                    {
                        // Generate a unique file name using the original file name + timestamp
                        string originalFileName = Path.GetFileNameWithoutExtension(Ngo_Category_Image.FileName);
                        string uniqueFileName = $"{originalFileName}_{DateTime.Now.Ticks}{fileExtension}";

                        // Specify the directory to save the file
                        string uploadPath = Server.MapPath("~/Content/Images/CategoryImages"); // Change this path as needed

                        // Combine the directory and file name
                        string filePath = Path.Combine(uploadPath, uniqueFileName);

                        // Save the file
                        Ngo_Category_Image.SaveAs(filePath);

                        // Update data in the database with the new file
                        up_data.Update_Gallery_Category(model.Ngo_Category_Id, model.Ngo_Category_Name, uniqueFileName);

                        TempData["SuccessMessage"] = "Data updated successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid file format. Only png, jpg, jpeg, pdf, docx files are allowed.";
                    }
                }
                else
                {
                    // No new file provided, update other information without changing the file
                    up_data.Update_Gallery_Category(model.Ngo_Category_Id, model.Ngo_Category_Name, model.Ngo_Category_Image);

                    TempData["SuccessMessage"] = "Data updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            // Redirect back to the form
            return RedirectToAction("Manage_Gallery_Category");
        }

        public ActionResult Delete_Manage_Gallery_Category(string GCategoryId)
        {
            try
            {
                dl_data.Delete_Gallery_Category(GCategoryId);
                TempData["Success"] = "SUCCESS! RECORD DELETED SUCCESSFULLY.";
            }
            catch (Exception ex)
            {
                TempData["Failed"] = ex.Message; // Use ex.Message to get the error message
            }
            return RedirectToAction("Manage_Gallery_Category");
        }

        // Manage Gallery Images

        public ActionResult Manage_Gallery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Manage_Gallery(AdminModel model, HttpPostedFileBase[] Gallery_Image)
        {
            try
            {
                // Check if any file is uploaded
                if (Gallery_Image != null && Gallery_Image.Any(file => file != null && file.ContentLength > 0))
                {
                    foreach (var file in Gallery_Image.Where(file => file != null && file.ContentLength > 0))
                    {
                        // Get the file extension
                        string fileExtension = Path.GetExtension(file.FileName).ToLower();

                        // Define the allowed file extensions
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                        // Validate file extension
                        if (allowedExtensions.Contains(fileExtension))
                        {
                            // Generate a unique filename based on the current timestamp
                            string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;

                            // Save the file to a specific path
                            string filePath = Path.Combine(Server.MapPath("~/Content/Images/GalleryImages"), uniqueFileName);
                            file.SaveAs(filePath);

                            // Insert data into the database
                            in_data.Insert_Gallery_Image(model.Image_Name, uniqueFileName, model.Gallery_Image_Category);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Invalid file extension. Allowed extensions: .jpg, .jpeg, .png.";
                            return View("Manage_Gallery");
                        }
                    }

                    TempData["SuccessMessage"] = "Data inserted successfully.";
                    return RedirectToAction("Manage_Gallery"); // Redirect to a success page or another action
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload at least one file.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            // If the model is not valid or file upload fails, return to the form with validation errors
            return View("Manage_Gallery", model);
        }

        [HttpPost]
        public ActionResult Manage_Gallery_Update(AdminModel model, HttpPostedFileBase Gallery_Image)
        {
            try
            {
                // Check if a new file is uploaded
                if (Gallery_Image != null && Gallery_Image.ContentLength > 0)
                {
                    // Get the file extension
                    string fileExtension = Path.GetExtension(Gallery_Image.FileName)?.ToLower();

                    // Define the allowed file extensions
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                    // Validate file extension
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        // Generate a unique filename based on the current timestamp
                        string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileExtension;

                        // Save the main file to a specific path
                        string mainFilePath = Path.Combine(Server.MapPath("~/Content/Images/GalleryImages"), uniqueFileName);
                        Gallery_Image.SaveAs(mainFilePath);

                        // Update data in the database, including the new file name
                        up_data.Update_Gallery_Image(model.Image_Id, model.Image_Name, uniqueFileName, model.Gallery_Image_Category);

                        TempData["SuccessMessage"] = "Data updated successfully.";
                        return RedirectToAction("Manage_Gallery"); // Redirect to a success page or another action
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid file extension. Allowed extensions: .jpg, .jpeg, .png.";
                    }
                }
                else
                {
                    // If no new file is uploaded, update data in the database without changing the image
                    up_data.Update_Gallery_Image(model.Image_Id, model.Image_Name, model.Gallery_Image, model.Gallery_Image_Category);

                    TempData["SuccessMessage"] = "Data updated successfully.";
                    return RedirectToAction("Manage_Gallery"); // Redirect to a success page or another action
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            // If the model is not valid or file upload fails, return to the form with validation errors
            return View("Manage_Gallery", model);
        }

        public ActionResult Delete_Gallery_Image(string Pid)
        {
            try
            {
                dl_data.Delete_Gallery_Image(Pid);
                TempData["Success"] = "SUCCESS! RECORD DELETED SUCCESSFULLY.";
            }
            catch (Exception ex)
            {
                TempData["Failed"] = ex.Message; // Use ex.Message to get the error message
            }
            return RedirectToAction("Manage_Gallery");
        }

        public ActionResult Manage_Gallery_Image_Disable(string Image_Id = null)
        {
            try
            {
                dl_data.Disable_Image(Image_Id, "0");
                TempData["Success"] = "SUCCESS! RECORD DISABLED SUCCESSFULLY.";
            }
            catch (Exception ex)
            {
                TempData["Failed"] = ex;
            }
            return RedirectToAction("Manage_Gallery");
        }

        public ActionResult Manage_Gallery_Image_Enable(string Image_Id = null)
        {
            try
            {
                dl_data.Disable_Image(Image_Id, "1");
                TempData["Success"] = "SUCCESS! RECORD DISABLED SUCCESSFULLY.";
            }
            catch (Exception ex)
            {
                TempData["Failed"] = ex;
            }
            return RedirectToAction("Manage_Gallery");
        }


        public ActionResult Manage_Payment_Details()
        {
            return View();
        }

        // FOR EMBEDING THE YOUTUBE VIDEO LINKS You CAN USE THIS PUBLIC METHOD
        public static string GetEmbeddedYouTubeUrl(string youtubeLink)
        {
            // Check if the link is already an embedded link
            if (IsEmbeddedLink(youtubeLink))
            {
                return youtubeLink;
            }

            // Extract video ID from the YouTube link
            string videoId = ExtractYouTubeVideoId(youtubeLink);

            // Create the embedded YouTube URL
            string embeddedUrl = $"https://www.youtube.com/embed/{videoId}";

            return embeddedUrl;
        }

        private static string ExtractYouTubeVideoId(string youtubeLink)
        {
            // Extract video ID from the YouTube link
            Uri uri = new Uri(youtubeLink);

            // Check if the link is in the short format (youtu.be)
            if (uri.Host.Equals("youtu.be", StringComparison.OrdinalIgnoreCase))
            {
                // Extract video ID from short format links like https://youtu.be/VIDEO_ID
                string videoId = uri.Segments.Last();
                return videoId;
            }

            // For the standard format links like https://www.youtube.com/watch?v=VIDEO_ID
            string videoIdStandardFormat = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("v");

            if (string.IsNullOrEmpty(videoIdStandardFormat))
            {
                // Handle the case where the video ID couldn't be extracted
                throw new ArgumentException("Invalid YouTube link format. Unable to extract video ID.");
            }

            return videoIdStandardFormat;
        }

        private static bool IsEmbeddedLink(string youtubeLink)
        {
            // Check if the link contains the string "embed"
            return youtubeLink.Contains("embed");
        }
    }
}