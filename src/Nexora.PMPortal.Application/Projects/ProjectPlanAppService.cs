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
    public interface IProjectPlanAppService : IAsyncCrudAppService<ProjectPlanDto, int>
    {
        Task<List<ProjectPlan>> GetProjectPlan(int projectId);
    }


    public class ProjectPlanAppService : AsyncCrudAppService<ProjectPlan, ProjectPlanDto, int>, IProjectPlanAppService
    {
        public ProjectPlanAppService(IRepository<ProjectPlan, int> repository)
            : base(repository)
        {
        }

        public async Task<List<ProjectPlan>> GetProjectPlan(int projectId)
        {
            return await Repository.GetAll().Where(a => a.ProjectId == projectId).OrderByDescending(a => a.Id).ToListAsync();
        }

    }

}
