using UnityEngine;

[CreateAssetMenu(
    fileName = "CraftingSlotData",
    menuName = "ScriptableObjects/BlockTypes/TrashBlockData")
]
public class BlockData : ScriptableObject
{
    public string blockName;
    public Sprite icon;
    public GameObject prefab;
}