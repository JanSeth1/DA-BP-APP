using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

public class DatabaseManager
{
    private string connectionString = "Server=localhost;Port=3306;Database=oma;Uid=root;Pwd=mysql;";

    public DataTable LoadFarmersData()
    {
        string query = "SELECT FarmerID, FirstName, LastName, Address, Barangay, ContactInfo, FarmSize, RegistrationDate FROM Farmers";
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    public List<Barangay> LoadBarangays()
    {
        List<Barangay> barangays = new List<Barangay>();
        string query = "SELECT DISTINCT Barangay FROM farmers";
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                barangays.Add(new Barangay { Name = reader["Barangay"].ToString() });
            }
        }
        return barangays;
    }

    public List<CommodityData> LoadBarangayData(string barangay)
    {
        string query = @"
            SELECT c.CommodityName, SUM(h.Quantity) AS TotalQuantity
            FROM Harvest h
            JOIN farmers f ON h.FarmerID = f.FarmerID
            JOIN commodities c ON h.CommodityID = c.CommodityID
            WHERE f.Barangay = @Barangay
            GROUP BY c.CommodityName";

        List<CommodityData> commodityData = new List<CommodityData>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Barangay", barangay);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                commodityData.Add(new CommodityData
                {
                    CommodityName = reader["CommodityName"].ToString(),
                    TotalQuantity = Convert.ToDouble(reader["TotalQuantity"])
                });
            }
        }
        return commodityData;
    }

    public List<Commodity> LoadCommodities()
    {
        List<Commodity> commodities = new List<Commodity>();
        string query = "SELECT CommodityID, CommodityName FROM commodities";
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                commodities.Add(new Commodity
                {
                    CommodityID = Convert.ToInt32(reader["CommodityID"]),
                    CommodityName = reader["CommodityName"].ToString()
                });
            }
        }
        return commodities;
    }
}
