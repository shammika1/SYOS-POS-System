namespace SYOSSytem.State;

public interface IBillingState
{
    void Handle(BillingContext context);
}