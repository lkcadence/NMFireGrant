using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMSFM.Data;

namespace NMSFM.ViewModels
{
    public class DetailedFGApparatus
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int ApparatusPartOfProject { get; set; }
        public int PumpTestsConducted { get; set; }
        public string ExplainNoPumpTests { get; set; }
        public int HoseTestConducted { get; set; }
        public string ExplainNoHostTests { get; set; }
        public DateTime DateEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsValid { get; set; }
        public string InvalidText { get; set; }
        public List<FG_App_ApparatusEquipment> ApparatusEquipment { get; set; }
    }

}
