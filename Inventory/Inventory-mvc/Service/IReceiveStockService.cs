﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
namespace Inventory_mvc.Service
{
    public class IReceiveStockService
    {
        List<Purchase_Detail> GetAllPurchaseDetail();
    }
}