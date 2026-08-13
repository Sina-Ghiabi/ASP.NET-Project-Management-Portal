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
    public interface IKatibe_DarkhastAppService : IAsyncCrudAppService<Katibe_DarkhastDto, int>
    {
        List<Katibe_Darkhast> GetAllKatibe_Darkhasts(List<string> onvans);
    }


    public class Katibe_DarkhastAppService : AsyncCrudAppService<Katibe_Darkhast, Katibe_DarkhastDto, int>, IKatibe_DarkhastAppService
    {
        public Katibe_DarkhastAppService(IRepository<Katibe_Darkhast, int> repository)
            : base(repository)
        {
        }


        public List<Katibe_Darkhast> GetAllKatibe_Darkhasts(List<string> onvans)
        {

            return Repository.GetAll().Where(a => onvans.Contains(a.CodeOnvan)).ToList();
        }

    }
}
