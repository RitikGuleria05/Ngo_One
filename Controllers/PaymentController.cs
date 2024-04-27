using MySql.Data.MySqlClient;
using Ngo_One.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Ngo_One.Controllers
{
    public class PaymentController : Controller
    {
        public string connection = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        ViewData data = new ViewData();
        InsertData in_data = new InsertData();
        UpdateData up_data = new UpdateData();

        //Indian Time Standard Datetime
        public static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);
        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void Payment(string userId, decimal amount)
        {
            string strURL, strClientCode, strClientCodeEncoded;
            byte[] b;
            //string strResponse = "";

            string MerchantLogin = "192";
            string MerchantPass = "Test@123";
            string TransactionType = "NBFundTransfer";
            string ProductID = "NSE";
            string TransactionID = "123";
            string TransactionAmount = amount.ToString();
            string TransactionCurrency = "INR";
            string ClientCode = userId;
            string TransactionServiceCharge = "0";
            string TransactionDateTime = dateTime_Indian.ToString("yyyy-MM-dd hh:mm:ss");
            string CustomerAccountNo = "1234567890";
            //string ru = "https://localhost:44371/Home/Fee_Result";
            string ru = "https://localhost:44342/Payment/Fee_Result";
            try
            {
                b = Encoding.UTF8.GetBytes(ClientCode);
                strClientCode = Convert.ToBase64String(b);
                strClientCodeEncoded = HttpUtility.UrlEncode(strClientCode);
                strURL = "";

                //  string reqHashKey = requestkey;
                string reqHashKey = "KEY123657234";
                string signature = "";
                string strsignature = MerchantLogin + MerchantPass + TransactionType + ProductID + TransactionID + TransactionAmount + TransactionCurrency;
                byte[] bytes = Encoding.UTF8.GetBytes(reqHashKey);
                byte[] bt = new System.Security.Cryptography.HMACSHA512(bytes).ComputeHash(Encoding.UTF8.GetBytes(strsignature));
                // byte[] b = new HMACSHA512(bytes).ComputeHash(Encoding.UTF8.GetBytes(prodid));
                signature = byteToHexString(bt).ToLower();
                strURL = strURL.Replace("[signature]", signature);

                string passphrase = "8E41C78439831010F81F61C344B7BFC7";
                string salt = "8E41C78439831010F81F61C344B7BFC7";
                byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                int iterations = 65536;

                string plaintext = "login=" + MerchantLogin + "&pass=" + MerchantPass + "&ttype=" + TransactionType + "&prodid=" + ProductID + "&amt=" + TransactionAmount + "&txncurr=" + TransactionCurrency + "&txnscamt=" + TransactionServiceCharge + "&ru=" + ru + "&clientcode=" + strClientCodeEncoded + "&txnid=" + TransactionID + "&date=" + TransactionDateTime + "&udf9=" + "&custacc=" + CustomerAccountNo + "&signature=" + signature + "&mprod=<products><product><id>5</id><name>NSE</name><amount>3000.00</amount></product><product><id>7</id><name>THREE</name><amount>0</amount></product></products>";
                //string hashAlgorithm = "SHA1";
                string Encryptval = Encrypt(plaintext, passphrase, salt, iv, iterations);

                string Decryptval = decrypt(Encryptval, passphrase, salt, iv, iterations);

                strURL = "https://paynetzuat.atomtech.in/paynetz/epi/fts?login=" + MerchantLogin + "&encdata=" + Encryptval;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                System.Web.HttpContext.Current.Response.Redirect(strURL, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Fee_Result()
        {
            try
            {
                NameValueCollection nvc = Request.Form;

                string passphrase = "8E41C78439831010F81F61C344B7BFC7";
                string salt = "8E41C78439831010F81F61C344B7BFC7";
                byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                int iterations = 65536;
                //int keysize = 256;
                //string hashAlgorithm = "SHA1";
                string decrt = nvc["encdata"].ToString().Trim();

                string Decryptval = decrypt(decrt, passphrase, salt, iv, iterations);
                string Decryptval1 = "https://payment.atomtech.in/paynetz/epi/fts?" + Decryptval;

                var uri = new Uri(Decryptval1);
                var newQueryString = HttpUtility.ParseQueryString(uri.Query);
                string postingmmp_txn = newQueryString["mmp_txn"].ToString();
                string postingmer_txn = newQueryString["mer_txn"].ToString();
                string postinamount = newQueryString["amt"].ToString();
                string postingprod = newQueryString["prod"].ToString();
                string postingdate = newQueryString["date"].ToString();
                string postingbank_txn = newQueryString["bank_txn"].ToString();
                string postingf_code = newQueryString["f_code"].ToString();
                string postingbank_name = newQueryString["bank_name"].ToString();
                string clientcode = newQueryString["clientcode"].ToString();
                string signature = newQueryString["signature"].ToString();
                string postingdiscriminator = newQueryString["discriminator"].ToString();
                string[] udf9data = (newQueryString["udf9"].ToString()).Split(new char[] { '/' });


                string respHashKey = "KEYRESP123657234";
                string ressignature = "";
                string strsignature = postingmmp_txn + postingmer_txn + postingf_code + postingprod + postingdiscriminator + postinamount + postingbank_txn;
                //string strsignature = postingmmp_txn + postingmer_txn1 + postingf_code + postingprod + discriminator + postinamount + postingbank_txn;
                byte[] bytes = Encoding.UTF8.GetBytes(respHashKey);
                byte[] b = new System.Security.Cryptography.HMACSHA512(bytes).ComputeHash(Encoding.UTF8.GetBytes(strsignature));
                ressignature = byteToHexString(b).ToLower();

                if (postingf_code == "Ok" || postingf_code == "ok" || postingf_code == "OK" || postingf_code == "oK")
                {
                    using (MySqlConnection conn = new MySqlConnection(connection))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("payment_record_update", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mmp_txn", postingmmp_txn);
                        cmd.Parameters.AddWithValue("@mer_txn", postingmer_txn);
                        cmd.Parameters.AddWithValue("@amt", postinamount);
                        cmd.Parameters.AddWithValue("@prod", postingprod);
                        cmd.Parameters.AddWithValue("@dte", dateTime_Indian.ToString("yyyy-MM-dd hh:mm:ss"));
                        cmd.Parameters.AddWithValue("@bank_txn", postingbank_txn);
                        cmd.Parameters.AddWithValue("@f_code", postingf_code);
                        cmd.Parameters.AddWithValue("@bank_name", postingbank_name);
                        cmd.Parameters.AddWithValue("@id", clientcode);
                        cmd.Parameters.AddWithValue("@discriminator", postingdiscriminator);

                        //MySqlCommand cmd2 = new MySqlCommand("student_semester_wise_record_payment_sts", conn);
                        //cmd2.CommandType = CommandType.StoredProcedure;
                        //cmd2.Parameters.AddWithValue("@dte", postingdate);
                        //cmd2.Parameters.AddWithValue("@id", clientcode);
                        //cmd2.Parameters.AddWithValue("@crs", course);
                        //cmd2.Parameters.AddWithValue("@sem", semester);
                        //cmd2.Parameters.AddWithValue("@itype", installment);
                        cmd.ExecuteNonQuery();
                        //cmd2.ExecuteNonQuery();
                        conn.Close();
                    }
                    TempData["Payment-Result"] = "Success!Your Payment of Rs." + postinamount + " has been Successfully processed at " + postingdate;
                    TempData["Payment-Result-Img"] = "success.png";
                }
                else if (postingf_code == "F" || postingf_code == "f")
                {
                    using (MySqlConnection conn = new MySqlConnection(connection))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("payment_record_update", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mmp_txn", postingmmp_txn);
                        cmd.Parameters.AddWithValue("@mer_txn", postingmer_txn);
                        cmd.Parameters.AddWithValue("@amt", postinamount);
                        cmd.Parameters.AddWithValue("@prod", postingprod);
                        cmd.Parameters.AddWithValue("@dte", dateTime_Indian.ToString("yyyy-MM-dd hh:mm:ss"));
                        cmd.Parameters.AddWithValue("@bank_txn", postingbank_txn);
                        cmd.Parameters.AddWithValue("@f_code", postingf_code);
                        cmd.Parameters.AddWithValue("@bank_name", postingbank_name);
                        cmd.Parameters.AddWithValue("@id", clientcode);
                        cmd.Parameters.AddWithValue("@discriminator", postingdiscriminator);

                        //MySqlCommand cmd2 = new MySqlCommand("student_semester_wise_record_payment_sts", conn);
                        //cmd2.CommandType = CommandType.StoredProcedure;
                        //cmd2.Parameters.AddWithValue("@dte", postingdate);
                        //cmd2.Parameters.AddWithValue("@id", clientcode);
                        //cmd2.Parameters.AddWithValue("@crs", course);
                        //cmd2.Parameters.AddWithValue("@sem", semester);
                        //cmd2.Parameters.AddWithValue("@itype", installment);
                        cmd.ExecuteNonQuery();
                        //cmd2.ExecuteNonQuery();
                        conn.Close();
                    }
                    TempData["Payment-Result"] = "Failure!Your Payment of Rs." + postinamount + " is Failed.Please Try Again.";
                    TempData["Payment-Result-Img"] = "warning.png";
                }
                else if (postingf_code == "C" || postingf_code == "c")
                {
                    using (MySqlConnection conn = new MySqlConnection(connection))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("payment_record_update", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mmp_txn", postingmmp_txn);
                        cmd.Parameters.AddWithValue("@mer_txn", postingmer_txn);
                        cmd.Parameters.AddWithValue("@amt", postinamount);
                        cmd.Parameters.AddWithValue("@prod", postingprod);
                        cmd.Parameters.AddWithValue("@dte", dateTime_Indian.ToString("yyyy-MM-dd hh:mm:ss"));
                        cmd.Parameters.AddWithValue("@bank_txn", postingbank_txn);
                        cmd.Parameters.AddWithValue("@f_code", postingf_code);
                        cmd.Parameters.AddWithValue("@bank_name", postingbank_name);
                        cmd.Parameters.AddWithValue("@id", clientcode);
                        cmd.Parameters.AddWithValue("@discriminator", postingdiscriminator);

                        //MySqlCommand cmd2 = new MySqlCommand("student_semester_wise_record_payment_sts", conn);
                        //cmd2.CommandType = CommandType.StoredProcedure;
                        //cmd2.Parameters.AddWithValue("@dte", postingdate);
                        //cmd2.Parameters.AddWithValue("@id", clientcode);
                        //cmd2.Parameters.AddWithValue("@crs", course);
                        //cmd2.Parameters.AddWithValue("@sem", semester);
                        //cmd2.Parameters.AddWithValue("@itype", installment);
                        cmd.ExecuteNonQuery();
                        //cmd2.ExecuteNonQuery();
                        conn.Close();
                    }
                    TempData["Payment-Result"] = "Failure!Your Payment of Rs." + postinamount + " has been Cancelled";
                    TempData["Payment-Result-Img"] = "warning.png";
                }
                else
                {
                    TempData["Payment-Result"] = "Failure!There is some problem.Please Try Again Later";
                    TempData["Payment-Result-Img"] = "warning.png";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public static string byteToHexString(byte[] byData)
        {
            StringBuilder sb = new StringBuilder((byData.Length * 2));
            for (int i = 0; (i < byData.Length); i++)
            {
                int v = (byData[i] & 255);
                if ((v < 16))
                {
                    sb.Append('0');
                }

                sb.Append(v.ToString("X"));

            }

            return sb.ToString();
        }
        public String Encrypt(String plainText, String passphrase, String salt, Byte[] iv, int iterations)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            string data = ByteArrayToHexString(Encrypt(plainBytes, GetSymmetricAlgorithm(passphrase, salt, iv, iterations))).ToUpper();


            return data;
        }
        public String decrypt(String plainText, String passphrase, String salt, Byte[] iv, int iterations)
        {
            byte[] str = HexStringToByte(plainText);

            string data1 = Encoding.UTF8.GetString(decrypt(str, GetSymmetricAlgorithm(passphrase, salt, iv, iterations)));
            return data1;
        }
        public byte[] Encrypt(byte[] plainBytes, SymmetricAlgorithm sa)
        {
            return sa.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        }
        public byte[] decrypt(byte[] plainBytes, SymmetricAlgorithm sa)
        {
            return sa.CreateDecryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        public SymmetricAlgorithm GetSymmetricAlgorithm(String passphrase, String salt, Byte[] iv, int iterations)
        {
            var saltBytes = new byte[16];
            var ivBytes = new byte[16];
            Rfc2898DeriveBytes rfcdb = new System.Security.Cryptography.Rfc2898DeriveBytes(passphrase, Encoding.UTF8.GetBytes(salt), iterations);
            saltBytes = rfcdb.GetBytes(32);
            var tempBytes = iv;
            Array.Copy(tempBytes, ivBytes, Math.Min(ivBytes.Length, tempBytes.Length));
            var rij = new RijndaelManaged(); //SymmetricAlgorithm.Create();
            rij.Mode = CipherMode.CBC;
            rij.Padding = PaddingMode.PKCS7;
            rij.FeedbackSize = 128;
            rij.KeySize = 128;

            rij.BlockSize = 128;
            rij.Key = saltBytes;
            rij.IV = ivBytes;
            return rij;
        }
        protected static byte[] HexStringToByte(string hexString)
        {
            try
            {
                int bytesCount = (hexString.Length) / 2;
                byte[] bytes = new byte[bytesCount];
                for (int x = 0; x < bytesCount; ++x)
                {
                    bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
                }
                return bytes;
            }
            catch
            {
                throw;
            }
        }
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}