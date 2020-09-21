using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Super.Core.Infrastruct.Configuration;

namespace Super.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
           string appId = AppConfiguration.Instance.GetValue("appId");
            string appSecret = AppConfiguration.Instance.GetValue("appSecret");
            string baseUrl = AppConfiguration.Instance.GetValue("baseUrl");

            string db_server = AppConfiguration.Instance.GetValue("db_server");
            string db_port = AppConfiguration.Instance.GetValue("db_port");
            string db_type = AppConfiguration.Instance.GetValue("db_type");

            string db_user = AppConfiguration.Instance.GetValue("db_user");
            string db_pwd = AppConfiguration.Instance.GetValue("db_pwd");


            return new string[] { appId, appSecret, baseUrl, db_server, db_port, db_type, db_user, db_pwd };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
