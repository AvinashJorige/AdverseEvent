using EntityLayer;
using LogicalLayer;
using Newtonsoft.Json;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace IIL_Adverse.Controllers
{
    public class AdverseDetailController : Controller
    {

        // GET: AdverseDetail
        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult UploadMedicalReportFiles()
        {
            int fileSize = Convert.ToInt32(ConfigurationManager.AppSettings["FileSizeAllowed"].ToString())  * 1024 * 1024;
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        int allowedFileSize = file.ContentLength;

                        if(file.ContentLength > fileSize)
                        {
                            return Json(string.Format("File size should be less then {0} MB.", Convert.ToInt32(ConfigurationManager.AppSettings["FileSizeAllowed"].ToString())));
                        }

                        string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                        string AllowedFileType = ConfigurationManager.AppSettings["FileTypeAllowed"].ToString();

                        if(!AllowedFileType.Contains(ext))
                        {
                            return Json("Allowed file type are " + AllowedFileType + ". Please upload accordingly.");
                        }

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        string fileName = Session["UserCode"].ToString() + "$" + DateTime.Now.Ticks + "_Medical" + ext;

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/MedicalReports/"), fileName);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        [HttpPost]
        public ActionResult UploadTreatmentFiles()
        {
            int fileSize = Convert.ToInt32(ConfigurationManager.AppSettings["FileSizeAllowed"].ToString()) * 1024 * 1024;
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        int allowedFileSize = file.ContentLength;

                        if (file.ContentLength > fileSize)
                        {
                            return Json(string.Format("File size should be less then {0} MB.", Convert.ToInt32(ConfigurationManager.AppSettings["FileSizeAllowed"].ToString())));
                        }

                        string ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                        string AllowedFileType = ConfigurationManager.AppSettings["FileTypeAllowed"].ToString();

                        if (!AllowedFileType.Contains(ext))
                        {
                            return Json("Allowed file type are " + AllowedFileType + ". Please upload accordingly.");
                        }

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        string fileName = Session["UserCode"].ToString() +"$" + DateTime.Now.Ticks+ "_Treatment" + ext;


                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/Treatment/"), fileName);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        [HttpPost]
        public ActionResult GetCustomerInfo()
        {
            CustomerRegistrationService registrationService = new CustomerRegistrationService();
            List<CustomerInfoEntity> customer = new List<CustomerInfoEntity>();
            string UserName = string.Empty;
            try
            {
                if(Session["UserCode"] != null)
                {
                    if (!string.IsNullOrEmpty(Session["UserCode"].ToString()))
                    {
                        UserName = Session["UserCode"].ToString();
                    }
                }

                customer = registrationService.GetCustomers(UserName);

                return Json(new { Status = 200, Message = Newtonsoft.Json.JsonConvert.SerializeObject(customer) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.SerializeObject("No data found"), JsonRequestBehavior.AllowGet);
            }
            finally
            {
                registrationService = null;
                customer = null;
            }
        }

        [HttpPost]
        public ActionResult AddCustomerDetail(AdverseDataEntity entity)
        {
            try
            {
                entity.UserId = Session["UserCode"].ToString();

                AdverseFormService service = new AdverseFormService();
                string IsSaved = service.SaveFormChanges(entity);                

                // string HTML = "<!DOCTYPE html><html lang='en'><head> <title>Bootstrap Example</title> <meta charset='utf-8'> <meta name='viewport' content='width=device-width, initial-scale=1'> <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css'> <script src='https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js'></script> <script src='https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js'></script> <style>@import url('https://fonts.googleapis.com/css2?family=Nunito:wght@300&display=swap'); body{background-color: #fff; font-family: 'Nunito', sans-serif;}.header-logo{text-align: center; display: flex;}.header{overflow: hidden; background-color: #f1f1f1; padding: 20px 10px;}.header a{float: left; color: black; text-align: center; padding: 12px; text-decoration: none; font-size: 18px; line-height: 25px; border-radius: 4px;}.header a.logo{font-size: 42px; font-weight: bold; line-height: 1em;}.header-right{float: right;}.header-right a{font-size: 28px; line-height: 36px; font-weight: bold;}.text-bold{font-weight: bold;}.text-light{font-weight: 400;}.textarea{resize: vertical; min-height: 100px; max-height: 200px;}.panel-heading{font-weight: bold;}.header-right-logout{position: absolute; right: 15px; top: 45px; cursor: pointer;}.header-right-logout:hover{cursor: pointer;}.select2{width: 100%!important;}.logo-sub{position: absolute; left: 185px; font-size: 15px; bottom: 2px;}sup{vertical-align: super; font-size: smaller;}@media screen and (min-width: 1200px){.container{width: 1300px;}}@media screen and (min-width: 1400px){.container{width: 1480px;}}@media screen and (min-width: 700px){.header-right-logout{position: absolute; right: 17px; top: 30px; cursor: pointer;}.header a.logo{font-size: 26px; font-weight: bold; line-height: 1em;}.logo-sub{position: absolute; left: 95px; font-size: 15px; bottom: 20px;}.header-right a{font-size: 22px; line-height: 30px; font-weight: bold;}}@media screen and (max-width: 376px){.header a.logo{font-size: 19px !important; font-weight: bold !important; line-height: 1em !important;}.logo-sub{position: absolute !important; left: 14px !important; font-size: 11px !important; bottom: -4px !important;}.header-right-logout{position: absolute !important; right: 16px !important; top: 100px !important; cursor: pointer !important;}.header-right a{font-size: 19px !important; line-height: 36px !important; font-weight: bold !important;}}@media screen and (max-width: 500px){.header a{float: none; display: block; text-align: left;}.header-right-logout{position: absolute; right: 20px; top: 94px; cursor: pointer;}.header-right a{font-size: 24px; line-height: 18px; padding-bottom: 40px; font-weight: bold;}.header-right{float: none;}.header{overflow: hidden; background-color: #f1f1f1; padding: 4px 10px;}a.logo.logo-2{display: flex; position: absolute; top: 2px; font-size: 24px; left: 65px;}.logo-sub{position: absolute; left: 14px; font-size: 14px; bottom: 0;}}</style></head><body> <div class='header'> <div class='row'> <div class='col-sm-12'> <a href='#' class='logo logo-1'> <img src='image/logo4.png' alt='' class='responsive-image'></a> <a href='#' class='logo logo-2'>Human Biologicals Institute <sub class='logo-sub'>A Division of Indian Immunologicals Limited</sub> </a> <div class='header-right'> <a class='active' href='#'>ADVERSE EVENT REPORTING</a> </div></div></div></div><br/> <div class='fluid-container col-md-12'> <div class='row'> <div class='col-sm-12'> <form class='form-horizontal' novalidate autocomplete='off'> <div class='panel panel-default'> <div class='panel-heading'><span class='Basic_Details'>Reporter Details</span></div><div class='panel-body'> <div class='row'> <div class='col-sm-12'> <div class='col-sm-2 text-right'> <label for=''><span class='Name'>Name</span> : </label> </div><div class='col-sm-4'> <label for='' class='nameval text-light'>{rName}</label> </div><div class='col-sm-2 text-right'> <label for=''><span class='Gender'>Gender (at birth)</span> : </label> </div><div class='col-sm-4'> <label for='' class='genderval text-light'>{rGender}</label> </div></div></div><div class='row'> <div class='col-sm-12'> <div class='col-sm-2 text-right'> <label for=''><span class='Occupation'>Occupation</span> : </label> </div><div class='col-sm-4'> <label for='' class='occupval text-light'>{rOccupation}</label> </div><div class='col-sm-2 text-right'> <label for=''><span class='Age'>Age</span>/ <span class='Date_of_birth'>Date of birth</span> : </label> </div><div class='col-sm-4'> <label for='' class='ageval text-light'>{rAge}/{rDob}</label> </div></div></div><div class='row'> <div class='col-sm-12'> <div class='col-sm-2 text-right'> <label for=''><span class='Phone'>Phone</span> : </label> </div><div class='col-sm-4'> <label for='' class='phoneval text-light'>{rPhone}</label> </div><div class='col-sm-2 text-right'> <label for=''><span class='Email'>Email</span> : </label> </div><div class='col-sm-4'> <label for='' class='emailval text-light'>{rEmail}</label> </div></div></div><div class='row'> <div class='col-sm-12'> <div class='col-sm-2 text-right'> <label for=''><span class='Street_Address'>Stree Address</span> : </label> </div><div class='col-sm-4'> <label for='' class='addressval text-light'>{rStreetAddress}</label> </div><div class='col-sm-2 text-right'> <label for=''><span class='City'>City</span> : </label> </div><div class='col-sm-4'> <label for='' class='cityval text-light'>{rCity}</label> </div></div></div><div class='row'> <div class='col-sm-12'> <div class='col-sm-2 text-right'> <label for=''><span class='State_Province_County'>State / Province / County</span> : </label> </div><div class='col-sm-4'> <label for='' class='stateval text-light'>{rState}</label> </div><div class='col-sm-2 text-right'> <label for=''><span class='Country'>Country</span> : </label> </div><div class='col-sm-4'> <label for='' class='countryval text-light'>{rCountry}</label> </div></div></div><div class='row'> <div class='col-sm-12'> <div class='col-sm-2 text-right'> <label for=''><span class='Postal_Code'>Postal Code</span> : </label> </div><div class='col-sm-4'> <label for='' class='postalval text-light'>{rPostal}</label> </div><div class='col-sm-2 text-right'> <label for=''> </label> </div><div class='col-sm-4'> <label for='' class=' text-light'></label> </div></div></div></div></div><div class='panel panel-default'> <div class='panel-heading'><span class='Other_Details'>Other Details</span></div><div class='panel-body'> <div class='form-group'> <label class='control-label col-sm-2' for='datetime_vacc'><span class='For_whom'>For whom</span> : </label> <div class='col-sm-10'>{oForWhom}</div></div></div></div><div class='panel panel-default'> <div class='panel-heading'><span class='Additional_information'> Patient Detials</span> </div><div class='panel-body'> <div class='row'> <div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Name'>Name</span> : </label> <div class='col-sm-8'>{pName}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Phone'>Phone</span> : </label> <div class='col-sm-8'>{pPhone}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='City'>City</span> : </label> <div class='col-sm-8'>{pCity}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Postal_Code'>Postal Code</span> : </label> <div class='col-sm-8'>{pPostalCode}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Gender'>Gender (at birth)</span> : </label> <div class='col-sm-8'>{pGender}</div></div></div><div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Email'>Email</span> : </label> <div class='col-sm-8'>{pEmail}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Street_Address'>Street Address</span> : </label> <div class='col-sm-8'>{pStreetAddress}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='State_Province_County'>State / Province / County</span> : </label> <div class='col-sm-8'>{pState}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Country'>Country</span> :</label> <div class='col-sm-8'>{pCountry}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Age'>Age</span> : </label> <div class='col-sm-8'>{pAge}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Date_of_birth'>Date of birth</span> : </label> <div class='col-sm-8'>{pDob}</div></div></div></div></div></div><div class='panel panel-default'> <div class='panel-heading'><span class='Information_VACCINE'>Information about the VACCINE(S)</span></span> </div><div class='panel-body'> <div class='row'> <div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for='datetime_vacc'><span class='Date_and_time_of_vaccination'>Date and time of vaccination</span> : </label> <div class='col-sm-8'>{vDateTimeVaccination}</div></div><div class='form-group'> <label class='control-label col-sm-4' for='manufac'><span class='Manufacturer'>Manufacturer</span> : </label> <div class='col-sm-8'>{vManufacture}</div></div><div class='form-group'> <label class='control-label col-sm-4' for='manufac'><span class='Manufacturing_Date_Month'>Manufacturing Date/ Month</span> : </label> <div class='col-sm-8'>{vManufactureDate}</div></div></div><div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for='vaccname'><span class='Vaccine_Name'>Vaccine Name</span> : </label> <div class='col-sm-8'>{vVaccineName}</div></div><div class='form-group'> <label class='control-label col-sm-4' for='batchno'><span class='Batch_number'>Batch number</span> : </label> <div class='col-sm-8'>{vBatchNumber}</div></div><div class='form-group'> <label class='control-label col-sm-4' for='batchno'><span class='Expiry_Date_Month'>Expiry Date/ Month</span> : </label> <div class='col-sm-8'>{vExpiryDate}</div></div></div></div></div></div><div class='panel panel-default'> <div class='panel-heading'><span class='Information_about_Vaccination'>Information about Vaccination</span></div><div class='panel-body'> <div class='row'> <div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Route_site_administration'>Route</span> : </label> <div class='col-sm-8'>{ivRoute}</div></div></div><div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Dose_number_vaccine_series'>Dose number</span> : </label> <div class='col-sm-8'>{ivDose}</div></div></div><div class='col-sm-12'> <div class='form-group'> <label class='control-label col-sm-2' for=''><span class='Site_administration'>Site of administration</span> : </label> <div class='col-sm-10'>{ivSiteOfAdministration}</div></div></div></div></div></div><div class='panel panel-default'> <div class='panel-heading'><span class='Information_about_the_Facility_vaccine_administered'>Information about the Facility (or Place) where the vaccine was given/ administered</span></div><div class='panel-body'> <div class='row'> <div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Facility_clinic_name'>Facility/clinic name</span> :</label> <div class='col-sm-8'>{fClinicName}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Street_Address'>Street Address</span> :</label> <div class='col-sm-8'>{fStreeAddress}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='State_Province_County'>State / Province / County</span> :</label> <div class='col-sm-8'>{fState}</div></div></div><div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Email'>Email</span> :</label> <div class='col-sm-8'>{fEmail}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='City'>City</span> :</label> <div class='col-sm-8'>{fCity}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Country'>Country</span> :</label> <div class='col-sm-8'>{fCountry}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Phone_Including_Country_code'>Phone (Including Country code)</span> :</label> <div class='col-sm-8'> ({fPhoneCode}) -{fPhone}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Fax_number'>Fax number</span> :</label> <div class='col-sm-8'>{fFax}</div></div></div></div></div></div><div class='panel panel-default'> <div class='panel-heading'><span class='Description_of_the_adverse_event'>Description of the adverse event, including medical treatment and diagnosis</span> <span class='text-danger'>(* <span class='Required_Fields'>Required Fields</span>)</span> </div><div class='panel-body'> <div class='row'> <div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Adverse_Event'>Adverse Event/ Symptom/ Complaint after receiving the vaccine</span> : </label> <div class='col-sm-8'>{aEvent}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Adverse_event_continuing'>Adverse event continuing</span> : </label> <div class='col-sm-8'>{aEventContinuing}</div></div><div class='form-group endstop'> <label class='control-label col-sm-4' for=''><span class='Stop_Date'>Stop Date</span> : </label> <div class='col-sm-8'>{aStopDate}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Doctor_consultation_taken'>Doctor's consultation taken</span> : </label> <div class='col-sm-8'>{aDoctorConsultation}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Street_Address'>Street Address</span> :</label> <div class='col-sm-8'>{aStreetAddress}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Phone_Including_Country_code'>Phone (Including Country code)</span> :</label> <div class='col-sm-8'> ({aPhoneCode}) /{aPhone}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Results_of_any_medical_tests'>Results of any medical tests and laboratory tests if done</span> : </label> <div class='col-sm-8'>{aMedicalTest}</div></div></div><div class='col-sm-6'> <div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Start_Date'>Start Date</span> : </label> <div class='col-sm-8'>{aStartDate}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''>&nbsp;</label> <div class='col-sm-8'> &nbsp; </div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Clinic_Hospital_Name'>Clinic / Hospital Name</span> : </label> <div class='col-sm-8'>{aClinic}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Doctor_Name'>Doctor Name</span> : </label> <div class='col-sm-8'>{aDoctorName}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='City'>City</span> :</label> <div class='col-sm-8'>{aCity}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='State_Province_County'>State / Province / County</span> :</label> <div class='col-sm-8'>{aState}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Country'>Country</span> :</label> <div class='col-sm-8'>{aCountry}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Email'>Email</span> :</label> <div class='col-sm-8'>{aEmail}</div></div><div class='form-group'> <label class='control-label col-sm-4' for=''><span class='Treatment_given'>Treatment given? If yes, please provide the details</span> : </label> <div class='col-sm-8'>{aTreatment}</div></div></div></div></div></div><div class='panel panel-default'> <div class='panel-heading'><span class='Relevant_Medical'>Relevant Medical History of the Person</span></div><div class='panel-body'> <div class='form-group row'> <div class='col-sm-12'> <label for='usr'><span class='Was_the_patient'>Was the patient pregnant at the time of vaccination?</span></label>{hPregnant}</div></div><div class='form-group row'> <div class='col-sm-12'> <label for='usr'><span class='Allergies_to_medications'>Intake of any medications at the time of vaccination. If yes, details to be provided</span></label> <label>{hIntake}</label> </div></div><div class='form-group row'> <div class='col-sm-12'> <label for='usr'><span class='Intake_of_any_medications'>Allergies to medications, food, or other products. If yes, details to be provided</span></label> <label>{hAllergies}</label> </div></div><div class='form-group row'> <div class='col-sm-12'> <label for='usr'><span class='Other_illnesses_at_the_time'>Other illnesses at the time of vaccination (and up to one month prior)</span> </label> <label>{hIllness}</label> </div></div><div class='form-group row'> <div class='col-sm-12'> <label for='usr'><span class='Chronic'>Chronic or long-standing health conditions</span> </label> <label>{hChronic}</label> </div></div></div></div></form> </div></div><br></div></body></html>";
                if (!string.IsNullOrEmpty(IsSaved) && IsSaved != "0")
                {
                    GenerateXMLFile(entity);
                    string HTML = GetMailTemplate(entity);
                    GenerateHTMLFile(HTML);
                    return Json(new { Status = 200, Message = "Information Saved successfully", Error = "" }, JsonRequestBehavior.AllowGet);
                }                    
                else
                    return Json(new { Status = 100, Message = "Information not saved due to some technical error. Please try again in some time.", Error = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 300, Message = "Information not saved due to some technical error. Please try again in some time.", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public void GenerateHTMLFile(string HTML)
        {
            StringReader sr = new StringReader(HTML);
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
            }

            PdfWriter writer2 = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            XMLWorkerHelper.GetInstance().ParseXHtml(writer2, pdfDoc, sr);
            pdfDoc.Close();
        }

        public void GenerateXMLFile(AdverseDataEntity entity)
        {

            XmlDocument xmlPatientDetail    = new ConversionMethod().ConvertObjectToXML<PatientDetail>(entity.PatientDetail);
            XmlDocument xmlAdverseEvent     = new ConversionMethod().ConvertObjectToXML<AdverseEvent>(entity.AdverseEvent);
            XmlDocument xmlFacility         = new ConversionMethod().ConvertObjectToXML<Facility>(entity.Facility);
            XmlDocument xmlHistory          = new ConversionMethod().ConvertObjectToXML<History>(entity.History);
            XmlDocument xmlVaccination      = new ConversionMethod().ConvertObjectToXML<Vaccination>(entity.Vaccination);
            XmlDocument xmlVaccine          = new ConversionMethod().ConvertObjectToXML<Vaccine>(entity.Vaccine);

            string xmlheader = "<?xml version=\"1.0\"?><AdverseEvent>";
            xmlheader = xmlheader + xmlPatientDetail.InnerXml + xmlAdverseEvent.InnerXml + xmlFacility.InnerXml + xmlHistory.InnerXml +
                xmlVaccination.InnerXml + xmlVaccine.InnerXml + "</AdverseEvent>";
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(xmlheader);
            xdoc.Save(Server.MapPath("~/Uploads/Document.xml"));
        }

        public string GetMailTemplate(AdverseDataEntity entity)
        {
            try
            {
                string[] lines;
                var list = new List<string>();
                var fileStream = new FileStream(Server.MapPath(@"/Models/CustomerAdverseEventForm.txt"), FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                lines = list.ToArray();

                return string.Join(" ", lines)
                    .Replace("{rName}", entity.PatientDetail.PName)
                    .Replace("{rGender}", entity.PatientDetail.PName)
                    .Replace("{rOccupation}", entity.PatientDetail.PName)
                    .Replace("{rAge}", entity.PatientDetail.PName)
                    .Replace("{rDob}", entity.PatientDetail.PName)
                    .Replace("{rPhone}", entity.PatientDetail.PName)
                    .Replace("{rEmail}", entity.PatientDetail.PName)
                    .Replace("{rStreetAddress}", entity.PatientDetail.PName)
                    .Replace("{rCity}", entity.PatientDetail.PName)
                    .Replace("{rState}", entity.PatientDetail.PName)
                    .Replace("{rCountry}", entity.PatientDetail.PName)
                    .Replace("{rPostal}", entity.PatientDetail.PName)

                    .Replace("{oForWhom}", entity.OtherDetails)
                    // Patient Details
                    .Replace("{pName}",             entity.PatientDetail.PName)
                    .Replace("{pEmail}",            entity.PatientDetail.PEmail)
                    .Replace("{pPhone}",            entity.PatientDetail.PPhone)
                    .Replace("{pStreetAddress}",    entity.PatientDetail.PStreetAddress)
                    .Replace("{pCity}",             entity.PatientDetail.PCity)
                    .Replace("{pState}",            entity.PatientDetail.PState)
                    .Replace("{pPostalCode}",       entity.PatientDetail.PPostalCode)
                    .Replace("{pCountry}",          entity.PatientDetail.PCountry)
                    .Replace("{pGender}",           entity.PatientDetail.PGender)
                    .Replace("{pAge}",              entity.PatientDetail.PAge)
                    .Replace("{pDob}",              entity.PatientDetail.PDOB)
                     // Vaccine
                    .Replace("{vDateTimeVaccination}",  entity.Vaccine.DateTimeVaccine)
                    .Replace("{vVaccineName}",          entity.Vaccine.VaccineName)
                    .Replace("{vManufacture}",          entity.Vaccine.Manufacturer)
                    .Replace("{vBatchNumber}",          entity.Vaccine.BatchNo)
                    .Replace("{vManufactureDate}",      entity.Vaccine.ManufactureDateMonth)
                    .Replace("{vExpiryDate}",           entity.Vaccine.ExpireDateMonth)
                    // Vaccination 
                    .Replace("{ivRoute}",                   entity.Vaccination.Route)
                    .Replace("{ivDose}",                    entity.Vaccination.DoseNumber)
                    .Replace("{ivSiteOfAdministration}",    entity.Vaccination.SiteAdmin)
                    //Facility
                    .Replace("{fClinicName}",   entity.Facility.FacilityName)
                    .Replace("{fEmail}",        entity.Facility.FEmail)
                    .Replace("{fStreeAddress}", entity.Facility.FStreetAddress)
                    .Replace("{fCity}",         entity.Facility.FCity)
                    .Replace("{fCountry}",      entity.Facility.FCountry)
                    .Replace("{fState}",        entity.Facility.FState)
                    .Replace("{fPhoneCode}",    entity.Facility.FPhoneCode)
                    .Replace("{fPhone}",        entity.Facility.FPhone)
                    .Replace("{fFax}",          entity.Facility.FFax)
                    // Adverse Event
                    .Replace("{aEvent}",                entity.AdverseEvent.Event)
                    .Replace("{aStartDate}",            entity.AdverseEvent.EStartDate)
                    .Replace("{aEventContinuing}",      entity.AdverseEvent.EventContinue)
                    .Replace("{aClinic}",               entity.AdverseEvent.EClinic)
                    .Replace("{aStopDate}",             entity.AdverseEvent.EStopDate)
                    .Replace("{aDoctorName}",           entity.AdverseEvent.EDocName)
                    .Replace("{aDoctorConsultation}",   entity.AdverseEvent.EDCT)
                    .Replace("{aCity}",                 entity.AdverseEvent.ECity)
                    .Replace("{aStreetAddress}",        entity.AdverseEvent.EStreetAddress)
                    .Replace("{aState}",                entity.AdverseEvent.EState)
                    .Replace("{aPhoneCode}",            entity.AdverseEvent.EPhoneCodeOther)
                    .Replace("{aPhone}",                entity.AdverseEvent.EPhone)
                    .Replace("{aCountry}",              entity.AdverseEvent.ECountry)
                    .Replace("{aMedicalTest}",          entity.AdverseEvent.EResultMedical)
                    .Replace("{aEmail}",                entity.AdverseEvent.EEmail)
                    .Replace("{aTreatment}",            entity.AdverseEvent.ETreatment)
                    // History
                    .Replace("{hPregnant}", entity.History.PregnantVaccination)
                    .Replace("{hIntake}",   entity.History.Medications)
                    .Replace("{hAllergies}",entity.History.Allergies)
                    .Replace("{hIllness}",  entity.History.Illnesses)
                    .Replace("{hChronic}",  entity.History.Chronic);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}