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
    public class FabController : ControllerBase
    {
        BLL.CarShowBLL _bll = new BLL.CarShowBLL();

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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

        // POST: api/Fab
        [HttpPost]
        public Entity.BaseResult Post()
        {
            Entity.BaseResult br = new Entity.BaseResult();
            try
            {
                Entity.ImageEntity enttiy = new Entity.ImageEntity();
                enttiy.UserOpenId = Request.Form["openid"];
                enttiy.Renom = Request.Form["renum"];
                enttiy.ImageId = Request.Form["imageid"];
                //插入之前需要先检查此人当日是否已经超过三次，若已满足，则不给点赞
                if (_bll.GetUserFabEnable(enttiy.UserOpenId))
                {
                    if (_bll.InsertFabInfo(enttiy))
                        br.status = 1;
                    else
                    {
                        br.status = 0;
                        br.desc = "插入失败，请检查！";
                    }
                }
                else {
                    br.status = 0;
                    br.desc = "今日点赞已满3次，请明日再来！";
                }
            }
            catch (Exception ex)
            {
                br.status = 0;
                br.desc = "点赞异常！";
            }

            return br;

        }


    }
}
