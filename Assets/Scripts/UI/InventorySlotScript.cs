using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    [SerializeField] private CraftingSlotData craftingSlotData;
    public CraftingSlotData Data => craftingSlotData;

    [Header("[== TEXT REFERENCES ==]")]
    [SerializeField] private TMP_Text materialCount;
    [SerializeField] private TMP_Text materialName;
    [SerializeField] private TMP_Text blockCount;
    public TMP_Text MaterialCount => materialCount;
    public TMP_Text MaterialName => materialName;
    public TMP_Text BlockCount => blockCount;

    [Header("[== IMAGE REFERENCES ==]")]
    [SerializeField] private Image materialImage;
    [SerializeField] private Image blockImage;
    public Image MaterialImage => materialImage;
    public Image BlockImage => blockImage;

    void Start()
    {
        materialImage.sprite = craftingSlotData.trashData.icon;
        blockImage.sprite = craftingSlotData.blockData.icon;
    }
}
