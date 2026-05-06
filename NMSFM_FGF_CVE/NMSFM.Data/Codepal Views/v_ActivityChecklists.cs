namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ActivityChecklists
    {
        public Guid? InspectionId { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid CheckListId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string CheckListName { get; set; }

        public short? CheckListOrder { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Inactive { get; set; }

        public int? QuestionCount { get; set; }

        public int? AnsweredQuestionCount { get; set; }

        public int? FailedQuestionCount { get; set; }

        public int? NAQuestionCount { get; set; }

        public int? NOQuestionCount { get; set; }

        public int? ViolationCount { get; set; }

        public bool? NotPrinted { get; set; }
    }
}
