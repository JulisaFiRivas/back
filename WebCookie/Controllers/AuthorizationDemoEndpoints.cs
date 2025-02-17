using Microsoft.AspNetCore.Authorization;

namespace WebCookie.Controllers
{
    public static class AuthorizationDemoEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/AdminOnly", AdminOnly);


            app.MapGet("/Admin", [Authorize(Roles = "Admin")] () =>
            { return "Admin "; });

            return app;
        }

        [Authorize(Roles = "Admin")]
        private static string AdminOnly()
        { return "Admin Only"; }
    }
}
