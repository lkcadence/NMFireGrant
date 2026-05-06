namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeeTypePen")]
    public partial class FeeTypePen
    {
        public Guid FeeTypePenId { get; set; }

        public Guid FeeTypeId { get; set; }

        public int NumberOfDays { get; set; }

        public decimal InitialPenalty { get; set; }

        public int? AmountPer { get; set; }

        public decimal? RatePer { get; set; }

        public Guid? UserDefFieldId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
