using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridPlacementUI : MonoBehaviour
{
    [Header("[== SYSTEM REFERENCES ==]")]
    [SerializeField] private GridPlacementSystem placementSystem;
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private InventoryNewUI inventoryNewUI;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CursorManager cursorManager;

    [Header("[== GRID BUTTONS ==]")]
    [SerializeField] private List<Button> gridButtons;

    [Header("[== UI REFERENCES ==]")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button orientationToggleButton;
    [SerializeField] private TMP_Text orientationLabel;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text affordabilityText;
    [SerializeField] private TMP_Text paymentModeLabel;
    [SerializeField] private Button paymentModeButton;
    [SerializeField] private CanvasGroup panelGroup;

    [Header("[== CELL COLORS ==]")]
    [SerializeField] private Color cellOffColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    [SerializeField] private Color cellOnColor = new Color(0.3f, 0.8f, 0.3f, 1f);
    [SerializeField] private Color cellOnVertColor = new Color(0.3f, 0.5f, 0.9f, 1f);
    [SerializeField] private Color affordableColor = new Color(0.2f, 0.9f, 0.2f, 1f);
    [SerializeField] private Color tooExpensive = new Color(0.9f, 0.2f, 0.2f, 1f);

    private bool[,] grid = new bool[3, 3];
    private bool isVertical = false;
    private bool useRawTrash = false;
    private bool isOpen = false;

    private List<Image> cellImages = new List<Image>();

    private void OnEnable()
    {
        inputReader.GridPlacementMenuEvent += ToggleMenu;
    }

    private void OnDisable()
    {
        inputReader.GridPlacementMenuEvent -= ToggleMenu;
    }

    private void Start()
    {
        for (int i = 0; i < gridButtons.Count && i < 9; i++)
        {
            int capturedCol = i % 3;
            int capturedRow = i / 3;
            gridButtons[i].onClick.AddListener(() => OnCellClicked(capturedCol, capturedRow));

            Image img = gridButtons[i].GetComponentInChildren<Image>();
            cellImages.Add(img);
        }

        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
        orientationToggleButton.onClick.AddListener(ToggleOrientation);
        paymentModeButton.onClick.AddListener(TogglePaymentMode);

        ClosePanel();
    }

    private void ToggleMenu()
    {
        if (isOpen) OnCancel();
        else OpenPanel();
    }

    private void OpenPanel()
    {
        isOpen = true;
        panelGroup.alpha = 1f;
        panelGroup.interactable = true;
        panelGroup.blocksRaycasts = true;
        cursorManager.ToggleCursorLock();
        inputReader.DisableAll();
        RefreshUI();
    }

    private void ClosePanel()
    {
        isOpen = false;
        panelGroup.alpha = 0f;
        panelGroup.interactable = false;
        panelGroup.blocksRaycasts = false;
    }

    private void OnCellClicked(int col, int row)
    {
        grid[col, row] = !grid[col, row];
        RefreshUI();
    }

    private void ToggleOrientation()
    {
        isVertical = !isVertical;
        orientationLabel.text = isVertical ? "Vertical" : "Horizontal";
        RefreshUI();
    }

    private void TogglePaymentMode()
    {
        useRawTrash = !useRawTrash;
        paymentModeLabel.text = useRawTrash ? "Pay: Raw Trash" : "Pay: Blocks";
        RefreshUI();
    }

    private void OnConfirm()
    {
        List<Vector2Int> cells = GetPaintedCells();
        if (cells.Count == 0) return;

        if (!CanAfford(cells.Count))
        {
            affordabilityText.text = "Not enough materials!";
            affordabilityText.color = tooExpensive;
            return;
        }

        placementSystem.TryPlaceShape(cells, useRawTrash, isVertical);

        ClearGrid();
        OnCancel();
    }

    private void OnCancel()
    {
        ClearGrid();
        ClosePanel();
        cursorManager.ToggleCursorLock();
        inputReader.SetPlayer();
    }

    private List<Vector2Int> GetPaintedCells()
    {
        var cells = new List<Vector2Int>();
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (!grid[col, row]) continue;
                cells.Add(new Vector2Int(col - 1, row - 1));
            }
        }
        return cells;
    }

    private void ClearGrid()
    {
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                grid[col, row] = false;
    }

    private bool CanAfford(int cellCount)
    {
        if (useRawTrash)
        {
            ItemData trash = GetCurrentTrash();
            if (trash == null) return false;
            return playerInventory.dictCount(trash) >= cellCount * craftingSystem.TrashPerBlock;
        }
        else
        {
            return craftingSystem.CurrentBlockCount >= cellCount;
        }
    }

    private ItemData GetCurrentTrash()
    {
        int idx = inventoryNewUI.InventorySlotCurrent;
        if (idx < 0 || idx >= inventoryNewUI.InventorySlots.Count) return null;
        return inventoryNewUI.InventorySlots[idx].Data.trashData;
    }

    private void RefreshUI()
    {
        BlockData currentBlock = craftingSystem.CurrentBlock;
        Sprite blockIcon = currentBlock != null ? currentBlock.icon : null;

        Color onColor = isVertical ? cellOnVertColor : cellOnColor;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                int i = row * 3 + col;
                if (i >= gridButtons.Count) continue;

                bool isPainted = grid[col, row];

                var colors = gridButtons[i].colors;
                colors.normalColor = isPainted ? onColor : cellOffColor;
                colors.selectedColor = colors.normalColor;
                colors.highlightedColor = isPainted ? onColor : new Color(0.35f, 0.35f, 0.35f);
                gridButtons[i].colors = colors;

                if (i < cellImages.Count && cellImages[i] != null)
                {
                    cellImages[i].sprite = blockIcon;
                    cellImages[i].color = isPainted ? Color.white : new Color(1f, 1f, 1f, 0.3f);
                }
            }
        }

        int paintedCount = GetPaintedCells().Count;
        if (useRawTrash)
        {
            int trashCost = paintedCount * craftingSystem.TrashPerBlock;
            ItemData trash = GetCurrentTrash();
            int have = trash != null ? playerInventory.dictCount(trash) : 0;
            costText.text = $"Cost: {trashCost} trash  (have {have})";
        }
        else
        {
            int have = craftingSystem.CurrentBlockCount;
            costText.text = $"Cost: {paintedCount} blocks  (have {have})";
        }

        bool affordable = paintedCount == 0 || CanAfford(paintedCount);
        affordabilityText.text = affordable ? (paintedCount > 0 ? "Ready!" : "Paint some cells") : "Not enough materials!";
        affordabilityText.color = affordable ? affordableColor : tooExpensive;

        confirmButton.interactable = paintedCount > 0 && affordable;
    }
}