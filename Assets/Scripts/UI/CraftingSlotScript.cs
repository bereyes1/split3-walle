using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotScript : MonoBehaviour
{
    [SerializeField] private CraftingSlotData craftingSlotData;
    public CraftingSlotData Data => craftingSlotData;

    [Header("[== TEXT FIELDS ==]")]
    [SerializeField] private TMP_Text materialText;
    [SerializeField] private TMP_Text blockText;
    [SerializeField] private TMP_Text requiredAmountText;
    public TMP_Text MaterialText => materialText;
    public TMP_Text BlockText => blockText;
    public TMP_Text RequiredAmountText => requiredAmountText;


    [Header("[== IMAGE FIELDS ==]")]
    [SerializeField] private Image craftableImage;
    [SerializeField] private Image uncraftableImage;
    public Image CraftableImage => craftableImage;
    public Image UncraftableImage => uncraftableImage;

    [Header("[== BUTTON FIELDS ==]")]
    [SerializeField] private Button craftButton;
    public Button CraftButton => craftButton;
}