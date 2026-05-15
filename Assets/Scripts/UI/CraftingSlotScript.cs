using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotScript : MonoBehaviour
{
    [SerializeField] private CraftingSlotData craftingSlotData;
    public CraftingSlotData Data => craftingSlotData;

    [Header("[== TEXT REFERENCES ==]")]
    [SerializeField] private TMP_Text materialText;
    [SerializeField] private TMP_Text blockText;
    [SerializeField] private TMP_Text requiredAmountText;
    public TMP_Text MaterialText => materialText;
    public TMP_Text BlockText => blockText;
    public TMP_Text RequiredAmountText => requiredAmountText;

    [Header("[== IMAGE REFERENCES ==]")]
    [SerializeField] private Image craftableImage;
    [SerializeField] private Image uncraftableImage;
    [SerializeField] private Image materialImage;
    [SerializeField] private Image blockImage;
    public Image CraftableImage => craftableImage;
    public Image UncraftableImage => uncraftableImage;
    public Image MaterialImage => materialImage;
    public Image BlockImage => blockImage;

    [Header("[== BUTTON REFERENCES ==]")]
    [SerializeField] private Button craftButton;
    public Button CraftButton => craftButton;

    void Start()
    {
        materialImage.sprite = craftingSlotData.trashData.icon;
        blockImage.sprite = craftingSlotData.blockData.icon;
    }
}