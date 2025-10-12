using System.ComponentModel.DataAnnotations;

namespace UsersApi.BLL.Models
{
    public class UserRequest
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(140, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [StringLength(140, MinimumLength = 3)]
        public string Surname { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;
    }
}
