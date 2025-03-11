namespace Hotels.WebAPI.Auth;

public class UserRepository : IUserRepository
{
    private List<UserDto> _users => new()
    {
        new UserDto("FirstUserName", "password"),
        new UserDto("SecondUserName", "qwerty"),
        new UserDto("ThirdUserName", "12345")
    };
    
    public UserDto GetUser(UserModel userModel)
    {
        return _users.FirstOrDefault(u =>
                   string.Equals(u.UserName, userModel.UserName) &&
                   string.Equals(u.Password, userModel.Password)) ??
               throw new Exception();
    }
}