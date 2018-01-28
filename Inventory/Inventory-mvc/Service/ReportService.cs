﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.DAO;
using System.Web.Mvc;
using Inventory_mvc.Models;
using Inventory_mvc.ViewModel;
using Inventory_mvc.Utilities;

namespace Inventory_mvc.Service
{
    public class ReportService : IReportService
    {
        IReportDAO reportDAO = new ReportDAO();
        IStationeryService stationeryService = new StationeryService();
        IUserService userService = new UserService();
        IRequisitionRecordService requisitionService = new RequisitionRecordService();

        public List<ReportViewModel> GetItemRequestTrend(string itemCode, int[] years)
        {
            List<Requisition_Detail> details = reportDAO.GetRequisitionDetailsByItemCodeAndYear(itemCode, years);

            List<ReportViewModel> vmList = new List<ReportViewModel>();

            foreach (var d in details)
            {
                // Get only approved and processed record
                string status = d.Requisition_Record.status;
                if(status == RequisitionStatus.PENDING_APPROVAL || status == RequisitionStatus.REJECTED)
                {
                    continue; // skip
                }
                vmList.Add(ConvertToReportViewModel(d));
            }

            return vmList;
        }


        //public List<ReportViewModel> RetrieveQty(DateTime ds,DateTime de)
        //{
        //    List<Purchase_Detail> details = reportDAO.RetrieveQty(ds,de);

        //    List<ReportViewModel> vmList = new List<ReportViewModel>();
        //    foreach (var d in details)
        //    {
        //        Stationery s = stationeryService.FindStationeryByItemCode(d.itemCode);

        //        ReportViewModel vm = new ReportViewModel();             
        //        vm.ItemCode = d.itemCode;
        //        vm.RequestQuantity = d.qty;
        //        vm.CategoryName = s.Category.categoryName;

        //        vmList.Add(vm);
        //    }
        //    return vmList;
        //}


        private ReportViewModel ConvertToReportViewModel(Requisition_Detail detail)
        {
            Stationery stationery = stationeryService.FindStationeryByItemCode(detail.itemCode);
            Requisition_Record record = detail.Requisition_Record;
            //Requisition_Record record = requisitionService.GetRequisitionByID(detail.requisitionNo);
            
            ReportViewModel vm = new ReportViewModel();
            vm.CategoryName = stationery.Category.categoryName;
            vm.Cost = stationery.price;
            vm.ItemCode = detail.itemCode;
            vm.ItemDescription = stationery.description;
            vm.Month = record.requestDate.Value.Month;
            vm.RequestQuantity = (detail.qty == null) ? 0 : (int)detail.qty;
            vm.RequesterDepartment = userService.FindDeptCodeByID(record.requesterID);
            vm.Year = record.requestDate.Value.Year;
            vm.Status = record.status;

            return vm;
        }

        public void GenerateRandomDataForRequisitionRecords()
        {
            List<User> users = new List<User>();
            users.AddRange(userService.FindUsersByRole(3));
            users.AddRange(userService.FindUsersByRole(4));
            users.AddRange(userService.FindUsersByRole(7));
            Random userR = new Random(50);

            List<string> items = stationeryService.GetListOfItemCodes();
            Random stationeryR = new Random(888);

            string[] status = { RequisitionStatus.PENDING_APPROVAL, RequisitionStatus.APPROVED_PROCESSING, RequisitionStatus.REJECTED, RequisitionStatus.PARTIALLY_FULFILLED, RequisitionStatus.COLLECTED };
            Random statusR = new Random(75);

            Random detailR = new Random(16);
            Random quantityR = new Random(115);
            Random dailyR = new Random(369);

            Random dayR = new Random(4568);

            var startTime = DateTime.Parse("2017-01-01");
            var endTime = startTime.AddYears(1);
            endTime = endTime.AddMonths(1);

            while (startTime <= endTime)
            {
                int no_of_record = dailyR.Next(8, 15);

                for(int i = 0; i <= no_of_record; i++)
                {
                    int index = userR.Next(users.Count);
                    string requesterID = users.ElementAt(index).userID;

                    Requisition_Record record = new Requisition_Record();
                    record.status = status[statusR.Next(status.Length)];
                    record.Requisition_Detail = new List<Requisition_Detail>();

                    int number_of_detail = detailR.Next(1, 11);
                    for(int j = 0; j < number_of_detail; j++)
                    {
                        Requisition_Detail d = new Requisition_Detail();
                        d.itemCode = items.ElementAt(stationeryR.Next(items.Count));
                        d.qty = quantityR.Next(1, 10);
                        d.allocatedQty = 0;
                        d.fulfilledQty = 0;

                        bool contain = false;

                        foreach (var item in record.Requisition_Detail)
                        {
                            if (item.itemCode == d.itemCode)
                            {
                                item.qty += d.qty;
                                contain = true;
                                break;
                            }
                        }

                        if (!contain)
                        {
                            record.Requisition_Detail.Add(d);
                        }

                    }

                    requisitionService.GenerateRandomRequisition(record, requesterID, startTime);
                }
                int day = dayR.Next(1, 4);
                startTime = startTime.AddDays(day);
            }
        }

        public List<int> GetSelectableYears(int baseYear)
        {
            List<int> years = new List<int>();
            int thisYear = DateTime.Today.Year;

            for(int i = baseYear; i <= thisYear; i++)
            {
                years.Add(i);
            }

            return years;
        }
    }
}