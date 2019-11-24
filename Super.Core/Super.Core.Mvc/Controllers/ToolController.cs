using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Super.Core.Mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolController : ControllerBase
    {
        readonly IApplicationLifetime _appLifetime;

        public ToolController(IApplicationLifetime appLifetime)
        {
            this._appLifetime = appLifetime;
        }


        public IActionResult Get()
        {
            this._appLifetime.StopApplication();
            return this.Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody]TestModel model)
        {
            throw new Exception("aaaa");
            return this.Ok(model);
        }

    }
}