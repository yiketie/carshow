using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CarShow.Entity;

namespace CarShow.BLL
{
    public class CarShowBLL
    {
        DAL.CarShowDAL _DAL = new DAL.CarShowDAL();
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertImageInfo(ImageEntity entity)
        {
            return _DAL.InsertImageInfo(entity);
        }

        /// <summary>
        /// 通过刊号获取
        /// </summary>
        /// <param name="renum"></param>
        /// <returns></returns>
        public List<ImageEntity> GetImage(string renum)
        {
            List<ImageEntity> list = new List<ImageEntity>();
            string[] paras = renum.Split("&");
            string id = string.Empty;
            string type = string.Empty;
            id = paras[0];
            if (paras[1].Equals("0"))  //刊号
            {
                ImageEntity entity = new ImageEntity();
                int irenu = Convert.ToInt16(id);
                DataTable dt = _DAL.GetImageByRenum(irenu);
                list = GetEntityList(dt);
            }
            else
            {
                ImageEntity entity = new ImageEntity();
                DataTable dt = _DAL.GetImageByImageId(id);
                list = GetEntityList(dt);
            }
            return list;
        }

        /// <summary>
        /// 获取最新最热的图片集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ImageEntity> GetHotNewImageList(int type)
        {
            List<ImageEntity> list = new List<ImageEntity>();
            DataTable dt = _DAL.GetNewestImage(type);
            list = GetEntityList(dt);
            return list;
        }


        /// <summary>
        /// 插入点赞数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertFabInfo(Entity.ImageEntity entity)
        {
            return _DAL.InsertFabInfo(entity);
        }

        /// <summary>
        /// 获取用户是否可以点赞
        /// </summary>
        /// <returns></returns>
        public bool GetUserFabEnable(string openid)
        {
            bool result = false;
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            int cou = _DAL.GetFabCount(openid, time);
            if (cou < 3)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 通过datatble获取实体集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<ImageEntity> GetEntityList(DataTable dt)
        {
            List<ImageEntity> list = new List<ImageEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                ImageEntity entity = new ImageEntity();
                entity.ImageId = dr["s_id"].ToString();
                entity.UserOpenId = dr["s_useropenid"].ToString();
                entity.FilePath = dr["s_imgpath"].ToString();

                entity.Renom = dr["s_ornum"].ToString();
                string count = dr["cou"].ToString();
                if (count.Length == 0)
                {
                    entity.FabCount = "0";
                }
                else
                {
                    entity.FabCount = count;
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// 插入token
        /// </summary>
        /// <returns></returns>
        public int InsertToken(string token, int type)
        {
            return _DAL.InsertIntoToken(token, type);
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string GetToken(int type)
        {
            return _DAL.GetToken(type);
        }
    }
}
