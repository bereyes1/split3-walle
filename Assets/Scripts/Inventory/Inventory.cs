using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 30;

    private readonly List<ItemData> items = new();
    private readonly Dictionary<ItemData, int> itemsDict = new();

    public event Action<ItemData> OnItemAdded;
    public event Action<ItemData> OnItemRemoved;
    public event Action OnInventoryFull;

    public bool IsFull => items.Count >= maxCapacity;
    public int InventoryCount => items.Count;
    public List<ItemData> Items => items;
    public Dictionary<ItemData, int> ItemsDict => itemsDict;

    public bool Add(ItemData item)
    {
        if (IsFull)
        {
            OnInventoryFull?.Invoke();
            return false;
        }

        // check if key exists in dictionary yet
        if (itemsDict.ContainsKey(item)) itemsDict[item]++;
        else itemsDict[item] = 1;

        items.Add(item);
        OnItemAdded?.Invoke(item);
        return true;
    }

    public bool Remove(ItemData item)
    {
        if (!items.Remove(item)) return false;

        OnItemRemoved?.Invoke(item);
        itemsDict[item]--;
        return true;
    }
}