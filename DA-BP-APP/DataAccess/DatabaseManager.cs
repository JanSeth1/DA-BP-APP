using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using DA_BP_APP.Models;

namespace DA_BP_APP.DataAccess
{
    public class DatabaseManager
    {

        // Method to load farmers data
        public DataTable LoadFarmersData()
        {
            string query = "SELECT FarmerID, FirstName, LastName, Address, Barangay, ContactInfo, FarmSize, RegistrationDate FROM Farmers";
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Method to load barangays
        public List<Barangay> LoadBarangays()
        {
            List<Barangay> barangays = new List<Barangay>();
            string query = "SELECT DISTINCT Barangay FROM farmers";
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    barangays.Add(new Barangay { Name = reader["Barangay"].ToString() });
                }
            }
            return barangays;
        }

        // Method to load data based on barangay
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
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Barangay", barangay);
                connection.Open();
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

        // Method to load commodities
        public List<Commodity> LoadCommodities()
        {
            List<Commodity> commodities = new List<Commodity>();
            string query = "SELECT CommodityID, CommodityName FROM commodities";
            using (MySqlConnection connection = new MySqlConnection(DatabaseHelper.GetConnectionString()))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
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
}
