using System;

namespace NMSFM.ViewModels
{
    public class StateViewModel
    {
        public Guid StateId { get; set; }

        public string StateAbbr { get; set; }

        public string State { get; set; }

        public Guid? CountryId { get; set; }
    }
}