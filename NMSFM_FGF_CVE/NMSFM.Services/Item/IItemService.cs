//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMSFM.Data;
using System;
using System.Web.Mvc;
using NMSFM.Services.Models;
using NMSFM.ViewModels;
namespace NMSFM.Services.Item
{
	public interface IItemService
	{
		Task<IEnumerable<v_Items>> GetItemsAsync();
		Task<v_Items> GetItemAsync(Guid? itemId);
		Task<List<ItemType>> GetItemTypesAsync();
		Task<List<v_Items>> GetChildItemsByIdAsync(Guid itemId);
		Task<List<v_InventoryItems>> GetInvItemByIdAsync(Guid itemId);
		Task<IEnumerable<v_InventoryItems>> GetInventoryItemListAsync(Guid itemId);
		Task<v_Activities> GetActivitiesByItemIdAsync(Guid id);
		Task<IEnumerable<v_Permits>> GetPermitsByItemIdAsync(Guid id);
		Task<IEnumerable<v_Files>> GetFilesByItemIdAsync(Guid id);
		Task<IEnumerable<Data.Note>> GetNotesByIdAsync(Guid id);
		Task<IEnumerable<UserDefinedValue>> GetUserDefinedValuesByItemIdAsync(Guid id, Guid pTypeId, Guid agency);

		Task<v_Locations> GetLocationAsync(Guid? locationId);

	}
}
