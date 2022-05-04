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
    public class UserRepo
    {
        private readonly string sqlConnectionString = string.Empty;
        public SqlConnection con;
        public UserRepo()
        {
            sqlConnectionString = SecurityManager.Decrypt(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        }

        private void connection()
        {
            con = new SqlConnection(sqlConnectionString);
        }


        public List<CustomerInfoEntity> ValidateUser(string UserCode, string Password)
        {
            try
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@UserCode", UserCode);
                queryParameters.Add("@Password", Password);

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
