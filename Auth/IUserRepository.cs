namespace Hotels.WebAPI.Auth;

public interface IUserRepository
{
    UserDto GetUser(UserModel userModel);
}