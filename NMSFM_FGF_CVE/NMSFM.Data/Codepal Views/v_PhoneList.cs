namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_PhoneList
    {
        [Key]
        [Column(Order = 0)]
        public Guid PhoneId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid PhoneTypeId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string Extension { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid PartyId { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(25)]
        public string PhoneType { get; set; }

        public int? Sequence { get; set; }
    }
}
