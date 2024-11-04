namespace SYOSSytem.Composite;

public class CategoryComposite : IItemComponent
{
    private readonly List<IItemComponent> _components = new();

    public CategoryComposite(string categoryName)
    {
        CategoryName = categoryName;
    }

    public string CategoryName { get; }

    public void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + CategoryName);
        foreach (var component in _components) component.Display(depth + 2);
    }

    public void Add(IItemComponent component)
    {
        _components.Add(component);
    }

    public void Remove(IItemComponent component)
    {
        _components.Remove(component);
    }

    public List<IItemComponent> GetComponents()
    {
        return _components;
    }
}