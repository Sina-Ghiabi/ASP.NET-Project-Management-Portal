using Abp.Application.Services;
using Abp.Domain.Repositories;
using Nexora.PMPortal.Managers.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Managers
{
    public interface IManagerReportAppService : IAsyncCrudAppService<ManagerReportDto, int>
    {
        Task<List<ManagerReport>> GetManagerReports();
    }


    public class ManagerReportAppService : AsyncCrudAppService<ManagerReport, ManagerReportDto, int>, IManagerReportAppService
    {
        public ManagerReportAppService(IRepository<ManagerReport, int> repository)
            : base(repository)
        {
        }

        public async Task<List<ManagerReport>> GetManagerReports()
        {
            return await Repository.GetAll().OrderByDescending(a => a.Id).ToListAsync();
        }


    }

}
