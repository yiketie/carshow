using CarShow.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarShow.Core
{
    public class SuperClass
    {
        
        /// <summary>
        /// 获取用户openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetUserOpenId(string code)
        {
            string result = string.Empty;
            string html = string.Empty;
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wx4c43cea86a410955&secret=b01e99aa941df6ee9127f68fdf133d86&code=" + code + "&grant_type=authorization_code";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream ioStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
            html = sr.ReadToEnd();
            sr.Close();
            ioStream.Close();
            response.Close();

            string key = "\"openid\":\"";
            int startIndex = html.IndexOf(key);
            if (startIndex != -1)
            {
                int endIndex = html.IndexOf("\",", startIndex);
                string openid = html.Substring(startIndex + key.Length, endIndex - startIndex - key.Length);
                //MyOpenId.Value=openid;
                result = openid;
            }
            else
            {
                result = html;
            }
            return result;
        }



        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string getAccessToken()
        {
            string AccessToken = string.Empty;
            BLL.CarShowBLL _bll = new BLL.CarShowBLL();
            AccessToken = _bll.GetToken(0);
            if (AccessToken == string.Empty)
            {
                string grant_type = "client_credential";//获取access_token填写client_credential   
                string AppId = "wx4c43cea86a410955";//第三方用户唯一凭证  
                string secret = "b01e99aa941df6ee9127f68fdf133d86";//第三方用户唯一凭证密钥，即appsecret   
                                                                   //这个url链接地址和参数皆不能变  
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=" + grant_type + "&appid=" + AppId + "&secret=" + secret;  //访问链接
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream ioStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
                string html = sr.ReadToEnd();
                sr.Close();
                ioStream.Close();
                response.Close();

                string key = "\"access_token\":\"";
                int startIndex = html.IndexOf(key);
                if (startIndex != -1)
                {
                    int endIndex = html.IndexOf("\",", startIndex);
                    string openid = html.Substring(startIndex + key.Length, endIndex - startIndex - key.Length);
                    //MyOpenId.Value=openid;
                    AccessToken = openid;
                    int instrs = _bll.InsertToken(AccessToken,0);
                }
            }
            return AccessToken;
        }



        /// <summary>
        /// 获取ticket
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string getTicket(string access_token2)
        {
            BLL.CarShowBLL _bll = new BLL.CarShowBLL();
            string ticket = null;
            ticket = _bll.GetToken(1);
            if (ticket == string.Empty)
            {
                string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + access_token2 + "&type=jsapi";//这个url链接和参数不能变 

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream ioStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
                string html = sr.ReadToEnd();
                sr.Close();
                ioStream.Close();
                response.Close();

                string key = "\"ticket\":\"";
                int startIndex = html.IndexOf(key);
                if (startIndex != -1)
                {
                    int endIndex = html.IndexOf("\",", startIndex);
                    string openid = html.Substring(startIndex + key.Length, endIndex - startIndex - key.Length);
                    //MyOpenId.Value=openid;
                    ticket = openid;
                    int instrs = _bll.InsertToken(ticket, 1);
                }
                
            }
     
            return ticket;
        }



        /// <summary>
        /// 生成时间错
        /// </summary>
        /// <returns></returns>
        private static string create_timestamp()
        {
            TimeSpan cha = (DateTime.Now - new System.DateTime(1970, 1, 1));
            long t = (long)cha.TotalMilliseconds;
            return (t / 1000).ToString();
        }


        /// <summary>
        /// 获取签名信息
        /// </summary>
        /// <param name="jsapi_ticket"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ShareEntity sign(string url)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string nonce_str = Guid.NewGuid().ToString();
            string timestamp = create_timestamp();
            string string1;
            string signature = "";
            string token = getAccessToken();
            string jsapi_ticket = getTicket(token);
            //注意这里参数名必须全部小写，且必须有序
            string1 = "jsapi_ticket=" + jsapi_ticket +
                      "&noncestr=" + nonce_str +
                      "&timestamp=" + timestamp +
                      "&url=" + url;

            byte[] byteUserPwd = Encoding.UTF8.GetBytes(string1);
            signature = SHA1(byteUserPwd).Trim();
        
            //返回Md5字符串

            ShareEntity entity = new ShareEntity();
            entity.Url = url;
            entity.Timestamp = timestamp;
            entity.NonceStr = nonce_str;
            entity.Signature = signature;
            //ret.Add("url", url);
            //ret.Add("nonceStr", nonce_str);
            //ret.Add("timestamp", timestamp);
            //ret.Add("signature", signature);
            return entity;
        }

        /// <summary>
        /// 进行加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string SHA1(byte[]  s)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider(); 
            byte[] bytes_out = sha1.ComputeHash(s);
            sha1.Dispose();
            string result = BitConverter.ToString(bytes_out);
            result = result.Replace("-", "");
            return result;
        }

    }
}
