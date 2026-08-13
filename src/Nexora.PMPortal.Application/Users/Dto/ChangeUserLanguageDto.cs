using System.ComponentModel.DataAnnotations;

namespace Nexora.PMPortal.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}