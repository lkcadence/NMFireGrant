using System;
using System.Collections.Generic;
using NMSFM.Data;

namespace NMSFMFireGrantWF.Application
{
  /// <summary>
  /// Remaps prior-year child row IDs to the current application before ViewState bind.
  /// </summary>
  public static class PrefillChildRowRemap
  {
    public static void RemapApparatusEquipment(
      IList<FG_App_ApparatusEquipment> rows, Guid currentApplicationId)
    {
      if (rows == null) { return; }
      foreach (var row in rows)
      {
        row.ApplicationId = currentApplicationId;
        row.ApparatusId = Guid.NewGuid();
      }
    }

    public static void RemapAidDistricts(
      IList<FG_App_AidDistricts> rows, Guid currentApplicationId)
    {
      if (rows == null) { return; }
      foreach (var row in rows)
      {
        row.ApplicationId = currentApplicationId;
        row.AidDistrictId = Guid.NewGuid();
      }
    }

    public static void RemapWaterSources(
      IList<FG_App_WaterSources> rows, Guid currentApplicationId)
    {
      if (rows == null) { return; }
      foreach (var row in rows)
      {
        row.ApplicationId = currentApplicationId;
        row.WaterSourceId = Guid.NewGuid();
      }
    }

    public static void RemapCommunicationEquipment(
      IList<FG_App_CommunicationEquipment> rows, Guid currentApplicationId)
    {
      if (rows == null) { return; }
      foreach (var row in rows)
      {
        row.ApplicationId = currentApplicationId;
        row.CommunicationEquipmentId = Guid.NewGuid();
      }
    }

    public static void RemapHazardThreatEvents(
      IList<FG_App_HazardThreatEvents> rows, Guid currentApplicationId)
    {
      if (rows == null) { return; }
      foreach (var row in rows)
      {
        row.ApplicationId = currentApplicationId;
        row.HazardId = Guid.NewGuid();
      }
    }
  }
}
