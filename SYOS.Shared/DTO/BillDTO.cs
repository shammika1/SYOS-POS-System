using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYOS.Shared.DTO
{
    public class BillDTO
    {
        public string BillID { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal CashTendered { get; set; }
        public decimal ChangeAmount { get; set; }
        public List<BillItemDTO> BillItems { get; set; } = new List<BillItemDTO>();
        public int Version { get; set; } // For optimistic concurrency
    }
}
