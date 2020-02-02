using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class AppClaim
    {
        public Guid Id { get; set; }=new Guid();
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
