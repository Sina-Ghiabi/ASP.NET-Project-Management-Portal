using System.Collections.Generic;
using Abp.Localization;

namespace Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction
{
    public class PaymentRequestTransactionViewModel
    {
        public int Id { get; set; }
        public string TodayDate { get; set; }
        public int Status { get; set; }
    }
}
