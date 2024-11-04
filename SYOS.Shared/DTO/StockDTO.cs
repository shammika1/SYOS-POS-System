using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYOS.Shared.DTO
{
    public class StockDTO
    {
        public int StockID { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int Version { get; set; }
    }
}
