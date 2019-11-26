using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Super.Core.Mvc.Models;
using Super.Core.Mvc.Utility;

namespace Super.Core.Mvc.Controllers
{
    [Route("api/jwt/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {


        [HttpPost]
        public IActionResult Post([BindRequired, FromBody]LoginModel model)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, model.userName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, model.userName));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Iss, JwtConst.Issuer));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Aud, JwtConst.Audience));
            //claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.ToInt().ToString()));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.Instance.GetValue("Jwt:SecurityKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var head = new JwtHeader(creds);
            var payload = new JwtPayload(claims);
            var tokenObj = new JwtSecurityToken(head, payload);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObj);

            return this.Ok(new { token = token });
        }

    }
}