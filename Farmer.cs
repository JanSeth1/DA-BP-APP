namespace DA_BP_APP.Models
{
    public class Farmer
    {
        public int FarmerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Barangay { get; set; }
        public string ContactInfo { get; set; }
        public double FarmSize { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
