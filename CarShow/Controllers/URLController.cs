using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class URLController : ControllerBase
    {


        // GET: api/URL/5
        [HttpGet("{id}", Name = "GetURL")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/URL
        [HttpPost]
        public Entity.ShareResult Post()
        {
            Entity.ShareResult result = new Entity.ShareResult();

            try
            {
                string url = Request.Form["url"];
                Entity.ShareEntity entity = Core.SuperClass.sign(url);
                result.data = entity;
                result.status = 1;
            }
            catch (Exception ex)
            {
                result.status = 0;
                result.desc = "分享异常！";
            }
            return result;
        }

        // PUT: api/URL/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
