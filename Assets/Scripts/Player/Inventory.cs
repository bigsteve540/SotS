using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public int Slots = 16;

    private Player player;
    private Item[] items;

    public Inventory(Player _player) { items = new Item[Slots]; player = _player; }

    public bool AddItem(Item _item)
    {
        if(SearchForItem(null, out int index)) //looking for an empty slot
        {
            items[index] = _item;
            ServerSend.ItemAddedToInventory(player.ID, index, _item);
            return true;
        }
        return false;
    }
    public bool RemoveItem(Item _item)
    {
        if(SearchForItem(_item, out int index)) //looking for item to remove
        {
            items[index] = null;
            ServerSend.ItemAddedToInventory(player.ID, index, null);
            return true;
        }
        return false;
    }

    public bool SearchForItem(Item _item, out int index)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == _item)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }
}
