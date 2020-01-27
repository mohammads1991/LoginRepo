using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Role
    {
        public string Id { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
