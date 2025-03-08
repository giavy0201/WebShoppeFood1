using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BLL.Model.Store
{
    public class ReqUpdateStore
    {
        public int StoreID { get; set; }
        public string Name { get; set; }
        public string? Img { get; set; }
        public string TimeOpen { get; set; }
        public string TimeClose { get; set; }
        public string Preferential { get; set; }
        public string Location { get; set; }
       // public int DistrictId { get; set; }            // ID của quận (thay vì DistrictID)
       // public int CityId { get; set; }                // ID của thành phố (thay vì CityID)
        public int WardID { get; set; }
        public int ContentID { get; set; }
        public string AdminName { get; set; }
        [Range(0, 2)]
        public int Status { get; set; }
        public IFormFile formFile { get; set; }
    }
}
