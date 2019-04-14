using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShow.Entity
{
    public class ImageEntity
    {
        /// <summary>
        /// 用户openid
        /// </summary>
        public string UserOpenId { get; set; }

        /// <summary>
        /// 图片id
        /// </summary>
        public string ImageId { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 刊号
        /// </summary>
        public string Renom { get; set; }

        /// <summary>
        /// 赞总数
        /// </summary>
        public string FabCount { get; set; }
    }
}
