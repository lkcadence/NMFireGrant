namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UDFCategoriesIcon
    {
        [Key]
        public Guid UserDefCategoryId { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] Icon { get; set; }

        public Guid rowguid { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime DateInserted { get; set; }
    }
}
