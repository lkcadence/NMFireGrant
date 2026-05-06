using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NMSFM.ViewModels
{
    public class PartyWebAccess
    {
        public Guid PartyId { get; set; }
        public Guid AgencyId { get; set; }
        [Required(ErrorMessage = "Please enter a username.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Locked { get; set; } 
    }
}
