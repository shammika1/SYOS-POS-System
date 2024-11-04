using SYOSSytem.Command;
using SYOSSytem.State;

namespace SYOSSytem.Factory;

public static class CommandFactory
{
    public static ICommand CreateProcessSaleCommand(BillingContext context)
    {
        return new ProcessSaleCommand(context);
    }
}