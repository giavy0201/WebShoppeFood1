﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ModelsRequest
{
    public class UpdateCommentReq
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int? FoodId { get; set; }
        public double StarRating { get; set; }
        public string UserId { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int StoreId { get; set; }
    }
}
