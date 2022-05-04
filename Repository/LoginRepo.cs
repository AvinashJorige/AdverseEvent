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
    public class LoginRepo
    {
        private readonly string sqlConnectionString = string.Empty;
        public SqlConnection con;
        public LoginRepo()
        {
            sqlConnectionString = SecurityManager.Decrypt(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        }

        private void connection()
        {
            con = new SqlConnection(sqlConnectionString);
        }

        public UserLoginDetail ValidateUser(string UserCode, string Password)
        {
            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Username", UserCode);
                queryParameters.Add("@Password", Password);

                connection();
                con.Open();
                UserLoginDetail LogDetail= SqlMapper.Query<UserLoginDetail>(con, "USP_ValidateUser", queryParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                con.Close();
                return LogDetail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int SaveResetPasswordCode(string ResetCode, string CustId)
        {
            try
            {
                CustId = CustId.Trim();
                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    int IsVallid = connection.Execute(@"Update tbl_Adv_CustLog_T set ResetPasswordCode = @ResetCode where CustId = @CustId",
                                         new { ResetCode, CustId });
                    connection.Close();
                    return IsVallid;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public string ResetPasswordFromMail(string UserCode, string Password, string ResetCode)
        {
            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@CustId", UserCode);
                queryParameters.Add("@Password", Password);
                queryParameters.Add("@ResetPasswordCode", ResetCode);

                connection();
                con.Open();
                string LogDetail = SqlMapper.Query<string>(con, "USP_ResetPassword", queryParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                con.Close();
                return LogDetail;
            }
            catch (Exception ex)
            {
                return "invalid";
            }
        }

        public string ChangePassword(string UserCode, string Password)
        {
            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@CustId", UserCode);
                queryParameters.Add("@Password", Password);

                connection();
                con.Open();
                string LogDetail = SqlMapper.Query<string>(con, "USP_ChangePassword", queryParameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                con.Close();
                return LogDetail;
            }
            catch (Exception ex)
            {
                return "invalid";
            }
        }

        public UserResetDetail GetCustomerByEmail(string EmailId)
        {
            UserResetDetail entity = new UserResetDetail();
            try
            {
                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    entity = connection.Query<UserResetDetail>(string.Format(@"SELECT top 1 Name as UserName,Email,CustomerCode as UserCode FROM tbl_Customer_Registration_T Nolock WHERE Email = '{0}'",
                        EmailId)).FirstOrDefault();
                    connection.Close();
                    return entity;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                entity = null;
            }
            return entity;
        }

        public UserResetDetail GetCustomerByResetCode(string ResetCode)
        {
            UserResetDetail entity = new UserResetDetail();
            try
            {
                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    entity = connection.Query<UserResetDetail>(string.Format(@"SELECT top 1 CustId as UserCode FROM tbl_Adv_CustLog_T Nolock WHERE ResetPasswordCode = '{0}'",
                        ResetCode)).FirstOrDefault();
                    connection.Close();
                    return entity;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                entity = null;
            }
            return entity;
        }

        public string GetValidResetCode(string ResetCode)
        {
            string entity = string.Empty;
            try
            {
                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    entity = connection.Query<string>(string.Format(@"SELECT top 1 ResetPasswordCode FROM tbl_Adv_CustLog_T Nolock WHERE ResetPasswordCode = '{0}'",
                        ResetCode)).FirstOrDefault();
                    connection.Close();
                    return entity;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                entity = null;
            }
            return entity;
        }

    }
}
