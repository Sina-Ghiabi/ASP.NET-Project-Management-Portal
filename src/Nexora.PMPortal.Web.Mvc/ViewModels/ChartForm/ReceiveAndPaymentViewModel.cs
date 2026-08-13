using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class ReceiveAndPaymentViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int Year { get; set; }

        public string PreReceived { get; set; }
        public string Statements { get; set; }
        public string DepositRelease { get; set; }
        public string WarrantyReduce { get; set; }
        public string OtherReceipt { get; set; }
        public string IndirectReceipt { get; set; }
        public string OnAccountReceipt { get; set; }
        public string ProjectPayments { get; set; }
        public string WarrantyWithWage { get; set; }
        public string SaleryWithTax { get; set; }
        public string IndirecetPayment { get; set; }
        public string ValueAddedTax { get; set; }

        public string TotalPayment { get; set; }
        public string TotalReceive { get; set; }

        public int? LastGridPage { get; set; }
    }
}
