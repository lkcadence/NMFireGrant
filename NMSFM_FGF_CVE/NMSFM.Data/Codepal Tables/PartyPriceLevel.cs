namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PartyPriceLevel
    {
        [Key]
        public Guid PartyPriceLevelsId { get; set; }

        public Guid PartyId { get; set; }

        public Guid InvItemTypeId { get; set; }

        public short PriceLevel { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
