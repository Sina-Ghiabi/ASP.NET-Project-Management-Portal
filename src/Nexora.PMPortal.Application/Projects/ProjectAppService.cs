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
    public interface IProjectAppService : IAsyncCrudAppService<ProjectDto, int>
    {
        Task<List<Project>> GetAllProjects();
        Task<List<Project>> GetProjectIDByProjectCode(string ProjectCode);
        Task<List<Project>> GetAllUsersProjects(long UserID);
        Task<List<Project>> GetProjectByID(int ProjectID);
        Task<List<Project>> GetProjectByProjectCode(string ProjectCode);
    }


    public class ProjectAppService : AsyncCrudAppService<Project, ProjectDto, int>, IProjectAppService
    {
        public ProjectAppService(IRepository<Project, int> repository)
            : base(repository)
        {
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await Repository.GetAll().OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<Project>> GetProjectIDByProjectCode(string ProjectCode)
        {
            return await Repository.GetAll().Where(a => a.Code == ProjectCode).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<Project>> GetAllUsersProjects(long UserID)
        {
            return await Repository.GetAll().Where(a=>a.CreatorUserId == UserID).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<Project>> GetProjectByID(int ProjectID)
        {
            return await Repository.GetAll().Where(a => a.Id == ProjectID).OrderByDescending(a => a.Id).ToListAsync();
        }
        public async Task<List<Project>> GetProjectByProjectCode(string ProjectCode)
        {
            return await Repository.GetAll().Where(a => a.Code == ProjectCode).OrderByDescending(a => a.Id).ToListAsync();
        }

    }

}
