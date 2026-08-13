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
    public interface IProjectReportAppService : IAsyncCrudAppService<ProjectReportDto, int>
    {
        Task<List<ProjectReport>> GetProjectReports(int projectId);
    }


    public class ProjectReportAppService : AsyncCrudAppService<ProjectReport, ProjectReportDto, int>, IProjectReportAppService
    {
        public ProjectReportAppService(IRepository<ProjectReport, int> repository)
            : base(repository)
        {
        }

        public async Task<List<ProjectReport>> GetProjectReports(int projectId)
        {
            return await Repository.GetAll().Where(a => a.ProjectId == projectId).OrderByDescending(a => a.Id).ToListAsync();
        }


    }

}
