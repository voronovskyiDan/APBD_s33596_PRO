using Application.DTOs.Revenue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRevenueService
    {
        Task<RevenueDto> GetCurrentRevenueAsync(int? softwareProductId = null);
        Task<RevenueDto> GetPredictedRevenueAsync(int? softwareProductId = null);
    }
}
