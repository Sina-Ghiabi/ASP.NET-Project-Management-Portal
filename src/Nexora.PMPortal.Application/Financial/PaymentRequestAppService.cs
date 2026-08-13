using Abp.Application.Services;
using Abp.Domain.Repositories;
using Nexora.PMPortal.Financial.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Financial
{
    public interface IPaymentRequestAppService : IAsyncCrudAppService<PaymentRequestDto, int>
    {
        Task<List<PaymentRequest>> GetAllPaymentRequests();
        Task<PaymentRequest> GetWillDetails(int id);
        Task<bool> DeletePaymentRequest(int id);

        Task<Tuple<List<PaymentRequest>, int, decimal, decimal, decimal>> GetPaymentRequestsPage(int paymentRequestStatus, int? id, int[] projectId, int[] currencyId,
           int[] paymentRequestType, int? page, int? pageSize, DateTime? from, DateTime? to, DateTime? chequeDueDate, string paymentRequestCode,
            string payto, string fourthLevelCode, List<int> projectIds, int[] paymentType, int? sortType = 1);
    }


    public class PaymentRequestAppService : AsyncCrudAppService<PaymentRequest, PaymentRequestDto, int>, IPaymentRequestAppService
    {
        public PaymentRequestAppService(IRepository<PaymentRequest, int> repository)
            : base(repository)
        {
        }

        public async Task<List<PaymentRequest>> GetAllPaymentRequests()
        {
            return await Repository.GetAll().Include(a => a.Project).Include(a => a.Payments).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<PaymentRequest> GetWillDetails(int id)
        {
            return await Repository.GetAll().Include(a => a.Project).Include(a => a.Payments).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Tuple<List<PaymentRequest>, int, decimal, decimal, decimal>> GetPaymentRequestsPage(int paymentRequestStatus, int? id, int[] projectId, int[] currencyId,
            int[] paymentRequestType, int? page, int? pageSize, DateTime? from, DateTime? to, DateTime? chequeDueDate, string paymentRequestCode,
            string payto, string fourthLevelCode, List<int> projectIds, int[] paymentType, int? sortType = 1)
        {
            var items = new List<PaymentRequest>();
            decimal totalAmount = 0;
            decimal paidAmount = 0;
            decimal totalCredit = 0;
            int total = 0;

            var queryable = Repository.GetAll()
                .Include(a => a.Project)
                .Include(a => a.Payments)
                .Where(a => projectIds.Contains(a.ProjectId))
                .OrderByDescending(a => a.CreationTime)
                .AsQueryable();

            queryable = queryable.Where(a => a.PaymentRequestStatus == (Enums.PaymentRequestStatus)paymentRequestStatus);

            if (id.HasValue) queryable = queryable.Where(a => a.Id == id.Value);

            if (projectId.Length > 0) queryable = queryable.Where(a => projectId.Contains(a.ProjectId));

            if (currencyId.Length > 0) queryable = queryable.Where(a => currencyId.Contains((int)a.CurrencyType));

            if (paymentRequestType.Length > 0) queryable = queryable.Where(a => paymentRequestType.Contains((int)a.PaymentRequestType));

            if (sortType == 1) queryable = queryable.OrderByDescending(a => a.CreationTime);

            if (sortType == 2) queryable = queryable.OrderBy(a => a.CreationTime);

            if (sortType == 3) queryable = queryable.OrderByDescending(a => a.Amount);

            if (sortType == 4) queryable = queryable.OrderBy(a => a.Amount);

            if (from.HasValue) queryable = queryable.Where(a => a.CreationTime.Date >= from.Value.Date);

            if (to.HasValue) queryable = queryable.Where(a => a.CreationTime.Date <= to.Value.Date);

            if (chequeDueDate.HasValue) queryable = queryable.Where(a => a.ChequeDueDate.Value.Date == chequeDueDate.Value.Date);

            if (payto != null) queryable = queryable.Where(a => a.PayTo.Contains(payto.Trim()));

            if (paymentType.Length > 0) queryable = queryable.Where(a => paymentType.Contains((int)a.PaymentType));

            if (fourthLevelCode != null)
            {
                if (fourthLevelCode == "-")
                {
                    queryable = queryable.Where(a => a.FourthLevelCode == null);
                }
                else
                {
                    queryable = queryable.Where(a => a.FourthLevelCode.Contains(fourthLevelCode.Trim()));
                }
            }

            if (paymentRequestCode != null) queryable = queryable.Where(a => a.PaymentRequestCode.Contains(paymentRequestCode.Trim()));

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
                paidAmount = await queryable.SumAsync(a => a.Payments.Sum(b => b.Amount));
                totalCredit = await queryable.SumAsync(a => a.CreditBalance.Value);
            }


            return new Tuple<List<PaymentRequest>, int, decimal, decimal, decimal>(items, total, totalAmount, paidAmount, totalCredit);
        }

        public async Task<bool> DeletePaymentRequest(int id)
        {
            var item = await Repository.GetAll().Include(a => a.Payments).FirstOrDefaultAsync(a => a.Id == id);
            if (item.Payments.Count == 0)
            {
                await Repository.DeleteAsync(id);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
