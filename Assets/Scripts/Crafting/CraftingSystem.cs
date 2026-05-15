using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    // [SerializeField] private InputReader inputReader;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private InventoryNewUI invUI;
    [SerializeField] private int trashPerBlock = 5;
    
    public int TrashPerBlock => trashPerBlock;

    public event Action OnCraftSuccess;
    public event Action OnCraftFailed;

    private readonly Dictionary<BlockData, int> TrashBlockCount = new();

    // private void OnEnable()
    // {
    //     inputReader.CraftEvent += Craft;
    // }

    // private void OnDisable()
    // {
    //     inputReader.CraftEvent -= Craft;
    // }

    public bool UseBlock()
    {
        InventorySlotScript slot = invUI.InventorySlots[invUI.InventorySlotCurrent];
        BlockData item = slot.Data.blockData;

        if (dictCount(item) <= 0)
            return false;
        
        TrashBlockCount[item]--;
        return true;
    }

    public bool CanCraft(ItemData item)
    {
        return playerInventory.dictCount(item) >= trashPerBlock;
    }

    public void Craft(CraftingSlotData slot)
    {
        ItemData trashData = slot.trashData;
        if (!CanCraft(trashData))
        {
            OnCraftFailed?.Invoke();
            Debug.Log("Not enough trash to craft a block.");
            return;
        }

        for (int i = 0; i < trashPerBlock; i++)
            playerInventory.Remove(slot.trashData);

        // check if key exists in dictionary yet
        BlockData blockData = slot.blockData;
        if (TrashBlockCount.ContainsKey(blockData)) TrashBlockCount[blockData]++;
        else TrashBlockCount[blockData] = 1;

        OnCraftSuccess?.Invoke();
    }

    public int dictCount(BlockData item)
    {
        return TrashBlockCount.ContainsKey(item)
            ? TrashBlockCount[item]
            : 0;
    }

    public BlockData currentBlock()
    {
        return invUI
            .InventorySlots[invUI.InventorySlotCurrent]
            .Data
            .blockData;
    }
}