using DataAccess.Models;

namespace UsersApi.BLL.Mapper
{
    public class UserMapper : IUserMapper
    {
        public User ToEntity(UserDto dto)
        {
            if (dto == null)
                return null!; 

            return new User
            {
                Id = dto.Id,
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                Created = dto.Id == 0 ? DateTime.UtcNow : default, 
                Updated = DateTime.UtcNow,
                IsDeleted = false
            };
        }

        public UserDto ToDto(User entity)
        {
            if (entity == null || entity.IsDeleted)
                return null!; 

            return new UserDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                Email = entity.Email
            };
        }
        public void UpdateEntity(UserDto dto, User entity)
        {
            if (dto == null || entity == null) return;

            entity.Name = dto.Name;
            entity.Surname = dto.Surname;
            entity.Email = dto.Email;
            entity.Updated = DateTime.UtcNow;            
        }
    }
}
