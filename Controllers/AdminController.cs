﻿using MovieTicketBooking.Bcrypt;
using MovieTicketBooking.Dao;
using System;
using System.Collections.Generic;
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
            return View();
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
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);  
                return false;
            }
        }
        public ActionResult Logout()
        {
            // Remove session variables
            Session.Remove("LoggedInAdminID");

            return RedirectToAction("AdminLoginPage", "Admin");
        }
        [CheckAdminLogin]
        public ActionResult Dashboard()
        {
            return View();
        }
        [CheckAdminLogin]
        public ActionResult Addnewmovie()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insertmovie(string namemovi, string directo, string languag, DateTime release_dat, int duratio, string genr, string descriptio, string urlimag, string cas)
        {

            MovieDao.Instance().Addnewmovie(namemovi, directo, languag, release_dat, duratio, genr, descriptio, urlimag, cas);

            return Redirect("/Admin/Dashboard");

        }
        [CheckAdminLogin]
        public ActionResult Listmovie()

        {
            var listMovie = MovieDao.Instance().GetAllMovies();
            return View(listMovie);
        }
    }
}