using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using DemoJWT.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DemoJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        public IConfiguration Configuration { get; set; }
        public readonly ApplicationDBContext context;

        public UserController(IConfiguration configuration, ApplicationDBContext context)
        {
            this.context = context;
            this.Configuration = configuration;

        }
        [HttpPost("Login")]



        public async Task<IActionResult> Login(User user)
        {
            if (user != null && user.UserName != null && user.Password != null)
            {
                var userData = await context.User.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
                var jwt = Configuration.GetSection("Jwt").Get<Jwt>();
                if (userData != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim( "Id",user.UserId.ToString()),
                        new Claim("UserName", user.UserName),
                        new  Claim("Password",user.Password)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signIn

                        );
                    User_Activity active = new User_Activity();
                    active.UserId = user.UserId;
                    active.UserName = user.UserName;
                    active.Timestamp= DateTime.UtcNow;
                    active.activityType = "Login";  
                    context.Activity.Add(active);
                    await context.SaveChangesAsync();
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    

                }

                else
                {
                    return BadRequest("Invalid Credentials");

                }
            }
            else
            {
                return BadRequest("Invalid Credentials");

            }
        }




        /*public  async Task<User> GetUser(string username, string password)
        {
            return await ;
        }*/


    }
}

