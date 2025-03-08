using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Model.ModelRequest
{
    public class SetStatusRequest
    {
        public int CartID { get; set; }
        public int Status { get; set; }
    }
}
