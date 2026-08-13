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
    public interface IReceiveAndPaymentAppService : IAsyncCrudAppService<ReceiveAndPaymentDto, int>
    {
        Task<List<ReceiveAndPayment>> GetAllItems();
        Task<List<ReceiveAndPayment>> GetProjectItems(int projectId);
    }


    public class ReceiveAndPaymentAppService : AsyncCrudAppService<ReceiveAndPayment, ReceiveAndPaymentDto, int>, IReceiveAndPaymentAppService
    {
        public ReceiveAndPaymentAppService(IRepository<ReceiveAndPayment, int> repository)
            : base(repository)
        {
        }


        public async Task<List<ReceiveAndPayment>> GetAllItems()
        {
            return await Repository.GetAll().Include(a => a.Project).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<ReceiveAndPayment>> GetProjectItems(int projectId)
        {
            return await Repository.GetAll().Where(a => a.ProjectId == projectId).Include(a => a.Project).OrderByDescending(a => a.Id).ToListAsync();
        }

    }
}
