using Azure;
using Microsoft.AspNetCore.Identity;
using MvcProject.Application.Dto;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Enums;
using MvcProject.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Services
{
    internal class RolesService : IRolesService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<BaseResponse> GrantRoleAsync(int usersId, Roles role, CancellationToken ct)
        {
            var response = new BaseResponse();

            try
            {
                var user = await _userManager.FindByIdAsync(usersId.ToString());

                if(user == null)
                {
                    response.Status = 1;
                    response.Error = "User does not exist";

                    return response;
                }
                
                await _userManager.AddToRoleAsync(user, Enum.GetName<Roles>(role)!);
            } 
            catch(Exception e)
            {
                response.Status = -1;
            }

            return response;
        }

        public async Task<BaseResponse> RemoveRoleAsync(int usersId, Roles role, CancellationToken ct)
        {
            var response = new BaseResponse();

            try
            {
                var user = await _userManager.FindByIdAsync(usersId.ToString());

                if (user == null)
                {
                    response.Status = 1;
                    response.Error = "User does not exist.";

                    return response;
                }

                if(role == Roles.Admin)
                {
                    response.Status = 2;
                    response.Error = "Cannot remove Admin role.";

                    return response;
                }

                if (role == Roles.User)
                {
                    response.Status = 2;
                    response.Error = "Cannot remove User role.";

                    return response;
                }

                await _userManager.RemoveFromRoleAsync(user, Enum.GetName<Roles>(role)!);
            }
            catch (Exception e)
            {
                response.Status = -1;
            }

            return response;
        }
    }
}
