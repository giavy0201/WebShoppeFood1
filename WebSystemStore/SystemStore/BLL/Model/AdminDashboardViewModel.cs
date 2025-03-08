using BLL.Model.Cart;
using BLL.Model.Comment;
using BLL.Model.Customer;
using BLL.Model.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace BLL.Model
{
    public class AdminDashboardViewModel
    {
        //public List<StoreDtos> Stores { get; set; }
        //public List<CustomerDtos> Customers { get; set; }
        public IPagedList<StoreDtos> Stores { get; set; }
        public IPagedList<CustomerDtos> Customers { get; set; }
        public IPagedList<CartDtos> Carts { get; set; }
        public IPagedList<CommentDtos> Comments { get; set; }
    }
}
