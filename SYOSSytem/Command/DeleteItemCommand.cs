using SYOSSytem.DataGateway;

namespace SYOSSytem.Command;

public class DeleteItemCommand : ICommand
{
    private readonly string itemCode;
    private readonly ItemGateway itemGateway;

    public DeleteItemCommand(string itemCode, ItemGateway itemGateway)
    {
        this.itemCode = itemCode;
        this.itemGateway = itemGateway;
    }

    public void Execute()
    {
        itemGateway.DeleteItem(itemCode);
    }
}