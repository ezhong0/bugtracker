using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Views.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public int RoleId { get; set; }
    }
}