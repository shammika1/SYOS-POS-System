using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYOS.Shared.DTO;

namespace SYOS.Shared.Interfaces
{
    public interface IStockService
    {
        Task<List<StockDTO>> GetAllStocksAsync();
        Task<List<StockDTO>> GetStocksByItemCodeAsync(string itemCode);
        Task<StockDTO> GetStockByIdAsync(int stockId);
        Task AddStockAsync(StockDTO stock);
        Task UpdateStockAsync(StockDTO stock);
        Task DeleteStockAsync(int stockId);
    }
}
