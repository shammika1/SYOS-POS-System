using SYOSSytem.DataGateway;

namespace SYOSSytem.Command;

public class DeleteShelfCommand : ICommand
{
    private readonly ShelfGateway shelfGateway;
    private readonly int shelfID;

    public DeleteShelfCommand(int shelfID, ShelfGateway shelfGateway)
    {
        this.shelfID = shelfID;
        this.shelfGateway = shelfGateway;
    }

    public void Execute()
    {
        shelfGateway.DeleteShelf(shelfID);
    }
}