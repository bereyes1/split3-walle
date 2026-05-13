using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryNewUI : MonoBehaviour
{
    [Header("[== SYSTEM REFERENCES ==]]")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private InputReader inputReader;

    [Header("[== IMAGE REFERENCES ==]")]
    [SerializeField] private Image hotbarSelectImage;

    [Header("[== CRAFTING SLOT REFERENCES ==]")]
    [SerializeField] private InventorySlotScript inventorySlot1;
    private List<InventorySlotScript> inventorySlots = new List<InventorySlotScript>();
    private int inventorySlotCurrent = 1;

    private void OnEnable()
    {
        inputReader.HotbarSwitchEvent += HandleHotbarSwitch;
        inventory.OnItemAdded += HandleInventoryChanged;
        inventory.OnItemRemoved += HandleInventoryChanged;
        craftingSystem.OnCraftSuccess += UpdateUI;
        placementSystem.OnBlockPlaced += UpdateUI;
    }

    private void OnDisable()
    {
        inputReader.HotbarSwitchEvent -= HandleHotbarSwitch;
        inventory.OnItemAdded -= HandleInventoryChanged;
        inventory.OnItemRemoved -= HandleInventoryChanged;
        craftingSystem.OnCraftSuccess -= UpdateUI;
        placementSystem.OnBlockPlaced -= UpdateUI;
    }

    private void Start()
    {
        inventorySlots.Add(inventorySlot1);
        UpdateUI();
    }

    private void HandleHotbarSwitch(float value)
    {
        inventorySlotCurrent = (int)value;
        float selectSize = 160f;
        Vector2 hotbarSelectPos = hotbarSelectImage.rectTransform.anchoredPosition;
        hotbarSelectImage.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(
                selectSize * (value - 2),
                hotbarSelectPos.y
            );
        // TODO: link inventorySlotCurrent to CraftingSystem
    }

    private void HandleInventoryChanged(ItemData item) => UpdateUI();

    private void UpdateUI()
    {
        int count = inventory.InventoryCount;
        int craftables = craftingSystem.TrashBlockCount;

        inventorySlot1.MaterialText.text = $"{count}";
        inventorySlot1.BlockText.text = $"{craftables}";
    }
}
