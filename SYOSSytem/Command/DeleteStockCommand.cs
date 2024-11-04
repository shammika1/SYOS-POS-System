using SYOSSytem.DataGateway;

namespace SYOSSytem.Command;

public class DeleteStockCommand : ICommand
{
    private readonly StockGateway stockGateway;
    private readonly int stockID;

    public DeleteStockCommand(int stockID, StockGateway stockGateway)
    {
        this.stockID = stockID;
        this.stockGateway = stockGateway;
    }

    public void Execute()
    {
        stockGateway.DeleteStock(stockID);
    }
}