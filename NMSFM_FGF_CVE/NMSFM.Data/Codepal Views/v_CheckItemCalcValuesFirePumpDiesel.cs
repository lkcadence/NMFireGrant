namespace NMSFM.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class v_CheckItemCalcValuesFirePumpDiesel
    {
		[Key]
		[Column(Order = 0)]
		public Guid? AddressId { get; set; }

        public DateTime? InspectionDate { get; set; }
		[Key]
		[Column(Order = 1)]
		public Guid InspectionId { get; set; }

        [StringLength(50)]
        public string InspectionNumber { get; set; }

        public Guid? ItemId { get; set; }

        [StringLength(3000)]
        public string Point0FirePumpFlowsGPM { get; set; }

        [StringLength(50)]
        public string Point0FirePumpOutletpsi { get; set; }

        [StringLength(50)]
        public string Point0SuctionPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point0NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point0RPM { get; set; }

        [StringLength(3000)]
        public string Point0L1 { get; set; }

        [StringLength(3000)]
        public string Point0L02 { get; set; }

        [StringLength(3000)]
        public string Point0L03 { get; set; }

        [StringLength(3000)]
        public string PrevPoint0FlowGPM { get; set; }

        [StringLength(3000)]
        public string PrevPoint0NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point1FirePumpFlowsGPM { get; set; }

        [StringLength(50)]
        public string Point1FirePumpOutletpsi { get; set; }

        [StringLength(50)]
        public string Point1SuctionPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point1NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point1RPM { get; set; }

        [StringLength(3000)]
        public string Point1L1 { get; set; }

        [StringLength(3000)]
        public string Point1L02 { get; set; }

        [StringLength(3000)]
        public string Point1L03 { get; set; }

        [StringLength(3000)]
        public string PrevPoint1FlowGPM { get; set; }

        [StringLength(3000)]
        public string PrevPoint1NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point2FirePumpFlowsGPM { get; set; }

        [StringLength(50)]
        public string Point2FirePumpOutletpsi { get; set; }

        [StringLength(50)]
        public string Point2SuctionPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point2NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point2RPM { get; set; }

        [StringLength(3000)]
        public string Point2L1 { get; set; }

        [StringLength(3000)]
        public string Point2L02 { get; set; }

        [StringLength(3000)]
        public string Point2L03 { get; set; }

        [StringLength(3000)]
        public string PrevPoint2FlowGPM { get; set; }

        [StringLength(3000)]
        public string PrevPoint2NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point3FirePumpFlowsGPM { get; set; }

        [StringLength(50)]
        public string Point3FirePumpOutletpsi { get; set; }

        [StringLength(50)]
        public string Point3SuctionPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point3NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point3RPM { get; set; }

        [StringLength(3000)]
        public string Point3L1 { get; set; }

        [StringLength(3000)]
        public string Point0302 { get; set; }

        [StringLength(3000)]
        public string Point3L03 { get; set; }

        [StringLength(3000)]
        public string PrevPoint3FlowGPM { get; set; }

        [StringLength(3000)]
        public string PrevPoint3NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point4FirePumpFlowsGPM { get; set; }

        [StringLength(50)]
        public string Point4FirePumpOutletpsi { get; set; }

        [StringLength(50)]
        public string Point4SuctionPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point4NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point4RPM { get; set; }

        [StringLength(3000)]
        public string Point4L1 { get; set; }

        [StringLength(3000)]
        public string Point4L02 { get; set; }

        [StringLength(3000)]
        public string Point4L03 { get; set; }

        [StringLength(3000)]
        public string PrevPoint4FlowGPM { get; set; }

        [StringLength(3000)]
        public string PrevPoint4NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point5FirePumpFlowsGPM { get; set; }

        [StringLength(50)]
        public string Point5FirePumpOutletpsi { get; set; }

        [StringLength(50)]
        public string Point5SuctionPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point5NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point5RPM { get; set; }

        [StringLength(3000)]
        public string Point5L1 { get; set; }

        [StringLength(3000)]
        public string Point5L02 { get; set; }

        [StringLength(3000)]
        public string Point5L03 { get; set; }

        [StringLength(3000)]
        public string PrevPoint5FlowGPM { get; set; }

        [StringLength(3000)]
        public string PrevPoint5NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point6FirePumpFlowsGPM { get; set; }

        [StringLength(50)]
        public string Point6FirePumpOutletpsi { get; set; }

        [StringLength(50)]
        public string Point6SuctionPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point6NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point6RPM { get; set; }

        [StringLength(3000)]
        public string Point6L1 { get; set; }

        [StringLength(3000)]
        public string Point6L02 { get; set; }

        [StringLength(3000)]
        public string Point6L03 { get; set; }

        [StringLength(3000)]
        public string PrevPoint6FlowGPM { get; set; }

        [StringLength(3000)]
        public string PrevPoint6NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point7FirePumpFlowsGPM { get; set; }

        [StringLength(50)]
        public string Point7FirePumpOutletpsi { get; set; }

        [StringLength(50)]
        public string Point7SuctionPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point7NetPressurepsi { get; set; }

        [StringLength(3000)]
        public string Point7RPM { get; set; }

        [StringLength(3000)]
        public string Point7L1 { get; set; }

        [StringLength(3000)]
        public string Point7L02 { get; set; }

        [StringLength(3000)]
        public string Point7L03 { get; set; }

        [StringLength(3000)]
        public string PrevPoint7FlowGPM { get; set; }

        [StringLength(3000)]
        public string PrevPoint7NetPressurepsi { get; set; }
    }
}
