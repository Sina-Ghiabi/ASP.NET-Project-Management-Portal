using Abp.Application.Services;
using Abp.Domain.Repositories;
using Nexora.PMPortal.Financial.Dto;
using MD.PersianDateTime;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Financial
{
    public interface IGrossProfitToNetAppService : IAsyncCrudAppService<GrossProfitToNetDto, int>
    {
        Task<List<GrossProfitToNet>> GetAllGrossProfitToNets();
        Task<List<GrossProfitToNet>> GetGrossProfitToNetsWithYear(int yearfrom);
    }


    public class GrossProfitToNetAppService : AsyncCrudAppService<GrossProfitToNet, GrossProfitToNetDto, int>, IGrossProfitToNetAppService
    {
        public GrossProfitToNetAppService(IRepository<GrossProfitToNet, int> repository)
            : base(repository)
        {
        }


        public async Task<List<GrossProfitToNet>> GetAllGrossProfitToNets()
        {
            return await Repository.GetAll().OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<GrossProfitToNet>> GetGrossProfitToNetsWithYear(int yearfrom)
        {
            return await Repository.GetAll().OrderByDescending(a => a.Id).Where(a => new PersianDateTime(a.Year).Year >= yearfrom).ToListAsync();
        }

    }
}
