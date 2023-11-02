using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Interfaces;
using MvcProject.Domain;
using MvcProject.Domain.Enums;
using MvcProject.Infrastructure.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            ILogger<AuthService> logger,
            IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
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
                    
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown in {0}, Exception: {1}", nameof(LoginAsync), ex);

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

                result.IsSuccessful = identityResult.Succeeded;

                if (!result.IsSuccessful)
                {
                    result.Errors = identityResult.Errors.Select(x => x.Description);

                    return result;
                }

                result.Token = await CreateJwtTokenAsync(user, expiryDate);
                result.ExpiresAt = expiryDate;
            } 
            catch(Exception ex)
            {
                _logger.LogError("Exception thrown in {0}, Exception: {1}", nameof(RegisterAsync), ex);

                result.IsSuccessful = false;
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
