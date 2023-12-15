using MvcProject.Application.Dto;
using MvcProject.Application.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserAuthenticationResponse> RegisterAsync(UserRegisterRequest req);
        Task<UserAuthenticationResponse> LoginAsync(UserLoginRequest req);
        public List<GetUsersData> GetUsersData();
        public Task<BaseResponse> VerifyUser(int usersId);
    }
}
