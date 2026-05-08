using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
}
