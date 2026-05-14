using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    private bool isOpen = false;
    private CursorManager cursorManager;

    [Header("[== SYSTEM REFERENCES ==]")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private InputReader inputReader;

    [Header("[== UI REFERENCES ==]")]
    [SerializeField] private Button closeButton;
    [SerializeField] private CanvasGroup panelGroup;

    [Header("[== CRAFTING SLOT REFERENCES ==]")]
    [SerializeField] private List<CraftingSlotScript> trashSlots;

    void Start()
    {
        if (CursorManager.Instance != null) cursorManager = CursorManager.Instance;
        cursorManager.LockCursor();

        CreateButtonListeners();
        InitCanvasWindow();
    }

    private void CreateButtonListeners()
    {
        foreach(CraftingSlotScript slot in trashSlots)
            slot.CraftButton.onClick.AddListener(craftingSystem.Craft);
        
        closeButton.onClick.AddListener(OpenCraftMenu);
    }

    private void InitCanvasWindow()
    {
        panelGroup.alpha = 0f;
        panelGroup.interactable = false;
        panelGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        inputReader.CraftMenuEvent += OpenCraftMenu;
        inputReader.CraftMenuEvent += UpdatePanel;
        craftingSystem.OnCraftSuccess += UpdatePanel;
        inventory.OnItemAdded += HandleInventoryChanged;
        inventory.OnItemRemoved += HandleInventoryChanged;
    }

    private void OnDisable()
    {
        inputReader.CraftMenuEvent -= OpenCraftMenu;
        inputReader.CraftMenuEvent -= UpdatePanel;
        craftingSystem.OnCraftSuccess -= UpdatePanel;
        inventory.OnItemAdded -= HandleInventoryChanged;
        inventory.OnItemRemoved -= HandleInventoryChanged;
    }

    private void OpenCraftMenu()
    {
        isOpen = !isOpen;
        panelGroup.alpha = isOpen ? 1f : 0f;
        panelGroup.interactable = isOpen;
        panelGroup.blocksRaycasts = isOpen;
        cursorManager.ToggleCursorLock();
    }

    private void HandleInventoryChanged(ItemData item) => UpdatePanel();

    private void UpdatePanel()
    {
        UpdatePanelColors();
        UpdatePanelText();
    }

    private void UpdatePanelColors()
    {
        Color white = new Color(1f, 1f, 1f, 1f);
        Color gray = new Color(0.5f, 0.5f, 0.5f, 1f);
        bool canCraft = craftingSystem.CanCraft();

        foreach(CraftingSlotScript slot in trashSlots)
        {
            slot.CraftableImage.enabled = canCraft;
            slot.UncraftableImage.enabled = !canCraft;
            slot.CraftButton.interactable = canCraft; // changes color of button, enabled doesn't

            slot.MaterialImage.color = inventory.InventoryCount > 0
                ? white
                : gray;
            slot.BlockImage.color = canCraft
                ? white
                : gray;
        }
    }

    private void UpdatePanelText()
    {
        int count = inventory.InventoryCount;

        foreach(CraftingSlotScript slot in trashSlots)
        {
            slot.MaterialText.text =
                $"{count}";
            slot.RequiredAmountText.text =
                $"COST: {craftingSystem.TrashPerBlock}";
            slot.BlockText.text =
                $"{count / craftingSystem.TrashPerBlock}";
        }
    }
}