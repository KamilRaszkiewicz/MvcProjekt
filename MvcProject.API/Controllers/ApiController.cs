using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MvcProject.Infrastructure.Identity;

namespace MvcProject.API.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        public UserContext UserContext => GetUserContext();
        private UserContext GetUserContext()
        {
            var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

            var userName = User.Identity?.Name;

            if(userName == null)
            {
                return new UserContext();
            }

            var currentUser = userManager.FindByNameAsync(userName);
            currentUser.Wait();

            if(currentUser == null || currentUser.Result == null) 
            {
                return new UserContext();
            }

            return new UserContext()
            {
                Id = currentUser.Result.Id,
                Email = currentUser.Result.Email,
                IsVerified = currentUser.Result.IsVerified
            };
        }
    }
}
