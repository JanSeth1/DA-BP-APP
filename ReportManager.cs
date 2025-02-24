using System.Collections.Generic;
using System.Data;
using System.Linq;
using YourNamespace.Models;

namespace DA_BP_APP.Services
{
    public class ReportManager
    {
        private readonly DatabaseManager _dbManager;

        public ReportManager(DatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        public List<ReportData> GetBarangayReport(int year, string commodity)
        {
            string query = @"
                SELECT Barangay, SUM(Quantity) AS TotalQuantity, SUM(Revenue) AS TotalRevenue
                FROM Harvest
                WHERE YEAR(HarvestDate) = @Year AND CommodityName = @Commodity
                GROUP BY Barangay";

            var parameters = new Dictionary<string, object>
            {
                { "@Year", year },
                { "@Commodity", commodity }
            };

            DataTable result = _dbManager.ExecuteQuery(query, parameters);

            return result.AsEnumerable().Select(row => new ReportData
            {
                Barangay = row["Barangay"].ToString(),
                TotalQuantity = Convert.ToDouble(row["TotalQuantity"]),
                TotalRevenue = Convert.ToDouble(row["TotalRevenue"])
            }).ToList();
        }
    }
}
