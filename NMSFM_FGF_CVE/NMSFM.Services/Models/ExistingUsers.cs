using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodepalWeb.ViewModels
{
    public class ExistingUsers
    {
        public Guid UserId { get; set; }
        public Guid? CodepalId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public bool? Inactive { get; set; }
    }
}