using DataAccess.Models;
using UsersApi.BLL.DTOs;

namespace UsersApi.BLL.Mapper
{
    public class UserMapper 
    {
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

        private static bool IsNull<T>(T obj) where T : class
        {
            return obj == null;
        }

        public static User MapUserRequestToUser(UserRequest request)
        {
            return new()
            {
                Id = request.Id,
                Name = request.Name.Trim(),
                Surname = request.Surname.Trim(),
                Email = request.Email.Trim()
            };
        }
    }
}
