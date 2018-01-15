﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public interface IStationeryDAO
    {
        List<StationeryDAO> GetAllStationery();
        User FindByItemCode(string itemCode);
        Boolean AddNewStationery(StationeryDAO stationery);
        int UpdateStationeryInfo(StationeryDAO stationery);
        Boolean DeleteStationery(string itemCode);
     
    }
}