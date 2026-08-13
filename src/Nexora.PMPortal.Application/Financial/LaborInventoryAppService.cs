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
    public interface ILaborInventoryAppService : IAsyncCrudAppService<LaborInventoryDto, int>
    {
        Task<List<LaborInventory>> GetAllLaborInventories();
        Task<List<LaborInventory>> GetProjectLaborInventories(int projectId);
        Task<List<LaborInventory>> GetCompanyLaborInventories(int yearfrom);
        Task<List<LaborInventory>> GetYearCompanyLaborInventories(int year);

    }


    public class LaborInventoryAppService : AsyncCrudAppService<LaborInventory, LaborInventoryDto, int>, ILaborInventoryAppService
    {
        public LaborInventoryAppService(IRepository<LaborInventory, int> repository)
            : base(repository)
        {
        }


        public async Task<List<LaborInventory>> GetAllLaborInventories()
        {
            return await Repository.GetAll().Include(a => a.Project).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<LaborInventory>> GetProjectLaborInventories(int projectId)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a => a.ProjectId == projectId).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<LaborInventory>> GetCompanyLaborInventories(int yearfrom)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a => new PersianDateTime(a.Year).Year >= yearfrom).OrderBy(a => new PersianDateTime(a.Year).Year).ToListAsync();
        }

        public async Task<List<LaborInventory>> GetYearCompanyLaborInventories(int year)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a => new PersianDateTime(a.Year).Year == year).ToListAsync();
        }


    }
}
