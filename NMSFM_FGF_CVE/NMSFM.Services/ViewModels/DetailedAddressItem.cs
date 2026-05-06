using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
    public class DetailedAddressItem
    {
        
        public Guid ItemId { get; set; }

        public string Description { get; set; }

        public Guid? ItemTypeId { get; set; }

       
        public string ItemType { get; set; }

  
        public string Barcode { get; set; }

      
        public string Location { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? LocationId { get; set; }

        public Guid? StatusId { get; set; }

   
        public string Status { get; set; }

        public string FullAddress { get; set; }

  
        public string Comments { get; set; }

        public decimal? Cost { get; set; }

   
        public string ItemNumber { get; set; }

        public DateTime? InServiceDate { get; set; }

        public DateTime? NextServiceDate { get; set; }

        public Guid? ServiceTypeId { get; set; }

        public bool Inactive { get; set; }

        public Guid? ActivityTypeId { get; set; }

        public Guid? AgencyId { get; set; }

        public string LocationBase { get; set; }
        public string LocationType { get; set; }

        public Guid? ItemCategoryId { get; set; }
        public string InspectionType { get; set; }
        public string ServiceType { get; set; }

        public Guid? InvItemId { get; set; }

        public Guid? LocationBaseId { get; set; }

        public string ExternalId { get; set; }
    }
}