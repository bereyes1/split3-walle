using TMPro;
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

    [Header("[== UI REFERENCES==]")]
    [SerializeField] private Button closeButton;
    [SerializeField] private CanvasGroup panelGroup;

    [Header("[== CRAFTING SLOT FIELDS ==]")]
    [SerializeField] private CraftingSlotScript trashSlot;

    void Start()
    {
        if (CursorManager.Instance != null)
        {
            cursorManager = CursorManager.Instance;
        }

        // add event listener for crafting button
        trashSlot.CraftButton.onClick.AddListener(craftingSystem.Craft);
        
        // add listener and lock cursor
        closeButton.onClick.AddListener(OpenCraftMenu);
        cursorManager.LockCursor();
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

    private void HandleInventoryChanged(ItemData item) => UpdatePanelText();

    private void UpdatePanel()
    {
        bool canCraft = craftingSystem.CanCraft();
        trashSlot.CraftableImage.enabled = canCraft;
        trashSlot.UncraftableImage.enabled = !canCraft;
        UpdatePanelText();
    }

    private void UpdatePanelText()
    {
        int count = inventory.InventoryCount;
        trashSlot.MaterialText.text =
            $"{count}";
        trashSlot.RequiredAmountText.text =
            $"{craftingSystem.TrashPerBlock} NEEDED";
        trashSlot.BlockText.text =
            $"{count / craftingSystem.TrashPerBlock}";
    }
}