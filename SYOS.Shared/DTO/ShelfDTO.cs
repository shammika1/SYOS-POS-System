using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYOS.Shared.DTO
{
    public class ShelfDTO
    {
        public int ShelfID { get; set; }
        public string ShelfLocation { get; set; }
        public int ShelfQuantity { get; set; }
        public string ItemCode { get; set; }
        public int Version { get; set; }
    }
}
