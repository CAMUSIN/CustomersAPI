﻿using CustomersAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersAPI.Business.Abstractions
{
    public interface ICustomerBusiness
    {
        ResultModel AddCustomer(CustomerModel customer);

        List<CustomerModel> GetCustomers();

        CustomerModel GetCustomerById(int Id);

        ResultModel UpdateCustomerById(int Id, CustomerModel customer);

        ResultModel DeleteCustomerById(int Id);

    }
}
