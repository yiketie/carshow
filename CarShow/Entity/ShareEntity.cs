using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShow.Entity
{
    public class ShareEntity
    {
        public string Url { get; set; }

        public string NonceStr { get; set;
         }

        public string Timestamp { get; set; }

        public string Signature { get; set; }
    }
}
