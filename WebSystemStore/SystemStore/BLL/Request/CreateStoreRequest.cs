using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Request
{
    public class CreateStoreRequest
    {

        [Required(ErrorMessage = "Vui Lòng Nhập Tên Cửa Hàng")]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string? NameNoDiacritic { get; set; }

        public string? Image { get; set; }

        public string TimeOpen { get; set; }

        public string TimeClose { get; set; }

        [MaxLength(50)]
        public string? Preferential { get; set; }

        [Range(0, 5)]
        public double StarEvaluate { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Địa Chỉ")]
        [MaxLength(150)]
        public string Address { get; set; }
        public string? AddressLocation { get; set; }
        [MaxLength(150)]
        public string? AddressNoDiacritic { get; set; }

        public int WardID { get; set; }
        public int DistrictID { get; set; }  // Add DistrictID for more clarity
        public int CityID { get; set; }      // Add CityID for location
        public int ContentID { get; set; }

        [MaxLength(50)]
        public string? AdminName { get; set; }

        public DateTime? AdminTime { get; set; }

        [Range(0, 2)]
        public int? Status { get; set; }

    }

}
