using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using SalesManagementApp.Data;
using SalesManagementApp.Entities;
using SalesManagementApp.Extensions;
using SalesManagementApp.Models.ReportModels;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services
{
    public class SalesOrderReportService : ISalesOrderReportService
    {
        private readonly SalesManagementDbContext _salesManagementDbContext;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public SalesOrderReportService(SalesManagementDbContext salesManagementDbContext, 
                                        AuthenticationStateProvider authenticationStateProvider)
        {
            _salesManagementDbContext = salesManagementDbContext;
            _authenticationStateProvider = authenticationStateProvider;
        }

        // SR
        public async Task<List<GroupedFieldPriceModel>> GetEmployeePricePerMonthData()
        {
            try
            {
                var employee = await GetLoggedOnEmployee();

                var reportData = await (from s in _salesManagementDbContext.SalesOrderReports
                                        where s.EmployeeId == 9
                                        group s by s.OrderDateTime.Month into GroupedData
                                        orderby GroupedData.Key
                                        select new GroupedFieldPriceModel
                                        {
                                            GroupedFieldKey = (
                                                GroupedData.Key == 1 ? "Jan" :
                                                GroupedData.Key == 2 ? "Feb" :
                                                GroupedData.Key == 3 ? "Mar" :
                                                GroupedData.Key == 4 ? "Apr" :
                                                GroupedData.Key == 5 ? "May" :
                                                GroupedData.Key == 6 ? "Jun" :
                                                GroupedData.Key == 7 ? "Jul" :
                                                GroupedData.Key == 8 ? "Aug" :
                                                GroupedData.Key == 9 ? "Sep" :
                                                GroupedData.Key == 10 ? "Oct" :
                                                GroupedData.Key == 11 ? "Nov" :
                                                GroupedData.Key == 12 ? "Dec" : ""                                              
                                            ),
                                            Price = Math.Round(GroupedData.Sum(o => o.OrderItemPrice), 2)
                                        }).ToListAsync();
                return reportData;
            }
            catch (Exception)
            {

                throw;
            }
        }    
        public async Task<List<GroupedFieldQtyModel>> GetQtyPerMonthData()
        {
            try
            {
                var employee = await GetLoggedOnEmployee();
                var reportData = await(from s in _salesManagementDbContext.SalesOrderReports
                                       where s.EmployeeId == employee.Id
                                       group s by s.OrderDateTime.Month into GroupedData
                                       orderby GroupedData.Key
                                       select new GroupedFieldQtyModel
                                       {
                                           GroupedFieldKey = (
                                               GroupedData.Key == 1 ? "Jan" :
                                               GroupedData.Key == 2 ? "Feb" :
                                               GroupedData.Key == 3 ? "Mar" :
                                               GroupedData.Key == 4 ? "Apr" :
                                               GroupedData.Key == 5 ? "May" :
                                               GroupedData.Key == 6 ? "Jun" :
                                               GroupedData.Key == 7 ? "Jul" :
                                               GroupedData.Key == 8 ? "Aug" :
                                               GroupedData.Key == 9 ? "Sep" :
                                               GroupedData.Key == 10 ? "Oct" :
                                               GroupedData.Key == 11 ? "Nov" :
                                               GroupedData.Key == 12 ? "Dec" : ""
                                           ),
                                           Quantity = GroupedData.Sum(o => o.OrderItemQuantity)
                                       }).ToListAsync();
                return reportData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<GroupedFieldQtyModel>> GetQtyPerProductCategory()
        {
            try
            {
                var employee = await GetLoggedOnEmployee();
                var reportData = await (from s in _salesManagementDbContext.SalesOrderReports
                                        where s.EmployeeId == employee.Id
                                        group s by s.ProductCategoryName into GroupedData
                                        orderby GroupedData.Key
                                        select new GroupedFieldQtyModel
                                        {
                                            GroupedFieldKey = GroupedData.Key,
                                            Quantity = GroupedData.Sum(oi => oi.OrderItemQuantity)
                                        }).ToListAsync();
                return reportData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // TL
        public async Task<List<GroupedFieldPriceModel>> GetGrossSalesPerTeamMemberData()
        {
            try
            {
                var employee = await GetLoggedOnEmployee();
                List<int> teamMemberIds = await GetTeamMemberIds(employee.Id);

                var reportData = await (from s in _salesManagementDbContext.SalesOrderReports
                                        where teamMemberIds.Contains(s.EmployeeId)
                                        group s by s.EmployeeFirstName into GroupedData
                                        orderby GroupedData.Key
                                        select new GroupedFieldPriceModel
                                        {
                                            GroupedFieldKey = GroupedData.Key,
                                            Price = Math.Round(GroupedData.Sum(oi => oi.OrderItemPrice),2)
                                        }).ToListAsync();

                return reportData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<GroupedFieldQtyModel>> GetQtyPerTeamMemberData()
        {
            try
            {
                var employee = await GetLoggedOnEmployee();
                List<int> teamMemberIds = await GetTeamMemberIds(employee.Id);
                var reportData = await (from s in _salesManagementDbContext.SalesOrderReports
                                        where teamMemberIds.Contains(s.EmployeeId)
                                        group s by s.EmployeeFirstName into GroupedData
                                        orderby GroupedData.Key
                                        select new GroupedFieldQtyModel
                                        {
                                            GroupedFieldKey = GroupedData.Key,
                                            Quantity = GroupedData.Sum(oi => oi.OrderItemQuantity)
                                        }).ToListAsync();

                return reportData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<GroupedFieldQtyModel>> GetTeamQtyPerMonthData()
        {
            var employee = await GetLoggedOnEmployee();
            List<int> teamMemberIds = await GetTeamMemberIds(employee.Id);
            var reportData = await(from s in _salesManagementDbContext.SalesOrderReports
                                   where teamMemberIds.Contains(s.EmployeeId)
                                   group s by s.OrderDateTime.Month into GroupedData
                                   orderby GroupedData.Key
                                   select new GroupedFieldQtyModel
                                   {
                                       GroupedFieldKey = (
                                           GroupedData.Key == 1 ? "Jan" :
                                           GroupedData.Key == 2 ? "Feb" :
                                           GroupedData.Key == 3 ? "Mar" :
                                           GroupedData.Key == 4 ? "Apr" :
                                           GroupedData.Key == 5 ? "May" :
                                           GroupedData.Key == 6 ? "Jun" :
                                           GroupedData.Key == 7 ? "Jul" :
                                           GroupedData.Key == 8 ? "Aug" :
                                           GroupedData.Key == 9 ? "Sep" :
                                           GroupedData.Key == 10 ? "Oct" :
                                           GroupedData.Key == 11 ? "Nov" :
                                           GroupedData.Key == 12 ? "Dec" : ""
                                       ),
                                       Quantity = GroupedData.Sum(o => o.OrderItemQuantity)
                                   }).ToListAsync();
            return reportData;
        }

        public async Task<List<LocationProductCategoryModel>> GetQtyLocationProductCatData()
        {
            try
            {
                var reportData = await (from s in _salesManagementDbContext.SalesOrderReports
                                        group s by s.RetailOutletLocation into GroupedData
                                        orderby GroupedData.Key
                                        select new LocationProductCategoryModel
                                        {
                                            Location = GroupedData.Key,
                                            MountainBikes = GroupedData.Where(p => p.ProductCategoryId == 1).Sum(o => o.OrderItemQuantity),
                                            RoadBikes = GroupedData.Where(p => p.ProductCategoryId == 2).Sum(o => o.OrderItemQuantity),
                                            Camping = GroupedData.Where(p => p.ProductCategoryId == 3).Sum(o => o.OrderItemQuantity),
                                            Hiking = GroupedData.Where(p => p.ProductCategoryId == 4).Sum(o => o.OrderItemQuantity),
                                            Boots = GroupedData.Where(p => p.ProductCategoryId == 5).Sum(o => o.OrderItemQuantity)
                                        }).ToListAsync();

                return reportData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<GroupedFieldQtyModel>> GetQtyPerLocationData()
        {
            try
            {
                var reportData = await (from s in _salesManagementDbContext.SalesOrderReports
                                        group s by s.RetailOutletLocation into GroupData
                                        orderby GroupData.Key
                                        select new GroupedFieldQtyModel
                                        {
                                            GroupedFieldKey = GroupData.Key,
                                            Quantity = GroupData.Sum(o => o.OrderItemQuantity)
                                        }).ToListAsync();

                return reportData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<MonthLocationModel>> GetQtyPerMonthLocationData()
        {
            var reportData = await (from s in _salesManagementDbContext.SalesOrderReports
                                    group s by s.OrderDateTime.Month into GroupedData
                                    orderby GroupedData.Key
                                    select new MonthLocationModel
                                    {
                                        Month = (
                                        GroupedData.Key == 1 ? "Jan" :
                                                GroupedData.Key == 2 ? "Feb" :
                                                GroupedData.Key == 3 ? "Mar" :
                                                GroupedData.Key == 4 ? "Apr" :
                                                GroupedData.Key == 5 ? "May" :
                                                GroupedData.Key == 6 ? "Jun" :
                                                GroupedData.Key == 7 ? "Jul" :
                                                GroupedData.Key == 8 ? "Aug" :
                                                GroupedData.Key == 9 ? "Sep" :
                                                GroupedData.Key == 10 ? "Oct" :
                                                GroupedData.Key == 11 ? "Nov" :
                                                GroupedData.Key == 12 ? "Dec" : ""
                                        ),
                                        TX = GroupedData.Where(l => l.RetailOutletLocation == "TX").Sum(o => o.OrderItemQuantity),
                                        CA= GroupedData.Where(l => l.RetailOutletLocation == "CA").Sum(o => o.OrderItemQuantity),
                                        NY = GroupedData.Where(l => l.RetailOutletLocation == "NY").Sum(o => o.OrderItemQuantity),
                                        WA = GroupedData.Where(l => l.RetailOutletLocation == "WA").Sum(o => o.OrderItemQuantity),
                                    }).ToListAsync();
            return reportData;
        }


        private async Task<List<int>> GetTeamMemberIds(int teamLeadId)
        {
            List<int> teamMemberIds = await _salesManagementDbContext.Employees
                                      .Where(e => e.ReportToEmpId == teamLeadId)
                                      .Select(e => e.Id).ToListAsync();

            return teamMemberIds;
        }

        private async Task<Employee> GetLoggedOnEmployee()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return await user.GetEmployeeObject(_salesManagementDbContext);
        }
    }
}
