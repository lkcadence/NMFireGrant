using System;
using System.Collections.Generic;
using NMSFM.Data;

namespace NMSFM.ViewModels
{
    public partial class DetailedInspector
    {
        public Guid InspectorId { get; set; }
        public string Code { get; set; }
        public string InspectorName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
        public Guid? AgencyId { get; set; }
        public string InspectorPhone { get; set; }
        public byte[] Signature { get; set; }
        public bool? LoggedIn { get; set; }
        public bool Madmin { get; set; }
        public Guid? GroupId { get; set; }
        public bool? Inactive { get; set; }
        public string ExternalId { get; set; }
        public string Email { get; set; }
        public bool? CodeExempt { get; set; }
        public bool GlobalUser { get; set; }
        public Guid rowguid { get; set; }
        public string RCLevel { get; set; }
        public string ActiveModules { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }
        public bool DisablePWChange { get; set; }
        public string SecQOne { get; set; }
        public string SecAOne { get; set; }
        public string SecQTwo { get; set; }
        public string SecATwo { get; set; }
        public string Title { get; set; }
    }
}
