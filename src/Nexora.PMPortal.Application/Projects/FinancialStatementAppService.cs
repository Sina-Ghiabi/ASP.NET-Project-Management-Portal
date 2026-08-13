using Abp.Application.Services;
using Abp.Domain.Repositories;
using Nexora.PMPortal.Projects.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Projects
{
    public interface IFinancialStatementAppService : IAsyncCrudAppService<FinancialStatementDto, int>
    {
        Task<List<FinancialStatement>> GetProjectFinancialStatements(int projectId);
    }


    public class FinancialStatementAppService : AsyncCrudAppService<FinancialStatement, FinancialStatementDto, int>, IFinancialStatementAppService
    {
        public FinancialStatementAppService(IRepository<FinancialStatement, int> repository)
            : base(repository)
        {
        }

        public async Task<List<FinancialStatement>> GetProjectFinancialStatements(int projectId)
        {
            return await Repository.GetAll().Where(a => a.ProjectId == projectId).OrderByDescending(a => a.Id).ToListAsync();
        }


    }

}
