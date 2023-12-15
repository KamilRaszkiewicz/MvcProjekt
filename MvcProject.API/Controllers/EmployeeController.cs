using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.API.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ApiController
    {
        private readonly IAuthService _authService;

        public EmployeeController(
            IAuthService authService
            )
        {
            _authService = authService;
        }

        /// <summary>
        /// Gets users data (roles, unreturned borrowings etc)
        /// </summary>
        [HttpGet("getUsersData")]
        [Authorize(Roles = "Employee")]
        public List<GetUsersData> GetUsersData()
        {
            var res = _authService.GetUsersData();

            return res;
        }


        /// <summary>
        /// Verifies user
        /// </summary>
        [HttpPost("verifyUser")]
        [Authorize(Roles = "Employee")]
        public async Task<BaseResponse> VerifyUser(int usersId)
        {
            var response = await _authService.VerifyUser(usersId);

            if(response.Status != 0)
            {
                if (response.Status == -1)
                    HttpContext.Response.StatusCode = 500;
                else
                    HttpContext.Response.StatusCode = 400;
            }


            return response;
        }

    }
}
