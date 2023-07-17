using CustomersAPI.Data.Abstractions;
using CustomersAPI.Data.Models;
using CustomersAPI.Data.Models.Enums;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CustomersAPI.Data.DataAccess
{
    public class CustomersData : ICustomerData
    {
        private readonly IConfiguration _config;

        public CustomersData(IConfiguration config) {
            _config = config;
        }

        private string GetConnectionString()
        {
            return _config.GetConnectionString("DDFurniture");
        }

        public CustomerModel GetCustomerById(int id) 
        {
            var customer = new CustomerModel();
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_GET_CUSTOMER_BY_ID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read()) {
                            customer.Success = true;
                            customer.Id = reader.GetInt32("ID_CUSTOMER");
                            customer.Name = reader.GetString("NAME_CUSTOMER");
                            customer.Address = reader.GetString("ADDRESS_CUSTOMER");
                            customer.Phone = reader.GetString("PHONE_CLIENT");
                            customer.Email = reader.GetString("EMAIL_CLIENT");
                            customer.Gender = (GenderEnum)reader.GetInt32("ID_GENDER_CUSTOMER");
                            customer.BirthDay = reader.GetString("BIRTHDAY_CUSTOMER");
                            customer.Contry = reader.GetInt32("ID_COUNTRY");
                            customer.State = reader.GetInt32("ID_STATE_PROVINCE");
                        }
                    }   
                    else
                    {
                        customer.Success = false;
                        customer.Message = "No results";
                    }
                }
            }
            catch (Exception ex)
            {
                customer.Success = false;
                customer.Message = ex.Message;
            }
            finally
            {
                conn.Dispose();
            }
            return customer;
        }

        public List<CustomerModel> GetCustomers()
        {
            var customers = new List<CustomerModel>();
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_GET_CUSTOMERS", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read()) {
                            customers.Add( new CustomerModel {
                                Success = true,
                                Id = reader.GetInt32("ID_CUSTOMER"),
                                Name = reader.GetString("NAME_CUSTOMER"),
                                Address = reader.GetString("ADDRESS_CUSTOMER"),
                                Phone = reader.GetString("PHONE_CLIENT"),
                                Email = reader.GetString("EMAIL_CLIENT"),
                                Gender = (GenderEnum)reader.GetInt32("ID_GENDER_CUSTOMER"),
                                BirthDay = reader.GetString("BIRTHDAY_CUSTOMER"),
                                Contry = reader.GetInt32("ID_COUNTRY"),
                                State = reader.GetInt32("ID_STATE_PROVINCE")
                            });
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
            return customers;
        }

        public ResultModel AddCustomer(CustomerModel customer) 
        {
            var result = new ResultModel { Success = false, Message = "", Result = "" };
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_ADD_CUSTOMER", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NAME", customer.Name);
                    cmd.Parameters.AddWithValue("@BIRTHDAY", customer.BirthDay);
                    cmd.Parameters.AddWithValue("@GENDER", (int)customer.Gender);
                    cmd.Parameters.AddWithValue("@COUNTRY", customer.Contry);
                    cmd.Parameters.AddWithValue("@STATE", customer.State);
                    cmd.Parameters.AddWithValue("@PHONE", customer.Phone);
                    cmd.Parameters.AddWithValue("@EMAIL", customer.Email);
                    cmd.Parameters.AddWithValue("@ADDRESS", customer.Address);

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

        public ResultModel DeleteCustomerById(int id)
        {
            var result = new ResultModel { Success = false, Message = "", Result = "" };
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_DEL_CUSTOMER_BY_ID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

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

        public ResultModel UpdateCustomerById(int Id, CustomerModel customer)
        {
            var result = new ResultModel { Success = false, Message = "", Result = "" };
            var conn = new SqlConnection(GetConnectionString());
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("SP_UPD_CUSTOMER_BY_ID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.Parameters.AddWithValue("@NAME", customer.Name);
                    cmd.Parameters.AddWithValue("@EMAIL", customer.Email);
                    cmd.Parameters.AddWithValue("@ADDRESS", customer.Address);
                    cmd.Parameters.AddWithValue("@PHONE", customer.Phone);
                    cmd.Parameters.AddWithValue("@GENDER", (int)customer.Gender);
                    cmd.Parameters.AddWithValue("@BIRTHDAY", customer.BirthDay);
                    cmd.Parameters.AddWithValue("@COUNTRY", customer.Contry);
                    cmd.Parameters.AddWithValue("@STATE", customer.State);

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
    }
}
