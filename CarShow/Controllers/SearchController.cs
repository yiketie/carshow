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
    public class SearchController : ControllerBase
    {
        BLL.CarShowBLL _bll = new BLL.CarShowBLL();

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


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

     


        // GET: api/Search/5
        [HttpGet("{type}", Name = "GetSearch")]
        public Entity.ImageResult Get(int type)
        {
            Entity.ImageResult result = new Entity.ImageResult();
            try
            {
                result.data = _bll.GetHotNewImageList(type);
                result.status = 1;
            }
            catch (Exception ex)
            {
                result.status = 0;
                result.desc = ex.StackTrace;
            }

            return result;
        }


    }
}
