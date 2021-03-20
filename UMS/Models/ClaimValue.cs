using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UMS.Models
{
    public class ClaimValue
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ClaimsType { get; set; }
        public string ClaimsValue { get; set; }
    }
}
