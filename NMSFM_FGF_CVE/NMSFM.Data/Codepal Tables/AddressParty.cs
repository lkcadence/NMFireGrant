namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AddressParty
    {
        public Guid AddressPartyId { get; set; }

        public Guid PartyID { get; set; }

        public Guid AddressID { get; set; }

        public Guid rowguid { get; set; }

        public bool Inactive { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid? RoleTypeId { get; set; }

        [StringLength(50)]
        public string ExternalValue { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
