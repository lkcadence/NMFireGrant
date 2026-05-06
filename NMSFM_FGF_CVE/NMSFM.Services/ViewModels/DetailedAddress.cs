using NMSFM.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NMSFM.ViewModels
{
	public class DetailedAddress
	{
		public Guid AddressId { get; set; }
		public bool Inactive { get; set; }
		public string AddressType { get; set; }
		public Guid AddressTypeId { get; set; }
		public string AddressCode { get; set; }
		public string AddressNumber { get; set; }
		public string Direction { get; set; }
		public string Address { get; set; }
		public string SubAddress { get; set; }
		public string City { get; set; }
		public string Suffix { get; set; }
		public string StateAbbr { get; set; }
		public Guid? StateId { get; set; }
		public string Zip { get; set; }
		public Guid? ZipId { get; set; }
		public string Comment { get; set; }
		public bool POBox { get; set; }
		public string Country { get; set; }
		public Guid? CountryId { get; set; }
		public string County { get; set; }
		public Guid? CountyId { get; set; }
		public string Region { get; set; }
		public Guid? RegionId { get; set; }
		public string Longitude { get; set; }
		public string Latitude { get; set; }
		public string Map { get; set; }
		public string Lot { get; set; }
		public string Block { get; set; }
		public string OccupancyType { get; set; }
		public Guid? OccupancyTypeId { get; set; }
		public string PropertyUseType { get; set; }
		public Guid? PropertyUseTypeId { get; set; }
		public string TaxParcel { get; set; }
		public string LegalDesc { get; set; }
		public Guid ParentAddressId { get; set; }
		public byte[] MapData { get; set; }


		public IEnumerable<DetailedAddressParty> AddressParties { get; set; }
		public IEnumerable<DetailedAddressItem> AddressItems { get; set; }
		public List<UserDefinedValue> UserValues { get; set; }
		//public IEnumerable<UserDefinedValue> UserValues { get; set; }
		public IEnumerable<AttachedImages> AttachedImages { get; set; }
		public IEnumerable<AttachedImages> AttachedPdfs { get; set; }
		public IEnumerable<AttachedPermits> AttachedPermits { get; set; }
		public IEnumerable<AttachedActivities> AttachedActivities { get; set; }
		public IEnumerable<AttachedComplaints> AttachedComplaints { get; set; }
		public IEnumerable<AttachedLocations> AttachedLocations { get; set; }
		public IEnumerable<AttachedLocationBases> AttachedLocationBases { get; set; }
		public IEnumerable<AttachedNotes> AttachedNotes { get; set; }
		public IEnumerable<AttachedProjects> AttachedProjects { get; set; }
		public IEnumerable<SearchAddress> AttachedRelatedAddresses { get; set; }
		public IEnumerable<HydrantAddresses> HydrantAddresses { get; set; }
	}
}