using SYOSSytem.DataGateway;
using SYOSSytem.DTO;

namespace SYOSSytem.Command;

public class AddShelfCommand : ICommand
{
    private readonly ShelfDTO shelf;
    private readonly ShelfGateway shelfGateway;

    public AddShelfCommand(ShelfDTO shelf, ShelfGateway shelfGateway)
    {
        this.shelf = shelf;
        this.shelfGateway = shelfGateway;
    }

    public void Execute()
    {
        shelfGateway.AddShelf(shelf);
    }
}