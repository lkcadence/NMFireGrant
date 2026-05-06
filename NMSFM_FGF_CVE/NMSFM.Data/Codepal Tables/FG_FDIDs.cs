namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class FG_FDIDs
    {
        [Key]
        public string FDID { get; set; }

        [StringLength(50)]
        public string FireDepartment { get; set; }
        public bool Inactive { get; set; }
    }
}
