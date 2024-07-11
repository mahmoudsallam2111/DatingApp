using DatingApp.Application.Interfaces;
using DatingApp.Application.Interfaces.Repositories;
using DatingApp.WebApi.Infrastracture.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.WebApi.Infrastracture.Filters
{
    public class LogUserActivity(IUnitOfWork unitOfWork) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated)  return;

            var userId = resultContext.HttpContext.User.GetUserId();

            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

            var user = await repo.GetUserByIdWithoutInclude(int.Parse(userId));

            if (user == null) throw new Exception("This User Is not Exist");

            user.LastActive = DateTime.UtcNow;

           await unitOfWork.SaveChangesAsync();
        }
    }
}
