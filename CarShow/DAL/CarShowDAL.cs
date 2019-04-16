using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CarShow.Entity;

namespace CarShow.DAL
{
    public class CarShowDAL
    {
        //static string conn = "server=;uid=root;pwd=;database=;trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=false;";
        public static string MysqlCnnectionString = "Server =; Database =; Uid =root; Pwd =;Pooling=true; Max Pool Size=20;Min Pool Size=10";
        ADONetHelper.MySql.MySqlClient client = null;


        public CarShowDAL()
        {
            MySql.Data.MySqlClient.MySqlConnection mySqlConnection = new MySql.Data.MySqlClient.MySqlConnection();
            mySqlConnection.ConnectionString = MysqlCnnectionString;
            client = new ADONetHelper.MySql.MySqlClient(mySqlConnection);

        }
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertImageInfo(ImageEntity entity)
        {
            bool result = false;
            string sql = "insert into cs_m_imageinfo  (s_id,s_useropenid,s_imgpath) VALUES('{0}','{1}','{2}') ";
            sql = string.Format(sql, entity.ImageId, entity.UserOpenId, entity.FilePath);
            sql = sql.Replace("\\", "\\\\");
            int exresult = client.ExecuteNonQuery(sql);
            if (exresult > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 通过刊号获取图片id
        /// </summary>
        /// <param name="renum"></param>
        /// <returns></returns>
        public DataTable GetImageByRenum(int renum)
        {
            string sql = @"SELECT
                                    t.s_id,
	                                t.s_imgpath,
	                                t.s_ornum,
	                                t.s_useropenid,
	                                m.cou
                                FROM
                                    cs_m_imageinfo t
                                LEFT JOIN(
                                    SELECT
                                        n.s_renum,
                                        count(*) cou
                                    FROM
                                        cs_m_fabrecord n
                                    WHERE
                                        n.s_renum ={0}
                                ) m ON t.s_ornum = m.s_renum
                                WHERE

                                    t.s_ornum = {0}";
            sql = string.Format(sql, renum);
            DataTable dt = client.GetDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 通过刊号获取图片id
        /// </summary>
        /// <param name="renum"></param>
        /// <returns></returns>
        public DataTable GetImageByImageId(string imageid)
        {
            string sql = @"SELECT
                                    t.s_id,
	                                t.s_imgpath,
	                                t.s_ornum,
	                                t.s_useropenid,
	                                m.cou
                                FROM
                                    cs_m_imageinfo t
                                LEFT JOIN(
                                    SELECT
                                        n.s_renum,
                                        count(*) cou
                                    FROM
                                        cs_m_fabrecord n
                                    WHERE
                                        n.s_imageid ={0}
                                ) m ON t.s_ornum = m.s_renum
                                WHERE
                                    t.s_id = {0}";
            sql = string.Format(sql, imageid);
            DataTable dt = client.GetDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 获取最新最热海报
        /// </summary>
        /// <param name="type">0-最新；1-最热</param>
        /// <returns></returns>
        public DataTable GetNewestImage(int type)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT
                                        t.s_id,
	                                    t.s_ornum,
	                                    t.s_imgpath,
	                                    t.s_useropenid,
	                                    case when m.labsum > 0 then m.labsum else 0 end cou
                                    FROM
                                        cs_m_imageinfo t
                                    LEFT JOIN(
                                        SELECT
                                            n.s_imageid,
                                            sum(1) labsum
                                        FROM
                                            cs_m_fabrecord n
                                    ) m ON t.s_id = m.s_imageid
                                    order by   {0}  desc LIMIT 10";
            string orderpara = "t.d_addtime";
            if (type == 1)
            {
                orderpara = "labsum";
            }
            sql = string.Format(sql, orderpara);
            dt = client.GetDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 插入点赞表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertFabInfo(Entity.ImageEntity entity)
        {
            bool result = false;
            string sql = @"INSERT INTO cs_m_fabrecord (
                                                    s_fabid,
	                                                s_openid,
	                                                s_renum,
	                                                s_fabtime,
	                                                s_imageid
                                                )
                                                VALUES
                                                    (
                                                        '{0}',
                                                        '{1}',
                                                        '{2}',
                                                        '{3}',
                                                        '{4}'
                                                    )";
            sql = string.Format(sql, Guid.NewGuid().ToString(),
                entity.UserOpenId, entity.Renom, DateTime.Now.ToString("yyyy-MM-dd"), entity.ImageId);
            int res = client.ExecuteNonQuery(sql);
            if (res > 0)
                result = true;
            return result;
        }


        /// <summary>
        /// 获取此人点赞次数
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetFabCount(string openid, string time)
        {
            int count = 0;
            string sql = "select  count(*)cou from cs_m_fabrecord t where t.s_openid='{0}' and t.s_fabtime='{1}'";
            sql = string.Format(sql,openid,time);
            DataTable dt = client.GetDataTable(sql);
            count = Convert.ToInt16(dt.Rows[0][0]);
            return count;
        }


        /// <summary>
        /// 插入token
        /// </summary>
        /// <returns></returns>
        public int InsertIntoToken(string tokencode,int type)
        {
            int result = 0;
            string sql = "insert  into cs_m_tokeninfo(tokencode,type) values('{0}',{1})";
            sql = string.Format(sql,tokencode,type);
            result = client.ExecuteNonQuery(sql);
            return result;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string GetToken(int type)
        {
            string token = string.Empty;
            string sql = "select  t.tokencode, date_format(t.addtime, '%Y-%m-%d %H:%i:%s') addtime from cs_m_tokeninfo t  where type={0} order by t.addtime desc LIMIT 1";
            sql = string.Format(sql,type);
            DataTable dt = client.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                string addtime = dt.Rows[0]["addtime"].ToString();
                DateTime dtadd = Convert.ToDateTime(addtime);
                TimeSpan ts = DateTime.Now - dtadd;
                if (ts.TotalMinutes <= 100)
                {
                    token = dt.Rows[0]["tokencode"].ToString();
                }
            }
            return token;
        }
    }
}
