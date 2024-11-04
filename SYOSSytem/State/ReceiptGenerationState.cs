using SYOSSytem.Factory;

namespace SYOSSytem.State;

public class ReceiptGenerationState : IBillingState
{
    public void Handle(BillingContext context)
    {
        var processSaleCommand = CommandFactory.CreateProcessSaleCommand(context);
        processSaleCommand.Execute();
    }
}