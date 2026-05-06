using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;

namespace NMSFM.ViewModels
{
    public class DetailedFGWaterAvailability
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int ComHydrantSys { get; set; }
        public int AvailableWaterCapacity { get; set; }
        public int WaterOnWheelsCapacity { get; set; }
        public int StationWaterCapacity { get; set; }
        public int TankAtStation { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
        public List<FG_App_WaterSources> WaterSources { get; set; }
    }
}
