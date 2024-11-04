using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYOS.Shared.DTO;

namespace SYOS.Shared.Interfaces
{
    public interface IBillService
    {
        Task<BillDTO> CreateBillAsync(BillDTO bill);
        Task<BillDTO> GetBillAsync(string billId);
        Task<List<BillDTO>> GetAllBillsAsync();
        Task<BillDTO> ProcessSaleAsync(List<BillItemDTO> billItems, decimal discount, decimal cashTendered);
    }
}
