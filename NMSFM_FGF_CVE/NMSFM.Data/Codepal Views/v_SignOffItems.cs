namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SignOffItems
    {
		[Key]
		[Column(Order = 0)]
		public Guid SignOffItemId { get; set; }

        public short SOItemSequence { get; set; }

        [StringLength(10)]
        public string SignOffItemType { get; set; }

        [StringLength(100)]
        public string LabelText { get; set; }

        [StringLength(1000)]
        public string Choices { get; set; }

        public bool Required { get; set; }

        public Guid SignOffTypeId { get; set; }

    }
}
