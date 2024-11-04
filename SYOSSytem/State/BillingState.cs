namespace SYOSSytem.State;

public abstract class BillingState
{
    protected BillingContext context;

    public void SetContext(BillingContext context)
    {
        this.context = context;
    }

    public abstract void Handle();
}