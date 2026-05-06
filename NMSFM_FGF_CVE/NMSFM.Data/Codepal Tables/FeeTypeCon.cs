namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeeTypeCon")]
    public partial class FeeTypeCon
    {
        public Guid FeeTypeConId { get; set; }

        public Guid rowguid { get; set; }

        public Guid FeeTypeId { get; set; }

        public int ConFeeType { get; set; }

        public Guid? AgreementTypeId { get; set; }

        public Guid? UserDefFieldId { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
