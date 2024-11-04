namespace SYOSSytem.Composite;

public class ItemLeaf : IItemComponent
{
    public ItemLeaf(string itemCode, string name)
    {
        ItemCode = itemCode;
        Name = name;
    }

    public string ItemCode { get; }
    public string Name { get; }

    public void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + $"{Name} (Code: {ItemCode}");
    }
}