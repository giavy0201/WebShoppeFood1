using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Model.Collection
{
    public class CollectionDtos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int CategoryID { get; set; }
    }
}
