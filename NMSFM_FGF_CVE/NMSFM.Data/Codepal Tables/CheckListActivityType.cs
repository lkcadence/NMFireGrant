namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CheckListActivityType
    {
        public Guid CheckListActivityTypeId { get; set; }

        public Guid CheckListId { get; set; }

        public Guid ActivityTypeId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public short? CheckListOrder { get; set; }
    }
}
