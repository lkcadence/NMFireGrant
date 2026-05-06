

namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class v_AddressPartyandRole
    {
            
        [Key]
        [Column(Order = 0)]
        public Guid AddressId { get; set; }
            
        [Key]
        [Column(Order = 1)]
        public Guid PartyId { get; set; }

        [StringLength(150)]
        public string PartyName { get; set; }

        public Guid? RoleTypeId { get; set; }

        [StringLength(50)]
        public string RoleType { get; set; }

        public bool Inactive { get; set; }

        public bool PTYInact { get; set; }

        public bool EmployeeType { get; set; }
    }
}
