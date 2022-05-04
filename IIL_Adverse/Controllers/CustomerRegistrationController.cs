using EntityLayer;
using LogicalLayer;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utility;

namespace IIL_Adverse.Controllers
{
    public class CustomerRegistrationController : Controller
    {
        // GET: CustomerRegistration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveInfo(CustomerInfoEntity entity)
        {
            MailServer service = new MailServer();
            CustomerRegistrationService registrationService = new CustomerRegistrationService();
            try
            {
                int ISaved = 0;
                CustomerRegistrationEntity customer = new CustomerRegistrationEntity();
                customer.Age            = Convert.ToInt32(entity.age);
                customer.City           = entity.city;
                customer.CountryCode    = entity.country;
                customer.DOB            = Convert.ToDateTime(entity.dob);
                customer.DoctorRegNo    = entity.docRegNo;
                customer.Email          = entity.email;
                customer.GenderCode     = CodeMapper.GenderMapping(entity.gender);
                customer.Language       = CodeMapper.LanguageMapping(entity.language);
                customer.Name           = entity.name;
                customer.Occupation     = CodeMapper.OccupationMapping(entity.occupation);
                customer.Phone          = Convert.ToDecimal(entity.Phone);
                customer.PostalCode     = entity.postalCode;
                customer.State          = entity.state;
                customer.StreetAddress  = entity.stAddress;

                HttpBrowserCapabilitiesBase objBrwInfo = HttpContext.Request.Browser;
                string PasswordGen         = CodeGenerator.GenerateAlphaNumericCode();
                string IPAddress        = HttpContext.Request.UserHostAddress;
                string Browser          = objBrwInfo.Browser + "|" + objBrwInfo.Type;
                string BrowserVersion   = objBrwInfo.Version;
                string CustomerCode     = customer.CountryCode + registrationService.GetCustomerCode();
                string Password         = SecurityManager.Encrypt(PasswordGen);
                string EmailTemplate    = GetMailTemplate(CustomerCode, PasswordGen, customer.Name);

                customer.CustomerCode = CustomerCode;

                ISaved = registrationService.AddCustomer(customer, IPAddress, Browser, BrowserVersion, Password);
                if (ISaved > 0)
                {
                    service.SendMailUsingMailKit(customer.Email, null, null, ConfigurationManager.AppSettings["CustomerAckSubject"].ToString(), EmailTemplate);
                    return Json(new { Message = "Successfully Saved.", Status = 200, JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Details are not saved. Please retry again.", Status = 100, JsonRequestBehavior.AllowGet });
            }
            finally
            {
                registrationService = null;
                service = null;
            }
            return null;
        }

        [HttpPost]
        public JsonResult VerifyEmail(string Email)
        {
            CustomerRegistrationService service = new CustomerRegistrationService();
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    return Json(new { Message = "Invalid Email address.", Status = 200, JsonRequestBehavior.AllowGet });
                }

                int IsValid = service.GetEmailDuplicate(Email);
                if(IsValid == 1)
                {
                    return Json(new { Message = "Already registered with this Email Address. Please try with other Email Address", Status = 200, JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Message = "", Status = 200, JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Invalid Email address.", Status = 200, JsonRequestBehavior.AllowGet });
            }
        }

        public string GetMailTemplate(string custid, string password, string customerName)
        {
            try
            {
                string[] lines;
                var list = new List<string>();
                var fileStream = new FileStream(Server.MapPath(@"/Models/CustomerAutoCodePassword.txt"), FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                lines = list.ToArray();

                return string.Join(" ", lines).Replace("{customername}", customerName)
                    .Replace("{userid}", custid)
                    .Replace("{password}", password);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}