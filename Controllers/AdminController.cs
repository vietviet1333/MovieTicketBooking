using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Dao;
using MovieTicketBooking.Others;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MovieTicketBooking.Controllers
{


    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult AdminLoginPage()
        {
            try
            {
                return View();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Errors");
            }
        }
        [HttpPost]
        public bool Login(string username, string password)
        {
            try
            {
                bool flagLogin = AdminDao.Instance().AdminLogin(username, password);
                if (flagLogin)
                {
                    Session["LoggedInAdminID"] = "admin";
                }
                return flagLogin;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public ActionResult Logout()
        {
            // Remove session variables
            try
            {
                Session.Remove("LoggedInAdminID");

                return RedirectToAction("AdminLoginPage", "Admin");
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Errors");
            }
        }
        [CheckAdminLogin]
        public ActionResult Dashboard()
        {
            try
            {
                return View();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Erorrs");
            }
        }
        [CheckAdminLogin]
        public ActionResult Addnewmovie()
        {
            try
            {
                return View();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Errors");
            }
        }
        [HttpPost]
        public bool Insertmovie(string namemovi, string directo, string languag, DateTime release_dat, int duratio, string genr, string descriptio, string urlimag, string cas)
        {
            bool flagInsert = true;
            try
            {
                bool insert = MovieDao.Instance().Addnewmovie(namemovi, directo, languag, release_dat, duratio, genr, descriptio, urlimag, cas);
                if(insert == true)
                {
                    flagInsert = true;
                }
                else
                {
                    flagInsert = false;
                }
               
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                flagInsert= false;
            }
            return flagInsert;
        }
        [CheckAdminLogin]
        public ActionResult Listmovie()

        {
            var listMovie = MovieDao.Instance().GetAllMovies();
            return View(listMovie);
        }
        [CheckAdminLogin]
        public ActionResult UserManagement()
        {
            try
            {
                var result = UserDao.Instance().GetAllUser();
                return View(result);
            }
            catch
            {
                return View("Errors");
            }
        }
        [CheckAdminLogin]
        public ActionResult UserBooking(int id_user)
        {
            try
            {
                var listBooking = BookingDao.Instance().GetBookingOfUser(id_user);

                return View(listBooking);

            }
            catch
            {
                return View("Errors");
            }
        }
        [HttpPost]
        public JsonResult RevenueByYear(int year)
        {
            try
            {
                var revenue = BookingDao.Instance().ChartByYear(year);
                return Json(revenue, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        public ActionResult CreateExcelAndPdf()
        {
            try
            {
                var excelPdfPrinter = new ExcelPdfPrinter();

                // Define the folder path where Excel and PDF files will be stored (relative to the application directory)
                string folderName = "App_Data/MovieExcelPdf";
                string folderPath = Server.MapPath("~/" + folderName);

                // Ensure the folder exists, create it if necessary
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Define the file names for Excel and PDF files
                string excelFileName = "YourExcelFile.xlsx";
                string pdfFileName = "YourPdfFile.pdf";
                string excelFilePath = Path.Combine(folderPath, excelFileName);
                string pdfFilePath = Path.Combine(folderPath, pdfFileName);
                excelPdfPrinter.CreateExcelFile(excelFilePath);
                excelPdfPrinter.PrintPdfFromExcel(excelFilePath, pdfFilePath);
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = "An error occurred while creating Excel and PDF files: " + ex.Message;
                return View("Errors");
            }
        }
        public ActionResult ForgotPassword()
        {
            try
            {
                string mail = "vietviet1333@gmail.com";
                Random random = new Random();
                int randomNumber = random.Next(100000, 999999);
                AdminDao.Instance().InsertCode(randomNumber, mail);
                Sendmail.Instance().Sendmaill(mail, randomNumber);
                return Redirect("/Admin/AdminLoginPage");
            }
            catch
            {
                return Redirect("/Admin/AdminLoginPage");
            }
        }
        [HttpPost]
        public ActionResult Changepass()
        {
            try
            {
                string email = Request.Form["email"];
                int verificationCode = int.Parse(Request.Form["verificationCode"]);
                string newpass = Request.Form["newPassword"];
                bool c = AdminDao.Instance().AdminChangePassword(email, verificationCode, newpass);
                if (c == true)
                {
                    TempData["changepass"] = "Change Password Success";
                    return Redirect("/Admin/AdminLoginPage");
                }
                else
                {
                    TempData["changepass"] = "Change Password Failed";
                    return Redirect("/Admin/AdminLoginPage");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["changepass"] = "Change Password Failed";
                return Redirect("/Admin/AdminLoginPage");
            }

        }
    }
    
}