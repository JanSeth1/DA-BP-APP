using DA_BP_APP.DataAccess;
using System.Collections.Generic;
using DA_BP_APP.Models;


namespace DA_BP_APP.ReportGeneration
{
   public class BarangayReportGenerator
{
    private DatabaseManager _dbManager;

    public BarangayReportGenerator(DatabaseManager dbManager)
    {
        _dbManager = dbManager;
    }

    public List<CommodityData> GenerateBarangayReport(string barangay)
    {
        var data = _dbManager.LoadBarangayData(barangay);
        return data;
    }
}
}

