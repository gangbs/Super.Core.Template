using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Super.Core.Mvc.Models;

namespace Super.Core.Mvc.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ApiBaseController
    {
        [Route("bad")]
        [HttpGet]
        public IActionResult Bad400()
        {
            //return this.BadRequest(new CommonResModel { code = 400, message = "yyg" });//
            //return this.Forbid();
            //return this.Ok(new CommonResModel { code = 400, message = "yyg" });
            return this.Ok();
        }
    }
}