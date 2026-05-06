namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DefaultFee
    {
        public Guid DefaultFeeId { get; set; }

        public Guid RecordId { get; set; }

        public Guid FeeTypeId { get; set; }

        public bool ForReInspection { get; set; }

        [StringLength(2)]
        public string ReInspectionLetter { get; set; }

        public bool ReInspForward { get; set; }

        [StringLength(2)]
        public string EndReInspectionLetter { get; set; }

        [StringLength(100)]
        public string FeeAmount { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public Guid? FeeSchedId { get; set; }


    }
}
