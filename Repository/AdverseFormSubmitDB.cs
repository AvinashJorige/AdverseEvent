using Dapper;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Utility;

namespace Repository
{
    public class AdverseFormSubmitDB
    {
        private readonly string sqlConnectionString = string.Empty;
        public SqlConnection con;
        public AdverseFormSubmitDB()
        {
            sqlConnectionString = SecurityManager.Decrypt(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        }

        private void connection()
        {
            con = new SqlConnection(sqlConnectionString);
        }

        public string SaveFormChanges(AdverseDataEntity entity)
        {
            try
            {
                var queryParameters = new DynamicParameters();
                #region MyRegion
                queryParameters.Add("@Patient_Name", entity.PatientDetail.PName);
                queryParameters.Add("@Patient_Email", entity.PatientDetail.PEmail);
                queryParameters.Add("@Patient_Phone", entity.PatientDetail.PPhone);
                queryParameters.Add("@Patient_StAddress ", entity.PatientDetail.PStreetAddress);
                queryParameters.Add("@Patient_City", entity.PatientDetail.PCity);
                queryParameters.Add("@Patient_State", entity.PatientDetail.PState);
                queryParameters.Add("@Patient_PostalCode", entity.PatientDetail.PPostalCode);
                queryParameters.Add("@Patient_CountryCode", entity.PatientDetail.PCountry);
                queryParameters.Add("@Patient_Gender", entity.PatientDetail.PGender);
                queryParameters.Add("@Patient_Age", entity.PatientDetail.PAge);
                queryParameters.Add("@Patient_Dob", entity.PatientDetail.PDOB);
                queryParameters.Add("@Vaccine_DateTimeVaccination", Convert.ToDateTime(entity.Vaccine.DateTimeVaccine.ToString()));
                queryParameters.Add("@Vaccine_VarccineName", entity.Vaccine.VaccineName);
                queryParameters.Add("@Vaccine_Manufacturer", entity.Vaccine.Manufacturer);
                queryParameters.Add("@Vaccine_BatchNo", entity.Vaccine.BatchNo);
                queryParameters.Add("@Vaccine_ManufacturingDate", Convert.ToDateTime(entity.Vaccine.ManufactureDateMonth.ToString()));
                queryParameters.Add("@Vaccine_ExpiryDate", Convert.ToDateTime(entity.Vaccine.ExpireDateMonth.ToString()));
                queryParameters.Add("@Vaccination_Route", entity.Vaccination.Route);
                queryParameters.Add("@Vaccination_SiteOfAdministration", entity.Vaccination.SiteAdmin);
                queryParameters.Add("@Vaccination_DoseNumber", entity.Vaccination.DoseNumber);
                queryParameters.Add("@Facility_Facility", entity.Facility.FacilityName);
                queryParameters.Add("@Facility_Email", entity.Facility.FEmail);
                queryParameters.Add("@Facility_StreetAddress", entity.Facility.FStreetAddress);
                queryParameters.Add("@Facility_City", entity.Facility.FCity);
                queryParameters.Add("@Facility_Country", entity.Facility.FCountry);
                queryParameters.Add("@Facility_Phone", entity.Facility.FPhone);
                queryParameters.Add("@Facility_State", entity.Facility.FState);
                queryParameters.Add("@AdverseEvent_AdverseEvent", entity.AdverseEvent.Event);
                queryParameters.Add("@AdverseEvent_StartDate", entity.AdverseEvent.EStartDate);
                queryParameters.Add("@AdverseEvent_StopDate", entity.AdverseEvent.EStopDate);
                queryParameters.Add("@AdverseEvent_AdverseEventContinuing", entity.AdverseEvent.EventContinue.ToUpper() == "YES");
                queryParameters.Add("@AdverseEvent_Clinic", entity.AdverseEvent.EClinic);
                queryParameters.Add("@AdverseEvent_DoctorName", entity.AdverseEvent.EDocName);
                queryParameters.Add("@AdverseEvent_City", entity.AdverseEvent.ECity);
                queryParameters.Add("@AdverseEvent_DoctorConsultationTaken", entity.AdverseEvent.EDCT.ToUpper() == "YES");
                queryParameters.Add("@AdverseEvent_State", entity.AdverseEvent.EState);
                queryParameters.Add("@AdverseEvent_CountryCode", entity.AdverseEvent.ECountry);
                queryParameters.Add("@AdverseEvent_StreetAddress", entity.AdverseEvent.EStreetAddress);
                queryParameters.Add("@AdverseEvent_Email", entity.AdverseEvent.EEmail);
                queryParameters.Add("@AdverseEvent_Phone", entity.AdverseEvent.EPhone);
                queryParameters.Add("@AdverseEvent_ResultOfMedicalLabTest", entity.AdverseEvent.EResultMedical);
                queryParameters.Add("@AdverseEvent_TreatmentGiven", entity.AdverseEvent.ETreatment);
                queryParameters.Add("@History_PatientPregnant", entity.History.PregnantVaccination);
                queryParameters.Add("@History_Allergies", entity.History.Allergies);
                queryParameters.Add("@History_Medications", entity.History.Medications);
                queryParameters.Add("@History_Illnesses", entity.History.Illnesses);
                queryParameters.Add("@History_Chronic", entity.History.Chronic);
                queryParameters.Add("@OtherDetail", entity.OtherDetails);
                queryParameters.Add("@userId", entity.UserId); 
                #endregion

                connection();
                con.Open();
                string CustFormSave = SqlMapper.Query<string>(con, "USP_SubmitAdverseEvent", queryParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                con.Close();
                return CustFormSave;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
