using EntityLayer;
using Newtonsoft.Json;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utility;

namespace IIL_Adverse.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserEntity entity)
        {
            LoginService service = new LoginService();
            try
            {
                if(string.IsNullOrEmpty(entity.UserCode) || string.IsNullOrEmpty(entity.Password))
                {
                    return Json(new { Status =100, Message  = "Invalid Username and Password. Please try again."}, JsonRequestBehavior.AllowGet);
                }

                UserLoginDetail loginDetail = service.ValidateUser(entity.UserCode, SecurityManager.Encrypt(entity.Password));

                if (loginDetail != null && loginDetail.UserCode != "0")
                {
                    // Valid User
                    FormsAuthentication.SetAuthCookie(loginDetail.UserName, false);
                    var userIdentity = new ClaimsIdentity(loginDetail.UserName);

                    Session["UserCode"] = loginDetail.UserCode;
                    Session["UserName"] = loginDetail.UserName;
                    Session["Email"] = loginDetail.Email;
                    Session["LastLogin"] = loginDetail.LastLogin;
                    return Json(new { Status = 200, Message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Invalid User
                    return Json(new { Status = 100, Message = "Invalid Username and Password. Please try again." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = 100, Message = "Invalid Username and Password. Please try again." }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                service = null;
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult ChangePassword()
        {

            ChangePasswordModel change = new ChangePasswordModel();
            return View(change);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            LoginService service = new LoginService();

            if (ModelState.IsValid)
            {
                string user = service.ChangePassword(Session["UserCode"].ToString() , SecurityManager.Encrypt(model.NewPassword));

                if (user != "invalid")
                {
                    ViewBag.success = "New password updated successfully";
                    ViewBag.error = string.Empty;
                }
                else
                {
                    ViewBag.error = "Something invalid. Please try again.";
                    ViewBag.success = string.Empty;
                }
            }
            else
            {
                ViewBag.success = string.Empty;
                ViewBag.error = "Something invalid. Please try again.";
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            MailServer mailServer = new MailServer();
            LoginService service  = new LoginService();
            try
            {
                string resetCode    = Guid.NewGuid().ToString();
                var verifyUrl       = "/Account/ResetPassword/" + resetCode;
                var link            = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                if (string.IsNullOrEmpty(EmailID))
                {
                    ViewBag.Message = "Email cannot be empty.";
                    return View();
                }

                UserResetDetail detail = service.GetCustomerByEmail(EmailID);
                if (detail != null)
                {
                    int ISaved = service.SaveResetPasswordCode(resetCode, detail.UserCode);
                    string EmailTemplate = GetMailTemplate(detail.UserName, link);
                    mailServer.SendMailUsingMailKit(detail.Email, null, null, ConfigurationManager.AppSettings["CustomerResetPwdSubject"].ToString(), EmailTemplate);
                    ViewBag.Message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return View();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                service = null;
                mailServer = null;
            }
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            LoginService service = new LoginService();
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
            string user = service.GetValidResetCode(id);

            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            LoginService service = new LoginService();

            if (ModelState.IsValid)
            {
                UserResetDetail userResetDetail = service.GetCustomerByResetCode(model.ResetCode);
                string user = service.ResetPasswordFromMail(userResetDetail.UserCode, SecurityManager.Encrypt(model.NewPassword), model.ResetCode);

                if (user != "invalid")
                {
                    ViewBag.success = "New password updated successfully";
                    ViewBag.error = string.Empty;
                }
                else
                {
                    ViewBag.error = "Something invalid. Please try again.";
                    ViewBag.success = string.Empty;
                }
            }
            else
            {
                ViewBag.success = string.Empty;
                ViewBag.error = "Something invalid. Please try again.";
            }            
            return View(model);
        }

        public string GetMailTemplate(string customerName, string link)
        {
            try
            {
                string[] lines;
                var list = new List<string>();
                var fileStream = new FileStream(Server.MapPath(@"/Models/CustomerPasswordReset.txt"), FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                lines = list.ToArray();

                return string.Join(" ", lines).Replace("{0}", customerName)
                    .Replace("{1}", link);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}