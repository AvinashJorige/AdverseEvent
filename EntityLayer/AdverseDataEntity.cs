using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class PatientDetail
    {
        [JsonProperty("pName")]
        public string PName { get; set; }

        [JsonProperty("pEmail")]
        public string PEmail { get; set; }

        [JsonProperty("pPhone")]
        public string PPhone { get; set; }

        [JsonProperty("pStreetAddress")]
        public string PStreetAddress { get; set; }

        [JsonProperty("pCity")]
        public string PCity { get; set; }

        [JsonProperty("pState")]
        public string PState { get; set; }

        [JsonProperty("pPostalCode")]
        public string PPostalCode { get; set; }

        [JsonProperty("pCountry")]
        public string PCountry { get; set; }

        [JsonProperty("pGender")]
        public string PGender { get; set; }

        [JsonProperty("pAge")]
        public string PAge { get; set; }

        [JsonProperty("pDOB")]
        public string PDOB { get; set; }
    }

    public class Vaccine
    {
        [JsonProperty("DateTimeVaccine")]
        public string DateTimeVaccine { get; set; }

        [JsonProperty("VaccineName")]
        public string VaccineName { get; set; }

        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("BatchNo")]
        public string BatchNo { get; set; }

        [JsonProperty("ManufactureDateMonth")]
        public string ManufactureDateMonth { get; set; }

        [JsonProperty("ExpireDateMonth")]
        public string ExpireDateMonth { get; set; }
    }

    public class Vaccination
    {
        [JsonProperty("Route")]
        public string Route { get; set; }

        [JsonProperty("DoseNumber")]
        public string DoseNumber { get; set; }

        [JsonProperty("SiteAdmin")]
        public string SiteAdmin { get; set; }
    }

    public class Facility
    {
        [JsonProperty("FacilityName")]
        public string FacilityName { get; set; }

        [JsonProperty("FStreetAddress")]
        public string FStreetAddress { get; set; }

        [JsonProperty("FState")]
        public string FState { get; set; }

        [JsonProperty("FEmail")]
        public string FEmail { get; set; }

        [JsonProperty("FCity")]
        public string FCity { get; set; }

        [JsonProperty("FCountry")]
        public string FCountry { get; set; }

        [JsonProperty("FPhoneCode")]
        public string FPhoneCode { get; set; }

        [JsonProperty("FPhone")]
        public string FPhone { get; set; }

        [JsonProperty("FFax")]
        public string FFax { get; set; }
    }

    public class Treatmentupload
    {
    }

    public class ResultMedicalupload
    {
    }

    public class AdverseEvent
    {
        [JsonProperty("Event")]
        public string Event { get; set; }

        [JsonProperty("EventContinue")]
        public string EventContinue { get; set; }

        [JsonProperty("EStopDate")]
        public string EStopDate { get; set; }

        [JsonProperty("EDCT")]
        public string EDCT { get; set; }

        [JsonProperty("ETreatment")]
        public string ETreatment { get; set; }

        [JsonProperty("EResultMedical")]
        public string EResultMedical { get; set; }

        [JsonProperty("EStartDate")]
        public string EStartDate { get; set; }

        [JsonProperty("EClinic")]
        public string EClinic { get; set; }

        [JsonProperty("EDocName")]
        public string EDocName { get; set; }

        [JsonProperty("ECity")]
        public string ECity { get; set; }

        [JsonProperty("EState")]
        public string EState { get; set; }

        [JsonProperty("ECountry")]
        public string ECountry { get; set; }

        [JsonProperty("EPhoneCodeOther")]
        public string EPhoneCodeOther { get; set; }

        [JsonProperty("EPhone")]
        public string EPhone { get; set; }

        [JsonProperty("EEmail")]
        public string EEmail { get; set; }

        [JsonProperty("EStreetAddress")]
        public string EStreetAddress { get; set; }

        [JsonProperty("treatmentupload")]
        public Treatmentupload Treatmentupload { get; set; }

        [JsonProperty("resultMedicalupload")]
        public ResultMedicalupload ResultMedicalupload { get; set; }
    }

    public class History
    {
        [JsonProperty("PregnantVaccination")]
        public string PregnantVaccination { get; set; }

        [JsonProperty("Medications")]
        public string Medications { get; set; }

        [JsonProperty("Allergies")]
        public string Allergies { get; set; }

        [JsonProperty("Illnesses")]
        public string Illnesses { get; set; }

        [JsonProperty("Chronic")]
        public string Chronic { get; set; }
    }

    public class AdverseDataEntity
    {
        [JsonProperty("PatientDetail")]
        public PatientDetail PatientDetail { get; set; }

        [JsonProperty("Vaccine")]
        public Vaccine Vaccine { get; set; }

        [JsonProperty("Vaccination")]
        public Vaccination Vaccination { get; set; }

        [JsonProperty("Facility")]
        public Facility Facility { get; set; }

        [JsonProperty("AdverseEvent")]
        public AdverseEvent AdverseEvent { get; set; }

        [JsonProperty("History")]
        public History History { get; set; }

        [JsonProperty("OtherDetails")]
        public string OtherDetails { get; set; }
        public string UserId { get; set; }
    }
}
