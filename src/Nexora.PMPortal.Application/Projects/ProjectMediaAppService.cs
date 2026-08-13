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
    public interface IProjectMediaAppService : IAsyncCrudAppService<ProjectMediaDto, int>
    {
        Task<List<ProjectMedia>> GetProjectMedias(int projectId);
    }


    public class ProjectMediaAppService : AsyncCrudAppService<ProjectMedia, ProjectMediaDto, int>, IProjectMediaAppService
    {
        public ProjectMediaAppService(IRepository<ProjectMedia, int> repository)
            : base(repository)
        {
        }

        public async Task<List<ProjectMedia>> GetProjectMedias(int projectId)
        {
            return await Repository.GetAll().Where(a => a.ProjectId == projectId).OrderByDescending(a => a.Id).ToListAsync();
        }


    }

}
