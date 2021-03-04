using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UMS.Models
{
    public class Photo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Url { get; set; }
    }
}
