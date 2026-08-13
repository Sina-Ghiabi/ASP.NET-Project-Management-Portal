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
    public interface IContractorAppService : IAsyncCrudAppService<ContractorDto, int>
    {
        Task<List<Contractor>> GetAllContractors();
    }


    public class ContractorAppService : AsyncCrudAppService<Contractor, ContractorDto, int>, IContractorAppService
    {
        public ContractorAppService(IRepository<Contractor, int> repository)
            : base(repository)
        {
        }


        public async Task<List<Contractor>> GetAllContractors()
        {
            return await Repository.GetAll().OrderByDescending(a => a.Id).ToListAsync();
        }

    }
}
