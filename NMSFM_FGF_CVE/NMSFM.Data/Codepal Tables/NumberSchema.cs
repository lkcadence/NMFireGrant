namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NumberSchema")]
    public partial class NumberSchema
    {
        public Guid NumberSchemaId { get; set; }

        public Guid ModuleId { get; set; }

        [Required]
        [StringLength(150)]
        public string Part1 { get; set; }

        [StringLength(150)]
        public string Part2 { get; set; }

        [StringLength(150)]
        public string Part3 { get; set; }

        [StringLength(150)]
        public string Part4 { get; set; }

        [StringLength(150)]
        public string Part5 { get; set; }

        [StringLength(150)]
        public string Part6 { get; set; }

        public Guid? AddressId { get; set; }

        [StringLength(50)]
        public string CurrentNumber { get; set; }

        public Guid rowguid { get; set; }
    }
}
