using DataAccess.Models;
using UsersApi.BLL.DTOs;

namespace UsersApi.BLL.Mapper
{
    public class UserMapper 
    {
        public static User ToEntity(UserDto dto)
        {
            if (IsNull(dto))
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

        public static UserDto ToDto(User entity)
        {
            if (IsNull(entity) || entity.IsDeleted)
                return null!;

            return new UserDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                Email = entity.Email
            };
        }

        public static UserResponse GetUserResponse(User user)
        {
            if (IsNull(user) || user.IsDeleted)
                return null!;

            return new UserResponse.Builder()
                .SetId(user.Id)
                .SetName(user.Name)
                .SetSurname(user.Surname)
                .SetEmail(user.Email)
                .Build();
        }

        public static void UpdateEntity(UserDto dto, User entity)
        {
            if (IsNull(dto) || IsNull(entity))
                return;

            entity.Name = dto.Name;
            entity.Surname = dto.Surname;
            entity.Email = dto.Email;
            entity.Updated = DateTime.UtcNow;            
        }

        private static bool IsNull<T>(T obj) where T : class
        {
            return obj == null;
        }
        public static UserDto MapToUserDto(UserRequest request)
        {
            return new UserDto
            {
                Id = request.Id,
                Name = request.Name.Trim(),
                Surname = request.Surname.Trim(),
                Email = request.Email.Trim()
            };
        }
    }
}
