﻿using Inventory_mvc.Models;
using Inventory_mvc.Service;
using Inventory_mvc.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Inventory_mvc.Controllers
{
    public class StationeryController : Controller
    {

        IStationeryService stationeryService = new StationeryService();
        ISupplierService supplierService = new SupplierService();
        IUserService userService = new UserService();
        ITransactionRecordService transactionService = new TransactionRecordService();

        // GET: Stationery
        public ActionResult Index(string searchString, int? page, string categoryID = "All")
        {
            // TODO: REMOVE HARD CODED REQUESTER ID
            //string requesterID = HttpContext.User.Identity.Name;
            string userID = "S1017"; // clerk
            //string userID = "S1016"; // supervisor

            // Store clerk roleID == 7
            int roleID = userService.GetRoleByID(userID);
            ViewBag.Role = (roleID.ToString() == "7") ? "StoreClerk" : "";


            List<StationeryViewModel> stationeries = stationeryService.GetStationeriesVMBasedOnCriteria(searchString, categoryID);

            ViewBag.CategoryList = stationeryService.GetAllCategory();
            ViewBag.CategoryID = (categoryID == "All") ? "All" : categoryID;
            ViewBag.SearchString = searchString;
            ViewBag.Page = page;

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(stationeries.ToPagedList(pageNumber, pageSize));
        }



        // GET: Supplier/Edit/{id}
        public ActionResult Edit(string id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }

            // Get select list for supplier
            List<SupplierViewModel> supplierList = supplierService.GetAllSuppliers();
            SelectList selectList = new SelectList(supplierList, "SupplierCode", "SupplierName");
            ViewBag.selectList = selectList;

            ViewBag.unitOfMeasure = stationeryService.GetAllUOMList();

            //Get selectedID list for category
            List<Category> categories = stationeryService.GetAllCategory();
            SelectList selectListID = new SelectList(categories, "categoryID", "categoryName");
            ViewBag.selectListofCategory = selectListID;

            StationeryViewModel stationeryVM = stationeryService.FindStationeryViewModelByItemCode(id);
            return View(stationeryVM);
        }


        // POST: Supplier/Edit/{id}
        [HttpPost]
        public ActionResult Edit(StationeryViewModel stationeryVM)
        {
            ViewBag.unitOfMeasure = stationeryService.GetAllUOMList();

            // Get select list for supplier
            List<SupplierViewModel> supplierList = supplierService.GetAllSuppliers();
            SelectList selectList = new SelectList(supplierList, "SupplierCode", "SupplierName");
            ViewBag.selectList = selectList;

            //Get selectedID list for category
            List<Category> categories = stationeryService.GetAllCategory();
            SelectList selectListID = new SelectList(categories, "categoryID", "categoryName");
            ViewBag.selectListofCategory = selectListID;

            string code = stationeryVM.ItemCode;
            int level = stationeryVM.ReorderLevel;
            int qty = stationeryVM.ReorderQty;
            decimal price = stationeryVM.Price;

            //for supplier validate
            string supplier1 = stationeryVM.FirstSupplierCode;
            string supplier2 = stationeryVM.SecondSupplierCode;
            string supplier3 = stationeryVM.ThirdSupplierCode;
            if (stationeryService.isExistingSupplierCode(supplier1, supplier2))
            {
                string errorMessage = String.Format("{0} has been used in First Supplier.", supplier1);
                ModelState.AddModelError("SecondSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingSupplierCode(supplier1, supplier3))
            {
                string errorMessage = String.Format("{0} has been used in First Supplier.", supplier1);
                ModelState.AddModelError("ThirdSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingSupplierCode(supplier2, supplier3))
            {
                string errorMessage = String.Format("{0} has been used in Second Supplier.", supplier2);
                ModelState.AddModelError("ThirdSupplierCode", errorMessage);
            }

            if (stationeryService.isPositiveLevel(level))
            {
                string errorMessage = String.Format("{0}  must be positive.", level);
                ModelState.AddModelError("ReorderLevel", errorMessage);
            }
            if (stationeryService.isPositiveQty(qty))
            {
                string errorMessage = String.Format("{0}  must be positive.", qty);
                ModelState.AddModelError("ReorderQty", errorMessage);
            }
            if (stationeryService.isPositivePrice(price))
            {
                string errorMessage = String.Format("{0}  must be positive.", price);
                ModelState.AddModelError("Price", errorMessage);
            }

            else if (ModelState.IsValid)
            {
                try
                {
                    if (stationeryService.UpdateStationeryInfo(stationeryVM))
                    {
                        TempData["SuccessMessage"] = String.Format("'{0}' has been updated", code);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = String.Format("There is not change to '{0}'.", code);
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.ExceptionMessage = e.Message;
                }
            }
            return View(stationeryVM);
        }

        // GET: Stationery/Create
        public ActionResult Create()
        {
            ViewBag.unitOfMeasure = stationeryService.GetAllUOMList();

            // Get select list for supplier
            List<SupplierViewModel> supplierList = supplierService.GetAllSuppliers();
            SelectList selectList = new SelectList(supplierList, "SupplierCode", "SupplierName");
            ViewBag.selectList = selectList;

            //Get selectedID list for category
            List<Category> categories = stationeryService.GetAllCategory();
            SelectList selectListID = new SelectList(categories, "categoryID", "categoryName");
            ViewBag.selectListofCategory = selectListID;

            // return View(new StationeryViewModel());
            return View();
        }

        // POST: Stationery/Create
        [HttpPost]
        public ActionResult Create(StationeryViewModel stationeryVM)
        {
            ViewBag.unitOfMeasure = stationeryService.GetAllUOMList();

            // Get select list for supplier
            List<SupplierViewModel> supplierList = supplierService.GetAllSuppliers();
            SelectList selectList = new SelectList(supplierList, "SupplierCode", "SupplierName");
            ViewBag.selectList = selectList;

            //Get selectedID list for category
            List<Category> categories = stationeryService.GetAllCategory();
            SelectList selectListID = new SelectList(categories, "categoryID", "categoryName");
            ViewBag.selectListofCategory = selectListID;

            string code = stationeryVM.ItemCode;
            int level = stationeryVM.ReorderLevel;
            int qty = stationeryVM.ReorderQty;
            decimal price = stationeryVM.Price;

            //for supplier validate
            string supplier1 = stationeryVM.FirstSupplierCode;
            string supplier2 = stationeryVM.SecondSupplierCode;
            string supplier3 = stationeryVM.ThirdSupplierCode;
            if (stationeryService.isExistingSupplierCode(supplier1,supplier2))
            {
                string errorMessage = String.Format("{0} has been used in First Supplier.",supplier1);
                ModelState.AddModelError("SecondSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingSupplierCode(supplier1, supplier3))
            {
                string errorMessage = String.Format("{0} has been used in First Supplier.", supplier1);
                ModelState.AddModelError("ThirdSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingSupplierCode(supplier2, supplier3))
            {
                string errorMessage = String.Format("{0} has been used in Second Supplier.", supplier2);
                ModelState.AddModelError("ThirdSupplierCode", errorMessage);
            }

            if (stationeryService.isExistingCode(code))
            {
                string errorMessage = String.Format("{0} has been used.", code);
                ModelState.AddModelError("ItemCode", errorMessage);
            }
            if (stationeryService.isPositiveLevel(level))
            {
                string errorMessage = String.Format("{0}  must be positive.", level);
                ModelState.AddModelError("ReorderLevel", errorMessage);
            }
            if (stationeryService.isPositiveQty(qty))
            {
                string errorMessage = String.Format("{0}  must be positive.", qty);
                ModelState.AddModelError("ReorderQty", errorMessage);
            }
            if (stationeryService.isPositivePrice(price))
            {
                string errorMessage = String.Format("{0}  must be positive.", price);
                ModelState.AddModelError("Price", errorMessage);
            }
            else if (ModelState.IsValid)
            {
                {
                    try
                    {
                        stationeryService.AddNewStationery(stationeryVM);
                        TempData["SuccessMessage"] = String.Format("Stationery '{0}' is added.", code);
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        TempData["ExceptionMessage"] = e.Message;
                    }
                }
            }

            return View(stationeryVM);
        }

        // GET: Stationery/Delete/{id}
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            if (stationeryService.DeleteStationery(id))
            {
                TempData["SuccessMessage"] = String.Format("Stationery '{0}' has been deleted", id);
            }
            else
            {
                TempData["ErrorMessage"] = String.Format("Cannot delete stationery '{0}'", id);
            }

            return RedirectToAction("Index");
        }


        // GET: Stationery/Details
        public ActionResult ViewStockCard(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            // TODO: REMOVE HARD CODED REQUESTER ID
            //string requesterID = HttpContext.User.Identity.Name;
            string userID = "S1017";

            // Store clerk roleID == 7
            int roleID = userService.GetRoleByID(userID);
            ViewBag.Role = (roleID.ToString() == "7") ? "StoreClerk" : "";


            ViewBag.SelectYear = new SelectList(transactionService.GetSelectableTransactionYear(DateTime.Today.Year));
            ViewBag.SelectMonth = new SelectList(transactionService.GetSelectableTransactionMonth(DateTime.Today.Month));

            return View(stationeryService.FindStationeryViewModelByItemCode(id));
        }


        public ActionResult ResetCatalogue()
        {
            return RedirectToAction("Index", new { searchString = "", categoryID = "All" });
        }

        // GET: Stationery/Browse
        //public ActionResult Browse()
        //{            
        //    ViewBag.CategoryList = stationeryService.GetAllCategory();

        //    return View();
        //}


        [HttpPost]
        public ActionResult ViewTransaction(string id, int selectedYear, int selectedMonth)
        {
            List<Transaction_Detail> records = transactionService.GetTransaciontDetailsByCriteria(selectedYear, selectedMonth, id);

            List<ItemTransactionRecordViewModel> vmList = new List<ItemTransactionRecordViewModel>();

            foreach (var r in records)
            {
                ItemTransactionRecordViewModel vm = new ItemTransactionRecordViewModel();

                vm.TransactionNo = r.transactionNo;
                vm.TransactionDate = (DateTime)r.Transaction_Record.date;
                vm.ItemCode = r.itemCode;
                vm.Quantity = r.adjustedQty;
                vm.BalanceQty = r.balanceQty;
                vm.TransactionType = r.Transaction_Record.type;
                vm.Remarks = r.remarks;

                vmList.Add(vm);                              
            }

            return PartialView("_ViewTransaction", vmList);
        }

    }
}