using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IHostingEnvironment hostingEnv;
        //public ValuesController(IHostingEnvironment hostingEnvironment)
        //{
        //    hostingEnv = hostingEnvironment;
        //}

        private ILogger<ValuesController> logger;
        //public ValuesController(ILogger<ValuesController> _logger)
        //{
        //    logger = _logger;
        //}

        public ValuesController(IHostingEnvironment hostingEnvironment, ILogger<ValuesController> _logger)
        {
            hostingEnv = hostingEnvironment;
            logger = _logger;
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 获取openid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet("{id}")]
        public Entity.UnitEntity Get(string id)
        {
            Entity.UnitEntity entity = new Entity.UnitEntity();
            string openid = Core.SuperClass.GetUserOpenId(id);
            entity.status = 1;
            entity.data = openid;
          //  logger.LogError(openid);
            return entity;
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
