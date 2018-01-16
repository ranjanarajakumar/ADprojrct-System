﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;

namespace Inventory_mvc.Service
{
    interface ICollectionPointService
    {
        bool isExistingCode(int collectionPointID);

        List<Collection_Point> GetAllCollectionPoint();

        Collection_Point GetCollectionPointByID(int collectionPointID);

       bool AddNewCollectionPoint(CollectionPointViewModel cpVM);

    }
}
