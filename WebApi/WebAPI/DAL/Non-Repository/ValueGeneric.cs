namespace DAL.Non_Repository
{
    public static class ValueGeneric
    {
        public const int OffActive = 0;
        public const int Active = 1;
        public const int Inactive = 2;
        public const int Blocked = 3;  // Tài khoản bị khóa
        public const int Pending = 4;  // Đang chờ duyệt

        public const int Delete = 99;
    }

    public static class ValueOrder
    {
        public const int NewOrder = 0;
       // public const int NewOrder = 1;
        public const int Order = 1;
        public const int Cancel = 2;
        public const int Done = 3;
        public const int ShipperConfirmed = 4; // Shipper đã xác nhận đơn hàng
    }

    public static class ValueDiscountDAL
    {
        public const int Discount = 1;
        public const int NotDiscount = 2;
    }
    public static class ValuePriceDAL
    {
        public const int Ascending = 1;
        public const int Descending = 2;
    }
    public static class ValueRole
    {
        public const string Admin = "Admin";
        public const string Staff = "Staff";
        //public const string Customer = "Customer";
    }
}
