namespace BLL.Models.DTOs
{
    public class StoreSystem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public TimeSpan TimeOpen { get; set; }
        public TimeSpan TimeClose { get; set; }
        public string? Preferential { get; set; }
        public double StarEvaluate { get; set; }
        public string Address { get; set; }
        //public int DistrictId { get; set; }            // ID của quận (thay vì DistrictID)
        //public int CityId { get; set; }                // ID của thành phố (thay vì CityID)
        public int WardID { get; set; }
        public int ContentID { get; set; }
        public string? AddressLocation { get; set; }
        public string? AdminName { get; set; }
        public DateTime? AdminTime { get; set; }
        public int? Status { get; set; }
    }
}
