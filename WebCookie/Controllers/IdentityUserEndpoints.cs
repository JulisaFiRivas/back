using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebCookie.Models;

namespace WebCookie.Controllers
{
    public class UserRegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserDto
    {
        public AppUser appUser { get; set; }
        public string RolName { get; set; }

    }

    public static class IdentityUserEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/signup", CreateUser);
            app.MapPost("/signin", SignIn);
            app.MapGet("/roles", GetRole);
            return app;
        }

        [Authorize]
        public static async Task<IResult> GetRole(RoleManager<IdentityRole> RoleManager)
        {
            return Results.Ok(await RoleManager.Roles.ToListAsync());
        }

        [AllowAnonymous]
        private static async Task<IResult> CreateUser(
                UserManager<AppUser> userManager,
                [FromBody] UserRegistrationModel userRegistrationModel)
        {
            AppUser user = new AppUser()
            {
                UserName = userRegistrationModel.Email,
                Email = userRegistrationModel.Email,
                FullName = userRegistrationModel.FullName,
            };
            var result = await userManager.CreateAsync(
                user,
                userRegistrationModel.Password);
            await userManager.AddToRoleAsync(user, userRegistrationModel.Role);

            if (result.Succeeded)
                return Results.Ok(result);
            else
                return Results.BadRequest(result);
        }

        [AllowAnonymous]
        private static async Task<IResult> SignIn(
            UserManager<AppUser> userManager,
                [FromBody] LoginModel loginModel,
                IOptions<Jwt> jwt)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                user.UltimaConexion = DateTime.Now;
                await userManager.UpdateAsync(user);
                var roles = await userManager.GetRolesAsync(user);
                var signInKey = new SymmetricSecurityKey(
                               Encoding.UTF8.GetBytes(jwt.Value.Key)
                               );
                ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                        //probando cosas 
          new Claim("userID",user.Id.ToString()),
          new Claim(ClaimTypes.Role,roles.First()),
                });
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(
                        signInKey,
                        SecurityAlgorithms.HmacSha256Signature
                        )
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                    
                UserDto userDto = new UserDto
                {
                    
                    appUser = user,
                    RolName =  roles.ToList().FirstOrDefault()

                };
                return Results.Ok(new { token, userDto });
            }
            else
                return Results.BadRequest(new { message = "Username or password is incorrect." });
        }
    }



}
