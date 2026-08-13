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
    public interface IReceiveBillAppService : IAsyncCrudAppService<ReceiveBillDto, int>
    {
        Task<List<ReceiveBill>> GetAllReceiveBillsForUsageReport(int? yearId);
        Task<bool> DeleteReceiveBill(int id);
        Task<Tuple<List<ReceiveBill>, int, decimal, decimal>> GetReceiveBillsPage(List<int> projectIds, int? id, int[] projectId, int[] currencyId, int[] receiveBillType, int? page, int? pageSize, DateTime? from, DateTime? to, int? sortType = 1);
        Task<List<ReceiveBill>> GetProjectsReceiveBills(List<int> projectIds, int currencyId, int year, int month);
    }


    public class ReceiveBillAppService : AsyncCrudAppService<ReceiveBill, ReceiveBillDto, int>, IReceiveBillAppService
    {
        public ReceiveBillAppService(IRepository<ReceiveBill, int> repository)
            : base(repository)
        {
        }

        public async Task<List<ReceiveBill>> GetAllReceiveBillsForUsageReport(int? yearId)
        {
            var queryable = Repository.GetAll().Include(a => a.Project).Include(a => a.Assignemnts).Where(a => a.ReceiveBillType == Enums.ReceiveBillType.Cash);
            if (yearId.HasValue) queryable = queryable.Where(a => new PersianDateTime(a.ReceiveDate).Year == yearId);
            return await queryable.OrderByDescending(a => a.Id).ToListAsync();
        }


        public async Task<Tuple<List<ReceiveBill>, int, decimal, decimal>> GetReceiveBillsPage(List<int> projectIds, int? id, int[] projectId, int[] currencyId, int[] receiveBillType,
            int? page, int? pageSize, DateTime? from, DateTime? to, int? sortType = 1)
        {
            var queryable = Repository.GetAll()
                .Include(a => a.Project)
                .Include(a => a.Assignemnts)
                .Where(a => projectIds.Contains(a.ProjectId))
                .OrderByDescending(a => a.CreationTime)
                .AsQueryable();

            var items = new List<ReceiveBill>();
            decimal totalAmount = 0;
            decimal assignedAmount = 0;
            int total = 0;

            if (id.HasValue) queryable = queryable.Where(a => a.Id == id.Value);

            if (projectId.Length > 0) queryable = queryable.Where(a => projectId.Contains(a.ProjectId));

            if (currencyId.Length > 0) queryable = queryable.Where(a => currencyId.Contains((int)a.CurrencyType));

            if (receiveBillType.Length > 0) queryable = queryable.Where(a => receiveBillType.Contains((int)a.ReceiveBillType));

            if (sortType == 1) queryable = queryable.OrderByDescending(a => a.CreationTime);

            if (sortType == 2) queryable = queryable.OrderBy(a => a.CreationTime);

            if (sortType == 3) queryable = queryable.OrderByDescending(a => a.Amount);

            if (sortType == 4) queryable = queryable.OrderBy(a => a.Amount);

            if (from.HasValue) queryable = queryable.Where(a => a.ReceiveDate >= from.Value.Date);

            if (to.HasValue) queryable = queryable.Where(a => a.ReceiveDate <= to.Value.Date);

            if (pageSize != null && page != null)
            {
                items = await queryable.Skip(pageSize.Value * (page.Value - 1)).Take(pageSize.Value).ToListAsync();
                total = await queryable.CountAsync();
            }
            else
            {
                items = await queryable.ToListAsync();
                total = await queryable.CountAsync();
                totalAmount = await queryable.SumAsync(a => a.Amount);
                assignedAmount = await queryable.SumAsync(a => a.Assignemnts.Sum(b => b.Amount));
            }

            return new Tuple<List<ReceiveBill>, int, decimal, decimal>(items, total, totalAmount, assignedAmount);
        }


        public async Task<bool> DeleteReceiveBill(int id)
        {
            var item = await Repository.GetAll().Include(a => a.Assignemnts).FirstOrDefaultAsync(a => a.Id == id);
            if (item.Assignemnts != null && item.Assignemnts.Count == 0)
            {
                await Repository.DeleteAsync(id);
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<List<ReceiveBill>> GetProjectsReceiveBills(List<int> projectIds, int currencyId, int year, int month)
        {
            var queryable = Repository.GetAll()
                .Include(a => a.Project)
                .Include(a => a.Assignemnts)
                .Where(a => a.ReceiveBillType == Enums.ReceiveBillType.Cash
                && projectIds.Contains(a.ProjectId)
                && a.CurrencyType == (Enums.CurrencyType)currencyId);

            if (year != 0)
            {
                queryable = queryable.Where(a => new PersianDateTime(a.CreationTime).Year == year);
            }

            if (month != 0)
            {
                queryable = queryable.Where(a => new PersianDateTime(a.CreationTime).Month == month);
            }
            return await queryable.OrderByDescending(a => a.Id).ToListAsync();
        }

    }
}
