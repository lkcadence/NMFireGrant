namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_SignOffCompletes
    {
        [Key]
        public Guid ObjectTypeId { get; set; }

        public Guid? UserId { get; set; }

        public Guid? ObjectId { get; set; }

        public bool? Complete { get; set; }
    }
}
