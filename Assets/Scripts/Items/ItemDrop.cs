using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ItemDrop : MonoBehaviour, IPoolable
{
    public event Action OnDestroyPoolObject;

    public Item Item;
    public int DropID;
    public Dimension Dimension;

    public void Initialize(int _dropID, Item _item, Dimension _dim)
    {
        DropID = _dropID;
        Item = _item;
        Dimension = _dim;
    }

    public void PickUp(Player _byPlayer)
    {
        _byPlayer.Inventory.AddItem(Item);
        gameObject.SetActive(false);
        ItemSpawner.OnPickedUpItem?.Invoke(DropID);
        ServerSend.ItemPickedUp(_byPlayer.ID, DropID);
    }

    private void OnDisable() { OnDestroyPoolObject(); }
}
