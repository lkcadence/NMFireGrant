namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Agency")]
    public partial class Agency
    {
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

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
