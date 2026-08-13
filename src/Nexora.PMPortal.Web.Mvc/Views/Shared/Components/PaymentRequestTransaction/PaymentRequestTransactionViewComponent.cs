using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Abp.Localization;
using Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction;
using MD.PersianDateTime;
using System;

namespace Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction
{
    public class PaymentRequestTransactionViewComponent : PMPortalViewComponent
    {
        private readonly ILanguageManager _languageManager;

        public PaymentRequestTransactionViewComponent(ILanguageManager languageManager)
        {
            _languageManager = languageManager;
        }

        public IViewComponentResult Invoke(int paymentRequestId, int status)
        {
            var model = new PaymentRequestTransactionViewModel
            {
                Id = paymentRequestId,
                TodayDate = new PersianDateTime(DateTime.Now).ToShortDateString(),
                Status = status
            };

            return View(model);
        }
    }
}
