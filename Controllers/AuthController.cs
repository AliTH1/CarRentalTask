using AutoMapper;
using CarRental.Entities.Auth;
using CarRental.Entities.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly TokenOption _tokenOption;
        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _tokenOption = _configuration.GetSection("TokenOptions").Get<TokenOption>();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            AppUser newUser = _mapper.Map<AppUser>(registerDto);

            IdentityResult createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!createUserResult.Succeeded)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errors = createUserResult.Errors
                });
            }

            IdentityResult roleResult = await _userManager.AddToRoleAsync(newUser, UserRoles.User.ToString());
            if (!roleResult.Succeeded)
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    errors = roleResult.Errors
                });
            }

            return Ok(new
            {
                Message = "User created"
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            AppUser user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user is null)
            {
                return NotFound();
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_tokenOption.SecurityKey));
            SigningCredentials signingCredentials = new SigningCredentials
                (securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtHeader header = new JwtHeader(signingCredentials);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FullName", user.FullName)
            };

            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtPayload payLoad = new JwtPayload(
                issuer: _tokenOption.Issuer,
                audience: _tokenOption.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_tokenOption.AccessTokenExpiration)
                );

            JwtSecurityToken securityToken = new JwtSecurityToken(header, payLoad);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(securityToken);

            return Ok(new
            {
                token = token,
                expires = DateTime.UtcNow.AddMinutes(_tokenOption.AccessTokenExpiration),
            });
        }

        //[HttpPost("AddRoles")]
        //public async Task<IActionResult> AddRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(UserRoles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString()});
        //        }
        //    }

        //    return Ok();
        //}
        enum UserRoles
        {
            User,
            Admin
        }
    }
}
