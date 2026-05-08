using System;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private int trashPerBlock = 5;
    public int TrashPerBlock => trashPerBlock;

    public event Action OnCraftSuccess;
    public event Action OnCraftFailed;

    public int TrashBlockCount { get; private set; }

    private void OnEnable()
    {
        inputReader.CraftEvent += Craft;
    }

    private void OnDisable()
    {
        inputReader.CraftEvent -= Craft;
    }

    public bool UseBlock()
    {
        if (TrashBlockCount <= 0) return false;
        TrashBlockCount--;
        return true;
    }

    public bool CanCraft()
    {
        return playerInventory.InventoryCount >= trashPerBlock;
    }

    public void Craft()
    {
        if (!CanCraft())
        {
            OnCraftFailed?.Invoke();
            Debug.Log("Not enough trash to craft a block.");
            return;
        }

        for (int i = 0; i < trashPerBlock; i++)
        {
            playerInventory.Remove(playerInventory.Items[0]);
        }

        TrashBlockCount++;
        OnCraftSuccess?.Invoke();
    }
}
