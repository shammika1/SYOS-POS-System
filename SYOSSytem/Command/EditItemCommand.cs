using SYOSSytem.DataGateway;
using SYOSSytem.DTO;

namespace SYOSSytem.Command;

public class EditItemCommand : ICommand
{
    private readonly ItemDTO item;
    private readonly ItemGateway itemGateway;

    public EditItemCommand(ItemDTO item, ItemGateway itemGateway)
    {
        this.item = item;
        this.itemGateway = itemGateway;
    }

    public void Execute()
    {
        itemGateway.EditItem(item);
    }
}