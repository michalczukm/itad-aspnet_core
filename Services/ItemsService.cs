using System.Collections.Generic;
using System.Linq;

public interface IItemsService 
{
    IEnumerable<Item> Get();
    Item GetById(int id);
    Item Create(Item item);
    bool DeleteById(int id);
    bool UpdateFor(int id, Item item);
}

public class ItemsService : IItemsService
{
    private static List<Item> ItemsList = new List<Item>
    { 
        new Item { Id = 1, Content = "First item" }, 
        new Item { Id = 2, Content = "Another item" }, 
        new Item { Id = 3, Content = "One more item" }, 
    };
    
    public IEnumerable<Item> Get()
    {
        return ItemsList;
    }

    public Item GetById(int id)
    {
        return ItemsList.Find(i => i.Id == id); 
    }

    public Item Create(Item item)
    {
        item.Id = ItemsList.Select(i => i.Id).DefaultIfEmpty(0).Max() + 1;
        ItemsList.Add(item);

        return item;
    }

    public bool DeleteById(int id)
    {
        var item = ItemsList.Single(i => i.Id == id);
        return ItemsList.Remove(item);
    }

    public bool UpdateFor(int id, Item item)
    {
        var existing = ItemsList.SingleOrDefault(i => i.Id == id);
        if (existing != null) 
        {
            item.Id = existing.Id;
            var index = ItemsList.IndexOf(existing);
            ItemsList[index]=item;
            return true;
        }
        else 
        {
            return false;
        }
    }
}