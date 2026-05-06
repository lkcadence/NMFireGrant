namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_Agency
    {
        [Key]
        [Column(Order = 0)]
        public Guid AgencyId { get; set; }

        [StringLength(50)]
        public string AgencyName { get; set; }

        [StringLength(50)]
        public string AgencySubName { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public Guid? StateId { get; set; }

        [StringLength(50)]
        public string Zip { get; set; }

        public Guid? CountryId { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [Column(TypeName = "image")]
        public byte[] ReportImage { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid rowguid { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(5)]
        public string StateAbbr { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string AgCity { get; set; }

        [StringLength(50)]
        public string AgZip { get; set; }
    }
}
