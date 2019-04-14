using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShow.Entity
{
    public class BaseResult
    {
        /// <summary>
        /// 返回的状态 0-失败；1-成功
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string desc { get; set; }
    }
}
