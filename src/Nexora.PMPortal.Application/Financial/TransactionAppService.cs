using Abp.Application.Services;
using Abp.Domain.Repositories;
using Nexora.PMPortal.Enums;
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
    public interface ITransactionAppService : IAsyncCrudAppService<TransactionDto, int>
    {
        Task<List<Transaction>> GetAllTransactionsForUsageReport(int? currency, int? yearId);
        Task<List<Transaction>> GetPaymentRequestTransactions(int paymentRequestId);
        Task<List<Transaction>> GetReceiveBillTransactions(int receiveBillId);
        Task<Tuple<List<Transaction>, int, decimal>> GetTransactionsPage(List<int> projectIds, int[] projectId, int[] currencyId, int[] transactionType, int[] paymentRequestType, int? page, int? pageSize, DateTime? from, DateTime? to, string payTo, string fourthLeveLCode,
            string paymentRequestCode, string paymentDescription, int[] paymentTypeId, int? sortType = 1);

        Task<List<Transaction>> GetProjectsPayments(List<int> projectIds, int currencyId, int year, int month);
    }


    public class TransactionAppService : AsyncCrudAppService<Transaction, TransactionDto, int>, ITransactionAppService
    {
        public TransactionAppService(IRepository<Transaction, int> repository)
            : base(repository)
        {
        }
        public async Task<List<Transaction>> GetAllTransactionsForUsageReport(int? currency, int? yearId)
        {
            var querable = Repository.GetAll()
                .Include(a => a.PaymentRequest)
                .Include(a => a.ReceiveBill)
                .Include(a => a.Project)
                .Where(a => (a.PaymentRequest != null && a.PaymentRequest.PaymentType == PaymentType.Cash)
                || (a.ReceiveBill != null && a.ReceiveBill.ReceiveBillType == ReceiveBillType.Cash)
                || (a.PaymentRequest != null && a.PaymentRequest.PaymentType == PaymentType.SameDayCheque)
                )
                .AsQueryable();


            if (currency != null) querable = querable.Where(a => (int)a.CurrencyType == currency);


            if (yearId != null) querable = querable.Where(a => new PersianDateTime(a.CreationTime).Year == yearId);

            return await querable.OrderBy(a => a.CreationTime).ToListAsync();
        }


        public async Task<List<Transaction>> GetTransactionsPage(int page, int pageSize, int? currency)
        {
            if (currency != null)
            {
                return await Repository.GetAll().Where(a => (int)a.CurrencyType == currency)
                                       .Include(a => a.Project).Include(a => a.PaymentRequest)
                                       .OrderByDescending(a => a.CreationTime)
                                       .Skip(pageSize * (page - 1))
                                       .Take(pageSize)
                                       .ToListAsync();
            }
            return await Repository.GetAll()
                                   .Include(a => a.Project)
                                   .Include(a => a.PaymentRequest)
                                   .OrderByDescending(a => a.CreationTime)
                                   .Skip(pageSize * (page - 1))
                                   .Take(pageSize).ToListAsync();
        }


        public async Task<Tuple<List<Transaction>, int, decimal>> GetTransactionsPage(List<int> projectIds, int[] projectId, int[] currencyId, int[] transactionType, int[] paymentRequestType, int? page, int? pageSize, DateTime? from, DateTime? to, string payTo,
            string fourthLeveLCode, string paymentRequestCode, string paymentDescription, int[] paymentTypeId, int? sortType = 1)
        {
            var items = new List<Transaction>();
            decimal totalAmount = 0;
            int total = 0;

            var queryable = Repository.GetAll()
                .Include(a => a.Project)
                .Include(a => a.PaymentRequest)
                .Include(a => a.ReceiveBill)
                .Where(a => a.ReceiveBill != null || a.PaymentRequest != null)
                .Where(a => projectIds.Contains(a.ProjectId.Value))
                .OrderByDescending(a => a.CreationTime)
                .AsQueryable();


            if (projectId.Length > 0) queryable = queryable.Where(a => projectId.Contains(a.ProjectId.Value));

            if (currencyId.Length > 0) queryable = queryable.Where(a => currencyId.Contains((int)a.CurrencyType));

            if (transactionType.Length > 0) queryable = queryable.Where(a => transactionType.Contains((int)a.TransactionType));

            if (paymentRequestType.Length > 0) queryable = queryable.Where(a => paymentRequestType.Contains((int)a.PaymentRequest.PaymentRequestType));

            if (paymentTypeId.Length > 0) queryable = queryable.Where(a => paymentTypeId.Contains((int)a.PaymentRequest.PaymentType) || paymentTypeId.Contains((int)a.ReceiveBill.ReceiveBillType));


            if (sortType == 1) queryable = queryable.OrderByDescending(a => a.CreationTime);

            if (sortType == 2) queryable = queryable.OrderBy(a => a.CreationTime);

            if (sortType == 3) queryable = queryable.OrderByDescending(a => a.Amount);

            if (sortType == 4) queryable = queryable.OrderBy(a => a.Amount);

            if (from.HasValue) queryable = queryable.Where(a => a.CreationTime.Date >= from.Value.Date);

            if (to.HasValue) queryable = queryable.Where(a => a.CreationTime.Date <= to.Value.Date);

            if (payTo != null) queryable = queryable.Where(a => a.PaymentRequest.PayTo.Contains(payTo));

            if (fourthLeveLCode != null)
            {
                if (fourthLeveLCode == "-")
                {
                    queryable = queryable.Where(a => a.PaymentRequest.FourthLevelCode == null);
                }
                else
                {
                    queryable = queryable.Where(a => a.PaymentRequest.FourthLevelCode.Contains(fourthLeveLCode));
                }
            }

            if (paymentRequestCode != null) queryable = queryable.Where(a => a.PaymentRequest.PaymentRequestCode.Contains(paymentRequestCode));

            if (paymentDescription != null) queryable = queryable.Where(a => a.PaymentRequest.PaymentDescription.Contains(paymentDescription));

            if (page != null && pageSize != null)
            {
                items = await queryable.Skip(pageSize.Value * (page.Value - 1)).Take(pageSize.Value).ToListAsync();
                total = await queryable.CountAsync();
            }
            else
            {
                items = await queryable.ToListAsync();
                total = await queryable.CountAsync();
                totalAmount = await queryable.SumAsync(a => a.Amount);
            }
            return new Tuple<List<Transaction>, int, decimal>(items, total, totalAmount);
        }


        public async Task<List<Transaction>> GetPaymentRequestTransactions(int paymentRequestId)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a => a.PaymentRequestId == paymentRequestId).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<Transaction>> GetReceiveBillTransactions(int receiveBillId)
        {
            return await Repository.GetAll().Include(a => a.Project).Where(a => a.ReceiveBillId == receiveBillId).OrderByDescending(a => a.Id).ToListAsync();
        }

        public async Task<List<Transaction>> GetProjectsPayments(List<int> projectIds, int currencyId, int year, int month)
        {
            var queryable = Repository.GetAll()
                .Include(a => a.Project)
                .Include(a => a.PaymentRequest)
                .Where(a => a.TransactionType == TransactionType.Payment
                && projectIds.Contains(a.PaymentRequest.ProjectId)
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
