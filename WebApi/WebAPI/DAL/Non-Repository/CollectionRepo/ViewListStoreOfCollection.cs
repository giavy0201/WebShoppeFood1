﻿namespace DAL.Non_Repository.CollectionRepo
{
    public class ViewListStoreOfCollection
    {
        public int Id { get; set; }
        public int CollectionID { get; set; }
        public int StoreID { get; set; }
        public string? StoreImg { get; set; }
        public string? StoreName { get; set; }
        public string? StoreAddress { get; set; }
        public string? StroreLocation { get; set; }
        public string? StorePreferential { get; set; }
    }
}
