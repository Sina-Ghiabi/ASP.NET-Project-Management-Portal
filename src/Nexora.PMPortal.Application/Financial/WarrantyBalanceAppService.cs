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
    public interface IWarrantyBalanceAppService : IAsyncCrudAppService<WarrantyBalanceDto, int>
    {
        Task<List<WarrantyBalance>> GetAllWarrantyBalances();
        Task<List<WarrantyBalance>> GetProjectWarrantyBalances(int projectId);
        Task<List<WarrantyBalance>> GetCompanyWarrantyBalances(int yearFrom);
    }


    public class WarrantyBalanceAppService : AsyncCrudAppService<WarrantyBalance, WarrantyBalanceDto, int>, IWarrantyBalanceAppService
    {
        public WarrantyBalanceAppService(IRepository<WarrantyBalance, int> repository)
            : base(repository)
        {
        }


        public async Task<List<WarrantyBalance>> GetAllWarrantyBalances()
        {
            return await Repository.GetAll().Include(a => a.Project).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<WarrantyBalance>> GetProjectWarrantyBalances(int projectId)
        {
            return await Repository.GetAll().Where(a => a.ProjectId == projectId).Include(a => a.Project).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<WarrantyBalance>> GetCompanyWarrantyBalances(int yearFrom)
        {
            return await Repository.GetAll().Where(a => new PersianDateTime(a.Year).Year >= yearFrom).Include(a => a.Project).OrderBy(a => new PersianDateTime(a.Year).Year).ToListAsync();
        }

    }
}
