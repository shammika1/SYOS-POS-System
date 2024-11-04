using SYOSSytem.DataGateway;
using SYOSSytem.DTO;

namespace SYOSSytem.Command;

public class AddStockCommand : ICommand
{
    private readonly StockDTO stock;
    private readonly StockGateway stockGateway;

    public AddStockCommand(StockDTO stock, StockGateway stockGateway)
    {
        this.stock = stock;
        this.stockGateway = stockGateway;
    }

    public void Execute()
    {
        stockGateway.AddStock(stock);
    }
}