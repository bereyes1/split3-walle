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
        if (CursorManager.Instance != null) cursorManager = CursorManager.Instance;
        cursorManager.LockCursor();

        CreateButtonListeners();
        InitCanvasWindow();
    }

    private void CreateButtonListeners()
    {
        trashSlot.CraftButton.onClick.AddListener(craftingSystem.Craft);
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
        UpdatePanelImage();
        UpdatePanelText();
    }

    private void UpdatePanelImage()
    {
        bool canCraft = craftingSystem.CanCraft();
        Color white = new Color(1f, 1f, 1f, 1f);
        Color gray = new Color(0.5f, 0.5f, 0.5f, 1f);

        trashSlot.CraftableImage.enabled = canCraft;
        trashSlot.UncraftableImage.enabled = !canCraft;

        trashSlot.MaterialImage.color = inventory.InventoryCount > 0
            ? white
            : gray;
        trashSlot.BlockImage.color = canCraft
            ? white
            : gray;
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