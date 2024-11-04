using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYOS.Shared.DTO
{
    public class BillItemDTO
    {
        public int BillItemID { get; set; }
        public string? BillID { get; set; }  //nullable
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
