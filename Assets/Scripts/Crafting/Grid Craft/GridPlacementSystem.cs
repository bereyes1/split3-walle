using System;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    [Header("[== SYSTEM REFERENCES ==]")]
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private InventoryNewUI inventoryNewUI;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform player;

    [Header("[== RAYCASTING SETTINGS ==]")]
    [SerializeField] private LayerMask placementLayers;
    [SerializeField] private float placementDistance = 2f;
    [SerializeField] private float raycastOriginHeight = 10f;

    [Header("[== GRID SETTINGS ==]")]
    [SerializeField] private float cellSize = 1f;

    public event Action OnBlockPlaced;

    public void TryPlaceShape(List<Vector2Int> cells, bool useRawTrash, bool isVertical)
    {
        if (cells == null || cells.Count == 0) return;

        BlockData currentBlock = craftingSystem.CurrentBlock;
        if (currentBlock == null)
        {
            Debug.LogWarning("GridPlacementSystem: No block selected.");
            return;
        }

        GameObject prefab = currentBlock.prefab;
        if (prefab == null)
        {
            Debug.LogWarning("GridPlacementSystem: Current block has no prefab.");
            return;
        }
        if (useRawTrash)
        {
            ItemData requiredTrash = GetTrashForBlock(currentBlock);
            if (requiredTrash == null)
            {
                Debug.LogWarning("GridPlacementSystem: Could not find trash type for current block.");
                return;
            }

            int trashNeeded = cells.Count * craftingSystem.TrashPerBlock;
            if (playerInventory.dictCount(requiredTrash) < trashNeeded)
            {
                Debug.Log($"GridPlacementSystem: Not enough trash. Need {trashNeeded}, have {playerInventory.dictCount(requiredTrash)}.");
                return;
            }

            for (int i = 0; i < trashNeeded; i++)
                playerInventory.Remove(requiredTrash);
        }
        else
        {
            if (craftingSystem.CurrentBlockCount < cells.Count)
            {
                Debug.Log($"GridPlacementSystem: Not enough blocks. Need {cells.Count}, have {craftingSystem.CurrentBlockCount}.");
                return;
            }

            for (int i = 0; i < cells.Count; i++)
                craftingSystem.UseBlock();
        }
        Vector3 placementPoint = player.position + player.forward * placementDistance;
        Vector3 rayOrigin = placementPoint + Vector3.up * raycastOriginHeight;

        bool hitGround = Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastOriginHeight + 5f, placementLayers);
        Vector3 anchor = hitGround ? hit.point : placementPoint;
        Vector3 colAxis = new Vector3(player.right.x, 0f, player.right.z).normalized;
        Vector3 rowAxis = isVertical
            ? Vector3.up
            : new Vector3(player.forward.x, 0f, player.forward.z).normalized;

        foreach (Vector2Int cell in cells)
        {
            int rowOffset = isVertical ? cell.y + 1 : cell.y;
            Vector3 worldPos = anchor
                + colAxis * (cell.x * cellSize)
                + rowAxis * (rowOffset * cellSize);

            GameObject obj = Instantiate(prefab, worldPos, player.rotation);
            if (!isVertical)
            {
                Renderer renderer = obj.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    float bottomOffset = obj.transform.position.y - renderer.bounds.min.y;
                    obj.transform.position += Vector3.up * bottomOffset;
                }
            }
        }

        OnBlockPlaced?.Invoke();
    }

    private ItemData GetTrashForBlock(BlockData block)
    {
        foreach (InventorySlotScript slot in inventoryNewUI.InventorySlots)
        {
            if (slot.Data.blockData == block)
                return slot.Data.trashData;
        }
        return null;
    }
}