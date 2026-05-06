namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ActivitySetting
    {
        public Guid ActivitySettingId { get; set; }

        public Guid ActivityTypeId { get; set; }

        [StringLength(50)]
        public string SubTypeLabel { get; set; }

        [StringLength(50)]
        public string NumberLabel { get; set; }

        [StringLength(50)]
        public string ReasonLabel { get; set; }

        [StringLength(50)]
        public string ItemLabel { get; set; }

        [StringLength(50)]
        public string PartyLabel { get; set; }

        [StringLength(50)]
        public string AltPartyLabel { get; set; }

        [StringLength(50)]
        public string InspectorLabel { get; set; }

        [StringLength(50)]
        public string CompleteLabel { get; set; }

        [StringLength(50)]
        public string StatusLabel { get; set; }

        [StringLength(50)]
        public string ReLabel { get; set; }

        [StringLength(50)]
        public string DetailHides { get; set; }

        [StringLength(50)]
        public string TabHides { get; set; }

        public bool PrimaryParty { get; set; }

        [StringLength(100)]
        public string ExternalId { get; set; }

        [StringLength(50)]
        public string CommentsLabel { get; set; }

        [StringLength(50)]
        public string TabCheckList { get; set; }

        [StringLength(50)]
        public string TabFiles { get; set; }

        [StringLength(50)]
        public string TabChildAct { get; set; }

        [StringLength(50)]
        public string TabSig { get; set; }

        [StringLength(50)]
        public string SecondaryInspectorLabel { get; set; }

        [StringLength(50)]
        public string GroupLabel { get; set; }

        [StringLength(50)]
        public string PartyRoleLabel { get; set; }

        [StringLength(50)]
        public string AltPartyRoleLabel { get; set; }

        public Guid rowguid { get; set; }

        [StringLength(50)]
        public string SecAddressLabel { get; set; }

        [StringLength(50)]
        public string TabInvNarr { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }

        public Guid? AgencyId { get; set; }
    }
}
