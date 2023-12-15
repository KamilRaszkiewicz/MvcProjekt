using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Borrowing;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Interfaces;
using MvcProject.Application.Options;
using MvcProject.Domain.Enums;
using MvcProject.Domain.Models;
using MvcProject.Infrastructure.Database;
using MvcProject.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<AuthService> _logger;
        private readonly ApplicationDbContext _ctx;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            ILogger<AuthService> logger,
            IOptions<JwtOptions> jwtOptions,
            ApplicationDbContext ctx)
        
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
            _ctx = ctx;
        }

        public List<GetUsersData> GetUsersData()
        {
            var queryable = _ctx.BookBorrowings.Include(x => x.Book.Authors);

            var query = from u in _ctx.Users
                        join ur in _ctx.UserRoles on u.Id equals ur.UserId
                        join r in _ctx.Roles on ur.RoleId equals r.Id
                        group new {u, r } by new
                        {
                            u.Id,
                            u.Email,
                        } into grp
                        select new GetUsersData
                        {
                            Id = grp.Key.Id,
                            Email = grp.Key.Email,
                            Roles = grp.Select(x => x.r.Name).ToList(),

                            UnreturnedBorrowings = queryable
                                                    .Where(x => x.UsersId == grp.Key.Id && x.ReturnedAt == null)
                                                    .Select(x => new BaseBorrowingDto
                            {
                                BorrowingsId = x.Id,
                                BooksId = x.BooksId,
                                BooksISBN = x.Book.ISBN,
                                BooksTitle = x.Book.Title,
                                BorrowedAt = x.BorrowedAt,
                                ReturnedAt = x.ReturnedAt,

                                BooksAuthors = x.Book.Authors.Select(x => x.Name + " " + x.LastName).ToList(),
                            }).ToList()
                        };


            return query.ToList();
        }

        public async Task<UserAuthenticationResponse> LoginAsync(UserLoginRequest req)
        {

            var result = new UserAuthenticationResponse();

            try
            {
                var user = await _userManager.FindByEmailAsync(req.Email);

                var isValid = user != null && await _userManager.CheckPasswordAsync(user, req.Password);

                if (!isValid)
                {
                    result.IsSuccessful = false;
                    result.Errors = new[] { "Credentials are wrong" };

                    return result;
                }

                var expiryDate = DateTime.Now.AddSeconds(_jwtOptions.ValidForSeconds);

                result.IsSuccessful = true;
                result.ExpiresAt = expiryDate;
                result.Token = await CreateJwtTokenAsync(user, expiryDate);
                result.Roles = await _userManager.GetRolesAsync(user);

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown in {method}, Exception: {ex}", nameof(LoginAsync), ex);

                result.IsSuccessful = false;
            }

            return result;
        }


        public async Task<UserAuthenticationResponse> RegisterAsync(UserRegisterRequest req)
        {
            var user = new ApplicationUser
            {
                UserName = req.Email,
                Email = req.Email
            };

            var result = new UserAuthenticationResponse();

            try
            {
                var identityResult = await _userManager.CreateAsync(user, req.Password);
                var expiryDate = DateTime.Now.AddSeconds(_jwtOptions.ValidForSeconds);

                

                if (!identityResult.Succeeded)
                {
                    result.IsSuccessful = identityResult.Succeeded;
                    result.Errors = identityResult.Errors.Select(x => x.Description);

                    return result;
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, Enum.GetName<Roles>(Roles.User)!);

                if (!addRoleResult.Succeeded)
                {
                    result.IsSuccessful = identityResult.Succeeded;
                    result.Errors = addRoleResult.Errors.Select(x => x.Description);

                    return result;
                }

                result.Token = await CreateJwtTokenAsync(user, expiryDate);
                result.ExpiresAt = expiryDate;
                result.Roles = await _userManager.GetRolesAsync(user);

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown in {method}, Exception: {ex}", nameof(RegisterAsync), ex);

                result.IsSuccessful = false;
            }

            return result;
        }

        public async Task<BaseResponse> VerifyUser(int usersId)
        {
            var result = new BaseResponse();
            try
            {
                var user = _ctx.Users.FirstOrDefault(x => x.Id == usersId);
                
                if(user == null)
                {
                    result.Status = 1;
                    result.Error = "User does not exist";

                    return result;
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "VerifiedUser");

                if(!roleResult.Succeeded)
                {
                    result.Status = 2;
                    result.Error = "User already in role";

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Status = -1;
            }

            return result;

        }

        private async Task<string> CreateJwtTokenAsync(ApplicationUser user, DateTime expiryDateTime)
        {
            var claims = (await _userManager.GetRolesAsync(user))
                .Select(x => new Claim(ClaimTypes.Role, x))
                .Prepend(new Claim(ClaimTypes.Name, user.UserName));

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.Secret)
                ),
                SecurityAlgorithms.HmacSha256
            );

            var jwtToken = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claims,
                    expires: expiryDateTime,
                    signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

    }
}
