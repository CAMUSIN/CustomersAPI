using CustomersAPI.Data.Abstractions;
using CustomersAPI.Data.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CustomersAPI.Data.DataAccess
{
    public class UserData : IUserData,ISecurityData
    {
        private readonly IConfiguration _config;
        public UserData(IConfiguration config)
        {
            _config = config;
        }

        private string GetConnectionString()
        {
            return _config.GetConnectionString("DDFurniture");
        }

        public ResultModel AddUser(UserLogin user) 
        {
            var result = new ResultModel { Success = false, Message = "", Result = "" };
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_ADD_USER", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NAME", user.UserName);
                    cmd.Parameters.AddWithValue("@PWD", user.Password);

                    result.Result = cmd.ExecuteNonQuery().ToString();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            finally 
            { 
                conn.Dispose();
            }
            return result;
        }

        public UserModel GetUserById(int id)
        {
            var result = new UserModel();
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_GET_USER_BY_ID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read()) {
                            result.Success = true;
                            result.Id = reader.GetInt32("ID_USER");
                            result.UserName = reader.GetString("NAME_USER");
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "No results";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success=false;
                result.Message = ex.Message;
            }
            finally
            {
                conn.Dispose();
            }
            return result;
        }

        public ResultModel UpdateUserById(int Id, UserLogin user)
        {
            var result = new ResultModel { Success = false, Message = "", Result = "" };
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_UPD_USER_BY_ID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.Parameters.AddWithValue("@NAME", user.UserName);
                    cmd.Parameters.AddWithValue("@PWD", user.Password);

                    result.Result = cmd.ExecuteNonQuery().ToString();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            finally
            {
                conn.Dispose();
            }
            return result;
        }

        public ResultModel DeleteUserById(int Id)
        {
            var result = new ResultModel { Success = false, Message = "", Result = "" };
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_DEL_USER_BY_ID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Id);

                    result.Result = cmd.ExecuteNonQuery().ToString();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            finally
            {
                conn.Dispose();
            }
            return result;
        }

        public ResultModel AddUserRole(int IdUser, int IdRole) {
            var result = new ResultModel { Success = false, Message = "", Result = "" };
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_ADD_USER_ROLE", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_USER", IdUser);
                    cmd.Parameters.AddWithValue("@ID_ROLE", IdRole);

                    result.Result = cmd.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            finally
            {
                conn.Dispose();
            }
            return result;
        }

        public string GetUserRoles(UserLogin userLogin)
        {
            var result = new StringBuilder();
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_GET_USER_ROLES", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NAME", userLogin.UserName);
                    cmd.Parameters.AddWithValue("@PWD", userLogin.Password);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read()) { 
                            result.Append(reader.GetString(0));
                            result.Append('|');
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Dispose();
            }
            return result.ToString();
        }

        public int FindUserByCredentials(UserLogin userLogin)
        {
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_VALIDATE_USER", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NAME",SqlDbType.NVarChar, 60).Value = userLogin.UserName;
                    cmd.Parameters.Add("@PWD",SqlDbType.NVarChar, 60).Value = userLogin.Password;

                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable("USERS");
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return (int)dt.Rows[0]["ID"];
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Dispose();
            }
        } 
    }
}
