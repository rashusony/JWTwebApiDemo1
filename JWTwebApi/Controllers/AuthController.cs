using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel;
using System.Security;

namespace JWTwebApi.Controllers
{
    [Route("api/{controller}")]
    public class AuthController : Controller
    {
        [HttpPost("token")]
        public IActionResult Token()
        {
            var header = Request.Headers["Authorization"];

            if (header.ToString().StartsWith("Basic"))
            {
                var credValue = header.ToString().Substring("Basic".Length).Trim();
                var userNamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(credValue));

                var userNameandPassword = userNamePassword.Split(":");

                //write the db login in order to check the username and password

                if (userNameandPassword[0] == "Admin" && userNameandPassword[1] == "admin")
                {
                    var claimsdata = new[] { new Claim(ClaimTypes.Name, "username") };

                    string key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                    var signInCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                    var token = new JwtSecurityToken(
                        issuer: "xyz.com",
                        audience: "xyz.com",
                        expires: DateTime.Now.AddMinutes(1),
                        claims: claimsdata,
                        signingCredentials: signInCred
                        );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(tokenString);


                }
                return BadRequest("Worng Request");

            }
            return BadRequest("Worng Request");
        }
    }
}