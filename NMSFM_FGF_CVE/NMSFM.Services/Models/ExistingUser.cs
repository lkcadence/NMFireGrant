using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    [Serializable]
    public class ExistingUser
    {
        public Guid UserId { get; set; }
        public Guid? CodepalId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public bool? Inactive { get; set; }
    }
}