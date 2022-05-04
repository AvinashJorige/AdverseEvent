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
    public class CustomerRegistrationRepo
    {
        private readonly string sqlConnectionString = string.Empty;
        public SqlConnection con;
        public CustomerRegistrationRepo()
        {
            sqlConnectionString = SecurityManager.Decrypt(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        }

        private void connection()
        {
            con = new SqlConnection(sqlConnectionString);
        }

        public string GetCustomerCode()
        {
            try
            {
                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    string seq = connection.Query<string>(string.Format(@"SELECT (count(*) + 1) FROM tbl_Customer_Registration_T Nolock")).FirstOrDefault();

                    string code = connection.Query<string>(string.Format(@"select dbo.customerNumber({0})", seq)).FirstOrDefault();
                    connection.Close();
                    return code;
                }
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

        public int AddRegisterCustomer(CustomerRegistrationEntity entity, string IPAddress, string Browser, string BrowserVersion, string Password)
        {
            try
            {
                string CustId = entity.CustomerCode.Trim();
                string CreatedBy = entity.CustomerCode.Trim();
                DateTime LastLogin = DateTime.Now;
                DateTime CreatedDate = DateTime.Now;

                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    var affectedRows = connection.Execute(@"Insert into tbl_Customer_Registration_T(CustomerCode,CountryCode,Language,Name,GenderCode,Occupation,DOB,Age,Phone,Email,StreetAddress,DoctorRegNo,City,State,PostalCode)
                                                        values(@customerCode,@CountryCode,@Language,@Name,@GenderCode,@Occupation,@DOB,@Age,@Phone,@Email,@StreetAddress,@DoctorRegNo,@City,@State,@PostalCode)",
                                        new
                                        {
                                          entity.CustomerCode,
                                          entity.CountryCode,
                                          entity.Language,
                                          entity.Name,
                                          entity.GenderCode,
                                          entity.Occupation,
                                          entity.DOB,
                                          entity.Age,
                                          entity.Phone,
                                          entity.Email,
                                          entity.StreetAddress,
                                          entity.DoctorRegNo,
                                          entity.City,
                                          entity.State,
                                          entity.PostalCode
                                        });

                    var InsertedRows = connection.Execute(@"Insert into tbl_Adv_CustLog_T(CustId, Password, Broswer, BroswerVersion, LastLogin, CreatedDate, ClientIP)
                                                        values(@CustId, @Password, @Browser, @BrowserVersion, @LastLogin, @CreatedDate, @IPAddress)",
                                        new
                                        {
                                            CustId,
                                            Password,
                                            Browser,
                                            BrowserVersion,
                                            LastLogin,
                                            CreatedDate,
                                            IPAddress
                                        });

                    var InsertPasswordLogRows = connection.Execute(@"Insert into tbl_Adv_PasswordLog_T(CustId, Password, CreatedBy, Status )
                                                        values(@CustId, @Password, @CreatedBy, 1)",
                                        new
                                        {
                                            CustId,
                                            Password,
                                            CreatedBy
                                        });
                    connection.Close();
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public int GetEmailDuplicate(string Email)
        {
            try
            {
                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    int IsValid = connection.Query<int>(string.Format(@"SELECT Count(Email) FROM tbl_Customer_Registration_T Nolock WHERE Email = '{0}'",
                        Email)).FirstOrDefault();
                    connection.Close();
                    return IsValid;
                }
            }
            catch (Exception)
            {

            }
            return 0;
        }

        public CustomerRegistrationEntity GetCustRegisted(string Id)
        {
            CustomerRegistrationEntity entity = new CustomerRegistrationEntity();
            try
            {
                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    entity = connection.Query<CustomerRegistrationEntity>(string.Format(@"SELECT CountryCode,Language,Name,GenderCode,Occupation,DOB,Age,Phone,Email,StreetAddress,DoctorRegNo,City,State,PostalCode
                                            FROM tbl_Customer_Registration_T Nolock WHERE Id = '{0}'", 
                        Id)).FirstOrDefault();
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
        public List<CustomerInfoEntity> GetCustomers(string CustomerId)
        {
            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@custId", CustomerId);

                connection();
                con.Open();
                IList<CustomerInfoEntity> CustList = SqlMapper.Query<CustomerInfoEntity>(con, "USP_GetCustomerInfo", queryParameters, commandType: CommandType.StoredProcedure).ToList();                
                con.Close();
                return CustList.ToList();
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
