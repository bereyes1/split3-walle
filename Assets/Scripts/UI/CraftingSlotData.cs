using UnityEngine;

[CreateAssetMenu(
    fileName = "CraftingSlotData",
    menuName = "ScriptableObjects/CraftingUISlots/CraftingSlotData")
]
public class CraftingSlotData : ScriptableObject
{
    public ItemData trashData;
    public BlockData blockData;
}