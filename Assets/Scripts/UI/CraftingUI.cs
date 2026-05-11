using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    private bool isOpen;
    private CursorManager cursorManager;

    [Header("[== SYSTEM REFERENCES ==]")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private InputReader inputReader;

    [Header("[== UI REFERENCES==]")]
    [SerializeField] private Button closeButton;
    [SerializeField] private CanvasGroup panelGroup;

    [Header("[== TEXT FIELDS ==]")]
    [SerializeField] private TMP_Text panelText;


    void Start()
    {
        if (CursorManager.Instance != null)
        {
            cursorManager = CursorManager.Instance;
        }
        
        isOpen = true;
        OpenCraftMenu();
        
        // add listener and lock cursor
        closeButton.onClick.AddListener(() => OpenCraftMenu());
        cursorManager.LockCursor();
    }

    private void OnEnable()
    {
        inputReader.CraftMenuEvent += OpenCraftMenu;
        inputReader.CraftMenuEvent += UpdatePanelText;
        craftingSystem.OnCraftSuccess += UpdatePanelText;
        inventory.OnItemAdded += HandleInventoryChanged;
        inventory.OnItemRemoved += HandleInventoryChanged;
    }

    private void OnDisable()
    {
        inputReader.CraftMenuEvent -= OpenCraftMenu;
        inputReader.CraftMenuEvent -= UpdatePanelText;
        craftingSystem.OnCraftSuccess -= UpdatePanelText;
        inventory.OnItemAdded -= HandleInventoryChanged;
        inventory.OnItemRemoved -= HandleInventoryChanged;
    }

    private void OpenCraftMenu()
    {
        isOpen = !isOpen;
        panelGroup.alpha = isOpen ? 1f : 0f;
        panelGroup.interactable = isOpen;
        panelGroup.blocksRaycasts = isOpen;

        // temporarily lock/unlock cursor for ui
        if (cursorManager.IsLocked)
            cursorManager.UnlockCursor();
        else
            cursorManager.LockCursor();
    }

    private void HandleInventoryChanged(ItemData item) => UpdatePanelText();

    private void UpdatePanelText()
    {
        int count = inventory.InventoryCount;
        panelText.text = 
            $"[{count}] Inventory => [{count / craftingSystem.TrashPerBlock}] Craftable Blocks";
    }
}