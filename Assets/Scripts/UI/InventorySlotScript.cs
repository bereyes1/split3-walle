using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    [SerializeField] private CraftingSlotData craftingSlotData;
    public CraftingSlotData Data => craftingSlotData;

    [Header("[== TEXT REFERENCES ==]")]
    [SerializeField] private TMP_Text materialText;
    [SerializeField] private TMP_Text blockText;
    public TMP_Text MaterialText => materialText;
    public TMP_Text BlockText => blockText;

    [Header("[== IMAGE REFERENCES ==]")]
    [SerializeField] private Image materialImage;
    [SerializeField] private Image blockImage;
    public Image MaterialImage => materialImage;
    public Image BlockImage => blockImage;

    void Start()
    {
        materialImage.sprite = craftingSlotData.trashIcon;
        blockImage.sprite = craftingSlotData.blockIcon;
    }
}
