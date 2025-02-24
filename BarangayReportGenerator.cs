using System.Collections.Generic;

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
