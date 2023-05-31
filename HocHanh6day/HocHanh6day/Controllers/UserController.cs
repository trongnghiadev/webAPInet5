using HocHanh6day.Data;
using HocHanh6day.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HocHanh6day.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly AppSetting _appSettings;

        public UserController(MyDbContext context, IOptionsMonitor<AppSetting> optionsMonitor) {
        
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public IActionResult Validate(LoginModel model)
        {
            var user = _context.nguoiDungs.SingleOrDefault(p => p.UserName == model.UserName && p.Password == model.Password);
            if (user == null)
            {
                return Ok(new APIResponse
                {
                    Success = false,
                    Message = "Invaid username/password"
                });
            } else
            {
                return Ok(new APIResponse
                {
                    Success = true,
                    Message = "Authenticate success",
                    Data = GenerateToken(user)

                });
            }
        }

        private string GenerateToken(NguoiDung nguoiDung) 
        {

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyByetes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim(ClaimTypes.Email, nguoiDung.Email),
                    new Claim("UserName", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.ID.ToString()),

                    // roles

                    new Claim("TokenId", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByetes), SecurityAlgorithms.HmacSha512Signature)};
                 var token = jwtTokenHandler.CreateToken(tokenDescription);
                  return jwtTokenHandler.WriteToken(token);
            }
        }

}

