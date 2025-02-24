using System.Configuration;  // Required for ConfigurationManager
using MySql.Data.MySqlClient; // Include this for MySQL connections

public class DatabaseHelper
{
    // Centralized method to get the connection string
    public static string GetConnectionString()
    {
        return ConfigurationManager.ConnectionStrings["MyDatabaseConnection"].ConnectionString;
    }
}
