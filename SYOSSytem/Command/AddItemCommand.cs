using SYOSSytem.DataGateway;
using SYOSSytem.DTO;

namespace SYOSSytem.Command;

public class AddItemCommand : ICommand
{
    private readonly ItemDTO item;
    private readonly ItemGateway itemGateway;

    public AddItemCommand(ItemDTO item, ItemGateway itemGateway)
    {
        this.item = item;
        this.itemGateway = itemGateway;
    }

    public void Execute()
    {
        itemGateway.AddItem(item);
    }
}