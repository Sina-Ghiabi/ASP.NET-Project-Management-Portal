using Abp.Application.Services.Dto;

namespace Nexora.PMPortal.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

