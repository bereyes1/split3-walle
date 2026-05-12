using UnityEngine;

[CreateAssetMenu(
    fileName = "TrashItemData",
    menuName = "ScriptableObjects/TrashTypes/TrashItemData")
]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
}