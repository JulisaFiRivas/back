using Backend.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebCookie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace WebCookie.Controllers
{
    public static class AuthorizationDemoEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Admin", ListUsers);
            app.MapGet("/Usuario", ListUsersPublic);
            app.MapPut("/Admin/UpdateUserRole", UpdateUser);
            app.MapDelete("/Admin/DeleteUser/{email}", DeleteUser);
            return app;
        }

        [Authorize(Roles = "Admin")]
        private static async Task<IResult> ListUsers(UserManager<AppUser> userManager)
        {
            var users = await userManager.Users.ToListAsync();

            var usersWithRoles = new List<object>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                usersWithRoles.Add(new
                {
                    Email = user.Email,
                    Role = roles.FirstOrDefault(),
                    UltimaConexion = user.UltimaConexion
                });
            }

            return Results.Ok(usersWithRoles);
        }

        [Authorize(Roles = "Admin,Usuario")]
        private static async Task<IResult> ListUsersPublic(UserManager<AppUser> userManager)
        {
            var users = await userManager.Users.ToListAsync();

            var userList = new List<object>();

            foreach (var user in users)
            {
                userList.Add(new
                {
                    Email = user.Email,
                    UltimaConexion = user.UltimaConexion
                });
            }

            return Results.Ok(userList);
        }

        [Authorize(Roles = "Admin")]
        private static async Task<IResult> UpdateUser(UserManager<AppUser> userManager,
                                              [FromBody] UpdateUserDto updateUserDto)
        {
            var email = updateUserDto.Email;
            var newRole = updateUserDto.NewRole;

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Results.NotFound("Usuario no encontrado.");
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, currentRoles);
            await userManager.AddToRoleAsync(user, newRole);

            return Results.Ok($"Usuario {email} actualizado con rol {newRole}.");
        }


        [Authorize(Roles = "Admin")]
        private static async Task<IResult> DeleteUser(UserManager<AppUser> userManager, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Results.NotFound("Usuario no encontrado.");
            }

            await userManager.DeleteAsync(user);
            return Results.Ok($"Usuario {user.Email} eliminado.");
        }

    }
}
