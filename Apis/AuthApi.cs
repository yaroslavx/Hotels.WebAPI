using Hotels.WebAPI.Auth;

namespace Hotels.WebAPI.Apis;

public class AuthApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/login", [AllowAnonymous] async (HttpContext httpContext,
            ITokenService tokenService, IUserRepository userRepository) => {
            UserModel userModel = new()
            {
                UserName = httpContext.Request.Query["username"],
                Password = httpContext.Request.Query["password"]
            };
            var userDto = userRepository.GetUser(userModel);
            if (userDto == null) return Results.Unauthorized();
            var token = tokenService.BuildToken(app.Configuration["Jwt:Key"], 
                app.Configuration["Jwt:Issuer"], userDto);
            return Results.Ok(token);
        });

    }
}