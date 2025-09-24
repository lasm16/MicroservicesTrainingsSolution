using DataAccess.Models;

namespace UsersApi.BLL.Mapper
{
    public interface IUserMapper
    {
        User ToEntity(UserDto dto);                    // для создания
        void UpdateEntity(UserDto dto, User entity);   // для обновления
        UserDto ToDto(User entity);
    }
}
