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

    public interface IProject_User_MappingAppService : IAsyncCrudAppService<Project_User_MappingDto, int>
    {
        Task<List<Project_User_Mapping>> GetUserProjects(long userId);
        Task<bool> IsDuplicate(long userId, int projectId);
    }


    public class Project_User_MappingAppService : AsyncCrudAppService<Project_User_Mapping, Project_User_MappingDto, int>, IProject_User_MappingAppService
    {
        public Project_User_MappingAppService(IRepository<Project_User_Mapping, int> repository)
            : base(repository)
        {
        }

        public async Task<List<Project_User_Mapping>> GetUserProjects(long userId)
        {
            return await Repository.GetAll().Where(a => a.UserId == userId).Include(a => a.Project).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<bool> IsDuplicate(long userId,int projectId)
        {
            return await Repository.GetAll().AnyAsync(a => a.UserId == userId && a.ProjectId == projectId);
        }


    }

}
