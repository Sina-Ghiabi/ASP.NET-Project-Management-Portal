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
    public interface ICostBenefitAppService : IAsyncCrudAppService<CostBenefitDto, int>
    {
        Task<List<CostBenefit>> GetAllCostBenefits();
        Task<List<CostBenefit>> GetProjectCostBenefits(int projectId);
        Task<List<CostBenefit>> GetYearCostBenefits(int yearfrom);
        Task<List<CostBenefit>> GetSpecificYearCostBenefits(int year);
    }


    public class CostBenefitAppService : AsyncCrudAppService<CostBenefit, CostBenefitDto, int>, ICostBenefitAppService
    {
        public CostBenefitAppService(IRepository<CostBenefit, int> repository)
            : base(repository)
        {
        }


        public async Task<List<CostBenefit>> GetAllCostBenefits()
        {
            return await Repository.GetAll().Include(a => a.Project).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<CostBenefit>> GetProjectCostBenefits(int projectId)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a => a.ProjectId == projectId).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<CostBenefit>> GetYearCostBenefits(int yearfrom)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a => new PersianDateTime(a.Year).Year >= yearfrom).ToListAsync();
        }

        public async Task<List<CostBenefit>> GetSpecificYearCostBenefits(int year)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a => new PersianDateTime(a.Year).Year == year).OrderByDescending(a => a.Id).ToListAsync();
        }

    }
}
