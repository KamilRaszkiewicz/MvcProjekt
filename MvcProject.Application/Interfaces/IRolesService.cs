using MvcProject.Application.Dto;
using MvcProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IRolesService
    {
        Task<BaseResponse> GrantRoleAsync(int usersId, Roles role, CancellationToken ct);
        Task<BaseResponse> RemoveRoleAsync(int usersId, Roles role, CancellationToken ct);
    }
}
