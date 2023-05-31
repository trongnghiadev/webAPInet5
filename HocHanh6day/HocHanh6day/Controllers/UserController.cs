using HocHanh6day.Data;
using HocHanh6day.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Validate(LoginModel model)
        {
            var user = _context.nguoiDungs.SingleOrDefault(p => p.UserName == model.UserName && p.Password == model.Password);
            if (user == null)
            {
                return Ok(new APIResponse
                {
                    Success = false,
                    Message = "Invaid username/password"
                });
            } 
      
                // cap token
                var token = await GenerateToken(user);
                return Ok(new APIResponse
                {
                    Success = true,
                    Message = "Authenticate success",
                    Data = token

                }); ;
            
        }

        private async Task<TokenModel> GenerateToken(NguoiDung nguoiDung) 
        {

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyByetes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim(JwtRegisteredClaimNames.Email, nguoiDung.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, nguoiDung.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.ID.ToString()),

                    // roles
                }),
                Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByetes), SecurityAlgorithms.HmacSha512Signature)};
                 var token = jwtTokenHandler.CreateToken(tokenDescription);
                  var accsessToken = jwtTokenHandler.WriteToken(token);
                  var refreshToken = GenerateRefreshToken();

            // luu data
            var RefreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtID = token.Id,
                UserId = nguoiDung.ID,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuadAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddSeconds(10)

            };
           await _context.AddAsync(RefreshTokenEntity);
           await _context.SaveChangesAsync();

                    return new TokenModel
                    {
                        AccessToken = accsessToken,
                        RefreshToken = refreshToken
                    };
            }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }

        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
                {
                // tu cap token
                ValidateIssuer = false,
                ValidateAudience = false,

                    //ky vao token
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false,

                };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return Ok(new APIResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                // Check han su dung
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault
                    (x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate < DateTime.UtcNow)
                {
                  return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Het Han Su Dung"

                    });
                }
                // check RefreshToken
                var storedToken = _context.refreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);

                if (storedToken == null)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Token khong hop le"

                    });
                }
                // kiem tra RefreshToken da su dung chua
                #region RefreshToken kiem tra
                if (storedToken.IsUsed)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Token da duoc su dung"

                    });
                }

                if (storedToken.IsRevoked)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Token da bi thu hoi"

                    });
                }
                #endregion

                // kiem tra Id
                var jwt = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if(storedToken.JwtID != jwt)
                {
                    return Ok(new APIResponse
                    {
                        Success = false,
                        Message = "Token not macth"

                    });
                }

                // update token 
                storedToken.IsUsed = true;
                storedToken.IsRevoked = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                // tao moi token
                // cap token
                var user = await _context.nguoiDungs.SingleOrDefaultAsync(nd => nd.ID == storedToken.UserId);
                var token = await GenerateToken(user);
                return Ok(new APIResponse
                {
                    Success = true,
                    Message = "Renew success",
                    Data = token

                }); 

            } catch( Exception ex) 
            {
                return BadRequest(new APIResponse
                {
                    Success = false,
                    Message = "Error"

                });
            }
         
        }
        /*
        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
        */
        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var resultDateTime = dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return resultDateTime;
        }

    }
}


