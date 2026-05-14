using UnityEngine;

[CreateAssetMenu(
    fileName = "CraftingSlotData",
    menuName = "ScriptableObjects/CraftingUISlots/CraftingSlotData")
]
public class CraftingSlotData : ScriptableObject
{
    [Header("[== TRASH DATA ==]")]
    public string trashName;
    public Sprite trashIcon;
    public ItemData trashData;

    [Header("[== BLOCK DATA ==]")]
    public string blockName;
    public Sprite blockIcon;
}