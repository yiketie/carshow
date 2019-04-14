
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;

namespace CarShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private ILogger<ValuesController> logger;
        private IHostingEnvironment hostingEnv;
        BLL.CarShowBLL _bll = new BLL.CarShowBLL();

        public ImageController(IHostingEnvironment hostingEnvironment, ILogger<ValuesController> _logger)
        {
            hostingEnv = hostingEnvironment;
            logger = _logger;
        }

        /// <summary>
        /// 查询彩报接口
        /// </summary>
        /// <param name="inputpara"></param>
        /// <returns></returns>
        // GET: api/Image/5
        [HttpGet("{type}", Name = "GetImage")]
        public Entity.ImageResult Get(string type)
        {
            Entity.ImageResult result = new Entity.ImageResult();
            result.data = new List<Entity.ImageEntity>();
            try
            {
                result.data = _bll.GetImage(type);
                result.status = 1;
            }
            catch (Exception ex)
            {
                result.status = 1;
                result.desc = ex.StackTrace;
            }
            return result;

        }

        /// <summary>
        /// 上传图片接口
        /// </summary>
        /// <param name="value"></param>
        // POST: api/Image
        [HttpPost]
        public Entity.BaseResult Post()
        {

            Entity.BaseResult result = new Entity.BaseResult();
            try
            {
                string openid = Request.Form["openid"];
                string imageid = Request.Form["imageid"];
                string filecontent = Request.Form["file"];
                string[] filess = filecontent.Split(",");
               // logger.LogError("wxk: "+filecontent);
                byte[] bit = Convert.FromBase64String(filess[1]);
              
                string filePath = hostingEnv.WebRootPath;
               // var files = Request.Form.Files;
                string filename = $@"\Files\Pictures\";
                //if (files.Count > 0)
                //{
                    //var file = files[0];
                    //创建文件夹
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string oldfilext = ".png";
                    //创建文件名称并写入文件
                    filename += Guid.NewGuid().ToString()+ oldfilext;
                    using (FileStream fs = System.IO.File.Create(filePath + filename))
                    {
                        //将byte数组写入文件中
                        fs.Write(bit, 0, bit.Length);
                        //所有流类型都要关闭流，否则会出现内存泄露问题
                        fs.Flush();
                        //file.CopyTo(fs);
                        //fs.Flush();
                    }

               // }
                Entity.ImageEntity entity = new Entity.ImageEntity();
                entity.ImageId = imageid;
                entity.UserOpenId = openid;
                entity.FilePath = filename;

                bool insresult = _bll.InsertImageInfo(entity);
                if (insresult)
                {
                    result.status = 1;
                }
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
