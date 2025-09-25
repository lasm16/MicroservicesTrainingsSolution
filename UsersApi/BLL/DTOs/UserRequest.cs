namespace UsersApi.BLL.Models
{
    public class UserRequest
    {        
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
