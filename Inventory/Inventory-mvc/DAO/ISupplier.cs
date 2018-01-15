﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface ISupplier
    {
        List<Supplier> GetAllSupplier();
        User FindBySupplierCode(string supplierCode);
        Boolean AddNewSupplier(Supplier supplier);
        int UpdateSupplierInfo(Supplier supplier);
        Boolean DeleteSupplier(string supplierCode);
    }
}