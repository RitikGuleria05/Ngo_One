using MySql.Data.MySqlClient;
using Ngo_One.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Ngo_One.Controllers
{
    public class HomeController : Controller
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

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Payment_Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Payment_Login(AdminModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Declare the MySqlCommand object to be used in this scope
                    MySqlCommand cmd = new MySqlCommand();

                    // Call the Insert_User_And_Get_Id method with the MySqlCommand object
                    in_data.Insert_User_And_Get_Id(model.Full_Name, model.Mobile_Number, model.Id_Proof, model.Id_Proof_Number, model.Email_Id, model.Amount, model.Address, model.Donation_Mode, model.Donation_Type, model.Section_Code, cmd);

                    // Retrieve the User_Id from the output parameter
                    int userId = Convert.ToInt32(cmd.Parameters["@p_User_Id"].Value);

                    // Redirect to the Payment controller with the obtained userId
                    return RedirectToAction("Payment", "Payment", new { userId, amount = model.Amount });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            // Return the view if there's an error or ModelState is not valid
            return View();
        }
    }
}