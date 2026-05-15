using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 30;
    private readonly Dictionary<ItemData, int> itemsDict = new();
    private int trashCollected = 0;

    public event Action<ItemData> OnItemAdded;
    public event Action<ItemData> OnItemRemoved;
    public event Action OnInventoryFull;

    public bool IsFull => InventoryCount >= maxCapacity;
    public int InventoryCount => itemsDict.Values.Sum();
    public int TrashCollected => trashCollected;
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

        trashCollected++;
        OnItemAdded?.Invoke(item);
        return true;
    }

    public bool Remove(ItemData item)
    {
        if (itemsDict[item] <= 0 || !itemsDict.ContainsKey(item))
        {
            itemsDict[item] = 0;
            return false;
        }

        OnItemRemoved?.Invoke(item);
        itemsDict[item]--;
        return true;
    }

    public int dictCount(ItemData item)
    {
        return itemsDict.ContainsKey(item)
            ? itemsDict[item]
            : 0;
    }
}