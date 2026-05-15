using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private PlacementSystem placementSystem;

    [Header("Text Fields")]
    [SerializeField] private TMP_Text inventoryCountText;
    [SerializeField] private TMP_Text craftCostText;

    private void OnEnable()
    {
        inventory.OnItemAdded += HandleInventoryChanged;
        inventory.OnItemRemoved += HandleInventoryChanged;
        craftingSystem.OnCraftSuccess += UpdateUI;
        placementSystem.OnBlockPlaced += UpdateUI;
    }

    private void OnDisable()
    {
        inventory.OnItemAdded -= HandleInventoryChanged;
        inventory.OnItemRemoved -= HandleInventoryChanged;
        craftingSystem.OnCraftSuccess -= UpdateUI;
        placementSystem.OnBlockPlaced -= UpdateUI;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void HandleInventoryChanged(ItemData item) => UpdateUI();

    private void UpdateUI()
    {
        int count = inventory.InventoryCount;
        int cost = craftingSystem.TrashPerBlock;

        inventoryCountText.text = "Inventory: " + count;
        craftCostText.text = "Cost: " + cost;
    }
}
