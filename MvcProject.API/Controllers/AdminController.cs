using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcProject.API.Controllers;
using MvcProject.API;
using MvcProject.Application.Dto;
using MvcProject.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using MvcProject.Application.Dto.Book;
using MvcProject.Domain.Enums;
using MvcProject.Application.Dto.Borrowing;
using MvcProject.Application.Dto.Admin;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Admin role required
/// </summary>
[ApiController]
[Route("api/admin")]
public class AdminController : ApiController
{
    private readonly IBorrowingService _borrowingService;
    private readonly IRolesService _roleService;

    public AdminController(
        IBorrowingService borrowingService,
        IRolesService roleService
        )
    {
        _borrowingService = borrowingService;
        _roleService = roleService;
    }

    /// <summary>
    /// Grants a role for user. Admin role required.
    /// </summary>
    [HttpPost("userRoles")]
    [Authorize(Roles = "Admin")]
    public async Task<BaseResponse> GrantRole([FromBody][Required]RoleRequest req, CancellationToken ct)
    {
        var res = await _roleService.GrantRoleAsync(req.UsersId, req.Role, ct);

        if (res.Status != 0)
        {
            if (res.Status == -1)
                HttpContext.Response.StatusCode = 500;
            else
                HttpContext.Response.StatusCode = 400;
        }

        return res;
    }


    /// <summary>
    /// Removes a role from user. Admin role required.
    /// </summary>
    [HttpDelete("userRoles")]
    [Authorize(Roles = "Admin")]
    public async Task<BaseResponse> RemoveRole([FromBody][Required] RoleRequest req, CancellationToken ct)
    {
        var res = await _roleService.RemoveRoleAsync(req.UsersId, req.Role, ct);

        if (res.Status != 0)
        {
            if (res.Status == -1)
                HttpContext.Response.StatusCode = 500;
            else
                HttpContext.Response.StatusCode = 400;
        }

        return res;
    }
}