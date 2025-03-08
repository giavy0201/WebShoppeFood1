using System.ComponentModel.DataAnnotations;

namespace DAL.ModelsRequest
{
    public class UpdateStoreReq
    {
        public int StoreID { get; set; }  // ID của cửa hàng cần cập nhật

        public string Name { get; set; }  // Tên cửa hàng mới

        public string TimeOpen { get; set; }  // Giờ mở cửa (dạng chuỗi để có thể chuyển thành TimeSpan)

        public string TimeClose { get; set; }  // Giờ đóng cửa (dạng chuỗi để có thể chuyển thành TimeSpan)

        public string Preferential { get; set; }  // Ưu đãi hoặc khuyến mãi của cửa hàng

        public string Location { get; set; }  // Địa chỉ cửa hàng
        //public int DistrictId { get; set; }            // ID của quận (thay vì DistrictID)
        //public int CityId { get; set; }                // ID của thành phố (thay vì CityID)
        public int WardID { get; set; }  // ID của phường

        public int ContentID { get; set; }  // ID của nội dung (hoặc một thông tin khác về cửa hàng)

        public string AdminName { get; set; }  // Tên quản lý cửa hàng

        public DateTime? AdminTime { get; set; }  // Thời gian chỉnh sửa (nếu cần)

        public int Status { get; set; }  // Trạng thái cửa hàng (ví dụ: 0: đóng cửa, 1: mở cửa)

        public string Img { get; set; }  // Đường dẫn đến hình ảnh của cửa hàng (có thể null)
    }
}
