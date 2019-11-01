using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LACoreApp.Application.Dapper.ViewModels;

namespace LACoreApp.Application.Dapper.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate);
    }
}
