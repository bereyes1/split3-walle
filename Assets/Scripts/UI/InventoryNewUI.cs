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
    [SerializeField] private List<InventorySlotScript> inventorySlots;
    private int inventorySlotCurrent = 0;
    
    public List<InventorySlotScript> InventorySlots => inventorySlots;
    public int InventorySlotCurrent => inventorySlotCurrent;

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
        UpdateUI();
    }

    private void HandleHotbarSwitch(float value)
    {
        inventorySlotCurrent = (int)value - 1;
        float selectSize = 160f;
        Vector2 hotbarSelectPos = hotbarSelectImage.rectTransform.anchoredPosition;
        hotbarSelectImage.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(
                selectSize * (value - 2),
                hotbarSelectPos.y
            );
    }

    private void HandleInventoryChanged(ItemData item) => UpdateUI();

    private void UpdateUI()
    {
        foreach(InventorySlotScript slot in inventorySlots)
        {
            CraftingSlotData data = slot.Data;
            int count = inventory.dictCount(data.trashData);
            int craftables = craftingSystem.dictCount(data.blockData);

            slot.MaterialText.text = $"{count}";
            slot.BlockText.text = $"{craftables}";
        }
    }
}