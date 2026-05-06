namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Mileage
    {
        [Key]
        [Column(Order = 0)]
        public Guid MileageId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid PartyId { get; set; }

        [StringLength(50)]
        public string InspectorName { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime MileageDate { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Miles { get; set; }

        [StringLength(200)]
        public string Comment { get; set; }

        public Guid? AgencyId { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }
    }
}
