using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.Models.Account
{
    public class ChangePasswordViewModel
    {
        public long UserId { get; set; }
        public string Password { get; set; }
    }
}
