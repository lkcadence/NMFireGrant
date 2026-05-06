namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class FG_App_Help
    {
        [Key]
        public Guid HelpId { get; set; }

        [StringLength(50)]
        public string Page { get; set; }
        [StringLength(50)]
        public string Section { get; set; }
        public byte[]  Image { get; set; }
        public int Number { get; set; }
        public string HelpText { get; set; }
        public bool Inactive { get; set; }
        public bool AdminOnly { get; set; }
    }
}
