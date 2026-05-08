using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 30;

    private readonly List<ItemData> items = new();

    public event Action<ItemData> OnItemAdded;
    public event Action<ItemData> OnItemRemoved;
    public event Action OnInventoryFull;

    public bool IsFull => items.Count >= maxCapacity;
    public int InventoryCount => items.Count;
    public List<ItemData> Items => items;

    public bool Add(ItemData item)
    {
        if (IsFull)
        {
            OnInventoryFull?.Invoke();
            return false;
        }

        items.Add(item);
        OnItemAdded?.Invoke(item);
        return true;
    }

    public bool Remove(ItemData item)
    {
        if (!items.Remove(item)) return false;

        OnItemRemoved?.Invoke(item);
        return true;
    }
}
