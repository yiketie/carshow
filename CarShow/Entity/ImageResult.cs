using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShow.Entity
{
    public class ImageResult : BaseResult
    {
        public List<ImageEntity> data { get; set; }
    }
}
