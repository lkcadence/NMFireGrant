namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SignOffItem")]
    public partial class SignOffItem
    {
        public Guid SignOffItemId { get; set; }

        public Guid SignOffTypeId { get; set; }

        [StringLength(10)]
        public string SignOffItemType { get; set; }

        [StringLength(100)]
        public string LabelText { get; set; }

        [StringLength(1000)]
        public string Choices { get; set; }

        public short? SOItemSequence { get; set; }

        public bool Required { get; set; }

        [StringLength(3000)]
        public string DefaultAns { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
