using UnityEngine;

[CreateAssetMenu(fileName = "TrashItemData", menuName = "ScriptableObjects/TrashItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
}