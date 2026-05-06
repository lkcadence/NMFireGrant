namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectTypesDefault
    {
        [Key]
        public Guid ProjectTypesDefaultsId { get; set; }

        public Guid ProjectTypeId { get; set; }

        public Guid RecordTypeId { get; set; }

        public Guid? AddressTypeId { get; set; }

        public Guid? OccupancyTypeId { get; set; }

        public Guid? PropertyUseTypeId { get; set; }

        public bool MainObject { get; set; }

        public bool Progressive { get; set; }

        public short? Sequence { get; set; }

        public short? ScheduleInterval { get; set; }

        public Guid? RoleTypeId { get; set; }

        public Guid? UserId { get; set; }

        public bool IsPermit { get; set; }

        public bool Inactive { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public Guid? HaltStatusId { get; set; }
    }
}
