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
    public interface IProgressPercentageAppService : IAsyncCrudAppService<ProgressPercentageDto, int>
    {
        Task<List<ProgressPercentage>> GetAllProgressPercentages();
        Task<List<ProgressPercentage>> GetAllUsersProgressPercentages(long UserID);
        Task<List<ProgressPercentage>> GetProjectProgressPercentages(int projectId);
        Task<List<ProgressPercentage>> GetUsersProjectProgressPercentages(int[] projectId);
    }


    public class ProgressPercentageAppService : AsyncCrudAppService<ProgressPercentage, ProgressPercentageDto, int>, IProgressPercentageAppService
    {
        public ProgressPercentageAppService(IRepository<ProgressPercentage, int> repository)
            : base(repository)
        {
        }


        public async Task<List<ProgressPercentage>> GetAllProgressPercentages()
        {
            return await Repository.GetAll().Include(a => a.Project).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<ProgressPercentage>> GetAllUsersProgressPercentages(long UserID)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a=>a.CreatorUserId == UserID).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<ProgressPercentage>> GetProjectProgressPercentages(int projectId)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a => a.ProjectId == projectId).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<ProgressPercentage>> GetUsersProjectProgressPercentages(int[] projectId)
        {
            var Result = new List<ProgressPercentage>();

            foreach (var Item in projectId)
            {
                Result = await Repository.GetAll().Include(a => a.Project).Where(a => a.ProjectId == Item).OrderByDescending(a => a.Id).ToListAsync();
            }

            return Result;
        }

    }
}
